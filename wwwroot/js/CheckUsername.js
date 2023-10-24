var usernameExist = true;
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

$(document).ready(function () {
    $('form.createAccount').submit(function (event) {
        if (usernameExist == true) {
            event.preventDefault();
        }
    });
});