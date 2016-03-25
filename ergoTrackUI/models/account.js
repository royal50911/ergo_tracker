var mongoose = require('mongoose'),
    Schema = mongoose.Schema,
    passportLocalMongoose = require('passport-local-mongoose'),
    bcrypt = require('bcrypt-nodejs');

var Account = new Schema({
	username: { type: String, unique: true, lowercase: true },
    password: String,
    date: Number,
    fullname: { type: String, default: '' },
    gender: { type: String, default: '' },
    location: { type: String, default: '' },
    website: { type: String, default: 'https://gravatar.com/avatar/8591beba149c94290c22a0ddb9c6c0a3?s=200&amp;d=retro' },
    SkeletonData: {
        // {jointname:{ type: String, default: 'Head'}, x:{ type: Number, default: 3.67567}, y:Number, z:Number},
        // {jointname:{ type: String, default: 'ShoulderCenter'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'ShoulderLeft'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'ShoulderRight'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'ElbowLeft'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'ElbowRight'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'Spine'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'HipCenter'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'HipLeft'}, x:Number, y:Number, z:Number},
        // {jointname:{ type: String, default: 'HipRight'}, x:Number, y:Number, z:Number}
        Head:  { type: [Number], default: [0,0,0] },
        ShoulderCenter:  { type: [Number], default: [0,0,0]  },
        ShoulderLeft: { type: [Number], default: [0,0,0]  },
        ShoulderRight: { type: [Number], default: [0,0,0]  },
        ElbowLeft: { type: [Number], default: [0,0,0]  },
        ElbowRight: { type: [Number], default: [0,0,0]  },
        Spine: { type: [Number], default: [0,0,0]  },
        HipCenter:{ type: [Number], default: [0,0,0]  },
        HipLeft: { type: [Number], default: [0,0,0]  },
        HipRight: { type: [Number], default: [0,0,0]  }
    }

  


});

Account.plugin(passportLocalMongoose);

module.exports = mongoose.model('Account', Account);
