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
            locals: {
                friends: $scope.friends
            },
            fullscreen: true
        })
        .then(function (group) {
            var data = {
                name: group.name,
                members: group.members.map(function (m) {
                    return m.id;
                })
            };

            $http.post('/group', data)
            .then(function () {
                $http.get('/group')
                    .then(function (response) {
                        $scope.groups.length = 0;
                        angular.forEach(response.data.result, function (g) {
                            if (!$scope.chats[g.id.toString()]) {
                                $scope.chats[g.id.toString()] = [];
                            }
                            $scope.groups.push(g);
                        });
                    });
            }, function (response) {
                $scope.showError(response.data);
            });
        });
    }

    function dialogController($scope, $mdDialog, friends) {
        $scope.friends = friends;

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.create = function (group) {
            //if (group.name.$error) {
            //    alert(group.name.$error);
            //    return;
            //}

            $mdDialog.hide(group);
        };

        $scope.isChecked = function (friend) {
            return $scope.newGroup.members.indexOf(friend) > -1;
        }

        $scope.selectFriend = function (friend) {
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