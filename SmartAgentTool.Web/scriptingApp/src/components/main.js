'use strict';

var ScriptingAppApp = require('./ScriptingAppApp');
var React = require('react');
var Router = require('react-router');
var Route = Router.Route;

var content = document.getElementById('content');

var Routes = (
  <Route handler={ScriptingAppApp}>
    <Route name="/" handler={ScriptingAppApp}/>
  </Route>
);

Router.run(Routes, function (Handler) {
  React.render(<Handler/>, content);
});
