
var add = function(x, y){
    return x | y;
};


var sub = function(x,y){
    return x &~ y;
};

var has = function(x,y){
    return (x & y) > 0;
};





module.exports = {
    add:add,
    sub:sub,
    has:has
};



