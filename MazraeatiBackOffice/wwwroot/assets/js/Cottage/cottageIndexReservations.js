///* cottageReservations.js */

//// دالة تهيئة عامة بتستقبل الإعدادات من الـ View
//function initCottageReservations(config) {
//    var urls = config.urls;
//    var initialCottageId = config.cottageId;

//    $(document).ready(function () {
//        $('.select2').select2({
//            placeholder: "اختر من القائمة",
//            width: '100%'
//        });

//        // ===== عند تغيير الكوخ في الفلتر =====
//        $('#cottageFilter').change(function () {
//            var cottageId = $(this).val();
//            if (cottageId) {
//                window.location.href = urls.index + '?cottageId=' + cottageId;
//            } else {
//                window.location.href = urls.index;
//            }
//        });
//    });

//    // ============================================================
//    // SweetAlert Configuration
//    // ============================================================
//    const Toast = Swal.mixin({
//        toast: true,
//        position: 'top-end',
//        showConfirmButton: false,
//        timer: 3000,
//        timerProgressBar: true,
//        didOpen: (toast) => {
//            toast.addEventListener('mouseenter', Swal.stopTimer);
//            toast.addEventListener('mouseleave', Swal.resumeTimer);
//        }
//    });

//    // ============================================================
//    // تغيير حالة الحجز (مع الـ Popup للتأكيد)
//    // ============================================================
//    function changeStatus(id, status) {
//        var isConfirm = status === 2;
//        var title = isConfirm ? 'تأكيد الحجز' : 'إلغاء الحجز';
//        var icon = isConfirm ? 'question' : 'warning';
//        debugger
//        if (isConfirm) {
//            // ===== عرض الـ Popup للتأكيد =====
//            Swal.fire({
//                title: 'جاري تحميل بيانات الحجز...',
//                text: 'الرجاء الانتظار',
//                allowOutsideClick: false,
//                didOpen: function () {
//                    Swal.showLoading();
//                }
//            });

//            $.ajax({
//                url: urls.getConfirmData,
//                type: 'GET',
//                data: { id: id },
//                success: function (data) {
//                    Swal.close();

//                    if (typeof data === 'string' && data.trim().startsWith('{') === false) {
//                        debugger
//                        $('#popupContainer').html(data);
//                        $('#confirmReservationCottageModal').modal('show');
//                    } else if (typeof data === 'object' && data.success === false) {
//                        Swal.fire({
//                            icon: 'error',
//                            title: 'خطأ',
//                            text: data.message || 'حدث خطأ أثناء تحميل البيانات'
//                        });
//                    } else {
//                        var parsed = typeof data === 'string' ? JSON.parse(data) : data;
//                        if (parsed && parsed.success === false) {
//                            Swal.fire({
//                                icon: 'error',
//                                title: 'خطأ',
//                                text: parsed.message || 'حدث خطأ أثناء تحميل البيانات'
//                            });
//                        }
//                    }
//                },
//                error: function (xhr) {
//                    Swal.close();
//                    Swal.fire({
//                        icon: 'error',
//                        title: 'خطأ',
//                        text: xhr.responseJSON?.message || 'حدث خطأ في الاتصال بالخادم'
//                    });
//                }
//            });
//        } else {
//            // ===== إلغاء الحجز =====
//            Swal.fire({
//                title: title,
//                text: 'هل أنت متأكد من إلغاء هذا الحجز؟',
//                icon: icon,
//                showCancelButton: true,
//                confirmButtonColor: '#dc3545',
//                cancelButtonColor: '#6c757d',
//                confirmButtonText: 'نعم، إلغاء',
//                cancelButtonText: 'إلغاء',
//                reverseButtons: true
//            }).then((result) => {
//                if (result.isConfirmed) {
//                    Swal.fire({
//                        title: 'سبب الإلغاء',
//                        input: 'text',
//                        inputPlaceholder: 'أدخل سبب الإلغاء...',
//                        showCancelButton: true,
//                        confirmButtonText: 'تأكيد الإلغاء',
//                        cancelButtonText: 'إلغاء',
//                        inputValidator: (value) => {
//                            if (!value || value.trim() === '') {
//                                return 'الرجاء إدخال سبب الإلغاء';
//                            }
//                            return null;
//                        }
//                    }).then((reasonResult) => {
//                        if (reasonResult.isConfirmed && reasonResult.value) {
//                            sendChangeStatus(id, status, reasonResult.value);
//                        }
//                    });
//                }
//            });
//        }
//    }

