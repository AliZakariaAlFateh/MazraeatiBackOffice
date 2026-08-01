//// ==========================
//// Start SignalR
//// ==========================
// ==========================
// notifications.js
// SignalR Notifications - External File
// ==========================


// While Page be Ready

// ==========================
// notifications.js
// SignalR Notifications - With LocalStorage & FarmId
// ==========================


//$(document).ready(function () {

//    // ==========================
//    // CONFIGURATION
//    // ==========================
//    const API_URL = window.location.hostname === 'localhost'
//        ? "http://localhost:61366/farmHub"
//        : "http://5.189.180.190/MazareatiAPI/farmHub";

//    const STORAGE_KEY = 'farm_notifications';
//    const EXPIRE_DAYS = 3; // 3 days

//    // ==========================
//    // STATE
//    // ==========================
//    let notificationCount = 0;
//    let notificationList = [];

//    // ==========================
//    // DOM REFS
//    // ==========================
//    const countElement = document.getElementById("notificationCount");
//    const listElement = document.getElementById("notiItems");
//    const dropdown = document.getElementById("notificationList");
//    const wrapper = document.querySelector(".notification-wrapper");

//    // ==========================
//    // LOCALSTORAGE HELPERS
//    // ==========================
//    function getStoredNotifications() {
//        try {
//            const data = localStorage.getItem(STORAGE_KEY);
//            if (!data) return [];

//            const parsed = JSON.parse(data);

//            // Filter expired notifications
//            const now = new Date().getTime();
//            const valid = parsed.filter(item => {
//                return (now - item.timestamp) < (EXPIRE_DAYS * 24 * 60 * 60 * 1000);
//            });

//            // If some expired, update storage
//            if (valid.length !== parsed.length) {
//                saveNotifications(valid);
//            }

//            return valid;
//        } catch (e) {
//            console.error('Error reading localStorage:', e);
//            return [];
//        }
//    }

//    function saveNotifications(notifications) {
//        try {
//            localStorage.setItem(STORAGE_KEY, JSON.stringify(notifications));
//        } catch (e) {
//            console.error('Error saving to localStorage:', e);
//        }
//    }

//    function addToStorage(message, farmData) {
//        const notifications = getStoredNotifications();
//        notifications.unshift({
//            message: message,
//            farmData: farmData,
//            timestamp: new Date().getTime()
//        });
//        saveNotifications(notifications);
//    }

//    function clearStorage() {
//        localStorage.removeItem(STORAGE_KEY);
//    }

//    // ==========================
//    // GET FARMER ID FROM EXTRA FEATURES
//    // ==========================
//    function getFarmerIdFromExtraFeatures(extraFeatures) {
//        if (!extraFeatures || extraFeatures.length === 0) {
//            return null;
//        }

//        // Try to get farmerId from first extra feature
//        const firstFeature = extraFeatures[0];
//        return firstFeature?.farmerId || null;
//    }

//    // ==========================
//    // BUILD NOTIFICATION MESSAGE
//    // ==========================
//    function buildNotificationMessage(farm) {
//        const farmName = farm.name ?? farm.Name ?? 'غير معروف';
//        const location = farm.locationDesc ?? farm.LocationDesc ?? '';

//        // Get farmerId from extraFeatures (if exists)
//        let farmerId = null;
//        if (farm.extraFeatures && Array.isArray(farm.extraFeatures) && farm.extraFeatures.length > 0) {
//            farmerId = getFarmerIdFromExtraFeatures(farm.extraFeatures);
//        }

//        // Build the message
//        let message = `✅ تم اضافة مزرعة : <strong>${farmName}</strong>`;

//        if (location) {
//            message += `<br/><small>📍 ${location}</small>`;
//        }

//        // Add link based on farmerId
//        if (farmerId) {
//            // لو فيه farmerId → رابط التعديل
//            message += `<br/><small>🔗 <a href='/MazraeatiBackOffice/Farmers/Edit/${farmerId}' target='_blank' style='color: #4CAF50; text-decoration: underline;'>تعديل المزرعة</a></small>`;
//        } else {
//            // لو مفيش farmerId → رابط عرض جميع المزارع
//            message += `<br/><small>🔗 <a href='/MazraeatiBackOffice/Farmers/Index' target='_blank' style='color: #2196F3; text-decoration: underline;'>عرض جميع المزارع</a></small>`;
//        }

//        return message;
//    }

//    // ==========================
//    // LOAD FROM LOCALSTORAGE
//    // ==========================
//    function loadFromStorage() {
//        const stored = getStoredNotifications();

//        if (stored.length > 0) {
//            // Clear current list
//            notificationList = [];

//            // Load from storage and rebuild messages
//            stored.forEach(item => {
//                if (item.farmData) {
//                    const message = buildNotificationMessage(item.farmData);
//                    notificationList.push(message);
//                } else {
//                    notificationList.push(item.message);
//                }
//            });

//            notificationCount = notificationList.length;
//            updateNotificationUI();
//            console.log(`📦 Loaded ${notificationCount} notifications from localStorage`);
//        }
//    }

//    // ==========================
//    // SIGNALR
//    // ==========================
//    const connection = new signalR.HubConnectionBuilder()
//        .withUrl(API_URL)
//        .withAutomaticReconnect()
//        .build();

//    connection.on("FarmAdded", function (farm) {
//        console.log('📨 New Farm Added:', farm);

//        // Build message
//        const message = buildNotificationMessage(farm);

//        // Save farm data for later use (keep all data)
//        const farmData = {
//            name: farm.name ?? farm.Name ?? 'غير معروف',
//            locationDesc: farm.locationDesc ?? farm.LocationDesc ?? '',
//            extraFeatures: farm.extraFeatures || []
//        };

//        // Add to list
//        notificationList.unshift(message);
//        notificationCount++;

//        // Save to localStorage with farm data
//        addToStorage(message, farmData);

//        // Update UI
//        updateNotificationUI();
//        showToast(message);
//    });

//    connection.start().catch(err => console.error('SignalR Error:', err));

//    // ==========================
//    // UI UPDATE
//    // ==========================
//    function updateNotificationUI() {
//        if (!countElement || !listElement) return;

//        countElement.textContent = notificationCount;
//        countElement.style.display = notificationCount > 0 ? "inline-block" : "none";

//        listElement.innerHTML = "";

//        if (notificationList.length === 0) {
//            listElement.innerHTML = '<li class="noti-empty">لا توجد إشعارات</li>';
//            return;
//        }

