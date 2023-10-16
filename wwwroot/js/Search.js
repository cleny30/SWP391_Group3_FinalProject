const search = () => {
    const searchbox = document.getElementById("search-item").value.toUpperCase();
    const storeitems = document.getElementById("product-list");
    const product = document.querySelectorAll(".product");
    const pname = storeitems.getElementsByTagName("h2");

    let hasMatchingProduct = false; // Biến để kiểm tra xem có kết quả tìm kiếm nào không
    if (searchbox.length >= 3) {
        $.ajax({
            url: '/Home/SearchItem',
            type: "POST",
            data: {
                searchbox: searchbox
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                var productlist = $('#product-list');
                productlist.empty();

                $.each(data, function (index, product) {
                    var url = '/Product/ShopDetail?pro_id=' + product.pro_id;
                    var productDiv = $('<div class="product" onclick="location.href=\'' + url + '\'">');
                    productDiv.append('<img src="' + product.pro_img[0] + '" alt="">');
                    var pDetailsDiv = $('<div class="p-details">');
                    pDetailsDiv.append('<h2>' + product.pro_name + '</h2>');
                    if (product.discount > 0) {
                        var priceDiscount = product.pro_price - (product.pro_price * product.discount) / 100;

                        pDetailsDiv.append('<h3>$' + priceDiscount + ' <span class="text-muted ml-2"><del>$' + product.pro_price + '</del></span></h3>');
                    } else {
                        pDetailsDiv.append('<h3>$' + product.pro_price + '</h3>');
                    }

                    productDiv.append(pDetailsDiv);
                    productlist.append(productDiv);
                });
                var seeDetailsDiv = $('<div class="see-details" onclick="location.href=\'/Product/ShopSearch?searchTerm=' + searchbox + '\'">');
                var dividerDiv = $('<div class="divider"></div>');
                var brElement = $('<br>');
                var seeMoreDetails = $('<p>See more details</p> </div>');
                seeDetailsDiv.append(dividerDiv);
                seeDetailsDiv.append(brElement);
                seeDetailsDiv.append(seeMoreDetails);
                productlist.append(seeDetailsDiv);
            }
        });
        for (var i = 0; i < pname.length; i++) {
            let match = product[i].getElementsByTagName("h2")[0];
            if (match) {
                let textvalue = match.textContent || match.innerHTML;
                if (textvalue.toUpperCase().includes(searchbox) || textvalue.toUpperCase().indexOf(searchbox) > -1) {
                    product[i].style.display = "";
                    hasMatchingProduct = true; // Đánh dấu có kết quả tìm kiếm

                } else {
                    product[i].style.display = "none";
                }
            }
        }

    }

    // Kiểm tra biến và ẩn hoặc hiển thị dòng showItem và showNoItem
    if (hasMatchingProduct) {
        storeitems.style.display = "block";

    } else {
        storeitems.style.display = "none";

    }
}

