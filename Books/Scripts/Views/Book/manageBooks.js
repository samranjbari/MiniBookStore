
app.controller('BookController', function ($rootScope, $scope, Restangular, $compile, $upload, notifyService) {
    var apiBook = Restangular.all('book');
    var apiUser = Restangular.all('userSession');
    var apiCategories = Restangular.all('category');
    var apiOneBook = Restangular.one('book');

    $rootScope.global.showMyLinks = true;

    $scope.session = {
        isAdmin: false,
        username: '',
        id: null
    };
    $scope.book = {
        amazonUrl: '',
        title: '',
        price: '0',
        coverUrl: '',
        description: '',
        author: '',
        gumroadUrl: '',
        categoryText: '',
        categoryId: '',
        categories: [],
        insertMode: true
    };
    

    var filebook;
    var filecover;

    $scope.onFileSelect = function ($files) {
        for (var i = 0; i < $files.length; i++) {
            filebook = $files[i];
        }
    }

    $scope.onCoverSelect = function ($files) {
        for (var i = 0; i < $files.length; i++) {
            filecover = $files[i];
        }
        $scope.tmpUpload(true);
    }

    $scope.tmpUpload = function (isTemp) {
        $upload.upload({
            url: '/api/fileUpload',
            file: filecover,
            data: {
                isTemporary: isTemp,
                fileType: 'Covers'
            }
        }).progress(function (evt) {
            //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {
            $scope.book.coverUrl = data;
        });
    }

    $scope.upload = function (theFile, fileType, func) {
        if (theFile === undefined) {
            func(null);
            return;
        }

        $upload.upload({
            url: '/api/fileUpload',
            file: theFile,
            data: { fileType: fileType }
        }).progress(function (evt) {
            //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {
            if (func) {
                func(data);
            }
        });
    }

    $scope.getBooks = function () {
        $scope.myBooks = [];
        apiBook.getList().then(function (list) {
            _.each(list, function (li) {
                if (li.isFeature) {
                    li.featureText = 'Unset Feature';
                }
                else {
                    li.featureText = 'Set Feature';
                }

                $scope.myBooks.push(li);
            });
        });
    }

    $scope.getUserSession = function () {
        apiUser.getList().then(function (resp) {
            $scope.session = Restangular.copy(resp);
        });
    }

    $scope.getBookById = function (id, func) {
        var categories = $scope.book.categories;
        $scope.book = {
            id: id
        };

        apiBook.getList($scope.book).then(function (obj) {
            $scope.book = Restangular.copy(obj);
            $scope.book.categories = categories;
            $scope.book.insertMode = false;

            if (func) {
                func();
            }
        });
    }

    $scope.loadViewLookups = function () {
        $scope.book.categories = [];
        apiCategories.getList().then(function (list) {
            _.each(list, function (li) {
                $scope.book.categories.push(li);
            });
        });
    }

    $scope.fetchBookInfo = function () {
        apiBook.getList($scope.book).then(function (resp) {
            $scope.book.amazonUrl = resp.amazonUrl;
            $scope.book.title = resp.title;
            $scope.book.price = resp.price;
            $scope.book.coverUrl = resp.coverUrl;
            $scope.book.description = resp.description;

            $scope.collapseDetails();
        }, function () {
            console.log('error');
        });
    }

    $scope.saveBook = function () {
        $("input[type=submit]").attr("disabled", "disabled");
        $("input[type=submit]").val('saving...');

        var coverPath, contentPath;

        // upload cover
        $scope.upload(filecover, 'Covers', function (cover) {
            coverPath = cover;

            // upload content
            $scope.upload(filebook, 'Books', function (content) {
                contentPath = content;

                // publish the rest of the book
                $scope.book.coverUrl = coverPath || $scope.book.coverUrl;
                $scope.book.bookUrl = contentPath || $scope.book.bookUrl;

                apiBook.post($scope.book).then(function (a) {
                    if (a.responseResult.resultStatus !== ResponseStatuses.Success) {
                        notifyService.notify(a.responseResult.resultStatus.toLowerCase(), a.responseResult.messages);
                    }
                    else {
                        $('#moreContent').slideUp('slow', function () {
                            $scope.getBooks();
                            var lk = $scope.book.categories;
                            $scope.book = null;
                            $scope.book = {
                                categories: lk
                            }

                            $('#moreContent').slideDown('slow');
                        });
                    }

                    $("input[type=submit]").removeAttr("disabled");
                    $("input[type=submit]").val('Add Book');
                    $scope.loadViewLookups();
                });
            });
        });
    }

    $scope.selectProduct = function (id) {
        $("input[type=submit]").val('Update Book');
        $scope.getBookById(id, function () {
            $scope.collapseDetails();
        });
    }

    $scope.destroyProduct = function (id) {
        $scope.book = {};
        $scope.book.id = id;
        apiOneBook.remove($scope.book).then(function () {
            $scope.getBooks();
        });
    }

    $scope.featureProduct = function (id, isFeature) {
        $scope.getBookById(id, function () {
            $scope.book.isFeature = isFeature;
            apiBook.post($scope.book).then(function () {
                var index = $scope.find($scope.myBooks, id);

                $scope.myBooks[index].featureText = isFeature ? "Unset Feature" : "Set Feature";
                $scope.myBooks[index].isFeature = isFeature;
            });
        });
    }

    $scope.collapseDetails = function () {
        if ($('#moreContent').is(':hidden')) {
            $('#moreContent').slideDown('slow');
        }
        else {
            $('#moreContent').slideUp('slow', function () {
                $('#moreContent').slideDown('slow');
            });
        }
    }

    $scope.closeBook = function () {
        if (!$('#moreContent').is(':hidden')) {
            $('#moreContent').slideUp();
        }
    }

    $scope.showOtherCategory = function (e) {
        e.preventDefault();
        $('#morecategoryLink').hide();
        $('#categoytText').show();
    }

    $scope.find = function (collection, _id) {
        var Break = {};
        var index = -1;
        try {
            angular.forEach(collection, function (obj, key) {
                index++;
                if (obj.id == _id) {
                    throw Break;
                };
            });
        }
        catch (e) {
            return index;
        }

        return index;
    };


    // get my books
    $scope.getBooks();
    // load lookups
    $scope.loadViewLookups();
    // load user session
    $scope.getUserSession();
});
