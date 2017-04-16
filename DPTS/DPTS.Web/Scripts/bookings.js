"use strict";
jQuery(document).ready(function ($) {
	//Constants
	//var empty_category	= scripts_vars.empty_category;
	//var complete_fields	= scripts_vars.complete_fields;
	//var system_error	= scripts_vars.system_error;
    //var custom_slots_dates	= scripts_vars.custom_slots_dates;
    var scripts_vars = {
        "ajaxurl": "",
        "award_name": "Award Name",
        "award_date": "Award Date",
        "award_description": "Award Description",
        "file_upload_title": "Avatar Upload",
        "delete_message": "Are you sure you want to delete your account?",
        "deactivate": "Are you sure you want to deactivate your account?",
        "delete_title": "Delete account?",
        "deactivate_title": "Deactivate account?",
        "docdirect_upload_nounce": "1025f290a0",
        "dir_close_marker": "https:\/\/themographics.com\/wordpress\/docdirect\/wp-content\/themes\/docdirect\/images\/close.png",
        "dir_cluster_marker": "\/\/themographics.com\/wordpress\/docdirect\/wp-content\/uploads\/2016\/04\/cluster.png",
        "dir_map_marker": {
            "attachment_id": "7", "url": "\/\/themographics.com\/wordpress\/docdirect\/wp-content\/uploads\/2016\/03\/03.png"
        }
    ,
        "dir_cluster_color": "#505050",
        "dir_map_type": "ROADMAP",
        "dir_zoom": "11",
        "dir_longitude": "-0.1262362",
        "dir_latitude": "51.5001524",
        "dir_datasize": "10485760",
        "data_size_in_kb": "10240kb",
        "dir_map_scroll": "false",
        "map_styles": "none",
        "site_key": "6Ld7fgcUAAAAANmJV2K3RulmACd_3DJwmYqT7bQw",
        "rating_1": "Not Satisfied",
        "rating_2": "Satisfied",
        "rating_3": "Good",
        "rating_4": "Very Good",
        "rating_5": "Excellent",
        "delete_award": "Delete Award",
        "delete_award_message": "Are you sure, you want to delete this award?",
        "delete_education": "Delete Degree",
        "delete_education_message": "Are you sure, you want to delete this Degree?",
        "delete_experience": "Delete Experience",
        "delete_experience_message": "Are you sure, you want to delete this experience?",
        "delete_category": "Delete Category",
        "delete_category_message": "Are you sure, you want to delete this category?",
        "delete_service": "Delete Service",
        "delete_service_message": "Are you sure, you want to delete this service?",
        "delete_slot": "Delete Slot",
        "delete_slot_message": "Are you sure, you want to delete this slot?",
        "delete_slot_date": "Delete Slot Date",
        "delete_slot_date_message": "Are you sure, you want to delete this slot date?",
        "approve_appointment": "Approve Appointment?",
        "approve_appointment_message": "Are you sure, you want to approve this appointment?",
        "cancel_appointment": "Cancel Appointment?",
        "cancel_appointment_message": "Are you sure, you want to cancel this appointment?",
        "booking_time": "Please select booking time.",
        "gmap_norecod": "No Record Found.",
        "fav_message": "Please login first.",
        "fav_nothing": "Nothing found.",
        "empty_category": "Please add category name.",
        "complete_fields": "Please fill all the fields.",
        "system_error": "Some error occur, please try again later.",
        "valid_email": "Please add valid email address.",
        "user_status": "true",
        "custom_slots_dates": "Atleast one date! start or end date is required.",
        "finish": "Finish"
    };
	var loder_html	= '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';

	var loader_fullwidth_html	= '<div class="docdirect-site-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';


	/*-------------------------------------------------
	 * Appointment process
	 *
	 *-------------------------------------------------*/

	//Tabs Management
	//jQuery('.booking-model-contents .tg-navdocappointment li').not('.active').addClass('disabled');
    //jQuery('.booking-model-contents .tg-navdocappointment li').not('.active').find('a').removeAttr("data-toggle");

	//Next step
	var Z_Steps = {};
	Z_Steps.booking_step = {};
	window.Z_Steps = Z_Steps;
	Z_Steps.booking_step	= 1;

	jQuery(document).on('click','.bk-step-next',function(e){
		e.preventDefault();
		var _this	= jQuery(this);

		var data_id	= jQuery('.tg-appointmenttabcontent').data('id');

		//Step 1 data
		//var bk_category   = _this.parents(".tg-appointmenttabcontent").find('.bk_category option:selected').val();
		//var bk_service	= _this.parents(".tg-appointmenttabcontent").find('.bk_service option:selected').val();

		//Step 2 data
		var bk_subject	     = _this.parents(".tg-appointmenttabcontent").find('input[name="subject"]').val();
		var bk_name	   		= _this.parents(".tg-appointmenttabcontent").find('input[name="username"]').val();
		var bk_userphone = _this.parents(".tg-appointmenttabcontent").find('input[name="mobilenumber"]').val();
		var bk_useremail	   = _this.parents(".tg-appointmenttabcontent").find('input[name="useremail"]').val();
		var bk_booking_note	= _this.parents(".tg-appointmenttabcontent").find('textarea[name="booking_note"]').val();

		if (
			Z_Steps.booking_step	== 1
		) {
			var is_time_checked	= jQuery('.step-one-slots input[name="slottime"]:checked').val();
			if( !is_time_checked ){
				jQuery.sticky(scripts_vars.booking_time, {classList: 'important', speed: 200, autoclose: 5000});
				return false;
			}

			jQuery('.booking-model-contents').append(loder_html);
			var dataString = 'data_id=' + data_id + '&action=docdirect_get_booking_step_two';
			jQuery.ajax({
				type: "POST",
				url: "/Appointment/VisitorContactDeatils/",
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();

					Z_Steps.booking_step	= 2;
					//jQuery('.step-two-contents').html(response.data);
					_this.parents(".tg-appointmenttabcontent").find('input[name="username"]').val(response.username);
					_this.parents(".tg-appointmenttabcontent").find('input[name="mobilenumber"]').val(response.mobilenumber);
					_this.parents(".tg-appointmenttabcontent").find('input[name="useremail"]').val(response.useremail);
                    //docdirect_booking_calender();
					docdirect_appointment_tabs(2);
					jQuery('.bk-step-2').trigger('click');
				}
			});
		}  else if(
			bk_subject
			&&
			bk_name
			&&
			bk_userphone
			&&
			bk_useremail
			&&
			bk_booking_note
			&&
			Z_Steps.booking_step	== 2
		) {
			if( !( docdirect_isValidEmailAddress(bk_useremail) ) ){
				jQuery('body').find('.docdirect-loader-wrap').remove();
				jQuery.sticky(scripts_vars.valid_email, {classList: 'important', speed: 200, autoclose: 5000});
				return false;
			}

			jQuery('.booking-model-contents').append(loder_html);
			var dataString = 'data_id='+data_id+'&action=docdirect_get_booking_step_two';
			jQuery.ajax({
				type: "POST",
				url: "/Appointment/PaymentMode/",
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();

					Z_Steps.booking_step	= 3;
					//jQuery('.step-three-contents').html(response.data);
					jQuery('.bk-step-3').trigger('click');
					//docdirect_intl_tel_input();
					docdirect_appointment_tabs(3);

				}
			});
		} else if (
	        !bk_subject
			||
			!bk_name
			||
			!bk_userphone
			||
			!bk_useremail
			||
			!bk_booking_note && Z_Steps.booking_step == 2
	    ) {
		    jQuery.sticky("all fields are mandatory !!", { classList: 'important', speed: 200, autoclose: 5000 });
	    } else if (
	        Z_Steps.booking_step == 3
	    ) {
	        jQuery('.booking-model-contents').append(loder_html);
	        var serialize_data = jQuery('.appointment-form').serialize();
	        var dataString = serialize_data + '&data_id=' + data_id + '&action=docdirect_do_process_booking';
	        jQuery.ajax({
	            type: "POST",
	            url: "/Appointment/FinishBooking/",
	            data: serialize_data,
	            dataType: "json",
	            success: function(response) {
	                jQuery('body').find('.docdirect-loader-wrap').remove();
	                if (response.result == "fail") {
	                    jQuery.sticky(scripts_vars.system_error, { classList: 'important', speed: 200, autoclose: 5000 });
	                    return false;
	                } else {
	                    //jQuery('.step-four-contents').html(response.data);
	                    Z_Steps.booking_step = 1;
	                    jQuery('.bk-step-4').trigger('click');
	                    docdirect_appointment_tabs(4);
	                    jQuery('.step-one-contents, .step-two-contents, .step-three-contents').remove();
	                    jQuery('.booking-step-button').find('.bk-step-prev').remove();
	                    jQuery('.booking-step-button').find('.bk-step-next').html('Finish');
	                    jQuery('.booking-step-button').find('.bk-step-next').addClass('finish-booking');
	                }
	            }
	        });
	    }

	});

	//Finish Booking
	jQuery(document).on('click', '.bk-step-next.finish-booking', function (e) {
	    //var redirectPath = $('#RedirectUrl').val();
	    window.location.href = '/Home';
	});

	//Prev step
	jQuery(document).on('click','.bk-step-prev',function(){

		if( Z_Steps.booking_step == 4 ){
			Z_Steps.booking_step = 3;
			docdirect_appointment_tabs(3);
		} else if( Z_Steps.booking_step == 3 ){
			Z_Steps.booking_step = 2;
			docdirect_appointment_tabs(2);
		} else if( Z_Steps.booking_step == 2 ){
			Z_Steps.booking_step = 1;
			docdirect_appointment_tabs(1);
		}else{
			Z_Steps.booking_step	= 1;
		}
	});

	//Booking detail show/hide
	jQuery(document).on('click','.get-detail',function(){
	    jQuery(this).parents('tr').next('tr').slideToggle(200);
	});

	//Change Appointment Status - doc
	jQuery(document).on('click','.get-process',function(){

		var _this	= jQuery(this);

		var type	= _this.data('type');
		var id	  = _this.data('id');

		var dataString = 'type='+type+'&id='+id+'&action=docdirect_change_appointment_status';

		if( type == 'approve' ) {
			var _title	= scripts_vars.approve_appointment;
			var _message	= scripts_vars.approve_appointment_message;
		} else{
			var _title	= scripts_vars.cancel_appointment;
			var _message	= scripts_vars.cancel_appointment_message;
		}

		jQuery.confirm({
			'title': _title,
			'message': _message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						jQuery('.booking-model-contents').append(loder_html);
						jQuery.ajax({
							type: "POST",
							url: scripts_vars.ajaxurl,
							data: dataString,
							dataType:"json",
							success: function(response) {
								jQuery('body').find('.docdirect-loader-wrap').remove();

								if( response.action_type == 'approved' ){
									var approved = wp.template( 'status-approved' );
									_this.parents('td').html(approved);

								} else if( response.action_type == 'cancelled' ){
									_this.parents('tr').remove();
									_this.parents('tr').next('tr').remove();
								}
							}
						});
					}
				},
				'No': {
					'class': 'gray',
					'action': function () {
						return false;
					}	// Nothing to do in this case. You can as well omit the action property.
				}
			}
		});

	});


	//Privacy Settings
	jQuery(document).on('change','.privacy-switch',function(){

		var _this	= jQuery(this);

		var type	= _this.data('type');
		var id	  = _this.data('id');

		var serialize_data	= jQuery('.tg-form-privacy').serialize();

		var dataString = serialize_data+'&action=docdirect_save_privacy_settings';
		jQuery('.booking-model-contents').append(loder_html);
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-loader-wrap').remove();
				jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
			}
		});

	});

	//Set Currency
	jQuery(document).on('change', '.currency_symbol', function() {
        var code = jQuery(this).val();
        var dataString = 'code=' + code + '&action=docdirect_get_currency_symbol';
        var _this = jQuery(this);
		jQuery('.booking-model-contents').append(loader_fullwidth_html);
        jQuery.ajax({
            type: "POST",
            url: ajaxurl,
            data: dataString,
            success: function(response) {
                _this.parents('.booking-currency-wrap').find("input[name=currency_symbol]").val(response);
                jQuery('body').find('.docdirect-site-wrap').remove();
            }
        });

        return false;
    });

	//Booking Seach - doc
	jQuery('.booking-search-date').datetimepicker({
	  format:'Y-m-d',
	  timepicker:false
	});
    //primery value
	jQuery('.booking-pickr strong').html(moment().format('MMM D, dddd'));
	jQuery('#booking_date').val(moment().format('YYYY-MM-DD'));

	docdirect_booking_calender(this);
});



