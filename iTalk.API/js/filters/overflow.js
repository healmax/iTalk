angular.module('iTalkFilters')
    .filter('overflow', function ($filter) {
        return function (input, limit, suffix, begin) {
            if (input) {
                var result = $filter('limitTo')(input, limit, begin);
                if (result.length < input.length) {
                    suffix = suffix ? suffix : '...';
                    return result + suffix;
                }
            }
            return input;
        }
    })