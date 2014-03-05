
var app = angular.module('app', ['restangular', 'common', 'ui.bootstrap', 'angularFileUpload'])
    .factory('notifyService', function ($rootScope) {
        var notifyService = {};
        $rootScope.alerts = [];

        notifyService.clear = function () {
            $rootScope.alerts = [];
        };

        notifyService.notify = function (type, msg) {
            $rootScope.alerts.push({ 'type': type, 'msgs': msg });
        };

        notifyService.closeAlert = function (index) {
            $rootScope.alerts.splice(index, 1);
        };

        return notifyService;
    })
    .config(function (RestangularProvider) {
        RestangularProvider.setBaseUrl('/api');
        RestangularProvider.setRequestInterceptor(function (elem, operation) {
            if (operation === "remove") {
                return undefined;
            }
            return elem;
        });
    });

    app.controller('LayoutController', function ($rootScope, $scope, Restangular, $log) {       
        var api = Restangular.all('category');
        var apiauthLogout = Restangular.all('auth/logout');
        var apiLogout = Restangular.all('Logon');

        $rootScope.global = {
            showMyLinks: false
        };

        $scope.categories = [];
        api.getList().then(function (list) {
            _.each(list, function (li) {
                $scope.categories.push(li);
            });
        });

        apiLogout.post({ getInfo: true }).then(function (isAuthenticated) {
            $rootScope.global.isAuthenticated = isAuthenticated;
        });

        $scope.logout = function () {
            apiauthLogout.post().then(function () {
                //apiLogout.post({ logOut: true }).then(function () {
                window.location = "/";
                //});
            });
        }
    });