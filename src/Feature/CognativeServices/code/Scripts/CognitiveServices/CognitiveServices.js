(function (window) {

    function webcamSnapshotTaker () {
    }

    webcamSnapshotTaker.prototype.initialize = function (options) {
        this.videoPlayerElement = document.getElementById(options.videoPlayer);
        this.snapshotCanvasElement = document.getElementById(options.snapshotCanvas);

        this.teardown();
        var _this = this;

        return new Promise(function (resolve, reject) {
            navigator.mediaDevices.getUserMedia({ video: true })
                .then(_this.attachWebcamStreamToVideoPlayer.bind(_this))
                .then(_this.displayVideoPlayer.bind(_this))
                .then(resolve)
                .catch(reject);
        });
    };

    webcamSnapshotTaker.prototype.teardown = function () {
        this.stopWebcamStream();
        this.hideVideoPlayer();
        this.hideSnapshot();
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

    window.WebcamSnapshotTaker = webcamSnapshotTaker;

})(window);