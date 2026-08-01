$(function () {
    var animationPlayedKey = 'eidAnimationPlayed'; // Key for localStorage
    var $runningLamb = $('#runningLamb');
    var eidTakbiratAudio = $('#eidTakbirat')[0]; // Get the native DOM audio element
    var marquee = $("#marqueeBox")
    // Define the audio playback duration: 1 minute and 50 seconds
    var audioPlayDuration = (1 * 60 + 50) * 1000; // Convert to milliseconds (110 seconds * 1000 = 110000 ms)

    // --- NEW: Date Check Logic ---
    var today = new Date();
    // Set the cutoff date to June 8, 2025.
    // Note: Month is 0-indexed, so 5 is June.
    var cutoffDate = new Date(2025, 5, 8); // Year, Month (0-11), Day

    // Check if today's date is on or after the cutoff date
    if (today.getTime() >= cutoffDate.getTime()) {
        console.log("Eid al-Adha animation and audio are now deactivated as of " + today.
            toLocaleDateString() + ".");
        //marquee.css('display', 'none')
         
        // Ensure image is not displayed and audio is stopped if it was somehow playing
        if ($runningLamb.length) {
            $runningLamb.css('display', 'none');
            $("#TextForEid").text('')
        }
        if (eidTakbiratAudio && !eidTakbiratAudio.paused) {
            eidTakbiratAudio.pause();
            eidTakbiratAudio.currentTime = 0;
        }
        // Exit the function, preventing any further execution of the animation/audio logic
        return;
    }
    // --- END NEW: Date Check Logic ---


        


    // The rest of your existing code now runs ONLY if the date check passes
    // (i.e., if today's date is BEFORE June 10, 2025)

    // Check if the animation has been played before using localStorage
    if (localStorage.getItem(animationPlayedKey) === null) {
        // If not played, set the flag in localStorage
        localStorage.setItem(animationPlayedKey, 'true');

        // --- Play Audio ---
        if (eidTakbiratAudio) {
            // Attempt to play audio, handling potential autoplay policy blocks
            eidTakbiratAudio.play().then(function () {
                console.log("Eid Takbirat audio started successfully.");

                // Set a timeout to pause the audio after 1 minute and 50 seconds
                setTimeout(function () {
                    if (eidTakbiratAudio && !eidTakbiratAudio.paused) {
                        eidTakbiratAudio.pause();
                        eidTakbiratAudio.currentTime = 0; // Optional: Reset audio to the beginning
                        console.log("Eid Takbirat audio paused after 1 minute 50 seconds.");
                    }
                }, audioPlayDuration);

            }).catch(function (error) {
                console.warn("Audio autoplay blocked or failed:", error);
                // You might inform the user or provide a button to play if blocked.
            });
        }

        // --- Animate the Lamb ---
        if ($runningLamb.length) { // Ensure the lamb element exists
            // Get the width of the viewport to determine animation range
            var viewportWidth = $(window).width();

            // Determine the vertical position of the lamb.
            // We'll try to position it just below the main header "لوحة التحكم".
            var topPosition = 0; // Default top position if header isn't found
  

            // Set initial position and display the lamb
            $runningLamb.css({
                'left': '-200px', // Start off-screen to the left (adjust -200px if your lamb is wider)
                'top': topPosition + 'px', // Set vertical position
                'height': '120px', // Set a reasonable height for the lamb (adjust as needed)
                'display': 'block' // Make the lamb visible
            });

            // Animate the lamb across the screen
            // The animation duration (5 seconds) is independent of the audio duration
            $runningLamb.animate({
                left: viewportWidth + 'px' // Move off-screen to the right
            }, 30000, 'linear', function () { // Changed back to 5000ms (5 seconds) as per initial request for animation duration
                // Animation complete callback: hide the lamb
                $(this).hide();
            });
        }
    }



    //console.log('jQuery شغال'); // تتأكد إن jQuery شغال

    //$('#adminName').on('click', function () {
    //    console.log('تم الضغط على adminName'); // تتأكد إن الحدث شغال

    //    Swal.fire({
    //        title: 'تسجيل الخروج',
    //        text: 'هل أنت متأكد من تسجيل الخروج؟',
    //        icon: 'question',
    //        showCancelButton: true,
    //        confirmButtonText: 'نعم، اخرج',
    //        cancelButtonText: 'لا',
    //        confirmButtonColor: '#d33'
    //    }).then(function (result) {
    //        if (result.isConfirmed) {
    //            Swal.fire({
    //                title: 'جاري تسجيل الخروج...',
    //                text: 'من فضلك انتظر',
    //                allowOutsideClick: false,
    //                showConfirmButton: false,
    //                didOpen: function () {
    //                    Swal.showLoading();
    //                }
    //            });

    //            setTimeout(function () {
    //                window.location.href = '@Url.Content("~/account/SignOut")';
    //            }, 500);
    //        }
    //    });
    //});

});




