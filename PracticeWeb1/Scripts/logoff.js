﻿$(function () {
    $(document).on("click", ".nav #logOff", function (e) {
        e.preventDefault();

        bootbox.confirm("Are you sure you want to empty cart & leave?", function (result) {
            if (result) {
                $.post("/UserAccount/Logoff/", null, function () {
                    location.href = "/Home/Index";
                });
            }
        });


    });
});