//Input Type Phone
function docdirect_intl_tel_input(){
	jQuery("#teluserphone").intlTelInput({
	  // allowDropdown: false,
	  // autoHideDialCode: false,
	  // autoPlaceholder: false,
	  // dropdownContainer: "body",
	  // excludeCountries: ["us"],
	  geoIpLookup: function(callback) {
	   jQuery.get("http://ipinfo.io", function() {}, "jsonp").always(function(resp) {
		 var countryCode = (resp && resp.country) ? resp.country : "";
		  callback(countryCode);
	   });
	   },
	  initialCountry: "auto",
	  // nationalMode: false,
	  // numberType: "MOBILE",
	  // onlyCountries: ['us', 'gb', 'ch', 'ca', 'do'],
	  // preferredCountries: ['cn', 'jp'],
	  separateDialCode: true,
	  //utilsScript: "build/js/utils.js"
	});
}
//Validate email
function docdirect_isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
//booking doc
var scripts_doc_vars = {

    "approve_appointment": "Approve Appointment?",
    "approve_appointment_message": "Are you sure, you want to approve this appointment?",
    "cancel_appointment": "Cancel Appointment?",
    "cancel_appointment_message": "Are you sure, you want to cancel this appointment?",
    "visit_appointment": "Visit Appointment?",
    "visit_appointment_message": "Are you sure, patient is visited this appointment?",
    "fail_appointment": "Fail Appointment?",
    "fail_appointment_message": "Are you sure, patient is failed to Visit?",
};
var loder_html_doc = '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';
function approveAppoinment(id, action) {
    var _this = $(this);
    if (action == 'approve') {
        var _title = scripts_doc_vars.approve_appointment;
        var _message = scripts_doc_vars.approve_appointment_message;
    } else if (action == 'cancel') {
        var _title = scripts_doc_vars.cancel_appointment;
        var _message = scripts_doc_vars.cancel_appointment_message;
    }
    else if (action == 'visit') {
        var _title = scripts_doc_vars.visit_appointment;
        var _message = scripts_doc_vars.visit_appointment_message;
    }
    else if (action == 'failed') {
        var _title = scripts_doc_vars.fail_appointment;
        var _message = scripts_doc_vars.fail_appointment_message;
    }
    var dataString = 'type=' + action + '&id=' + id + '&action=docdirect_change_appointment_status';

    jQuery.confirm({
        'title': _title,
        'message': _message,
        'buttons': {
            'Yes': {
                'class': 'blue',
                'action': function () {
                    jQuery('.booking-model-contents').append(loder_html_doc);
                    jQuery.ajax({
                        type: "POST",
                        url: "/Doctor/ChangeBookingStatus/",
                        data: dataString,
                        dataType: "json",
                        success: function (response) {
                            jQuery('body').find('.docdirect-loader-wrap').remove();
                            //if (response.action_type == 'approved') {
                            //    var approved = wp.template('status-approved');
                            //    _this.parents('td').html(approved);
                            //} else if (response.action_type == 'cancelled') {
                            //    _this.parents('tr').remove();
                            //    _this.parents('tr').next('tr').remove();
                            //}if (response.action_type == 'visited') {
                            //    _this.parents('tr').remove();
                            //    _this.parents('tr').next('tr').remove();
                            //}
                            window.location.reload();
                        }
                    });
                }
            },
            'No': {
                'class': 'gray',
                'action': function () {
                    return false;
                }	// Nothing to do in this case. You can as well omit the action property.
            }
        }
    });
}
//Booking Calender
function docdirect_appointment_tabs(current){
	//Tab Items
	jQuery('.tg-navdocappointment li').removeClass('active');
	var _navitems = jQuery(".tg-navdocappointment li");
	_navitems.each(function(index, li) {
		if( parseInt(index) < parseInt(current) ) {
			jQuery(this).addClass('active');
		}
	});

	//Tab Contents
	jQuery('.tg-appointmenttabcontent .tab-pane').hide();

	if( current == 1 ){
		jQuery('.tg-appointmenttabcontent .step-one-contents').show();
	} else if( current == 2 ){
		jQuery('.tg-appointmenttabcontent .step-two-contents').show();
	} else if( current == 3 ){
		jQuery('.tg-appointmenttabcontent .step-three-contents').show();
	} else if( current == 4 ){
		jQuery('.tg-appointmenttabcontent .step-four-contents').show();
	}

}

