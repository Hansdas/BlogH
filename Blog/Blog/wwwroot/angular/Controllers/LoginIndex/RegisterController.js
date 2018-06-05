loginService.controller("RegisterController", function ($scope, loginRequestService) {
	//注册
	$scope.SignUp = function () {
		if (!$scope.myRegister.$valid) {
			var V = $scope.username;
			if ($scope.username == undefined) {
				angular.element(document.getElementsByClassName("alert alert-error hide")).show();
				angular.element(".alertError").html("请输入账号和密码");
				$scope.registerError = true;
			}
			return false;
		}
		else {
			angular.element(document.getElementsByClassName("alert alert-error hide")).hide();
			$scope.registerError = false;
		}
		if ($scope.password != $scope.rpassword) {
			angular.element(document.getElementsByClassName("alert alert-error hide")).show();
			angular.element(".alertError").html("两次输入密码不一致");
			$scope.registerError = true;
			return false;
		}
		$scope.params = {
			"username": $scope.username,
			"password": $scope.password,
		};
		loginRequestService.Register($scope.params).success(function (response) {
			var result = angular.fromJson(response)
			if (result.isSuccess == false) {
				angular.element(document.getElementsByClassName("alert alert-error hide")).show()
				angular.element(".alertError").html(result.message);
				$scope.registerError = true;
			}
			else {
				window.location.href = "/Home/index.html";
			}

		}).error(function (response) {

			});
	}
});