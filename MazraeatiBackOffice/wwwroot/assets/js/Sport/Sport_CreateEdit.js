// ============================================================
// Base URL للـ API (حسب البيئة)
// ============================================================
var BaseUrl = window.location.hostname === 'localhost'
    ? 'http://localhost:62550'
    : 'http://5.189.180.190/MazraeatiBackOffice';


// ============================================================
// عند تحميل الصفحة (في Edit)
// ============================================================
$(document).ready(function () {
    // جلب الـ selectedRegionId من الـ input المخفي
    var selectedRegionId = $('#hdnRegionId').val();
    var cityId = $('#CityId').val();

    if (cityId) {
        loadRegions(cityId, selectedRegionId);
    }

    // ===== عند تغيير المدينة =====
    $('#CityId').change(function () {
        var cityId = $(this).val();
        var selectedRegionId = $('#hdnRegionId').val();
        loadRegions(cityId, selectedRegionId);
    });


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

    // ===== معاينة الصور =====
    $('input[name="Images"]').on('change', function () {
        var files = this.files;
        var previewContainer = $('#imagePreviews');
        previewContainer.empty();
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var imgElement = $('<div style="display:inline-block;margin:5px;position:relative;">');
                    var img = $('<img>').attr('src', e.target.result).css({ 'width': '150px', 'height': '100px', 'object-fit': 'cover', 'border-radius': '5px', 'border': '1px solid #ddd' });
                    imgElement.append(img);
                    previewContainer.append(imgElement);
                };
                reader.readAsDataURL(files[i]);
            }
        }
    });

    // ===== معاينة الفيديوهات =====
    $('input[name="Videos"]').on('change', function () {
        var files = this.files;
        var previewContainer = $('#videoPreviews');
        previewContainer.empty();
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var videoElement = $('<div style="display:inline-block;margin:5px;position:relative;">');
                    var video = $('<video controls style="width:200px;border-radius:5px;border:1px solid #ddd;">');
                    var source = $('<source>').attr('src', e.target.result);
                    video.append(source);
                    videoElement.append(video);
                    previewContainer.append(videoElement);
                };
                reader.readAsDataURL(files[i]);
            }
        }
    });


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


// ============================================================
// دالة جلب المناطق حسب المدينة
// ============================================================
function loadRegions(cityId, selectedRegionId) {
    if (!cityId) {
        $('#RegionId').empty().append('<option value="">-- الرجاء اختيار المنطقة --</option>');
        return;
    }

    $.ajax({
        url: BaseUrl + '/Sports/GetRegionsByCityId',
        type: 'GET',
        data: { cityId: cityId },
        dataType: 'json',
        success: function (response) {
            var select = $('#RegionId');
            select.empty();
            select.append('<option value="">-- الرجاء اختيار المنطقة --</option>');

            // تأكد من أن response هي array
            var regions = Array.isArray(response) ? response : (response.data || response.d || []);

            if (regions.length === 0) {
                select.append('<option value="" disabled>لا توجد مناطق</option>');
            }

            $.each(regions, function (index, region) {
                var id = region.id;
                var name = region.descAr || region.descEn;

                if (id && name) {
                    var selected = (id == selectedRegionId) ? 'selected' : '';
                    select.append('<option value="' + id + '" ' + selected + '>' + name + '</option>');
                }
            });
        },
        error: function (xhr, status, error) {
            console.log('Error loading regions:', error);
            $('#RegionId').empty().append('<option value="">-- خطأ في تحميل المناطق --</option>');
        }
    });
}
function deleteSportImage(imageId) {
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
            $.post(`${BaseUrl}/Sports/DeleteSportImage`, { id: imageId }, function (response) {
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
function deleteSportVideo(videoId) {
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
            $.post(`${BaseUrl}/Sports/DeleteSportVideo`, { id: videoId }, function (response) {
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


