let BaseUrl_Dev = 'http://localhost:62550'
let BaseUrl_Pro ='http://5.189.180.190/MazraeatiBackOffice'
$(function () {
    //start for region
    debugger



    //function fillLocationsFromSelectedRegion() {
    //    var $selected = $("#RegionId").find("option:selected");
    //    var descAr = $selected.data("descar") || "";
    //    var descEn = $selected.data("descen") || "";

    //    $("#LocationDesc").val(descAr);
    //    $("#LocationDescEn").val(descEn);
    //}

    //$("#CityId").on("change", function () {
    //    var cityId = $(this).val();
    //    var $region = $("#RegionId");

    //    // Reset
    //    $region.empty().append(
    //        $('<option/>', { text: '--الرجاء اختيار المنطقة--', disabled: true, selected: true })
    //    ).prop("disabled", true);

    //    if (!cityId) {
    //        $region.selectpicker('refresh');
    //        return;
    //    }

    //    $.ajax({
    //        url: `${BaseUrl_Dev}/Farmers/GetRegionsByCityId`,
    //        type: 'GET',
    //        data: { cityId: cityId },
    //        success: function (data) {

    //            $region.empty().append(
    //                $('<option/>', { text: '--الرجاء اختيار المنطقة--', disabled: true })
    //            );

    //            if (data && data.length > 0) {
    //                $.each(data, function (i, item) {
    //                    var $opt = $('<option/>', {
    //                        value: item.id,
    //                        text: item.descAr
    //                    });

    //                    // data-*
    //                    $opt.attr("data-descar", item.descAr);
    //                    $opt.attr("data-descen", item.descEn);

    //                    $region.append($opt);
    //                });

    //                $region.prop("disabled", false);
    //            } else {
    //                $region.append($('<option/>', { text: 'لا يوجد مناطق لهذه المدينة', disabled: true }));
    //            }

    //            // ✅ لو Edit: اعمل Selected بعد ما options اتضافت
    //            if (window.editRegionId && window.editRegionId > 0) {
    //                // الأفضل مع bootstrap-select
    //                $region.selectpicker('val', String(window.editRegionId));

    //                // احتياطي لو val فوق فشل لأي سبب
    //                if (!$region.val()) {
    //                    $region.val(String(window.editRegionId));
    //                }

    //                // املى LocationDesc/En
    //                fillLocationsFromSelectedRegion();
    //            }

    //            // 🔥 مهم جدًا
    //            $region.selectpicker('refresh');
    //        },
    //        error: function () {
    //            Swal.fire({
    //                icon: 'error',
    //                title: 'خطأ',
    //                text: 'حدث خطأ أثناء تحميل المناطق'
    //            });
    //        }
    //    });
    //});

    //// Event بتاع bootstrap-select (اختيار المستخدم)
    //$("#RegionId").on("changed.bs.select", function () {
    //    fillLocationsFromSelectedRegion();
    //});

    //// ✅ عند فتح صفحة Edit: حمّل المناطق واختر Region تلقائي
    //if (window.editCityId && window.editCityId > 0) {
    //    $("#CityId").val(String(window.editCityId)).trigger("change");
    //}



    //On Change City ...
    //$("#CityId").on("change", function () {
    //    debugger
    //    var cityId = $(this).val();
    //    var $region = $("#RegionId");

    //    // Reset
    //    $region.empty()
    //        .append('<option selected disabled>--الرجاء اختيار المنطقة--</option>')
    //        .prop("disabled", true);

    //    if (!cityId) return;
    //    debugger
    //    $.ajax({
    //        url: `${BaseUrl_Dev}/Farmers/GetRegionsByCityId`,
    //        type: 'GET',
    //        data: { cityId: cityId },
    //        success: function (data) {
    //            debugger
    //            var $region = $("#RegionId");

    //            $region.empty().append(
    //                $('<option/>', {
    //                    text: '--الرجاء اختيار المنطقة--',
    //                    disabled: true,
    //                    selected: true
    //                })
    //            );

    //            if (data && data.length > 0) {
    //                //$region.append('<option value="' + item.id + '">' + item.descAr + '</option>');
    //                $.each(data, function (i, item) {

    //                    var $opt = $('<option/>', {
    //                        value: item.id,
    //                        text: item.descAr
    //                    });

    //                    debugger
    //                    $opt.attr("data-descar", item.descAr);
    //                    $opt.attr("data-descen", item.descEn);

    //                    $region.append($opt);
    //                });

    //                $region.prop("disabled", false);
    //            }

    //            // 🔥 مهم جدًا مع bootstrap-select
    //            $region.selectpicker('refresh');
    //        },
    //        error: function () {
    //            // Optional: SweetAlert
    //            Swal.fire({
    //                icon: 'error',
    //                title: 'خطأ',
    //                text: 'حدث خطأ أثناء تحميل المناطق'
    //            });
    //        }
    //    });
    //});
    //On Change City ...















    //second or third code comented ...
    //$("#CityId").on("change", function () {
    //    var cityId = $(this).val();
    //    var $region = $("#RegionId");

    //    // Reset
    //    $region.empty()
    //        .append('<option selected disabled>--الرجاء اختيار المنطقة--</option>')
    //        .prop("disabled", true)
    //        .selectpicker('refresh'); // تحديث الشكل فوراً

    //    if (!cityId) return;

    //    $.ajax({
    //        url: `${BaseUrl_Dev}/Farmers/GetRegionsByCityId`,
    //        type: 'GET',
    //        data: { cityId: cityId },
    //        success: function (data) {
    //            var $region = $("#RegionId");
    //            $region.empty().append(
    //                $('<option/>', {
    //                    text: '--الرجاء اختيار المنطقة--',
    //                    disabled: true,
    //                    selected: true
    //                })
    //            );

    //            if (data && data.length > 0) {
    //                $.each(data, function (i, item) {
    //                    var $opt = $('<option/>', {
    //                        value: item.id,
    //                        text: item.descAr
    //                    });
    //                    $opt.attr("data-descar", item.descAr);
    //                    $opt.attr("data-descen", item.descEn);
    //                    $region.append($opt);
    //                });
    //                $region.prop("disabled", false);
    //            }

    //            // تحديث المكتبة
    //            $region.selectpicker('refresh');

    //            // 🔥 الإضافة هنا: إطلاق حدث مخصص لإعلام الأكواد الأخرى بالانتهاء
    //            $region.trigger('regions:loaded');
    //        },
    //        error: function () {
    //            Swal.fire({
    //                icon: 'error',
    //                title: 'خطأ',
    //                text: 'حدث خطأ أثناء تحميل المناطق'
    //            });
    //        }
    //    });
    //});







    //start for region

    //set values in Locations for selectpicker
    //$("#RegionId").on("changed.bs.select", function () {
    //    var $selected = $(this).find("option:selected");
    //    var descAr = $selected.attr("data-descar") || "";
    //    var descEn = $selected.attr("data-descen") || "";

    //    $("#LocationDesc").val(descAr);
    //    $("#LocationDescEn").val(descEn);
    //});


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
    $(document).ready(function () {
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
    });






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









});


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
            $.post(`${BaseUrl_Pro}/Farmers/DeleteFarmerImage`, { imageId: imageId }, function (response) {
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
            $.post(`${BaseUrl_Pro}/Farmers/DeleteFarmerVideo`, { videoId: videoId }, function (response) {
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


