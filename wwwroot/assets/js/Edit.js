function EditBrand(element) {
    var brandID = element.getAttribute("data-brandedit");

    // Check if brandID is not null or empty
    if (!brandID) {
        console.error('Brand ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetBrandInfo',
        type: "POST",
        data: {
            brand_id: brandID
        },
        dataType: 'json',
        success: function (data) {
            // Update DOM elements with retrieved data
            $('#Brand_Name_edit').val(data.brand_name);
            $('#Brand_Image_tmp').val(data.brand_img);
            $('#Brand_ID_edit').val(data.brand_id);
            $('#brand_img_edit').attr('src', data.brand_img);
            $('#contactFormBrandEdit').fadeToggle();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}


function EditCat(element) {
    var cateID = element.getAttribute("data-CatEdit");

    // Check if brandID is not null or empty
    if (!cateID) {
        console.error('Category ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetCategoryInfo',
        type: "POST",
        data: {
            cate_id: cateID
        },
        dataType: 'json',
        success: function (data) {
            // Update DOM elements with retrieved data
            $('#Cat_Name_edit').val(data.cate_name);
            $('#Cat_ID_edit').val(data.cate_id);
            $('#contactFormEditCategory').fadeToggle();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}


function ShowIR(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }
    
    $.ajax({
        url: '/Dashboard/GetIRInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            $('#IR_Name').html(''); // Clear the Name element
            $('#IR_ID').html('');   // Clear the ID element
            $('#IR_Date').html(''); // Clear the Date element
            $('#IR_Payment').html(''); // Clear the Payment element

            // Update DOM elements with retrieved data
            $('#IR_Name').html(data.person_In_Charge );
            $('#IR_ID').html(data.reciept_ID);
            var rawDate = new Date(data.date_Import);

            // Format the date as "MM/DD/YYYY"
            var formattedDate = (rawDate.getMonth() + 1) + '/' + rawDate.getDate() + '/' + rawDate.getFullYear();

            // Update the HTML element with the formatted date
            $('#IR_Date').html(formattedDate);
            $('#IR_Payment').html('$' + data.payment);
            

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

function ShowRP(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetRPInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            var tableBody = $('#receiptTable tbody');
            tableBody.empty();
            for (var i = 0; i < data.length; i++) {
                var receipt = data[i];

                // Create a new row for the receipt data
                var newRow = $('<tr>');

                newRow.append($('<td>').text(i+1));
                // Append columns for Name, ID, Date, and Payment
                newRow.append($('<td>').text(receipt.pro_name));
                // Append a dollar sign "$" in front of the payment
                newRow.append($('<td>').text('$' + receipt.price));
                // Format the date as "MM/DD/YYYY"
                newRow.append($('<td>').text(receipt.amount));

               

                // Append the row to the table body
                tableBody.append(newRow);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}
