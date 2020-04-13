var content = {
    init: function () {
        content.registerEvents();
    },
    registerEvents: function () {
        $('#btnSelectImage').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                var txtimage = document.getElementById('txtImage');
                if (txtimage.value == "") {
                    document.getElementById('txtImage').value = url;
                    $('#imageone').append('<div class="px-2"><img src="' + url + '" width="100"/><a href="#" class="btn-delImages2"><i class="fa fa-times"></i></a><input type="hidden" class="hidImage" value=' + url + '></div>');
                    $('.btn-delImages2').off('click').on('click', function (e) {
                        e.preventDefault();
                        $(this).parent().remove();
                        document.getElementById('txtImage').value = "";
                    });
                }
                else {
                    alert("Chỉ được chọn 1 ảnh");
                }
            };
            finder.popup();
        })

        var editor = CKEDITOR.replace('txtContent', {
            customConfig: '/Assets/Admin/js/Plugins/ckeditor/config.js'
        })

        $('#btnSelectListImage').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#imageList').append('<div class="px-2"><img src="' + url + '" width="100"/><a href="#" class="btn-delImages"><i class="fa fa-times"></i></a><input type="hidden" class="hidImage" value=' + url + '></div>');
                $('.btn-delImages').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
            };
            finder.popup();
        })
    }
}
content.init();