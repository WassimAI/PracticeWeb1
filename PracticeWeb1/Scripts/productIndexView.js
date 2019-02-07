$(function () {

    ///////////////////////////////////////////
    //Clear Search Button
    $(".clear-btn").click(clearSearchText);

    function clearSearchText() {
        $(".search-text").val("");
        $(".categories-drop-down").val("All");
        $(".search-btn").click();
    }

    /////////////////////////////////////
    //Select All checkbox

    $('#selectAll').click(function () {
        $('input:checkbox').not(this).prop('checked', this.checked);
    });

  

    ///////////////////////////////////////////
    //Delete Multiple function through Aja
    $(".delete-multiple").click(deleteMultiple);

    function deleteMultiple() {

            var count = $("input[name='productsToDelete']:checked").length;
            if (count == 0) {
                alert("No rows selected to delete");
                return false;
            }
            else {
                confirm(count + " row(s) will be deleted");
                var ids = [];

                $.each($("input[name='productsToDelete']:checked"), function () {
                    ids.push($(this).val());
                });

                $.post("/Admin/Product/DeleteMany", { ids: ids }, function (data) {
                    window.location = "/Admin/Product";
                });
            }


        //This also works:

        //$.ajax({
        //    url: "/Admin/Product/DeleteMany",
        //    type: 'POST',
        //    traditional: true,
        //    data: { ids: ids},
        //    success: function (response) {
        //        window.location = "/Admin/Product";
        //    }
        //});
    }

});