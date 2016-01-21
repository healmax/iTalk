iTalkApp.controller('chatController', ['$scope', '$http', function ($scope, $http) {
    /**
     @id friend or gropu id
    */
    $scope.sendDialog = function () {
        var chat = {
            targetId: $scope.current.id,
            content: $scope.current.input,
            date: new Date()
        };

        $http.post('/' + $scope.getControllerName() + '/dialog', chat)
            .then(function (response) {
                //$scope.chats[$scope.current.id].push({
                //    content: chat.content,
                //    date: chat.date,
                //    senderId: $scope.me.id
                //});
                $scope.current.input = '';
            }, function (response) {
                $scope.showError(response.data);
            });
    };
}]);