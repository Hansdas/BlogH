loginService.controller("loginValidate", function ($scope, loginRequestService) {
	//登录验证
	$scope.LoginCheck = function () {
		if (!$scope.myForm.$valid) {
			angular.element(document.getElementsByClassName("alert alert-error hide")).show()
			angular.element(".alertError").html("请输入账号和密码");
			$scope.sName = true;
			return;
		}
		else {
			angular.element(document.getElementsByClassName("alert alert-error hide")).hide();
			$scope.sName = false;
		};
		$scope.params = {
			"username": $scope.username,
			"password": $scope.password
		};
		loginRequestService.login($scope.params).success(function (response) {
			var result = angular.fromJson(response);
			if (result.isSuccess == false) {
				angular.element(document.getElementsByClassName("alert alert-error hide")).show()
				angular.element(".alertError").html(result.message);
				$scope.sName = true;
			}
			else {
				window.location.href = '/Home/Index';
			}
		}).error(function (response) {

		});
	}
	//创建账户
	$scope.Register = function () {
		angular.element(document.getElementsByClassName("form-vertical login-form")).hide()
		angular.element(document.getElementsByClassName("form-vertical register-form")).show()
	}
});