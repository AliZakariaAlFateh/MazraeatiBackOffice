let BaseUrl = window.location.hostname === 'localhost'
    ? 'http://localhost:62550'
    : 'http://5.189.180.190/MazraeatiBackOffice';

// =============================================
// Global Variables (خارج document.ready عشان تكون عامة)
// =============================================
var isSaving = false;
var userId = 0;
var selectedRows = new Set();


// =============================================
// Toast Notification
// =============================================
function showToast(message, isError) {
    isError = isError || false;
    var container = $('#toastContainer');

    // نشيل أي Toast قديم
    container.empty();

    var toast = $('<div>')
        .addClass('toast-message' + (isError ? ' error' : ''))
        .html(`
                    <i class="fas ${isError ? 'fa-times-circle text-danger' : 'fa-check-circle text-success'}"></i>
                    <span>${message}</span>
                `);
    container.append(toast);

    // نضيف كلاس show بعد 100ms عشان تظهر بحركة سلسة
    setTimeout(function () {
        toast.addClass('show');
    }, 100);

    // نخفيها بعد 3 ثواني
    setTimeout(function () {
        toast.removeClass('show');
        setTimeout(function () {
            toast.remove();
        }, 500);
    }, 3000);
}



// =============================================
// Loading Spinner
// =============================================
function showLoading(show) {
    $('#loadingSpinner').css('display', show ? 'flex' : 'none');
}

// =============================================
// ===== Update Other Checkboxes (تعطيل/تفعيل) =====
// =============================================
function updateOtherCheckboxes($row, isViewChecked) {
    var $allCheckboxes = $row.find('.perm-checkbox');
    $allCheckboxes.each(function () {
        var $cb = $(this);
        var permType = $cb.data('perm');
        if (permType !== 'view') {
            $cb.prop('disabled', false);
        }
    });
}



// =============================================
// Update Permission
// =============================================
function updatePermission(screenId, permType, value) {
    if (isSaving) return;

    var $checkbox = $(`input[data-screen="${screenId}"][data-perm="${permType}"]`);
    $checkbox.prop('disabled', true);

    $.ajax({
        url: '/Account/UpdatePermission',
        type: 'POST',
        data: {
            userId: userId,
            screenId: screenId,
            permissionType: permType,
            value: value
        },
        success: function (response) {
            if (response.success) {
                // بس لو كان type = view، نحدث الباقي
                if (permType === 'view') {
                    var $row = $(`tr[data-screen-id="${screenId}"]`);
                    if ($row.length) {
                        var $viewCheckbox = $row.find('.perm-checkbox[data-perm="view"]');
                        if ($viewCheckbox.length) {
                            updateOtherCheckboxes($row, $viewCheckbox.prop('checked'));
                        }
                    }

                    if (!value) {
                        if (selectedRows.has(screenId.toString())) {
                            var $rowCheckbox = $(`.row-selector[data-screen-id="${screenId}"]`);
                            if ($rowCheckbox.length) {
                                $rowCheckbox.prop('checked', false);
                                selectedRows.delete(screenId.toString());
                                var $row = $(`tr[data-screen-id="${screenId}"]`);
                                $row.removeClass('selected');
                                updateSelectedActionsBar();
                            }
                        }
                    }
                }
                // أي نوع تاني (Create, Edit, Delete, Export) يتم حفظه فقط
            } else {
                showToast(response.message, true);
                $checkbox.prop('checked', !value);
            }
        },
        error: function () {
            showToast('حدث خطأ', true);
            $checkbox.prop('checked', !value);
        },
        complete: function () {
            $checkbox.prop('disabled', false);
        }
    });
}




// =============================================
// ===== Row Selection (الصف بالكامل) =====
// =============================================
function onRowSelect(checkbox) {
    var $checkbox = $(checkbox);
    var screenId = $checkbox.data('screen-id');
    var $row = $checkbox.closest('tr');
    var isChecked = $checkbox.prop('checked');

    // 1. نغير كل التشيك بوكسات في الصف
    var $permCheckboxes = $row.find('.perm-checkbox');
    $permCheckboxes.each(function () {
        var $cb = $(this);
        $cb.prop('checked', isChecked);
        var permType = $cb.data('perm');
        var screenId2 = $cb.data('screen');
        updatePermission(screenId2, permType, isChecked);
    });

    // 2. نحدث حالة التشيك بوكسات (تعطيل/تفعيل)
    var $viewCheckbox = $row.find('.perm-checkbox[data-perm="view"]');
    if ($viewCheckbox.length) {
        updateOtherCheckboxes($row, $viewCheckbox.prop('checked'));
    }

    // 3. نضيف/نشيل الصف من المختارين
    if (isChecked) {
        selectedRows.add(screenId);
        $row.addClass('selected');
    } else {
        selectedRows.delete(screenId);
        $row.removeClass('selected');
    }

    updateSelectedActionsBar();
    updateHeaderRowCheckbox();
}

// =============================================
// ===== Toggle All Row Checkboxes =====
// =============================================
function toggleAllRowCheckboxes(checked) {
    $('.row-selector').each(function () {
        var $cb = $(this);
        $cb.prop('checked', checked);
        var screenId = $cb.data('screen-id');
        var $row = $cb.closest('tr');

        var $permCheckboxes = $row.find('.perm-checkbox');
        $permCheckboxes.each(function () {
            var $cb2 = $(this);
            $cb2.prop('checked', checked);
            var permType = $cb2.data('perm');
            var screenId2 = $cb2.data('screen');
            updatePermission(screenId2, permType, checked);
        });

        var $viewCheckbox = $row.find('.perm-checkbox[data-perm="view"]');
        if ($viewCheckbox.length) {
            updateOtherCheckboxes($row, $viewCheckbox.prop('checked'));
        }

        if (checked) {
            selectedRows.add(screenId);
            $row.addClass('selected');
        } else {
            selectedRows.delete(screenId);
            $row.removeClass('selected');
        }
    });

    updateSelectedActionsBar();
}