//        notificationList.forEach(n => {
//            const li = document.createElement("li");
//            li.innerHTML = n;
//            listElement.appendChild(li);
//        });

//        // Add clear button
//        const clearBtn = document.createElement("li");
//        clearBtn.className = "noti-clear-btn";
//        clearBtn.innerHTML = `<button onclick="clearAllNotifications()">🗑️ مسح الكل</button>`;
//        listElement.appendChild(clearBtn);
//    }

//    // ==========================
//    // TOGGLE
//    // ==========================
//    window.toggleNotifications = function () {
//        if (!dropdown) return;

//        dropdown.classList.toggle("active");

//        if (dropdown.classList.contains("active")) {
//            notificationCount = 0;
//            if (countElement) {
//                countElement.style.display = "none";
//            }
//        }
//    };

//    // ==========================
//    // CLEAR ALL - SweetAlert2
//    // ==========================
//    window.clearAllNotifications = function () {
//        if (notificationList.length === 0) return;

//        Swal.fire({
//            title: '🧹 مسح الإشعارات',
//            text: 'هل أنت متأكد من مسح جميع الإشعارات؟',
//            icon: 'question',
//            showCancelButton: true,
//            confirmButtonColor: '#d33',
//            cancelButtonColor: '#3085d6',
//            confirmButtonText: 'نعم، امسح الكل',
//            cancelButtonText: 'إلغاء',
//            reverseButtons: true
//        }).then((result) => {
//            if (result.isConfirmed) {
//                notificationList = [];
//                notificationCount = 0;
//                clearStorage();
//                updateNotificationUI();
//                dropdown.classList.remove('active');

//                Swal.fire({
//                    toast: true,
//                    position: 'top-end',
//                    icon: 'success',
//                    title: '✅ تم مسح جميع الإشعارات',
//                    showConfirmButton: false,
//                    timer: 2000,
//                    timerProgressBar: true
//                });
//            }
//        });
//    };

//    // ==========================
//    // CLOSE ON OUTSIDE CLICK
//    // ==========================
//    document.addEventListener("click", function (e) {
//        if (!wrapper) return;

//        if (!wrapper.contains(e.target)) {
//            if (dropdown) {
//                dropdown.classList.remove("active");
//            }
//        }
//    });

//    // ==========================
//    // SWEET TOAST
//    // ==========================
//    function showToast(message, type = 'success') {
//        if (typeof Swal === 'undefined') {
//            console.log('🔔', message);
//            return;
//        }

//        Swal.fire({
//            toast: true,
//            position: 'top-end',
//            icon: type,
//            html: message,
//            showConfirmButton: false,
//            timer: 3000,
//            timerProgressBar: true
//        });
//    }

//    // ==========================
//    // KEYBOARD SHORTCUTS
//    // ==========================
//    document.addEventListener("keydown", function (e) {
//        if (e.key === 'Escape' && dropdown) {
//            dropdown.classList.remove('active');
//        }
//    });

//    // ==========================
//    // INIT - LOAD FROM STORAGE
//    // ==========================
//    loadFromStorage();

//    // Check expired every hour
//    setInterval(() => {
//        const stored = getStoredNotifications();
//        if (stored.length !== notificationList.length) {
//            loadFromStorage();
//        }
//    }, 60 * 60 * 1000);

//    console.log('✅ Notifications system loaded successfully');
//    console.log(`📦 ${notificationList.length} notifications loaded from storage`);

//}); // end document ready























//All in One .....
// signalr_notification.js

//$(document).ready(function () {

//    // ==========================
//    // CONFIGURATION
//    // ==========================
//    const FARM_API_URL = window.location.hostname === 'localhost'
//        ? "http://localhost:61366/farmHub"
//        : "http://5.189.180.190/MazareatiAPI/farmHub";

//    const PRICE_API_URL = window.location.hostname === 'localhost'
//        ? "http://localhost:61366/priceHub"
//        : "http://5.189.180.190/MazareatiAPI/priceHub";

//    const STORAGE_KEY = 'all_notifications';
//    const EXPIRE_DAYS = 3;

//    // ==========================
//    // STATE
//    // ==========================
//    let notificationCount = 0;
//    let notificationList = [];

//    // ==========================
//    // DOM REFS
//    // ==========================
//    const countElement = document.getElementById("notificationCount");
//    const listElement = document.getElementById("notiItems");
//    const dropdown = document.getElementById("notificationList");
//    const wrapper = document.querySelector(".notification-wrapper");

//    console.log('🚀 SignalR Notifications System Starting...');
//    console.log('📌 DOM Elements found:', {
//        countElement: !!countElement,
//        listElement: !!listElement,
//        dropdown: !!dropdown,
//        wrapper: !!wrapper
//    });

//    // ==========================
//    // LOCALSTORAGE HELPERS
//    // ==========================
//    function getStoredNotifications() {
//        try {
//            const data = localStorage.getItem(STORAGE_KEY);
//            if (!data) return [];

//            const parsed = JSON.parse(data);
//            const now = new Date().getTime();
//            const valid = parsed.filter(item => {
//                return (now - item.timestamp) < (EXPIRE_DAYS * 24 * 60 * 60 * 1000);
//            });

//            if (valid.length !== parsed.length) {
//                saveNotifications(valid);
//            }

//            return valid;
//        } catch (e) {
//            console.error('Error reading localStorage:', e);
//            return [];
//        }
//    }

//    function saveNotifications(notifications) {
//        try {
//            localStorage.setItem(STORAGE_KEY, JSON.stringify(notifications));
//        } catch (e) {
//            console.error('Error saving to localStorage:', e);
//        }
//    }

//    function addToStorage(message, data, type) {
//        const notifications = getStoredNotifications();
//        notifications.unshift({
//            message: message,
//            data: data,
//            type: type,
//            timestamp: new Date().getTime()
//        });
//        saveNotifications(notifications);
//    }

//    function clearStorage() {
//        localStorage.removeItem(STORAGE_KEY);
//    }

//    // ==========================
//    // BUILD NOTIFICATION MESSAGE - FARM
//    // ==========================
//    function buildFarmNotificationMessage(farm) {
//        console.log('🏗️ Building farm notification for:', farm);

//        const farmName = farm.name || farm.Name || 'غير معروف';
//        const location = farm.locationDesc || farm.LocationDesc || '';

