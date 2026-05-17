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
            right: 'dayGridMonth,listWeek'
        },
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

                        events.push({

                            id: item.id,

                            title: item.title,

                            start: item.start,

                            allDay: true,

                            classNames: [item.className],

                            extendedProps: {

                                phone: item.phone,

                                amount: item.amount,

                                persons: item.persons,

                                note: item.note,

                                deposit: item.deposit,

                                remain: item.remain
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

            Swal.fire({
                title: event.title,
                html:
                    `
                    <div style="text-align:right">
                        <p><b>رقم الهاتف:</b> ${event.extendedProps.phone ?? ''}</p>
                        <p><b>المبلغ:</b> ${event.extendedProps.amount ?? 0}</p>
                        <p><b>عدد الأشخاص:</b> ${event.extendedProps.persons ?? 0}</p>
                        <p><b>المدفوع:</b> ${event.extendedProps.deposit ?? 0}</p>
                        <p><b>المتبقي:</b> ${event.extendedProps.remain ?? 0}</p>
                        <p><b>الملاحظات:</b> ${event.extendedProps.note ?? ''}</p>
                    </div>
                    `,
                icon: 'info',
                width: '400px',
                height:"450px",
                padding: '1rem',
                confirmButtonText: 'موافق'
            });
        }
    });

    calendar.render();

});