iTalkApp.controller('mainController', ['$scope', '$http', '$mdSidenav', function ($scope, $http, $mdSidenav) {
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
}])