var express = require('express');
var router = express.Router();
var db = require('../db/db_actions.js');

var bodyParser = require('body-parser');
// parse application/x-www-form-urlencoded 
router.use(bodyParser.urlencoded({
    extended: false
}));

// parse application/json 
router.use(bodyParser.json());


/* GET diagnostic data. */
router.get('/get/diagnostic_data', function(req, res, next) {
    var fromDate = new Date(req.query.fromDate);
    var toDate = new Date(req.query.toDate);
    var userId = req.query.userId;;
    // var date = new Date();
    // var prevd = new Date();
    //assert for the data types
    if (typeof(fromDate) == 'undefined' || typeof(toDate) == 'undefined' || typeof(userId) == 'undefined') {
        res.status(500);
        res.send("One of [fromDate, toDate, userId undefined");
        return;
    }
    db.findByDate(fromDate,toDate, userId,function(err, results){
        res.json(results.map(function(x){
            return {
                score:x.score,
                date:x.date
            };
        }));
    });        
});

/* POST kinect data. */

var algorithm = require('2015-cse218-group5-algorithm');
//var cache = require('memory-cache');
//Need to trigger a database insert on cache expiry
//so using node-cache

var node_cache = require('node-cache');
var cache = new node_cache();
cache.on("expired", function(userId, avg_score){
    db.insertPoints(userId, avg_score, function(err){
        if(err)
           console.log("Missed inserting data in cache");
        else
         return;
    });
});
algorithm.FrameRate(5);



router.post('/post/kinect_data', function(req, res, next) {    
    var num, avg_score,result;
    algorithm(req.body.points, function(err, score, isBreak) {
        if (cache.get(req.body.userId)) {
            var t = cache.get(req.body.userId);
            num = t.num;
            avg_score = t.avg_score;
            avg_score = ((avg_score * num++) + score) / num;
        } else {
            avg_score = score;
            cache.set(req.body.userId,{
                num:1,
                avg_score:score
            }, 1000*60);
            
        }
        result = {
            "error": null,
            "score": score,
            "break": isBreak,
            "avg_score": avg_score.toFixed(2) //so far today
        };
        console.log(score,isBreak);
        db.insertPoints(req.body, score , function(err) {
            if (err)
                res.json({
                    "message": "Datebase Error",
                    "code": 1
                });
            else {                
                res.json(result);
            }
        });        
    });        
});


/* GET today's score. */
router.get('/get/today_score', function(req, res, next) {
    var date = new Date()
    var score = Math.round(Math.random() * 100) / 100
    var userId = req.query.userId;
    if (typeof(userId) == "undefined") {
        res.status(500);
        res.send("userId not sent");
        return;
    }
    
    db.findByDate(date,date, userId,function(err, results){
        res.json(results.map(function(x){
            return {
                score:x.score,
                date:x.date
            };
        }));
    });
});
/* GET home page. */
router.get('/', function(req, res, next) {
    res.render('index', {
        title: 'Express'
    });
});
module.exports = router;
