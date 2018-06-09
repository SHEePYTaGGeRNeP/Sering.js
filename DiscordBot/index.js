var http = require('http');


const Discord = require('discord.js');
const client = new Discord.Client();

const beerMessage = 'bier halen';

client.on('ready', () => {
  console.log(`Logged in as ${client.user.tag}!`);
});

client.on('message', msg => {
  const messageSuffix = ' ga bier halen';
  if (msg.content.endsWith(messageSuffix)){
    return;
  }
  if (msg.content === 'ping') {
    msg.reply('pong');
    return;
  }
  msg.channel.sendMessage(msg.channel.members.find(x => !x.username.endsWith('bot')).random(1).toString() + messageSuffix);

});

client.login('NDU0OTc2NDAzNzQyNjU0NDY0.Df1Rzg.Qt_NgGveJkMG1IaIvJulGSri3BM');

var sys = require('sys');
var http = require('http');
var router = require('./router');

// Handle your routes here, put static pages in ./public and they will server
router.register('/', function(req, res) {
  res.writeHead(200, {'Content-Type': 'text/plain'});
  res.write('Hello World');  
  res.end();
});


// Handle your routes here, put static pages in ./public and they will server
router.register('/bier', function(req, res) {
  res.writeHead(200, {'Content-Type': 'text/plain'});
  res.write('Bier halen');

  const hook = new Discord.WebhookClient('454993125627265045', 'KaxD3XHhRqvsGWfKG866j5odrMtnEb_txbiYDRyYELV16iBPssqh59siQyakfZtUGk5L');
  // Send a message using the 
  var channel = client.channels.first(1);   
  res.write('channel: ' + channel);
  hook.send(beerMessage);
  res.end();
});

// We need a server which relies on our router
var server = http.createServer(function (req, res) {
  handler = router.route(req);
  handler.process(req, res);
});

// Start it up
server.listen(8080);
sys.puts('Server running');


// http.createServer(function (req, res) {
//   res.writeHead(200, {'Content-Type': 'text/html'});
//   res.end('Hellofe!' + req + ' ed' + res);
//   client.channels.first().send('hello');
//   console.log(req);
//   console.log(res);
//   // res.end(req);
//   // if (req == "bier")
//   // {
//   //   res.writeHead(200, {'Content-Type': 'text/html'});
//   //   //res.end('Bier!');
//   // }
//   // else{
//   // }
// }).listen(8080);