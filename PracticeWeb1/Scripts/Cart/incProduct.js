$(function () {

    /*Increment Product*/
    $(document).on("click", ".btn-inc-prod", function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/Cart/incProduct";

        $.getJSON(url, { id: id }, function (data) {
            $("td#itemPrice_" + id).html(data.itemPrice);
            $("td#quantity_" + id).html(data.quantity);

            var qty = $("td#quantity_" + id).text();
            var price = $("td#itemPrice_" + id).text();
            
            //alert(qty + " " + price);
            $("td#itemTotalPrice_" + id).html(qty * price);

            $("td#totalQuantity").html(data.totalQuantity);
            $("td#totalPrice").html(data.totalPrice);
            $("td#itemTotalPrice_" + id).html(data.itemTotalPrice);
        });
    });

    /*Decrement Product*/
    $(document).on("click", ".btn-dec-prod", function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/Cart/decProduct";

        $.getJSON(url, { id: id }, function (data) {

            if (!data.isLastItem) {
                $("td#itemPrice_" + id).html(data.itemPrice);
                $("td#quantity_" + id).html(data.quantity);

                var qty = $("td#quantity_" + id).text();
                var price = $("td#itemPrice_" + id).text();

                $("td#itemTotalPrice_" + id).html(qty * price);

                $("td#totalQuantity").html(data.totalQuantity);
                $("td#totalPrice").html(data.totalPrice);
                $("td#itemTotalPrice_" + id).html(data.itemTotalPrice);
            } else {
                $("#row_" + id).remove();
                $("td#totalQuantity").html(data.totalQuantity);
                $("td#totalPrice").html(data.totalPrice);
                if (data.isCartEmpty) {
                    $(".place-order-btn").hide();
                    $(".clear-cart").hide();
                    $(".table").remove();
                    $(".empty-cart").append("<p>There are no items in your cart.</p>");
                }
            }
            
        });
    });

});