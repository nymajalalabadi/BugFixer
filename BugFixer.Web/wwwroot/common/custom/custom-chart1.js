function loadChart(json) {
    const ctx = document.getElementById('LineChartQuestions').getContext('2d');

    const chartData = [];
    const labels = [];
    const dataArray = JSON.parse(json.replace(/&quot;/g, '"'));

    for (const item of dataArray) {
        chartData.push(item.UseCount);
        labels.push(item.Title);
    }

    const data = {
        labels: labels,
        datasets: [{
            label: 'آمار سوالات بر اساس تگ',
            data: chartData,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            scales: {
                x: {
                    ticks: {
                        font: {
                            size: 14,
                            family: "WYekan"
                        }
                    }
                },
                y: {
                    min: 0,
                    max: Math.max(...chartData) + 1,
                    ticks: {
                        font: {
                            size: 14,
                            family: "WYekan"
                        }
                    }
                }
            },
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 14,
                            family: "WYekan"
                        }
                    }
                },
                tooltip: {
                    titleFont: {
                        size: 12,
                        family: "WYekan"
                    },
                    bodyFont: {
                        size: 12,
                        family: "WYekan"
                    },
                    footerFont: {
                        size: 12,
                        family: "WYekan"
                    }
                }
            }
        }
    };

    const myChart = new Chart(ctx, config);
}