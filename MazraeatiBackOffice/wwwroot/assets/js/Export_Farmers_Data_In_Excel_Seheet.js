//$(document).ready(function () {
//    debugger
//    $('#exportButton').on('click', function () {
//        var search = $('#searchInput').val();
//        var cityBy = $('#citySelect').val();
//        var reservationDate = $('#reservationDateInput').val();
//        window.location.href = `FarmerExcel?${search}&&${cityBy}&&${reservationDate}`;
//    });
//});

//this code work success
//$(document).ready(function () {
//    $('#exportButton').on('click', function () {
//        debugger
//        var search = $('#searchInput').val();
//        var cityBy = $('#citySelect').val();
//        var reservationDate = $('#reservationDateInput').val();
//        var finalCityText = (cityBy === "0")
//            ? 'AllFarmsJordan'
//            : $('#citySelect option:selected').text().trim();

//        const url = `/Farmers/FarmerExcel?search=${encodeURIComponent(search)}&CityBy=${encodeURIComponent(cityBy)}&ReservationDate=${encodeURIComponent(reservationDate)}
//          &filename=${encodeURIComponent(finalCityText)}`;
//        window.location.href = url;
//    });
//});

let BaseUrl_Dev = 'http://localhost:62550/';
let BaseUrl_Pro = 'http://5.189.180.190/'
$(document).ready(function () {
    $('#exportButton').on('click', function () {
        debugger
        var search = $('#searchInput').val().trim();
        var cityBy = $('#citySelect').val();
        var reservationDate = $('#reservationDateInput').val();
        var selectedCityText = $('#citySelect option:selected').text().trim();

        let finalCityText = '';

        if (cityBy === "0" && search !== '') {
            finalCityText = 'مزارع' + '_' + search ;
        } else if (cityBy !== "0" && search === '') {
            finalCityText = 'مزارع' + '_' + selectedCityText ;
        } else if (cityBy !== "0" && search !== '') {
            finalCityText = selectedCityText;
        } else {
            finalCityText = "";
        }

        const url = `Farmers/FarmerExcel?search=${encodeURIComponent(search)}&CityBy=${encodeURIComponent(cityBy)}&ReservationDate=${encodeURIComponent(reservationDate)}&filename=${encodeURIComponent(finalCityText)}`;


        window.location.href = BaseUrl_Dev + url;
        
    });
});
