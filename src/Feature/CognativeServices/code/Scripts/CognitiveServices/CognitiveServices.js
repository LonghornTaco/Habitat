(function (window) {

    function webcamSnapshotTaker() {
    }

    webcamSnapshotTaker.prototype.initialize = function (options) {
        this.webcamAccessErrorSectionElement = document.getElementById(options.webcamAccessErrorSection);
        this.videoPlayerElement = document.getElementById(options.videoPlayer);
        this.snapshotCanvasElement = document.getElementById(options.snapshotCanvas);
        this.webcamSectionElement = document.getElementById(options.webcamSection);

        this.teardown();

        navigator.mediaDevices.getUserMedia({ video: true })
            .then(this.attachWebcamStreamToVideoPlayer.bind(this))
            .then(this.hideWebcamAccessErrorSection.bind(this))
            .then(this.displayVideoPlayer.bind(this))
            .then(this.displayWebcamSection.bind(this))
            .catch(this.handleWebcamAccessError.bind(this));
    };

    webcamSnapshotTaker.prototype.teardown = function () {
        this.stopWebcamStream();
        this.hideWebcamAccessErrorSection();
        this.hideVideoPlayer();
        this.hideSnapshot();
        this.hideWebcamSection();
    };

    webcamSnapshotTaker.prototype.isInitialized = function () {
        return !!this.videoTracks;
    };

    webcamSnapshotTaker.prototype.hide = function (element) {
        element.classList.add("hidden");
    };

    webcamSnapshotTaker.prototype.show = function (element) {
        element.classList.remove("hidden");
    };

    webcamSnapshotTaker.prototype.attachWebcamStreamToVideoPlayer = function (stream) {
        // Attach the video stream to the video element and autoplay.
        this.videoPlayerElement.srcObject = stream;
        this.videoTracks = stream.getVideoTracks();
    };

    webcamSnapshotTaker.prototype.captureSnapshot = function () {
        var context = this.snapshotCanvasElement.getContext("2d");
        context.drawImage(this.videoPlayerElement,
            0,
            0,
            this.snapshotCanvasElement.width,
            this.snapshotCanvasElement.height);

        // Stop all video streams.
        this.stopWebcamStream();

        this.hideVideoPlayer();
        this.displaySnapshot();
    };

    webcamSnapshotTaker.prototype.getSnapshot = function () {
        return this.snapshotCanvasElement.toDataURL("image/jpeg", 1.0);
    };

    webcamSnapshotTaker.prototype.stopWebcamStream = function () {
        if (this.isInitialized()) {
            this.videoTracks.forEach(function (track) {
                track.stop();
            });
            this.videoTracks = undefined;
        }
    };

    webcamSnapshotTaker.prototype.displayWebcamSection = function () {
        this.show(this.webcamSectionElement);
    };

    webcamSnapshotTaker.prototype.hideWebcamSection = function () {
        this.hide(this.webcamSectionElement);
    };

    webcamSnapshotTaker.prototype.displayVideoPlayer = function () {
        this.show(this.videoPlayerElement);
    };

    webcamSnapshotTaker.prototype.hideVideoPlayer = function () {
        this.hide(this.videoPlayerElement);
    };

    webcamSnapshotTaker.prototype.displaySnapshot = function () {
        this.show(this.snapshotCanvasElement);
    };

    webcamSnapshotTaker.prototype.hideSnapshot = function () {
        this.hide(this.snapshotCanvasElement);
    };

    webcamSnapshotTaker.prototype.hideWebcamAccessErrorSection = function () {
        this.hide(this.webcamAccessErrorSectionElement);
    };

    webcamSnapshotTaker.prototype.handleWebcamAccessError = function (error) {
        if (error.name === "PermissionDeniedError") {
            this.show(this.webcamAccessErrorSectionElement);
        }
    };

    window.WebcamSnapshotTaker = webcamSnapshotTaker;

})(window);