
app.controller('HomeController', function ($rootScope, $scope, $modal, $log, Restangular) {

    var apiLogout = Restangular.all('Logon');
    apiLogout.post({ getInfo: true }).then(function (isAuthenticated) {
        if (isAuthenticated == "false") {
            setTimeout(function () {
                showNotification({
                    message: "Join the other 20,000 readers to learn about free daily ebooks. &nbsp;&nbsp;&nbsp;<a class='btn btn-danger' href='/account/register'>Join Now!</a> &nbsp;&nbsp;&nbsp;or <a class='' href='/account/logon'>login</a>",
                    type: "warning"
                });
            }, 1000);


            if ($.cookie('cocainebooks_visits') == null) {
                $.cookie('cocainebooks_visits', 0);
            }
            var c = $.cookie('cocainebooks_visits');
            c = parseInt(c) + 1;
            $.cookie('cocainebooks_visits', c);
            if (c <= 3) {
                Drawium.circle('addbooktxt', { delay: 1600, duration: 15000 });
            }
        }
    });

    $scope.categories = [];
    $scope.features = [];

    var api = Restangular.all('categorybook');
    var apiFeatures = Restangular.all('featureBook');

    apiFeatures.getList().then(function (list) {
        _.each(list, function (li) {
            var item = {};
            item.id = li.id;
            item.coverUrl = li.coverUrl;
            item = Restangular.copy(li);
            $scope.features.push(item);
        });
    });

    api.getList().then(function (list) {
        _.each(list, function (li) {
            var item = {};
            item.id = li.id;
            item.name = li.name;
            item.books = li.categoryBooks;

            $scope.categories.push(item);
        });
    });

    $scope.details = {
        id: 0
    };

    $scope.openDetailWindow = function (id) {
        window.location = '/home/detail/' + id;
    };

    $scope.open = function (id) {
        $scope.details = {
            id: id
        };

        var bookApi = Restangular.all('book');
        bookApi.getList($scope.details).then(function (obj) {
            $scope.details.amazonUrl = obj.amazonUrl;
            $scope.details.title = obj.title;
            $scope.details.price = obj.price;
            $scope.details.coverUrl = obj.coverUrl;
            $scope.details.description = obj.description;
            $scope.details.author = obj.author;
            $scope.details.gumroadUrl = obj.gumroadUrl;
            $scope.details.rating = 3.5;

            openDetails();
        });
    }

    function openDetails() {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/BookDetailsModal.htm',
            controller: ModalInstanceCtrl,
            windowClass: 'modal-window',
            resolve: {
                items: function () {
                    return $scope.details;
                }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        });
    }

    $scope.$watch('details.rating', function (value) {
        //$log.info(value);
    });
});

var ModalInstanceCtrl = function ($scope, $modalInstance, items) {
    
    $scope.items = items;
    $scope.selected = {
        item: $scope.items[0]
    };

    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};