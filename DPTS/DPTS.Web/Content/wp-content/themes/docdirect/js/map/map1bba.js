/* global document */

jQuery(document).ready(function () {
    "use strict";
	var dir_latitude	 = scripts_vars.dir_latitude;
	var dir_longitude	= scripts_vars.dir_longitude;
	var dir_map_type	 = scripts_vars.dir_map_type;
	var dir_close_marker		  = scripts_vars.dir_close_marker;
	var dir_cluster_marker		= scripts_vars.dir_cluster_marker;
	var dir_map_marker			= scripts_vars.dir_map_marker;
	var dir_cluster_color		 = scripts_vars.dir_cluster_color;
	var dir_zoom				  = scripts_vars.dir_zoom;
	var dir_map_scroll				  = scripts_vars.dir_map_scroll;
	var user_status	= scripts_vars.user_status;
	var fav_message	= scripts_vars.fav_message;
	var fav_nothing	= scripts_vars.fav_nothing;
	
	
	//Click Function for Map Banner
    jQuery(".search_banner").on('click', function (event) {
        event.stopPropagation();
		var _this	= jQuery(this);
		_this.append("<i class='fa fa-refresh fa-spin'></i>");
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: jQuery('.directory-map').serialize() + '&action=docdirect_get_map_directory',
			dataType: "json",
			success: function (response) {
				//Call map init
				docdirect_init_map_script( response );
				_this.find('i').remove();
				jQuery('.tg-banner-content').fadeOut(2000);
      			jQuery('.show-search').fadeIn(2000);
			}
	   }); 
    });
	
	//Search View 1 form submission
	jQuery(".form-searchdoctors").submit(function (event) {
	  event.preventDefault();
	  jQuery('.search_banner').trigger('click');//triger search
	});
	
    jQuery(".show-search").on('click', function (event) {
        event.preventDefault();
        jQuery('.tg-banner-content').fadeIn(1000);
        jQuery(this).fadeOut(500);
		jQuery(".infoBox").hide();
    });
	
	//Swap Titles
	jQuery(document).on('click','.current-directory',function(){	
		jQuery(this).parents('ul').find('li').removeClass('active');
		jQuery(this).addClass('active');
		var dir_name	= jQuery(this).data('dir_name');
		var id	= jQuery(this).data('id');
		jQuery(this).parents('.tg-banner-content').find('em.current_directory').html(dir_name);
		
		if( Z_Editor.elements[id] ) {
			var load_subcategories = wp.template( 'load-subcategories' );
			var data = [];
			data['childrens']	 = Z_Editor.elements[id];
			data['parent']	    = dir_name;
			var _options	= load_subcategories(data);
			jQuery( '.subcats' ).html(_options);
		}

	});
	
	//Prepare Subcatgories
	jQuery(document).on('change','.directory_type', function (event) {
		var id		  = jQuery('option:selected', this).attr('id');		
		var dir_name	= jQuery(this).find(':selected').data('dir_name');
		
		if( jQuery( '.dynamic-title' ).length ){
			jQuery( '.dynamic-title' ).html(dir_name);
		}
		
		if( Z_Editor.elements[id] ) {
			var load_subcategories = wp.template( 'load-subcategories' );
			var data = [];
			data['childrens']	 = Z_Editor.elements[id];
			data['parent']	    = dir_name;
			var _options	= load_subcategories(data);
			jQuery( '.subcats' ).html(_options);
		}
	});
	
	//Prepare Subcatgories
	jQuery(document).on('change','.sort_by, .order_by', function (event) {
		jQuery(".form-refinesearch").submit();
	});
	
	//Change View
	jQuery(document).on('click','.tg-listing-views a.list, .tg-listing-views a.grid', function (event) {
		var current_class	= jQuery(this).attr('class');
		jQuery(this).parents('ul.tg-listing-views').find('li').removeClass('active');
		jQuery(this).parent('li').addClass('active');
		
		jQuery(this).parents('.tg-doctors-list').find('.tg-view').addClass('tg-list-view').removeClass('tg-grid-view');
		if( current_class == 'grid' ){
			jQuery(this).parents('.tg-doctors-list').find('.tg-view').addClass('tg-grid-view').removeClass('tg-list-view');
		}
	});
		
});

