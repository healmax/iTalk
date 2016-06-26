angular.module('iTalkApp')
    .controller('chatController', ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {
        /**
         @id friend or gropu id
        */
        $scope.sendDialog = function () {
            $scope.$root.isLoading = true;
            var id = $scope.current.id;

            var chat = {
                targetId: id,
                content: $scope.current.input,
                date: new Date()
            };

            $http.post('/' + $scope.getControllerName(id) + '/dialog', chat)
                .then(function (response) {
                    //$scope.chats[$scope.current.id].push({
                    //    content: chat.content,
                    //    date: chat.date,
                    //    senderId: $scope.me.id
                    //});
                    $scope.current.input = '';
                }, function (response) {
                    $scope.showError(response.data);
                })
                .finally(function () {
                    $scope.$root.isLoading = false;
                });
        };
    }]);