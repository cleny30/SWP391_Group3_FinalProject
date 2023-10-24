var usernameExist = true;
var emailExist = true;
$('#Username').on('blur', function () {
    var username = $('#Username').val();
    if (username !== '') {

        $.ajax({
            url: '/Dashboard/CheckUsername',
            type: "POST",
            data: {
                username: username
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Fail") {
                    usernameExist = true;
                    $('#usexist').text('Username has already been taken!');
                } else {
                    usernameExist = false;
                    $('#usexist').text('');
                }
            }
        });
    }
});

$('#Email').on('blur', function () {
    var email = $('#Email').val();
    if (email !== '') {

        $.ajax({
            url: '/Dashboard/CheckEmail',
            type: "POST",
            data: {
                email: email
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Fail") {
                    emailExist = true;
                    $('#emailexist').text('Email has already been used!');
                } else {
                    emailExist = false;
                    $('#emailexist').text('');
                }
            }
        });
    }
});

$(document).ready(function () {
    $('form.createAccount').submit(function (event) {
        if (emailExist || usernameExist) {
            event.preventDefault();
        }
    });
});