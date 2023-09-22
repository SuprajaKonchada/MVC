$(document).ready(function () {
    $("#instruction").click(function () {
        // Perform your action by making an AJAX request
        var button = $(this);
        var modalId = button.data("target");
        //var popupContent = $(modalId).find(".popupContent");
        //console.log("Clicked course Id: " + courseId);
        $(modalId).modal("show");
    });
});