// ==========================
// Start Delete Functions
// ==========================



// function Delete(controller, id, action, redirectAction) {
//     if (action == undefined)
//         action = "Index";


//     if (confirm("هل انت متأكد من انك تريد الغاء هذا السجل")) {
//         $.ajax({
//             url: '../' + controller + '/' + action,
//             data: { id: id },
//             success: function (s) {
//                 if (s == 1) {
//                     window.location.href = '../' + controller + '/' + redirectAction + '';
//                     return;
//                 }
//                 alert(s);
//             },
//             error: function (e) { }
//         });
//     }

// }


//Old Delete before permission ....


// function Delete(controller, id, action, redirectAction) {

//     if (action == undefined)
//         action = "Index";

//     Swal.fire({
//         title: 'هل أنت متأكد؟',
//         text: "لن تستطيع استرجاع هذا السجل!",
//         icon: 'warning',
//         showCancelButton: true,
//         confirmButtonText: 'نعم، احذف',
//         cancelButtonText: 'إلغاء',
//         confirmButtonColor: '#d33',
//         cancelButtonColor: '#3085d6'
//     }).then((result) => {

//         if (result.isConfirmed) {

//             $.ajax({
//                 url: '../' + controller + '/' + action,
//                 data: { id: id },
//                 success: function (s) {

//                     if (s == 1) {

//                         Swal.fire({
//                             title: 'تم الحذف!',
//                             text: 'تم حذف السجل بنجاح',
//                             icon: 'success',
//                             timer: 1500,
//                             showConfirmButton: false
//                         }).then(() => {
//                             window.location.href = '../' + controller + '/' + redirectAction;
//                         });

//                     } else {

//                         Swal.fire({
//                             title: 'خطأ!',
//                             text: s,
//                             icon: 'error'
//                         });
//                     }
//                 },
//                 error: function () {
//                     Swal.fire({
//                         title: 'خطأ!',
//                         text: 'حدث خطأ أثناء الحذف',
//                         icon: 'error'
//                     });
//                 }
//             });

//         }

//     });
// }

//1 - New Delete  after permission .....

function Delete(controller, id, action, redirectAction) {
    debugger
    if (action == undefined)
        action = "Index";

    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: "لن تستطيع استرجاع هذا السجل!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'نعم، احذف',
        cancelButtonText: 'إلغاء',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        debugger
        if (result.isConfirmed) {

            // ===== إضافة Loading =====
            Swal.fire({
                title: 'جاري الحذف...',
                html: 'يرجى الانتظار',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
            debugger
            $.ajax({
                
                url: '../' + controller + '/' + action,
                data: { id: id },
                // ===== إضافة الـ Header عشان الـ Filter يعرف إنه طلب AJAX =====
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                success: function (s) {
                    Swal.close();

                    // ===== لو كان الرد JSON (من الـ PermissionFilter) =====
                    if (typeof s === 'object' && s !== null) {
                        if (s.success === false) {
                            // ===== لو كان خطأ صلاحية =====
                            if (s.errorType === 'permission_denied') {
                                Swal.fire({
                                    title: '⛔ غير مصرح بالدخول',
                                    text: s.message || 'ليس لديك صلاحية للقيام بهذا الإجراء',
                                    icon: 'error',
                                    confirmButtonText: 'حسناً',
                                    confirmButtonColor: '#dc3545'
                                });
                                return;
                            } else {
                                Swal.fire({
                                    title: 'خطأ!',
                                    text: s.message || 'حدث خطأ',
                                    icon: 'error',
                                    confirmButtonText: 'حسناً'
                                });
                                return;
                            }
                        }
                        // لو كان success = true، نكمل
                        if (s.success === true) {
                            Swal.fire({
                                title: 'تم الحذف!',
                                text: s.message || 'تم حذف السجل بنجاح',
                                icon: 'success',
                                timer: 1500,
                                showConfirmButton: false
                            }).then(() => {
                                window.location.href = '../' + controller + '/' + redirectAction;
                            });
                            return;
                        }
                    }

                    // ===== الكود الأصلي (لو السيرفر رجع 1 أو نص) =====
                    if (s == 1) {
                        Swal.fire({
                            title: 'تم الحذف!',
                            text: 'تم حذف السجل بنجاح',
                            icon: 'success',
                            timer: 1500,
                            showConfirmButton: false
                        }).then(() => {
                            window.location.href = '../' + controller + '/' + redirectAction;
                        });
                    } else {
                        Swal.fire({
                            title: 'خطأ!',
                            text: s,
                            icon: 'error',
                            confirmButtonText: 'حسناً'
                        });
                    }
                },
                error: function (xhr) {
                    Swal.close();

                    // ===== لو كان 403 Forbidden (من الـ Filter) =====
                    if (xhr.status === 403) {
                        Swal.fire({
                            title: '⛔ غير مصرح بالدخول',
                            text: 'ليس لديك صلاحية للقيام بهذا الإجراء',
                            icon: 'error',
                            confirmButtonText: 'حسناً',
                            confirmButtonColor: '#dc3545'
                        });
                        return;
                    }

                    // ===== الكود الأصلي للخطأ =====
                    Swal.fire({
                        title: 'خطأ!',
                        text: 'حدث خطأ أثناء الحذف',
                        icon: 'error',
                        confirmButtonText: 'حسناً'
                    });
                }
            });

        }

    });
}

