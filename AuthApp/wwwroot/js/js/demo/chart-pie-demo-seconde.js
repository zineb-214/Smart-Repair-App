function initializePieChart(labels, data) {
    var ctx = document.getElementById("mysecondpiechart");

    var mysecondpiechart = new Chart(ctx, {
        type: 'doughnut', // Type de graphique (en anneau)
        data: {
            labels: labels, // Catégories
            datasets: [{
                data: data, 
                backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#f6c23e', '#e74a3b', '#858796'], // Couleurs des segments
                hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', '#d39e00', '#c0392b', '#6e707e'],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: true // Afficher la légende
            },
            cutout: '80%', // Réduire la taille du cercle central
        },
    });
}
