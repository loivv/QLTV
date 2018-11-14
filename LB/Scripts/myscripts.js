Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
};
function showModel(id) {

    $('#' + id).modal('show');
}
function showModelFix(id) {
    $('#' + id).modal({
        backdrop: 'static',
        keyboard: false
    });
    $('#' + id).modal('show');
}
function hideModel(id) {
    $('#' + id).modal('hide');
};


function showLoader(isShow) {

    if (isShow) {
        $("#loader").css("display", "block");
    } else {
        $("#loader").css("display", "none");
    }
};

function showNotify( mesenger) {

    $.notify(mesenger, {
        type: 'success'
    });
    
};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});