//    // ============================================================
//    // إرسال تغيير الحالة
//    // ============================================================
//    function sendChangeStatus(id, status, reason) {
//        Swal.fire({
//            title: 'جاري المعالجة...',
//            text: 'الرجاء الانتظار',
//            allowOutsideClick: false,
//            didOpen: function () {
//                Swal.showLoading();
//            }
//        });

//        $.ajax({
//            url: urls.changeStatus,
//            type: 'POST',
//            data: { id: id, status: status, reason: reason },
//            success: function (data) {
//                Swal.close();

//                if (data.success) {
//                    updateStatusInTable(id, status, reason);
//                    updateStatistics(id, status);

//                    Toast.fire({
//                        icon: 'success',
//                        title: data.message || 'تم تغيير حالة الحجز بنجاح'
//                    });
//                } else {
//                    Swal.fire({
//                        icon: 'error',
//                        title: 'خطأ',
//                        text: data.message || 'حدث خطأ أثناء تغيير الحالة'
//                    });
//                }
//            },
//            error: function (xhr) {
//                Swal.close();
//                Swal.fire({
//                    icon: 'error',
//                    title: 'خطأ',
//                    text: xhr.responseJSON?.message || 'حدث خطأ في الاتصال بالخادم'
//                });
//            }
//        });
//    }

//    // ============================================================
//    // تحديث الحالة في الجدول (مع إخفاء الأزرار)
//    // ============================================================
//    function updateStatusInTable(id, status, reason) {
//        var row = $('tr').find('a[href*="Edit/' + id + '"]').closest('tr');
//        if (row.length === 0) return;

//        // ===== 1. تحديث حالة الحجز =====
//        var statusCell = row.find('td:eq(8)');
//        var statusClass = status === 2 ? 'status-confirmed' : (status === 3 ? 'status-cancelled' : 'status-pending');
//        var statusText = status === 2 ? 'مؤكد' : (status === 3 ? 'ملغى' : 'قيد الانتظار');

//        statusCell.html('<span class="status-badge ' + statusClass + '">' + statusText + '</span>');

//        if (status === 2 && reason) {
//            statusCell.find('.status-badge').attr('title', 'سبب الإلغاء: ' + reason);
//        }

//        // ===== 2. إخفاء/إظهار أزرار الإجراءات =====
//        var actionsCell = row.find('td:last-child');

//        if (status === 1 || status === 2) {
//            actionsCell.find('.btn-confirm').hide();
//            actionsCell.find('.btn-cancel').hide();
//        } else {
//            actionsCell.find('.btn-confirm').show();
//            actionsCell.find('.btn-cancel').show();
//        }
//    }

//    // ============================================================
//    // تحديث الإحصائيات
//    // ============================================================
//    function updateStatistics(id, status) {
//        var row = $('tr').find('a[href*="Edit/' + id + '"]').closest('tr');
//        if (row.length === 0) return;

//        var statusCell = row.find('td:eq(8)');
//        var oldStatusText = statusCell.text().trim();
//        var oldStatus = -1;

//        if (oldStatusText === 'قيد الانتظار') {
//            oldStatus = 0;
//        } else if (oldStatusText === 'تمت الموافقة') {
//            oldStatus = 1;
//        } else if (oldStatusText === 'مؤكد') {
//            oldStatus = 2;
//        } else if (oldStatusText === 'ملغى') {
//            oldStatus = 3;
//        } else {
//            return;
//        }

//        if (oldStatus === status) return;

//        var oldCard = getStatCard(oldStatus);
//        if (oldCard) {
//            var oldNumber = parseInt(oldCard.find('.number').text()) || 0;
//            oldCard.find('.number').text(Math.max(0, oldNumber - 1));
//        }

//        var newCard = getStatCard(status);
//        if (newCard) {
//            var newNumber = parseInt(newCard.find('.number').text()) || 0;
//            newCard.find('.number').text(newNumber + 1);
//        }
//    }

//    function getStatCard(status) {
//        switch (status) {
//            case 0: return $('.stat-card.pending');
//            case 1: return $('.stat-card.accepted');
//            case 2: return $('.stat-card.confirmed');
//            case 3: return $('.stat-card.cancelled');
//            default: return null;
//        }
//    }