//        let message = `
//            <div class="notification-item farm-notification">
//                <div class="noti-icon">🌾</div>
//                <div class="noti-content">
//                    <div class="noti-title">✅ تم اضافة مزرعة</div>
//                    <div class="noti-text"><strong>${farmName}</strong></div>
//                    ${location ? `<div class="noti-location">📍 ${location}</div>` : ''}
//                    <div class="noti-time">🕐 ${new Date().toLocaleString('ar-EG')}</div>
//                </div>
//            </div>
//        `;

//        // رابط التعديل
//        message += `
//            <div class="noti-action">
//                <a href='/MazraeatiBackOffice/Farmers/Index' target='_blank'
//                   style='color: #2196F3; text-decoration: none; font-size: 12px;'>
//                    🔗 عرض جميع المزارع
//                </a>
//            </div>
//        `;

//        return message;
//    }

//    // ==========================
//    // BUILD NOTIFICATION MESSAGE - PRICE
//    // ==========================
//    function buildPriceNotificationMessage(priceData) {
//        console.log('🏗️ Building price notification for:', priceData);

//        const farmerName = priceData.farmerName || 'مزارع';
//        const farmName = priceData.farmName || 'مزرعة';
//        const totalCount = priceData.totalCount || 0;

//        let changedCount = 0;
//        let increasedCount = 0;
//        let decreasedCount = 0;

//        if (priceData.priceDiffs && priceData.priceDiffs.length > 0) {
//            changedCount = priceData.priceDiffs.filter(d => d.hasChanges).length;
//            increasedCount = priceData.priceDiffs.filter(d => d.morningDiff > 0 || d.eveningDiff > 0 || d.fullDayDiff > 0).length;
//            decreasedCount = priceData.priceDiffs.filter(d => d.morningDiff < 0 || d.eveningDiff < 0 || d.fullDayDiff < 0).length;
//        }

//        let message = `
//            <div class="notification-item price-notification">
//                <div class="noti-icon">📊</div>
//                <div class="noti-content">
//                    <div class="noti-title">🔄 تحديث قائمة الأسعار</div>
//                    <div class="noti-text">
//                        <div class="farmer-name">👨‍🌾 ${farmerName}</div>
//                        <div class="farm-name">🏠 ${farmName}</div>
//                    </div>
//                    <div class="price-summary">
//                        <span class="total">📊 ${totalCount} سعر</span>
//                        ${changedCount > 0 ? `<span class="changed">🔄 ${changedCount} تغير</span>` : ''}
//                        ${increasedCount > 0 ? `<span class="increase">📈 ${increasedCount} زيادة</span>` : ''}
//                        ${decreasedCount > 0 ? `<span class="decrease">📉 ${decreasedCount} انخفاض</span>` : ''}
//                    </div>
//                    <div class="noti-time">🕐 ${new Date(priceData.updatedDate).toLocaleString('ar-EG')}</div>
//                </div>
//            </div>
//        `;

//        // زر عرض التفاصيل
//        const encodedData = encodeURIComponent(JSON.stringify(priceData));
//        message += `
//            <div class="noti-action">
//                <button onclick="showPriceDetails('${encodedData}')"
//                        style="color: #2196F3; background: none; border: none; cursor: pointer; font-size: 12px; padding: 5px 10px;">
//                    📋 عرض التفاصيل
//                </button>
//            </div>
//        `;

//        return message;
//    }

//    // ==========================
//    // SHOW PRICE DETAILS IN MODAL
//    // ==========================
//    window.showPriceDetails = function (priceDataJson) {
//        try {
//            const priceData = JSON.parse(decodeURIComponent(priceDataJson));
//            console.log('📋 Showing price details:', priceData);

//            let tableHtml = '';
//            if (priceData.newPrices && priceData.newPrices.length > 0) {
//                tableHtml = `
//                    <table style="width: 100%; border-collapse: collapse; font-size: 13px; direction: rtl; margin-top: 10px;">
//                        <thead>
//                            <tr style="background: #f8f9fa; border-bottom: 2px solid #dee2e6;">
//                                <th style="padding: 8px; text-align: center;">اليوم</th>
//                                <th style="padding: 8px; text-align: center; color: #dc3545;">القديم</th>
//                                <th style="padding: 8px; text-align: center; color: #28a745;">الجديد</th>
//                                <th style="padding: 8px; text-align: center;">التغيير</th>
//                            </tr>
//                        </thead>
//                        <tbody>
//                            ${priceData.newPrices.map((newPrice) => {
//                    const oldPrice = priceData.oldPrices ? priceData.oldPrices.find(x => x.id === newPrice.id) : {};
//                    const diff = priceData.priceDiffs ? priceData.priceDiffs.find(x => x.id === newPrice.id) : null;
//                    const hasChange = diff && diff.hasChanges;

//                    return `
//                                    <tr style="border-bottom: 1px solid #f0f0f0; ${hasChange ? 'background: #fff8e1;' : ''}">
//                                        <td style="padding: 6px; text-align: center; font-weight: bold;">${newPrice.dayDescAr}</td>
//                                        <td style="padding: 6px; text-align: center; color: #dc3545; text-decoration: line-through;">
//                                            ${oldPrice && oldPrice.morningPrice ? oldPrice.morningPrice : '-'}
//                                            /
//                                            ${oldPrice && oldPrice.eveningPrice ? oldPrice.eveningPrice : '-'}
//                                            /
//                                            ${oldPrice && oldPrice.fullDayPrice ? oldPrice.fullDayPrice : '-'}
//                                        </td>
//                                        <td style="padding: 6px; text-align: center; color: #28a745; font-weight: bold;">
//                                            ${newPrice.morningPrice}
//                                            /
//                                            ${newPrice.eveningPrice}
//                                            /
//                                            ${newPrice.fullDayPrice}
//                                        </td>
//                                        <td style="padding: 6px; text-align: center;">
//                                            ${hasChange ?
//                            `<span style="color: #ff9800;">🔄 تغير</span>` :
//                            `<span style="color: #28a745;">✅ ثابت</span>`
//                        }
//                                        </td>
//                                    </tr>
//                                `;
//                }).join('')}
//                        </tbody>
//                    </table>
//                `;
//            }

