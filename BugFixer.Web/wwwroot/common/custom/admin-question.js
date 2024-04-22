function changeQuestionIsCheckedStatus(id) {
    swal({
        title: "آیا مطمئن هستی ؟",
        text: "در صورت انجام این عملیات قادر به بازگردانی آن نمی باشید.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/admin/Question/ChangeIsCheckedQuestion",
                    type: "post",
                    data: {
                        id: id
                    },
                    beforeSend: function () {
                        StartLoading();
                    },
                    success: function (response) {
                        EndLoading();
                        if (response.status === "error") {
                            swal({
                                title: "خطا",
                                text: response.message,
                                icon: "error",
                                button: "باشه"
                            });
                        }
                        else {
                            let element = $(`#question-is-checked-status-${id}`);
                            element.removeClass("danger").addClass("success");
                            element.html("بررسی شده");
                            $(`#question-is-checked-button-${id}`).css("display", "none");
                            swal({
                                title: "اعلان",
                                text: response.message,
                                icon: "success",
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
        });
}

function deleteQuestion(id) {
    swal({
        title: "آیا مطمئن هستی ؟",
        text: "در صورت انجام این عملیات قادر به بازگردانی آن نمی باشید.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/admin/Question/DeleteQuestion",
                    type: "post",
                    data: {
                        id: id
                    },
                    beforeSend: function () {
                        StartLoading();
                    },
                    success: function (response) {
                        EndLoading();
                        if (response.status === "error") {
                            swal({
                                title: "خطا",
                                text: response.message,
                                icon: "error",
                                button: "باشه"
                            });
                        }
                        else {
                            $(`#question-row-${id}`).fadeOut(500);
                            swal({
                                title: "اعلان",
                                text: response.message,
                                icon: "success",
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
        });
}