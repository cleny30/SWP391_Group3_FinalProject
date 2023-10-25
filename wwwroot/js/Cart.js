function DeleteCart(element) {
    var us = element.getAttribute('data-us');
    var id = element.getAttribute('data-pro_id');
    var url = window.location.href;
    $.ajax({
        url: '/Cart/DeleteCart',
        type: "POST",
        data: {
            us: us,
            pro_id: id
        },
        success: function (data) {
            if (data === "Success") {
                $('#itemCart-' + id).remove();
                window.location.href = url;
            }
        }
    });
}

function AddToCart(element) {
    var id = element.getAttribute('data-pro_id');
    var quan = element.getAttribute('$quan_input');
    if (!isNaN(quan)) {
        quan = 1;
    }
    $.ajax({
        url: '/Cart/AddToCart',
        type: "POST",
        data: {
            pro_id: id,
            quantity: quan
        },
        success: function (data) {
            if (data === "fail") {
                window.location.href = "/Login"
            } else {
                $('.cart-value').text(data);

            }
        }
    });
}

function updateCartQuantity(productId, username, quantityChange) {
    var currentQuantity = parseInt($('.itemCart').val());
    var newQuantity = currentQuantity + quantityChange;
    var url = window.location.href;
    if (newQuantity < 1) {
        alert("Quantity cannot be less than 1.");
        return;
    }

    $.ajax({
        url: '/Cart/UpdateQuantity',
        method: 'POST',
        data:
        {
            pro_id: productId,
            username: username,
            quantity: newQuantity
        },
        success: function (data) {
            if (data === 'Success') {
                $('.itemCart').val(newQuantity);
                //window.location.href = url;
            } else {
                alert("Failed to update the cart.");
            }
        },
        error: function () {
            alert("An error occurred while updating the cart.");
        }
    });
}

