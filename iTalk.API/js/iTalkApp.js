﻿angular.module('iTalkApp', ['matchmedia-ng', 'ngMaterial', 'ngMessages', 'iTalkFilters', 'iTalkDirectives', 'iTalkServices'])
    .config(['$compileProvider', function ($compileProvider) {
        $compileProvider.debugInfoEnabled(false);
        //$locationProvider.html5Mode({ enabled: true, requireBase: false });
    }])
    .run(['$rootScope', '$window', function ($rootScope, $window) {
        /**
        * @id friend or group id
        * return web api controll name
        */
        $rootScope.getControllerName = function (targetId) {
            return targetId > 0 ? 'chat' : 'groupChat';
        };

        $rootScope.isSameDay = function (date1, date2) {
            return new Date(date1).toLocaleDateString() == new Date(date2).toLocaleDateString();
        }

        $rootScope.toLocaleDateString = function (date) {
            return new Date(date).toLocaleDateString();
        }

        $rootScope.errors;

        $rootScope.showError = function (result, title) {
            var errorText = result.statusCode + ' : ' + result.message;
            if (title) {
                errorText = title + '\n' + errorText
            }

            alert(errorText);

            if (!$rootScope.errors) {
                $rootScope.errors = [];
            }

            $rootScope.errors.push(errorText);
        }

        //$rootScope.count = function (dict) {
        //    return Object.keys(dict).length;
        //}

        //$rootScope.copyObject = function (source, dest) {
        //    for (var name in source) {
        //        dest[name] = source[name];
        //    }
        //}

        $rootScope.scrollChatListToBottom = function (targetId, scrollOnlyNotAtTop) {
            setTimeout(function () {
                var md = angular.element("#chat-list-" + targetId);
                if (md[0].scrollTop === 0 || !scrollOnlyNotAtTop) {
                    md[0].scrollTop = md[0].scrollHeight;
                }
            }, 0);
        }

        $rootScope.open = function (url) {
            if (url) {
                if (angular.isString(url)) {
                }
                else {
                    url = angular.element(url).css('background-image');
                }
                $window.open(url);
            }
        }

        Array.prototype.find = function (callback) {
            for (var i = 0; i < this.length; i++) {
                if (callback(this[i], i, this)) {
                    return this[i];
                }
            }
        }

        $rootScope.isMobile = function () {
            // TODO : 暫時先這樣判斷
            return $.mobile;
        }
    }]);

angular.module('iTalkFilters', []);
angular.module('iTalkDirectives', []);
angular.module('iTalkServices', []);