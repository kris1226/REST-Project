'use strict';

describe('ScriptingAppApp', () => {
  let React = require('react/addons');
  let ScriptingAppApp, component;

  beforeEach(() => {
    let container = document.createElement('div');
    container.id = 'content';
    document.body.appendChild(container);

    ScriptingAppApp = require('components/ScriptingAppApp.js');
    component = React.createElement(ScriptingAppApp);
  });

  it('should create a new instance of ScriptingAppApp', () => {
    expect(component).toBeDefined();
  });
});
