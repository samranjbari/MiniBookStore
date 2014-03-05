

app.controller('CategoriesController', function ($scope, Restangular) {
    var bookApi = Restangular.all('book');

    $scope.loadBook = function () {
        var url = document.URL.split('/');
        var category = url.pop();

        $scope.categoryId = category;
        $scope.books = [];

        bookApi.getList($scope).then(function (list) {
            _.each(list, function (obj) {
                var b = Restangular.copy(obj);

                $scope.books.push(b);
            });
        });
    };

    $scope.$watch('details.rating', function (value) {
        //$log.info(value);
    });

    $scope.loadBook();
});