/*
 Ticky-Ticky Tape Ticker
 (C) 2003 by Alejandro Guerrieri - Magicom
 alejandro@magicom-bcn.net
 
 This Script is Open Source. You can use it in your site at no cost
 as long as you leave this legend intact.
 
 Please send comments, suggestions, improvements, etc. to my e-mail address
  
 If you use it in your site, please drop me a line so I know if my job
 was worth the effort and I´m actually helping someone! :)
*/

var direction = new Array();
var temporary = new Array();
//This do the real job (moves the ticker)
function move_ticker( mess, speed, msg_start, msg_end, fname ) {
  var doc = eval(''+fname+'.scroll');
  var len = doc.size;
  var dir = direction[fname];
  if (dir > 0) {
    if (msg_end >= mess.length) {
      mess = mess.substring(msg_start, mess.length) + mess.substring(0, msg_start);
      msg_start = 0;
      msg_end = len;
    }
  } else {
    if (msg_start <= 0) {
      mess = mess.substring(msg_end, mess.length) + mess.substring(0, msg_end);
      msg_start = mess.length - msg_end;
      msg_end = mess.length;
    }
  }
  doc.value=mess.substring(msg_start, msg_end);
  msg_start+= dir;
  msg_end+= dir;
  window.setTimeout("move_ticker('"+mess+"', "+speed+", "+msg_start+", "+msg_end+", '"+fname+"')", speed);
}

function test() {  //reloads the window if Nav4 resized
    alert('hello');
}

//This inits the ticker and starts the movement. Executed only once at startup time
function init_ticker(fname, mess, speed, dir) {

    var len = eval('' + fname + '.scroll.size');
   
  direction[fname] = dir;
  mess = mess + '       ';
  while (mess.length < len) {
    mess = '' + mess + mess;
}

  window.setTimeout("move_ticker('"+mess+"', "+speed+", 0, "+len+", '"+fname+"')", speed);
}

//This switches the ticker´s state (stop or start)
function switch_ticker(fname) {
  if (direction[fname] != 0) {
    temporary[fname] = direction[fname];
    direction[fname] = 0;
  } else {
    direction[fname] = temporary[fname];
  }
}

//This restarts the movement after a stop
function start_ticker(fname) {
   direction[fname] = temporary[fname];
}

//This stops the ticker
function stop_ticker(fname) {
  temporary[fname] = direction[fname];
  direction[fname] = 0;
}

//This reverts the ticker´s direction
function revert_ticker(fname) {
  temporary[fname] = -temporary[fname];
  direction[fname] = -direction[fname];
}
