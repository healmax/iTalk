angular.module('iTalkApp')
    .controller('mainController', ['$scope', '$http', '$mdDialog', '$location', function ($scope, $http, $mdDialog, $location) {
        $scope.me = null;

        //$scope.init = function (data) {
        //    $scope.me = data;
        //}

        $scope.initUser = function (username) {
            $http.get('/account?userName=' + username)
                .then(function (response) {
                    $scope.me = response.data.result;
                }, function (response) {
                    $scope.showError(response.data, "初始化使用者失敗");
                })
            //.finally(function () {
            //    loadingComplete();
            //})
        }

        $scope.showBasicSettingDialog = function (ev) {
            $mdDialog.show({
                controller: dialogController,
                templateUrl: 'basicSetting',
                //parent: angular.element('body > div .container'),
                targetEvent: ev,
                //locals: {
                //    me: $scope.me
                //},
                scope: $scope,
                preserveScope: true,
                fullscreen: true
            })
        }

        function dialogController($scope, $mdDialog) {
            //$scope.me = me;

            $scope.cancel = function () {
                $mdDialog.cancel();
            };
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