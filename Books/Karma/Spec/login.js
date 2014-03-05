'use strict';

describe('LogonController', function () {

    beforeEach(module('logonApp'));
    

	var mainCtrl,
		scope;

	beforeEach(inject(function($controller, $rootScope){
		scope = $rootScope.$new();
		mainCtrl = $controller('LogonController', {
			$scope: scope
		})
	}));

    it('should count the list in array', function () {
    	scope.username= 'same';
    	scope.password= '123';
        expect(scope.login()).toBe(4);
    });
});