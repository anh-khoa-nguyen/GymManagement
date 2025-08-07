@section Scripts {
    <script>
        $(document).ready(function () {
            $('.js-momo-payment').on('click', function () {
                var button = $(this);
                var hoaDonId = button.data('id');

                button.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i>');

                $.ajax({
                    url: '@Url.Action("CreatePaymentRequest", "Momo")',
                    type: 'POST',
                    data: { hoaDonId: hoaDonId },
                    success: function (response) {
                        if (response.success) {
                            window.open(response.payUrl, '_blank');
                        } else {
                            alert(response.message);
                        }
                        button.prop('disabled', false).html('<i class="fas fa-qrcode"></i>');
                    },
                    error: function () {
                        alert('Có lỗi xảy ra. Vui lòng thử lại.');
                        button.prop('disabled', false).html('<i class="fas fa-qrcode"></i>');
                    }
                });
            });
        });
    </script>
}