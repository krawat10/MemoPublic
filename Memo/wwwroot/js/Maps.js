var getUserMarkersUrl = window.location.origin + '/api/Markers/';
var getSharedMarkersUrl = window.location.origin + '/api/Markers/Shared';
var getMarkerElementUrl = window.location.origin + '/Markers/Details/';
var markers = [];
var selectedNewMarker;
var map;
var info;


function initMap() {
    return new google.maps.Map(document.getElementById('map'),
        { zoom: 4, center: { lat: -25.344, lng: 131.036 } });
}

function setMarkersOnMap(data, map) {
    var markers = [];

    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        var latLng = {
            lat: item.coordinates.latitude,
            lng: item.coordinates.longitude
        };

        var marker = new google.maps.Marker({
            position: latLng,
            map: map,
            title: item.name,
            description: item.description,
            created: item.created,
            userName: item.userName,
            id: item.id,
            isCenterPoint: item.isCenterPoint,
            animation: google.maps.Animation.DROP
        });

        markers.push(marker);
    }

    var centerMarker = markers.find(marker => marker.isCenterPoint);

    if (!centerMarker) {
        if (markers.length > 0) {
            centerMarker = markers[0];
        }
    }

    if (centerMarker) {
        map.setCenter(centerMarker.position);
    }
    

    return markers;
}

function getUserMarkers() {
    return $.ajax({
        type: 'GET',
        url: getUserMarkersUrl,
        contentType: 'application/json; charset=utf-8',
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function getSharedMarkers() {
    return $.ajax({
        type: 'GET',
        url: getSharedMarkersUrl,
        contentType: 'application/json; charset=utf-8'
    });
}

function initUserMap() {
    map = initMap();

    getUserMarkers().done((data) => {
        markers = setMarkersOnMap(data, map);

        markers.forEach(marker => {
            marker.addListener('click', (event) => toggleBounce(event, markers));
            marker.addListener('click', (event) => bindDetailWindow(marker));
        });

    });

    map.addListener('click',
        function (event) {
            if (selectedNewMarker) {
                selectedNewMarker.setMap(null);
            }

            document.getElementById('x-input').value = event.latLng.lat();
            document.getElementById('y-input').value = event.latLng.lng();
            map.setCenter(event.latLng);
            selectedNewMarker = new google.maps.Marker({
                position: event.latLng,
                map: map,
                animation: google.maps.Animation.DROP
            });
        });
}

function initSharedMap() {
    map = initMap();
    info = new google.maps.InfoWindow();

    getSharedMarkers().done((data) => {
        markers = setMarkersOnMap(data, map);

        markers.forEach(marker => {
            marker.addListener('click', (event) => toggleBounce(event, markers));
            marker.addListener('click', (event) => bindMapWindow(marker, map, info));
        });
    });

    map.addListener('click',
        function (event) {
            if (selectedNewMarker) {
                selectedNewMarker.setMap(null);
            }

            map.setCenter(event.latLng);
            selectedNewMarker = new google.maps.Marker({
                position: event.latLng,
                map: map,
                animation: google.maps.Animation.DROP
            });
        });
}

function getMarkerRow(marker) {
    var id = "marker_" +
        marker.position.lng().toFixed(5) +
        "_" +
        marker.position.lat().toFixed(5);

    return document.getElementById(id);
}

function find() {
    var distance = parseInt(document.getElementById('distance-input').value);

    // Iterate through all markers
    for (var i = 0; i < markers.length; i++) {
        if (markers[i] === selectedUserMarker) continue; // If iterated marker is selected by user -> skip

        var srcLocation = selectedUserMarker.position; // location of selected marker
        var dstLocation = markers[i].position; // location of iterated marker
        var actualDistance = google // distance between these markers
            .maps
            .geometry
            .spherical
            .computeDistanceBetween(srcLocation, dstLocation) /
            1000;

        // Iterated marker is in search area
        if (actualDistance <= distance) {
            markers[i].setAnimation(google.maps.Animation.BOUNCE); // Bounce this marker
            getMarkerRow(markers[i]).classList.add("success"); // Set succes class to list item ("green background")
        } else {
            markers[i].setAnimation(null);
            getMarkerRow(markers[i]).classList.remove("success");
        }
    }

    console.log(selectedUserMarker);
}

function toggleBounce(event, markers) {


    for (var i = 0; i < markers.length; i++) {
        if (markers[i].position === event.latLng) {
            selectedUserMarker = markers[i];
            selectedUserMarker.setAnimation(google.maps.Animation.BOUNCE);

            if (getMarkerRow(selectedUserMarker)) {
                getMarkerRow(selectedUserMarker).classList.add("success");
            }

        } else {
            markers[i].setAnimation(null);

            if (getMarkerRow(markers[i])) {
                getMarkerRow(markers[i]).classList.remove("success");
            }
        }
    }
}

function bindMapWindow(marker, map, window) {
    window.close();

    window.setContent(
        '<h5>' +
        marker.title +
        '</h5>' +
        '<p>' +
        marker.description +
        '</p>' +
        '<p>' +
        marker.created +
        '</p>' +
        marker.userName +
        '</p>');

    window.open(map, marker);
}


function bindDetailWindow(marker) {
    $.ajax({
        type: 'GET',
        url: getMarkerElementUrl + marker.id,
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    }).done((data) => {
        document.getElementById('marker-details').innerHTML = data;
    });
}




