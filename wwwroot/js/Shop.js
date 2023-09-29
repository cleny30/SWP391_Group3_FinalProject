$(document).ready(function () {
    $('input[type="checkbox"]').change(function () {
        filterProducts();
    });

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
});
