
function loadMapScript() {
    var a = $(".shop-resources").attr("data-googleapikey").trim(),
        b = "https://maps.googleapis.com/maps/api/js?libraries=places&callback=initializeMap";
    a && (b = b + "&key=" + a);
    var c = document.createElement("script");
    c.src = b, document.body.appendChild(c)
}

function getCustomMapStyles() {
    var a = $(".shop-resources").attr("data-mapstyles"),
        b = "";
    if (!a) return "";
    try {
        b = JSON.parse(a)
    } catch (a) {
        console.log("Invalid custom map styles value!")
    }
    return b
}

function initializeMap() {
    var a = document.getElementById(mapWrapperId),
        b = {
            center: new google.maps.LatLng(shopX, shopY),
            zoom: 15,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            styles: getCustomMapStyles()
        };
    map = new google.maps.Map(a, b), google.maps.event.addListenerOnce(map, "idle", function() {
        onMapLoad(), createMapSearchBox()
    })
}

function onMapLoad() {
    bounds = new google.maps.LatLngBounds;
    for (var a = 0; a < allShops.length; a++) null != allShops[a] && addMarker(allShops[a], !1);
    allShops.length > 1 && map.fitBounds(bounds), directionsService = new google.maps.DirectionsService, directionsDisplay = new google.maps.DirectionsRenderer, directionsDisplay.setPanel(document.getElementById("directions-panel")), $(".getUserGeoLocation").on("click", function(a) {
        a.preventDefault(), getUserGeoLocation(),
        $(".distance-line").css("display", "block"),
        $(".distance-line").css("margin", "20px")
    })
}

function addMarker(a, b) {
    var c = new google.maps.LatLng(a.lat, a.lng),
        d = new google.maps.Marker({
            position: c,
            map: map,
            icon:"../Content/wp-content/uploads/2016/12/02.png",
            title: a.title,
            draggable: b
        });
    bounds.extend(c), map.setCenter(c), d.setMap(map), markers[a.index] = d;
    var e = $('.shops-list > li[data-ind="' + a.index + '"] .short-description').html() || "",
        f = new google.maps.InfoWindow({
            maxWidth: 400,
            content: "<h5>" + a.title + "</h5><div>" + e + "</div>"
        });
    google.maps.event.addListener(d, "click", function() {
        f.open(map, d), $(".shops-list > li").removeClass("active"), $('.shops-list > li[data-ind="' + a.index + '"]').addClass("active")
    })
}

function setAllMap(a) {
    for (var b = 0; b < markers.length; b++) null != markers[b] && markers[b].setMap(a)
}

function clearMarkers() {
    setAllMap(null)
}

function showMarkers() {
    setAllMap(map)
}

function deleteMarkers() {
    clearMarkers(), markers = []
}

function createMapSearchBox() {
    var a = document.getElementById("location-address");
    //map.controls[google.maps.ControlPosition.].push(a);
    var b = new google.maps.places.SearchBox(a);
    a.style.display = "block", google.maps.event.addListener(b, "places_changed", function() {
        var a = b.getPlaces();
        0 != a.length && (createUserMarker(a[0].geometry.location), userMarker.setPosition(a[0].geometry.location), map.setCenter(a[0].geometry.location), searchResultCoords = a[0].geometry.location, showDistancesFromShopsToUser())
    })
}
function createUserMarker(a) {
    if (!isUserMarkerCreated) {
        var b = "../Content/wp-content/themes/docdirect/images/mimg.png",
            c = new google.maps.Marker({
                position: a,
                map: map,
                icon: b,
                animation: google.maps.Animation.DROP,
                title: $(".shop-resources").attr("data-youarehere"),
                draggable: !0
            });
        c.setMap(map), userMarker = c, bounds.extend(a), $(".distanceAndDirections").show(), $('#sortingSelect option[value="sortbydistance"]').removeAttr("disabled"), isUserMarkerCreated = !0, google.maps.event.addListener(c, "dragend", function() {
            showMarkers(), $(".shops-list > li").show(), showDistancesFromShopsToUser()
        })
    }
}

