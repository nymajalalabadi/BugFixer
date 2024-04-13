function loadTagsModal(url)
{

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
                <button onclick="LoadCraeteTagModal()" class="btn btn-success btn-xs mr-5">افزودن تگ جدید</button>
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



function LoadCraeteTagModal()
{

    $.ajax({
        url: "/admin/home/LoadCreateTagPartial",
        type: "get",
        beforeSend: function () {
            StartLoading("#LargeModalBody");
        },
        success: function (response) {
            EndLoading("#LargeModalBody");

            $("#MediumModalLabel").html("افزودن تگ جدید");
            $("#MediumModalBody").html(response);

            $('#create-tag-form').removeData('validator', 'unobtrusiveValidation');
            $.validator.unobtrusive.parse('#create-tag-form');

            $("#MediumModal").modal("show");
        },
        error: function () {
            EndLoading("#LargeModalBody");
            swal({
                title: "خطا",
                text: "عملیات با خطا مواجه شد لطفا مجدد تلاش کنید .",
                icon: "error",
                button: "باشه"
            });
        }
    });

}


function CreateTagDone(response)
{
    if (response.status === "error")
    {
        swal({
            title: "خطا",
            text: response.message,
            icon: "error",
            button: "باشه"
        });
    }
    else
    {
        $("#MediumModal").modal("hide");

        $("#filter_ajax_form").submit();

        swal({
            title: "اعلان",
            text: response.message,
            icon: "success",
            button: "باشه"
        });
    }
}