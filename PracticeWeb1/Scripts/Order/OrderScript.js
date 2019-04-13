$(function () {

    $(document).on("click", "a.place-order-btn", function (e) {
        //e.preventDefault();

        $("#PlaceOrderModel").modal("show");
    });

    $(document).on("click", ".confirm-order-btn", function (e) {
        e.preventDefault();
        var url = "/Order/PlaceOrder";

        $.post(url, null, function (data) {
            location.href = "/Order/UserOrders";
        });
    });

    /*Order Details Partial*/
    $(document).on("click", ".order-details" , function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var url = "/Order/OrderDetailsPartial/" + id;

        $.get(url, null , function (data) {
            $(".order-details-div").html(data);
        });
    });

});