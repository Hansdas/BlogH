
$(function () {
	//导航栏点击变色
	$(".nav a").click(function () {
		$(".nav a").each(function () {
			$(this).removeClass("active");
		});
		var thisItem = $(this)[0];
		$(this).addClass("active");
		$(".header .btn").empty();
		if (thisItem.innerText == "微语") {
			$("#console").attr("src", "../whisper/index");
			$(".header .btn").append('<a href="../whisper/addWhisper" class="layui-btn layui-btn-normal">发表微语</a>');
		}
		if (thisItem.innerText == "文章") {
			$("#console").attr("src", "../article/index");
			//$(".header .btn").append('<a href="../whisper/addWhisper" class="layui-btn layui-btn-normal">发表文章</a>');
		}
	});
	//iframe自适应内容高度
	$("#console").each(function (index) {
		var that = $(this);
		(function () {
			setInterval(function () {
				setIframeHeight(that[0]);
			}, 300);
		})(that);
	});
	showPage();
	//$(".layui-nav-item a").mouseover(function () {
	//	$(this).attr("style", "color:black !important");
	//});
});
function setIframeHeight(iframe) {
    if (iframe) {
        var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
        if (iframeWin.document.body) {
            iframe.height = iframeWin.document.body.scrollHeight;
        }
    }
}
function showPage() {
	var urlParamater = window.location.search;
	if (urlParamater == "")
		return;
	var page = getSearchString("pagepath", urlParamater);
	$("#console").attr("src", "../" + page + "/Index");
	$(".nav a").each(function (index) {
		var thisItem = $(this)[0];
		if (thisItem.innerText == "微语") {
			$(this).addClass("active");
		}
		else {
			$(this).removeClass("active");
		}

	});
}
function getSearchString(key, Url) {
	var str = Url;
	str = str.substring(1, str.length); // 获取URL中?之后的字符（去掉第一位的问号）
	// 以&分隔字符串，获得类似name=xiaoli这样的元素数组
	var arr = str.split("&");
	var obj = new Object();

	// 将每一个数组元素以=分隔并赋给obj对象 
	for (var i = 0; i < arr.length; i++) {
		var tmp_arr = arr[i].split("=");
		obj[decodeURIComponent(tmp_arr[0])] = decodeURIComponent(tmp_arr[1]);
	}
	return obj[key];
}
