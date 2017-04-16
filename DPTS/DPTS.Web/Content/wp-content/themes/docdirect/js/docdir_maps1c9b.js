/**
 * Holds google map object and related utility entities.
 * @constructor
 */
 
//Init Map
jQuery.docdirect_init_map = function(map_lat,map_lng){

	var mapwrapper = jQuery('#location-pickr-map');
	
	if(typeof(scripts_vars) != "undefined" && scripts_vars !== null) {
		var dir_latitude	 = scripts_vars.dir_latitude;
		var dir_longitude	= scripts_vars.dir_longitude;
		var dir_map_type	 = scripts_vars.dir_map_type;
		var dir_close_marker		  = scripts_vars.dir_close_marker;
		var dir_cluster_marker		= scripts_vars.dir_cluster_marker;
		var dir_map_marker			= scripts_vars.dir_map_marker;
		var dir_cluster_color		 = scripts_vars.dir_cluster_color;
		var dir_zoom				  = scripts_vars.dir_zoom;
		var dir_map_scroll			= scripts_vars.dir_map_scroll;
	} else{
		var dir_latitude	 = 51.5001524;
		var dir_longitude	= -0.1262362;
		var dir_map_type	 = 'ROADMAP';
		var dir_zoom				  = 12;
		var dir_map_scroll			= false;
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
	
	if( dir_map_scroll == 'false' ){
		scrollwheel	= false;
	}

	mapwrapper.gmap3({
	  map:{
		  options:{
			panControl: false,
			scaleControl: false,
			navigationControl: false,
			draggable:true,
			scrollwheel: scrollwheel,
			streetViewControl: false,
			center:[map_lat,map_lng],
			zoom:  parseInt(dir_zoom),
			mapTypeId: map_id,
			mapTypeControl: true,
			mapTypeControlOptions: {
			  style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
			  position: google.maps.ControlPosition.RIGHT_BOTTOM
			},
			zoomControl: true,
			zoomControlOptions: {
			  position: google.maps.ControlPosition.LEFT_BOTTOM
			},
		  },
		  callback:function(){
			setTimeout(function(){
				jQuery.docdirect_map_fallback();
			},300);
		  }
	  },
	  marker:{
		values:[{
			latLng:[map_lat,map_lng],
		}],
		options:{
		  draggable: true
		},
		events:{
			dragend: function(marker){
				jQuery('#location-latitude').val(marker.getPosition().lat());
				jQuery('#location-longitude').val(marker.getPosition().lng());
			},
		},
	  }

	});
};
//Call To Add Map
jQuery.docdirect_map_fallback = function(){
	var map_div = jQuery('#location-pickr-map').gmap3("get");
	var map_input = document.getElementById("location-address");
	jQuery("#location-address").bind("keypress", function(e) {
	  if (e.keyCode == 13) {               
		e.preventDefault();
		return false;
	  }
	});
		
	var autocomplete = new google.maps.places.Autocomplete(map_input);
	autocomplete.bindTo("bounds", map_div);
		
	google.maps.event.addListener(autocomplete, "place_changed", function() {
		var place = autocomplete.getPlace();
		if (!place.geometry) {
		  return;
		}
		
		if (place.geometry.viewport) {
		  map_div.fitBounds(place.geometry.viewport);
		} else {
		  map_div.setCenter(place.geometry.location);
		  //map_div.setZoom(11);
		}

		var marker = jQuery('#location-pickr-map').gmap3({get:"marker"});
	    marker.setPosition(place.geometry.location);
		jQuery("#location-latitude").val(marker.getPosition().lat());
		jQuery("#location-longitude").val(marker.getPosition().lng());
		
	});

}
jQuery(document).ready(function(e) {
	
	//Geo Locate
	jQuery(document).on("click",".geolocate",function(){
		jQuery('#location-pickr-map').gmap3({
		  getgeoloc:{
			callback : function(latLng){
			  if (latLng){
				var geocoder = new google.maps.Geocoder();
				geocoder.geocode({"latLng":latLng},function(data,status){
				    if (status == google.maps.GeocoderStatus.OK) {
						if (data[0]) {
							jQuery('#location-pickr-map').gmap3({
							  marker:{ 
								latLng:latLng
							  },
							  map:{
								options:{
								  zoom: 11
								}
							  }
							});
							jQuery("#location-address").val(data[0].formatted_address);
							jQuery("#location-latitude").val(latLng.lat());
							jQuery("#location-longitude").val(latLng.lng());
							//jQuery("#lat").val(latLng.lat());
							//jQuery("#lng").val(latLng.lng());
						}
					}
				});
			  }
			}
		  }
		});
		return false;
	});

	jQuery.docdirect_init_map();

    //Toogle Radius Search
	jQuery(document).on('click', '.geodistance', function () {
	    jQuery('.geodistance_range').toggle();
	});
});