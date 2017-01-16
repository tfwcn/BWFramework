function AjaxLoad(url, formID, fuBeforeSend, fuSuccess, fuError, fuComplete, fData) {
    if (isAjax) {
        alert('請耐心等待上一次的操作完成！');
        return false;
    }

    //拿全部數據后
    var formData = {};
    if (fData) {
        formData = fData;
    } else {
        if (formID)
            formData = $(formID).serialize(); //獲取表單數據
    }

    var isAjax = true;
    window.setTimeout(function () {
        $.ajax(new function () {
            this.dataType = "html";
            this.type = "post";
            this.url = url;
            this.cache = false;
            this.data = formData;
            this.beforeSend = function (req) {
                //提交前
                if ($.isFunction(fuBeforeSend)) {
                    fuBeforeSend(req);
                }
            };
            this.success = function (data) {
                //返回結果
                if ($.isFunction(fuSuccess)) {
                    fuSuccess(data);
                }
            };
            this.error = function (req, msg) {
                //提交錯誤
                isAjax = false;
                if ($.isFunction(fuError)) {
                    //req.statusText
                    fuError(req, msg);
                }
            };
            this.complete = function (req, msg) {
                //提交成功
                isAjax = false;
                if ($.isFunction(fuComplete)) {
                    fuComplete(req, msg);
                }
            };
        });
    }, 200);
}
//提交表單(含文件)
function SubmitFormAndUploadFile(url, formID, fuSuccess) {
    var data = {};
    $.each($(formID).serializeArray(), function (index) {
        if (data[this['name']]) {
            data[this['name']] = data[this['name']] + "," + this['value'];
        } else {
            data[this['name']] = this['value'];
        }
    });
    var fileID = $(formID).find("input[type='file']");
    AjaxUploadFile(url, data, fileID, fuSuccess);
}
//提交表單(含文件)
function AjaxUploadFile(url, data, fileID, fuSuccess) {
    if (isAjax) {
        alert('請耐心等待上一次的操作完成！');
        return false;
    }
    isAjax = true;
    var iframe = $('<iframe style="display: none;"></iframe>');
    $("body").append(iframe);
    iframe.attr('src', 'about:blank');
    iframe.one('load', function () {
        var from = $('<form method="post" enctype="multipart/form-data" action="' + url + '"></form>');
        jQuery.each(data, function (i, val) {
            var input = $('<input type="text" />');
            input.attr("name", i);
            input.val(val);
            from.append(input);
        });
        jQuery.each(fileID, function (i, val) {
            var old = $(val);
            var input = old.clone(false);
            old.replaceWith(input);
            from.append(old);
        });
        iframe.contents().find("body").append(from);
        iframe.one('load', function () {
            isAjax = false;
            fuSuccess(iframe.contents().find("body").html());
            iframe.remove();
        });
        from.submit();
    });
}
//提交文件(選擇文件後直接提交)
function AjaxUploadFileOnly(url, data, fuSuccess) {
    if (isAjax) {
        alert('請耐心等待上一次的操作完成！');
        return false;
    }
    var iframe = $('<iframe class="uploadFileOnlyFrame" style="display: none;"></iframe>');
    $("body").append(iframe);
    iframe.attr('src', 'about:blank');
    iframe.one('load', function () {
        var from = $('<form method="post" enctype="multipart/form-data" action="' + url + '"></form>');
        jQuery.each(data, function (i, val) {
            var input = $('<input type="text" />');
            input.attr("name", i);
            input.val(val);
            from.append(input);
        });
        var input = $('<input type="file" />');
        input.attr("name", "file");
        from.append(input);
        iframe.contents().find("body").append(from);
        input.change(function () {
            if ($(this).val() == '') {
                isAjax = false;
                iframe.remove();
                return false;
            }
            $(window.parent.document).find(".uploadFileOnlyFrame").each(function () {
                if ($(this).contents().find("input[name='file']").val() == '') {
                    $(this).remove();
                }
            });
            isAjax = true;
            LoadingShow('loadong...');
            iframe.one('load', function () {
                isAjax = false;
                fuSuccess(iframe.contents().find("body").html());
                iframe.remove();
                LoadingHide('loadong...');
            });
            from.submit();
        });
        input.click();
    });
}