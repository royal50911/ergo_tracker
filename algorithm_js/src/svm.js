var svm = require('node-svm');
var translate = require('./point_translate.js');
var model_parameter = require('../all_model.json');
var config = require('./config.js');

var c = config.constant;
var model = svm.restore(model_parameter);



function isSit(point){
    return c.sit ==  model.predictSync(translate(point));        
}


module.exports = {
    isSit:isSit
};
