function OpenAvatarInput() {
    $("#UserAvatar").click();
}

function UploadUserAvatar(url) {

    var avatarInput = document.getElementById("UserAvatar");

    if (avatarInput.files.length)
    {
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


function UploadUserAvatarAdmin(url, userId) {

    var avatarInput = document.getElementById("UserAvatar");

    if (avatarInput.files.length) {
        var file = avatarInput.files[0];

        var formData = new FormData();

        formData.append("userAvatar", file);
        formData.append("userId", userId);

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

$(function () {

    if ($("#CountryId").val() === '') {
        $("#CityId").prop("disabled", true);
    }

});


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


var editorsArray = [];
var editors = document.querySelectorAll(".editor");
if (editors.length)
{
    $.getScript("/common/ckeditor/build/ckeditor.js",
        function (data, textStatus, jqxhr)
        {
            for (editor of editors)
            {
                ClassicEditor
                    .create(editor,
                        {
                            licenseKey: '',
                            simpleUpload:
                            {
                                uploadUrl: '/Home/UploadEditorImage'
                            }
                        })
                    .then(editor =>
                    {
                        window.editor = editor;
                        editorsArray.push(editor);
                    })
                    .catch(error =>
                    {
                        console.log(error);
                    });
            }
        });
}

function AnswerQuestionFormDone(response) {
    EndLoading('#submit-comment');

    if (response.status === "Success") {
        swal("اعلان", "پاسخ شما با موفقیت ثبت شد .", "success");


        $("#AnswersBox").load(location.href + " #AnswersBox");

        $('html, body').animate({
            scrollTop: $("#AnswersBox").offset().top
        }, 1000);
    }
    else if (response.status === "EmptyAnswer") {
        swal("هشدار", "متن پاسخ شما نمی تواند خالی  باشد .", "warning");
    }
    else if (response.status === "Error") {
        swal("خطا", "خطایی رخ داده است لطفا مجدد تلاش کنید .", "error");
    }

    for (var editor of editorsArray) {
        editor.setData('');
    }
}

function SubmitQuestionForm() {
    $("#filter_form").submit();
}

function SubmitTagForm() {
    $("#filter_form").submit();
}

function SubmitFilterFormPagination(pageId) {
    $("#CurrentPage").val(pageId);
    $("#filter_form").submit();
}

function SubmitFilterFormAjaxPagination(pageId) {
    $("#CurrentPage").val(pageId);
    $("#filter_ajax_form").submit();
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


function selectTrueAnswer(answerId) {

    $.ajax({
        url: "/SelectTrueAnswer",
        type: "post",
        data: {
            answerId: answerId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();
            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#AnswersBox").load(location.href + " #AnswersBox");
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "NotAccess") {
                swal({
                    title: "خطا",
                    text: "شما به این عملیات دسترسی ندارید .",
                    icon: "error",
                    button: "بستن"
                });
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


function ScoreUpForQuestion(questionId) {
    $.ajax({
        url: "/ScoreUpForQuestion",
        type: "post",
        data: {
            questionId: questionId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#QuestionDetailMainBox").load(location.href + " #QuestionDetailMainBox");
            }
            else if (response.status === "NotEnoughScoreForDown") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
            }
            else if (response.status === "Error") {
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد .",
                    icon: "error",
                    button: "بستن"
                });
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "UserCreateScoreBefore") {
                swal({
                    title: "اعلان",
                    text: "شما قبلا برای این پاسخ امتیاز داده اید .",
                    icon: "info",
                    button: "بستن"
                });
            }
            else if (response.status === "NotEnoughScoreForUp") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
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

function ScoreDownForQuestion(questionId) {
    $.ajax({
        url: "/ScoreDownForQuestion",
        type: "post",
        data: {
            questionId: questionId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#QuestionDetailMainBox").load(location.href + " #QuestionDetailMainBox");
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "NotEnoughScoreForDown") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
            }
            else if (response.status === "Error") {
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد .",
                    icon: "error",
                    button: "بستن"
                });
            }
            else if (response.status === "UserCreateScoreBefore") {
                swal({
                    title: "اعلان",
                    text: "شما قبلا برای این پاسخ امتیاز داده اید .",
                    icon: "info",
                    button: "بستن"
                });
            }
            else if (response.status === "NotEnoughScoreForUp") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
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

function ScoreUpForAnswer(answerId) {
    $.ajax({
        url: "/ScoreUpForAnswer",
        type: "post",
        data: {
            answerId: answerId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#AnswersBox").load(location.href + " #AnswersBox");
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "NotEnoughScoreForDown") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
            }
            else if (response.status === "Error") {
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد .",
                    icon: "error",
                    button: "بستن"
                });
            }
            else if (response.status === "UserCreateScoreBefore") {
                swal({
                    title: "اعلان",
                    text: "شما قبلا برای این پاسخ امتیاز داده اید .",
                    icon: "info",
                    button: "بستن"
                });
            }
            else if (response.status === "NotEnoughScoreForUp") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
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

function ScoreDownForAnswer(answerId) {
    $.ajax({
        url: "/ScoreDownForAnswer",
        type: "post",
        data: {
            answerId: answerId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#AnswersBox").load(location.href + " #AnswersBox");
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "NotEnoughScoreForDown") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
            }
            else if (response.status === "Error") {
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد .",
                    icon: "error",
                    button: "بستن"
                });
            }
            else if (response.status === "UserCreateScoreBefore") {
                swal({
                    title: "اعلان",
                    text: "شما قبلا برای این پاسخ امتیاز داده اید .",
                    icon: "info",
                    button: "بستن"
                });
            }
            else if (response.status === "NotEnoughScoreForUp") {
                swal({
                    title: "اعلان",
                    text: "شما امیتاز کافی برای ثبت امتیاز منفی را ندارید .",
                    icon: "warning",
                    button: "باشه"
                });
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

function AddQuestionToBookmark(questionId) {
    $.ajax({
        url: "/AddQuestionToBookmark",
        type: "post",
        data: {
            questionId: questionId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            if (response.status === "Success") {
                swal({
                    title: "اعلان",
                    text: "عملیات با موفقیت انحام شد .",
                    icon: "success",
                    button: "بستن"
                });

                $("#QuestionDetailMainBox").load(location.href + " #QuestionDetailMainBox");
            }
            else if (response.status === "NotAuthorize") {
                swal({
                    title: "اعلان",
                    text: "برای انتخاب پاسخ درست ابتدا وارد سایت شوید .",
                    icon: "info",
                    button: "باشه"
                });
            }
            else if (response.status === "Error") {
                swal({
                    title: "خطا",
                    text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                    icon: "error",
                    button: "بستن"
                });
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

function LoadUrl(url)
{
    location.href = url;
}
