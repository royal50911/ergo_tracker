var path = require('path');

var c  = {
    constant:{
        head_pitch:2,
        body_twist:4,
        lean:8,
        sit:0,
        stand:1
    },
    data:path.join(__dirname,'../Data/'),
    sit:0,
    stand:1,
    mode: 'correctness'// 'correctness'
};

module.exports = c;
