/* 自訂 filter 支持 dictionary (non-array) object 的搜尋 */
angular.module('iTalkFilters')
    .filter('custom', function () {
        return function (input, search) {
            if (!input) return input;
            if (!search) return input;

            var propertyNames = Object.keys(search);
            var expected;
            if (angular.isArray(propertyNames) && propertyNames.length > 0) {
                expected = ('' + search[propertyNames[0]]).toLowerCase();
            }
            else {
                expected = ('' + search).toLowerCase();
            }
            var result = {};
            angular.forEach(input, function (value, key) {
                var actual;
                if (angular.isArray(propertyNames) && propertyNames.length > 0) {
                    actual = ('' + value[propertyNames[0]]).toLowerCase();
                }
                else {
                    var actual = ('' + value).toLowerCase();
                }
                if (actual.indexOf(expected) !== -1) {
                    result[key] = value;
                }
            });
            return result;
        }
    })