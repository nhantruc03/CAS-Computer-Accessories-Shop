var productcategory = {
    init: function () {
        productcategory.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/ProductCategory/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Kích hoạt');
                    }
                    else {
                        btn.text('Khóa');
                    }
                }
            });
        });

        $('.btn-delete-pdc').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                data: { id: id },
                url: "/Admin/ProductCategory/Delete",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        var $tr = btn.closest('tr');
                        $tr.remove();
                    }
                    else {
                        alert("Có sản phẩm đang sử dụng danh mục này");
                    }
                }
            })
        });
    }
}
productcategory.init();