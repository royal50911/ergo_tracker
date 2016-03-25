var path = require("path");
exports.contact = function(req, res){
    res.render('contact', { title: 'Raging Flame Laboratory - Contact', page: 'contact' })
};
exports.index = function(req, res){
  res.render('index', { title: "Passport-Examples"});
};
