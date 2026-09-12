// sportReservation.js

var SportReservation = {
    // المتغيرات العامة
    redeemRules: [],
    customerPoints: 0,
    isUpdating: false,
    customerId: null,

    // ============================================================
    // التهيئة
    // ============================================================
    init: function (options) {
        this.redeemRules = options.redeemRules || [];
        this.customerPoints = options.customerPoints || 0;
        this.customerId = options.customerId || null;

        this.bindEvents();
        this.calculateValues();
        this.updateAll();
    },

    // ============================================================
    // ربط الأحداث
    // ============================================================
    bindEvents: function () {
        var self = this;

        // تغيير العميل
        $('#CustomerId').on('change', function () {
            var selectedOption = $(this).find('option:selected');
            var Cname = selectedOption.data('customername');
            var CPhone = selectedOption.data('customerphone');
            $('#CustomerName').val(Cname);
            $('#CustomerPhone').val(CPhone);

            var customerId = $(this).val();
            if (customerId) {
                self.customerId = customerId;
                self.getCustomerPoints(customerId);
            }
        });

        // حساب القيم
        $('.cost-amount, .reservation-amt, .deposit-amt').on('input', function () {
            self.calculateValues();
        });

        // استخدام النقاط
        $('#usePointsCheck').change(function () {
            debugger
            if ($(this).is(':checked')) {
                $('#pointsRedeemSection').show();
                $('#redeemPoints').val(0);
                self.updateAll();
            } else {
                $('#pointsRedeemSection').hide();
                $('#redeemPoints').val(0);
                $('#discountResultsSection').hide();
                $('#pointsValidationMessage').hide();
                var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
                $('#ReservationAmt').val(Number(total));
                self.calculateValues();
            }
        });

        // تغيير عدد النقاط
        $('#redeemPoints').on('input', function () {
            debugger
            var points = parseInt($(this).val()) || 0;
            if (points > self.customerPoints) {
                $(this).val(self.customerPoints);
            }
            self.updateAll();
        });

        // تغيير المبلغ
        //$('#ReservationAmt').on('input', function () {
        //    if (self.isUpdating) return;
        //    var val = parseFloat($(this).val()) || 0;
        //    if (val > 0) {
        //        $('#ReservationAmtOriginal').val(val);
        //    }
        //    self.updateAll();
        //});
        // ✅ السماح بإدخال الأرقام والكسور فقط
        $('#ReservationAmt').on('input', function () {
            var val = $(this).val();
            // السماح بالأرقام ونقطة عشرية واحدة فقط
            val = val.replace(/[^0-9.]/g, '');
            // منع أكثر من نقطة عشرية
            var parts = val.split('.');
            if (parts.length > 2) {
                val = parts[0] + '.' + parts.slice(1).join('');
            }
            $(this).val(val);

            if (self.isUpdating) return;
            var numVal = parseFloat(val) || 0;
            if (numVal > 0) {
                $('#ReservationAmtOriginal').val(numVal);
            }
            self.updateAll();
        });

        //  لما يخرج من الحقل، لو فاضي أو نص، نحوله لـ 0
        $('#ReservationAmt').on('blur', function () {
            var val = $(this).val();
            if (val === '' || val === '.' || isNaN(parseFloat(val))) {
                $(this).val('0');
                $('#ReservationAmtOriginal').val(0);
                self.calculateValues();
                self.updateAll();
            }
        });


        //  زر مسح المبلغ
        $('#clearReservationAmt').on('click', function () {
            debugger
            $('#ReservationAmt').val('');
            $('#ReservationAmt').focus();
            self.calculateValues();
            //self.updateAll();
        });


        // تاريخ الحجز
        var todayStr = new Date().toISOString().split('T')[0];
        var $input = $('#ReservationDate');
        $input.attr({ min: todayStr });
        if (!$input.val()) $input.val(todayStr);
        $input.attr('readonly', false)
            .on('keydown paste drop input', function (e) {
                e.preventDefault();
                return false;
            });
        $input.on('click', function () {
            try { this.showPicker && this.showPicker(); } catch (e) { }
            this.blur();
        });
        $input.on('change', function () {
            var val = $(this).val();
            if (!val || val < todayStr) {
                $(this).val(todayStr);
            }
        });

        // نوع الرياضة
        $('#SportTypeId').change(function () {
            var sportTypeId = $(this).val();
            var selectedSportId = $('#SportId').data('selected-id') || 0;
            if (sportTypeId) {
                $.ajax({
                    url: $(this).data('url'),
                    type: 'GET',
                    data: { sportTypeId: sportTypeId },
                    success: function (data) {
                        var select = $('#SportId');
                        select.empty();
                        select.append('<option value="">-- اختر القسم --</option>');
                        $.each(data, function (index, sport) {
                            var selected = (sport.id == selectedSportId) ? 'selected' : '';
                            select.append('<option value="' + sport.id + '" ' + selected + '>' + sport.nameAr + '</option>');
                        });
                    }
                });
            } else {
                $('#SportId').empty().append('<option value="">-- اختر القسم --</option>');
            }
        });
    },

    // ============================================================
    // جلب نقاط العميل
    // ============================================================
    getCustomerPoints: function (customerId) {
        var self = this;
        $.ajax({
            url: $('#CustomerId').data('points-url'),
            type: 'GET',
            data: { customerId: customerId },
            success: function (data) {
                self.customerPoints = data.points || 0;
                $('#customerPointsDisplay').text(self.customerPoints);
                $('#maxPointsDisplay').text(self.customerPoints);
                $('#redeemPoints').attr('max', self.customerPoints);
                $('#pointsStatusBadge')
                    .text(self.customerPoints > 0 ? 'متاحة للخصم' : 'لا توجد نقاط')
                    .removeClass(self.customerPoints > 0 ? 'badge-secondary' : 'badge-success')
                    .addClass(self.customerPoints > 0 ? 'badge-success' : 'badge-secondary');
                self.updateAll();
            }
        });
    },

    // ============================================================
    // حساب القيم (الربح والمتبقي)
    // ============================================================
    calculateValues: function () {
        debugger
        var cost = parseFloat($('.cost-amount').val()) || 0;
        var total = parseFloat($('.reservation-amt').val()) || 0;
        var deposit = parseFloat($('.deposit-amt').val()) || 0;
        var profit = (total - cost).toFixed(3);
        var remain = (total - deposit).toFixed(3);
        $('#netProfit').val(profit);
        $('#reservationRemain').val(remain);
    },

    // ============================================================
    // حساب الخصم - بدون تقريب إجباري
    // ============================================================
    calculateDiscount: function (points) {
        if (!this.redeemRules || this.redeemRules.length === 0) {
            return { discount: 0, rule: null, canRedeem: false };
        }

        var sortedRules = this.redeemRules.slice().sort(function (a, b) {
            return b.points - a.points;
        });

        for (var i = 0; i < sortedRules.length; i++) {
            if (points >= sortedRules[i].points) {
                var ratio = points / sortedRules[i].points;
                var discount = ratio * sortedRules[i].discountAmount;
                return {
                    discount: discount,
                    rule: sortedRules[i],
                    canRedeem: true
                };
            }
        }
        return { discount: 0, rule: null, canRedeem: false };
    },

    // ============================================================
    // تحديث الحسابات
    // ============================================================
    //updateCalculations: function (pointsToRedeem) {
    //    if (this.isUpdating) return;
    //    this.isUpdating = true;

    //    var self = this;
    //    var usePoints = $('#usePointsCheck').is(':checked');
    //    var points = pointsToRedeem || parseInt($('#redeemPoints').val()) || 0;

    //    if (!usePoints || points <= 0) {
    //        $('#discountResultsSection').hide();
    //        var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
    //        $('#ReservationAmt').val(Number(total));
    //        this.isUpdating = false;
    //        this.calculateValues();
    //        return;
    //    }

    //    if (points > this.customerPoints) {
    //        points = this.customerPoints;
    //        $('#redeemPoints').val(points);
    //    }

    //    var result = this.calculateDiscount(points);
    //    var discount = result.discount;
    //    var totalAmount = parseFloat($('#ReservationAmtOriginal').val()) || 0;
    //    var newTotal = totalAmount - discount;
    //    var remainingPoints = this.customerPoints - points;

    //    if (result.canRedeem && discount > 0) {
    //        $('#discountResultsSection').show();
    //        $('#discountAmountDisplay').text(discount + ' دينار');
    //        $('#remainingPointsDisplay').text(remainingPoints + ' نقطة');
    //        $('#newTotalDisplay').text(newTotal + ' دينار');
    //        if (result.rule) {
    //            $('#discountRuleDisplay').text(result.rule.points + ' نقطة = ' + result.rule.discountAmount + ' دينار');
    //        }
    //        $('#ReservationAmt').val(Number(newTotal));
    //        $('#pointsValidationMessage').hide();
    //    } else {
    //        $('#discountResultsSection').hide();
    //        var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
    //        $('#ReservationAmt').val(Number(total));
    //    }

    //    this.calculateValues();
    //    this.isUpdating = false;
    //},


    // ============================================================
    // تحديث الحسابات
    // ============================================================
    updateCalculations: function (pointsToRedeem) {
        if (this.isUpdating) return;
        this.isUpdating = true;

        var self = this;
        var usePoints = $('#usePointsCheck').is(':checked');
        var points = pointsToRedeem || parseInt($('#redeemPoints').val()) || 0;

        if (!usePoints || points <= 0) {
            $('#discountResultsSection').hide();
            var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
            // ✅ تقريب وعرض بدون .00
            $('#ReservationAmt').val(total);
            this.isUpdating = false;
            this.calculateValues();
            return;
        }

        if (points > this.customerPoints) {
            points = this.customerPoints;
            $('#redeemPoints').val(points);
        }

        var result = this.calculateDiscount(points);
        var discount = result.discount;
        var totalAmount = parseFloat($('#ReservationAmtOriginal').val()) || 0;
        var newTotal = totalAmount - discount;
        var remainingPoints = this.customerPoints - points;

        // ✅ تقريب لـ 3 أرقام عشرية
        discount = Math.round(discount * 1000) / 1000;
        newTotal = Math.round(newTotal * 1000) / 1000;

        if (result.canRedeem && discount > 0) {
            $('#discountResultsSection').show();
            $('#discountAmountDisplay').text(discount + ' دينار');
            $('#remainingPointsDisplay').text(remainingPoints + ' نقطة');
            $('#newTotalDisplay').text(newTotal + ' دينار');
            if (result.rule) {
                $('#discountRuleDisplay').text(result.rule.points + ' نقطة = ' + result.rule.discountAmount + ' دينار');
            }
            $('#ReservationAmt').val(newTotal);
            $('#pointsValidationMessage').hide();
        } else {
            $('#discountResultsSection').hide();
            var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
            $('#ReservationAmt').val(total);
        }

        this.calculateValues();
        this.isUpdating = false;
    },

    // ============================================================
    // التحقق من النقاط
    // ============================================================
    validatePoints: function (points, customerId) {
        if (!customerId || points <= 0) {
            $('#pointsValidationMessage').hide();
            return;
        }

        var self = this;
        $.ajax({
            url: $('#CustomerId').data('validate-url'),
            type: 'GET',
            data: { customerId: customerId, points: points },
            success: function (data) {
                if (data.success && data.canRedeem) {
                    $('#pointsValidationMessage').hide();
                    self.updateCalculations(points);
                } else {
                    $('#pointsValidationMessage')
                        .text(data.message || 'لا يمكن استخدام هذا العدد من النقاط')
                        .show();
                    $('#discountResultsSection').hide();
                }
            },
            error: function () {
                $('#pointsValidationMessage').text('حدث خطأ في الاتصال').show();
            }
        });
    },

    // ============================================================
    // تحديث الكل
    // ============================================================
    updateAll: function () {
        debugger
        var usePoints = $('#usePointsCheck').is(':checked');
        var points = parseInt($('#redeemPoints').val()) || 0;
        if (usePoints && points > 0) {
            this.validatePoints(points, $('#CustomerId').val());
        } else {
            $('#discountResultsSection').hide();
            $('#pointsValidationMessage').hide();
            var total = parseFloat($('#ReservationAmtOriginal').val()) || 0;
            $('#ReservationAmt').val(Number(total));
            this.calculateValues();
        }
    }
};