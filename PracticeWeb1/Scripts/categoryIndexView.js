$(function () {

    /////////////////////
    /*Search functionality, pagination and sorting are common with productIndexView.js*/

    ///////////////////////////////////////////
    //Delete Multiple function through Aja

    /*Uncomment this when implementing delete multi*/

    //$(".delete-multiple").click(deleteMultiple);

    //function deleteMultiple() {

    //    var count = $("input[name='productsToDelete']:checked").length;
    //    if (count == 0) {
    //        alert("No rows selected to delete");
    //        return false;
    //    }
    //    else {
    //        if (confirm(count + " row(s) will be deleted")) {
    //            var ids = [];

    //            $.each($("input[name='productsToDelete']:checked"), function () {
    //                ids.push($(this).val());
    //            });

    //            $.post("/Admin/Product/DeleteMany", { ids: ids }, function (data) {
    //                window.location = "/Admin/Product";
    //            });
    //        } else { return false;}

            

    //    }

    //}




});