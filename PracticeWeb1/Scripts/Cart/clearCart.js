$(function () {
    $(document).on("click", ".clear-cart", function (e) {
        e.preventDefault();

        var url1 = "/Cart/ClearCart";
        var url2 = "/Cart/CartPartial";

        $.post(url1, null, function (data) {
            $(".cartDetailsPartial").html(data);
        });

        $.get(url2, null, function (data) {
            $("#cart-link").html(data);
        });

    });
});