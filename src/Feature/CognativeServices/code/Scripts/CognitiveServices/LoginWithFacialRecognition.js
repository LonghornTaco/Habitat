(function ($, window) {

    $(function () {
        var fakeImageElement = document.getElementById("fakeImage");
        var webcamSectionElement = document.getElementById("webcamPanel");
        var searchingLabelElement = document.getElementById("searchingLabel");

        var webcamAccessErrorSectionElement = document.getElementById("webcamAccessErrorSection");
        var unableToIdentifyErrorSectionElement = document.getElementById("unableToIdentifyErrorSection");

        var webcamCapture = new window.WebcamSnapshotTaker();
        var retryLoginTimeoutObject;
        var currentloginAttemptNumber = 0;
        var MAXIMUM_NUMBER_OF_LOGIN_RETRY = 10;
        var RETRY_DELAY_IN_MILLISECONDS = 2000;

        function tryLogin() {
            if (webcamCapture.isInitialized()) {
                webcamCapture.captureSnapshot();
                var snapshot = webcamCapture.getSnapshot();
                currentloginAttemptNumber += 1;

                var jqxhr = $.post('/api/cognitiveservices/loginwithhello',
                    { 'CapturedImage': snapshot },
                    function (data, textStatus, jqXHR) {
                        webcamCapture.switchToSnapshot();
                        window.location.href = "/";
                    },
                    "json")
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        retryLoginTimeoutObject = setTimeout(retryLogin, RETRY_DELAY_IN_MILLISECONDS);
                    });
            }
        }

        function retryLogin() {
            if (currentloginAttemptNumber < MAXIMUM_NUMBER_OF_LOGIN_RETRY) {
                tryLogin();
            } else {
                webcamCapture.teardown();
                $(webcamSectionElement).hide();
                $(searchingLabelElement).hide();
                $(fakeImageElement).show();
                $(unableToIdentifyErrorSectionElement).show();
            }
        }

        webcamCapture.initialize({
            videoPlayer: "loginPlayer",
            snapshotCanvas: "loginSnapshot"
        })
            .then(function () {
                $(fakeImageElement).hide();
                $(searchingLabelElement).show();
                $(webcamSectionElement).show();
                retryLoginTimeoutObject = setTimeout(retryLogin, RETRY_DELAY_IN_MILLISECONDS);
            })
            .catch(function() {
                $(webcamAccessErrorSectionElement).show();
            });
    });

})(jQuery, window);