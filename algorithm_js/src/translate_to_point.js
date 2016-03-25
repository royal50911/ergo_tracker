var joints = [
    'head',
    'shoulder',
    'shoulder_left',
    'shoulder_right',
    'elbow_left',
    'elbow_right',
    'spine',
    'hip_center',
    'hip_left',
    'hip_right'
];


var types = ['all', 'develop', 'correctness'];
// var types = ['develop'];



types = types.map(function(x){
    return __dirname + '/../Data/' + x;
});

var data = [];




var train_suffix = '_train_set.json';
var test_suffix = '_test_set.json';
var points_suffix = '_points';

types.forEach(function(t) {
    var p_train = t + train_suffix,
        p_test = t + test_suffix,
        p_train_points = t + points_suffix + train_suffix,
        p_test_points = t + points_suffix + test_suffix;
    data.push({
        pathToSave: p_train_points,
        json: require(p_train)
    });
    data.push({
        pathToSave: p_test_points,
        json: require(p_test)
    });
});



var jsonfile = require('jsonfile');
function mkpoint(vec){
    var count = 0;
    var p = {};
    for(var i = 0; i<joints.length;i++){
        count = i*3;
        var rd = p[joints[i]] = {};
        rd.x = vec[count];
        rd.y = vec[count+1];
        rd.z = vec[count+2];
    }
    return p;
}

function translate(rows) {
    return rows
        .map(function(r) {
            var vec = r[0];
            return mkpoint(vec);
        });
};


data.forEach(function(d) {
    jsonfile.writeFileSync(d.pathToSave, translate(d.json));
});
