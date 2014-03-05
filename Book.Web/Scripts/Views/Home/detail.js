
app.controller('DetailController', function ($scope, $modal, $log, Restangular) {

    var bookApi = Restangular.all('book');
    var bookpropApi = Restangular.all('bookproperty');

    $scope.loadBook = function (id, func) {
        $scope.details = {
            id: id
        };

        bookApi.getList($scope.details).then(function (obj) {
            $scope.details = Restangular.copy(obj);

            if (func) {
                func();
            }
        });
    };

    $scope.updateReadCount = function (id) {
        $scope.details.readCount += 1;
        bookApi.post($scope.details);
    };

    $scope.updateVoteCount = function (id, value) {
        var bookprop = {};
        bookprop.bookId = id;

        bookpropApi.getList(bookprop).then(function (obj) {

            bookprop = Restangular.copy(obj);
            bookprop.rating = value;

            bookpropApi.post(bookprop);
        });
    };

    $scope.$watch('details.rating', function (value) {
        //updateVoteCount(id, value);
    });

    var url = document.URL.split('/');
    var id = url.pop();

    $scope.loadBook(id, function () {
        $scope.updateReadCount(id);
    });
});