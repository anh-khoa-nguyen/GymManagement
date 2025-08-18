$(function () {
    // --- 1. LẤY THÔNG TIN NGƯỜI DÙNG TỪ THẺ META ---
    const currentUserId = $('meta[name="user-id"]').attr('content');
    const currentUserName = $('meta[name="user-name"]').attr('content');
    const currentUserRole = $('meta[name="user-role"]').attr('content');

    if (!currentUserId) return;

    $('#open-chat-button').show();

    // --- 2. KHAI BÁO CÁC BIẾN VÀ ELEMENT JQUERY ---
    const $chatContainer = $('#chat-container');
    const $chatListView = $('#chat-list-view');
    const $chatMessageView = $('#chat-message-view');
    const $chatListUsers = $('#chat-list-users');
    const $chatWithNametag = $('#chat-with-name');
    const $chatMessages = $('#chat-messages');
    const $messageInput = $('#chat-message-input');

    let currentChatRoomId = null;
    let messagesListener = null; // Biến để lưu trữ listener hiện tại

    // --- 3. CÁC HÀM XỬ LÝ SỰ KIỆN ---

    $('#open-chat-button').on('click', function () {
        // Nếu đang ở màn hình chat chi tiết, không làm gì cả, chỉ toggle
        if ($chatMessageView.is(':visible')) {
            $chatContainer.toggle();
            return;
        }
        // Ngược lại, tải danh sách và hiển thị
        $chatListView.show();
        $chatMessageView.hide();
        loadChatList();
        $chatContainer.toggle();
    });

    $('#close-chat-widget, #close-chat-widget-2').on('click', function () {
        $chatContainer.hide();
    });

    $('#back-to-chat-list').on('click', function () {
        $chatMessageView.hide();
        $chatListView.show();
        // QUAN TRỌNG: Ngắt kết nối listener cũ để không nhận tin nhắn từ phòng cũ nữa
        if (messagesListener && currentChatRoomId) {
            database.ref('chats/' + currentChatRoomId).off('child_added', messagesListener);
        }
        currentChatRoomId = null;
    });

    $chatListUsers.on('click', '.list-group-item', function (e) {
        e.preventDefault();
        const recipientId = $(this).data('userid');
        const recipientName = $(this).data('name');
        startChat(recipientId, recipientName);
    });

    $('#send-chat-message').on('click', sendMessage);
    $messageInput.on('keypress', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) { // Gửi khi nhấn Enter, xuống dòng khi Shift+Enter
            e.preventDefault();
            sendMessage();
        }
    });

    // --- 4. CÁC HÀM LOGIC CHÍNH ---

    // Tải danh sách người có thể chat
    function loadChatList() {
        let apiUrl = '';
        if (currentUserRole === 'HoiVien') {
            apiUrl = '/HoiVien/GetPTListForChat';
        } else if (currentUserRole === 'PT') {
            apiUrl = '/PT/GetMemberListForChat';
        } else {
            return;
        }

        $.get(apiUrl, function (users) {
            $chatListUsers.empty();
            if (users && users.length > 0) {
                users.forEach(function (user) {
                    const userHtml = `
                     <a href="#" class="list-group-item list-group-item-action" data-userid="${user.Id}" data-name="${user.Name}">
                         ${user.Name}
                     </a>`;
                    $chatListUsers.append(userHtml);
                });
            } else {
                $chatListUsers.html('<p class="p-3 text-muted">Không có ai để chat.</p>');
            }
        });
    }

    function startChat(recipientId, recipientName) {
        console.log("--- Bắt đầu hàm startChat ---");
        console.log("Chat với:", recipientName, "(ID:", recipientId, ")");

        $chatListView.hide();
        $chatMessageView.show();
        $chatWithNametag.text(recipientName);
        $chatMessages.empty();

        // Kiểm tra lại các biến global
        if (!currentUserId || !database) {
            console.error("Lỗi nghiêm trọng: currentUserId hoặc database chưa được định nghĩa!");
            return;
        }

        currentChatRoomId = [currentUserId, recipientId].sort().join('_');
        console.log("ID phòng chat:", currentChatRoomId);

        const messagesRef = database.ref('chats/' + currentChatRoomId);
        console.log("Đã tạo tham chiếu đến:", messagesRef.toString());

        // Gắn listener mới
        console.log("Đang gắn listener 'child_added'...");
        messagesListener = messagesRef.limitToLast(50).on('child_added', function (snapshot) {
            // DÒNG NÀY PHẢI ĐƯỢC IN RA KHI CÓ TIN NHẮN MỚI
            console.log("ĐÃ NHẬN ĐƯỢC TIN NHẮN MỚI:", snapshot.val());

            const message = snapshot.val();
            if (!message) return;

            const messageClass = message.senderId === currentUserId ? 'sent' : 'received';
            const escapedText = $('<div>').text(message.text).html();
            const messageHtml = `<div class="message ${messageClass}">${escapedText}</div>`;

            $chatMessages.append(messageHtml);
            $chatMessages.scrollTop($chatMessages[0].scrollHeight);
        }, function (error) {
            // In ra lỗi nếu không thể lắng nghe
            console.error("Lỗi khi lắng nghe tin nhắn:", error);
        });

        console.log("--- Kết thúc hàm startChat, listener đã được gắn. ---");
    }

    function sendMessage() {
        const messageText = $messageInput.val().trim();
        // Kiểm tra lại currentChatRoomId để chắc chắn đang ở trong một phòng chat
        if (!messageText || !currentChatRoomId) return;

        const messagesRef = database.ref('chats/' + currentChatRoomId);

        messagesRef.push({
            senderId: currentUserId,
            senderName: currentUserName, // Gửi cả tên để dễ hiển thị
            text: messageText,
            timestamp: firebase.database.ServerValue.TIMESTAMP
        });

        $messageInput.val('');
        $messageInput.focus();
    }
});