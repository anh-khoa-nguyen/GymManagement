$(function () {
    // --- KHỞI TẠO CÁC BIẾN CẦN THIẾT ---
    // Lấy phần tử Offcanvas từ HTML
    var offcanvasElement = document.getElementById('crudOffcanvas');
    // Tạo một đối tượng Offcanvas của Bootstrap để điều khiển (show/hide)
    var bsOffcanvas = new bootstrap.Offcanvas(offcanvasElement);
    // Vùng chứa nội dung của Offcanvas
    var offcanvasBody = $('#offcanvas-body-content');
    // Tiêu đề của Offcanvas
    var offcanvasTitle = $('#offcanvasLabel');


    // --- GẮN SỰ KIỆN CLICK ---
    // Tìm tất cả các phần tử có class 'js-open-offcanvas' trên toàn trang
    // Sử dụng $(document).on() để nó hoạt động với cả các phần tử được thêm vào sau này
    $(document).on('click', '.js-open-offcanvas', function (e) {
        e.preventDefault(); // Ngăn hành vi mặc định của thẻ <a> (chuyển trang)

        // Lấy URL của Partial View từ thuộc tính 'data-url' của nút được click
        var url = $(this).data('url');

        // --- HIỂN THỊ TRẠNG THÁI ĐANG TẢI ---
        // Đặt tiêu đề thành "Đang tải..."
        offcanvasTitle.text('Đang tải...');
        // Hiển thị icon xoay xoay trong lúc chờ dữ liệu
        offcanvasBody.html('<div class="d-flex justify-content-center align-items-center" style="height:100%;"><div class="spinner-border text-light" role="status"><span class="visually-hidden">Loading...</span></div></div>');

        // Mở Offcanvas
        bsOffcanvas.show();

        // --- GỌI AJAX ĐỂ LẤY DỮ LIỆU ---
        $.get(url, function (data) {
            // Khi nhận được dữ liệu thành công, đưa nó vào body của Offcanvas
            offcanvasBody.html(data);

            // Tìm thẻ h2, h3, h4 đầu tiên trong dữ liệu nhận về để làm tiêu đề
            var title = offcanvasBody.find('h2, h3, h4').first().text();
            offcanvasTitle.text(title || 'Thông tin'); // Nếu không có tiêu đề, dùng chữ "Thông tin"

            // Xóa thẻ tiêu đề trong body đi để tránh bị lặp lại
            offcanvasBody.find('h2, h3, h4').first().remove();
        }).fail(function () {
            // Nếu có lỗi xảy ra trong quá trình gọi AJAX
            offcanvasTitle.text('Lỗi');
            offcanvasBody.html('<p class="text-danger">Không thể tải nội dung. Vui lòng thử lại.</p>');
        });
    });
    // Xủ lý sự kiện click cho nút "Submit" trong Offcanvas
    $(document).on('submit', '#offcanvasForm', function (e) {
        e.preventDefault();
        const form = $(this);

        // Kiểm tra riêng cho form đánh giá
        if (form.attr('action').toLowerCase().includes('guidanhgia')) {
            if (form.find('input[name=soSao]:checked').length === 0) {
                alert('Vui lòng chọn số sao để đánh giá.');
                return;
            }
        }

        $.ajax({
            url: form.attr('action'),
            method: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    bootstrap.Offcanvas.getInstance(document.getElementById('crudOffcanvas')).hide();
                    alert(response.message);
                    // Có thể cần tải lại lịch nếu là hành động đặt lịch
                    if (form.attr('action').toLowerCase().includes('taolichtap')) {
                        calendar.refetchEvents();
                    }
                } else {
                    // Nếu có lỗi validation, server sẽ trả về lại PartialView với lỗi
                    $('#offcanvas-body-content').html(response);
                }
            }
        });
    });
});