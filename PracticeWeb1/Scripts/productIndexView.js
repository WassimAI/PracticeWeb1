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
            if (confirm(count + " row(s) will be deleted")) {
                var ids = [];

                $.each($("input[name='productsToDelete']:checked"), function () {
                    ids.push($(this).val());
                });

                $.post("/Admin/Product/DeleteMany", { ids: ids }, function (data) {
                    window.location = "/Admin/Product";
                });
            } else { return false;}

            

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

    //////////////////////////////////
    /*Saving Gallery Images on Edit with DropZone*/
    Dropzone.options.dropzoneForm = {
        acceptedFiles: "image/*",
        init: function() {
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    location.reload();
                }
            });

            this.on("sending", function (file, xhr, formData) {
                formData.append("id", parseInt($(".dropzone").attr("data-id-model")));
            });
        }
    };

    ////////////////////////////
    /*Deleting Gallery Image*/
    $("a.deleteimage").click(function (e) {
        e.preventDefault();

        if (!confirm("Confirm deletion")) return false;

        var $this = $(this);
        var url = "/admin/Product/DeleteImage";
        var imageName = $this.data("name");

        $.post(url, { id: parseInt($(".dropzone").attr("data-id-model")), imageName: imageName }, function (data) {
            $this.parent().fadeOut("fast");
        });

    });

    ////////////////////////////
    /*Implementing FancyBox*/

    $(".fancybox").fancybox();

});