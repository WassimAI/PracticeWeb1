$(function () {
    $(document).on("click", "#add-cart-link", function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/Cart/AddToCart";
        //alert("this item id is " + id);

        $.post(url, { productId: id }, function (data) {
            
            $(".cartDetailsPartial").html(data);
        });
    });
});