function ChangeAddress() {
    // Select the checked radio input
    var selectInput = $('input[name="radio_add"]:checked');

    // Use .attr() to retrieve attribute values, not .getAtribute
    var fullname = selectInput.attr('data-AFN');
    var phonenum = selectInput.attr('data-APN');
    var address = selectInput.attr('data-AA');

    console.log('Full Name:', fullname);
    console.log('Phone Number:', phonenum);
    console.log('Address:', address);

    // Update the input fields and paragraphs
    $('#ChosenFNA').val(fullname);
    $('#ChosenFNA').next('p').text(fullname);

    $('#ChosenPNA').val(phonenum);
    $('#ChosenPNA').next('p').text(phonenum);

    $('#ChosenADA').val(address);
    $('#ChosenADA').next('p').text(address);
    closeFormAddress();
}


function PlaceOrder() {
    //Address
    var fullname = $('#ChosenFNA').val();
    var phonenum = $('#ChosenPNA').val();
    var address = $('#ChosenADA').val();

    //Note
    var des = $('#description').val();

    var bill = $('#bill').val();

    if (des === "") {
        des = "None";
    }

    $.ajax({
        url: '/Cart/Checkout',
        type: "POST",
        data: {
            fullname: fullname,
            phonenum: phonenum,
            address: address,
            des: des,
            bill: bill
        },
        success: function (data) {
            window.location.href = "/Cart/Checkout";
        }
    });
}