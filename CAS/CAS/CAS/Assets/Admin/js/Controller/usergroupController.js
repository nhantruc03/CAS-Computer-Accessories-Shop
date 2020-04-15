var usrgroup = {
    init: function () {
        usrgroup.registerEvents();
    },
    registerEvents: function () {
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                data: { id: btn.data('id') },
                url: "/Admin/UserGroup/Delete",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        var $tr = btn.closest('tr');
                        $tr.remove();
                    }
                    else {
                        alert("Có người dùng hoặc phân quyền đang sử dụng nhóm người dùng này");
                    }
                }
            })
        });
    }
}
usrgroup.init();