//Init Map Scripts
function docdirect_init_map_script( _data_list ){
	var dir_latitude	 = scripts_vars.dir_latitude;
	var dir_longitude	= scripts_vars.dir_longitude;
	var dir_map_type	 = scripts_vars.dir_map_type;
	var dir_close_marker		  = scripts_vars.dir_close_marker;
	var dir_cluster_marker		= scripts_vars.dir_cluster_marker;
	var dir_map_marker			= scripts_vars.dir_map_marker;
	var dir_cluster_color		 = scripts_vars.dir_cluster_color;
	var dir_zoom				  = scripts_vars.dir_zoom;
	var dir_map_scroll			= scripts_vars.dir_map_scroll;
	var gmap_norecod			  = scripts_vars.gmap_norecod;
	var map_styles			    = scripts_vars.map_styles;


	if( _data_list.status == 'found' ){
		var response_data	= _data_list.users_list;
	    if( typeof(response_data) != "undefined" && response_data !== null ) {
			var location_center = new google.maps.LatLng(response_data[0].latitude,response_data[0].longitude);
		} else {
				var location_center = new google.maps.LatLng(dir_latitude,dir_longitude);
		}
	} else{
		var location_center = new google.maps.LatLng(dir_latitude,dir_longitude);
	}
	
	if(dir_map_type == 'ROADMAP'){
		var map_id = google.maps.MapTypeId.ROADMAP;
	} else if(dir_map_type == 'SATELLITE'){
		var map_id = google.maps.MapTypeId.SATELLITE;
	} else if(dir_map_type == 'HYBRID'){
		var map_id = google.maps.MapTypeId.HYBRID;
	} else if(dir_map_type == 'TERRAIN'){
		var map_id = google.maps.MapTypeId.TERRAIN;
	} else {
		var map_id = google.maps.MapTypeId.ROADMAP;
	}
	
	var scrollwheel	   = true;
	var lock		   = 'lock';
	
	if( dir_map_scroll == 'false' ){
		scrollwheel	= false;
		lock		   = 'unlock';
		
	}
	
	var mapOptions = {
		zoom: parseInt(dir_zoom),
		maxZoom: 20,
		mapTypeId: map_id,
		scaleControl: true,
		scrollwheel: false,
		disableDefaultUI: true,
		draggable:scrollwheel,
	}
	
	var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
	
	var styles = docdirect_get_map_styles(map_styles);
	if(styles != ''){
		var styledMap = new google.maps.StyledMapType(styles, {name: 'Styled Map'});
		map.mapTypes.set('map_style', styledMap);
		map.setMapTypeId('map_style');
	}
		
	var bounds = new google.maps.LatLngBounds();
	
	//Zoom In
	if(  document.getElementById('doc-mapplus') ){ 
		 google.maps.event.addDomListener(document.getElementById('doc-mapplus'), 'click', function () {      
		   var current= parseInt( map.getZoom(),10 );
		   current++;
		   if(current>20){
			   current=20;
		   }
		   map.setZoom(current);
		   jQuery(".infoBox").hide();
		});  
	}
	
	//Zoom Out
	if(  document.getElementById('doc-mapminus') ){ 
		google.maps.event.addDomListener(document.getElementById('doc-mapminus'), 'click', function () {      
			var current= parseInt( map.getZoom(),10);
			current--;
			if(current<0){
				current=0;
			}
			map.setZoom(current);
			jQuery(".infoBox").hide();
		});  
	}
	
	//Lock Map
	if( document.getElementById('doc-lock') ){ 
		google.maps.event.addDomListener(document.getElementById('doc-lock'), 'click', function () {
			if(lock == 'lock'){
				map.setOptions({ 
						scrollwheel: false,
						draggable: false 
					}
				);
				
				jQuery("#doc-lock").html('<i class="fa fa-lock" aria-hidden="true"></i>');
				lock = 'unlock';
			}else if(lock == 'unlock'){
				map.setOptions({ 
						scrollwheel: false,
						draggable: true 
					}
				);
				jQuery("#doc-lock").html('<i class="fa fa-unlock" aria-hidden="true"></i>');
				lock = 'lock';
			}
		});
	}
	//
	if( _data_list.status == 'found' && typeof(response_data) != "undefined" && response_data !== null ){
		jQuery('#gmap-noresult').html('').hide(); //Hide No Result Div
		var markers = new Array();
		var info_windows = new Array();

		for (var i=0; i < response_data.length; i++) {
			markers[i] = new google.maps.Marker({
				position: new google.maps.LatLng(response_data[i].latitude,response_data[i].longitude),
				map: map,
				icon: response_data[i].icon,
				title: response_data[i].title,
				animation: google.maps.Animation.DROP,
				visible: true
			});
		
			bounds.extend(markers[i].getPosition());
			
			var boxText = document.createElement("div");
			
			boxText.className = 'directory-detail';
			var innerHTML = "";
			boxText.innerHTML += response_data[i].html.content;
			
			var myOptions = {
				content: boxText,
				disableAutoPan: true,
				maxWidth: 0,
				alignBottom: true,
				pixelOffset: new google.maps.Size( 40, 150 ),
				zIndex: null,
				closeBoxMargin: "0 0 -16px -16px",
				closeBoxURL: dir_close_marker,
				infoBoxClearance: new google.maps.Size( 1, 1 ),
				isHidden: false,
				pane: "floatPane",
				enableEventPropagation: false
			};
		
			var ib = new InfoBox( myOptions );
			attachInfoBoxToMarker( map, markers[i], ib );
			
			//oms = new OverlappingMarkerSpiderfier(map);   
    		//MapSpiderfyMarkers(markers);

		}
		
		map.fitBounds(bounds);
		
		/* Marker Clusters */
		var markerClustererOptions = {
			ignoreHidden: true,
			//maxZoom: 14,
			styles: [{
				textColor: scripts_vars.dir_cluster_color,
				url: scripts_vars.dir_cluster_marker,
				height: 48,
				width: 48
			}]
		};
		
		var markerClusterer = new MarkerClusterer( map, markers, markerClustererOptions );
	} else{
		map.fitBounds(bounds);
		jQuery('#gmap-noresult').html(gmap_norecod).show();
	}
}
//Assign Info window to marker
function attachInfoBoxToMarker( map, marker, infoBox ){
	google.maps.event.addListener( marker, 'click', function(){
		var scale = Math.pow( 2, map.getZoom() );
		var offsety = ( (100/scale) || 0 );
		var projection = map.getProjection();
		var markerPosition = marker.getPosition();
		var markerScreenPosition = projection.fromLatLngToPoint( markerPosition );
		var pointHalfScreenAbove = new google.maps.Point( markerScreenPosition.x, markerScreenPosition.y - offsety );
		var aboveMarkerLatLng = projection.fromPointToLatLng( pointHalfScreenAbove );
		map.setCenter( aboveMarkerLatLng );
		
		jQuery(".infoBox").hide();
		infoBox.open( map, marker );
		
		infoBox.addListener("domready", function() {
			jQuery('.tg-map-marker').on('click', '.add-to-fav', function (event) {
				event.preventDefault();
				
				var user_status	= scripts_vars.user_status;
				var fav_message	= scripts_vars.fav_message;
				var fav_nothing	= scripts_vars.fav_nothing;
	
				if( user_status == 'false' ){
					jQuery.sticky(fav_message, {classList: 'important', speed: 200, autoclose: 7000});
					return false;	
				}
				
				var _this	= jQuery(this);
				var wl_id	= _this.data('wl_id');
				_this.append('<i class="fa fa-refresh fa-spin"></i>');
				_this.addClass('loading');
				
				jQuery.ajax({
					type: "POST",
					url: scripts_vars.ajaxurl,
					data: 'wl_id='+wl_id+'&action=docdirect_update_wishlist',
					dataType: "json",
					success: function (response) {
						_this.removeClass('loading');
						_this.find('i.fa-spin').remove();
						jQuery('.login-message').show();
						if (response.type == 'success') {
							jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000,position: 'top-right',});
							_this.removeClass('tg-like add-to-fav');
							_this.addClass('tg-dislike');
						} else {
							jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
						}
					}
			   });
			});
		});
	});
}


