$(function () {

    $(document).on("click", ".btn-inc-prod", function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/Cart/incProduct";

        $.post(url, { id: id }, function (data) {
            $("body .body-content").html(data);
        });
    });
});