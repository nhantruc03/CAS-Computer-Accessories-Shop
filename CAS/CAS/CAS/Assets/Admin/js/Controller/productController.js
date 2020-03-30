var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeStatus",
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
    },
    registerEvents: function () {
        $('.btn-active2').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeVATStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã bao gồm');
                    }
                    else {
                        btn.text('Chưa bao gồm');
                    }
                }
            });
        });
    }
}
product.init();

//var product2 = {
//    init: function () {
//        product2.registerEvents();
//    },
//    registerEvents: function () {
//        $('.btn-active2').off('click').on('click', function (e) {
//            e.preventDefault();
//            var btn = $(this);
//            var id = btn.data('id');
//            $.ajax({
//                url: "/Admin/Product/ChangeVATStatus",
//                data: { id: id },
//                dataType: "json",
//                type: "POST",
//                success: function (response) {
//                    console.log(response);
//                    if (response.status == true) {
//                        btn.text('Đã bao gồm');
//                    }
//                    else {
//                        btn.text('Chưa bao gồm');
//                    }
//                }
//            });
//        });
//    }
//}
//product2.init();