//Init detail page Map Scripts
function docdirect_init_detail_map_script( _data_list ){
	var dir_latitude	 = scripts_vars.dir_latitude;
	var dir_longitude	= scripts_vars.dir_longitude;
	var dir_map_type	 = scripts_vars.dir_map_type;
	var dir_close_marker		  = scripts_vars.dir_close_marker;
	var dir_cluster_marker		= scripts_vars.dir_cluster_marker;
	var dir_map_marker			= scripts_vars.dir_map_marker;
	var dir_cluster_color		 = scripts_vars.dir_cluster_color;
	var dir_zoom				  = 8;
	var dir_map_scroll			= scripts_vars.dir_map_scroll;
	var gmap_norecod			  = scripts_vars.gmap_norecod;
	var map_styles			    = scripts_vars.map_styles;


	if( _data_list.status == 'found' ){
		var response_data	= _data_list.users_list;
	    if( typeof(response_data) != "undefined" && response_data !== null ) {
			var location_center = new google.maps.LatLng(response_data[0].latitude,response_data[0].longitude);
		} else {
				var location_center = new google.maps.LatLng(dir_latitude,dir_longitude);
		}
	} else{
		var location_center = new google.maps.LatLng(dir_latitude,dir_longitude);
	}
	
	if(dir_map_type == 'ROADMAP'){
		var map_id = google.maps.MapTypeId.ROADMAP;
	} else if(dir_map_type == 'SATELLITE'){
		var map_id = google.maps.MapTypeId.SATELLITE;
	} else if(dir_map_type == 'HYBRID'){
		var map_id = google.maps.MapTypeId.HYBRID;
	} else if(dir_map_type == 'TERRAIN'){
		var map_id = google.maps.MapTypeId.TERRAIN;
	} else {
		var map_id = google.maps.MapTypeId.ROADMAP;
	}
	
	var scrollwheel	= true;
	var lock		   = 'unlock';
	
	if( dir_map_scroll == 'false' ){
		scrollwheel	= false;
		lock		   = 'lock';
	}
	
	var mapOptions = {
		zoom: parseInt(dir_zoom),
		maxZoom: 20,
		mapTypeId: map_id,
		scaleControl: true,
		scrollwheel: scrollwheel,
		disableDefaultUI: true
	}
	
	var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
	
	var styles = docdirect_get_map_styles(map_styles);
	if(styles != ''){
		var styledMap = new google.maps.StyledMapType(styles, {name: 'Styled Map'});
		map.mapTypes.set('map_style', styledMap);
		map.setMapTypeId('map_style');
	}
		
	var bounds = new google.maps.LatLngBounds();
	
	//Zoom In
	if(  document.getElementById('doc-mapplus') ){ 
		 google.maps.event.addDomListener(document.getElementById('doc-mapplus'), 'click', function () {      
		   var current= parseInt( map.getZoom(),10 );
		   current++;
		   if(current>20){
			   current=20;
		   }
		   map.setZoom(current);
		   jQuery(".infoBox").hide();
		});  
	}
	
	//Zoom Out
	if(  document.getElementById('doc-mapminus') ){ 
		google.maps.event.addDomListener(document.getElementById('doc-mapminus'), 'click', function () {      
			var current= parseInt( map.getZoom(),10);
			current--;
			if(current<0){
				current=0;
			}
			map.setZoom(current);
			jQuery(".infoBox").hide();
		});  
	}
	
	//Lock Map
	if( document.getElementById('doc-lock') ){ 
		google.maps.event.addDomListener(document.getElementById('doc-lock'), 'click', function () {
			if(lock == 'lock'){
				map.setOptions({ 
						scrollwheel: true,
						draggable: true 
					}
				);
				
				jQuery("#doc-lock").html('<i class="fa fa-unlock-alt" aria-hidden="true"></i>');
				lock = 'unlock';
			}else if(lock == 'unlock'){
				map.setOptions({ 
						scrollwheel: false,
						draggable: false 
					}
				);
				
				jQuery("#doc-lock").html('<i class="fa fa-lock" aria-hidden="true"></i>');
				lock = 'lock';
			}
		});
	}

	//
	if( _data_list.status == 'found' && typeof(response_data) != "undefined" && response_data !== null ){
		jQuery('#gmap-noresult').html('').hide(); //Hide No Result Div
		var markers = new Array();
		var info_windows = new Array();

		for (var i=0; i < response_data.length; i++) {
			markers[i] = new google.maps.Marker({
				position: new google.maps.LatLng(response_data[i].latitude,response_data[i].longitude),
				map: map,
				icon: response_data[i].icon,
				title: response_data[i].title,
				animation: google.maps.Animation.DROP,
				visible: true
			});
		
			bounds.extend(markers[i].getPosition());
			
			var boxText = document.createElement("div");
			
			boxText.className = 'directory-detail';
			var innerHTML = "";
			boxText.innerHTML += response_data[i].html.content;
			
			var myOptions = {
				content: boxText,
				disableAutoPan: true,
				maxWidth: 0,
				alignBottom: true,
				pixelOffset: new google.maps.Size( 65, 15 ),
				zIndex: null,
				infoBoxClearance: new google.maps.Size( 1, 1 ),
				isHidden: false,
				pane: "floatPane",
				enableEventPropagation: false
			};
		
			var ib = new InfoBox( myOptions );
			attachInfoBoxToMarker( map, markers[i], ib );
			ib.open(map,markers[i]);

		}
		
		map.fitBounds(bounds);
		
		/* Marker Clusters */
		var markerClustererOptions = {
			ignoreHidden: true,
			//maxZoom: 14,
			styles: [{
				textColor: scripts_vars.dir_cluster_color,
				url: scripts_vars.dir_cluster_marker,
				height: 48,
				width: 48
			}]
		};
		
		var markerClusterer = new MarkerClusterer( map, markers, markerClustererOptions );
	} else{
		map.fitBounds(bounds);
		jQuery('#gmap-noresult').html(gmap_norecod).show();
	}
}
//Set Spiderfy Markers
function MapSpiderfyMarkers(markers){
    for (var i = 0; i < markers.length; i++) {
        if(typeof oms !== 'undefined'){
           oms.addMarker(markers[i]);
        }
    }
}

