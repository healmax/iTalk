var iTalkApp = angular.module('iTalkApp', ['SignalR'])
    .controller('indexController', ['$scope', '$http', 'Hub', function ($scope, $http, Hub) {
        $scope.friends = [];
        $scope.currentFriend = '';

        $http.get('/relationship')
            .then(function (response) {
                angular.forEach(response.data.friends, function (val, key) {
                    $scope.friends.push({
                        userName: val,
                        init: false,
                    });
                });
            });

        $scope.setCurrentFriend = function (friend) {
            $scope.currentFriend = friend;

            if (!friend.init) {
                $http.get('/chat?username=' + friend.userName)
                    .then(function (response) {
                        //angular.forEach(response.data.chats, function (chat, key) {
                        //    chat.date = new Date(chat.date).toJSON();
                        //});

                        friend.chats = response.data.chats;
                        friend.init = true;
                    })
            };
        };

        $scope.send = function () {
            var chat = {
                friendname: $scope.currentFriend.userName,
                content: $scope.currentFriend.input,
                date: new Date().toJSON()
            };

            $http.post('/chat', chat)
            .then(function (response) {
                $scope.currentFriend.chats.push({
                    content: chat.content,
                    date: chat.date,
                    sender: $scope.myName
                });
                $scope.currentFriend.input = '';
            }, function (response) {
                showError(response.data);
            });
        };

        $scope.tab = 2;

        $scope.setTabIndex = function (index) {
            $scope.tab = index;
        };

        $scope.isCurrentTab = function (index) {
            return $scope.tab === index;
        };

        $scope.searchStatus = -1;

        $scope.searchUser = function () {
            $http.get('/account?username=' + $scope.userToSearch)
                .then(function (response) {
                    $scope.searchStatus = 1;
                    $scope.foundUser = response.data;
                }, function () {
                    $scope.searchStatus = 0;
                    $scope.foundUser = null;
                });
        };

        $scope.addFriend = function () {
            $http.post('/relationship', {
                friendname: $scope.foundUser.userName
            })
            .then(function (response) {
                if (response.data.success) {
                    $scope.friends.push({
                        userName: $scope.foundUser.userName,
                        init: false
                    });

                    alert('已經將用戶' + $scope.foundUser.userName + '加為好友');
                    $scope.endAddFriend();
                }
                else {
                    showError(response.data);
                }
            }, function (response) {
                showError(response.data);
            });
        };

        function showError(result) {
            alert(result.statusCode + ' : ' + result.message);
        };

        $scope.endAddFriend = function () {
            $scope.searchStatus = -1;
            $scope.userToSearch = '';
        }

        // SignalR
        var hub = new Hub("chatHub", {
            listeners: {
                'receiveChat': function (chat) {
                    angular.forEach($scope.friends, function (f, key) {
                        if (f.userName == chat.sender) {
                            f.chats.push(chat);
                            return;
                        }
                    });
                    $scope.$apply();
                }
            },
            //methods: ['send'],
            errorHandler: function (error) {
                console.error(error);
            },
            hubDisconnected: function () {
                if (hub.connection.lastError) {
                    hub.connection.start();
                }
            },
            //transport: 'webSockets',
            logging: true
        });

        //var serverTimeHubProxy = signalRHubProxy(
        //    signalRHubProxy.defaultServer, 'serverTimeHub');

        //clientPushHubProxy.on('receiveChat', function (chat) {
        //    angular.forEach($scope.friends, function (f, key) {
        //        if (f.userName == chat.sender) {
        //            f.chats.push(chat);
        //            return;
        //        }
        //    });
        //});

        //$scope.getServerTime = function () {
        //    serverTimeHubProxy.invoke('getServerTime', function (data) {
        //        $scope.currentServerTimeManually = data;
        //    });
        //};
    }]);