$(function () {

    /*Modal Showing*/
    $(document).on("click", "a.place-order-btn", function (e) {
        //e.preventDefault();

        $("#PlaceOrderModel").modal("show");
    });

    /*Placing order function*/
    $(document).on("click", ".confirm-order-btn", function (e) {
        $("div.spinner").hide();
        e.preventDefault();
        var url1 = "/Cart/checkAddress";
        var url2 = "/Order/PlaceOrder";
        var userName = $("#userNameDiv").data("id");

        $.getJSON(url1, { userName: userName }, function (data) {
            if (data.response == "error") {
                //Work on UI & UX here
                alert("Error has happened, user Does not exist!");
            } else if (data.response == "No") {
                //Work on UI & UX here
                location.href = "/UserAccount/userProfile?userName=" + userName + "&returnUrl=/Cart/PlaceOrder";
            } else {
                $("#checkAddressModal").modal("show");
                $.get("/UserAccount/AddressPartial", null, function (data) { $(".address-partial-div").html(data); });

                $(document).on("click", ".btn-proceed", function (e) {
                    e.preventDefault();
                    $("div.spinner").show();
                    $(".address-partial-div").html("<p>Please wait while we process your order.</p>");
                    $.post(url2, null, function (data) {
                        setTimeout(function () {
                            location.href = "/Order/UserOrders";
                        },2000);
                    });
                });

                $(document).on("click", ".btn-edit-address", function (e) {
                    e.preventDefault();
                    location.href = "/UserAccount/userProfile?userName=" + userName + "&returnUrl=/Cart/PlaceOrder";
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