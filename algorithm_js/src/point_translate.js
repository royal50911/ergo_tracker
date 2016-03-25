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
module.exports = exports = function(point) {
    var r = [];
    joints.forEach(function(f){
        r.push(point[f].x);
        r.push(point[f].y);
        r.push(point[f].z);
    });
    return r;
};
