"use strict";
jQuery(document).ready(function ($) {
	
    var scripts_vars = {
        "booking_time": "Please select booking time.",
        "gmap_norecod": "No Record Found.",
        "complete_fields": "Please fill all the fields.",
        "system_error": "Some error occur, please try again later.",
        "valid_email": "Please add valid email address.",
        "custom_slots_dates": "Atleast one date! start or end date is required.",
        "finish": "Finish"
    };
	var loder_html	= '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';

	var loader_fullwidth_html	= '<div class="docdirect-site-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';


	/*-------------------------------------------------
	 * Rescheduling process
	 *
	 *-------------------------------------------------*/

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
		var doctorId = _this.parents(".tg-appointmenttabcontent").find('input[name="DoctorId"]').val();
		var PatientId = _this.parents(".tg-appointmenttabcontent").find('input[name="PatientId"]').val();
		var BookingId = _this.parents(".tg-appointmenttabcontent").find('input[name="BookingId"]').val();

		if (
			doctorId
			&&
			PatientId
			&&
			BookingId
			&&
			Z_Steps.booking_step == 1
		) {
		    var is_time_checked = jQuery('.step-one-slots input[name="slottime"]:checked').val();
		    if (!is_time_checked) {
		        jQuery.sticky(scripts_vars.booking_time, { classList: 'important', speed: 200, autoclose: 5000 });
		        return false;
		    }

		    jQuery('.booking-model-contents').append(loder_html);
		    var dataString = 'doctorId=' + doctorId + '&patientId=' + PatientId + '&bookingId=' + BookingId + '';
		    jQuery.ajax({
		        type: "POST",
		        url: "/Appointment/VerifyReScheduling/",
		        data: dataString,
		        dataType: "json",
		        success: function (response) {
		            jQuery('body').find('.docdirect-loader-wrap').remove();
		            if (response.result == "success")
		            {
		                Z_Steps.booking_step = 1;
		                jQuery('.bk-step-2').trigger('click');
		                docdirect_appointment_tabs(2);
		                // jQuery('.step-one-contents').remove();
		                jQuery('#nDate').html($('#booking_date').val());
		                jQuery('#nTime').html(jQuery('.step-one-slots input[name="slottime"]:checked').val());
                        jQuery('.booking-step-button').find('.bk-step-prev').remove();
                        jQuery('.booking-step-button').find('.bk-step-next').html('Finish');
                        jQuery('.booking-step-button').find('.bk-step-next').addClass('finish-booking');
		            } else {
		                jQuery.sticky(scripts_vars.system_error, { classList: 'important', speed: 200, autoclose: 5000 });
		                return false;
		            }
		        },
                 error: function () {
                     alert(scripts_vars.system_error);
                    }
		    });
		}
	});
	//Finish Booking
	jQuery(document).on('click', '.bk-step-next.finish-booking', function (e) {
	    ////var redirectPath = $('#RedirectUrl').val();
	    //window.location.href = '/Home';
	    jQuery('.booking-model-contents').append(loder_html);
	        var serialize_data = jQuery('.appointment-form').serialize();
	        jQuery.ajax({
	            type: "POST",
	            url: "/Appointment/FinishReScheduling/",
	            data: serialize_data,
	            dataType: "json",
	            success: function(response) {
	                jQuery('body').find('.docdirect-loader-wrap').remove();
	                if (response.result == "success") {
	                    //var redirectPath = $('#RedirectUrl').val();
	                    window.location.href = '/Doctor/BookingListings';
	                } else {
	                    jQuery.sticky(scripts_vars.system_error, { classList: 'important', speed: 200, autoclose: 5000 });
	                    return false;
	                }
	            }
	        });
	});

	//Prev step
	jQuery(document).on('click','.bk-step-prev',function(){

	  if( Z_Steps.booking_step == 2 ){
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
function docdirect_update_timeslots_data(data,_this){
	var data = JSON.stringify(jQuery('.custom-slots-main').serializeObject());
	jQuery('.custom_timeslots_object').val(data);
}