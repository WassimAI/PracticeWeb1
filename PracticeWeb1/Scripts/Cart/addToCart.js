$(function () {
    $(document).on("click", "#add-cart-link", function (e) {
        e.preventDefault();

        var id = $(this).data("id");

        var url1 = "/Cart/AddToCart";
        var url2 = "/Cart/CartPartial";

        $.post(url1, { productId: id }, function (data) {
            $(".cartDetailsPartial").html(data);
        });

    });
});