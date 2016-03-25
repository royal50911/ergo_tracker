var path = require('path');
var config = require('./config.js');
var jsonfile = require('jsonfile');
var mode = process.env.DATA || config.mode || 'correctness'; // correctness, all, develop

var model_file = path.join(config.data, mode + '_model.json');

var report_file = path.join(config.data, mode + '_report.json');
var test_transformed_file = path.join(config.data, mode + '_test_set.json');
var svm = require('node-svm');


var model = jsonfile.readFileSync(model_file);
var test_data = jsonfile.readFileSync(test_transformed_file);


var m = svm.restore(model);

var v = m.evaluate(test_data);

debugger;


