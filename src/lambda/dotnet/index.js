'use strict';

var exec = require('child_process').exec;

module.exports.handler = function(event, context, cb) {

  var output = "";
  var err = "";

  dotnet = exec("./dotnet", function(error) => {
    try{
      cb(error, output);
    } catch {
      cb(error, err);
    }
    //context.done(error, 'Process Complete!')
  });

  dotnet.stdin.write("[ {\"foo\": \"bar\", \"baz\": \"boom\"}, {\"foo\": \"bar\", \"baz\": \"boom\"} ]");

  dotnet.stdout.on('data', function (data) => {
    output += data;
    console.log('stdout: ' + data);
  });

  dotnet.stderr.on('data', function (data) => {
    err += data;
    console.error('stderr: ' + data);
  });

  dotnet.on('exit', function (code) => {
    console.log('child process exited with code ' + code);
  });

  //return cb(null, {"id":"12345678-1234-1234-1234-1234567890AB","name":"Default","lanes":[{"id":"12345678-1234-1234-1234-123456789000","name":"Backlog","items":["00000000-1234-1234-1234-123456789000","00000000-1234-1234-1234-123456789001","00000000-1234-1234-1234-123456789002"]},{"id":"12345678-1234-1234-1234-123456789001","name":"Blocked","items":[]},{"id":"12345678-1234-1234-1234-123456789002","name":"In Progress","items":[]},{"id":"12345678-1234-1234-1234-123456789003","name":"Testing","items":[]},{"id":"12345678-1234-1234-1234-123456789004","name":"Done","items":[]},{"id":"12345678-1234-1234-1234-123456789005","name":"Accepted","items":[]}]});
};
