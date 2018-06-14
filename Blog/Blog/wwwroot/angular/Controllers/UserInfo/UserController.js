userService.controller("photoController", function ($scope, updateUserService) {
	$scope.Update = function () {
		alert("11");
		$scope.params = {
			"username": $scope.username,
			"mobilephone": $scope.mobilephone,
			"mobilephone": $scope.mobilephone,
			"email": $scope.email,
			"sex": $scope.sex,
			"descript": $scope.descript
		};
		updateUserService.Update($scope.params).success(function (response) {
			var result = angular.fromJson(response);
			if (result.isSuccess == false) {
				//angular.element(document.getElementsByClassName("alert alert-error hide")).show()
				//angular.element(".alertError").html(result.message);
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