//            Swal.fire({
//                title: `📊 تفاصيل تحديث الأسعار - ${priceData.farmerName || 'مزارع'}`,
//                html: `
//                    <div style="text-align: right;">
//                        <div style="margin-bottom: 10px; padding: 10px; background: #f8f9fa; border-radius: 8px;">
//                            <strong>👨‍🌾 ${priceData.farmerName || 'مزارع'}</strong><br/>
//                            <strong>🏠 ${priceData.farmName || 'مزرعة'}</strong><br/>
//                            <small>🕐  ${new Date(priceData.updatedDate).toLocaleString('ar-EG')}</small>
//                        </div>
//                        <div style="max-height: 400px; overflow-y: auto;">
//                            ${tableHtml}
//                        </div>
//                        <div style="margin-top: 10px; font-size: 12px; color: #6c757d; padding: 5px; background: #f8f9fa; border-radius: 4px;">
//                            📋 القديم (مشطوب) | 📋 الجديد (أخضر) | 🔄 تغير | ✅ ثابت
//                        </div>
//                    </div>
//                `,
//                icon: 'info',
//                confirmButtonText: 'إغلاق',
//                width: 750,
//                confirmButtonColor: '#2196F3'
//            });
//        } catch (e) {
//            console.error('❌ Error showing price details:', e);
//            showToast('حدث خطأ في عرض التفاصيل', 'error');
//        }
//    };

//    // ==========================
//    // LOAD FROM LOCALSTORAGE
//    // ==========================
//    function loadFromStorage() {
//        const stored = getStoredNotifications();
//        console.log('📦 Loading from storage:', stored.length);

//        if (stored.length > 0) {
//            notificationList = [];

//            stored.forEach(item => {
//                let message = '';
//                if (item.type === 'farm' && item.data) {
//                    message = buildFarmNotificationMessage(item.data);
//                } else if (item.type === 'price' && item.data) {
//                    message = buildPriceNotificationMessage(item.data);
//                } else if (item.message) {
//                    message = item.message;
//                }

//                if (message) {
//                    notificationList.push(message);
//                }
//            });

//            notificationCount = notificationList.length;
//            updateNotificationUI();
//            console.log(`📦 Loaded ${notificationCount} notifications from localStorage`);
//        }
//    }

//    // ==========================
//    // SIGNALR - FARM CONNECTION
//    // ==========================
//    console.log('📡 Connecting to Farm Hub...');
//    const farmConnection = new signalR.HubConnectionBuilder()
//        .withUrl(FARM_API_URL)
//        .withAutomaticReconnect()
//        .build();

//    // مستمع إضافة مزرعة
//    farmConnection.on("FarmAdded", function (farm) {
//        console.log('🌾✅ FarmAdded event received!', farm);

//        try {
//            // بناء الرسالة
//            const message = buildFarmNotificationMessage(farm);

//            // إضافة للإشعارات
//            notificationList.unshift(message);
//            notificationCount++;

//            // حفظ في localStorage
//            const farmData = {
//                name: farm.name || farm.Name || 'غير معروف',
//                locationDesc: farm.locationDesc || farm.LocationDesc || '',
//                extraFeatures: farm.extraFeatures || []
//            };
//            addToStorage(message, farmData, 'farm');

//            // تحديث الواجهة
//            updateNotificationUI();

//            // عرض Toast
//            const farmName = farm.name || farm.Name || 'مزرعة';
//            showToast(`🌾 تم إضافة مزرعة: ${farmName}`, 'success');

//            console.log('✅ Farm notification added successfully');
//        } catch (error) {
//            console.error('❌ Error processing farm notification:', error);
//        }
//    });

//    // بدء اتصال Farm
//    farmConnection.start()
//        .then(() => {
//            console.log('✅ Farm Hub connected successfully to:', FARM_API_URL);
//        })
//        .catch(err => {
//            console.error('❌ Farm Hub connection error:', err);
//        });

//    // ==========================
//    // SIGNALR - PRICE CONNECTION
//    // ==========================
//    console.log('📡 Connecting to Price Hub...');
//    const priceConnection = new signalR.HubConnectionBuilder()
//        .withUrl(PRICE_API_URL)
//        .withAutomaticReconnect()
//        .build();

//    // مستمع تحديث الأسعار
//    priceConnection.on("PricesBatchUpdated", function (batchData) {
//        debugger
//        console.log("Ia any thing of prices updated !!!!!!!!!!!!!")
//        console.log(batchData)
//        console.log('📊✅ PricesBatchUpdated event received!', batchData);

//        try {
//            // بناء الرسالة
//            const message = buildPriceNotificationMessage(batchData);

//            // إضافة للإشعارات
//            notificationList.unshift(message);
//            notificationCount++;

//            // حفظ في localStorage
//            addToStorage(message, batchData, 'price');

//            // تحديث الواجهة
//            updateNotificationUI();

//            // عرض Toast مع ملخص
//            const changedCount = batchData.priceDiffs ? batchData.priceDiffs.filter(d => d.hasChanges).length : 0;
//            const totalCount = batchData.totalCount || 0;
//            const farmerName = batchData.farmerName || 'مزارع';

//            showToast(
//                `📊 تم تحديث أسعار ${farmerName}<br/>${totalCount} سعر ${changedCount > 0 ? `(${changedCount} تغيير)` : ''}`,
//                changedCount > 0 ? 'info' : 'success'
//            );

//            console.log('✅ Price notification added successfully');
//        } catch (error) {
//            console.error('❌ Error processing price notification:', error);
//        }
//    });

//    // مستمع إضافة سعر فردي (اختياري)
//    priceConnection.on("PriceAdded", function (priceData) {
//        console.log('💰 PriceAdded event received:', priceData);
//        // يمكنك إضافة معالجة هنا لو حبيت
//    });

//    // مستمع تحديث سعر فردي (اختياري)
//    priceConnection.on("PriceUpdated", function (priceData) {
//        console.log('💰 PriceUpdated event received:', priceData);
//        // يمكنك إضافة معالجة هنا لو حبيت
//    });

//    // بدء اتصال Price
//    priceConnection.start()
//        .then(() => {
//            console.log('✅ Price Hub connected successfully to:', PRICE_API_URL);
//        })
//        .catch(err => {
//            console.error('❌ Price Hub connection error:', err);
//        });

//    // ==========================
//    // UI UPDATE
//    // ==========================
//    function updateNotificationUI() {
//        console.log('🔄 Updating UI, count:', notificationCount);

//        if (!countElement || !listElement) {
//            console.error('❌ DOM elements not found!');
//            return;
//        }

//        // تحديث العدد
//        countElement.textContent = notificationCount;
//        countElement.style.display = notificationCount > 0 ? "inline-block" : "none";

//        // تحديث القائمة
//        listElement.innerHTML = "";

//        if (notificationList.length === 0) {
//            listElement.innerHTML = '<li class="noti-empty">لا توجد إشعارات</li>';
//            return;
//        }

