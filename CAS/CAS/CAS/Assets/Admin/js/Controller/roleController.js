var role = {
    init: function () {
        role.registerEvents();
    },
    registerEvents: function () {
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                data: { id: btn.data('id') },
                url: "/Admin/Role/Delete",
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        var $tr = btn.closest('tr');
                        $tr.remove();
                    }
                    else {
                        alert("Có phân quyền đang sử dụng quyền này");
                    }
                }
            })
        });
    }
}
role.init();