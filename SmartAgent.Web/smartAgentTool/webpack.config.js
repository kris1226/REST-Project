'use strict';

var path = require('path');
var webpack = require('webpack');

module.exports = {
  devtool: 'cheap-module-eval-source-map',
  entry: {
      main: [
        'webpack-hot-middleware/client',
        './index'
      ]
  },
  output: {
    path: path.join(__dirname, 'dist'),
    filename: 'bundle.js',
    publicPath: '/static/'
  },
  plugins: [
    new webpack.optimize.OccurenceOrderPlugin(),
    new webpack.HotModuleReplacementPlugin(),
    new webpack.NoErrorsPlugin()
  ],
  module: {
      loaders: [{
            test: /\.jsx?$/,
            include: __dirname,
            exclude: '/node_modules/',
            loader: 'babel',
            query: {
              presets: ['react', 'es2015']
            }
        },
        {
            test: /\.scss$/,
            include: __dirname,
            loader: 'style!css!sass'
        }]
  }
};
