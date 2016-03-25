var passport = require('passport');
var Account = require('./models/account');
var nodemailer = require('nodemailer');
var contact_email = process.env.contact_email;
var contact_pass = process.env.contact_pass;
module.exports = function (app) {
    
  app.get('/', function (req, res) {
      res.render('home', { user : req.user });
  });

  app.get('/signup', function(req, res) {
      res.render('signup', { });
  });

  app.post('/signup', function(req, res) {
      Account.register(new Account({ 
        username: req.body.username,
        fullname: req.body.fullname

        }), 
        req.body.password,
       function(err, account) {
          if (err) {
            return res.render("signup", {signupinfo: "The username already exists. Try again."});
          }
          passport.authenticate('local')(req, res, function () {
            console.log("got in");
            res.redirect('/loggedin_home');
          });
      });


  });

  app.get('/login', function(req, res) {
      res.render('login', { user : req.user });
  });

  app.post('/login', 
  passport.authenticate('local', { failureRedirect: '/login' }),
  function(req, res) {
    res.redirect('/loggedin_home');
  });

app.get('/info', function(req, res){
  res.render('info', { user: req.user });
});

app.get('/loggedin_home', function(req, res){
  res.render('loggedin_home', { user: req.user });
});

app.get('/profile', function(req, res){
  res.render('profile', { user: req.user });
});



app.post('/profile', function(req, res){
Account.findOne( { username:req.user.username}, function(err,bot) {  
  console.log(bot);
  //console.log(req.body.email);
    bot.username = req.body.email;
   bot.website = req.body.website;
   bot.location = req.body.location;
   bot.fullname = req.body.name;
  bot.save(function(err){
    if (err) {
     console.log(bot.username + " unsaved " + err);
   } else {
     res.redirect('/profile');
     console.log(bot.username + " updated");
   }
  })
});

}); 

    
// app.post('/profile', function(req, res){
//   Account.update({'_id':'564b0c36841e40ac30cf2744'},{
// 'username': req.body.email,
// 'website': req.body.website,
// 'location': req.body.location,
// 'fullname': req.body.name

//   },function(err){
//     console.log(err)
//     debugger
//     if(!err)
//       res.render('profile', { user: req.user });
//   })  
// });

app.get('/contact', function(req, res){
  res.render('contact', { user: req.user });
});

app.post('/contact', function (req, res) {
  var mailOpts, smtpTrans;
  //Setup Nodemailer transport, I chose gmail. Create an application-specific password to avoid problems.
  smtpTrans = nodemailer.createTransport('SMTP', {
      service: 'Gmail',
      auth: {
          user: contact_email,
          pass: contact_pass
      }
  });
  //Mail options
  mailOpts = {
      from: req.body.name + ' &lt;' + req.body.email + '&gt;', //grab form data from the request body object
      to: 'maxubi118@gmail.com',
      replyTo: req.body.email,
      subject: 'MaxUbi Team Support',
      text: req.body.message,
      html: 'From: ' + req.body.name + ' &lt;' + req.body.email + '&gt; <br>' + req.body.message + '<br>'
  };
  smtpTrans.sendMail(mailOpts, function (error, response) {
      //Email not sent
      if (error) {
          console.log(error);
          res.render('contact', { title: 'Your Contact With MaxUbi', msg: 'Error occured, message not sent.', err: true, page: 'contact' })
      }
      //Yay!! Email sent
      else {
        console.log(response);
          res.render('contact', { title: 'Your Contact With MaxUbi', msg: 'Message sent! Thank you.', err: false, page: 'contact' })
      }
  });
});

app.get('/forgot', function(req, res){
  res.render('forgot', { user: req.user });
});

app.post('/forgot', function(req, res){
  res.render('forgot', { user: req.user });
});

  app.get('/logout', function(req, res) {
      req.logout();
      res.redirect('/');
  });
  
};
