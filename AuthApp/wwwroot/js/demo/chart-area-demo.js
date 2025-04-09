document.addEventListener("DOMContentLoaded", function () {
    try {
        console.log("DOM charg� !");
        var ctx = document.getElementById('magasinsParDateChart');

        // V�rifier si l'�l�ment existe
        if (!ctx) {
            console.error("L'�l�ment #magasinsParDateChart n'existe pas.");
            return;
        }

        console.log("Canvas d�tect� ?", ctx);
        ctx = ctx.getContext('2d');

        // V�rification des donn�es
        if (!window.magasinsParDateData || !window.magasinsParDateData.dates || !window.magasinsParDateData.counts) {
            console.error("Les donn�es ne sont pas d�finies correctement.");
            return;
        }

        var dates = window.magasinsParDateData.dates;
        var counts = window.magasinsParDateData.counts;

        console.log("Donn�es r�cup�r�es :", { dates, counts });

        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dates,
                datasets: [{
                    label: "Nombre de magasins par date",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgb(31, 31, 32)",
                    pointHoverBorderColor: "rgb(10, 10, 11)",
                    pointHitRadius: 10,
                    pointBorderWidth: 2,
                    fill: true,
                    data: counts
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(0, 0, 0, 0.1)' // Grille horizontale seulement
                        },
                        ticks: {
                            stepSize: 1,
                            precision: 0,
                            callback: function (value) {
                                return Number.isInteger(value) ? value : '';
                            }
                        }
                    },
                    x: {
                        grid: {
                            display: false // Suppression des lignes verticales
                        },
                        ticks: {
                            autoSkip: true,
                            maxTicksLimit: 10 // �vite d'afficher trop de dates
                        }
                    }
                }
            }
        });

        console.log("Graphique cr�� :", myChart);
    } catch (error) {
        console.error("Erreur lors de la cr�ation du graphique :", error);
    }
});
