$(document).on("focus", ".select-all", function () {
    $(this).select();
});

$(document).on("click", ".select-all", function () {
    $(this).select();
});
$(document).on("focus", "input.form-control", function () {
    this.select();
});

$(document).ready(function () {

    $(".datatable").DataTable({
        pageLength: 10,
        ordering: true,
        searching: true,
        lengthChange: true
    });

});