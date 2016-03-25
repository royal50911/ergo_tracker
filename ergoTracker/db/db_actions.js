var ip = '52.89.152.186';
var url = 'mongodb://' + ip + ':27017/ErgoDB';
var partPoints = 'PartPoints';
var avg_score = 'AvgScore';

var mongoose = require('mongoose');

mongoose.connect(url);
var PartPoints = mongoose.model('PartPoints', {
    userId: String,
    date: Date,
    score: Number,
    points: mongoose.Schema.Types.Mixed
});

var AvgScore = mongoose.model('AvgScore', {
    userId: String,
    avg: Number,
    date: Date,
});

function insertAvgScore(userId, avg_score, cb){
    var p = new AvgScore({
            "userId": userId,
            "avg": avg_score,
            "date": new Date().getTime()
        });
    p.save(function(err){
        if(err)
            cb(err);
        else
            cb(null);
    });
}

function insertPoints(data, score, cb) {
    var p = new PartPoints({
        "userId": data.userId,
        "points": data.points,
        "score": score,
        "date": new Date().getTime()
    });
    p.save(function(err) {
        if (err)
            cb(err);
        else
            cb(null);
    });
}

function findByDate(from, to,userId, cb) {
    AvgScore.find( //query today up to tonight
        {
            "userId":userId,
            "date": {
                "$gte": from,
                "$lt": to
            }
        }, function(err, results){
            if(err)
                cb(err);
            else
                cb(null, results);
        });
}



exports.insertPoints = insertPoints;
exports.findByDate = findByDate;
