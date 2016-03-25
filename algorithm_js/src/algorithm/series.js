var config = require('./config.js'),
    util = require('./util.js');



// stat.stand  bool
// stat.headPitch  number
// stat.bodyTwist  number
function series(gestures) {
    var FrameRate = config.pointsPerSecond;
    var r = [];
 
    for(var i = 0;i<gestures.length;i++){
        var v = gestures[i];        
        if(util.hasHeadPitch(v)){
                r.push(config.headPitch);
        }
        if(util.hasBodyTwist(v)){
                r.push(config.bodyTwist);
        }
        if(util.isStand(v)){
                r.push(config.stand);
        }
        if(util.isLean(v)){
            r.push(config.lean);
        }
    }
    return r;    
};
module.exports = exports = series;
