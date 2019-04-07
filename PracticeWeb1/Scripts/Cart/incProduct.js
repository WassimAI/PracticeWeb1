$(function () {

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
});