// tao controller
var app = angular.module('myApp', ['ui.select2']);
app.controller('myCtrl', function ($scope, $http) {

    $scope.schools = [];

    $scope.searchInfo = { MaTruong: '' };

    $scope.accounts = [];
    $scope.account = {};

    $scope.createAccount = function () {
        $scope.account = {};
        showModel('createAccount');
    };

    $http.get("/quantri/GetFrist").then(function (response) {

        $scope.schools = response.data.schools;

    });

    $scope.getData = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/quantri/GetAccounts",
            data: $scope.searchInfo
        }).then(function success(response) {
            showLoader(false);

            $scope.accounts = response.data.data;

        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });

    };


    $scope.activeAccounts = function (idx) {
        showLoader(true);
        var data = $scope.accounts[idx];
        $http({
            method: "POST",
            url: "/quantri/ActiveAccounts",
            data: {
                UserName: data.UserName,
                IsActive: data.IsActive
            }
        }).then(function success(response) {
            showLoader(false);

        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });

    };


    $scope.finishCreate = function () {
        showLoader(true);

        $http({
            method: "POST",
            url: "/account/RegisterAdmin",
            data: $scope.account
        }).then(function success(response) {
            showLoader(false);
            var result = response.data;

            if (result.error === 1) {
                alert(result.msg);
            } else {
                hideModel('createAccount');
                showNotify("Đã tạo tài khoản " + $scope.account.UserName);
                $scope.searchInfo.MaTruong = $scope.account.MaTruong;
                $scope.getData();
            }

        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });
    };

    $scope.changePass = {};
    $scope.showChangePass = function (idx) {
        var data = $scope.accounts[idx];

        $scope.changePass = { UserName: data.UserName, FullName: data.FullName, Password: '', ConfirmPassword: '' };

        showModel('changepassmodal');

    };

    $scope.finishChangePass = function () {
        showLoader(true);

        $http({
            method: "POST",
            url: "/manage/SetPasswordAdmin",
            data: $scope.changePass
        }).then(function success(response) {
            showLoader(false);
            var result = response.data;
            if (result.error === 1) {
                alert(result.msg);
            } else {
                hideModel('changepassmodal');
                showNotify("Đã đổi mật khẩu");
            }
        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });

    };


});