// =============================================
// ===== Update Header Row Checkbox =====
// =============================================
function updateHeaderRowCheckbox() {
    var total = $('.row-selector').length;
    var checked = $('.row-selector:checked').length;
    var $headerCheckbox = $('#selectAllRows');

    if (total === 0) {
        $headerCheckbox.prop('checked', false);
        $headerCheckbox.prop('indeterminate', false);
    } else if (checked === total) {
        $headerCheckbox.prop('checked', true);
        $headerCheckbox.prop('indeterminate', false);
    } else if (checked > 0) {
        $headerCheckbox.prop('checked', false);
        $headerCheckbox.prop('indeterminate', true);
    } else {
        $headerCheckbox.prop('checked', false);
        $headerCheckbox.prop('indeterminate', false);
    }
}

// =============================================
// ===== Select All Rows =====
// =============================================
function selectAllRows(checked) {
    $('#selectAllRows').prop('checked', checked);
    toggleAllRowCheckboxes(checked);
}

// =============================================
// ===== Update Selected Actions Bar =====
// =============================================
function updateSelectedActionsBar() {
    var count = selectedRows.size;
    var $bar = $('#selectedActionsBar');
    var $countSpan = $('#selectedCount');

    $countSpan.text(count);

    if (count > 0) {
        $bar.addClass('show');
    } else {
        $bar.removeClass('show');
    }

    updateHeaderRowCheckbox();
}

// =============================================
// ===== Select All / Deselect All (للأعمدة) =====
// =============================================
function selectAll(permType, value) {
    $(`.perm-checkbox[data-perm="${permType}"]`).each(function () {
        var $cb = $(this);
        if (!$cb.prop('disabled')) {
            $cb.prop('checked', value);
            var screenId = $cb.data('screen');
            updatePermission(screenId, permType, value);
        }
    });

    if (permType === 'view') {
        $('tbody tr').each(function () {
            var $row = $(this);
            var $viewCheckbox = $row.find('.perm-checkbox[data-perm="view"]');
            if ($viewCheckbox.length) {
                updateOtherCheckboxes($row, $viewCheckbox.prop('checked'));
            }
        });
    }
}

// =============================================
// ===== Save All Permissions =====
// =============================================
function saveAllPermissions() {
    if (isSaving) return;
    isSaving = true;
    showLoading(true);

    var permissions = [];

    $('tbody tr').each(function () {
        var $row = $(this);
        var screenId = $row.data('screen-id');
        var $checkboxes = $row.find('.perm-checkbox');

        var perm = {
            ScreenId: parseInt(screenId),
            CanView: false,
            CanCreate: false,
            CanEdit: false,
            CanDelete: false,
            CanExport: false
        };

        $checkboxes.each(function () {
            var $cb = $(this);
            var pt = $cb.data('perm');
            switch (pt) {
                case 'view':
                    perm.CanView = $cb.prop('checked');
                    break;
                case 'create':
                    perm.CanCreate = $cb.prop('checked');
                    break;
                case 'edit':
                    perm.CanEdit = $cb.prop('checked');
                    break;
                case 'delete':
                    perm.CanDelete = $cb.prop('checked');
                    break;
                case 'export':
                    perm.CanExport = $cb.prop('checked');
                    break;
            }
        });

        permissions.push(perm);
    });

    $.ajax({
        url: '/Account/SaveAllPermissions',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ userId: userId, permissions: permissions }),
        success: function (response) {
            if (response.success) {
                showToast(response.message);
            } else {
                showToast(response.message, true);
            }
        },
        error: function () {
            showToast('حدث خطأ', true);
        },
        complete: function () {
            isSaving = false;
            showLoading(false);
        }
    });
}

// =============================================
// ===== Keyboard Shortcuts =====
// =============================================
$(document).on('keydown', function (e) {
    if (e.ctrlKey && e.key === 's') {
        e.preventDefault();
        saveAllPermissions();
    }
    if (e.ctrlKey && e.key === 'a') {
        e.preventDefault();
        selectAllRows(true);
    }
});

// =============================================
// ===== Initialize =====
// =============================================
$(document).ready(function () {

    // 1. نفتح كل التشيك بوكسات في البداية
    $('.perm-checkbox').prop('disabled', false);

    // 2. نحدث حالة التشيك بوكسات حسب الـ View
    $('tbody tr').each(function () {
        var $row = $(this);
        var $viewCheckbox = $row.find('.perm-checkbox[data-perm="view"]');
        if ($viewCheckbox.length) {
            updateOtherCheckboxes($row, $viewCheckbox.prop('checked'));
        }
    });

    // 3. نحدث شريط الإجراءات
    updateSelectedActionsBar();

    // 4. نحدد كل الصفوف المختارة في البداية (لو في)
    $('.row-selector:checked').each(function () {
        var screenId = $(this).data('screen-id');
        selectedRows.add(screenId);
        $(this).closest('tr').addClass('selected');
    });
    updateSelectedActionsBar();

}); // end document.ready

// ===== تعريف الدوال في النطاق العام (Global) عشان الـ HTML يوصلها =====
window.onRowSelect = onRowSelect;
window.toggleAllRowCheckboxes = toggleAllRowCheckboxes;
window.selectAllRows = selectAllRows;
window.selectAll = selectAll;
window.saveAllPermissions = saveAllPermissions;
window.updatePermission = updatePermission;