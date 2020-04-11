var minicart = {
    init: function () {
        minicart.regEvents();
    },
    regEvents: function () {
        // from mini cart
        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                cache: false,
                url: '/Cart/AddToCart',
                data: {
                    productID: id,
                    quantity: 1,
                },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        var item = res.entity;
                        $('#itemcount').text("(" + res.count + ")");
                        if (res.alrhave == false) {
                            $('.iteminminicart').append('<tr>' +
                                '<td style = "max-width:10px" >' + item.Descriptions + '</td>' +
                                '<td><img src="' + item.Image + '" width="100" /></td>' +
                                //'<td><input type="number" class="txtQuantityFromMiniCart text-wrap" data-id="' + item.ID + '" value="' + res.quantity + '"readonly/></td>' +
                                '<td><a class="txtQuantityFromMiniCart" data-id="' + item.ID + '">' + res.quantity + '</a></td>' +
                                '<td>' + res.Price + '</td>' +
                                '<td>' + res.lastPrice + '</td>' +
                                '</tr>');
                        }
                        else {
                            alert("Sản phẩm đã có trong giỏ hàng");
                        }
                        $('#mycart').modal('show');
                    }
                }
            });
        });

        $('#btnUpdateFromMiniCart').off('click').on('click', function () {
            var listProduct = $('.txtQuantityFromMiniCart');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });
            $.ajax({
                url: '/Cart/UpdateFromCart',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {

                    }
                }
            })
        });

        $('#btnDeleteAllFromMiniCart').off('click').on('click', function () {

            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        $('#itemcount').text("(0)");
                        $('.iteminminicart').html("");
                    }
                }
            })
        });

        $('#btnPaymentFromMiniCart').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });

        $(document).click(function (e) {

            if ($('.modal.in, .modal.show').length) {
                if (!$(e.target).closest("#mycart").length) {
                    $("body").find("#mycart").modal('hide');
                }
            }
        });
    }
}
minicart.init();