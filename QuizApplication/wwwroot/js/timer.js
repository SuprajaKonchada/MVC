// Function to update the remaining time and automatically click the "End Exam" button
function updateTimer() {
    // Get a reference to the countdown element
    let countdownElement = document.getElementById('countdown');

    // Retrieve the initial category duration from ViewBag and convert it to milliseconds
    let initialQuestionDuration = parseInt('@ViewBag.QuestionDuration') * 1000; // Convert to milliseconds

    // Calculate the remaining time in milliseconds
    let remainingTime = initialQuestionDuration - elapsedMilliseconds;

    // Function to update the countdown
    function updateCountdown() {
        let seconds = Math.floor(remainingTime / 1000);
        let minutes = Math.floor(seconds / 60);
        seconds %= 60;

        // Display the remaining time in the format MM:SS
        countdownElement.textContent = `Time remaining: ${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;

        // Decrement the remaining time by 1 second
        remainingTime -= 1000;

        // If the remaining time reaches 0, click the "End Exam" button
        if (remainingTime < 0) {
            clearInterval(timerInterval); // Stop the countdown
            autoSubmit();
        }
    }

    // Initial update
    updateCountdown();

    // Update the countdown every second
    let timerInterval = setInterval(updateCountdown, 1000);
}

// Function to automatically click the "End Exam" button
function autoSubmit() {
    // Get a reference to the "End Exam" button
    let endSubmit = document.getElementById('submitBtn');

    // Programmatically click the button
    endSubmit.click();
}
// Calculate the elapsed time when the page loads
let elapsedMilliseconds = 0;
let initialTime = new Date();

// Add an event listener for form submission to calculate elapsed time
let formElement = document.querySelector('form');
formElement.addEventListener('submit', function () {
    let currentTime = new Date();
    elapsedMilliseconds += currentTime - initialTime;
});

// Set a timeout to trigger the updateTimer function after a brief delay
setTimeout(updateTimer, 100); // Delay by 100msec to start the timer
