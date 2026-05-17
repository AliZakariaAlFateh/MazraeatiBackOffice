$(function () {
    var animationPlayedKey = 'eidAnimationPlayed'; // Key for localStorage
    var $runningLamb = $('#runningLamb');
    var eidTakbiratAudio = $('#eidTakbirat')[0]; // Get the native DOM audio element
    var marquee = $("#marqueeBox")
    // Define the audio playback duration: 1 minute and 50 seconds
    var audioPlayDuration = (1 * 60 + 50) * 1000; // Convert to milliseconds (110 seconds * 1000 = 110000 ms)

    // --- NEW: Date Check Logic ---
    var today = new Date();
    // Set the cutoff date to June 8, 2025.
    // Note: Month is 0-indexed, so 5 is June.
    var cutoffDate = new Date(2025, 5, 8); // Year, Month (0-11), Day

    // Check if today's date is on or after the cutoff date
    if (today.getTime() >= cutoffDate.getTime()) {
        console.log("Eid al-Adha animation and audio are now deactivated as of " + today.toLocaleDateString() + ".");
        //marquee.css('display', 'none')
         
        // Ensure image is not displayed and audio is stopped if it was somehow playing
        if ($runningLamb.length) {
            $runningLamb.css('display', 'none');
            $("#TextForEid").text('')
        }
        if (eidTakbiratAudio && !eidTakbiratAudio.paused) {
            eidTakbiratAudio.pause();
            eidTakbiratAudio.currentTime = 0;
        }
        // Exit the function, preventing any further execution of the animation/audio logic
        return;
    }
    // --- END NEW: Date Check Logic ---


        


    // The rest of your existing code now runs ONLY if the date check passes
    // (i.e., if today's date is BEFORE June 10, 2025)

    // Check if the animation has been played before using localStorage
    if (localStorage.getItem(animationPlayedKey) === null) {
        // If not played, set the flag in localStorage
        localStorage.setItem(animationPlayedKey, 'true');

        // --- Play Audio ---
        if (eidTakbiratAudio) {
            // Attempt to play audio, handling potential autoplay policy blocks
            eidTakbiratAudio.play().then(function () {
                console.log("Eid Takbirat audio started successfully.");

                // Set a timeout to pause the audio after 1 minute and 50 seconds
                setTimeout(function () {
                    if (eidTakbiratAudio && !eidTakbiratAudio.paused) {
                        eidTakbiratAudio.pause();
                        eidTakbiratAudio.currentTime = 0; // Optional: Reset audio to the beginning
                        console.log("Eid Takbirat audio paused after 1 minute 50 seconds.");
                    }
                }, audioPlayDuration);

            }).catch(function (error) {
                console.warn("Audio autoplay blocked or failed:", error);
                // You might inform the user or provide a button to play if blocked.
            });
        }

        // --- Animate the Lamb ---
        if ($runningLamb.length) { // Ensure the lamb element exists
            // Get the width of the viewport to determine animation range
            var viewportWidth = $(window).width();

            // Determine the vertical position of the lamb.
            // We'll try to position it just below the main header "لوحة التحكم".
            var topPosition = 0; // Default top position if header isn't found
  

            // Set initial position and display the lamb
            $runningLamb.css({
                'left': '-200px', // Start off-screen to the left (adjust -200px if your lamb is wider)
                'top': topPosition + 'px', // Set vertical position
                'height': '120px', // Set a reasonable height for the lamb (adjust as needed)
                'display': 'block' // Make the lamb visible
            });

            // Animate the lamb across the screen
            // The animation duration (5 seconds) is independent of the audio duration
            $runningLamb.animate({
                left: viewportWidth + 'px' // Move off-screen to the right
            }, 30000, 'linear', function () { // Changed back to 5000ms (5 seconds) as per initial request for animation duration
                // Animation complete callback: hide the lamb
                $(this).hide();
            });
        }
    }
});