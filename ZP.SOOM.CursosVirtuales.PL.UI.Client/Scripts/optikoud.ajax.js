function ajaxPost(formData, success, error) {
    var ajax = {
        type: 'POST',
        url: $(formData).attr('action'),
        data: new FormData(formData),
        success: function (data) {
            stopLoading();

            if (data.Code == 0 && success !== null)
                success(data);
            else if (data.Code !== 0 && error !== null)
                error(data);
        }
    }
    if ($(formData).attr('enctype') == "multipart/form-data") {
        ajax["contentType"] = false;
        ajax["processData"] = false;
    }
    startLoading();
    $.ajax(ajax);
    return false;
}