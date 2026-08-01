// --- Global Variables (Accessible throughout the script) ---
const $imageInput = $('#formFile'); // Your file input field
const $imagePreviewArea = $('#imagePreviewArea'); // Div where preview will be shown
const $noImageText = $('#noImageText'); // Text to show when no image is selected

// Helper function for SweetAlerts (from previous discussions - ensure this function is defined if you use it)
function showSweetAlert(icon, title, text) {
    if (typeof Swal !== 'undefined') { // Check if SweetAlert2 is loaded
        Swal.fire({
            icon: icon,
            title: title,
            text: text,
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
    } else {
        console.warn('SweetAlert2 is not loaded. Alerting:', title, text);
        alert(title + '\n' + text);
    }
}


// Function to display an image in the preview area
function displayImagePreview(src, isExisting = false) {
    $imagePreviewArea.empty(); // Clear existing content
    $noImageText.hide(); // Hide "No image selected" text

    const previewItem = `
                <div class="preview-item">
                    <img src="${src}" alt="Image Preview">
                    <button type="button" class="remove-preview-btn" ${isExisting ? 'data-existing="true"' : ''}>X</button>
                </div>
            `;
    $imagePreviewArea.append(previewItem);
}


// --- Document Ready / jQuery Initialization ---
$(function () {

    // --- Initial setup for Edit View on page load ---
    // Check if an existing image is rendered by Razor on page load
    // This targets the div created by Razor if Model.Image exists
    if ($imagePreviewArea.find('.existing-image-preview').length > 0) {
        $noImageText.hide(); // Hide "No image selected" text if an existing image is present
    } else {
        $noImageText.show(); // Show "No image selected" if no existing image (e.g., Create view or Edit with no image)
    }


    // --- Event Listener for File Input Change ---
    $imageInput.on('change', function (e) {
        const file = e.target.files[0]; // Get the first selected file
        if (file) {
            // Basic validation for image type
            if (!file.type.startsWith('image/')) {
                if (typeof showSweetAlert === 'function') {
                    showSweetAlert('warning', 'نوع ملف غير مدعوم', 'الرجاء اختيار ملف صورة (مثل .jpg, .png, .gif).');
                } else {
                    console.warn('Invalid file type selected. Please choose an image file.');
                }
                $imageInput.val(''); // Clear the selected file in the input
                $imagePreviewArea.empty(); // Clear preview area
                $noImageText.show(); // Show "No image selected" text
                return;
            }

            const reader = new FileReader();
            reader.onload = function (e) {
                displayImagePreview(e.target.result); // Display the new image preview (data URL)
            };
            reader.readAsDataURL(file); // Read the file as a data URL for preview
        } else {
            // If no file is selected (e.g., user opens dialog and cancels or clears input)
            $imagePreviewArea.empty(); // Clear current preview

            // For Edit view, if there was an existing image, re-display it
            // For Create view, or if no existing image, show "No image selected"
            // Ensure this URL is correctly resolved by Razor when the page is rendered
            const existingImageUrl = '@(Model.Image != null ? Url.Content("~/Images/" + Model.Image) : "")'; // Corrected path
            if (existingImageUrl && existingImageUrl !== '') {
                displayImagePreview(existingImageUrl, true);
            } else {
                // For Create view, or if no original image, show "No image selected" text
                $noImageText.show();
            }
        }
    });

    // --- Event Listener for "Remove Preview" Button ---
    // Using event delegation because the button is added dynamically
    $(document).on('click', '.remove-preview-btn', function () {
        const $button = $(this);
        const isExistingImage = $button.data('existing');

        $imagePreviewArea.empty(); // Clear the current preview content
        $imageInput.val(''); // Clear the file input field (important for submission)

        // If it was an existing image's preview, the hidden input will carry its path.
        // We just ensure the UI looks correct: "no image selected"
        $noImageText.show();
    });

}); // End of $(function() { ... });