(function ($, window) {

    $(function () {
        var checkboxElement = document.getElementById("EnableFacialRecognition");
        var saveFaceButton = document.getElementById("saveFace");
        var webcamCapture = new window.WebcamSnapshotTaker();

        checkboxElement.addEventListener("click", function () {
            if (checkboxElement.checked) {
                webcamCapture.initialize({
                    webcamAccessErrorSection: "webcamAccessErrorSection",
                    videoPlayer: "player",
                    snapshotCanvas: "snapshot",
                    webcamSection: "webcamPanel"
                });
            } else if (webcamCapture.isInitialized()) {
                webcamCapture.teardown();
            }
        });

        saveFaceButton.addEventListener("click", function (event) {
            var shouldSubmit = true;

            if (checkboxElement.checked && webcamCapture.isInitialized()) {
                event.preventDefault();
                shouldSubmit = false;

                webcamCapture.captureSnapshot();
                var snapshot = webcamCapture.getSnapshot();

                var jqxhr = $.post('/api/cognitiveservices/setpersonimage',
                    { 'CapturedImage': snapshot },
                    function (data, textStatus, jqXHR) {
                        console.log('success');
                    },
                    "json")
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        console.log("fail");
                    });
            }

            return shouldSubmit;
        });
    });

})(jQuery, window);