var menutype = {
    init: function () {
        menutype.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/MenuType/ChangeStatus",
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

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                data: { id: btn.data('id') },
                url: "/Admin/MenuType/Delete",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        var $tr = btn.closest('tr');
                        $tr.remove();
                    }
                    else {
                        alert("Có menu đang sử dụng loại menu này");
                    }
                }
            })
        });
    }
}
menutype.init();