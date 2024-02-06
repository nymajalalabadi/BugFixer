function OpenAvatarInput() {
    $("#UserAvatar").click();
}

function UploadUserAvatar(url) {

    var avatarInput = document.getElementById("UserAvatar");

    if (avatarInput.files.length) {

        var file = avatarInput.files[0];

        var formData = new FormData();

        formData.append("userAvatar", file);

        $.ajax({
            url: url,
            type: "post",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                StartLoading('#UserInfoBox');
            },
            success: function (response) {
                EndLoading('#UserInfoBox');
                if (response.status === "Success") {
                    location.reload();
                } else {
                    swal({
                        title: "خطا",
                        text: "فرمت فایل ارسال شده معتبر نمی باشد .",
                        icon: "error",
                        button: "باشه"
                    });
                }
            },
            error: function () {
                EndLoading('#UserInfoBox');
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                    icon: "error",
                    button: "باشه"
                });
            }
        });
    }

}

function StartLoading(selector = 'body') {
    $(selector).waitMe({
        effect: 'bounce',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255, 255, 255, 0.7)',
        color: '#000'
    });
}

function EndLoading(selector = 'body') {
    $(selector).waitMe('hide');
}

$("#CountryId").on("change", function () {
    var countryId = $("#CountryId").val();
    if (countryId !== '' && countryId.length) {
        $.ajax({
            url: $("#CountryId").attr("data-url"),
            type: "get",
            data: {
                countryId: countryId
            },
            beforeSend: function () {
                StartLoading();
            },
            success: function (response) {
                EndLoading();
                $("#CityId option:not(:first)").remove();
                $("#CityId").prop("disabled", false);
                for (var city of response) {
                    $("#CityId").append(`<option value="${city.id}">${city.title}</option>`);
                }
            },
            error: function () {
                EndLoading();
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                    icon: "error",
                    button: "باشه"
                });
            }
        });
    }
    else {
        $("#CityId option:not(:first)").remove();
        $("#CityId").prop("disabled", true);
    }
})


var datepickers = document.querySelectorAll(".datepicker");
if (datepickers.length) {
    for (datepicker of datepickers) {
        var id = $(datepicker).attr("id");
        kamaDatepicker(id, {
            placeholder: 'مثال : 1400/01/01',
            twodigit: true,
            closeAfterSelect: false,
            forceFarsiDigits: true,
            markToday: true,
            markHolidays: true,
            highlightSelectedDay: true,
            sync: true,
            gotoToday: true
        });
    }
}

$(function () {

    if ($("#CountryId").val() === '') {
        $("#CityId").prop("disabled", true);
    }

});

var editors = document.querySelectorAll(".editor");
if (editors.length) {
    $.getScript("/common/ckeditor/build/ckeditor.js",
        function (data, textStatus, jqxhr) {
            for (editor of editors) {
                ClassicEditor
                    .create(editor,
                        {
                            licenseKey: '',
                            simpleUpload: {
                                uploadUrl: '/Home/UploadEditorImage'
                            }
                        })
                    .then(editor => {
                        window.editor = editor;
                    })
                    .catch(error => {
                        console.log(error);
                    });
            }
        });
}


function SubmitQuestionForm() {
    $("#filter_form").submit();
}

function SubmitFilterFormPagination(pageId) {
    $("#CurrentPage").val(pageId);
    $("#filter_form").submit();
}
