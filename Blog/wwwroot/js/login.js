layui.use(["form"], function () {
	var form = layui.form;
	var layer = layui.layer;
	form.on("submit(login)", function (data) {
		var loginData = data.field;
		var index = layer.load(2)
		$.ajax({
			url: "/Login/LoginIn",
			type: "post",
			data: {
				"Account": loginData.Account,
				"Password": loginData.Password
			},
			success: function (response) {
				if (response.message == "OK") {
					window.location.href = "../home/index";

				}
				else {
					layer.close(index)
					layer.msg(response.message, { icon: 5 });
				}
			},
			error: function (response) {
				layer.close(index)
				layer.msg("响应服务器失败", { icon: 2 });
			}
		});
		return false;
	})
})