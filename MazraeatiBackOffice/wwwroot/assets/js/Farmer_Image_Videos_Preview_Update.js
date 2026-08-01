let BaseUrl_Dev = 'http://localhost:62550'
let BaseUrl_Pro = 'http://5.189.180.190/MazraeatiBackOffice'


var BaseUrl = window.location.hostname === 'localhost'
    ? 'http://localhost:62550'
    : 'http://5.189.180.190/MazraeatiBackOffice';


$(function () {
    //start for region
    debugger



    //this for select2
    $("#RegionId").on("change", function () {
        var $selected = $(this).find("option:selected");

        var descAr = $selected.data("descar") || "";
        var descEn = $selected.data("descen") || "";

        $("#LocationDesc").val(descAr);
        $("#LocationDescEn").val(descEn);
    });
    ////
    var initialCity = $("#CityId").val();
    //var initialRegion = '@Model.RegionId'; 

    if (initialCity) {
        $("#CityId").trigger("change");
    }
    //for region



    //$(".delete-image-btn").onclick(function () {
    //    $(this).data("imageid")
    //})
    //Alert if I can Use it.....

    //function showSweetAlert(icon, title, text) {
    //    Swal.fire({
    //        icon: icon,
    //        title: title,
    //        text: text,
    //        customClass: {
    //            confirmButton: 'btn btn-primary'
    //        },
    //        buttonsStyling: false
    //    });
    //}


  


    //for handle Preview Image and Video 
    // Handle Image Previews
    $('#Images').on('change', function (e) {
        $('#imagePreviews').empty(); // Clear previous previews
        const files = e.target.files;
        if (files.length === 0) return;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            if (!file.type.startsWith('image/')) {
                continue; // Skip non-image files
            }

            const reader = new FileReader();
            reader.onload = function (e) {
                const previewItem = `
                            <div class="farmer-image-item preview-item">
                                <img src="${e.target.result}" alt="Image Preview">
                                <button type="button" class="delete-image-btn remove-preview-btn" data-index="${i}">X</button>
                            </div>
                        `;
                $('#imagePreviews').append(previewItem);
            };
            reader.readAsDataURL(file);
        }
    });

    // Handle Video Previews
    $('#Videos').on('change', function (e) {
        $('#videoPreviews').empty(); // Clear previous previews
        const files = e.target.files;
        if (files.length === 0) return;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            if (!file.type.startsWith('video/')) {
                continue; // Skip non-video files
            }

            const reader = new FileReader();
            reader.onload = function (e) {
                const previewItem = `
                            <div class="farmer-video-item preview-item">
                                <video controls>
                                <source src="${e.target.result}" type="${file.type}">
                                Your browser does not support the video tag.
                            </video>
                            <button type="button" class="delete-image-btn remove-preview-btn" data-index="${i}">X</button>
                        </div>
                    `;
                $('#videoPreviews').append(previewItem);
            };
            reader.readAsDataURL(file);
        }
    });





    // alertHandler.js
    //$(document).ready(function () {




        const $alertElement = $('#errorMessageAlert');

        if ($alertElement.length) { // Check if the alert element exists on the page
            // Set a timeout to fade out and remove the alert after 5 seconds
            setTimeout(function () {
                // Use Bootstrap's built-in 'hide' method for alerts, or jQuery's fadeOut
                // Using Bootstrap's method ensures proper handling of the 'fade' class.
                $alertElement.alert('close'); // This triggers the fade-out (if 'fade' class is present) and removes the element.

                // If .alert('close') doesn't fully remove it from DOM immediately
                // or you want a specific custom fadeOut, you can use:
                // $alertElement.fadeOut(600, function() { // Fade out over 600ms
                //     $(this).remove(); // Remove the element from the DOM after fadeOut completes
                // });

            }, 5000); // 5000 milliseconds = 5 seconds
        }
    /*});*/






    // Handle removal of previewed items
    $(document).on('click', '.remove-preview-btn', function () { // Keep '.remove-preview-btn' for event delegation
        const $itemToRemove = $(this).closest('.preview-item');
        const indexToRemove = $(this).data('index'); // This identifies the position in the original file list

        // Find the parent container (image or video previews)
        const $parentContainer = $itemToRemove.parent();
        const inputId = $parentContainer.attr('id') === 'imagePreviews' ? 'Images' : 'Videos';
        const fileInput = document.getElementById(inputId);

        // Remove the item from the displayed previews
        $itemToRemove.remove();

        // IMPORTANT: Create a new DataTransfer object to modify the file list
        const dataTransfer = new DataTransfer();
        const currentFiles = Array.from(fileInput.files);

        currentFiles.forEach((file, index) => {
            if (index !== indexToRemove) {
                dataTransfer.items.add(file);
            }
        });

        // Update the file input's files property
        fileInput.files = dataTransfer.files;

        // Re-index remaining preview buttons if necessary (for visual consistency,
        // though the core logic relies on the DataTransfer object)
        $parentContainer.find('.remove-preview-btn').each(function (idx) {
            $(this).data('index', idx);
        });
    });









    //For SelectOption Of Region and City


    var BaseUrl = window.location.hostname === 'localhost'
        ? 'http://localhost:62550'
        : 'http://5.189.180.190/MazraeatiBackOffice';

    // ============================================================
    // 2. REGION LOADER - Dynamic Dropdown Based on City Selection
    // ============================================================
    // When a city is selected, this function fetches associated regions
    // from the server and populates the region dropdown.
    // ============================================================

    /**
     * IMPORTANT: Get the selected RegionId from server-side ViewBag
     * This value is rendered into the page by ASP.NET MVC
     * and used to preselect the correct region after loading
     */
    var selectedCityId = $('#CityId').val();        // Current city ID from dropdown
    var selectedRegionId = $('#SelectedRegionId').val() || '0';     // ✅ Preselected region ID from ViewBag

    // --- Event Handler: City Dropdown Change ---
    $("#CityId").on("change", function () {
        var cityId = $(this).val();
        var $region = $("#RegionId");

        // Reset and disable region dropdown while loading
        $region.empty()
            .append('<option selected disabled>--الرجاء اختيار المنطقة--</option>')
            .prop("disabled", true);
        $region.trigger('change');

        // Exit if no city is selected
        if (!cityId || cityId === "0") return;

        // --- AJAX Request: Fetch Regions by City ID ---
        $.ajax({
            url: BaseUrl + '/Farmers/GetRegionsByCityId',
            type: 'GET',
            data: { cityId: cityId },
            success: function (data) {
                // Reset dropdown with placeholder option
                $region.empty().append($('<option/>', {
                    text: '--الرجاء اختيار المنطقة--',
                    disabled: true,
                    selected: true
                }));

                // Populate regions if data exists
                if (data && data.length > 0) {
                    $.each(data, function (i, item) {
                        var $opt = $('<option/>', {
                            value: item.id,
                            text: item.descAr
                        });
                        // Add data attributes for potential future use
                        $opt.attr("data-descar", item.descAr);
                        $opt.attr("data-descen", item.descEn);
                        $region.append($opt);
                    });

                    // Enable the dropdown
                    $region.prop("disabled", false);
                }

                // Trigger change event for any dependent logic
                $region.trigger('change');

                /**
                 * ✅ CRITICAL: Preselect the region from ViewBag
                 * After loading regions, check if we have a preselected region ID
                 * from the ViewBag and select it automatically.
                 * This ensures the previously saved region is shown correctly.
                 */
                if (selectedRegionId && selectedRegionId !== "0") {
                    $region.val(selectedRegionId);
                    $region.trigger('change');

                    // Reset the variable to prevent re-selection on subsequent manual changes
                    selectedRegionId = "0";
                }
            },
            error: function () {
                console.error("خطأ في جلب المناطق");
                Swal.fire({
                    icon: 'error',
                    title: 'خطأ',
                    text: 'حدث خطأ أثناء تحميل المناطق. يرجى المحاولة مرة أخرى.'
                });
            }
        });
    });

    // --- Auto-trigger on page load if city is preselected ---
    if (selectedCityId && selectedCityId !== "0") {
        $("#CityId").trigger('change');
    }

    // ============================================================
    // 3. INTERNATIONAL PHONE VALIDATION
    // ============================================================
    // Initializes intl-tel-input for phone number validation
    // Supports multiple countries with auto-detection
    // ============================================================

    /**
     * Initialize international telephone input
     * @param {string} inputId - CSS selector for the input element
     * @returns {object} intlTelInput instance
     */
    

    // --- Initialize Phone Input ---
    var iti1 = initPhone("#phone1");

    // ============================================================
    // 4. FORM SUBMISSION HANDLER
    // ============================================================
    // Validates phone number before form submission
    // Prevents submission if phone number is invalid
    // ============================================================

    $("form").on("submit", function () {
        // Validate phone number before submitting
        if (!iti1.isValidNumber()) {
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'رقم الموبايل غير صحيح. يرجى إدخال رقم صحيح مع مفتاح الدولة.'
            });
            return false; // Prevent form submission
        }

        // Update hidden input with full international number
        $("#phone1").val(iti1.getNumber());

        // Form will submit normally
        return true;
    });

    // ============================================================
    // 5. CLEANUP AND ADDITIONAL FEATURES (Optional)
    // ============================================================
    // Handle any additional UI interactions or cleanup tasks
    // ============================================================

    // Example: Reset region dropdown when city is cleared
    // You can add additional functionality here as needed






    // ===== تحديث الـ Person لكل الأيام في نفس المجموعة =====
    // لما يتغير الـ Person في أي TextBox
    $('input[type="number"][id^="person_"]').on('change', function () {
        var newPerson = $(this).val();

        // لو القيمة فاضية، اخرج
        if (newPerson === '') {
            return;
        }

        // خد الصف الحالي (اللي فيه الـ Person)
        var parentRow = $(this).closest('tr');

        // خد كل الصفوف اللي بعد الصف الحالي لحد ما نوصل لصف جديد بنفس الـ Person
        var nextRows = parentRow.nextUntil('tr[style*="background:#e9f7ef"]');

        // غير الـ Hidden في الصف الحالي
        parentRow.find('input.person-hidden').val(newPerson);

        // غير الـ Hidden في الصفوف التالية (كل الأيام)
        nextRows.each(function () {
            $(this).find('input.person-hidden').val(newPerson);
        });
    });

    // ===== عند إرسال الفورم، اتأكد من تطابق الـ Person =====
    $('form').on('submit', function () {
        var allPersonHidden = $(this).find('input.person-hidden');

        // لو في أي Hidden مختلف، غير الكل لأول قيمة
        if (allPersonHidden.length > 0) {
            var firstValue = allPersonHidden.first().val();
            allPersonHidden.each(function () {
                $(this).val(firstValue);
            });
        }
    });


});


