/// <reference path="/Scripts/Views/Shared/enums.js" />

app.controller('FindBookController', function ($rootScope, $scope, Restangular) {
    var api = Restangular.all('searchbook');
    $scope.books = [];
    $scope.search = {
        author: '',
        popular: ''
    };

    $scope.search = function () {
        api.getList($scope.search).then(function (li) {
            $scope.books = Restangular.copy(li);
        });
    }
});