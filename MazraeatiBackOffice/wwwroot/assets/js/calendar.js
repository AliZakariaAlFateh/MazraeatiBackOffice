let BaseUrl_Dev = 'http://localhost:62550'
let BaseUrl_Pro = 'http://5.189.180.190/MazraeatiBackOffice'
document.addEventListener("DOMContentLoaded", function () {

    var calendarEl = document.getElementById("calendar");

    // ===============================
    // Get Farmer Id
    // ===============================
    function getFarmerId() {

        const params = new URLSearchParams(window.location.search);

        return params.get("farmerid");
    }

    // ===============================
    // Calendar
    // ===============================
    var calendar = new FullCalendar.Calendar(calendarEl, {

        // تحويل التقويم بالكامل للغة العربية (الأيام، الشهور، والأزرار)
        locale: 'ar',

        // ضبط اتجاه التقويم من اليمين لليسار ليناسب اللغة العربية
        direction: 'rtl',
        /*initialView: 'dayGridMonth',*/
        initialView: window.innerWidth < 768
            ? 'listWeek'
            : 'dayGridMonth',

        themeSystem: 'bootstrap',

        selectable: true,

        editable: false,

        navLinks: true,

        height: "auto",

        //headerToolbar: {
        //    left: 'prev,next today',
        //    center: 'title',
        //    right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
        //},
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        views: {
            dayGridMonth: {
                buttonText: 'شهر' // تقدر تغير النصوص هنا للعربي لو تحب
            },
            timeGridWeek: {
                buttonText: 'أسبوع'
            },
            timeGridDay: {
                buttonText: 'يوم'
            },
            listWeek: {
                buttonText: 'قائمة'
            }
        },
        handleWindowResize: true,
        windowResizeDelay: 100,
        // ===============================
        // Load Events
        // ===============================
        events: function (fetchInfo, successCallback, failureCallback) {

            const farmerId = getFarmerId();

            console.log("FarmerId:", farmerId);

            $.ajax({

                url: `${BaseUrl_Dev}/FarmerReservation/FillCalendarReservation`,

                type: 'GET',

                data: {
                    farmerid: farmerId
                },

                success: function (response) {

                    console.log("Response:", response);

                    let events = [];

                    $.each(response, function (index, item) {

                        // ============================
                        // تحديد اللون حسب نوع الحجز
                        // ============================

                        let eventColor = 'bg-success';

                        switch (item.reservationType) {

                            case 40:
                                eventColor = 'bg-success'; // يوم كامل
                                break;

                            case 41:
                                eventColor = 'bg-primary'; // صباحي
                                break;

                            case 42:
                                eventColor = 'bg-warning'; // مسائي
                                break;

                            default:
                                eventColor = 'bg-secondary';
                                break;
                        }

                        // ============================
                        // Push Event
                        // ============================

                        events.push({

                            id: item.id,

                            title: item.title,

                            start: item.start,

                            allDay: true,

                            classNames: [eventColor],

                            extendedProps: {

                                phone: item.phone,

                                amount: item.amount,

                                persons: item.persons,

                                note: item.note,

                                deposit: item.deposit,

                                remain: item.remain,

                                reservationType: item.reservationType
                            }
                        });
                    });

                    console.log("Events:", events);

                    successCallback(events);
                },





                error: function (xhr) {

                    console.log(xhr);

                    failureCallback(xhr);

                    Swal.fire({
                        icon: 'error',
                        title: 'خطأ',
                        text: 'فشل تحميل الحجوزات'
                    });
                }
            });
        },

        // ===============================
        // Event Click
        // ===============================
        eventClick: function (info) {

            const event = info.event;

            let reservationTypeText = '';

            switch (event.extendedProps.reservationType) {

                case 40:
                    reservationTypeText = 'يوم كامل';
                    break;

                case 41:
                    reservationTypeText = 'صباحي';
                    break;

                case 42:
                    reservationTypeText = 'مسائي';
                    break;

                default:
                    reservationTypeText = 'غير معروف';
                    break;
            }

            Swal.fire({

                title: event.title,

                html:
                    `
            <div style="text-align:right;font-size:14px">

                <p><b>نوع الحجز:</b> ${reservationTypeText}</p>

                <p><b>رقم الهاتف:</b> ${event.extendedProps.phone ?? ''}</p>

                <p><b>المبلغ:</b> ${event.extendedProps.amount ?? 0}</p>

                <p><b>عدد الأشخاص:</b> ${event.extendedProps.persons ?? 0}</p>

                <p><b>المدفوع:</b> ${event.extendedProps.deposit ?? 0}</p>

                <p><b>المتبقي:</b> ${event.extendedProps.remain ?? 0}</p>

                <p><b>الملاحظات:</b> ${event.extendedProps.note ?? ''}</p>

            </div>
            `,

                icon: 'info',

                width: '350px',

                padding: '0.8rem',

                confirmButtonText: 'موافق'
            });
        }






    });

    calendar.render();

});