$(document).ready(function () {

    $('#refreshCode').click(
        function () {
            var url = "/ValidationCode/RefreshCode"
            var data = "";
            $.get(url, data, function (response) {
                $('#validationBox').attr('value', response);
                window.clearInterval(setTimer);
                run();
            });
        });

    window.onload = run;
});

function countDown() {
    var s = document.getElementById('countDownSpan');
    if (s.innerHTML != 0) {
        s.innerHTML = s.innerHTML * 1 - 1;
    }
};

var setTimer;

function run() {
    document.getElementById('countDownSpan').innerHTML = 30;
    setTimer = window.setInterval("countDown();", 1000);
}