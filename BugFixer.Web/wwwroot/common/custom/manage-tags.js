function LoadTagsModal(url) {

    $.ajax({
        url: url,
        type: "get",
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();

            $("#LargeModalBody").html(response);
            $("#LargeModalLabel").html(`
                <span>مدیریت تگ ها</span>
                <button onclick="loadCreateTagModal()" class="btn btn-success btn-xs mr-5">افزودن تگ جدید</button>
            `);

            $("#LargeModal").modal("show");
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
