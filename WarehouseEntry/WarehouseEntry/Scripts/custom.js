var alertTimerId;

function showSeccess(data, redirect) {
    $(".alert-top-wrapper .alert").removeClass("alert-danger");
    $(".alert-top-wrapper .alert").addClass("alert-success");
    $(".alert-top-wrapper .icon").removeClass("icon-remove");
    $(".alert-top-wrapper .icon").addClass("icon-ok");
    showAlert(data, redirect);
}

function showFailed(data, redirect) {
    $(".alert-top-wrapper .alert").removeClass("alert-success");
    $(".alert-top-wrapper .alert").addClass("alert-danger");
    $(".alert-top-wrapper .icon").removeClass("icon-ok");
    $(".alert-top-wrapper .icon").addClass("icon-remove");
    showAlert(data, redirect);
}

function showAlert(data, redirect) {
    if (alertTimerId != undefined) {
        clearTimeout(alertTimerId);
        alertTimerId = undefined;
    }
    $(".alert-top-wrapper .message-container").text(data.Msg);
    $(".alert-top-wrapper").removeClass("ng-hide");
    if (redirect == undefined || typeof redirect != "string") {
        if (data.ObjectModel != undefined) {
            redirect = data.ObjectModel.backUrl;
        }
    }
    if (redirect) {
        alertTimerId = setTimeout(function () {
            window.location.href = redirect;
        }, 1000);
    } else {
        alertTimerId = setTimeout(hideAlert, 3000);
    }
}

function hideAlert() {
    $(".alert-top-wrapper").addClass("ng-hide");
}

function ajaxPost(requestUrl, requestData, ready, successFunc, errorFunc, isJson) {
    if (ready != undefined && !ready()) {
        return;
    }
    $.ajax({
        type: 'POST',
        url: requestUrl,
        data: requestData,
        contentType: "application/x-www-form-urlencoded",
        success: function (responseData, textStatus, jqXhr) {
            if (isJson != undefined && !isJson) {
                successFunc(responseData);
            } else {
                if (responseData != undefined && typeof (responseData) == "string") {
                    responseData = $.parseJSON(responseData);
                }
                if (responseData == undefined || responseData.Status) {
                    successFunc(responseData);
                } else {
                    errorFunc(responseData);
                }
            }
        },
        error: errorFunc
    });
}