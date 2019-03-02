$(function () {

    $(".alertDiv").addClass("hide");

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

                    var $response = parseInt(data);

                    if ($response == 1) {
                        window.location = "/Admin/Category";
                    } if ($response == -1) {
                        $(".alertDiv").removeClass("hide");
                        $(".alertDiv").addClass("show");
                        $(".alertDiv").html("One or more categories have products assigned to it, you must delete those products first.");
                        $(".alertDiv").append("<a href='#' class='removeDiv pull-right'><span class='glyphicon glyphicon-remove'></span></a>");
                        $(".alertDiv .removeDiv").click(function (e) {
                            $(".alertDiv").removeClass("show");
                            $(".alertDiv").addClass("hide");
                        });
                    }

                });
            } else { return false; }

        }

    }


});