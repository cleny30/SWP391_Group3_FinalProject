$(document).ready(function () {
    $('form.AddForm').submit(function (event) {
        var pro_name = getValueById('pro_name');
        var pro_price = getValueById('pro_price');
        var noError = true;

        // Kiểm tra pro_name không được để trống
        if (!pro_name) {
            showError('pro_name', 'Hãy điền tên sản phẩm');
            noError = false;
        } else if (pro_name.length > 100) {
            showError('pro_name', 'Tên sản phẩm không quá');
            noError = false;
        } else {
            hideError('pro_name');
        }

        if (!pro_price) {
            showError('pro_price', 'Hãy điền giá sản phẩm');
            noError = false;
        }
        if (pro_price.trim() === '' || isNaN(parseInt(pro_price))) {
            showError('pro_price','Vui lòng nhập giá sản phẩm là một số nguyên.');
            noError = false;
        } else {
            hideError('pro_price');
        }


        // Kiểm tra các input file
        var validExtensions = ['png', 'jpg', 'jpeg'];

        $('.imgFile').each(function () {
            var inputFile = $(this)[0].files[0];
            var errorParagraph = $(this).next('p');

            // Kiểm tra nếu không có file được chọn
            if (!inputFile) {
                errorParagraph.text('Vui lòng chọn file hình ảnh.');
                noError = false;
            } else {
                var fileName = inputFile.name.toLowerCase();
                var fileExtension = fileName.split('.').pop();

                // Kiểm tra đuôi file
                if (!validExtensions.includes(fileExtension)) {
                    errorParagraph.text('Đuôi file hình ảnh phải là .png, .jpg hoặc .jpeg.');
                    noError = false;
                } else {
                    errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
                }
            }
        });

        $('.feature').each(function () {
            var feature = $(this).val();
            var errorParagraph = $(this).next('p');

            // Kiểm tra nếu không có file được chọn
            if (!feature) {
                errorParagraph.text('Vui lòng nhập thông tin kỹ thuật.');
                noError = false;
            } else if (feature.length > 100) {
                errorParagraph.text('Quá số lượng.');
                noError = false;
            } else {
                errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
            }
        });
        $('.description').each(function () {
            var description = $(this).val();
            var errorParagraph = $(this).next('p');

            // Kiểm tra nếu không có file được chọn
            if (!description) {
                errorParagraph.text('Vui lòng nhập mô tả kỹ thuật.');
                noError = false;
            } else if (description.length > 100) {
                errorParagraph.text('Quá số lượng.');
                noError = false;
            } else {
                errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
            }
        });

        var pro_des = getValueById('pro_des');
        if (!pro_des) {
            showError('pro_des', 'Hãy điền mô tả sản phẩm');
            noError = false;
        } else if (pro_des.length > 500) {
            showError('pro_des', 'Mô tả sản phẩm không quá');
            noError = false;
        } else {
            hideError('pro_des');
        }

        // Nếu các kiểm tra đều thành công, tiếp tục submit form
        if (!noError) {
            event.preventDefault();
            // Để form tiếp tục submit, không cần event.preventDefault();
        }
    });
});

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