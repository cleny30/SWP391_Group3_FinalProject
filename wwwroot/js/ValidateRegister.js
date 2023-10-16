$(document).ready(function () {
    $('form.RegisterForm').submit(function (event) {
        $(window).on('unload', function () {// Reset giá trị các biến về trạng thái ban đầu
            noError = true;
        });

        var username = getValueById('ustxt');
        var fullname = getValueById('nametxt');
        var phone = getValueById('phonetxt');
        var email = getValueById('emailtxt');
        var password = getValueById('pwdtxt');
        var repassword = getValueById('re_pwdtxt');

        var noError = true;


        //tài khoản
        if (!username) {
            showError('ustxt', 'Please enter username');
            noError = false;
        } else if (username.length > 20 || username.length < 6) {
            showError('ustxt', 'Username must be from 6 - 20 characters');
            noError = false;
        } else {
            hideError('ustxt');
        }

        //Họ và tên
        if (!fullname) {
            showError('nametxt', 'Please enter your full name');
            noError = false;
        } else if (fullname.length > 100) {
            showError('nametxt', 'Your full name is less than 100 characters');
            noError = false;
        } else {
            hideError('nametxt');
        }

        // Số điện thoại
        if (!phone) {
            showError('phonetxt', 'Please enter your phone number.');
            noError = false;
        } else if (!isValidPhoneNumber(phone)) {
            showError('phonetxt', 'Invalid phone number.');
            noError = false;
        } else {
            hideError('phonetxt');
        }

        //Email
        if (!email) {
            showError('emailtxt', 'Please enter email address.');
            noError = false;
        } else if (!isValidEmail(email)) {
            showError('emailtxt', 'Email address is not valid.');
            noError = false;
        } else {
            hideError('emailtxt');
        }

        //Password
        if (!password) {
            showError('pwdtxt', 'Please enter a password.');
            noError = false;
        } else if (password.length < 8) {
            showError('pwdtxt', 'Password must be at least 8 characters.');
            noError = false;
        } else {
            hideError('pwdtxt');
        }

        //Repassword
        if (!repassword) {
            showError('re_pwdtxt', 'Please re-enter your password.');
            noError = false;
        } else if (repassword !== password) {
            showError('re_pwdtxt', 'Password incorrect.');
            noError = false;
        } else {
            hideError('re_pwdtxt');
        }

        if (!noError) {
            event.preventDefault();
        }
    });
});


$('#emailtxt').on('blur', function () {
    var email = getValueById('emailtxt');
    if (email !== null || email !== '') {
        $.ajax({
            url: '/Login/CheckEmail',
            type: "POST",
            data: {
                email: email
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data == 'true') {
                    $('#btnResigterSubmit').css('pointer-events', 'none');
                    showError('emailtxt', 'Email is already exist');
                } else {
                    $('#btnResigterSubmit').css('pointer-events', 'auto');
                    hideError('emailtxt');
                }
            }
        });
    }

});

$('#ustxt').on('blur', function () {
    var username = getValueById('ustxt');
    if (username !== null || username !== '') {
        $.ajax({
            url: '/Login/CheckUsername',
            type: "POST",
            data: {
                username: username
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data == 'true') {
                    $('#btnResigterSubmit').css('pointer-events', 'none');
                    showError('ustxt', 'Username is already exist');
                } else {
                    $('#btnResigterSubmit').css('pointer-events', 'auto');
                    hideError('ustxt');
                }
            }
        });
    }
})

// Lấy giá tin input
function getValueById(id) {
    return $('#' + id).val();
}

// Hiển thị thông báo lỗi cho một trường
function showError(id, message) {
    $('#' + id).next('p').text(message);
}

// Ẩn thông báo lỗi của một trường
function hideError(id) {
    $('#' + id).next('p').text('');
}

function isValidPhoneNumber(phoneNum) {
    const phonePattern = /^0\d{9}$/;
    return phonePattern.test(phoneNum);
}

// Kiểm tra tính hợp lệ của địa chỉ email
function isValidEmail(email) {
    const emailPattern = /^[a-zA-Z][a-zA-Z0-9._%+-]+@[^\s@]+\.[^\s@]{2,}$/;
    return emailPattern.test(email);
}