function docdirect_get_map_styles(style) {

    var styles = '';
    if (style == 'view_1') {
        var styles = [{ "featureType": "administrative.country", "elementType": "geometry", "stylers": [{ "visibility": "simplified" }, { "hue": "#ff0000" }] }];
    } else if (style == 'view_2') {
        var styles = [{ "featureType": "water", "elementType": "all", "stylers": [{ "hue": "#7fc8ed" }, { "saturation": 55 }, { "lightness": -6 }, { "visibility": "on" }] }, { "featureType": "water", "elementType": "labels", "stylers": [{ "hue": "#7fc8ed" }, { "saturation": 55 }, { "lightness": -6 }, { "visibility": "off" }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "hue": "#83cead" }, { "saturation": 1 }, { "lightness": -15 }, { "visibility": "on" }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "hue": "#f3f4f4" }, { "saturation": -84 }, { "lightness": 59 }, { "visibility": "on" }] }, { "featureType": "landscape", "elementType": "labels", "stylers": [{ "hue": "#ffffff" }, { "saturation": -100 }, { "lightness": 100 }, { "visibility": "off" }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "hue": "#ffffff" }, { "saturation": -100 }, { "lightness": 100 }, { "visibility": "on" }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "hue": "#bbbbbb" }, { "saturation": -100 }, { "lightness": 26 }, { "visibility": "on" }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "hue": "#ffcc00" }, { "saturation": 100 }, { "lightness": -35 }, { "visibility": "simplified" }] }, { "featureType": "road.highway", "elementType": "geometry", "stylers": [{ "hue": "#ffcc00" }, { "saturation": 100 }, { "lightness": -22 }, { "visibility": "on" }] }, { "featureType": "poi.school", "elementType": "all", "stylers": [{ "hue": "#d7e4e4" }, { "saturation": -60 }, { "lightness": 23 }, { "visibility": "on" }] }];
    } else if (style == 'view_3') {
        var styles = [{ "featureType": "water", "stylers": [{ "saturation": 43 }, { "lightness": -11 }, { "hue": "#0088ff" }] }, { "featureType": "road", "elementType": "geometry.fill", "stylers": [{ "hue": "#ff0000" }, { "saturation": -100 }, { "lightness": 99 }] }, { "featureType": "road", "elementType": "geometry.stroke", "stylers": [{ "color": "#808080" }, { "lightness": 54 }] }, { "featureType": "landscape.man_made", "elementType": "geometry.fill", "stylers": [{ "color": "#ece2d9" }] }, { "featureType": "poi.park", "elementType": "geometry.fill", "stylers": [{ "color": "#ccdca1" }] }, { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "color": "#767676" }] }, { "featureType": "road", "elementType": "labels.text.stroke", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "poi", "stylers": [{ "visibility": "off" }] }, { "featureType": "landscape.natural", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#b8cb93" }] }, { "featureType": "poi.park", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.sports_complex", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.medical", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.business", "stylers": [{ "visibility": "simplified" }] }];

    } else if (style == 'view_4') {
        var styles = [{ "elementType": "geometry", "stylers": [{ "hue": "#ff4400" }, { "saturation": -68 }, { "lightness": -4 }, { "gamma": 0.72 }] }, { "featureType": "road", "elementType": "labels.icon" }, { "featureType": "landscape.man_made", "elementType": "geometry", "stylers": [{ "hue": "#0077ff" }, { "gamma": 3.1 }] }, { "featureType": "water", "stylers": [{ "hue": "#00ccff" }, { "gamma": 0.44 }, { "saturation": -33 }] }, { "featureType": "poi.park", "stylers": [{ "hue": "#44ff00" }, { "saturation": -23 }] }, { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "hue": "#007fff" }, { "gamma": 0.77 }, { "saturation": 65 }, { "lightness": 99 }] }, { "featureType": "water", "elementType": "labels.text.stroke", "stylers": [{ "gamma": 0.11 }, { "weight": 5.6 }, { "saturation": 99 }, { "hue": "#0091ff" }, { "lightness": -86 }] }, { "featureType": "transit.line", "elementType": "geometry", "stylers": [{ "lightness": -48 }, { "hue": "#ff5e00" }, { "gamma": 1.2 }, { "saturation": -23 }] }, { "featureType": "transit", "elementType": "labels.text.stroke", "stylers": [{ "saturation": -64 }, { "hue": "#ff9100" }, { "lightness": 16 }, { "gamma": 0.47 }, { "weight": 2.7 }] }];

    } else if (style == 'view_5') {
        var styles = [{ "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#e9e9e9" }, { "lightness": 17 }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 20 }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }, { "lightness": 17 }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#ffffff" }, { "lightness": 29 }, { "weight": 0.2 }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 18 }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 16 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 21 }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#dedede" }, { "lightness": 21 }] }, { "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#ffffff" }, { "lightness": 16 }] }, { "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#333333" }, { "lightness": 40 }] }, { "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#f2f2f2" }, { "lightness": 19 }] }, { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#fefefe" }, { "lightness": 20 }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#fefefe" }, { "lightness": 17 }, { "weight": 1.2 }] }];

    } else if (style == 'view_6') {
        var styles = [{ "featureType": "landscape", "stylers": [{ "hue": "#FFBB00" }, { "saturation": 43.400000000000006 }, { "lightness": 37.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.highway", "stylers": [{ "hue": "#FFC200" }, { "saturation": -61.8 }, { "lightness": 45.599999999999994 }, { "gamma": 1 }] }, { "featureType": "road.arterial", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 51.19999999999999 }, { "gamma": 1 }] }, { "featureType": "road.local", "stylers": [{ "hue": "#FF0300" }, { "saturation": -100 }, { "lightness": 52 }, { "gamma": 1 }] }, { "featureType": "water", "stylers": [{ "hue": "#0078FF" }, { "saturation": -13.200000000000003 }, { "lightness": 2.4000000000000057 }, { "gamma": 1 }] }, { "featureType": "poi", "stylers": [{ "hue": "#00FF6A" }, { "saturation": -1.0989010989011234 }, { "lightness": 11.200000000000017 }, { "gamma": 1 }] }];
    } else {
        var styles = [{ "featureType": "administrative.country", "elementType": "geometry", "stylers": [{ "visibility": "simplified" }, { "hue": "#ff0000" }] }];
    }
    return styles;
}
