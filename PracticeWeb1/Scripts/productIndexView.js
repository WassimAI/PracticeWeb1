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
        var rows = $(".table tr").length;
        var ids = [];

        $.each($("input[name='employeesToDelete']:checked"), function () {
            ids.push($(this).val());
        });

        //alert(ids);

        $.post("/Admin/Product/DeleteMany", ids, function () {
            alert("post is called!");
        });
    }

});