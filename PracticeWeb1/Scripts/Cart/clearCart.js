$(function () {
    $(document).on("click", ".clear-cart", function (e) {
        e.preventDefault();

        var url1 = "/Cart/ClearCart";

        $.post(url1, null, function (data) {
            location.href = "/Cart/Index";
        });


    });
});