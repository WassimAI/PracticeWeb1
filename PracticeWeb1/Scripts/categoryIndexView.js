$(function () {

    /////////////////////
    /*Search functionality, pagination and sorting are common with productIndexView.js*/

    ///////////////////////////////////////////
    //Delete Multiple function through Aja

    /*Uncomment this when implementing delete multi*/


    $(".delete-multiple-category").click(deleteMultiple);

    function deleteMultiple(e) {
        e.preventDefault();
        var count = $("input[name='categoriesToDelete']:checked").length;
        if (count == 0) {
            alert("No rows selected to delete");
            return false;
        }
        else {
            if (confirm(count + " row(s) will be deleted")) {
                var ids = [];

                $.each($("input[name='categoriesToDelete']:checked"), function () {
                    ids.push($(this).val());
                });

                $.post("/Admin/Category/DeleteMany", { ids: ids }, function (data) {
                    window.location = "/Admin/Category";
                });
            } else { return false;}

            

        }

    }




});