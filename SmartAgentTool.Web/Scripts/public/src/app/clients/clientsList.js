(function () {
    'use strict';
    angular.module('clientsList', [])
          .directive('cClientsList', [cClientsList])
          .controller('clientsCtrl', ['clientsRepository', clientsCtrl]);

    function cClientsList() {
        console.log('Directive');
        return {
            scope: {},
            templateUrl: '/Scripts/public/src/app/clients/smartagent_clients.html',
            replace: true,
            controller: 'clientsCtrl',
            controllerAs: 'ctrl'
        };
    }

    function clientsCtrl($scope, clientsRepository) {
        $scope.clients = [];
        $scope.client = 'loading clients...';
        $scope.clientKeys = "loading client keys...";
        $scope.working = true;
        getClients();

        function getClients() {
            clientsRepository.getClients(function (results) {
                $scope.clients = results
            });
        }
        //$scope.clients = clientsServices.getAll();
        return $scope.clients;

        //$http({
        //    method: 'GET',
        //    url: '/api/clients'
        //}).success(function (result) {
        //    $scope.clients = result;
        //    return $scope.clients;
        //});
    }

}());