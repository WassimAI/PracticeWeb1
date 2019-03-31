$(function () {
    $(document).on("click", ".clear-cart", function (e) {
        e.preventDefault();

        var url = "/Cart/ClearCart";

        $.post(url, null, function (data) {
            $(".cartPartial").html(data);
        });

    });
});