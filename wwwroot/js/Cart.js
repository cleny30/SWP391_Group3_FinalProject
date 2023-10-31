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
    var quan = element.getAttribute('data-quan_input');
    if (quan===null) {
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
                $('#myModal-check').css('display', 'block');
                setTimeout(function () {
                    $('#myModal-check').css('display', 'none');
                }, 1500);
            }
        }
    });
}

function updateCartQuantity(productId, username, quantityChange) {
    var currentQuantity = parseInt($('#num-' + productId).val());

    if (quantityChange === null) {
        currentQuantity = 0;
        quantityChange = $('#num-' + productId).val();
    }
    
    var newQuantity = parseInt(currentQuantity + quantityChange);
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
            if (data.noti === 'Success' || data.noti === 'Out of Stock!') {
                $('#num-' + productId).val(data.quanN);

                $('#item-price-' + productId).text('');
                $('#item-price-' + productId).text('$' + data.total);

                $('#bill-cart').text('');
                $('#bill-cart').text('$' + data.bill);
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

window.onclick = function (event) {
    var modal = $('#myModal-check');
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

function SubmitCheckOut() {
    // Lấy tất cả các phần tử có lớp "status-Stock"
    var stockElements = $(".status-Stock");
    var hasZeroValue = false;
    var msg = $('#msg-alert-msg');
    // Lặp qua tất cả các phần tử
    stockElements.each(function () {
        // Lấy giá trị của phần tử hiện tại và chuyển đổi nó thành số nguyên
        var value = parseInt($(this).val());

        // Kiểm tra nếu giá trị bằng 0
        if (value === 1) {
            // Đánh dấu có giá trị bằng 0
            hasZeroValue = true;
            msg.text('');
            msg.text('Warning: Your cart contains out of stock products');
            // Hiển thị thông báo
            return false;
        } else if (value === 2) {
            // Đánh dấu có giá trị bằng 0
            hasZeroValue = true;
            msg.text('');
            msg.text('Warning: Your cart contains unavailable products');
            // Hiển thị thông báo
            return false;
        }
    });

    // Kiểm tra nếu không có giá trị nào bằng 0
    if (!hasZeroValue) {
        // Chuyển hướng trang
        window.location.href = 'Cart/Checkout';
    } else {
        alertMessage();
    }
}

function alertMessage() {
    $('#alert-message-checkout').removeClass("hide");
    $('#alert-message-checkout').addClass("show");
    $('#alert-message-checkout').addClass("showAlert");

    setTimeout(function () {
        $('#alert-message-checkout').removeClass("show");
        $('#alert-message-checkout').removeClass("showAlert");
        $('#alert-message-checkout').addClass("hide");
    }, 2000)
}

$('#close-btn-alert').click(function () {
    $('#alert-message-checkout').removeClass("show");
    $('#alert-message-checkout').removeClass("showAlert");
    $('#alert-message-checkout').addClass("hide");
})