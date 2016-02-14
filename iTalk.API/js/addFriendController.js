iTalkApp.controller('addFriendController', ['$scope', '$http', '$mdDialog', function ($scope, $http, $mdDialog) {
    $scope.user = emptyUser();

    $scope.searchUser = function () {
        if (!$scope.user.input) {
            $scope.user = emptyUser();
            return;
        }

        if ($scope.user.input.toUpperCase() === $scope.me.userName.toUpperCase()) {
            $scope.user.status = 1;
            $scope.user.found = $scope.me;
            return;
        }

        $http.get('/account?userName=' + $scope.user.input)
            .then(function (response) {
                $scope.user.status = 1;
                $scope.user.found = response.data.result;
            }, function () {
                $scope.user.status = 0;
                $scope.user.found = null;
            });
    };

    $scope.addFriend = function () {
        $http.post('/friend', {
            id: $scope.user.found.id
        })
        .then(function (response) {
            if (response.data.success) {
                //$scope.friends.push($scope.user.found);
                //$scope.users.push($scope.user.found);
                //$scope.chats[$scope.user.found.id.toString()] = [];

                alert('已經將用戶' + $scope.user.found.alias + '加為好友');
                $scope.endAddFriend();
            }
            else {
                $scope.showError(response.data);
            }
        }, function (response) {
            $scope.showError(response.data);
        });
    };

    $scope.endAddFriend = function () {
        $scope.user.input = '';
        $scope.user.status = -1;
    }

    function emptyUser() {
        return {
            name: '',
            status: -1,
            found: {}
        };
    }
}]);