// ===== دالة لحذف قائمة أسعار =====
function DeletePriceList_Full(farmerId) {
    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: "سيتم حذف جميع الأسعار الخاصة بهذه المزرعة!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'نعم، احذف',
        cancelButtonText: 'إلغاء'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'جاري الحذف...',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => Swal.showLoading()
            });

            $.ajax({
                url: BaseUrl + '/Farmers/DeletePriceList_Full',
                type: 'POST',
                data: { farmerId: farmerId },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            confirmButtonText: 'حسناً'
                        }).then(() => location.reload());
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'حدث خطأ',
                            text: response.message || 'فشل الحذف',
                            confirmButtonText: 'حسناً'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'خطأ في الاتصال',
                        text: 'حدث خطأ في الاتصال بالخادم',
                        confirmButtonText: 'حسناً'
                    });
                }
            });
        }
    });
}


function initPhone(inputId) {
    var input = document.querySelector(inputId);

    var iti = window.intlTelInput(input, {
        initialCountry: "auto",
        geoIpLookup: function (callback) {
            $.get("https://ipapi.co/json/", function (data) {
                callback(data.country_code.toLowerCase());
            }).fail(function () {
                callback("eg"); // Fallback to Egypt if geo lookup fails
            });
        },
        separateDialCode: true,
        preferredCountries: ["eg", "sa", "ae", "jo"], // Egypt, Saudi Arabia, UAE, Jordan
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
    });

    // Set existing value if present (e.g., from database)
    var existingValue = $(inputId).val();
    if (existingValue) {
        iti.setNumber(existingValue);
    }

    return iti;
}