//2 - Old Delete before permission .....
// function DeleteWithAction(controller, action , id , redirectAction) {
//     if (action == undefined)
//         action = "Index";

//     if (confirm("هل انت متأكد من انك تريد الغاء هذا السجل")) {
//         $.ajax({
//             url: '../' + controller + '/' + action,
//             data: { id: id },
//             success: function (s) {
//                 if (s == 1) {
//                     window.location.href = '../' + controller + '/' + redirectAction + '';
//                     return;
//                 }
//                 alert(s);
//             },
//             error: function (e) { }
//         });
//     }

// }



//New Delete after permission .....

function DeleteWithAction(controller, action, id, redirectAction) {
    if (action == undefined)
        action = "Index";

    if (confirm("هل انت متأكد من انك تريد الغاء هذا السجل")) {

        // ===== إضافة Loading =====
        Swal.fire({
            title: 'جاري الحذف...',
            html: 'يرجى الانتظار',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            url: '../' + controller + '/' + action,
            data: { id: id },
            // ===== إضافة الـ Header عشان الـ Filter يعرف إنه طلب AJAX =====
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (s) {
                Swal.close();

                // ===== لو كان الرد JSON (من الـ PermissionFilter) =====
                if (typeof s === 'object' && s !== null) {
                    if (s.success === false) {
                        // ===== لو كان خطأ صلاحية =====
                        if (s.errorType === 'permission_denied') {
                            Swal.fire({
                                title: '⛔ غير مصرح بالدخول',
                                text: s.message || 'ليس لديك صلاحية للقيام بهذا الإجراء',
                                icon: 'error',
                                confirmButtonText: 'حسناً',
                                confirmButtonColor: '#dc3545'
                            });
                            return;
                        } else {
                            Swal.fire({
                                title: 'خطأ!',
                                text: s.message || 'حدث خطأ',
                                icon: 'error',
                                confirmButtonText: 'حسناً'
                            });
                            return;
                        }
                    }
                    // لو كان success = true، نكمل
                    if (s.success === true) {
                        Swal.fire({
                            title: 'تم الحذف!',
                            text: s.message || 'تم حذف السجل بنجاح',
                            icon: 'success',
                            timer: 1500,
                            showConfirmButton: false
                        }).then(() => {
                            window.location.href = '../' + controller + '/' + redirectAction;
                        });
                        return;
                    }
                }

                // ===== الكود الأصلي =====
                if (s == 1) {
                    window.location.href = '../' + controller + '/' + redirectAction + '';
                    return;
                }

                // ===== لو كان الرد نص خطأ =====
                if (typeof s === 'string') {
                    Swal.fire({
                        title: 'خطأ!',
                        text: s,
                        icon: 'error',
                        confirmButtonText: 'حسناً'
                    });
                } else {
                    alert(s);
                }
            },
            error: function (xhr) {
                Swal.close();

                // ===== لو كان 403 Forbidden (من الـ Filter) =====
                if (xhr.status === 403) {
                    Swal.fire({
                        title: '⛔ غير مصرح بالدخول',
                        text: 'ليس لديك صلاحية للقيام بهذا الإجراء',
                        icon: 'error',
                        confirmButtonText: 'حسناً',
                        confirmButtonColor: '#dc3545'
                    });
                    return;
                }

                // ===== الكود الأصلي =====
                // (فاضي، بس لو عايز تضيف معالجة للخطأ)
                if (xhr.responseText) {
                    Swal.fire({
                        title: 'خطأ!',
                        text: xhr.responseText || 'حدث خطأ أثناء الحذف',
                        icon: 'error',
                        confirmButtonText: 'حسناً'
                    });
                }
            }
        });
    }
}

//3 - Old Delete before permission .....
//for Delete Reservation ::::::
// function DeleteFarmerReservationWithParampter(controller, id, action, redirectAction, parampterId) {

//     if (action == undefined)
//         action = "Index";

//     Swal.fire({
//         title: 'هل أنت متأكد؟',
//         text: "لن تستطيع التراجع بعد الحذف!",
//         icon: 'warning',
//         showCancelButton: true,
//         confirmButtonColor: '#3085d6',
//         cancelButtonColor: '#d33',
//         confirmButtonText: 'نعم، احذف!',
//         cancelButtonText: 'إلغاء'
//     }).then((result) => {

