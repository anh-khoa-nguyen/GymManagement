function initializePoseTracker(config) {
    // Lấy các phần tử DOM
    const video = document.getElementById('webcam');
    const canvas = document.getElementById('canvas');
    const ctx = canvas.getContext('2d');
    const repCounter = document.getElementById('rep-counter');
    const startButton = document.getElementById('startButton');
    const statusDiv = document.getElementById('status');

    // Lấy thông tin từ đối tượng config được truyền vào
    const targetReps = config.targetReps;
    const dangKyKeHoachId = config.dangKyKeHoachId;
    const baiTapId = config.baiTapId;
    const completeUrl = config.completeUrl;
    const redirectUrl = config.redirectUrl;
    const loginUrl = config.loginUrl;
    const antiForgeryToken = config.antiForgeryToken;

    // Biến toàn cục
    let detector;
    let repCount = 0;
    let poseState = 'up';
    let isTracking = false;

    async function setupCamera() {
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            throw new Error('Trình duyệt không hỗ trợ API camera.');
        }
        const stream = await navigator.mediaDevices.getUserMedia({ 'audio': false, 'video': { width: 640, height: 480 } });
        video.srcObject = stream;
        return new Promise((resolve) => {
            video.onloadedmetadata = () => {
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                resolve(video);
            };
        });
    }
    async function loadModel() {
        statusDiv.innerText = 'Đang tải mô hình AI...';
        const detectorConfig = { modelType: poseDetection.movenet.modelType.SINGLEPOSE_LIGHTNING };
        detector = await poseDetection.createDetector(poseDetection.SupportedModels.MoveNet, detectorConfig);
        statusDiv.innerText = 'Mô hình đã sẵn sàng!';
    }
    async function detectPose() {
        if (!isTracking) return;
        const poses = await detector.estimatePoses(video);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        if (poses && poses.length > 0) {
            drawKeypoints(poses[0].keypoints);
            countReps(poses[0]);
        }
        requestAnimationFrame(detectPose);
    }
    function drawKeypoints(keypoints) {
        for (let i = 0; i < keypoints.length; i++) {
            const keypoint = keypoints[i];
            if (keypoint.score > 0.5) {
                ctx.beginPath();
                ctx.arc(keypoint.x, keypoint.y, 5, 0, 2 * Math.PI);
                ctx.fillStyle = '#a5ff36'; // Màu xanh neon
                ctx.fill();
            }
        }
    }
    function countReps(pose) {
        const leftHip = pose.keypoints.find(k => k.name === 'left_hip');
        const leftKnee = pose.keypoints.find(k => k.name === 'left_knee');
        if (leftHip && leftKnee && leftHip.score > 0.5 && leftKnee.score > 0.5) {
            if (poseState === 'up' && leftHip.y > leftKnee.y) {
                poseState = 'down';
                statusDiv.innerText = 'Hạ xuống...';
                statusDiv.className = 'status-warning';
            }
            if (poseState === 'down' && leftHip.y < leftKnee.y) {
                repCount++;
                repCounter.innerText = repCount;
                poseState = 'up';
                statusDiv.innerText = `Tốt! Lần thứ ${repCount}`;
                statusDiv.className = '';

                if (repCount >= targetReps) {
                    isTracking = false;
                    video.srcObject.getTracks().forEach(track => track.stop());
                    statusDiv.className = 'status-success';
                    statusDiv.innerText = 'Hoàn thành! Đang lưu kết quả...';
                    sendCompletionToServer();
                }
            }
        }
    }

    function sendCompletionToServer() {
        $.ajax({
            url: completeUrl, 
            method: 'POST',
            data: {
                __RequestVerificationToken: antiForgeryToken,
                dangKyKeHoachId: dangKyKeHoachId,
                baiTapId: baiTapId
            },
            success: function (result) {
                alert(result.message);
                window.location.href = redirectUrl; 
            },
            error: function (jqXHR) {
                console.error("AJAX Error:", jqXHR.responseText);
                if (jqXHR.responseText && jqXHR.responseText.toLowerCase().includes("<title>đăng nhập")) {
                    alert('Phiên đăng nhập của bạn đã hết hạn. Vui lòng đăng nhập lại.');
                    window.location.href = loginUrl;
                } else {
                    alert('Có lỗi xảy ra khi lưu kết quả. Vui lòng thử lại.');
                }
            }
        });
    }

    startButton.onclick = async () => {
        try {
            startButton.disabled = true;
            startButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang xử lý...';
            await setupCamera();
            await loadModel();
            isTracking = true;
            detectPose();
            startButton.style.display = 'none';
        } catch (error) {
            statusDiv.className = 'status-error';
            statusDiv.innerText = 'Lỗi: ' + error.message;
            startButton.disabled = false;
            startButton.innerHTML = '<i class="fas fa-camera"></i> Bắt đầu';
        }
    };
}