function getUserGeoLocation() {
    var a = "Geolocation is not supported by this browser.";
    return navigator.geolocation && navigator.geolocation.getCurrentPosition(function(b) {
        a = "Now we use your current position. ( Maybe a confirmation is required by the browser to use it. )";
        var c = new google.maps.LatLng(b.coords.latitude, b.coords.longitude);
        createUserMarker(c), userMarker.setPosition(c), map.setCenter(c), showDistancesFromShopsToUser()
    }, function(a) {
        $(".getUserGeoLocation").hide(), console.log(a.message);
        var b = new google.maps.LatLng(map.center.lat(), map.center.lng());
        createUserMarker(b), userMarker.setPosition(b), showDistancesFromShopsToUser()
    }), a
}

function showDirectionsToShop(a) {
    if (null != markers[a]) {
        var b = "Imperial" == $(".shop-resources").attr("data-units"),
            c = {
                origin: new google.maps.LatLng(userMarker.position.lat(), userMarker.position.lng()),
                destination: new google.maps.LatLng(markers[a].position.lat(), markers[a].position.lng()),
                travelMode: google.maps.TravelMode.DRIVING,
                unitSystem: b ? google.maps.UnitSystem.IMPERIAL : google.maps.UnitSystem.METRIC
            };
        directionsService.route(c, function(a, b) {
            b == google.maps.DirectionsStatus.OK && (directionsDisplay.setMap(map), directionsDisplay.setDirections(a), $("#directions-panel, .all-shops-page .map-wrapper, #backToResults").addClass("directions-shown"))
        })
    }
}

function showDistancesFromShopsToUser() {
    $(".shops-list > li").each(function() {
        var a = $(this),
            b = a.attr("data-ind");
        null != markers[b] && getDistanceToPosition(markers[b].position, a)
    }), $('#sortingSelect option[value="sortbydistance"]').attr("selected", "selected"), sortShopsByDistance()
}

function getDistanceToPosition(a, b) {

    var c = "Imperial" == $(".shop-resources").attr("data-units"),
        d = 6371e3,
        e = Math.toRadians(userMarker.position.lat()),
        f = Math.toRadians(userMarker.position.lng()),
        g = Math.toRadians(a.lat()),
        h = Math.toRadians(a.lng()),
        i = g - e,
        j = h - f,
        k = Math.sin(i / 2) * Math.sin(i / 2) + Math.cos(e) * Math.cos(g) * Math.sin(j / 2) * Math.sin(j / 2),
        l = 2 * Math.atan2(Math.sqrt(k), Math.sqrt(1 - k)),
        m = d * l / 1e3;
    c ? (b.find(".distance-value").html(Math.round(.621371192 * m) + " mi."), b.find(".distanceToShopValue").val(.621371192 * m)) : (b.find(".distance-value").html(Math.round(m) + " km."), b.find(".distanceToShopValue").val(m))
}

function sortShopsBySelectedMethod() {
    var a = $("#sortingSelect").val();
    "sortbydistance" == a ? sortShopsByDistance() : "sortbyname" == a ? sortShopsByName() : "sortbyid" == a && sortShopsById()
}

function sortShopsByName() {
    var a = $(".shops-list > li");
    a.sort(function(a, b) {
        var c = $(a).find(".tg-small").text().trim(),
            d = $(b).find(".tg-small").text().trim();
        return c.localeCompare(d)
    }), $(".shops-list").html(a)
}

function sortShopsByDistance() {
    if (isUserMarkerCreated) {
        var a = $(".shops-list > li");
        a.sort(function(a, b) {
            var c = parseFloat($(a).find(".distanceToShopValue").val()),
                d = parseFloat($(b).find(".distanceToShopValue").val());
            return c - d
        }), $(".shops-list").html(a)
    }
}

