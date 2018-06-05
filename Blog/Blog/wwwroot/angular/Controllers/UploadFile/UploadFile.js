var app = angluar.module("myApp", []);
app.controller("photoController", ["$Scope", "FileUploader", function ($Scope, FileUploader) {
	$Scope.uploadStatus = $Scope.uploadStatus = false;
	var uploader = $Scope.uploader = new FileUploader({
		url: "",
		limit: 1,
		FileUploader: File,
		removeAfterUpload: true
	});
	uploader.onAfterAddingFile = function (file) {

	};
	Upload.upload({
	})
}])