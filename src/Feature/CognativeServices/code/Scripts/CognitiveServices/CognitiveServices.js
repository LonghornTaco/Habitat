(function($) {

    var webcamCapture = {
        videoTracks: undefined,

        initialize: function(options) {
            this.webcamAccessErrorSectionElement = document.getElementById(options.webcamAccessErrorSection);
            this.videoPlayerElement = document.getElementById(options.videoPlayer);
            this.snapshotCanvasElement = document.getElementById(options.snapshotCanvas);

            navigator.mediaDevices.getUserMedia({ video: true })
                .then(this.attachWebcamStreamToVideoPlayer.bind(this))
                .then(this.hideWebcamAccessErrorSection.bind(this))
                .then(this.displayVideoPlayer.bind(this))
                .catch(this.handleWebcamAccessError.bind(this));
        },

        teardown: function() {
            this.stopWebcamStream();
            this.hideWebcamAccessErrorSection();
            this.hideVideoPlayer();
            this.hideSnapshot();
        },

        hide: function(element) {
            element.classList.add("hidden");
        },

        show: function(element) {
            element.classList.remove("hidden");
        },

        attachWebcamStreamToVideoPlayer: function(stream) {
            // Attach the video stream to the video element and autoplay.
            this.videoPlayerElement.srcObject = stream;
            this.videoTracks = stream.getVideoTracks();
        },

        captureSnapshot: function() {
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
        },

        getSnapshot: function() {
            return this.snapshotCanvasElement.toDataURL("image/jpeg", 1.0);
        },

        stopWebcamStream: function() {
            this.videoTracks.forEach(function(track) {
                track.stop();
            });
        },

        displayVideoPlayer: function() {
            this.show(this.videoPlayerElement);
        },

        hideVideoPlayer: function() {
            this.hide(this.videoPlayerElement);
        },

        displaySnapshot: function() {
            this.show(this.snapshotCanvasElement);
        },

        hideSnapshot: function() {
            this.hide(this.snapshotCanvasElement);
        },

        hideWebcamAccessErrorSection: function() {
            this.hide(this.webcamAccessErrorSectionElement);
        },

        handleWebcamAccessError: function(error) {
            if (error.name === "PermissionDeniedError") {
                this.show(this.webcamAccessErrorSectionElement);
            }
        }
    };


    $(function() {
        var checkboxElement = document.getElementById("EnableFacialRecognition");

        checkboxElement.addEventListener("click", function () {
            if (checkboxElement.checked) {
                webcamCapture.initialize({
                    webcamAccessErrorSection: "webcamAccessErrorSection",
                    videoPlayer: "player",
                    snapshotCanvas: "snapshot"
                });
            } else if (webcamCapture.videoTracks) {
                webcamCapture.teardown();
            }
        });

        function onSaveClick() {
            if (checkboxElement.checked) {
                webcamCapture.captureSnapshot();
                var snapshot = webcamCapture.getSnapshot();
                // TODO: Set a hidden field value with the snapshot before submit.
            }

            return true;
        }
    });

})(jQuery);