    var chart = c3.generate({
    bindto: '#chart',
    data: {
      x: 'x',
      columns: [
         ['x', '2015-11-25', '2015-11-26', '2015-11-27', '2015-11-28', '2015-11-29',
          '2015-11-30', '2015-12-01', '2015-12-02', '2015-12-03','2015-12-04'
          ,'2015-12-05','2015-12-06','2015-12-07'],
        ['data1', 40, 85, 25, 65, 89, 46, 58, 74, 90, 37, 49, 81, 69],

        ['data2', 40, 85, 25, 65, 89, 46, 58, 74, 90, 37, 49, 81, 69]
      ],

     
      names: {
            data1: 'Score',
            data2: 'Score'
        },

      axes: {
        data2: 'y1'
      },
      types: {
        data2: 'bar' // ADD

      },
      color: function (color, d) {
            console.log(d.value);
            if(d.value>=85)
              return "#3ADF00";
            if(d.value<85&&d.value>=65)
              return "#FFFF00";
            if(d.value<65)
              return "#FF0000";
        }

    },

    legend: {
        hide: 'data2'
    },
    tooltip: {
        grouped: false // Default true
    },
    axis: {
      y: {
        label: {
          text: 'Score',
          position: 'outer-middle'
        }
      },
      x: {
            type: 'timeseries',
            tick: {
                format: '%Y-%m-%d'
            }
        },

      y2: {
        show: false,
        label: {
          //text: 'Y2 Label',
          position: 'outer-middle'
        }
      }
    }
});