//        // إضافة كل الإشعارات
//        notificationList.forEach((n, index) => {
//            const li = document.createElement("li");
//            li.className = "noti-item";
//            li.setAttribute('data-index', index);
//            li.innerHTML = n;
//            listElement.appendChild(li);
//        });

//        // زر مسح الكل
//        const clearLi = document.createElement("li");
//        clearLi.className = "noti-clear";
//        clearLi.innerHTML = `<button onclick="clearAllNotifications()" class="clear-btn">🗑️ مسح الكل</button>`;
//        listElement.appendChild(clearLi);

//        console.log('✅ UI updated successfully');
//    }

//    // ==========================
//    // TOGGLE NOTIFICATIONS
//    // ==========================
//    window.toggleNotifications = function () {
//        console.log('🔄 Toggling notifications');

//        if (!dropdown) {
//            console.error('❌ Dropdown element not found!');
//            return;
//        }

//        dropdown.classList.toggle("active");

//        if (dropdown.classList.contains("active")) {
//            // تصفير العداد عند الفتح
//            notificationCount = 0;
//            if (countElement) {
//                countElement.style.display = "none";
//            }
//            console.log('📬 Notifications opened, count reset');
//        }
//    };

//    // ==========================
//    // CLEAR ALL NOTIFICATIONS
//    // ==========================
//    window.clearAllNotifications = function () {
//        console.log('🗑️ Clearing all notifications');

//        if (notificationList.length === 0) {
//            showToast('لا توجد إشعارات لمسحها', 'info');
//            return;
//        }

//        Swal.fire({
//            title: '🧹 مسح الإشعارات',
//            text: 'هل أنت متأكد من مسح جميع الإشعارات؟',
//            icon: 'question',
//            showCancelButton: true,
//            confirmButtonColor: '#d33',
//            cancelButtonColor: '#3085d6',
//            confirmButtonText: 'نعم، امسح الكل',
//            cancelButtonText: 'إلغاء',
//            reverseButtons: true
//        }).then((result) => {
//            if (result.isConfirmed) {
//                notificationList = [];
//                notificationCount = 0;
//                clearStorage();
//                updateNotificationUI();
//                if (dropdown) {
//                    dropdown.classList.remove('active');
//                }

//                Swal.fire({
//                    toast: true,
//                    position: 'top-end',
//                    icon: 'success',
//                    title: '✅ تم مسح جميع الإشعارات',
//                    showConfirmButton: false,
//                    timer: 2000,
//                    timerProgressBar: true
//                });
//                console.log('✅ All notifications cleared');
//            }
//        });
//    };

//    // ==========================
//    // CLOSE ON OUTSIDE CLICK
//    // ==========================
//    document.addEventListener("click", function (e) {
//        if (!wrapper) return;

//        if (!wrapper.contains(e.target)) {
//            if (dropdown) {
//                dropdown.classList.remove("active");
//            }
//        }
//    });

//    // ==========================
//    // SWEET TOAST
//    // ==========================
//    function showToast(message, type = 'success') {
//        console.log('🔔 Toast:', message);

//        if (typeof Swal === 'undefined') {
//            console.log('⚠️ Swal not defined, showing console notification:', message);
//            return;
//        }

//        Swal.fire({
//            toast: true,
//            position: 'top-end',
//            icon: type,
//            html: message,
//            showConfirmButton: false,
//            timer: 4000,
//            timerProgressBar: true
//        });
//    }

//    // ==========================
//    // KEYBOARD SHORTCUTS
//    // ==========================
//    document.addEventListener("keydown", function (e) {
//        if (e.key === 'Escape' && dropdown) {
//            dropdown.classList.remove('active');
//            console.log('🔑 Esc pressed, closing notifications');
//        }
//    });

//    // ==========================
//    // INIT - LOAD FROM STORAGE
//    // ==========================
//    loadFromStorage();

//    // Check expired every hour
//    setInterval(() => {
//        const stored = getStoredNotifications();
//        if (stored.length !== notificationList.length) {
//            console.log('🔄 Refreshing notifications from storage');
//            loadFromStorage();
//        }
//    }, 60 * 60 * 1000);

//    // ==========================
//    // FINAL LOG
//    // ==========================
//    console.log('✅ Notifications system loaded successfully');
//    console.log(`📦 ${notificationList.length} notifications loaded from storage`);
//    console.log('🎯 Listening for FarmAdded and PricesBatchUpdated events');

//}); // end document ready











