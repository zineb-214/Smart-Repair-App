$(window).on('load', function () {
    $('#dataTable').DataTable({
        // Options explicites
        paging: true,  // Force la pagination
        responsive: true
    });
});