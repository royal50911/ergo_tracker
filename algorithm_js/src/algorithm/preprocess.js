var angle = require('../angle.js');
var util = require('./util.js');
var svm = require('../svm.js');


/*
@parameters Array(point) from the data posted, 
@return Array(Number)
 */
module.exports = exports = function(arr){
    return arr.map(function(a){
        var v = 0;
        if(angle.isHeadPitch(a)){
            v = util.addHeadPitch(v);
        }
        if(angle.isBodyTwist(a)){
            v = util.addBodyTwist(v);
        }
        if(angle.isLean(a)){
            v = util.addLean(v);
        }        
        if(!svm.isSit(a)){
            v = util.addStand(v);
        }        
        return v;
    });
};
