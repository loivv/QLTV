// tao controller
var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope, $http, $filter) {

    // lay du lieu
    $scope.allDanhMuc = [];
    $scope.GetData = function () {
        showLoader(true);
        $http({
            method: "GET",
            url: "/NhomNguoiDung/getNhomNguoiDung"
        }).then(function mySuccess(response) {
            showLoader(false);

            if (response.data.error === 0) {

                $scope.allDanhMuc = response.data.data;
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    }
    // goi lay du liẹ

});
