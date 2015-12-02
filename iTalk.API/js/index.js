var iTalkApp = angular.module('iTalkApp', ['SignalR', 'matchmedia-ng', 'ui.bootstrap', 'ngMaterial'])
    .controller('indexController', ['$scope', '$http', 'Hub', 'matchmedia', '$mdDialog', function ($scope, $http, Hub, matchmedia, $mdDialog) {
        matchmedia.onPhone(function (mediaQueryList) {
            $scope.isPhone = mediaQueryList.matches;
        });

        $scope.friends = [];
        $scope.groups = [];
        $scope.currentFriend = null;
        $scope.currentGroup = null;

        $http.get('/friend')
            .then(function (response) {
                $scope.friends = response.data.friends;
                angular.forEach(response.data.friends, function (f, key) {
                    f.chats = [];
                    //$scope.friends.push({
                    //    userName: val,
                    //    init: false,
                    //});
                });
            });

        $http.get('/group')
            .then(function (response) {
                $scope.groups = response.data.groups;
                angular.forEach(response.data.friends, function (g, key) {
                    g.chats = [];
                    //$scope.friends.push({
                    //    userName: val,
                    //    init: false,
                    //});
                });
            });

        $scope.setCurrentFriend = function (friend) {
            $scope.currentFriend = friend;

            if (!friend.init) {
                $http.get('/chat?targetId=' + friend.id)
                    .then(function (response) {
                        //angular.forEach(response.data.chats, function (chat, key) {
                        //    chat.date = new Date(chat.date).toJSON();
                        //});

                        friend.chats = response.data.chats;
                        friend.init = true;
                    })
            };
        };

        $scope.setCurrentGroup = function (group) {
            $scope.currentGroup = group;

            if (!group.init) {
                $http.get('/group_chat?targetId=' + group.id)
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
            return chat.senderId === $scope.me.id;
        };

        $scope.send = function () {
            var chat = {
                targetId: $scope.currentFriend.id,
                content: $scope.currentFriend.input,
                date: new Date().toJSON()
            };

            $http.post('/chat/dialog', chat)
            .then(function (response) {
                $scope.currentFriend.chats.push({
                    content: chat.content,
                    date: chat.date,
                    senderId: $scope.me.id
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

        function emptyUser() {
            return {
                name: '',
                status: -1,
                found: {}
            };
        }

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
                    $scope.user.found = response.data;
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
                    $scope.friends.push($scope.user.found);
                    //$scope.friends.push({
                    //    userName: $scope.user.found.userName,
                    //    init: false
                    //});

                    alert('已經將用戶' + $scope.user.found.alias + '加為好友');
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
            $scope.user.input = '';
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
                        if (f.id == chat.targetId) {
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

        //$scope.showCreateGroupDialog = function () {
        //    $mdSidenav('left').toggle();
        //    $mdSidenav('right').toggle();
        //}

        $scope.showCreateGroupDialog = function (ev) {
            $mdDialog.show({
                controller: dialogController,
                templateUrl: 'createGroupView',
                parent: angular.element(document.body),
                targetEvent: ev,
                locals: {
                    friends: $scope.friends
                },
            })
            .then(function (group) {
                $http.post('/group', {
                    name: group.name,
                });
            });
        }

        function dialogController($scope, $mdDialog, friends) {
            $scope.friends = friends;

            $scope.cancel = function () {
                $mdDialog.cancel();
            };

            $scope.create = function (group) {
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

            $scope.querySearch = function (query) {
                var results = query ?
                    $scope.friends.filter(createFilterFor(query)) : [];
                return results;
            }

            function createFilterFor(query) {
                var lowercaseQuery = angular.lowercase(query);
                return function filterFn(friend) {
                    return (angular.lowercase(friend.alias).indexOf(lowercaseQuery) != -1);
                };
            }
        }
    }]);