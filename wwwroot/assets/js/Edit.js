function EditBrand(element) {
    var brandID = element.getAttribute("data-BrandEdit");
    $.ajax({
        url: "/Dashboard/GetBrandInfo",
        type: "POST",
        data: {
            brand_id: brandID
        },
        dataType = 'json',
        success: function (data) {
            $('#Brand_Name').val(data.brand_name);
            $('#Brand_Image').val(data.brand_img);
            $('#Brand_ID').val(data.brand_id);
        }

    });
    
}