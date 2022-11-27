//leftNav Effect
$('.fa-chevron-left').click(function (e) {
    e.preventDefault();
    $('#divf').attr('style', 'animation: ex1 1s normal both;');
    $('#divs').attr('style', 'animation: ex3 1s normal both;');
    $('.fa-chevron-left').parent().addClass('d-none');
    $('.fa-chevron-right').parent().removeClass('d-none');
    $('#divf a').attr('style', 'animation: op1 1ms normal both;');

});


$('.fa-chevron-right').click(function (e) {
    e.preventDefault();
    $('#divf').attr('style', 'animation: ex2 1s normal both;');
    $('.fa-chevron-right').parent().addClass('d-none');
    $('.fa-chevron-left').parent().removeClass('d-none');
    $('#divs').attr('style', 'animation: ex4 1s normal both;');
    $('#divf a').attr('style', 'animation: op2 2s normal both;');
});

//leftNav Effect


//chart bar
var ctx1 = document.getElementById("myPie");
var myChart = new Chart(ctx1, {
    type: 'doughnut',
    data: {
        labels: [
            'a1', 'a2', 'a3', 'a4'
        ],
        datasets: [{
            data: [300, 50, 100, 200],
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(54, 162, 235)',
                'rgb(255, 205, 86)',
                'green'
            ],
            hoverOffset: 2
        }]
    },
    options:{
        title:{
            display:true,
            text:'Yorum',
            fontSize:25,
            fontColor:'black'
        },
        responsive:false
    }
});

var ctx2 = document.getElementById("myPie2");
var myChart = new Chart(ctx2, {
    type: 'doughnut',
    data: {
        labels: [
            'a1', 'a2', 'a3', 'a4'
        ],
        datasets: [{
            data: [300, 50, 100, 200],
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(54, 162, 235)',
                'rgb(255, 205, 86)',
                'green'
            ],
            hoverOffset: 2
        }]
    },
    options:{
        title:{
            display:true,
            text:'Begeni',
            fontSize:25,
            fontColor:'black'
        },
        responsive:false
    }
});

//chart bar


//chart bar
var ctx = document.getElementById("myChart");
var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ["2015-01", "2015-02", "2015-03", "2015-04", "2015-05", "2015-06", "2015-07", "2015-08", "2015-09", "2015-10"],
        datasets: [{
            label: '# of Goruntulenme',
            name: 'series1',
            data: [12, 19, 3, 5, 2, 3, 20, 3, 5, 6],
            backgroundColor:
                'green'
            ,
            borderColor:
                'rgba(54, 162, 235, 1)'
            ,
            borderWidth: 1
        }]
    },
    options: {
        legend:
        {
            labels: {
                fontColor: 'black',
                fontSize: 18
            }

        },
        responsive: true,
        scales: {
            xAxes: [{
                ticks: {
                    maxRotation: 90,
                    minRotation: 80,
                    display: true,
                    fontColor: 'black',
                    fontSize: 20
                },
                gridLines: {
                    offsetGridLines: true // à rajouter
                }
            },
            {
                position: "top",
                ticks: {
                    maxRotation: 90,
                    minRotation: 80,
                    display: true,
                    fontColor: 'black',
                    fontSize: 16
                },
                gridLines: {
                    offsetGridLines: true // et matcher pareil ici
                }
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    display: true,
                    fontColor: 'black',
                    fontSize: 20
                }
            }]
        }
    }
});
//chart bar


//DataTable
$(document).ready(function () {
    $('#example').DataTable();
});
//DataTable