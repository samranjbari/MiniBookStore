/// <reference path="../Shared/enums.js" />


var app = angular.module('logonApp', ['restangular', 'ui.bootstrap'])
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
    });

    app.controller('LogonController', function ($rootScope, $scope, Restangular, $location, notifyService) {
        var apiLogin = Restangular.all('logon');

        $scope.logon = { username: '', password: '', rememberMe: false };
        $scope.logon.resetPassword = false;
        $scope.logon.email = '';

        $rootScope.closeAlert = notifyService.closeAlert;

        var url = document.URL.split('/');
        var registered = url.pop();
        if (registered == "registered") {
            notifyService.notify('success', ['Your account is successfully created. Please login']);
        }

        $scope.login = function () {
            var api = Restangular.all('auth/credentials');

            api.post($scope.logon).then(function (resp) {
                if (resp.sessionId == null) {
                    notifyService.clear();
                    notifyService.notify('error', ['Invalid username and/or password. Please try again.']);
                }
                else {
                    apiLogin.getList($scope.logon).then(function () {
                        window.location = '/Home';
                    });
                }
            });
        }

        $scope.resetPassword = function () {
            apiLogin.post($scope.logon).then(function (resp) {
                notifyService.clear();
                notifyService.notify(resp.responseResult.resultStatus.toLowerCase(), resp.responseResult.messages);
            });
        }
    });