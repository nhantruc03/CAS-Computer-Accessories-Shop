var discountcode = {
    init: function () {
        discountcode.registerEvents();
    },
    registerEvents: function () {
        $('.btn-delete-dcc').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                data: { id: id },
                url: "/Admin/DiscountCode/Delete",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        var $tr = btn.closest('tr');
                        $tr.remove();
                    }
                    else {
                        alert("Có đơn hàng đang sử dụng mã giảm giá này");
                    }
                }
            })
        });
    }
}
discountcode.init();