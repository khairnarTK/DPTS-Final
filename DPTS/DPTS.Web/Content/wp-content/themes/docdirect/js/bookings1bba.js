"use strict";
jQuery(document).ready(function ($) {
	//Constants
	var empty_category	= scripts_vars.empty_category;
	var complete_fields	= scripts_vars.complete_fields;
	var system_error	= scripts_vars.system_error;
	var custom_slots_dates	= scripts_vars.custom_slots_dates;
	var loder_html	= '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';
	
	var loader_fullwidth_html	= '<div class="docdirect-site-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';
	
	
	/*-------------------------------------------------
	 * Categories Action
	 *
	 *-------------------------------------------------*/
	 
	//Add category
	jQuery(document).on('click','.bk-add-category-item',function(){
		var load_catgories = wp.template( 'append-service-category' );
		jQuery( '.bk-category-wrapper' ).append(load_catgories);
	});
	
	//Edit category
	jQuery(document).on('click','.bk-edit-category',function(){
		jQuery(this).parents('.bk-category-item').find('.bk-current-category').slideToggle(200);
	});
	
	
	//Add,Edit categories
	jQuery(document).on('click','.bk-maincategory-add',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var key 	  = _this.data('key');
		var type 	 = _this.data('type');
		
		var title	= _this.parents('.bk-current-category').find('.service-category-title').val();
		
		jQuery('body').append(loader_fullwidth_html);
		
		if( title == '' ){
			jQuery('body').find('.docdirect-site-wrap').remove();
			jQuery.sticky(empty_category, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'key='+key+'&type='+type+'&title='+title+'&action=docdirect_update_service_category';

		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-site-wrap').remove();
				if( response.message_type == 'error' ) {
					jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
				} else{
					var update_category = wp.template( 'update-service-category' );
					var category_item	= update_category(response);
					_this.parents('.bk-category-item').html(category_item);
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
					
					//each categories
					var categories	= {};
					jQuery(".bk-category-wrapper input[type=text]").each(function() {
						var _key	= jQuery( this ).data('key');
						var _val	= jQuery( this ).val();
						if( _key && _val ) {
							categories[_key]	= _val;
						}
					});
					
					//refresh services categories
					jQuery(".bk-services-wrapper select[name=service_category]").each(function() {
						var _template 	= wp.template( 'append-options' );
						var _options	= _template(categories);
						jQuery( this ).html(_options);
					});
					
				}
			}
		});
		return false;
	});
	
	//Delete categories
	jQuery(document).on('click','.bk-delete-category',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var key 	  = _this.data('key');
		var type     = _this.data('type');
		
		//Process newly embed item
		if( type == 'new-delete' ) {
			jQuery( this ).parents('.bk-category-item').remove();
			return false;
		}
		
		if( key == '' ){
			jQuery.sticky(system_error, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'key='+key+'&action=docdirect_delete_service_category';
		
		jQuery.confirm({
			'title': scripts_vars.delete_category,
			'message': scripts_vars.delete_category_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						//Process dadtabase item
						jQuery('body').append(loader_fullwidth_html);
						jQuery.ajax({
							type: "POST",
							url: scripts_vars.ajaxurl,
							data: dataString,
							dataType:"json",
							success: function(response) {
								jQuery('body').find('.docdirect-site-wrap').remove();
								if( response.message_type == 'error' ) {
									jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
								} else{
									jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
									_this.parents('.bk-category-item').remove();
									
									//each categories
									var categories	= {};
									jQuery(".bk-category-wrapper input[type=text]").each(function() {
										var _key	= jQuery( this ).data('key');
										var _val	= jQuery( this ).val();
										if( _key && _val ) {
											categories[_key]	= _val;
										}
									});
									
									//refresh services categories
									jQuery(".bk-services-wrapper select[name=service_category]").each(function() {
										var _template 	= wp.template( 'append-options' );
										var _options	= _template(categories);
										jQuery( this ).html(_options);
									});
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
		
		
		return false;
	});
	
	
	
	/*-------------------------------------------------
	 * Services Action
	 *
	 *-------------------------------------------------*/
	 //Add Service
	jQuery(document).on('click','.bk-add-service-item',function(){
		var categories	= {};
		 jQuery(".bk-category-wrapper input[type=text]").each(function() {
			var _key	= jQuery( this ).data('key');
			var _val	= jQuery( this ).val();
            if( _key && _val ) {
                categories[_key]	= _val;
            }
        });

		var load_services = wp.template( 'append-service' );
		var services_item	= load_services(categories);
		jQuery( '.bk-services-wrapper' ).append(services_item);
		
	});
	
	//Edit Service
	jQuery(document).on('click','.bk-edit-service',function(){
		jQuery(this).parents('.bk-service-item').find('.bk-current-service').slideToggle(200);
	});
	
	jQuery('input[name="service_price"]').on('keyup', function() {
        //validateAmount();
    });

	//Add,Edit services
	jQuery(document).on('click','.bk-service-add',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var key 	  = _this.data('key');
		var type 	 = _this.data('type');
		
		var service_title	= _this.parents('.bk-current-service').find('input[name=service_title]').val();
		var service_category	= _this.parents(".bk-current-service").find('.service_category option:selected').val();
		var service_price	= _this.parents('.bk-current-service').find('input[name=service_price]').val();
		
		jQuery('body').append(loader_fullwidth_html);
		
		if( service_title == '' || service_category == '' || service_price == '' ){
			jQuery('body').find('.docdirect-site-wrap').remove();
			jQuery.sticky(complete_fields, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'key='+key+'&type='+type+'&service_title='+service_title+'&service_category='+service_category+'&service_price='+service_price+'&action=docdirect_update_services';

		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-site-wrap').remove();
				if( response.message_type == 'error' ) {
					jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
				} else{
					var categories	= {};
					jQuery(".bk-category-wrapper input[type=text]").each(function() {
						var _key	= jQuery( this ).data('key');
						var _val	= jQuery( this ).val();
						if( _key && _val ) {
							categories[_key]	= _val;
						}
					});
					
					response.cats	=  categories;
					var update_service = wp.template( 'update-service' );
					var service_item	= update_service(response);
					_this.parents('.bk-service-item').html(service_item);
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
		
				}
			}
		});
		return false;
	});
	
	//Delete Service
	jQuery(document).on('click','.bk-delete-service',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var key 	  = _this.data('key');
		var type     = _this.data('type');
		
		//Process newly embed item
		if( type == 'new-delete' ) {
			jQuery( this ).parents('.bk-service-item').remove();
			return false;
		}

		if( key == '' ){
			jQuery.sticky(system_error, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'key='+key+'&action=docdirect_delete_service';
		
		jQuery.confirm({
			'title': scripts_vars.delete_service,
			'message': scripts_vars.delete_service_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						//Process dadtabase item
						jQuery('body').append(loader_fullwidth_html);
						
						jQuery.ajax({
							type: "POST",
							url: scripts_vars.ajaxurl,
							data: dataString,
							dataType:"json",
							success: function(response) {
								jQuery('body').find('.docdirect-site-wrap').remove();
								if( response.message_type == 'error' ) {
									jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
								} else{
									jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
									_this.parents('.bk-service-item').remove();
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
		
		return false;
	});
	
	/*-------------------------------------------------
	 * Default Slots
	 *
	 *-------------------------------------------------*/
	 
	//Add slots
	jQuery(document).on('click','.add-default-slots',function(){
		var _this	= jQuery(this);
		var _form	= jQuery(this).parents('.tg-daytimeslot').find('.timeslots-form-area .tg-timeslotswrapper');

		if( _form.length == 0 ){
			var default_slots = wp.template( 'default-slots' );
			_this.parents('.tg-daytimeslot').find('.tg-timeslots .timeslots-form-area').append(default_slots);
		}
		
	});
	
	//Remove Form
	jQuery(document).on('click','.remove-slots-form',function(){
		jQuery(this).parents('.tg-timeslotswrapper').remove();
	});
	
	//Add Default Slots
	jQuery(document).on('click','.save-time-slots',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		
		var day	  		 = _this.parents('.tg-daytimeslot').data('day');
		var slot_title	  = _this.parents('.tg-daytimeslot').find('input[name=slot_title]').val();
		var start_time	  = _this.parents(".tg-daytimeslot").find('.start_time option:selected').val();
		var end_time	    = _this.parents(".tg-daytimeslot").find('.end_time option:selected').val();
		var meeting_time	= _this.parents(".tg-daytimeslot").find('.meeting_time option:selected').val();
		var padding_time	= _this.parents(".tg-daytimeslot").find('.padding_time option:selected').val();
		
		jQuery('body').append(loader_fullwidth_html);
		
		if( start_time == '' 
			|| 
			end_time == '' 
			|| 
			meeting_time == '' 
			|| 
			padding_time == '' 
		){
			jQuery('body').find('.docdirect-site-wrap').remove();
			jQuery.sticky(complete_fields, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'day='+day+'&slot_title='+slot_title+'&start_time='+start_time+'&end_time='+end_time+'&meeting_time='+meeting_time+'&padding_time='+padding_time+'&action=docdirect_add_time_slots';

		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-site-wrap').remove();
				if( response.message_type == 'error' ) {
					jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
				} else{
					_this.parents('.tg-daytimeslot').find('.timeslots-data-area').html(response.slots_data);
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
		
				}
			}
		});
		return false;
	});
	
	//Disbale/Enable Timings
	jQuery(document).on('change','select[name=start_time]',function(e) {
		
		var endTimeSelect = jQuery(this).parents('.tg-timeslotswrapper').find('select[name=end_time]');
		var startTimeVal = jQuery(this).val();
		endTimeSelect.find('option').removeAttr('disabled');
		endTimeSelect.find('option').each(function() {
			var current = jQuery(this).val();
			if ( current <= startTimeVal){
				jQuery(this).attr('disabled',true);
			}
		});
	});
	
	//Delete Time Slot
	jQuery(document).on('click','.delete-current-slot',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var day 	  = _this.data('day');
		var time     = _this.data('time');
		
		if( day == '' || time == '' ){
			jQuery.sticky(system_error, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		var dataString = 'day='+day+'&time='+time+'&action=docdirect_delete_time_slot';
		
		jQuery.confirm({
			'title': scripts_vars.delete_slot,
			'message': scripts_vars.delete_slot_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						//Process dadtabase item
						jQuery('body').append(loader_fullwidth_html);
						
						jQuery.ajax({
							type: "POST",
							url: scripts_vars.ajaxurl,
							data: dataString,
							dataType:"json",
							success: function(response) {
								jQuery('body').find('.docdirect-site-wrap').remove();
								if( response.type == 'error' ) {
									jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
								} else{
									jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
									_this.parents('.tg-doctimeslot').remove();
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
		
		return false;
	});
	
	/*-------------------------------------------------
	 * Custom Slots
	 *
	 *-------------------------------------------------*/
	 
	 //Add Custom dates
	jQuery(document).on('click','.bk-add-dates',function(){

		var slot_dates = wp.template( 'custom-timelines' );
		jQuery('.custom-timeslots-dates_wrap').append(slot_dates);
		
		docdirect_update_timeslots_data();//update data
		
		jQuery('.slots-datepickr').datetimepicker({
		  format:'Y-m-d',
		  minDate: new Date(),
		  timepicker:false
		});
		
	});
	
	
	//Slots Date Pciker
	jQuery('.slots-datepickr').datetimepicker({
	  format:'Y-m-d',
	  timepicker:false
	});
	 

	//Add Custom Timings
	jQuery(document).on('click','.bk-save-custom-slots',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		
		docdirect_update_timeslots_data();//update data
		
		var custom_timeslots_object	= jQuery('.custom_timeslots_object').val();
		var dataString = 'custom_timeslots_object='+custom_timeslots_object+'&action=docdirect_save_custom_slots';
		
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-loader-wrap').remove();
				if( response.message_type == 'error' ) {
					//jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 5000});
				} else{
					_this.parents('.tg-daytimeslot').find('.timeslots-data-area').html(response.slots_data);
					//jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
		
				}
			}
		});
		
	});
	
	//Add custom slots
	jQuery(document).on('click','.add-custom-timeslots',function(){
		var _this	= jQuery(this);
		var _form	= jQuery(this).parents('.custom-time-periods').find('.custom-timeslots-data .tg-timeslotswrapper');

		if( _form.length == 0 ){
			var default_slots = wp.template( 'custom-slots' );
			_this.parents('.custom-time-periods').find('.custom-timeslots-data').append(default_slots);
		}
		
	});
	
	//Add Default Slots
	jQuery(document).on('click','.save-custom-time-slots',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		
		var formData = JSON.stringify(jQuery(this).parents('form').serializeObject());
			
		var slot_title	  = _this.parents('.custom-timeslots-data').find('input[name=slot_title]').val();
		var start_time	  = _this.parents(".custom-timeslots-data").find('.start_time option:selected').val();
		var end_time	    = _this.parents(".custom-timeslots-data").find('.end_time option:selected').val();
		var meeting_time	= _this.parents(".custom-timeslots-data").find('.meeting_time option:selected').val();
		var padding_time	= _this.parents(".custom-timeslots-data").find('.padding_time option:selected').val();
		var cus_start_date	= _this.parents(".custom-time-periods").find('input[name=cus_start_date]').val();
		var cus_end_date	= _this.parents(".custom-time-periods").find('input[name=cus_end_date]').val();
		
		var cus_end_date	= _this.parents(".custom-time-periods").find('input[name=cus_end_date]').val();
		var cus_end_date	= _this.parents(".custom-time-periods").find('input[name=cus_end_date]').val();
		
		jQuery('body').append(loader_fullwidth_html);
		
		if( start_time == '' 
			|| 
			end_time == '' 
			|| 
			meeting_time == '' 
			|| 
			padding_time == '' 
		){
			jQuery('body').find('.docdirect-site-wrap').remove();
			jQuery.sticky(complete_fields, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}
		
		if( cus_start_date == '' 
			&&
			cus_end_date == '' 
		){
			jQuery('body').find('.docdirect-site-wrap').remove();
			jQuery.sticky(custom_slots_dates, {classList: 'important', speed: 200, autoclose: 5000});
			return false;
		}

		var dataString = 'cus_start_date='+cus_start_date+'&cus_end_date='+cus_end_date+'&slot_title='+slot_title+'&start_time='+start_time+'&end_time='+end_time+'&meeting_time='+meeting_time+'&padding_time='+padding_time+'&action=docdirect_add_custom_time_slots';

		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				 
				_this.parents('.custom-time-periods').find('.custom_time_slots').val(JSON.stringify(response.timeslot));
				_this.parents('.custom-time-periods').find('.custom_time_slot_details').val(JSON.stringify(response.timeslot_details));
				
				docdirect_get_list(response,_this);
			}
		});
		return false;
	});
	
	//Delete Custom dates
	jQuery(document).on('click','.bk-delete-dates',function(){

		var slot_dates = wp.template( 'custom-timelines' );
		jQuery('.custom-timeslots-dates_wrap').append(slot_dates);
		
		docdirect_update_timeslots_data();//update data
		
		jQuery('.slots-datepickr').datetimepicker({
		  format:'Y-m-d',
		  minDate: new Date(),
		  timepicker:false
		});
	});
	
	//Delete Custom slot
	jQuery(document).on('click','.delete-custom-slot',function(){

		var _this	= jQuery(this);
		var current_slots	= _this.parents('.custom-time-periods').find('.custom_time_slots').val();
		var current_slot_details	= _this.parents('.custom-time-periods').find('.custom_time_slot_details').val();
		var slot	= _this.data('time');
		
		var dataString = 'slot='+slot+'&current_slots='+current_slots+'&current_slot_details='+current_slot_details+'&action=docdirect_delete_custom_time_slots';
		
		jQuery.confirm({
			'title': scripts_vars.delete_slot,
			'message': scripts_vars.delete_slot_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						jQuery.ajax({
							type: "POST",
							url: scripts_vars.ajaxurl,
							data: dataString,
							dataType:"json",
							success: function(response) {
								jQuery('body').find('.docdirect-loader-wrap').remove();
								_this.parents('.custom-time-periods').find('.custom_time_slots').val(JSON.stringify(response.timeslot));
								_this.parents('.custom-time-periods').find('.custom_time_slot_details').val(JSON.stringify(response.timeslot_details));
								_this.parents('.tg-doctimeslot').remove();
								jQuery('.bk-save-custom-slots').trigger('click');//Save Slots
								
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
	
	//Delete slot date
	jQuery(document).on('click','.delete-slot-date',function(){

		var _this	= jQuery(this);
		jQuery.confirm({
			'title': scripts_vars.delete_slot_date,
			'message': scripts_vars.delete_slot_date_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						_this.parents('.custom-time-periods').remove();
						jQuery('.bk-save-custom-slots').trigger('click');//Save Slots
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
	
	//Get Services
	jQuery(document).on('change','.bk_category', function (event) {
		var id		  = jQuery(this).val();		
		
		if( Z_Editor.services[id] ) {
			var load_services = wp.template( 'load-services' );
			var data = [];
			var services	 = Z_Editor.services[id];
			var _options	= load_services(services);
			jQuery( '.bk_service' ).html(_options);
		} else{
			var load_services = wp.template( 'load-services' );
			var data = [];
			var services	 = Z_Editor.all_services;
			var _options	= load_services(services);
			jQuery( '.bk_service' ).html(_options);
		}
	});
	
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
		var bk_category   = _this.parents(".tg-appointmenttabcontent").find('.bk_category option:selected').val();
		var bk_service	= _this.parents(".tg-appointmenttabcontent").find('.bk_service option:selected').val();
		
		//Step 2 data
		var bk_subject	     = _this.parents(".tg-appointmenttabcontent").find('input[name="subject"]').val();
		var bk_name	   		= _this.parents(".tg-appointmenttabcontent").find('input[name="username"]').val();
		var bk_userphone	   = _this.parents(".tg-appointmenttabcontent").find('input[name="userphone"]').val();
		var bk_useremail	   = _this.parents(".tg-appointmenttabcontent").find('input[name="useremail"]').val();
		var bk_booking_note	= _this.parents(".tg-appointmenttabcontent").find('textarea[name="booking_note"]').val();
		
		//Check step 1
		if( bk_category 
			&&
			bk_service
			&&
			Z_Steps.booking_step	== 1
		) {

			jQuery('.booking-model-contents').append(loder_html);
			var dataString = 'data_id='+data_id+'&action=docdirect_get_booking_step_two';
			jQuery.ajax({
				type: "POST",
				url: scripts_vars.ajaxurl,
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();

					Z_Steps.booking_step	= 2;
					jQuery('.step-two-slots .tg-timeslotswrapper').html(response.data);
					docdirect_booking_calender();
					docdirect_appointment_tabs(2);
					jQuery('.bk-step-2').trigger('click');
					
				}
			});
		} else if( 
			Z_Steps.booking_step	== 2
		) {
			var is_time_checked	= jQuery('.step-two-slots input[name="slottime"]:checked').val();
			if( !is_time_checked ){
				jQuery.sticky(scripts_vars.booking_time, {classList: 'important', speed: 200, autoclose: 5000});
				return false;
			}
			
			jQuery('.booking-model-contents').append(loder_html);
			var dataString = 'data_id='+data_id+'&action=docdirect_get_booking_step_three';
			jQuery.ajax({
				type: "POST",
				url: scripts_vars.ajaxurl,
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();

					Z_Steps.booking_step	= 3;
					jQuery('.step-three-contents').html(response.data);
					docdirect_appointment_tabs(3);
					jQuery('.bk-step-3').trigger('click');
					docdirect_intl_tel_input();
					
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
			Z_Steps.booking_step	== 3
		) {
			if( !( docdirect_isValidEmailAddress(bk_useremail) ) ){
				jQuery('body').find('.docdirect-loader-wrap').remove();
				jQuery.sticky(scripts_vars.valid_email, {classList: 'important', speed: 200, autoclose: 5000});
				return false;
			}
			
			jQuery('.booking-model-contents').append(loder_html);
			var dataString = 'data_id='+data_id+'&action=docdirect_get_booking_step_four';
			jQuery.ajax({
				type: "POST",
				url: scripts_vars.ajaxurl,
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();

					Z_Steps.booking_step	= 4;
					jQuery('.step-four-contents').html(response.data);
					jQuery('.bk-step-4').trigger('click');
					docdirect_intl_tel_input();
					docdirect_appointment_tabs(4);
					
				}
			});
		}  else if( 
			Z_Steps.booking_step	== 4
		) {
			jQuery('.booking-model-contents').append(loder_html);
			var serialize_data	= jQuery('.appointment-form').serialize();
			var dataString = serialize_data+'&data_id='+data_id+'&action=docdirect_do_process_booking';
			jQuery.ajax({
				type: "POST",
				url: scripts_vars.ajaxurl,
				data: dataString,
				dataType:"json",
				success: function(response) {
					jQuery('body').find('.docdirect-loader-wrap').remove();
					if (response.payment_type == 'paypal') {
						jQuery('body').append(response.form_data);
						//Z_Steps.booking_step	= 1;
						//jQuery('.booking-step-button').find('button').prop('disabled', true);
						
					} else if (response.payment_type == 'stripe') {
						
						var obj = [];
						jQuery.each(response, function(index, element) {
							obj[index] = element;
						});
						
						var handler = StripeCheckout.configure({
							key: obj.key,
							token: function(token) {
								 jQuery('body').append(loder_html);
								 jQuery.ajax({
									type: "POST",
									url: scripts_vars.ajaxurl,
									data: { 
										'action': 'docdirect_complete_booking_stripe_payment',
										
										'username': obj.username,
										'email': obj.email,
										'order_no': obj.order_no,
										'user_to': obj.user_to,
										'user_from': obj.user_from,
										'subject': obj.subject,
										'process': obj.process,
										'name': obj.name,
										'amount': obj.amount,
										'total_amount': obj.total_amount,
										'currency': obj.currency,
										'data': obj.data,
										'process': '',
										'type': obj.type,
										'payment_type': obj.payment_type,
										'token': token,
									},
									dataType: "json",
									success: function(response) {
										handler.close();
										
										jQuery('body').find('.docdirect-loader-wrap').remove();
										jQuery('.step-five-contents').html(response.data);
										Z_Steps.booking_step	= 1;
										jQuery('.bk-step-5').trigger('click');
										docdirect_appointment_tabs(5);
										jQuery('.step-one-contents, .step-two-contents, .step-three-contents, .step-four-contents').remove();
										jQuery('.booking-step-button').find('button').prop('disabled', true);
									}
								});
							}
						});

						handler.open({
						  name: obj.name,
						  description: obj.subject,
						  amount: obj.amount,
						  email: obj.email,
						  currency: obj.currency,
						  allowRememberMe: false,
						  opened:function(){
							//Some Action
						  },
						  closed:function(){
							//Reload
						  }
						});
						
					} else{
						jQuery('body').find('.docdirect-loader-wrap').remove();
						jQuery('.step-five-contents').html(response.data);
						Z_Steps.booking_step	= 1;
						jQuery('.bk-step-5').trigger('click');
						docdirect_appointment_tabs(5);
						jQuery('.step-one-contents, .step-two-contents, .step-three-contents, .step-four-contents').remove();
						jQuery('.booking-step-button').find('.bk-step-prev').remove();
						jQuery('.booking-step-button').find('.bk-step-next').html(scripts_vars.finish);
						jQuery('.booking-step-button').find('.bk-step-next').addClass('finish-booking');
					}	
				}
			});
		} 
		
	});
	
	//Finish Booking
	jQuery(document).on('click','.bk-step-next.finish-booking',function(e){
		window.location.reload();
	});
	
	//Prev step
	jQuery(document).on('click','.bk-step-prev',function(){

		if( Z_Steps.booking_step == 5 ){
			Z_Steps.booking_step = 4;
			docdirect_appointment_tabs(4);
		} else if( Z_Steps.booking_step == 4 ){
			Z_Steps.booking_step = 3;
			docdirect_appointment_tabs(3);
		} else if( Z_Steps.booking_step == 3 ){
			Z_Steps.booking_step = 2;
			docdirect_appointment_tabs(2);
		} else if( Z_Steps.booking_step == 2 ){
			Z_Steps.booking_step = 1;
			docdirect_appointment_tabs(1);
		} else{
			Z_Steps.booking_step	= 1;
		}
	});
	
	//Booking detail show/hide
	jQuery(document).on('click','.get-detail',function(){
		jQuery(this).parents('tr').next('tr').slideToggle(200);
	});
	
	//Change Appointment Status
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
	
	//Booking Seach
	jQuery('.booking-search-date').datetimepicker({
	  format:'Y-m-d',
	  timepicker:false
	});
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
	} else if( current == 5 ){
		jQuery('.tg-appointmenttabcontent .step-five-contents').show();
	}

}

//Booking Calender
function docdirect_booking_calender(val){
	//Booking Calender
	var loder_html	= '<div class="docdirect-loader-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';
	  
	  jQuery('.booking-pickr').datetimepicker({
	  format:'Y-m-d',
	  minDate: new Date(),
	  timepicker:false,
	  onChangeDateTime:function(dp,$input){
		var slot_date	= moment(dp).format('YYYY-MM-DD');
		jQuery('.booking-pickr strong').html(moment(dp).format('MMM D, dddd'));
		
		jQuery('.booking_date').val(slot_date);
		
		var _this	= jQuery(this);
		var data_id	= jQuery('.tg-appointmenttabcontent').data('id');
		
		jQuery('.booking-model-contents').append(loder_html);
		var dataString = 'slot_date='+slot_date+'&data_id='+data_id+'&action=docdirect_get_booking_step_two';
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-loader-wrap').remove();
				Z_Steps.booking_step	= 2;
				jQuery('.step-two-slots .tg-timeslotswrapper').html(response.data);
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