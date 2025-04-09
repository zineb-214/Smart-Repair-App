function initializeMagasinChart() {
    // 1. V�rifier que l'�l�ment canvas existe
    const ctx = document.getElementById('myBarChart');
    if (!ctx) {
        console.error("�l�ment #myBarChart introuvable");
        return;
    }

    // 2. V�rifier les donn�es
    if (!window.magasinsParVilleData || !Array.isArray(window.magasinsParVilleData.villes) || !Array.isArray(window.magasinsParVilleData.counts)) {
        console.error("Donn�es invalides ou manquantes");
        return;
    }

    // 3. Configuration compatible Chart.js v3+
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: window.magasinsParVilleData.villes,
            datasets: [{
                label: "Nombre de magasins",
                data: window.magasinsParVilleData.counts,
                backgroundColor: '#4e73df',
                borderColor: '#4e73df',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.dataset.label}: ${Math.round(context.raw)}`;
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxRotation: 45,
                        minRotation: 45
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        color: '#eaecef',
                        drawBorder: false
                    },
                    ticks: {
                        precision: 0,
                        callback: function (value) {
                            return Math.round(value);
                        }
                    }
                }
            }
        }
    });
}

// Initialisation diff�r�e
if (document.readyState === 'complete') {
    initializeMagasinChart();
} else {
    document.addEventListener('DOMContentLoaded', initializeMagasinChart);
}