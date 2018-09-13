

var chartDataSource = [
    { year: 1950, Africa: 227, Americas: 331,
      Asia: 1436, Europe: 547, Oceania: 12 },
    { year: 1960, Africa: 285, Americas: 416,
      Asia: 1718, Europe: 605, Oceania: 15 },
    { year: 1970, Africa: 365, Americas: 512,
      Asia: 2156, Europe: 657, Oceania: 19 },
    { year: 1980, Africa: 478, Americas: 612,
      Asia: 2644, Europe: 695, Oceania: 22 },
    { year: 1990, Africa: 633, Americas: 720,
      Asia: 3180, Europe: 722, Oceania: 26 },
    { year: 2000, Africa: 810, Americas: 833,
      Asia: 3678, Europe: 731, Oceania: 30 },
    { year: 2010, Africa: 1016, Americas: 936,
      Asia: 4149, Europe: 728, Oceania: 35 }
];

$(function () {
    var chartModel = {
        chartConfiguration: {
            dataSource: chartDataSource,
            commonSeriesSettings: {
                argumentField: 'year',
                type: 'fullStackedArea',
                label: {
                    visible: true,
                    connector: { visible: true }
                }
            },
            series: [
                { name: 'Oceania', valueField: 'Oceania' },
                { name: 'Africa', valueField: 'Africa' },
                { name: 'Americas', valueField: 'Americas' },
                { name: 'Asia', valueField: 'Asia' },
                { name: 'Europe', valueField: 'Europe' }
            ],
            title: 'Continental Population Shift (in millions)',
            legend: {
                horizontalAlignment: 'center',
                verticalAlignment: 'bottom'
            },
            pathModified: true,
            tooltip: {
                enabled: true,
                percentPrecision: 2,
                customizeTooltip: function (value) {
                    return { text: value.percentText };
                }
            }
        }
    };
    
    ko.applyBindings(chartModel, document.getElementById('chartContainer'));
});
