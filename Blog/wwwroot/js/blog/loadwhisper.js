layui.config({
	base: '/js/'
}).use(['element', 'laypage', 'form', 'menu', 'laytpl'], function () {
	var index = layer.load(2)
	element = layui.element, laypage = layui.laypage, form = layui.form, menu = layui.menu, laytpl = layui.laytpl;
	element.init();
	var whisper = document.getElementById("whisper-item").innerHTML;
	var total = loadtotal();
	laypage.render({
		elem: 'page'
		, count: total
		, layout: ['count', 'prev', 'page', 'next', 'refresh', 'skip']
		, jump: function (obj) {
			var pageSize = obj.limit;
			var pageIndex = obj.curr;
			loadWhisper(pageIndex, pageSize, whisper, index);

		}
	});
})
function loadtotal() {
	var total = 0;
	$.ajax({
		url: "/Whisper/LoadTotal",
		type: "get",
		async: false,
		success: function (response) {
			total = response;
		},
	});
	return total;
}
function loadWhisper(pageIndex, pageSize, whisper, index) {
	$.ajax({
		url: "/Whisper/LoadWhisper",
		type: "get",
		data: {
			"pageIndex": pageIndex,
			"pageSize": pageSize
		},
		datatype: "json",
		success: function (response) {
			var data = { "list": response.data };
			whisperview = document.getElementById("whisper-list-id");
			laytpl(whisper).render(data, function (html) {
				whisperview.innerHTML = html;
			});
			form.render();
			menu.init();
			menu.off();
			menu.submit();
			layer.close(index)
		},
		error: function (response) {
			layer.close(index)
			layer.msg("响应服务器失败", { icon: 7 });
		}
	});
}