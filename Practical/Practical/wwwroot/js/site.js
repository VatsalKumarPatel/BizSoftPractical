// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SaveData(url,formId) {
    var form = $('#' + formId);
    var formData = new FormData(form[0]);
    AjaxMethod(url, "POST", function (data) { SucessSave(data) }, formData);
}

function SucessSave(data) {
    debugger;
    if (!data.success) {
        var validationSummary = $("#ValidationSummary");
        validationSummary.html(data.message);
        validationSummary.show();
    }
    else {
        $("#ValidationSummary1").hide();
        showToster(data.message, data.success);
        window.location.reload();
    }
}

function AjaxMethod(url, type, successfunction, data) {
    $('.btn-primary').prop('disabled', true);
    $('#spinner').show();
    $.ajax({
        url: url,
        type: type,
        contentType: false,
        data: data,
        async: false,
        success: successfunction,
        complete: function () {
            $('#spinner').hide();
            $('.btn-primary').prop('disabled', false);
        },
        cache: false,
        processData: false
    });
}

function showToster(message, IsSuccess) {
    var shortCutFunction = IsSuccess ? "success" : "error";
    var msg = message;
    var title = '';

    toastr.options = {
        closeButton: true,
        debug: false,
        newestOnTop: true,
        progressBar: true,
        positionClass: "toast-top-right",
        preventDuplicates: false,
        onclick: null,
        showDuration: 300,
        hideDuration: 1000,
        timeOut: 5000,
        extendedTimeOut: 1000,
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };


    $('#toastrOptions').text(
        'toastr.options = '
        + JSON.stringify(toastr.options, null, 2)
        + ';'
        + '\n\ntoastr.'
        + shortCutFunction
        + '("'
        + msg
        + (title ? '", "' + title : '')
        + '");'
    );

    var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
    $toastlast = $toast;
    if (typeof $toast === 'undefined') {
        return;
    }
}
