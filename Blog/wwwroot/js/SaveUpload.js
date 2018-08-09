function Update() {
	var formData = new FormData();
	formData.append("uploadImage", $('#uploadImage').get(0).files[0]);
	formData.append('username', $("input[name='username']").val());
	formData.append('mobilephone', $("input[name='mobilephone']").val());
	formData.append('email', $("input[name='email']").val());
	formData.append('sex', $("input[name='sex']:checked").val());
	formData.append('descript', $("input[name='descript']").val());
	$.ajax({
		url: '/User/Update',
		type: 'post',
		contentType: false,
		data: formData,
		processData: false,
		success: function (info) {
			$('.backimg').attr('src', JSON.parse(info).msg);
		},
		error: function (err) {
			console.log(err)
		}
	});


}