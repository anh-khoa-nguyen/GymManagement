$(document).ready(function () {
    const checkAll = $('#checkAll');
    const rowCheckboxes = $('.row-checkbox');
    const deleteBtn = $('#deleteSelectedBtn');

    // Nếu không có các phần tử này trên trang, không làm gì cả
    if (!checkAll.length || !rowCheckboxes.length || !deleteBtn.length) {
        return;
    }

    // Hàm để kiểm tra và hiển thị/ẩn nút xóa
    function toggleDeleteButton() {
        if ($('.row-checkbox:checked').length > 0) {
            deleteBtn.show();
        } else {
            deleteBtn.hide();
        }
    }

    // Sự kiện khi nhấn vào checkbox "Chọn tất cả"
    checkAll.on('change', function () {
        rowCheckboxes.prop('checked', $(this).prop('checked'));
        toggleDeleteButton();
    });

    // Sự kiện khi nhấn vào một checkbox trên một hàng
    rowCheckboxes.on('change', function () {
        if (rowCheckboxes.length === $('.row-checkbox:checked').length) {
            checkAll.prop('checked', true);
        } else {
            checkAll.prop('checked', false);
        }
        toggleDeleteButton();
    });
});