//For Delete Price List ....
function DeletePriceList(farmerId, personNumber) {
    if (confirm("هل انت متأكد من انك تريد الغاء هذا السجل")) {
        $.post('@Url.Content("~/Farmers/DeletePriceList")', { person: personNumber, farmerId: farmerId }, function (result) {
            if (result == 1) {
                alert(" تم حذف الفترة ");
                window.location.reload();
            } else {
                alert(" لم يتم حذف الفترة ");
            }
        });
    }
}


//Delete Image 


// --- Global Utility Functions (assuming showSweetAlert is already defined and accessible) ---

// Your existing showSweetAlert function (make sure it's globally accessible if used elsewhere)
function showSweetAlert(icon, title, text) {
    if (typeof Swal !== 'undefined') {
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


// --- Modified DeleteFarmerImage Function with SweetAlert Confirmation ---
function DeleteFarmerImage(imageId) {
    debugger
    // Use SweetAlert2 for confirmation
    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد أنك تريد حذف هذه الصورة؟ لن تتمكن من التراجع عن هذا!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33', // Red color for delete confirmation
        cancelButtonColor: '#3085d6', // Blue color for cancel
        confirmButtonText: 'نعم، احذفها!',
        cancelButtonText: 'إلغاء',
        customClass: {
            confirmButton: 'btn btn-danger mx-1', // Apply custom styling if using Bootstrap or similar
            cancelButton: 'btn btn-secondary mx-1'
        },
        buttonsStyling: false // Disable default styling if using custom classes
    }).then((result) => {
        // If user confirms (clicks "نعم، احذفها!")
        if (result.isConfirmed) {
            debugger // Keep for debugging if needed
            console.log("Attempting to delete image with ID:", imageId);

            // AJAX call to delete the image
            $.post(`${BaseUrl}/Farmers/DeleteFarmerImage`, { imageId: imageId }, function (response) {
                if (response.success) {
                    console.log("تم حذف الصورة بنجاح:", response.message);
                    // Remove the element directly for better UX
                    $(`button[data-imageid="${imageId}"]`).closest('.farmer-image-item').remove();
                    // Show a success alert
                    showSweetAlert('success', 'حذف ناجح!', 'تم حذف الصورة بنجاح.');
                } else {
                    console.error("فشل حذف الصورة:", response.message);
                    // Show an error alert
                    showSweetAlert('error', 'فشل الحذف!', 'لم يتم حذف الصورة: ' + response.message);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error deleting image:", textStatus, errorThrown, jqXHR.responseText);
                // Show an error alert for AJAX failure
                showSweetAlert('error', 'خطأ في الاتصال!', 'حدث خطأ أثناء الاتصال بالخادم. يرجى المحاولة مرة أخرى.');
            });
        } else {
            console.log("Image deletion cancelled by user.");
            // Optional: show a small info alert that deletion was cancelled
            // showSweetAlert('info', 'تم الإلغاء', 'تم إلغاء عملية الحذف.');
        }
    });
}


// --- Modified DeleteFarmerVideo Function with SweetAlert Confirmation ---
function DeleteFarmerVideo(videoId) {
    // Use SweetAlert2 for confirmation
    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد أنك تريد حذف هذا الفيديو؟ لن تتمكن من التراجع عن هذا!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33', // Red color for delete confirmation
        cancelButtonColor: '#3085d6', // Blue color for cancel
        confirmButtonText: 'نعم، احذفه!',
        cancelButtonText: 'إلغاء',
        customClass: {
            confirmButton: 'btn btn-danger mx-1',
            cancelButton: 'btn btn-secondary mx-1'
        },
        buttonsStyling: false
    }).then((result) => {
        // If user confirms (clicks "نعم، احذفه!")
        if (result.isConfirmed) {
            debugger // Keep for debugging if needed
            console.log("Attempting to delete video with ID:", videoId);

            // AJAX call to delete the video
            $.post(`${BaseUrl}/Farmers/DeleteFarmerVideo`, { videoId: videoId }, function (response) {
                if (response.success) {
                    console.log("تم حذف الفيديو بنجاح:", response.message);
                    // Remove the element directly for better UX
                    $(`button[data-videoid="${videoId}"]`).closest('.farmer-video-item').remove();
                    // Show a success alert
                    showSweetAlert('success', 'حذف ناجح!', 'تم حذف الفيديو بنجاح.');
                } else {
                    console.error("فشل حذف الفيديو:", response.message);
                    // Show an error alert
                    showSweetAlert('error', 'فشل الحذف!', 'لم يتم حذف الفيديو: ' + response.message);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error deleting Video:", textStatus, errorThrown, jqXHR.responseText);
                // Show an error alert for AJAX failure
                showSweetAlert('error', 'خطأ في الاتصال!', 'حدث خطأ أثناء الاتصال بالخادم. يرجى المحاولة مرة أخرى.');
            });
        } else {
            console.log("Video deletion cancelled by user.");
            // Optional: show a small info alert that deletion was cancelled
            // showSweetAlert('info', 'تم الإلغاء', 'تم إلغاء عملية الحذف.');
        }
    });
}


