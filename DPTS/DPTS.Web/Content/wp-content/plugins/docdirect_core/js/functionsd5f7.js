"use strict";
jQuery(document).ready(function (e) {

	jQuery(".open-map").on("click",function(){
		var $this	= jQuery(this);
		$this.parents('figure').find(".overlay-map").slideToggle("slow");
		jQuery(this, ".see-map").toggleClass("active");
	});

});

/*
 * @get absolute path
 * @return{}
*/
function getAbsolutePath() {
    var loc = window.location;
    var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);
    return loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length));
}