$(document).ready(function () {

    // ==========================
    // CONFIGURATION
    // ==========================
    const NOTIFICATION_URL = window.location.hostname === 'localhost'
        ? '/Notification'
        : 'http://5.189.180.190/MazraeatiBackOffice/Notification';

    const FARM_API_URL = window.location.hostname === 'localhost'
        ? "http://localhost:61366/farmHub"
        : "http://5.189.180.190/MazareatiAPI/farmHub";

    const PRICE_API_URL = window.location.hostname === 'localhost'
        ? "http://localhost:61366/priceHub"
        : "http://5.189.180.190/MazareatiAPI/priceHub";

    const MVC_UPDATE_URL = window.location.hostname === 'localhost'
        ? "/Farmers/EditPriceList"
        : "http://5.189.180.190/MazraeatiBackOffice/Farmers/EditPriceList";

    // ==========================
    // STATE
    // ==========================
    let notificationCount = 0;
    let notificationList = [];
    let notificationIds = [];
    let isFirstLoad = true;
    let isLoading = false;
    let isDropdownOpen = false;

    // ==========================
    // DOM REFS
    // ==========================
    const countElement = document.getElementById("notificationCount");
    const listElement = document.getElementById("notiItems");
    const dropdown = document.getElementById("notificationList");
    const wrapper = document.querySelector(".notification-wrapper");

    console.log('🚀 Notifications System Starting...');

    // ==========================
    // API CALLS (خفيفة وسريعة)
    // ==========================
    async function fetchNotifications() {
        try {
            const response = await $.ajax({
                url: `${NOTIFICATION_URL}/GetNotifications`,
                type: 'GET',
                timeout: 5000 // 5 ثواني فقط
            });
            return response;
        } catch (error) {
            console.error('❌ Error fetching notifications:', error);
            return { success: false, count: 0, notifications: [] };
        }
    }

    async function fetchNotificationsCount() {
        try {
            const response = await $.ajax({
                url: `${NOTIFICATION_URL}/GetUnreadCount`,
                type: 'GET',
                timeout: 3000 // 3 ثواني فقط
            });
            return response;
        } catch (error) {
            console.error('❌ Error fetching count:', error);
            return { success: false, count: 0 };
        }
    }

    async function markAsRead(id) {
        try {
            const response = await $.ajax({
                url: `${NOTIFICATION_URL}/MarkAsRead`,
                type: 'POST',
                data: { id: id },
                timeout: 5000
            });
            return response;
        } catch (error) {
            console.error('❌ Error marking as read:', error);
            return { success: false };
        }
    }

    async function markAllAsRead() {
        try {
            const response = await $.ajax({
                url: `${NOTIFICATION_URL}/MarkAllAsRead`,
                type: 'POST',
                timeout: 5000
            });
            return response;
        } catch (error) {
            console.error('❌ Error marking all as read:', error);
            return { success: false };
        }
    }

    async function confirmNotification(id, isConfirmed) {
        try {
            const response = await $.ajax({
                url: `${NOTIFICATION_URL}/Confirm`,
                type: 'POST',
                data: { id: id, isConfirmed: isConfirmed },
                timeout: 10000
            });
            return response;
        } catch (error) {
            console.error('❌ Error confirming notification:', error);
            return { success: false, message: error.message };
        }
    }








    // wwwroot/js/notifications.js

    // ==========================
    // إضافة إشعار فوري (بدون تحميل)
    // ==========================
    function addNotificationDirectly(notification) {
        console.log('⚡ Adding notification directly:', notification);

        // ✅ التحقق من وجود بيانات
        if (!notification || !notification.Id && !notification.id) {
            console.error('❌ Invalid notification data:', notification);
            return;
        }

        // ✅ توحيد أسماء الخصائص
        const id = notification.Id || notification.id;
        const type = notification.Type || notification.type;
        const title = notification.Title || notification.title;
        const message = notification.Message || notification.message;
        const createdDate = notification.CreatedDate || notification.createdDate || new Date();

        // ✅ معالجة البيانات (Farm)
        let newData = notification.NewData || notification.newData || notification.Data || notification.data;
        let oldData = notification.OldData || notification.oldData;

        // ✅ لو البيانات جايه كـ object مش string
        if (typeof newData === 'object') {
            newData = JSON.stringify(newData);
        }
        if (typeof oldData === 'object') {
            oldData = JSON.stringify(oldData);
        }

        const formattedNotification = {
            id: id,
            type: type,
            title: title || (type === 'Farm' ? '🌾 إضافة مزرعة جديدة' : '📊 تحديث قائمة الأسعار'),
            message: message || (type === 'Farm' ? 'تم إضافة مزرعة جديدة' : 'تم استلام تحديث جديد لقائمة الأسعار'),
            newData: newData,
            oldData: oldData,
            createdDate: createdDate
        };

        console.log('📋 Formatted notification:', formattedNotification);

        let html = '';
        if (formattedNotification.type === 'Farm') {
            html = buildFarmNotification(formattedNotification);
        } else if (formattedNotification.type === 'Price') {
            html = buildPriceNotification(formattedNotification);
        }

        if (html) {
            // ✅ إضافة في أول القائمة
            notificationList.unshift(html);
            notificationIds.unshift(formattedNotification.id);
            notificationCount++;

            // ✅ تحديث الواجهة فوراً
            updateUI();

            // ✅ عرض Toast
            const toastMessage = formattedNotification.type === 'Farm'
                ? '🌾 تم إضافة مزرعة جديدة'
                : '📊 تم تحديث الأسعار';
            showToast(toastMessage, 'success');

            console.log(`⚡ Notification added instantly: ${formattedNotification.id}`);
        } else {
            console.error('❌ Failed to build notification HTML');
            // ✅ لو فشل، نعمل reload عشان نجيب البيانات من قاعدة البيانات
            loadNotifications();
        }
    }

    // ==========================
    // بناء رسالة إشعار المزرعة (مطور)
    // ==========================
    function buildFarmNotification(notification) {
        console.log('🏗️ Building farm notification:', notification);

        let data = {};
        try {
            // ✅ محاولة parse البيانات
            if (typeof notification.newData === 'string') {
                data = JSON.parse(notification.newData);
            } else if (typeof notification.newData === 'object') {
                data = notification.newData;
            } else {
                data = {};
            }
        } catch (e) {
            console.error('❌ Error parsing farm data:', e);
            data = {};
        }

        console.log('🌾 Farm Data:', data);

        // ✅ جلب اسم المزرعة من عدة أماكن
        const farmName = data.Name || data.name || data.FarmName || data.farmName || 'غير معروف';
        const location = data.LocationDesc || data.locationDesc || data.Location || data.location || '';

        // ✅ جلب الـ FarmerId من عدة أماكن
        let farmerId = null;

        // ١- من ExtraFeatures
        if (data.ExtraFeatures && Array.isArray(data.ExtraFeatures) && data.ExtraFeatures.length > 0) {
            farmerId = data.ExtraFeatures[0]?.FarmerId || data.ExtraFeatures[0]?.farmerId || null;
        }

        // ٢- من FarmerId المباشر
        if (!farmerId && data.FarmerId) {
            farmerId = data.FarmerId;
        }
        if (!farmerId && data.farmerId) {
            farmerId = data.farmerId;
        }

        console.log('✅ FarmerId:', farmerId);

        let html = `
        <div class="notification-item farm-notification" data-id="${notification.id}">
            <div class="noti-icon">🌾</div>
            <div class="noti-content">
                <div class="noti-title">✅ تم اضافة مزرعة</div>
                <div class="noti-text"><strong>${farmName}</strong></div>
                ${location ? `<div class="noti-location">📍 ${location}</div>` : ''}
                <div class="noti-time">🕐 ${new Date(notification.createdDate).toLocaleString('ar-EG')}</div>
                <div class="noti-actions">
                    <button onclick="markAsReadNotification(${notification.id})" 
                            style="background: #28a745; color: white; border: none; padding: 2px 12px; border-radius: 4px; cursor: pointer; font-size: 11px;">
                        👁️ تم المشاهدة
                    </button>
                </div>
            </div>
        </div>
    `;

        // ✅ رابط التعديل
        if (farmerId) {
            html += `<div class="noti-action">
            <a href='/MazraeatiBackOffice/Farmers/Edit/${farmerId}' target='_blank' 
               style='color: #4CAF50; text-decoration: underline; font-size: 12px;'>
                🔗 تعديل المزرعة
            </a>
        </div>`;
        } else {
            html += `<div class="noti-action">
            <a href='/MazraeatiBackOffice/Farmers/Index' target='_blank' 
               style='color: #2196F3; text-decoration: underline; font-size: 12px;'>
                🔗 عرض جميع المزارع
            </a>
        </div>`;
        }

        return html;
    }

    // ==========================
    // بناء رسالة إشعار الأسعار (مطور)
    // ==========================
    function buildPriceNotification(notification) {
        console.log('🏗️ Building price notification:', notification);

        let oldData = [];
        let newData = [];

        try {
            // ✅ parse البيانات
            if (typeof notification.oldData === 'string') {
                oldData = JSON.parse(notification.oldData);
            } else if (typeof notification.oldData === 'object') {
                oldData = notification.oldData;
            }

            if (typeof notification.newData === 'string') {
                newData = JSON.parse(notification.newData);
            } else if (typeof notification.newData === 'object') {
                newData = notification.newData;
            }
        } catch (e) {
            console.error('❌ Error parsing price data:', e);
        }

        console.log('📊 Old Data:', oldData);
        console.log('📊 New Data:', newData);

        // ✅ جلب اسم المزارع
        let farmerName = 'مزارع';
        if (newData && newData.length > 0) {
            farmerName = newData[0]?.FarmerName || newData[0]?.farmerName || 'مزارع';
        }

        const totalCount = newData?.length || 0;

        let changedCount = 0;
        if (newData && newData.length > 0) {
            newData.forEach((newPrice) => {
                const oldPrice = oldData?.find(x => x.Id === newPrice.Id || x.id === newPrice.id);
                const diff = (newPrice.MorningPrice || newPrice.morningPrice || 0) - (oldPrice?.MorningPrice || oldPrice?.morningPrice || 0);
                if (diff !== 0) changedCount++;
            });
        }

        const encodedData = encodeURIComponent(JSON.stringify({
            id: notification.id,
            farmerName: farmerName,
            oldData: oldData,
            newData: newData,
            createdDate: notification.createdDate,
            message: notification.message
        }));

        return `
        <div class="notification-item price-notification" data-id="${notification.id}">
            <div class="noti-icon">📊</div>
            <div class="noti-content">
                <div class="noti-title">🔄 تحديث قائمة الأسعار</div>
                <div class="noti-text">
                    <div class="farmer-name">👨‍🌾 ${farmerName}</div>
                    <div style="font-size: 12px; color: #6c757d; margin-top: 4px;">
                        📝 ${notification.message || 'تم استلام تحديث جديد'}
                    </div>
                </div>
                <div class="price-summary">
                    <span class="total">📊 ${totalCount} سعر</span>
                    ${changedCount > 0 ? `<span class="changed">🔄 ${changedCount} تغير</span>` : ''}
                </div>
                <div class="noti-time">🕐 ${new Date(notification.createdDate).toLocaleString('ar-EG')}</div>
                <div class="noti-actions">
                    <button onclick="showPriceConfirmation('${encodedData}')" 
                            style="color: #2196F3; background: none; border: 1px solid #2196F3; border-radius: 4px; cursor: pointer; font-size: 11px; padding: 2px 10px;">
                        📋 مراجعة وتأكيد
                    </button>
                    <button onclick="markAsReadNotification(${notification.id})" 
                            style="background: #28a745; color: white; border: none; padding: 2px 12px; border-radius: 4px; cursor: pointer; font-size: 11px;">
                        👁️ تم المشاهدة
                    </button>
                </div>
            </div>
        </div>
    `;
    }

    // ==========================
    // SIGNALR - استقبال الإشعارات (مطور)
    // ==========================
    const farmConnection = new signalR.HubConnectionBuilder()
        .withUrl(FARM_API_URL)
        .withAutomaticReconnect()
        .build();

    farmConnection.on("FarmAdded", function (data) {
        console.log('🌾 FarmAdded (Raw):', data);

        // ✅ التحقق من وجود البيانات
        if (!data) {
            console.error('❌ No data received');
            return;
        }

        // ✅ استخراج البيانات من الهيكل
        let notificationData = data;

        // لو البيانات جاية في Data
        if (data.Data) {
            notificationData = data.Data;
        }

        // لو البيانات جاية في NewData
        if (data.NewData) {
            notificationData = data.NewData;
        }

        // ✅ التأكد من وجود Id
        const id = data.Id || data.id || Date.now();

        const notification = {
            id: id,
            type: 'Farm',
            title: '🌾 إضافة مزرعة جديدة',
            message: 'تم إضافة مزرعة جديدة',
            newData: notificationData,
            createdDate: data.CreatedDate || data.createdDate || new Date()
        };

        console.log('📋 Processed Farm Notification:', notification);

        addNotificationDirectly(notification);
    });

    farmConnection.start().catch(err => console.error('Farm Hub error:', err));

    const priceConnection = new signalR.HubConnectionBuilder()
        .withUrl(PRICE_API_URL)
        .withAutomaticReconnect()
        .build();

    priceConnection.on("PricesBatchUpdated", function (data) {
        console.log('📊 PricesBatchUpdated (Raw):', data);

        if (!data) {
            console.error('❌ No data received');
            return;
        }

        // ✅ استخراج البيانات
        let oldData = data.OldData || data.oldData || [];
        let newData = data.NewData || data.newData || [];
        let id = data.Id || data.id || Date.now();

        const notification = {
            id: id,
            type: 'Price',
            title: '📊 تحديث قائمة الأسعار',
            message: 'تم استلام تحديث جديد لقائمة الأسعار',
            oldData: oldData,
            newData: newData,
            createdDate: data.CreatedDate || data.createdDate || new Date()
        };

        console.log('📋 Processed Price Notification:', notification);

        addNotificationDirectly(notification);
    });

    priceConnection.start().catch(err => console.error('Price Hub error:', err));



    // ==========================
    // MARK AS READ
    // ==========================
    window.markAsReadNotification = async function (id) {
        try {
            // ✅ حذف من القائمة فوراً (UI أولاً)
            const item = $(`.notification-item[data-id="${id}"]`);
            if (item.length) {
                item.fadeOut(200, function () {
                    $(this).remove();

                    // ✅ تحديث العدد محلياً
                    notificationCount--;
                    if (notificationCount < 0) notificationCount = 0;

                    const index = notificationIds.indexOf(id);
                    if (index !== -1) {
                        notificationIds.splice(index, 1);
                        notificationList.splice(index, 1);
                    }

                    updateUI();
                });
            }

            // ✅ ثم التحديث في الخلفية (Database + Cache)
            const result = await markAsRead(id);

            if (!result.success) {
                console.error('❌ Failed to mark as read in DB:', id);
                // لو فشل، نرجع الإشعار (Rollback UI)
                loadNotifications();
            }
        } catch (error) {
            console.error('❌ Error:', error);
        }
    };

    // ==========================
    // SHOW PRICE CONFIRMATION
    // ==========================
    window.showPriceConfirmation = function (priceDataJson) {
        try {
            const priceData = JSON.parse(decodeURIComponent(priceDataJson));

            let tableHtml = '';
            if (priceData.newData && priceData.newData.length > 0) {
                tableHtml = `
                    <table style="width: 100%; border-collapse: collapse; font-size: 13px; direction: rtl; margin-top: 10px;">
                        <thead>
                            <tr style="background: #f8f9fa; border-bottom: 2px solid #dee2e6;">
                                <th style="padding: 8px; text-align: center;">اليوم</th>
                                <th style="padding: 8px; text-align: center; color: #dc3545;">السعر القديم</th>
                                <th style="padding: 8px; text-align: center; color: #28a745;">السعر الجديد</th>
                                <th style="padding: 8px; text-align: center;">التغيير</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${priceData.newData.map((newPrice) => {
                    const oldPrice = priceData.oldData ? priceData.oldData.find(x => x.Id === newPrice.Id) : {};
                    const morningDiff = (newPrice.MorningPrice || 0) - (oldPrice?.MorningPrice || 0);
                    const hasChange = morningDiff !== 0;

                    return `
                                    <tr style="border-bottom: 1px solid #f0f0f0; ${hasChange ? 'background: #fff8e1;' : ''}">
                                        <td style="padding: 6px; text-align: center; font-weight: bold;">${newPrice.DayDescAr || 'غير محدد'}</td>
                                        <td style="padding: 6px; text-align: center; color: #dc3545; text-decoration: line-through;">
                                            صباح: ${oldPrice?.MorningPrice ?? '-'}<br/>
                                            مساء: ${oldPrice?.EveningPrice ?? '-'}<br/>
                                            كامل: ${oldPrice?.FullDayPrice ?? '-'}
                                        </td>
                                        <td style="padding: 6px; text-align: center; color: #28a745; font-weight: bold;">
                                            صباح: ${newPrice.MorningPrice ?? '-'}<br/>
                                            مساء: ${newPrice.EveningPrice ?? '-'}<br/>
                                            كامل: ${newPrice.FullDayPrice ?? '-'}
                                        </td>
                                        <td style="padding: 6px; text-align: center; color: ${hasChange ? '#ff9800' : '#28a745'};">
                                            ${hasChange ? '🔄 تغير' : '✅ ثابت'}
                                        </td>
                                    </tr>
                                `;
                }).join('')}
                        </tbody>
                    </table>
                `;
            }

            Swal.fire({
                title: '📊 تأكيد تحديث الأسعار',
                html: `
                    <div style="text-align: right;">
                        <div style="margin-bottom: 10px; padding: 10px; background: #f8f9fa; border-radius: 8px;">
                            <strong>👨‍🌾 المزارع: ${priceData.farmerName || 'مزارع'}</strong><br/>
                            <small>🕐 ${new Date(priceData.createdDate).toLocaleString('ar-EG')}</small>
                        </div>
                        <div style="max-height: 400px; overflow-y: auto;">
                            ${tableHtml}
                        </div>
                        <div style="margin-top: 10px; font-size: 12px; color: #6c757d; padding: 5px; background: #f8f9fa; border-radius: 4px;">
                            📋 القديم (مشطوب أحمر) | 📋 الجديد (أخضر)
                        </div>
                        <div style="margin-top: 15px; padding: 10px; background: #fff3cd; border-radius: 8px; border: 1px solid #ffc107;">
                            ⚠️ هل أنت متأكد من تحديث هذه الأسعار؟
                        </div>
                    </div>
                `,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#28a745',
                cancelButtonColor: '#dc3545',
                confirmButtonText: '✅ تأكيد التحديث',
                cancelButtonText: '❌ إلغاء',
                reverseButtons: true,
                width: 850,
                showLoaderOnConfirm: true,
                preConfirm: async () => {
                    try {
                        const priceListToUpdate = priceData.newData.map(price => ({
                            id: price.Id,
                            farmerId: price.FarmerId || 0,
                            day: price.Day,
                            person: price.Person || null,
                            morningPrice: price.MorningPrice || 0,
                            eveningPrice: price.EveningPrice || 0,
                            fullDayPrice: price.FullDayPrice || 0,
                            offerPrice: price.OfferPrice || 0,
                            offerEveningPrice: price.OfferEveningPrice || 0,
                            offerFullDayPrice: price.OfferFullDayPrice || 0,
                            morningPeriodText: price.MorningPeriodText || null,
                            eveningPeriodText: price.EveningPeriodText || null,
                            fullDayPeriodText: price.FullDayPeriodText || null
                        }));
                        debugger
                        const response = await $.ajax({
                            url: MVC_UPDATE_URL,
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify(priceListToUpdate),
                            timeout: 15000
                        });

                        debugger
                        if (response.success) {
                            debugger
                            // ✅ حذف من القائمة فوراً
                            $(`.notification-item[data-id="${priceData.id}"]`).fadeOut(200, function () {
                                $(this).remove();
                                notificationCount--;
                                if (notificationCount < 0) notificationCount = 0;

                                const index = notificationIds.indexOf(priceData.id);
                                if (index !== -1) {
                                    notificationIds.splice(index, 1);
                                    notificationList.splice(index, 1);
                                }

                                updateUI();
                            });

                            // ✅ تأكيد في الخلفية
                            await confirmNotification(priceData.id, true);

                            return { success: true, message: response.message };
                        } else {
                            throw new Error(response.message || 'فشل التحديث');
                        }
                    } catch (error) {
                        Swal.showValidationMessage(`❌ خطأ: ${error.message}`);
                        return false;
                    }
                }
            }).then((result) => {
                debugger
                if (result.isConfirmed && result.value?.success) {
                    Swal.fire({
                        icon: 'success',
                        title: '✅ تم التحديث بنجاح',
                        timer: 3000,
                        timerProgressBar: true
                    });
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    debugger
                    // ✅ حذف من القائمة فوراً
                    $(`.notification-item[data-id="${priceData.id}"]`).fadeOut(200, function () {
                        $(this).remove();
                        notificationCount--;
                        if (notificationCount < 0) notificationCount = 0;

                        const index = notificationIds.indexOf(priceData.id);
                        if (index !== -1) {
                            notificationIds.splice(index, 1);
                            notificationList.splice(index, 1);
                        }

                        updateUI();
                    });

                    // ✅ إلغاء في الخلفية
                    confirmNotification(priceData.id, false);

                    Swal.fire({
                        icon: 'info',
                        title: '❌ تم الإلغاء',
                        timer: 2000,
                        timerProgressBar: true
                    });
                }
            });

        } catch (e) {
            console.error('❌ Error:', e);
            showToast('حدث خطأ في عرض التفاصيل', 'error');
        }
    };

    // ==========================
    // LOAD NOTIFICATIONS (سريعة)
    // ==========================
    async function loadNotifications() {
        if (isLoading) return;
        isLoading = true;

        try {
            const response = await fetchNotifications();

            if (response.success && response.notifications && response.notifications.length > 0) {
                notificationList = [];
                notificationIds = [];

                response.notifications.forEach(notification => {
                    let html = '';
                    if (notification.type === 'Farm') {
                        html = buildFarmNotification(notification);
                    } else if (notification.type === 'Price') {
                        html = buildPriceNotification(notification);
                    }

                    if (html) {
                        notificationList.push(html);
                        notificationIds.push(notification.id);
                    }
                });

                notificationCount = response.count || response.notifications.length;
                updateUI();

                console.log(`📦 Loaded ${notificationList.length} notifications`);
            } else {
                notificationList = [];
                notificationIds = [];
                notificationCount = 0;
                updateUI();
            }
        } catch (error) {
            console.error('❌ Error loading notifications:', error);
        } finally {
            isLoading = false;
            isFirstLoad = false;
        }
    }

    // ==========================
    // UPDATE UI (سريعة)
    // ==========================
    function updateUI() {
        if (!countElement || !listElement) return;

        countElement.textContent = notificationCount;
        countElement.style.display = notificationCount > 0 ? "inline-block" : "none";

        // ✅ لو القائمة مفتوحة، نحدث المحتوى
        if (isDropdownOpen) {
            listElement.innerHTML = "";

            if (notificationList.length === 0) {
                listElement.innerHTML = '<li class="noti-empty">لا توجد إشعارات</li>';
                return;
            }

            notificationList.forEach((html, index) => {
                const li = document.createElement("li");
                li.className = "noti-item";
                li.setAttribute('data-index', index);
                li.innerHTML = html;
                listElement.appendChild(li);
            });

            const clearLi = document.createElement("li");
            clearLi.className = "noti-clear";
            clearLi.innerHTML = `
                <div style="display: flex; gap: 10px; justify-content: center; flex-wrap: wrap;">
                    <button onclick="markAllAsReadNotifications()" 
                            style="background: #28a745; color: white; border: none; padding: 5px 15px; border-radius: 4px; cursor: pointer; font-size: 12px;">
                        ✅ تعيين الكل كمقروء
                    </button>
                    <a href="/MazraeatiBackOffice/Notification/Index" target="_blank"
                       style="background: #17a2b8; color: white; border: none; padding: 5px 15px; border-radius: 4px; cursor: pointer; font-size: 12px; text-decoration: none;">
                        📋 عرض الكل
                    </a>
                </div>
            `;
            listElement.appendChild(clearLi);
        }
    }

    // ==========================
    // MARK ALL AS READ
    // ==========================
    window.markAllAsReadNotifications = async function () {
        try {
            // ✅ حذف الكل من القائمة فوراً
            notificationList = [];
            notificationIds = [];
            notificationCount = 0;
            updateUI();

            // ✅ تحديث في الخلفية
            const result = await markAllAsRead();
            if (!result.success) {
                console.error('❌ Failed to mark all as read in DB');
                loadNotifications();
            } else {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: '✅ تم تعيين الكل كمقروء',
                    timer: 2000,
                    timerProgressBar: true
                });
            }
        } catch (error) {
            console.error('❌ Error:', error);
        }
    };

    // ==========================
    // TOGGLE NOTIFICATIONS
    // ==========================
    window.toggleNotifications = function () {
        if (!dropdown) return;

        isDropdownOpen = !isDropdownOpen;
        dropdown.classList.toggle("active");

        if (isDropdownOpen) {
            notificationCount = 0;
            if (countElement) {
                countElement.style.display = "none";
            }
            // ✅ تحميل سريع (ياخد 1-2 ثانية)
            loadNotifications();
        }
    };

    // ==========================
    // HELPERS
    // ==========================
    function showToast(message, type = 'success') {
        if (typeof Swal === 'undefined') return;
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: type,
            html: message,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }

    // ==========================
    // CLOSE ON OUTSIDE CLICK
    // ==========================
    document.addEventListener("click", function (e) {
        if (!wrapper) return;
        if (!wrapper.contains(e.target)) {
            if (dropdown) {
                dropdown.classList.remove("active");
                isDropdownOpen = false;
            }
        }
    });

    // ==========================
    // KEYBOARD SHORTCUTS
    // ==========================
    document.addEventListener("keydown", function (e) {
        if (e.key === 'Escape' && dropdown) {
            dropdown.classList.remove('active');
            isDropdownOpen = false;
        }
    });

    // ==========================
    // INIT
    // ==========================
    // ✅ أول تحميل (مرة واحدة)
    loadNotifications();

    // ✅ تحديث العدد كل 30 ثانية (خفيف)
    setInterval(async function () {
        try {
            const response = await fetchNotificationsCount();
            if (response.success && response.count !== notificationCount) {
                notificationCount = response.count;
                updateUI();
            }
        } catch (error) {
            console.error('❌ Error updating count:', error);
        }
    }, 30000);

    console.log('✅ Notifications system loaded successfully (Fast)');

});