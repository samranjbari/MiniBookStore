/// <reference path="/Scripts/Views/Shared/enums.js" />


app.controller('RegisterController', function ($rootScope, $scope, Restangular, notifyService) {
    var api = Restangular.all('account');

    $rootScope.closeAlert = notifyService.closeAlert;

    $scope.account = { username: '', email: '', firstName: '', lastName: '', password: '', acceptAgreement: false, isMember: false };

    $scope.register = function () {
        notifyService.clear();
        api.post($scope.account).then(function (resp) {
            if (resp.responseResult.resultStatus !== ResponseStatuses.Success) {
                notifyService.notify(resp.responseResult.resultStatus.toLowerCase(), resp.responseResult.messages);
            }
            else {
                window.location = '/account/Logon/registered';
            }
        });
    }
});