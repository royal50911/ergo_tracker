var algorithm = require('../');

// describe('Basic Test', function() {
//     describe('#algorithm()', function () {
//         it('should be called without exception.', function (done) {

//         });

//         it('should return a score.', function (done) {

//         });
//     });
// });

algorithm.FrameRate(1);

var sliceSize = 100;
var total = 0;


var type = 'correctness';

var points = require('../Data/'+type+'_points_train_set.json');



while (total < points.length) {
    algorithm(points.slice(total, total + sliceSize), function(err, score, hasBreak) {      
        console.log(err,score,hasBreak);
    });
    total += sliceSize;
}
