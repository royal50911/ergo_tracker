var config = require('../config.js');
var option = require('../option.js');
var c = config.constant;


module.exports = {
    hasHeadPitch: function(v) {
        return option.has(v, c.head_pitch);
    },
    hasBodyTwist: function(v) {
        return option.has(v, c.body_twist);
    },
    isStand: function(v) {
        return option.has(v, c.stand);
    },
    isLean: function(v) {
        return option.has(v, c.lean);
    },
    addLean: function(v) {
        return option.add(v, c.lean);
    },
    addHeadPitch: function(v) {
        return option.add(v, c.head_pitch);
    },
    addBodyTwist: function(v) {
        return option.add(v, c.body_twist);
    },
    addStand: function(v) {
        return option.add(v, c.stand);
    },
    noop:function(){}
};
