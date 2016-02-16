iTalkApp.controller('groupController', ['$scope', '$http', '$mdDialog', function ($scope, $http, $mdDialog) {
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

    function dialogController($scope, $mdDialog) {
        //$scope.friends = friends;

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.create = function (group) {
            $scope.isLoading = true;

            var data = new FormData();
            data.append('name', group.name);
            data.append('members', group.members.map(function (m) {
                return m.id;
            }));
            data.append('portrait', angular.element('input[name=portrait]')[0].files[0]);

            $http.post('/group', data, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function () {
                //$http.get('/group')
                //    .then(function (response) {
                //        $scope.groups.length = 0;
                //        angular.forEach(response.data.result, function (g) {
                //            if (!$scope.chats[g.id.toString()]) {
                //                $scope.chats[g.id.toString()] = [];
                //            }
                //            $scope.groups.push(g);
                //        });
                $mdDialog.hide(group);
                $scope.isLoading = false;
                //}, function (response) {
                //    $scope.showError(response.data);
                //    $scope.isLoading = false;
                //});
            }, function (response) {
                $scope.showError(response.data);
                $scope.isLoading = false;
            })
        };

        $scope.isChecked = function (friend) {
            return $scope.newGroup.members.indexOf(friend) > -1;
        }

        $scope.selectFriend = function (friend) {
            if ($scope.isLoading) return;
            var index = $scope.newGroup.members.indexOf(friend);
            if (index > -1) {
                $scope.newGroup.members.splice(index, 1);
            }
            else {
                $scope.newGroup.members.push(friend);
            }
        }

        //$scope.querySearch = function (query) {
        //    var results = query ?
        //        $scope.friends.filter(createFilterFor(query)) : [];
        //    return results;
        //}

        //function createFilterFor(query) {
        //    var lowercaseQuery = angular.lowercase(query);
        //    return function filterFn(friend) {
        //        return (angular.lowercase(friend.alias).indexOf(lowercaseQuery) != -1);
        //    };
        //}
    }
}]);