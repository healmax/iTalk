iTalkApp.controller('indexController', ['$scope', '$http', '$mdSidenav', '$mdToast', 'Hub', 'matchmedia', function ($scope, $http, $mdSidenav, $mdToast, Hub, matchmedia) {
    $scope.isInit = 2;

    matchmedia.onPhone(function (mediaQueryList) {
        $scope.isPhone = mediaQueryList.matches;
    });

    $scope.users = {};
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

    //$scope.getUser = function (userId) {
    //    var id = parseInt(userId);
    //    return $scope.users.find(function (u) {
    //        return u.id === id;
    //    })
    //}

    // 目前對話的朋友或群組
    $scope.current = null;

    $http.get('/friend')
        .then(function (response) {
            $scope.friends = response.data.result;
            angular.forEach(response.data.result, function (f, i) {
                $scope.chats[f.id] = [];
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
                $scope.chats[g.id] = [];
                $http.get('/group?groupId=' + g.id)
                    .then(function (response) {
                        updateUsers(response.data.result);
                    }, function (response) {
                        $scope.showError(response.data);
                    })
            });
            loadingComplete();
        }, function (response) {
            $scope.showError(response.data);
            loadingComplete();
        });


    // 更新朋友或群組
    function updateTarget(source, dest) {
        for (var name in source) {
            if (angular.isArray(dest)) {
                for (var i = 0; i < dest; i++) {
                    angular.merge(source[i] = dest[i]);
                }
            }
            else {
                dest[name] = source[name];
            }
        }
    }

    // 使用 members array 更新 $scope.users
    function updateUsers(members) {
        angular.forEach(members, function (m) {
            var exist = $scope.users[m.id];
            if (!exist) {
                $scope.users[m.id] = m;
            }
            else {
                updateTarget(exist, m);
            }
        })
    }

    function loadingComplete() {
        $scope.isInit--;

        if (!$scope.isInit) {
            function callback() {
                if ($scope.me) {
                    angular.element('#loading-modal').modal('hide');

                    var hash = parseInt($scope.getHash());

                    if (hash) {
                        var target = $scope.getTarget(hash);
                        if (target) {
                            $scope.setCurrent(target);
                        }
                    }
                    remove();
                }
            }
            var remove = $scope.$watch('me', callback);
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
        if ($scope.current !== target || !$scope.isTalking) {
            $scope.$root.isLoading = true;
            $scope.current = target;

            $http.get('/' + $scope.getControllerName(target.id) + '?targetId=' + target.id)
                .then(function (response) {
                    // todo: 只抓未下載的
                    angular.merge($scope.chats[target.id], response.data.result);
                    $scope.setHash(target.id);
                }, function (response) {
                    $scope.showError(response.data);
                }).finally(function () {
                    updateNotice(target);
                    $scope.$root.isLoading = false;
                    $scope.isTalking = true;
                    //$scope.scrollChatListToBottom(target.id);
                })
        }
        else {
            $scope.isTalking = true;
        }
    }

    function updateNotice(target, lastChat) {
        if (!lastChat) {
            var chats = $scope.chats[target.id];
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
                $scope.detail = $scope.users[id];
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
        var chats = $scope.chats[target.id];
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

    $scope.showNotify = function (target, chat, group) {
        $mdToast.show({
            controller: 'notifyController',
            templateUrl: 'chatNotificationView',
            hideDelay: 5000,
            position: $scope.isMobile() ? 'top left' : 'bottom right',
            locals: { 'user': target, 'chat': chat, 'group': group },
            //preserveScope: true,
            //scope: $scope
        }).then(function () {
            $scope.notifications.shift();
        });
    }

    $scope.sortByChatTime = function (target) {
        return target.lastChat ? target.lastChat.date : '';
    }

    $scope.backToMain = function () {
        $scope.setHash('');
        $scope.isTalking = false;
    }

    function pushChat(target, chat) {
        target.lastChat = chat;

        if (chat.senderId !== $scope.me.id) {
            // server push 他人的對話
            if ($scope.current && $scope.current.id === target.id && $scope.isTalking) {
                // 正在對話中...
                $scope.chats[target.id].push(chat);
                updateNotice(target, chat);
                $scope.scrollChatListToBottom(target.id);
            }
            else {
                // 不在對話中
                target.unreadMessageCount++;
                $scope.notifications.push(chat);

                if (target.id < 0) {
                    $scope.showNotify($scope.users[chat.senderId], chat, target);
                }
                else {
                    $scope.showNotify(target, chat);
                }
            }
        }
        else {
            // server push 自己的對話
            if ($scope.current && $scope.current.id === target.id) {
                $scope.chats[target.id].push(chat);
                $scope.scrollChatListToBottom(target.id);
            }
        }

        $scope.$apply();
    }

    // SignalR
    var hub = new Hub("chatHub", {
        listeners: {
            'receiveChat': function (friendId, chat) {
                pushChat($scope.getTarget(friendId), chat);
            },
            'receiveGroupChat': function (groupId, chat) {
                pushChat($scope.getTarget(groupId), chat);
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
            },
            'updateRelationship': function (newObject) {
                var list = newObject.id < 0 ? $scope.groups : $scope.friends;
                var exist = $scope.getTarget(newObject.id);
                if (exist) {
                    updateTarget(exist, newObject);
                }
                else {
                    list.push(newObject);
                    $scope.chats[newObject.id] = [];
                }
                if (newObject.id < 0) {
                    updateUsers(newObject.members);
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
.controller('notifyController', function ($scope, $mdToast, user, chat, group) {
    $scope.user = user;
    $scope.chat = chat;
    $scope.group = group;

    $scope.closeNotification = function () {
        $mdToast.hide();
    }
})