iTalkApp.controller('indexController', ['$scope', '$http', '$mdSidenav', '$mdToast', 'Hub', 'matchmedia', function ($scope, $http, $mdSidenav, $mdToast, Hub, matchmedia) {
    $scope.isInit = 3;

    matchmedia.onPhone(function (mediaQueryList) {
        $scope.isPhone = mediaQueryList.matches;
    });

    $scope.users = [];
    $scope.friends = [];
    $scope.groups = [];
    $scope.chats = {};

    $scope.getTarget = function (targetId) {
        var id = parseInt(targetId);
        return targetId > 0 ?
            $scope.friends.find(function (f) {
                return f.id === id;
            }) :
            $scope.groups.find(function (g) {
                return g.id === id;
            });
    }

    $scope.getUser = function (userId) {
        var id = parseInt(userId);
        return $scope.users.find(function (u) {
            return u.id === id;
        })
    }

    // 目前對話的朋友或群組
    $scope.current = null;

    $scope.initUser = function (username) {
        $http.get('/account?userName=' + username)
            .then(function (response) {
                $scope.me = response.data.result;
            }).finally(function () {
                loadingComplete();
            })
    }

    $http.get('/friend')
        .then(function (response) {
            $scope.friends = response.data.result;
            angular.forEach(response.data.result, function (f, i) {
                $scope.chats[f.id.toString()] = [];
            });
            loadingComplete();
        }, function (response) {
            showError(response.data);
            loadingComplete();
        });

    $http.get('/group')
        .then(function (response) {
            $scope.groups = response.data.result;
            angular.forEach(response.data.result, function (g, i) {
                $scope.chats[g.id.toString()] = [];
                $http.get('/group?groupId=' + g.id)
                    .then(function (response) {
                        angular.forEach(response.data.result, function (user) {
                            var exist = $scope.getUser(user.id);
                            if (!exist) {
                                $scope.users.push(user);
                            }
                            else {
                                $scope.copyObject(exist, user);
                            }
                        })
                    }, function (response) {
                        $scope.showError(response.data);
                    })
            });
            loadingComplete();
        }, function (response) {
            showError(response.data);
            loadingComplete();
        });

    function loadingComplete() {
        $scope.isInit--;

        if (!$scope.isInit) {
            //if (!$scope.friendAndGroups) {
            //    $scope.friendAndGroups = [];
            //    function create() {
            //        if ($scope.friends && $scope.groups) {
            //            $scope.friendAndGroups.length = 0;
            //            angular.forEach($scope.friends, function (f) {
            //                $scope.friendAndGroups.push(f);
            //            });
            //            angular.forEach($scope.groups, function (g) {
            //                $scope.friendAndGroups.push(g);
            //            });
            //            $scope.friendAndGroups.sort(function (a, b) {
            //                if (a.lastChat) {
            //                    return b.lastChat ? new Date(a.lastChat.date) - new Date(b.lastChat.date) : 1;
            //                }
            //                else {
            //                    return b.lastChat ? -1 : 0;
            //                }

            //            }).reverse();
            //        }
            //    }

            //    $scope.$watchCollection('friends', function (newVal, oldVal) {
            //        create();
            //    });
            //    $scope.$watchCollection('groups', function (newVal, oldVal) {
            //        create();
            //    });
            //}

            angular.element('#loading-modal').modal('hide');
        }
    }

    $scope.tabIndex = 1;

    $scope.setTabIndex = function (index) {
        $scope.tabIndex = index;
    };

    $scope.isCurrentTab = function (index) {
        return $scope.tabIndex === index;
    };

    $scope.isSender = function (chat) {
        return chat.senderId === $scope.me.id;
    };

    $scope.setCurrent = function (target) {
        if ($scope.current !== target) {
            $scope.$root.isLoading = true;
            $scope.current = target;

            $http.get('/' + $scope.getControllerName(target.id) + '?targetId=' + target.id)
                .then(function (response) {
                    angular.merge($scope.chats[target.id.toString()], response.data.result);
                }, function (response) {
                    $scope.showError(response.data);
                }).finally(function () {
                    updateNotice(target);
                    $scope.$root.isLoading = false;
                    //$scope.scrollChatListToBottom(target.id);
                })
        }
    }

    function updateNotice(target, lastChat) {
        if (!lastChat) {
            var chats = $scope.chats[target.id.toString()];
            for (var i = chats.length - 1; i >= 0; i--) {
                if (chats[i].senderId !== $scope.me.id) {
                    lastChat = chats[i];
                    break;
                }
            }
        }
        if (lastChat) {
            function update(callback) {
                $http.post('/notice', { 'id': target.id, 'readTime': lastChat.date })
                 .then(function (response) {
                     callback();
                 }, function (response) {
                     $scope.showError(response.data, '更新失敗');
                 });
            }
            if (target.id > 0) {
                if (target.myReadTime < lastChat.date) {
                    update(function () {
                        target.myReadTime = lastChat.date;
                    });
                }
            }
            else {
                var member = target.members.find(function (m) {
                    return m.id === $scope.me.id;
                });

                if (member.readTime < lastChat.date) {
                    update(function () {
                        member.readTime = lastChat.date;
                    });
                }
            }
        }
    }

    $scope.detail = null;

    $scope.showDetail = function (navId, id) {
        if ($.isNumeric(id)) {
            if (id > 0) {
                $scope.detail = $scope.getUser(id);
            }
            else {
                $scope.detail = $scope.getTarget(id);
            }
        }
        else {
            $scope.detail = $scope.current;
        }

        $mdSidenav(navId).toggle();
    };

    $scope.calcReadCount = function (groupId, chatTime) {
        var count = 0;
        angular.forEach($scope.getTarget(groupId).members, function (member, index) {
            if (member.id !== $scope.me.id && member.readTime >= chatTime) {
                count++;
            }
        })

        return count;
    }

    $scope.totalUnreadMessageCount = function () {
        // ng-repeat alias 在自己的 scope 中，外面取不到只好這樣計算...
        var count = $scope.friends.reduce(function (previous, current) {
            return previous + current.unreadMessageCount;
        }, 0);

        count += $scope.groups.reduce(function (previous, current) {
            return previous + current.unreadMessageCount;
        }, 0);

        return count ? count : null;
    }

    //$scope.getFriendsAndGroups = function () {
    //    var raw = [];
    //    angular.extend(raw, $scope.friends);
    //    angular.extend(raw, $scope.groups);
    //    var result = Object.keys(raw).map(function (key) {
    //        return raw[key];
    //    }).sort(function (a, b) {
    //        if (a.lastChat) {
    //            return b.lastChat ? new Date(a.lastChat.date) - new Date(b.lastChat.date) : 1;
    //        }
    //        else {
    //            return b.lastChat ? -1 : 0;
    //        }

    //    }).reverse();

    //    return result;
    //}

    $scope.updateUnreadMessageCount = function (target) {
        var chats = $scope.chats[target.id.toString()];
        if ($scope.current && $scope.current.id === target.id) {
            var count = 0;
            angular.forEach(chats, function (c) {
                if (target.myReadTime < c.date) {
                    count++;
                }
            })
            target.unreadMessageCount = count;
        }

        return target.unreadMessageCount;
    }

    //$scope.test = function (id, chat) {
    //    var result = $scope.getTarget(id).readTime >= chat.date;
    //    return result;
    //}

    $scope.notifications = [];

    $scope.showNotify = function () {
        $mdToast.show({
            controller: 'notifyController',
            templateUrl: 'chatNotificationView',
            hideDelay: 5000,
            position: 'bottom right',
            preserveScope: true,
            scope: $scope
        }).then(function () {
            $scope.notifications.shift();
        });
    }

    $scope.sortByChatTime = function (target) {
        return target.lastChat ? target.lastChat.date : '';
    }

    function pushChat(chat, target) {
        target.lastChat = chat;

        if (chat.senderId !== $scope.me.id) {
            // server push 他人的對話
            if ($scope.current && $scope.current.id === target.id) {
                // 正在對話中...
                $scope.chats[target.id.toString()].push(chat);
                updateNotice(target, chat);
                $scope.scrollChatListToBottom(target.id);
            }
            else {
                // 不在對話中
                target.unreadMessageCount++;
                $scope.notifications.push(chat);
                $scope.showNotify();
            }
        }
        else {
            // server push 自己的對話
            if ($scope.current && $scope.current.id === target.id) {
                $scope.chats[target.id.toString()].push(chat);
                $scope.scrollChatListToBottom(target.id);
            }
        }

        $scope.$apply();
    }
    // SignalR
    var hub = new Hub("chatHub", {
        listeners: {
            'receiveChat': function (friendId, chat) {
                pushChat(chat, $scope.getTarget(friendId));
            },
            'receiveGroupChat': function (groupId, chat) {
                pushChat(chat, $scope.getTarget(groupId));
            },
            'updateFriendReadTime': function (friendId, readTime) {
                $scope.getTarget(friendId).readTime = readTime;
                $scope.$apply();
            },
            'updateGroupMemberReadTime': function (groupId, friendId, readTime) {
                var members = $scope.getTarget(groupId).members;
                for (var i in members) {
                    if (members[i].id === friendId) {
                        members[i].readTime = readTime;
                        break;
                    }
                }
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
        logging: false
    });
}])
.controller('notifyController', function ($scope, $mdToast) {
    $scope.closeNotification = function () {
        $mdToast.hide();
    };
});;