$(function () {
    $(".btn-register").click(function (e) {
        if ($("#registrationForm").valid()) {
            toastr.success("Thank you for registering!")
        }
    });
});