//         if (result.isConfirmed) {

//             $.ajax({
//                 url: '../' + controller + '/' + action,
//                 data: { id: id },
//                 success: function (s) {

//                     if (s == 1) {

//                         Swal.fire({
//                             icon: 'success',
//                             title: 'تم الحذف بنجاح',
//                             showConfirmButton: false,
//                             timer: 1500
//                         }).then(() => {

//                             if (parampterId == 0) {
//                                 window.location.href = '../' + controller + '/' + redirectAction;
//                             } else {
//                                 window.location.href = '../' + controller + '/' + redirectAction + '?farmerId=' + parampterId;
//                             }

//                         });

//                     } else {
//                         Swal.fire({
//                             icon: 'error',
//                             title: 'خطأ',
//                             text: s
//                         });
//                     }
//                 },
//                 error: function () {
//                     Swal.fire({
//                         icon: 'error',
//                         title: 'خطأ',
//                         text: 'حدث خطأ أثناء الحذف'
//                     });
//                 }
//             });

//         }

//     });
// }


//3 - New Delete after permission .....

function DeleteFarmerReservationWithParampter(controller, id, action, redirectAction, parampterId) {

    if (action == undefined)
        action = "Index";

    Swal.fire({
        title: 'هل أنت متأكد؟',
        text: "لن تستطيع التراجع بعد الحذف!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، احذف!',
        cancelButtonText: 'إلغاء'
    }).then((result) => {

        if (result.isConfirmed) {

            // ===== إضافة Loading =====
            Swal.fire({
                title: 'جاري الحذف...',
                html: 'يرجى الانتظار',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            $.ajax({
                url: '../' + controller + '/' + action,
                data: { id: id },
                // ===== إضافة الـ Header عشان الـ Filter يعرف إنه طلب AJAX =====
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                success: function (s) {
                    Swal.close();

                    // ===== لو كان الرد JSON (من الـ PermissionFilter) =====
                    if (typeof s === 'object' && s !== null) {
                        if (s.success === false) {
                            // ===== لو كان خطأ صلاحية =====
                            if (s.errorType === 'permission_denied') {
                                Swal.fire({
                                    title: '⛔ غير مصرح بالدخول',
                                    text: s.message || 'ليس لديك صلاحية للقيام بهذا الإجراء',
                                    icon: 'error',
                                    confirmButtonText: 'حسناً',
                                    confirmButtonColor: '#dc3545'
                                });
                                return;
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'خطأ',
                                    text: s.message || 'حدث خطأ',
                                    confirmButtonText: 'حسناً'
                                });
                                return;
                            }
                        }
                        // لو كان success = true، نكمل
                        if (s.success === true) {
                            Swal.fire({
                                icon: 'success',
                                title: s.message || 'تم الحذف بنجاح',
                                showConfirmButton: false,
                                timer: 1500
                            }).then(() => {
                                if (parampterId == 0) {
                                    window.location.href = '../' + controller + '/' + redirectAction;
                                } else {
                                    window.location.href = '../' + controller + '/' + redirectAction + '?farmerId=' + parampterId;
                                }
                            });
                            return;
                        }
                    }

                    // ===== الكود الأصلي =====
                    if (s == 1) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            if (parampterId == 0) {
                                window.location.href = '../' + controller + '/' + redirectAction;
                            } else {
                                window.location.href = '../' + controller + '/' + redirectAction + '?farmerId=' + parampterId;
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'خطأ',
                            text: s,
                            confirmButtonText: 'حسناً'
                        });
                    }
                },
                error: function (xhr) {
                    Swal.close();

                    // ===== لو كان 403 Forbidden (من الـ Filter) =====
                    if (xhr.status === 403) {
                        Swal.fire({
                            title: '⛔ غير مصرح بالدخول',
                            text: 'ليس لديك صلاحية للقيام بهذا الإجراء',
                            icon: 'error',
                            confirmButtonText: 'حسناً',
                            confirmButtonColor: '#dc3545'
                        });
                        return;
                    }

                    // ===== الكود الأصلي =====
                    Swal.fire({
                        icon: 'error',
                        title: 'خطأ',
                        text: 'حدث خطأ أثناء الحذف',
                        confirmButtonText: 'حسناً'
                    });
                }
            });

        }

    });
}

function Approve(id) {
    if (confirm("هل انت متأكد من انك تريد انهاء هذا الطلب")) {
        $.ajax({
            url: '../Home/ApproveRequest',
            data: { id: id },
            success: function (s) {
                if (s == 1) {
                    window.location.reload();
                    return;
                }
                alert(s);
            },
            error: function (e) { }
        });
    }

}



// ==========================
// End Delete Functions
// ==========================







