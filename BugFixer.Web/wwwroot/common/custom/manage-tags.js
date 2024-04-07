function LoadTagsModal(url) {

    $.ajax({
        url: url,
        type: "get",
        beforeSend: function () {
            StartLoading("#LargeModal");
        },
        success: function (response) {
            EndLoading("#LargeModal");

            $("#LargeModalBody").html(response);
            $("#LargeModalLabel").html("مدیریت  تگ ها");
            $("#LargeModal").modal("show");
        },
        error: function () {
            EndLoading("#LargeModal");
            swal({
                title: "خطا",
                text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                icon: "error",
                button: "باشه"
            });
        }
    });

}
