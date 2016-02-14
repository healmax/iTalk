iTalkApp.controller('mainController', ['$scope', '$http', '$mdSidenav', '$location', function ($scope, $http, $mdSidenav, $location) {
    $scope.me = null;

    //$scope.init = function (data) {
    //    $scope.me = data;
    //}

    $scope.initUser = function (username) {
        $http.get('/account?userName=' + username)
            .then(function (response) {
                $scope.me = response.data.result;
            })
        //.finally(function () {
        //    loadingComplete();
        //})
    }

    //$scope.setPath = function (path) {
    //    $location.path(path);
    //}

    //$scope.getPath = function () {
    //    return $location.path().substring(1);
    //}

    $scope.setHash = function (hash) {
        $location.hash(hash);
    }

    $scope.getHash = function () {
        return $location.hash();
    }

    //var hash = $scope.getHash();

    //if (path !== "friends" && path !== "groups" && path !== "addFriend") {
    //    $location.path('chats');
    //}
}])