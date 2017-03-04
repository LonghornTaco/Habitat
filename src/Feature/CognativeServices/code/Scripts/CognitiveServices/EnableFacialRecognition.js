(function ($, window) {

    $(function () {
        var enableFacialRecognitionButton = document.getElementById("EnableFacialRecognition");
        var webcamAccessErrorSectionElement = document.getElementById("webcamAccessErrorSection");
        var webcamSectionElement = document.getElementById("webcamPanel");
        var saveFaceButton = document.getElementById("saveFace");
        var saveErrorSectionElement = document.getElementById("saveErrorSection");
        var saveSuccessSectionElement = document.getElementById("saveSuccessSection");
        var webcamCapture = new window.WebcamSnapshotTaker();

        var snapshot;

        enableFacialRecognitionButton.addEventListener("click", function () {
            webcamCapture.initialize({
                videoPlayer: "player",
                snapshotCanvas: "snapshot"
            })
                .then(function () {
                    $(webcamAccessErrorSectionElement).hide();
                    $(enableFacialRecognitionButton).hide();
                    $(webcamSectionElement).show();
                })
                .catch(function() {
                    $(webcamAccessErrorSectionElement).show();
                });
        });

        saveFaceButton.addEventListener("click", function (event) {
            var shouldSubmit = true;

            if (webcamCapture.isInitialized() || snapshot) {
                event.preventDefault();
                shouldSubmit = false;

                if (!snapshot) {
                    webcamCapture.captureSnapshot();
                    snapshot = webcamCapture.getSnapshot();
                }

                var jqxhr = $.post('/api/cognitiveservices/setpersonimage',
                    { 'CapturedImage': snapshot },
                    function (data, textStatus, jqXHR) {
                        $(webcamSectionElement).hide();
                        $(saveSuccessSectionElement).show();
                    },
                    "json")
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        $(saveErrorSectionElement).show();
                    });
            }

            return shouldSubmit;
        });
    });

})(jQuery, window);