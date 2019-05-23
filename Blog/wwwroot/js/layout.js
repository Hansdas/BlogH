
$(function () {
	//导航栏点击变色
	$(".nav a").click(function () {
		$(".nav a").each(function (index) {
			$(this).removeClass("active");
		});
		var thisItem = $(this)[0];
		$(this).addClass("active");
		if (thisItem.innerText == "微语") {
			$("#console").attr("src", "../Whisper/Index");
			$(".header .btn").append('<a href="../Whisper/AddWhisper" class="layui-btn layui-btn-normal">发表微语</a>')
		}
	});
	//iframe自适应内容高度
	$("#console").each(function (index) {
		var that = $(this);
		(function () {
			setInterval(function () {
				setIframeHeight(that[0]);
			}, 200);
		})(that);
	});
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
