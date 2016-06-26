angular.module('iTalkFilters')
    .filter('displayDate', function ($filter) {
        return function (input) {
            if (input) {
                var date = new Date(input);
                var now = new Date();
                if (date.toDateString() == new Date().toDateString()) {
                    return $filter('date')(date, 'a hh:mm', 'utc +08:00');
                }
                else if (date == new Date().setDate(now.getDate() - 1)) {
                    return '昨天';
                }
                else if (date == new Date().setDate(now.getDate() - 2)) {
                    return '前天';
                }
                else {
                    return $filter('date')(date, 'yy.MM.dd', 'utc +08:00');
                }
            }
            return input;
        }
    })