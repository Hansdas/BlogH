//服务
var loginService = angular.module("loginApp",[]);
//请求服务
loginService.factory("loginRequestService", ["$http",function ($http) {
	//请求参数
	var requestParams = {
		method: "",
		url: "",
		contentType: "",
		params:{}
	};
	//$http({requst})
	var request = {
		//登录
		login:function (data) {
			requestParams.method = "GET";
			requestParams.url = "/Login/Login";
			requestParams.contentType = "application/json;charset=utf-8";
			requestParams.params = data;
			return RequestService($http, requestParams);
		},
		//注册账号
		Register:function (data) {
			requestParams.method = "POST";
			requestParams.url = "/Login/Register";
			requestParams.contentType = "application/x-www-form-urlencoded;charset=utf-8";
			requestParams.params = data;
			return RequestService($http, requestParams);
		}
	};
	return request;
}]);
//请求服务
function RequestService($http, request) {
	var result = $http(request);
	return result;
}
