// tao controller
var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope, $http, $filter) {

    // lay du lieu
    $scope.groups = [];
    $scope.getData = function () {
        showLoader(true);
        $http({
            method: "GET",
            url: "/NhomNguoiDung/GetNhom"
        }).then(function mySuccess(response) {
            showLoader(false);

            $scope.groups = response.data;

            for (var i = 0; i < $scope.groups.length; i++) {
                if ($scope.groups[i].IsActive === 1) {
                    $scope.groups[i].active = true;
                } else {
                    $scope.groups[i].active = false;
                }
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    }
    // goi lay du liẹ
    $scope.getData();


    $scope.group = {};
    $scope.action = 'edit';

    $scope.showAddGroups = function () {
        $scope.group = {};
        $scope.action = 'add';
        showModel('addGroupModal');
    };

    $scope.showEditGroup = function (idx) {
        $scope.group = $scope.groups[idx];
        $scope.action = 'edit';
        showModel('addGroupModal');
    };


    $scope.finishForm = function () {

        showLoader(true);

        if ($scope.action === "edit") {
            $http({
                method: "POST",
                url: "/NhomNguoiDung/EditGroup",
                data: {
                    name: $scope.group.GroupName,
                    id: $scope.group.GroupID
                }
            }).then(function mySuccess(response) {
                showLoader(false);
                $scope.getData();
                hideModel('addGroupModal');

            }, function myError(response) {
                showLoader(false);
                showNotify('Connect error');
            });
        } else {
            $http({
                method: "POSt",
                url: "/NhomNguoiDung/AddGroup",
                data: {
                    name: $scope.group.GroupName
                }
            }).then(function mySuccess(response) {
                showLoader(false);
                $scope.getData();
                hideModel('addGroupModal');

            }, function myError(response) {
                showLoader(false);
                showNotify('Connect error');
            });
        }

    };

    $scope.activeGroups = function (idx) {
        showLoader(true);
        $scope.groups[idx].IsActive = $scope.groups[idx].active ? 1 : 0;
        $http({
            method: "POSt",
            url: "/NhomNguoiDung/ActiveGroup",
            data: {
                id: $scope.groups[idx].GroupID,
                isActive: $scope.groups[idx].active
            }
        }).then(function mySuccess(response) {
            showLoader(false);


        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

    $scope.menus = [];
    $scope.groupMenus = groupMenus;
    $scope.GroupID = '';
    $scope.GroupName = '';
    $scope.showMenus = function (idx) {
        showLoader(true);

        $http({
            method: "GET",
            url: "/NhomNguoiDung/GrouUserMenu?id=" + $scope.groups[idx].GroupID
        }).then(function mySuccess(response) {
            showLoader(false);
            $scope.menus = response.data;
            $scope.GroupID = $scope.groups[idx].GroupID;
            $scope.GroupName = $scope.groups[idx].GroupName;
            $('.nav-tabs a[href="#tab_sophat"]').tab('show');
            console.log($scope.menus);
            for (var i = 0; i < $scope.menus.length; i++) {
                if ($scope.menus[i].groupUserId === null) {
                    $scope.menus[i].IsActive = false;
                } else {
                    $scope.menus[i].IsActive = true;

                    if ($scope.menus[i].canedit === 1) {
                        $scope.menus[i].IsEdit = true;
                    } else {
                        $scope.menus[i].IsEdit = false;
                    }
                }
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });

    };

    $scope.addRole = function (idx) {

        showLoader(true);

        $http({
            method: "POST",
            url: "/NhomNguoiDung/AddRole",
            data: {
                menuId: $scope.menus[idx].Id,
                groupId: $scope.GroupID,
                role: $scope.menus[idx].IsActive
            }
        }).then(function mySuccess(response) {
            showLoader(false);
            var result = response.data;

            if (result.error === 1) {
                showNotify(result.msg);
                $scope.menus[idx].IsActive = !$scope.menus[idx].IsActive;
            } else {
                if ($scope.menus[idx].IsActive) {
                    showNotify("Đã cấp quyền");
                    $scope.menus[idx].IsEdit = false;
                } else {
                    showNotify("Đã hủy quyền");
                    $scope.menus[idx].IsEdit = false;
                }
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

    $scope.addEdit = function (idx) {

        showLoader(true);

        $http({
            method: "POST",
            url: "/NhomNguoiDung/AddEdit",
            data: {
                menuId: $scope.menus[idx].Id,
                groupId: $scope.GroupID,
                role: $scope.menus[idx].IsEdit
            }
        }).then(function mySuccess(response) {
            showLoader(false);
            var result = response.data;

            if (result.error === 1) {
                showNotify(result.msg);
                $scope.menus[idx].IsEdit = !$scope.menus[idx].IsEdit;
            } else {


                if ($scope.menus[idx].IsEdit) {
                    showNotify("Cho chỉnh sửa");
                } else {
                    showNotify("Không cho chỉnh sửa");
                }
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

});
