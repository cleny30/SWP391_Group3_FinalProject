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

function ShowStaff(element) {
    var StaffID = element.getAttribute("data-employeeid");

    if (!StaffID) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetStaffInfo',
        type: "POST",
        data: {
            ID: StaffID
        },
        dataType: 'json',
        success: function (data) {
            $('#Staff_Name').html('');
            $('#Staff_PhoneNum').html('');
            $('#Staff_Email').html('');
            $('#Staff_SSN').html('');
            $('#Staff_LivingAddress').html('');
            $('#Staff_Username').html('');

            $('#Staff_Name').html(data.fullname);
            $('#Staff_PhoneNum').html(data.phone);
            $('#Staff_Email').html(data.email);
            $('#Staff_SSN').html(data.ssn);
            $('#Staff_LivingAddress').html(data.address);
            $('#Staff_Username').html(data.username);
            var modal = document.getElementById("myModal");
            modal.style.display = "block";
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}

function GetOrderReceipt(element) {
    var OrderID = element.getAttribute("data-orderid");
    if (!OrderID) {
        console.error('Order ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetOrderInfo',
        type: "POST",
        data: {
            ID: OrderID
        },
        dataType: 'json',
        success: function (data) {
            $('#Order_ID').html('');
            $('#Created_Date').html('');
            $('#Shipped_Date').html('');
            $('#Name').html('');
            $('#PhoneNumber').html('');
            $('#Email').html('');
            $('#Address').html('');
            $('#Staff_Name').html('');
            $('#Description').html('');
            $('#TotalPrice').html('');

            $('#Order_ID').html(data.order.orderId);
            $('#TotalPrice').html("$" + data.order.totalPrice);
            var dateString = data.order.startDay;

            // Parse the date and time
            var date = new Date(dateString);

            // Format the date as 'dd/mm/yyyy'
            var day = date.getDate();
            var month = date.getMonth() + 1; // Months are 0-based
            var year = date.getFullYear();

            // Ensure single-digit day and month are formatted with leading zeros
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }

            var formattedDate = day + "/" + month + "/" + year; // Result: '29/10/2023'


            $('#Created_Date').html(formattedDate);

            var dateString2 = data.order.endDay;

            // Parse the date and time
            var date2 = new Date(dateString2);

            // Format the date as 'dd/mm/yyyy'
            var day2 = date2.getDate();
            var month2 = date2.getMonth() + 1; // Months are 0-based
            var year2 = date2.getFullYear();

            // Ensure single-digit day and month are formatted with leading zeros
            if (day2 < 10) {
                day2 = "0" + day2;
            }
            if (month2 < 10) {
                month2 = "0" + month2;
            }

            var formattedDate2 = day2 + "/" + month2 + "/" + year2; // Result: '29/10/2023'
            if (data.order.endDay === null) {
                $('#Shipped_Date').html('To be Decided');
            } else {
                $('#Shipped_Date').html(formattedDate2);
            }

            $('#Name').html(data.address.fullname);
            $('#PhoneNumber').html(data.address.phonenum);
            $('#Email').html(data.email);
            $('#Address').html(data.address.address);
            if (data.order.staffId === null) {
                $('#Staff_Name').html('To be Decided');
            } else {
                $('#Staff_Name').html(data.staff.fullname);
            }

            $('#Description').html(data.order.description);

            var table = document.getElementById("OrderReceipt"); 
            var rowsToRemove = table.querySelectorAll(".item");

            rowsToRemove.forEach(function (row) {
                row.remove();
            });

            // Create a new table row (tr) with the "item" class
            // Define the number of rows you want to create
            var numRowsToCreate = data.orderDetail.length;

            for (var i = 0; i < numRowsToCreate; i++) {
                var receipt = data.orderDetail[i];
                // Create a new table row (tr) with the "item" class
                var newRow = document.createElement("tr");
                newRow.className = "item";
                // Create three table cells (td) for each column
                var cell1 = document.createElement("td");
                var cell2 = document.createElement("td");
                var cell3 = document.createElement("td");

                // Set the text content of the cells
                cell1.textContent = receipt.productName;
                cell2.textContent = receipt.quantity;
                cell3.textContent = "$" +receipt.price;

                // Append the cells to the row
                newRow.appendChild(cell1);
                newRow.appendChild(cell2);
                newRow.appendChild(cell3);

                // Find the "total" row
                var totalRow = document.querySelector(".total");

                // Insert the new row before the "total" row
                totalRow.parentNode.insertBefore(newRow, totalRow);
            }

            var order_status = data.order.status; // Replace with the actual order_status value

            var buttonsContainer = document.getElementById("buttonsContainer");

            // Clear the content of the buttonsContainer
            buttonsContainer.innerHTML = '';

            // Check the order_status and append anchor tags accordingly
            if (order_status === 1) {
                // Append "Accept Order" and "Cancel Order" anchor tags
                var acceptLink = document.createElement("a");
                acceptLink.className = "btn btn-primary";
                acceptLink.textContent = "Accept Order";
                acceptLink.href = "/Dashboard/AcceptOrder/" + data.order.orderId;
                acceptLink.style.marginBottom = "10px"; // Add padding to the link


                var cancelLink = document.createElement("a");
                cancelLink.className = "btn btn-danger";
                cancelLink.textContent = "Cancel Order";
                cancelLink.href = "/Dashboard/CancelOrder/" + data.order.orderId;

                buttonsContainer.appendChild(acceptLink);
                buttonsContainer.appendChild(cancelLink);
            } else if (order_status === 3) {
                // Append "Complete Order" anchor tag
                var completeLink = document.createElement("a");
                completeLink.className = "btn btn-primary";
                completeLink.textContent = "Complete Order";
                completeLink.href = "/Dashboard/CompletedOrder/" + data.order.orderId;

                buttonsContainer.appendChild(completeLink);
            } else if (order_status === 2) {
                var AcceptedLink = document.createElement("a");
                AcceptedLink.className = "btn btn-primary";
                AcceptedLink.textContent = "Shipped Order";
                AcceptedLink.href = "/Dashboard/ShippedOrder/" + data.order.orderId;
                buttonsContainer.appendChild(AcceptedLink);
            }




            var modal = document.getElementById("myModal");
            modal.style.display = "block";
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