function sortShopsById() {
    var a = $(".shops-list > li");
    a.sort(function(a, b) {
        var c = parseInt($(a).attr("data-ind")),
            d = parseInt($(b).attr("data-ind"));
        return c - d
    }), $(".shops-list").html(a)
}

function searchForShopsMatchingTheSearchCriteria() {
    var a = isUserMarkerCreated,
        b = $("#searchByTagsInput").val(),
        c = b.length > 2,
        d = parseFloat($("#searchRadius").val());
    isNaN(d) && (a = !1), (a || c) && ($(".shops-list > li").each(function() {
        var e = $(this),
            f = parseInt(e.attr("data-ind")),
            g = !0,
            h = !0;
        if (a) {
            var i = parseFloat(e.find(".distanceToShopValue").val());
            i > d && (g = !1)
        }
        if (c) {
            var j = e.find(".shop-tags-hidden-field").val().toLowerCase();
            void 0 != j && j.indexOf(b.toLowerCase()) != -1 || (h = !1)
        }
        g && h ? (e.show(), null != markers[f] && markers[f].setMap(map)) : (e.hide(), null != markers[f] && markers[f].setMap(null))
    }), 0 == $(".shops-list > li:visible").length ? $(".no-shops-after-filtering").show() : $(".no-shops-after-filtering").hide(), sortShopsBySelectedMethod())
}
var mapWrapperId = "map_canvas",
    shopCoordinatesElementClass = ".shop-coordinates",
    allShops = [],
    directionsService, directionsDisplay, map, markers = [],
    userMarker, bounds, searchResultCoords, shopX = 0,
    shopY = 0,
    patternCoords = new RegExp(/^-?\d+\.?\d*$/),
    isUserMarkerCreated = !1;
$(document).ready(function() {
    $(shopCoordinatesElementClass).length > 0 && null != document.getElementById(mapWrapperId) && ($(shopCoordinatesElementClass).each(function() {
        var a = $(this).attr("data-latitude"),
            b = $(this).attr("data-longitude");
        if (null != a && null != b && patternCoords.test(a) && patternCoords.test(b)) {
            var c = $(this).closest(".shops-item").attr("data-ind"),
                d = null != c ? parseInt(c) : 0,
                e = {
                    index: d,
                    lat: a,
                    lng: b,
                    title: $(this).attr("data-shop-title")
                };
            allShops[d] = e
        }
    }), loadMapScript(), $("#backToResults").on("click", function () {

        directionsDisplay.setMap(null), $("#directions-panel, .map-wrapper, #backToResults").removeClass("directions-shown")
    }), $("#searchForFilteredShops").on("click", function(a) {
        a.preventDefault(), searchForShopsMatchingTheSearchCriteria(), $("#clearShopFilters").css("display", "inline-block")
    }), $(".shops-list").on("click", ".show-directions", function (a) {
        $("#directions-panel").css("display", "block");
        if (a.preventDefault(), isUserMarkerCreated) {
            var b = parseInt($(this).closest("li.shops-item").attr("data-ind"));
            b >= 0 && showDirectionsToShop(b), $("html, body").animate({
                scrollTop: $(".map-wrapper").offset().top - 100
            }, 500)
        }
    }), $(".shops-list").on("mouseenter", "li", function() {
        var a = markers[$(this).attr("data-ind")];
        null != a && a.setAnimation(google.maps.Animation.BOUNCE)
    }).on("mouseleave", "li", function() {
        var a = markers[$(this).attr("data-ind")];
        null != a && a.setAnimation(null)
    }), $("#clearShopFilters").on("click", function() {
        showMarkers(), $(".shops-list > li").show(), $("#searchByTagsInput").val(""), $("#searchRadius").val(""), $(this).hide(), 0 !== $(".shops-list > li:visible").length && $(".no-shops-after-filtering").hide()
    }), $("#sortingSelect").on("change", sortShopsBySelectedMethod), $(".align-map-button").on("click", function() {
        map.fitBounds(bounds)
    }))
}), Math.toRadians = function(a) {
    return a * Math.PI / 180
};