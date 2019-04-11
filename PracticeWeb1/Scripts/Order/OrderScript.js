$(function () {

    $(document).on("click", "a.place-order-btn", function (e) {
        //e.preventDefault();

        $("#PlaceOrderModel").modal("show");
    });

    $(document).on("click", ".confirm-order-btn", function (e) {
        e.preventDefault();
        alert("Got it man!");
        var url = "/Order/PlaceOrder";

        $.post(url, null, function (data) {
            location.href = "/Order/UserOrders";
        });
    });

});