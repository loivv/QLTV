var app = angular.module('myApp', ['ui.select2']);
app.controller('myCtrl', function ($scope, $http, $filter) {

    $scope.groups = groups;
    $scope.accounts = [];
    $scope.getData = function () {

        showLoader(true);
        $http({
            method: "GET",
            url: "/nguoidung/GetAll"
        }).then(function success(response) {
            showLoader(false);

            $scope.accounts = response.data.data;

        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });

    };

    $scope.getData();

    $scope.account = {};
    $scope.createAccount = function () {
        showModel('addNgDungModal');
        $scope.account = {};
    };

    $scope.modify = function (idx) {
        $scope.account = $scope.accounts[idx];
        showModel('modifyNgDungModal');
    };

    $scope.finishCreate = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/Account/AddAccount",
            data: $scope.account
        }).then(function success(response) {
            showLoader(false);
            var result = response.data;

            if (result.error === 1) {
                alert(result.msg);
            } else {
                hideModel('addNgDungModal');
                showNotify("Đã tạo tài khoản " + $scope.account.UserName);
                $scope.searchInfo.FullName = '';
                $scope.getData();
            }

        }, function error() {
            showLoader(false);
            showNotify("Không thể lấy dữ liệu");
        });
    };

    $scope.finishModify = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/NguoiDung/Modify",
            data: $scope.account
        }).then(function success(response) {
            showLoader(false);
            var result = response.data;

            if (result.error === 1) {
                alert(result.msg);
            } else {
                hideModel('modifyNgDungModal');
                showNotify("Đã chỉnh sửa");
                $scope.getData();
            }

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
            url: "/nguoidung/ActiveAccounts",
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
            url: "/manage/SetPasswordUser",
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