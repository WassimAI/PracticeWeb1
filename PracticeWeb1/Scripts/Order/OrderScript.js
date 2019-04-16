$(function () {

    /*Modal Showing*/
    $(document).on("click", "a.place-order-btn", function (e) {
        //e.preventDefault();

        $("#PlaceOrderModel").modal("show");
    });

    /*Placing order function*/
    $(document).on("click", ".confirm-order-btn", function (e) {
        e.preventDefault();
        var url1 = "/Cart/checkAddress";
        var url2 = "/Order/PlaceOrder";
        var userName = $("#userNameDiv").data("id");

        $.getJSON(url1, { userName: userName }, function (data) {
            if (data.response == "error") {
                alert("Error has happened, user Does not exist!");
            } else if (data.response == "No") {
                //alert("you do not have address chosen");
                $("#checkAddressModal").modal("show");
            } else {
                $.post(url2, null, function (data) {
                    location.href = "/Order/UserOrders";
                });
            }
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