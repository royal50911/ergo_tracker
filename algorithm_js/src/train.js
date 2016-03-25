var path = require('path');
var config = require('./config.js');
var jsonfile = require('jsonfile');
var mode = process.env.DATA || config.mode || 'correctness'; // correctness, all, develop

var train_file = path.join(config.data, mode + '_train_set.csv');
var test_file = path.join(config.data, mode + '_test_set.csv');

var train_transformed_file = path.join(config.data, mode + '_train_set.json');
var test_transformed_file = path.join(config.data, mode + '_test_set.json');


var train_data = jsonfile.readFileSync(train_transformed_file);
var test_data = jsonfile.readFileSync(test_transformed_file);

var model_file = path.join(config.data, mode + '_model.json');

var report_file = path.join(config.data, mode + '_report.json');

var svm = require('node-svm');
var olf = new svm.OneClassSVM();

olf.train(train_data)
    .done(function(model,report) {
        console.log("Training Finished");
        jsonfile.writeFileSync(model_file, model);
        jsonfile.writeFileSync(report_file, report);        
    });