//Booking Calender
function docdirect_booking_calender(val){
	//Booking Calender
	var loder_html	= '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';

	jQuery('.booking-pickr').datetimepicker({
	    format: 'Y-m-d',
	    minDate: new Date(),
	    timepicker: false,
	    onChangeDateTime: function (dp, $input) {
	        var slot_date = moment(dp).format('YYYY-MM-DD');
	        jQuery('.booking-pickr strong').html(moment(dp).format('MMM D, dddd'));

	        jQuery('#booking_date').val(slot_date);

	        var _this = jQuery(this);
	        var data_id = jQuery('.tg-appointmenttabcontent').data('id');

	        jQuery('.booking-model-contents').append(loder_html);
	        var docId = $('#doctorId').val();
	        var dataString = 'slot_date=' + slot_date + '&doctorId=' + docId + '';
	        jQuery.ajax({
	            type: "POST",
	            url: "/Appointment/BookingScheduleByDate/",
	            data: dataString,
	            dataType: "json",
	            success: function (c) {
	                jQuery('body').find('.docdirect-loader-wrap').remove();
	                Z_Steps.booking_step = 1;
	                jQuery('.step-one-slots .tg-timeslotswrapper').html(c.response);
	                docdirect_booking_calender();
	            }
	        });
	        return false;
	    }
	});
}

//Check if empty value
function docdirect_isEmpty_value(val){
    return (val === undefined || val == null || val.length <= 0) ? true : false;
}
//Get Time Slot List
function docdirect_get_list(data,_this){
	var json_list	= JSON.stringify(data);
	var dataString = 'json_list='+json_list+'&action=docdirect_get_time_slots_list';

	jQuery.ajax({
		type: "POST",
		url: scripts_vars.ajaxurl,
		data: dataString,
		dataType:"json",
		success: function(response) {
			jQuery('body').find('.docdirect-site-wrap').remove();
			_this.parents('.custom-time-periods').find('.custom-timeslots-data-area').html(response.timeslot_list);
			jQuery('.bk-save-custom-slots').trigger('click');

		}
	});
	return false;
}

//Get Time Slot List
function docdirect_update_timeslots_data(data,_this){
	var data = JSON.stringify(jQuery('.custom-slots-main').serializeObject());
	jQuery('.custom_timeslots_object').val(data);
}