$(document).ready(function () {
    //Khai bÃ¡o biáº¿n
    var $email = $('#email');
    var $BtnSend = $('#BtnSend');
    var $BtnOtp = $('#BtnOtp');
    var $emailErrorMessage = $email.next('p');
    var $otp1 = $('#otp1');
    var $ServerOTP = $('#ServerOTP');
    var $password = $('#password');
    var $rePassword = $('#rePassword');
    var $submit = $('#submit');

    //VÃ´ hiá»‡u hoÃ¡ nÃºt gá»­i OTP Ä‘áº¿n emai vÃ  nÃºt Kiá»ƒm tra mÃ£ OTP
    $BtnSend.prop("disabled", true);
    $BtnOtp.prop("disabled", true);

    //Chuyá»ƒn tá»« form email sang form nháº­p OTP
    $("#BtnSend").click(function () {
        $("#EntryOTP").addClass('showform');
        $("#Entryemail").removeClass('showform');
    });


    //Chuyá»ƒn tá»« form OTP vá» form email
    $(".arrow").click(function () {
        $("#EntryOTP").removeClass("showform");
        $("#Entryemail").addClass("showform");
    });

    //Chuyá»ƒn tá»« form nháº­p pass vá» OTP
    $(".arrow2").click(function () {
        $("#Resetpass").removeClass("showform");
        $("#EntryOTP").addClass("showform");
    });

    //Kiá»ƒm tra email há»£p lá»‡ hay khÃ´ng
    $email.on('change', function () {
        var email = $email.val();
        var noError = true;

        const emailPattern = /^[a-zA-Z][a-zA-Z0-9._%+-]+@[^\s@]+\.[^\s@]{2,}$/;

        if (email === '') {
            $emailErrorMessage.text("Please enter your email address");
            noError = false;
        } else if (!emailPattern.test(email)) {
            $emailErrorMessage.text("Email khong hop le.");
            noError = false;
        } else {
            $emailErrorMessage.text("");
        }
        //Cáº­p nháº­t tráº¡ng thÃ¡i nÃºt tÆ°Æ¡ng á»©ng vá»›i noError
        $BtnSend.prop("disabled", !noError);

        if (noError) {
            var request = $.ajax({
                type: 'POST',
                data: {
                    email: email,
                },
                url: '/SendEmail/CheckEmail'
            });

            request.done(function (result) {
                if (result === "false") {
                    console.log("result lÃ : " + result);
                    $emailErrorMessage.text("email khÃ´ng tá»“n táº¡i");
                    $BtnSend.prop("disabled", true);
                } else {
                    $BtnSend.prop("disabled", false);
                }
            });
        }
        $('#emailSend').val(email);
        $(".emailmsg").html("<span>" + email + "</span>");
    });

    //Äáº£m báº£o OTP Ä‘Æ°á»£c nháº­p
    $otp1.on('change', function () {
        var otp1 = $otp1.val();
        if (otp1 !== null) {
            $BtnOtp.prop("disabled", false);
        }
    });

    //Khi nÃºt Send Ä‘Æ°á»£c nháº¥n thÃ¬ sáº½ gá»­i mÃ£ OTP Ä‘áº¿n email tÆ°Æ¡ng á»©ng
    $BtnSend.click(function () {
        var email = $email.val();
        var request = $.ajax({
            type: 'POST',
            data: {
                email: email
            },
            url: '/SendEmail'
        });

        request.done(function (result) {
            if (result !== null) {
                console.log("result lÃ : " + result);
                $ServerOTP.val(result);
                console.log("mÃ£ server lÃ : " + $ServerOTP.val());
            }
        });

        request.fail(function (jqXHR, textStatus) {
            console.log("Request failed: " + textStatus);
        });
    });

    //Láº¥y giÃ¡ trá»‹ cá»§a mÃ£ OTP ngÆ°á»i dÃ¹ng nháº­p vÃ  so sÃ¡nh vá»›i mÃ£ OTP cá»§a server xem há»£p lá»‡ khÃ´ng
    $BtnOtp.click(function () {
        var OTP = "";
        for (var i = 1; i <= 6; i++) {
            OTP += $('#otp' + i).val();
        }

        var ServerOtp = $ServerOTP.val();
        console.log("JS OTP is: " + OTP);
        console.log("JS ServerOtp is: " + ServerOtp);
        if (OTP === ServerOtp) {
            $("#Resetpass").addClass("showform");
            $("#EntryOTP").removeClass("showform");
        }
    });

    //Kiá»ƒm tra pass há»£p lá»‡ khÃ´ng
    $password.on('blur', function () {
        var password = $password.val();
        $submit.prop("disabled", true);
        if (password === "") {
            $password.next('p').text("Vui lÃ²ng nháº­p máº­t kháº©u.");
        } else if (password.length < 8) { //Ä‘á»•i chá»— nÃ y
            $password.next('p').text("Máº­t kháº©u pháº£i Ã­t nháº¥t 8 kÃ­ tá»±.");
        } else {
            $password.next('p').text("");
            $submit.prop("disabled", false);
        }
        console.log("pass" + $submit.prop("disabled"));
    });

    //Kiá»ƒm tra repass há»£p lá»‡ khÃ´ng + Ä‘Ãºng vá»›i pass
    $rePassword.on('blur', function () {
        $submit.prop("disabled", true);
        var rePassword = $rePassword.val();
        var password = $password.val();
        if (rePassword === "") {
            $rePassword.next('p').text("Vui lÃ²ng nháº­p láº¡i máº­t kháº©u.");
        } else if (rePassword !== password) {
            $rePassword.next('p').text("Máº­t kháº©u khÃ´ng khá»›p.");
        } else {
            $rePassword.next('p').text("");
            $submit.prop("disabled", false);
        }
        console.log("repass" + $submit.prop("disabled"));
    });
});
