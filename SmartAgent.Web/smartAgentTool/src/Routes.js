'use strict';

import React, { PropTypes, Component } from'react';
import { Router, Route } from 'react-router';

var App = require('./components/App.jsx');


export default class Routes extends Component {
  debugger;
    render() {
        return (
            <Router>
                <Route name='app' path='/' component={App}>
                  <Route path="users" component={App}>
                    <Route path="/user/:userId" component={App}/>
                    </Route>
                    <Route path="*" component={App}/>
                    </Route>
            </Router>
        );
    }
}
