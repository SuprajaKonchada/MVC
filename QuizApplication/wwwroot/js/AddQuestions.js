$(document).ready(function () {
    // Handle radio button change event
    $('input[type="radio"]').change(function () {
        if ($(this).val() === 'create') {
            $('#createForm').show();
            $('#uploadForm').hide();
        } else if ($(this).val() === 'upload') {
            $('#createForm').hide();
            $('#uploadForm').show();
        }
    });
});
function displayFileName(input) {
    const fileName = input.files[0].name;
    const label = input.parentElement.querySelector('.custom-file-label');
    label.innerText = fileName;
}

