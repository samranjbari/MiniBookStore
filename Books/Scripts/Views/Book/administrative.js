(function () {
    'use strict';

    var controllerId = 'administrativeController';
    angular.module('app').controller(controllerId,
        ['$rootScope', 'common', 'Restangular', administrativeController]);

    function administrativeController($rootScope, common, Restangular) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        // Bindable properties and functions are placed on vm.
        vm.categories = [];
        vm.destroyCategory = destroyCategory;

        activate();

        function activate() {
            common.activateController([getCategories()], controllerId)
                    .then(function () {
                        log('activated attendees view');
                    });

            $rootScope.global.showMyLinks = true;
        }

        function getCategories() {
            var apiCategories = Restangular.all('categorybook');
            apiCategories.getList().then(function (list) {
                _.each(list, function (li) {
                    vm.categories.push(li);
                });
            });
        }

        function destroyCategory(id) {
            var cm = { id: id };

            var apiOneCategory = Restangular.one('categorybook');
            apiOneCategory.remove(cm).then(function () {
                log('Removed Category');
                vm.categories = [];
                getCategories();
            });
        }
    }
})();
