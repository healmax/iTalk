var iTalkApp = angular.module('iTalkApp', ['SignalR', 'matchmedia-ng', 'ui.bootstrap'])
    .controller('indexController', ['$scope', '$http', 'Hub', 'matchmedia', function ($scope, $http, Hub, matchmedia) {
        matchmedia.onPhone(function (mediaQueryList) {
            $scope.isPhone = mediaQueryList.matches;
        });

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

        $scope.isSender = function (chat) {
            return chat.sender === $scope.myName;
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

        $scope.tabIndex = 2;

        $scope.setTabIndex = function (index) {
            $scope.tabIndex = index;
        };

        $scope.isCurrentTab = function (index) {
            return $scope.tabIndex === index;
        };

        $scope.user = {
            name: '',
            status: -1,
            found: {}
        };

        $scope.searchUser = function () {
            if ($scope.user.name === $scope.myName) {
                $scope.user.status = 1;
                $scope.user.found = {
                    userName: $scope.myName,
                    isFriend: false
                };
                return;
            }

            $http.get('/account?username=' + $scope.user.name)
                .then(function (response) {
                    $scope.user.status = 1;
                    $scope.user.found = response.data;
                }, function () {
                    $scope.user.status = 0;
                    $scope.user.found = null;
                });
        };

        $scope.addFriend = function () {
            $http.post('/relationship', {
                friendname: $scope.user.found.userName
            })
            .then(function (response) {
                if (response.data.success) {
                    $scope.friends.push({
                        userName: $scope.user.found.userName,
                        init: false
                    });

                    alert('已經將用戶' + $scope.user.found.userName + '加為好友');
                    $scope.endAddFriend();
                }
                else {
                    showError(response.data);
                }
            }, function (response) {
                showError(response.data);
            });
        };

        $scope.endAddFriend = function () {
            $scope.user.name = '';
            $scope.user.status = -1;
        }

        function showError(result) {
            alert(result.statusCode + ' : ' + result.message);
        };

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