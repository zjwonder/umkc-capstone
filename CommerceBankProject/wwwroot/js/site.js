// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function trackSlider(slide, output) {
    document.getElementById(output).value = slide.value;
}

function toggleEnabled(rangeName) {
    var ele = document.getElementById(rangeName);
    if (ele.disabled) {
        ele.disabled = false;
    }
    else {
        ele.disabled = true;
    }
}

function toggleReadOnly(timeRuleCheck) {
    var ele = document.getElementById(timeRuleCheck);
    if (timeRuleCheck.checked) {
        document.getElementById("timeStart").readOnly = false;
        document.getElementById("timeEnd").readOnly = false;
    }
    else {
        document.getElementById("timeStart").readOnly = true;
        document.getElementById("timeEnd").readOnly = true;
    }
}