
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

$('#open-door').click(function () {
    var selectedSpeed = $('input[name="speed"]:checked').val();
    $.post('/Home/OpenDoor', { speed: selectedSpeed })
        .done(function (response) {
            if (response.isSuccess === true) {
                console.log('İşlem başarıyla tamamlandı.');
                Swal.fire({
                    icon: "success",
                    title: "Başarılı!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {
                console.log('İşlem tamamlanamadı.');
                Swal.fire({
                    icon: "error",
                    title: "Hata!",
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        })
        .fail(function (response) {
            Swal.fire({
                icon: "error",
                title: "Hata!",
                showConfirmButton: false,
                timer: 2000
            });
        });
});

 $('#close-door').click(function () {
    $.post('/Home/CloseDoor')
        .done(function (response) {
            if (response.isSuccess === true) {
                console.log('İşlem başarıyla tamamlandı.');
                Swal.fire({
                    icon: "success",
                    title: "Başarılı!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {
                console.log('İşlem tamamlanamadı.');
                Swal.fire({
                    icon: "error",
                    title: "Hata!",
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        })
        .fail(function (response) {
            Swal.fire({
                icon: "error",
                title: "Hata!",
                showConfirmButton: false,
                timer: 2000
            });
        });
});

$('#send-gas').click(function () {
    var selectedGasSpeed = $('input[name="gasSpeed"]:checked').val();

    $.post('/Home/SendGas', { gasSpeed: selectedGasSpeed })
        .done(function (response) {
            if (response.isSuccess === true) {
                console.log('İşlem başarıyla tamamlandı.');
                Swal.fire({
                    icon: "success",
                    title: "Başarılı!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {
                console.log('İşlem tamamlanamadı.');
                Swal.fire({
                    icon: "error",
                    title: "Error!",
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        })
        .fail(function (response) {
            Swal.fire({
                icon: "error",
                title: "Error!",
                showConfirmButton: false,
                timer: 2000
            });
        });
});

function getProximitySensorData() {
    $.getJSON('http://172.20.10.12:5000/flask/sensor/hcsr04')
        .done(function (data) {
            console.log(data);
            $('#proximity-sensor').text('Sensör Değeri: ' + data + " cm");
            if (data <= 5) {
                $('#proximity-box').addClass('highlight');
            } else {
                $('#proximity-box').removeClass('highlight');
            }
        })
        .fail(function () {
            console.error('Yakınlık sensörü verileri alınamadı.');
        });
}

function getGasSensorData() {
    $.getJSON('http://172.20.10.12:5000/flask/sensor/gas')
        .done(function (data) {
            console.log(data);
            if (data >= 1) {
                $('#gas-box').addClass('highlight');
            } else {
                $('#gas-box').removeClass('highlight');
            }
        })
        .fail(function () {
            console.error('Gaz sensörü verileri alınamadı.');
        });
}

function getGasSensorData() {
    $.getJSON('http://172.20.10.12:5000/flask/sensor/gas/0')
        .done(function (data) {
            console.log(data);
            $('#gas-sensor').text('Sensör Değeri: ' + data.sensor_value);
        })
        .fail(function () {
            console.error('Gaz sensörü I2C verileri alınamadı.');
        });
}

function checkRecordingStatus() {
    $.ajax({
        url: '/stream/checkRecordingStatus',
        type: 'GET',
        success: function (response) {
            if (response.isSuccess === true) {
                $('#recordingIndicator').addClass('recording');
            } else {
                $('#recordingIndicator').removeClass('recording');
            }
        },
        error: function () {
            console.error('Durum alınamadı');
        }
    });
}

$("#startRecordingBtn").click(function () {
    $.ajax({
        url: '/stream/startRecording',
        type: 'POST',
        success: function (response) {
            debugger;
            if (response.isSuccess === true) {
                Swal.fire({
                    icon: "success",
                    title: "Başarılı!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
                $("#startRecordingBtn").prop("disabled", true);
                $("#stopRecordingBtn").prop("disabled", false);
            } else {
                Swal.fire('Hata', 'Kayıt başlatılamadı!', 'error');
            }
        },
        error: function () {
            Swal.fire('Hata', 'İstek gönderilirken bir hata oluştu!', 'error');
        }
    });
});

$("#stopRecordingBtn").click(function () {
    $.ajax({
        url: '/stream/stopRecording',
        type: 'POST',
        success: function (response) {
            debugger;
            if (response.isSuccess === true) {
                Swal.fire({
                    icon: "success",
                    title: "Başarılı!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
                $("#startRecordingBtn").prop("disabled", false);
                $("#stopRecordingBtn").prop("disabled", true);
            } else {
                Swal.fire('Hata', 'Kayıt durdurulamadı!', 'error');
            }
        },
        error: function () {
            Swal.fire('Hata', 'İstek gönderilirken bir hata oluştu!', 'error');
        }
    });
});

$("#photo").click(function () {
    $.ajax({
        url: '/Stream/CapturePhoto',
        type: 'POST',
        success: function (response) {
            debugger;
            if (response.isSuccess === true) {
                $("#FileNamea").val(response.data.fileName);
                $("#latestImg").attr("src", "/File/GetFile?filePath=" + response.data.fileName + ".png");
                $("#exampleModal").modal('show');
                refreshComponent();
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Hata!",
                    text: response.message,
                    showConfirmButton: false,
                    timer: 2000
                })
            }
        },
        error: function () {
            Swal.fire('Hata', 'İstek gönderilirken bir hata oluştu!', 'error');
        }
    });
});

