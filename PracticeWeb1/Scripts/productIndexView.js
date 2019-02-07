$(function () {

    ///////////////////////////////////////////
    //Clear Search Button
    $(".clear-btn").click(clearSearchText);

    function clearSearchText() {
        $(".search-text").val("");
        $(".categories-drop-down").val("All");
        $(".search-btn").click();
    }

    ///////////////////////////////////////////
    //Delete Multiple function through Aja
    $(".delete-multiple").click(deleteMultiple);

    function deleteMultiple() {

        var ids = [];

        $.each($("input[name='employeesToDelete']:checked"), function () {
            ids.push($(this).val());
        });

        $.post("/Admin/Product/DeleteMany", { ids : ids }, function(data){
            window.location = "/Admin/Product";
        });

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