//    // ============================================================
//    // حذف السجل
//    // ============================================================
//    function DeleteCottageReservation(id, cottageId) {
//        Swal.fire({
//            title: 'هل أنت متأكد؟',
//            text: 'لا يمكنك التراجع عن هذا الإجراء!',
//            icon: 'warning',
//            showCancelButton: true,
//            confirmButtonColor: '#dc3545',
//            cancelButtonColor: '#6c757d',
//            confirmButtonText: 'نعم، احذف',
//            cancelButtonText: 'إلغاء',
//            reverseButtons: true
//        }).then((result) => {
//            if (result.isConfirmed) {
//                $.ajax({
//                    url: urls.delete,
//                    type: 'POST',
//                    data: { id: id },
//                    success: function (data) {
//                        if (data.success) {
//                            Toast.fire({
//                                icon: 'success',
//                                title: 'تم الحذف بنجاح'
//                            }).then(() => {
//                                window.location.href = urls.index + '?cottageId=' + cottageId;
//                            });
//                        } else {
//                            Swal.fire({
//                                icon: 'error',
//                                title: 'خطأ',
//                                text: data.message || 'حدث خطأ أثناء الحذف'
//                            });
//                        }
//                    },
//                    error: function () {
//                        Swal.fire({
//                            icon: 'error',
//                            title: 'خطأ',
//                            text: 'حدث خطأ في الاتصال بالخادم'
//                        });
//                    }
//                });
//            }
//        });
//    }

//    // ===== دالة تفريغ كل الفلاتر =====
//    function clearAllFilters() {
//        var cottageId = initialCottageId;
//        window.location.href = urls.index + '?cottageId=' + cottageId;
//    }

//    // كشف الدوال العامة (عشان الـ onclick في الـ HTML يشتغل)
//    window.changeStatus = changeStatus;
//    window.DeleteCottageReservation = DeleteCottageReservation;
//    window.clearAllFilters = clearAllFilters;
//}



/* ============================================================
   إعدادات عامة (Global Config)
   ============================================================ */
var urls = window.cottageUrls;   // هيتم تعبئتها من الـ View
var initialCottageId = window.initialCottageId;

$(document).ready(function () {
    $('.select2').select2({
        placeholder: "اختر من القائمة",
        width: '100%'
    });

    // ===== عند تغيير الكوخ في الفلتر =====
    $('#cottageFilter').change(function () {
        var cottageId = $(this).val();
        if (cottageId) {
            window.location.href = urls.index + '?cottageId=' + cottageId;
        } else {
            window.location.href = urls.index;
        }
    });
});

// ============================================================
// SweetAlert Configuration
// ============================================================
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
});

// ============================================================
// تغيير حالة الحجز (مع الـ Popup للتأكيد)
// ============================================================
window.changeStatus = function (id, status) {
    debugger
    var isConfirm = status === 2;
    var title = isConfirm ? 'تأكيد الحجز' : 'إلغاء الحجز';
    var icon = isConfirm ? 'question' : 'warning';

    if (isConfirm) {
        Swal.fire({
            title: 'جاري تحميل بيانات الحجز...',
            text: 'الرجاء الانتظار',
            allowOutsideClick: false,
            didOpen: function () {
                Swal.showLoading();
            }
        });

        $.ajax({

            url: urls.getConfirmData,
            type: 'GET',
            data: { id: id },
            success: function (data) {
                Swal.close();
                debugger
                if (typeof data === 'string' && data.trim().startsWith('{') === false) {
                    $('#popupContainer').html(data);
                    debugger
                    $('#confirmReservationCottageModal').modal('show');
                } else if (typeof data === 'object' && data.success === false) {
                    Swal.fire({ icon: 'error', title: 'خطأ', text: data.message || 'حدث خطأ أثناء تحميل البيانات' });
                } else {
                    var parsed = typeof data === 'string' ? JSON.parse(data) : data;
                    if (parsed && parsed.success === false) {
                        Swal.fire({ icon: 'error', title: 'خطأ', text: parsed.message || 'حدث خطأ أثناء تحميل البيانات' });
                    }
                }
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire({ icon: 'error', title: 'خطأ', text: xhr.responseJSON?.message || 'حدث خطأ في الاتصال بالخادم' });
            }
        });
    } else {
        // ===== إلغاء الحجز =====
        Swal.fire({
            title: title,
            text: 'هل أنت متأكد من إلغاء هذا الحجز؟',
            icon: icon,
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'نعم، إلغاء',
            cancelButtonText: 'إلغاء',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'سبب الإلغاء',
                    input: 'text',
                    inputPlaceholder: 'أدخل سبب الإلغاء...',
                    showCancelButton: true,
                    confirmButtonText: 'تأكيد الإلغاء',
                    cancelButtonText: 'إلغاء',
                    inputValidator: (value) => {
                        if (!value || value.trim() === '') {
                            return 'الرجاء إدخال سبب الإلغاء';
                        }
                        return null;
                    }
                }).then((reasonResult) => {
                    if (reasonResult.isConfirmed && reasonResult.value) {
                        sendChangeStatus(id, status, reasonResult.value);
                    }
                });
            }
        });
    }
};

