function openForm(element) {
    var id = element.getAttribute('data-orderId');
    $.ajax({
        url: '/Account/OrderDetail',
        type: "POST",
        data: {
            id: id
        },
        success: function (data) {
            // Update DOM elements with retrieved data
            var listOrderD = $('#listOrderDetail');
            listOrderD.empty();
            $.each(data, function (index, item) {
                var htmlCode = '<div class="row">' +
                    '<div class="col-9">' +
                    '<span id="name">' + item.productName + '</span>' +
                    '</div>' +
                    '<div class="col-3">' +
                    '<span id="price">&pound;' + item.price + '</span>' +
                    '</div>' +
                    '</div>';
                listOrderD.append(htmlCode);
            })
        }
    });
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}