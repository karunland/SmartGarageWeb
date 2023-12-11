
function OnAjaxSuccess(result) {
    debugger;
    Swal.fire(
        result.isSuccess === true ? 'Başarılı' : 'Hata',
        result.message,
        result.isSuccess === true ? 'success' : 'error',
        2000
    );
}

function OnAjaxError(result) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        timer: 2000,
        html: result.responseJSON.message.join("<br>")
    })
    $(".btn[type='submit']").removeAttr("disabled", "disabled");
}