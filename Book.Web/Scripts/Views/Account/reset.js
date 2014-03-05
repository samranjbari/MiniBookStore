/// <reference path="../Shared/enums.js" />


app.controller('ResetController', function ($rootScope, $scope, Restangular, notifyService) {
    var api = Restangular.all('resetAccount');

    $rootScope.closeAlert = notifyService.closeAlert;

    $scope.account = { password: '', confirmPassword: '', hash: '' };

    var url = document.URL.split('?');
    $scope.account.hash = url.pop();

    $scope.reset = function () {
        notifyService.clear();
        api.post($scope.account).then(function (resp) {
            if (resp.responseResult.resultStatus !== ResponseStatuses.Success) {
                notifyService.notify(resp.responseResult.resultStatus.toLowerCase(), resp.responseResult.messages);
            }
            else {
                window.location = '/account/logon';
            }
        });
    }
});