///All In One
$(document).ready(function () {

    // ==========================
    // CONFIGURATION
    // ==========================
    const FARM_API_URL = window.location.hostname === 'localhost'
        ? "http://localhost:61366/farmHub"
        : "http://5.189.180.190/MazareatiAPI/farmHub";

    const PRICE_API_URL = window.location.hostname === 'localhost'
        ? "http://localhost:61366/priceHub"
        : "http://5.189.180.190/MazareatiAPI/priceHub";

    // MVC API URL للتحديث
    const MVC_UPDATE_URL = window.location.hostname === 'localhost'
        ? "/Farmers/EditPriceList"
        : "/MazraeatiBackOffice/Farmers/EditPriceList";

    const STORAGE_KEY = 'all_notifications';
    const EXPIRE_DAYS = 3;

    // ==========================
    // STATE
    // ==========================
    let notificationCount = 0;
    let notificationList = [];

    // ==========================
    // DOM REFS
    // ==========================
    const countElement = document.getElementById("notificationCount");
    const listElement = document.getElementById("notiItems");
    const dropdown = document.getElementById("notificationList");
    const wrapper = document.querySelector(".notification-wrapper");

    console.log('🚀 SignalR Notifications System Starting...');
    console.log('📌 DOM Elements found:', {
        countElement: !!countElement,
        listElement: !!listElement,
        dropdown: !!dropdown,
        wrapper: !!wrapper
    });

    // ==========================
    // LOCALSTORAGE HELPERS
    // ==========================
    function getStoredNotifications() {
        try {
            const data = localStorage.getItem(STORAGE_KEY);
            if (!data) return [];

            const parsed = JSON.parse(data);
            const now = new Date().getTime();
            const valid = parsed.filter(item => {
                return (now - item.timestamp) < (EXPIRE_DAYS * 24 * 60 * 60 * 1000);
            });

            if (valid.length !== parsed.length) {
                saveNotifications(valid);
            }

            return valid;
        } catch (e) {
            console.error('Error reading localStorage:', e);
            return [];
        }
    }

    function saveNotifications(notifications) {
        try {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(notifications));
        } catch (e) {
            console.error('Error saving to localStorage:', e);
        }
    }

    function addToStorage(message, data, type) {
        const notifications = getStoredNotifications();
        notifications.unshift({
            message: message,
            data: data,
            type: type,
            timestamp: new Date().getTime()
        });
        saveNotifications(notifications);
    }

    function clearStorage() {
        localStorage.removeItem(STORAGE_KEY);
    }

    // ==========================
    // BUILD NOTIFICATION MESSAGE - FARM
    // ==========================
    function buildFarmNotificationMessage(farm) {
        debugger
        const firstFeature = farm.extraFeatures[0];
        console.log("Extra Features ....")
        console.log(firstFeature)
        let farmerId= firstFeature?.farmerId || null;
        console.log('🏗️ Building farm notification for:', farm);

        const farmName = farm.name  || 'غير معروف';
        const location = farm.locationDesc || farm.LocationDesc || '';

        let message = `
            <div class="notification-item farm-notification">
                <div class="noti-icon">🌾</div>
                <div class="noti-content">
                    <div class="noti-title">✅ تم اضافة مزرعة</div>
                    <div class="noti-text"><strong>${farmName}</strong></div>
                    ${location ? `<div class="noti-location">📍 ${location}</div>` : ''}
                    <div class="noti-time">🕐 ${new Date().toLocaleString('ar-EG')}</div>
                </div>
            </div>
        `;

        //message += `
        //    <div class="noti-action">
        //        <a href='/MazraeatiBackOffice/Farmers/Index' target='_blank' 
        //           style='color: #2196F3; text-decoration: none; font-size: 12px;'>
        //            🔗 عرض جميع المزارع
        //        </a>
        //    </div>
        //`;
        if (farmerId) {
            // لو فيه farmerId → رابط التعديل
            message += `<br/><small>🔗 <a href='/MazraeatiBackOffice/Farmers/Edit/${farmerId}' target='_blank' style='color: #4CAF50; text-decoration: underline;'>تعديل المزرعة</a></small>`;
        } else {
            // لو مفيش farmerId → رابط عرض جميع المزارع
            message += `<br/><small>🔗 <a href='/MazraeatiBackOffice/Farmers/Index' target='_blank' style='color: #2196F3; text-decoration: underline;'>عرض جميع المزارع</a></small>`;
        }

        return message;
    }

    // ==========================
    // BUILD NOTIFICATION MESSAGE - PRICE (MODIFIED)
    // ==========================
    function buildPriceNotificationMessage(priceData) {
        console.log('🏗️ Building price notification for:', priceData);

        const farmerName = priceData.farmerName || 'مزارع';
        const farmName = priceData.farmName || 'مزرعة';
        const totalCount = priceData.totalCount || 0;

        let changedCount = 0;
        let increasedCount = 0;
        let decreasedCount = 0;

        if (priceData.priceDiffs && priceData.priceDiffs.length > 0) {
            changedCount = priceData.priceDiffs.filter(d => d.hasChanges).length;
            increasedCount = priceData.priceDiffs.filter(d => d.morningDiff > 0 || d.eveningDiff > 0 || d.fullDayDiff > 0).length;
            decreasedCount = priceData.priceDiffs.filter(d => d.morningDiff < 0 || d.eveningDiff < 0 || d.fullDayDiff < 0).length;
        }

        let message = `
            <div class="notification-item price-notification">
                <div class="noti-icon">📊</div>
                <div class="noti-content">
                    <div class="noti-title">🔄 تحديث قائمة الأسعار</div>
                    <div class="noti-text">
                        <div class="farmer-name">👨‍🌾 ${farmerName}</div>
                        <div class="farm-name">🏠 ${farmName}</div>
                    </div>
                    <div class="price-summary">
                        <span class="total">📊 ${totalCount} سعر</span>
                        ${changedCount > 0 ? `<span class="changed">🔄 ${changedCount} تغير</span>` : ''}
                        ${increasedCount > 0 ? `<span class="increase">📈 ${increasedCount} زيادة</span>` : ''}
                        ${decreasedCount > 0 ? `<span class="decrease">📉 ${decreasedCount} انخفاض</span>` : ''}
                    </div>
                    <div class="noti-time">🕐 ${new Date(priceData.updatedDate).toLocaleString('ar-EG')}</div>
                </div>
            </div>
        `;

        // تخزين البيانات كـ JSON string في الـ onclick
        const encodedData = encodeURIComponent(JSON.stringify(priceData));
        message += `
            <div class="noti-action">
                <button onclick="showPriceConfirmation('${encodedData}')" 
                        style="color: #2196F3; background: none; border: none; cursor: pointer; font-size: 12px; padding: 5px 10px;">
                    📋 مراجعة وتأكيد التحديث
                </button>
            </div>
        `;

        return message;
    }

    // ==========================
    // SHOW PRICE CONFIRMATION WITH TWO BUTTONS (NEW LOGIC)
    // ==========================
    window.showPriceConfirmation = function (priceDataJson) {
        try {
            const priceData = JSON.parse(decodeURIComponent(priceDataJson));
            console.log('📋 Showing price confirmation:', priceData);
            console.log("Old Prices ...")
            console.log(priceData.oldData)
            console.log("New Prices ...")
            console.log(priceData.newData)
            // بناء جدول المقارنة
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
                                const oldPrice = priceData.oldData ? priceData.oldData.find(x => x.id === newPrice.id) : {};
                            const diff = priceData.priceDiffs ? priceData.priceDiffs.find(x => x.id === newPrice.id) : null;
                            const hasChange = diff && diff.hasChanges;

                            return `
                                    <tr style="border-bottom: 1px solid #f0f0f0; ${hasChange ? 'background: #fff8e1;' : ''}">
                                        <td style="padding: 6px; text-align: center; font-weight: bold;">${newPrice.dayDescAr || newPrice.day || 'غير محدد'}</td>
                                        <td style="padding: 6px; text-align: center; color: #dc3545; text-decoration: line-through;">
                                            صباح: ${oldPrice && oldPrice.morningPrice ? oldPrice.morningPrice : '-'}<br/>
                                            مساء: ${oldPrice && oldPrice.eveningPrice ? oldPrice.eveningPrice : '-'}<br/>
                                            يوم كامل: ${oldPrice && oldPrice.fullDayPrice ? oldPrice.fullDayPrice : '-'}
                                        </td>
                                        <td style="padding: 6px; text-align: center; color: #28a745; font-weight: bold;">
                                            صباح: ${newPrice.morningPrice || '-'}<br/>
                                            مساء: ${newPrice.eveningPrice || '-'}<br/>
                                            يوم كامل: ${newPrice.fullDayPrice || '-'}
                                        </td>
                                        <td style="padding: 6px; text-align: center;">
                                            ${hasChange ?
                            `<span style="color: #ff9800;">🔄 تغير</span>` :
                            `<span style="color: #28a745;">✅ ثابت</span>`
                        }
                                        </td>
                                    </tr>
                                `;
                }).join('')}
                        </tbody>
                    </table>
                `;
            }

            // عرض النافذة مع زرين
            Swal.fire({
                title: `📊 تأكيد تحديث الأسعار`,
                html: `
                    <div style="text-align: right;">
                        <div style="margin-bottom: 10px; padding: 10px; background: #f8f9fa; border-radius: 8px;">
                            <strong>👨‍🌾 المزارع: ${priceData.farmerName || 'مزارع'}</strong><br/>
                            <strong>🏠 المزرعة: ${priceData.farmName || 'مزرعة'}</strong><br/>
                            <small>🕐 ${new Date(priceData.updatedDate).toLocaleString('ar-EG')}</small>
                        </div>
                        <div style="max-height: 400px; overflow-y: auto;">
                            ${tableHtml}
                        </div>
                        <div style="margin-top: 10px; font-size: 12px; color: #6c757d; padding: 5px; background: #f8f9fa; border-radius: 4px;">
                            📋 القديم (مشطوب أحمر) | 📋 الجديد (أخضر) | 🔄 تغير | ✅ ثابت
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
                    debugger
                    try {
                        // تحويل الـ newPrices إلى List<FarmerPriceList>
                        const priceListToUpdate = priceData.newData.map(price => ({
                            id: price.id,
                            farmerId: price.farmerId || priceData.farmerId,
                            day: price.day,
                            person: price.person || null,
                            morningPrice: price.morningPrice || null,
                            eveningPrice: price.eveningPrice || null,
                            fullDayPrice: price.fullDayPrice || null,
                            offerPrice: price.offerPrice || null,
                            offerEveningPrice: price.offerEveningPrice || null,
                            offerFullDayPrice: price.offerFullDayPrice || null,
                            morningPeriodText: price.morningPeriodText || null,
                            eveningPeriodText: price.eveningPeriodText || null,
                            fullDayPeriodText: price.fullDayPeriodText || null
                        }));

                        console.log('📤 Sending to MVC:', priceListToUpdate);

                        // استدعاء الـ MVC Action
                        const response = await $.ajax({
                            url: MVC_UPDATE_URL,
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify(priceListToUpdate),
                            //headers: {
                            //    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() || ''
                            //}
                        });

                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: '✅ تم التحديث بنجاح',
                                text: `تم تحديث ${priceListToUpdate.length} سعر`,
                                timer: 3000,
                                timerProgressBar: true
                            });

                            // إضافة إشعار نجاح في القائمة
                            const successMessage = `
                                <div class="notification-item price-notification success">
                                    <div class="noti-icon">✅</div>
                                    <div class="noti-content">
                                        <div class="noti-title">تم تحديث الأسعار بنجاح</div>
                                        <div class="noti-text">
                                            <strong>${priceData.farmerName}</strong> - ${priceListToUpdate.length} سعر
                                        </div>
                                        <div class="noti-time">🕐 ${new Date().toLocaleString('ar-EG')}</div>
                                    </div>
                                </div>
                            `;

                            // إضافة للإشعارات
                            notificationList.unshift(successMessage);
                            notificationCount++;
                            updateNotificationUI();

                            return true;
                        } else {
                            throw new Error(response.message || 'فشل التحديث');
                        }
                    } catch (error) {
                        console.error('❌ Update error:', error);
                        Swal.showValidationMessage(`❌ خطأ: ${error.message}`);
                        return false;
                    }
                }
            }).then((result) => {
                if (result.isDismissed && result.dismiss === Swal.DismissReason.cancel) {
                    // المستخدم ألغى العملية
                    Swal.fire({
                        icon: 'info',
                        title: 'تم الإلغاء',
                        text: 'لم يتم تحديث الأسعار',
                        timer: 2000,
                        timerProgressBar: true
                    });
                }
            });

        } catch (e) {
            console.error('❌ Error showing price confirmation:', e);
            showToast('حدث خطأ في عرض التفاصيل', 'error');
        }
    };

    // ==========================
    // LOAD FROM LOCALSTORAGE
    // ==========================
    function loadFromStorage() {
        debugger
        const stored = getStoredNotifications();
        console.log('📦 Loading from storage:', stored.length);

        if (stored.length > 0) {
            notificationList = [];

            stored.forEach(item => {
                let message = '';
                if (item.type === 'farm' && item.data) {
                    message = buildFarmNotificationMessage(item.data);
                } else if (item.type === 'price' && item.data) {
                    message = buildPriceNotificationMessage(item.data);
                } else if (item.message) {
                    message = item.message;
                }

                if (message) {
                    notificationList.push(message);
                }
            });

            notificationCount = notificationList.length;
            updateNotificationUI();
            console.log(`📦 Loaded ${notificationCount} notifications from localStorage`);
        }
    }

    // ==========================
    // SIGNALR - FARM CONNECTION
    // ==========================
    console.log('📡 Connecting to Farm Hub...');
    const farmConnection = new signalR.HubConnectionBuilder()
        .withUrl(FARM_API_URL)
        .withAutomaticReconnect()
        .build();

    farmConnection.on("FarmAdded", function (farm) {
        console.log('🌾✅ FarmAdded event received!', farm);
        console.log("farm data only ..... ")
        console.log(farm.data)
        console.log("Farm name is :: ")
        console.log(farm.data.name)
        console.log(farm.name)
        try {
            const message = buildFarmNotificationMessage(farm);
            notificationList.unshift(message);
            notificationCount++;
            //|| farm.data.Name
            const farmData = {
                name: farm.data.name  || 'غير معروف',
                locationDesc: farm.data.locationDesc || farm.data.LocationDesc || '',
                extraFeatures: farm.data.extraFeatures || []
            };
            addToStorage(message, farmData, 'farm');
            updateNotificationUI();

            const farmName = farm.data.name || 'مزرعة';
            showToast(`🌾 تم إضافة مزرعة: ${farmName}`, 'success');
            console.log('✅ Farm notification added successfully');
        } catch (error) {
            console.error('❌ Error processing farm notification:', error);
        }
    });

    farmConnection.start()
        .then(() => {
            console.log('✅ Farm Hub connected successfully to:', FARM_API_URL);
        })
        .catch(err => {
            console.error('❌ Farm Hub connection error:', err);
        });

    // ==========================
    // SIGNALR - PRICE CONNECTION
    // ==========================
    console.log('📡 Connecting to Price Hub...');
    const priceConnection = new signalR.HubConnectionBuilder()
        .withUrl(PRICE_API_URL)
        .withAutomaticReconnect()
        .build();

    // مستمع تحديث الأسعار - المعدل
    priceConnection.on("PricesBatchUpdated", function (batchData) {
        console.log('📊✅ PricesBatchUpdated event received!', batchData);

        try {
            // بناء الرسالة مع زر التأكيد
            const message = buildPriceNotificationMessage(batchData);

            // إضافة للإشعارات
            notificationList.unshift(message);
            notificationCount++;

            // حفظ في localStorage
            addToStorage(message, batchData, 'price');

            // تحديث الواجهة
            updateNotificationUI();

            // عرض Toast مع ملخص
            const changedCount = batchData.priceDiffs ? batchData.priceDiffs.filter(d => d.hasChanges).length : 0;
            const totalCount = batchData.totalCount || 0;
            const farmerName = batchData.farmerName || 'مزارع';

            // عرض نافذة التأكيد تلقائياً (اختياري)
            // لو عايز تظهر تلقائياً من غير ما يدوس على الزر في الإشعار
            // فك الكومنت على السطر التالي:
            // const encodedData = encodeURIComponent(JSON.stringify(batchData));
            // setTimeout(() => showPriceConfirmation(encodedData), 1000);

            showToast(
                `📊 تم استلام تحديث أسعار ${farmerName}<br/>${totalCount} سعر ${changedCount > 0 ? `(${changedCount} تغيير)` : ''}<br/>اضغط على الإشعار للمراجعة`,
                changedCount > 0 ? 'info' : 'success'
            );

            console.log('✅ Price notification added successfully');
        } catch (error) {
            console.error('❌ Error processing price notification:', error);
        }
    });

    // مستمع إضافة سعر فردي
    priceConnection.on("PriceAdded", function (priceData) {
        console.log('💰 PriceAdded event received:', priceData);
    });

    // مستمع تحديث سعر فردي
    priceConnection.on("PriceUpdated", function (priceData) {
        console.log('💰 PriceUpdated event received:', priceData);
    });

    // بدء اتصال Price
    priceConnection.start()
        .then(() => {
            console.log('✅ Price Hub connected successfully to:', PRICE_API_URL);
        })
        .catch(err => {
            console.error('❌ Price Hub connection error:', err);
        });

    // ==========================
    // UI UPDATE
    // ==========================
    function updateNotificationUI() {
        console.log('🔄 Updating UI, count:', notificationCount);

        if (!countElement || !listElement) {
            console.error('❌ DOM elements not found!');
            return;
        }

        countElement.textContent = notificationCount;
        countElement.style.display = notificationCount > 0 ? "inline-block" : "none";

        listElement.innerHTML = "";

        if (notificationList.length === 0) {
            listElement.innerHTML = '<li class="noti-empty">لا توجد إشعارات</li>';
            return;
        }

        notificationList.forEach((n, index) => {
            const li = document.createElement("li");
            li.className = "noti-item";
            li.setAttribute('data-index', index);
            li.innerHTML = n;
            listElement.appendChild(li);
        });

        const clearLi = document.createElement("li");
        clearLi.className = "noti-clear";
        clearLi.innerHTML = `<button onclick="clearAllNotifications()" class="clear-btn">🗑️ مسح الكل</button>`;
        listElement.appendChild(clearLi);

        console.log('✅ UI updated successfully');
    }

    // ==========================
    // TOGGLE NOTIFICATIONS
    // ==========================
    window.toggleNotifications = function () {
        console.log('🔄 Toggling notifications');

        if (!dropdown) {
            console.error('❌ Dropdown element not found!');
            return;
        }

        dropdown.classList.toggle("active");

        if (dropdown.classList.contains("active")) {
            notificationCount = 0;
            if (countElement) {
                countElement.style.display = "none";
            }
            console.log('📬 Notifications opened, count reset');
        }
    };

    // ==========================
    // CLEAR ALL NOTIFICATIONS
    // ==========================
    window.clearAllNotifications = function () {
        console.log('🗑️ Clearing all notifications');

        if (notificationList.length === 0) {
            showToast('لا توجد إشعارات لمسحها', 'info');
            return;
        }

        Swal.fire({
            title: '🧹 مسح الإشعارات',
            text: 'هل أنت متأكد من مسح جميع الإشعارات؟',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'نعم، امسح الكل',
            cancelButtonText: 'إلغاء',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                notificationList = [];
                notificationCount = 0;
                clearStorage();
                updateNotificationUI();
                if (dropdown) {
                    dropdown.classList.remove('active');
                }

                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: '✅ تم مسح جميع الإشعارات',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true
                });
                console.log('✅ All notifications cleared');
            }
        });
    };

    // ==========================
    // CLOSE ON OUTSIDE CLICK
    // ==========================
    document.addEventListener("click", function (e) {
        if (!wrapper) return;

        if (!wrapper.contains(e.target)) {
            if (dropdown) {
                dropdown.classList.remove("active");
            }
        }
    });

    // ==========================
    // SWEET TOAST
    // ==========================
    function showToast(message, type = 'success') {
        console.log('🔔 Toast:', message);

        if (typeof Swal === 'undefined') {
            console.log('⚠️ Swal not defined, showing console notification:', message);
            return;
        }

        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: type,
            html: message,
            showConfirmButton: false,
            timer: 4000,
            timerProgressBar: true
        });
    }

    // ==========================
    // KEYBOARD SHORTCUTS
    // ==========================
    document.addEventListener("keydown", function (e) {
        if (e.key === 'Escape' && dropdown) {
            dropdown.classList.remove('active');
            console.log('🔑 Esc pressed, closing notifications');
        }
    });

    // ==========================
    // INIT - LOAD FROM STORAGE
    // ==========================
    loadFromStorage();

    setInterval(() => {
        const stored = getStoredNotifications();
        if (stored.length !== notificationList.length) {
            console.log('🔄 Refreshing notifications from storage');
            loadFromStorage();
        }
    }, 60 * 60 * 1000);

    console.log('✅ Notifications system loaded successfully');
    console.log(`📦 ${notificationList.length} notifications loaded from storage`);
    console.log('🎯 Listening for FarmAdded and PricesBatchUpdated events');

}); // end document ready