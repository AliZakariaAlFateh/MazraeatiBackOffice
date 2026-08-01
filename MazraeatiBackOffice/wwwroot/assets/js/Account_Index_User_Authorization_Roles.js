    debugger
        // تحديد الـ Base URL حسب البيئة
    let BaseUrl = window.location.hostname === 'localhost'
    ? 'http://localhost:62550'
    : 'http://5.189.180.190/MazraeatiBackOffice';

        // ============= Delete with AJAX =============
        $('.delete-ajax-btn').on('click',function (e) {
            e.preventDefault();

            var userId = $(this).data('user-id');
            var userName = $(this).data('user-name');
            var userFullName = $(this).data('user-fullname');

            Swal.fire({
                title: 'هل أنت متأكد؟',
                html: '<div style="text-align: right;">' +
                    '<p><strong>اسم المستخدم:</strong> ' + userName + '</p>' +
                    '<p><strong>الاسم الكامل:</strong> ' + userFullName + '</p>' +
                    '<p style="color: red;">⚠️ هذا الإجراء لا يمكن التراجع عنه!</p>' +
                    '</div>',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'نعم, احذف',
                cancelButtonText: 'إلغاء'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'جاري الحذف...',
                        allowOutsideClick: false,
                        didOpen: () => {
                            Swal.showLoading();
                        }
                    });

                    $.ajax({
                        url: BaseUrl + '/Account/Delete/' + userId,
                        type: 'POST',
                        data: {
                            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').first().val()
                        },
                        success: function (response) {
                            if (response.success) {
                                $('#row-' + userId).fadeOut(500, function () {
                                    $(this).remove();
                                });

                                Swal.fire({
                                    title: 'تم الحذف!',
                                    text: response.message,
                                    icon: 'success',
                                    confirmButtonText: 'حسناً',
                                    timer: 2000
                                });
                            } else {
                                Swal.fire({
                                    title: 'خطأ!',
                                    text: response.message,
                                    icon: 'error',
                                    confirmButtonText: 'حسناً'
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            Swal.fire({
                                title: 'خطأ!',
                                text: 'حدث خطأ أثناء الاتصال بالخادم',
                                icon: 'error',
                                confirmButtonText: 'حسناً'
                            });
                        }
                    });
                }
            });
        });

    // ============= Toggle Status with AJAX =============
    $('.toggle-status-form').on('submit', function(e) {
        e.preventDefault();

    var form = $(this);
    var userId = form.find('.toggle-status-btn').data('user-id');
    var currentButton = form.find('.toggle-status-btn');
    var currentText = currentButton.text().trim();

    Swal.fire({
        title: 'تغيير حالة المستخدم',
    text: 'هل أنت متأكد من ' + (currentText === 'تعطيل' ? 'تعطيل' : 'تفعيل') + ' هذا المستخدم؟',
    icon: 'question',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'نعم',
    cancelButtonText: 'إلغاء'
                }).then((result) => {
                    if (result.isConfirmed) {
        Swal.fire({
            title: 'جاري التغيير...',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
        debugger
        //BaseUrl+
    $.ajax({
    url: form.attr('action'),
    type: 'POST',
    data: form.serialize(),
    success: function(response) {
    if (response.success) {
        // ========== التعديل المهم هنا ==========
        // تحديث خلية الحالة فقط (Status Cell) مش الـ Roles
        // var statusCell = $('#row-' + userId).find('.status-cell-' + userId);
        // var toggleBtn = $('#row-' + userId).find('.toggle-status-btn');

        // if (response.isActive) {
        //     // تغيير إلى نشط
        //     statusCell.html('<span class="badge bg-success status-badge">نشط</span>');
        //     toggleBtn.text('تعطيل');
        // } else {
        //     // تغيير إلى غير نشط
        //     statusCell.html('<span class="badge bg-danger status-badge">غير نشط</span>');
        //     toggleBtn.text('تفعيل');
        // }
        var statusCell = $('#row-' + userId).find('.status-cell-' + userId);
    var toggleBtn = $('#row-' + userId).find('.toggle-status-btn');

    if (response.isActive) {
        statusCell.html('<span class="status-badge active">نشط</span>');
    toggleBtn.text('تعطيل');
                                    } else {
        statusCell.html('<span class="status-badge inactive">غير نشط</span>');
    toggleBtn.text('تفعيل');
                                    }

    // Add animation to the row
    $('#row-' + userId).addClass('row-updated');
    setTimeout(() => {
        $('#row-' + userId).removeClass('row-updated');
        }, 500);
    // تغيير لون الزر نفسه
    if (response.isActive) {
        toggleBtn.removeClass('btn-info').addClass('btn-warning');
                                    } else {
        toggleBtn.removeClass('btn-warning').addClass('btn-info');
                                    }

    Swal.fire({
        title: 'تم التغيير!',
    text: response.message,
    icon: 'success',
    confirmButtonText: 'حسناً',
    timer: 2000
      });
     } else {
        Swal.fire({
            title: 'خطأ!',
            text: response.message,
            icon: 'error',
            confirmButtonText: 'حسناً'
        });
                                }
                            },
    error: function(xhr, status, error) {
        Swal.fire({
            title: 'خطأ!',
            text: 'حدث خطأ أثناء تغيير الحالة',
            icon: 'error',
            confirmButtonText: 'حسناً'
        });
                            }
                        });
                    }
                });
    });










