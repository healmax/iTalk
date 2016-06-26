angular.module('iTalkApp')
    .controller('groupController', ['$scope', '$http', '$mdDialog', function ($scope, $http, $mdDialog) {
        //$scope.showCreateGroupDialog = function () {
        //    $mdSidenav('left').toggle();
        //    $mdSidenav('right').toggle();
        //}

        $scope.showCreateGroupDialog = function (ev) {
            $mdDialog.show({
                controller: dialogController,
                templateUrl: 'createGroupView',
                //parent: angular.element('body > div .container'),
                targetEvent: ev,
                //locals: {
                //    friends: $scope.friends
                //},
                fullscreen: true,
                scope: $scope,
                preserveScope: true
            })
            .then(function (group) {

            });
        }
    }]);