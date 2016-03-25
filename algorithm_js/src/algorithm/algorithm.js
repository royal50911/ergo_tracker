var util = require('./util.js'),
    preprocess = require('./preprocess.js'),
    Q = require('q');

var series = require('./series.js'),
    score = require('./score.js'),
    config = require('./config.js');


var algorithm = function(points, cb) {
    cb = cb || util.noop;

    return Q.fcall(function() {
            return preprocess(points);
        })
        .then(series)
        .then(score)
        .then(function(r){            
             cb(null, 100 - r.score > 0 ? 100-r.score : 0,r.break);
            return r;
        })
        .fail(function(err) {
            cb(err, null);
            throw err;
        });
};


algorithm.FrameRate = function(num){
    config.pointsPerSecond = num;
};


module.exports = exports = algorithm;
