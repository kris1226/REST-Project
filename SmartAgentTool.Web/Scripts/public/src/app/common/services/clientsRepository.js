(function () {
    'use strict';
    angular.module('common.services')
           .factory('clientsRepository', ['$http', clientsServices]);

    function clientsServices($http) {
        return {
            getClients: function (results) {
                $http({
                    method: 'GET',
                    url: '/api/clients'
                }).success(function (results) {
                    return results
                });
            }
        };
        //var getAll = function() {
        //    $http({
        //        method: 'GET',
        //        url: '/api/clients'
        //    }).success(function (result) {
        //        return result;
        //    });
        //};
        //return {
        //    getAll: getAll
        //};
    }

})();