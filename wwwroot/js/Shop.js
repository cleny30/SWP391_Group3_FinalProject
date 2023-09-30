﻿//$(document).ready(function () {

//});

$('input[type="checkbox"]').change(function () {
    filterProducts();
});


function RemoveUsedFilter(element, index) {
    // Assuming your checkboxes have unique IDs like "checkbox1", "checkbox2", etc.
    var cateCheckbox = element.getAttribute("data-cateFilter");
    var BrandCheckbox = element.getAttribute("data-brandFilter");
    element.parentNode.removeChild(element);
    if (cateCheckbox) {
        document.getElementById("cat-" + cateCheckbox).checked = false;
    }
    if (BrandCheckbox) {
        document.getElementById("brand-" + BrandCheckbox).checked = false;
    }

    filterProducts();
    // Add any other desired functionality for removing the used filter here
}


function filterProducts() {
    var selectedCategories = $('input[id^=cat]:checked').map(function () {
        return this.value;
    }).get();

    var selectedBrands = $('input[id^="brand"]:checked').map(function () {
        return this.value;
    }).get();

    // Prepare the URL with selected categories and brands
    var url = '/Product/Shop';

    if (selectedCategories.length > 0) {
        url += '?category=' + selectedCategories.join(',');
    }

    if (selectedBrands.length > 0) {
        url += (url.includes("?") ? "&" : "?") + 'brand=' + selectedBrands.join(',');
    }

    window.location.href = url;
}