function sendChangeStatus(id, status, reason) {
    Swal.fire({
        title: 'جاري المعالجة...',
        text: 'الرجاء الانتظار',
        allowOutsideClick: false,
        didOpen: function () {
            Swal.showLoading();
        }
    });

    $.ajax({
        url: urls.changeStatus,
        type: 'POST',
        data: { id: id, status: status, reason: reason },
        success: function (data) {
            Swal.close();
            if (data.success) {
                updateStatusInTable(id, status, reason);
                updateStatistics(id, status);
                Toast.fire({ icon: 'success', title: data.message || 'تم تغيير حالة الحجز بنجاح' });
            } else {
                Swal.fire({ icon: 'error', title: 'خطأ', text: data.message || 'حدث خطأ أثناء تغيير الحالة' });
            }
        },
        error: function (xhr) {
            Swal.close();
            Swal.fire({ icon: 'error', title: 'خطأ', text: xhr.responseJSON?.message || 'حدث خطأ في الاتصال بالخادم' });
        }
    });
}

window.updateStatusInTable = function (id, status, reason) {
    var row = $('tr').find('a[href*="Edit/' + id + '"]').closest('tr');
    if (row.length === 0) return;

    var statusCell = row.find('td:eq(8)');
    var statusClass = status === 2 ? 'status-confirmed' : (status === 3 ? 'status-cancelled' : 'status-pending');
    var statusText = status === 2 ? 'مؤكد' : (status === 3 ? 'ملغى' : 'قيد الانتظار');

    statusCell.html('<span class="status-badge ' + statusClass + '">' + statusText + '</span>');

    if (status === 2 && reason) {
        statusCell.find('.status-badge').attr('title', 'سبب الإلغاء: ' + reason);
    }

    var actionsCell = row.find('td:last-child');
    if (status === 1 || status === 2) {
        actionsCell.find('.btn-confirm').hide();
        actionsCell.find('.btn-cancel').hide();
    } else {
        actionsCell.find('.btn-confirm').show();
        actionsCell.find('.btn-cancel').show();
    }
};

window.updateStatistics = function (id, status) {
    var row = $('tr').find('a[href*="Edit/' + id + '"]').closest('tr');
    if (row.length === 0) return;

    var statusCell = row.find('td:eq(8)');
    var oldStatusText = statusCell.text().trim();
    var oldStatus = -1;

    if (oldStatusText === 'قيد الانتظار') {
        oldStatus = 0;
    } else if (oldStatusText === 'تمت الموافقة') {
        oldStatus = 1;
    } else if (oldStatusText === 'مؤكد') {
        oldStatus = 2;
    } else if (oldStatusText === 'ملغى') {
        oldStatus = 3;
    } else {
        return;
    }

    if (oldStatus === status) return;

    var oldCard = getStatCard(oldStatus);
    if (oldCard) {
        var oldNumber = parseInt(oldCard.find('.number').text()) || 0;
        oldCard.find('.number').text(Math.max(0, oldNumber - 1));
    }

    var newCard = getStatCard(status);
    if (newCard) {
        var newNumber = parseInt(newCard.find('.number').text()) || 0;
        newCard.find('.number').text(newNumber + 1);
    }
};

window.getStatCard = function (status) {
    switch (status) {
        case 0: return $('.stat-card.pending');
        case 1: return $('.stat-card.accepted');
        case 2: return $('.stat-card.confirmed');
        case 3: return $('.stat-card.cancelled');
        default: return null;
    }
};

window.DeleteCottageReservation = function (id, cottageId) {
    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'لا يمكنك التراجع عن هذا الإجراء!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'نعم، احذف',
        cancelButtonText: 'إلغاء',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: urls.delete,
                type: 'POST',
                data: { id: id },
                success: function (data) {
                    if (data.success) {
                        Toast.fire({ icon: 'success', title: 'تم الحذف بنجاح' }).then(() => {
                            window.location.href = urls.index + '?cottageId=' + cottageId;
                        });
                    } else {
                        Swal.fire({ icon: 'error', title: 'خطأ', text: data.message || 'حدث خطأ أثناء الحذف' });
                    }
                },
                error: function () {
                    Swal.fire({ icon: 'error', title: 'خطأ', text: 'حدث خطأ في الاتصال بالخادم' });
                }
            });
        }
    });
};

window.clearAllFilters = function () {
    var cottageId = initialCottageId;
    window.location.href = urls.index + '?cottageId=' + cottageId;
};