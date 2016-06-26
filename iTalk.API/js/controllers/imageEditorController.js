angular.module('iTalkApp')
    .controller('imageEditorController', ['$scope', function ($scope) {
        var defaultImage = constants.DEFAULT_IMAGE;

        $scope.currentImage = null;

        $scope.previewImage = function (event) {
            var container = angular.element('.image-container');
            $scope.currentImage = defaultImage;
            var image = event.target.files[0];

            if (image) {
                if (image.size > 1024 * 1024) {
                    alert('圖片不能超過 1 MB!!!');
                    event.target.value = '';
                }
                else {
                    if (image.type.split('/')[0] !== 'image') {
                        alert('只能選擇圖片!!!');
                        event.target.value = '';
                    }
                    else {
                        $scope.currentImage = URL.createObjectURL(image);
                    }
                }
            }

            container.css('background-image', 'url(' + $scope.currentImage + ')');
        }
    }])