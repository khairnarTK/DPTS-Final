"use strict";
jQuery(document).ready(function ($) {
	//Constants
	var award_name	= scripts_vars.award_name;
	var award_date	= scripts_vars.award_date;
	var award_description	= scripts_vars.award_description;
	var file_upload_title	= scripts_vars.file_upload_title;
	var docdirect_upload_nounce	= scripts_vars.docdirect_upload_nounce;
	var delete_message	= scripts_vars.delete_message;
	var deactivate	= scripts_vars.deactivate;
	var delete_title	= scripts_vars.delete_title;
	var deactivate_title	= scripts_vars.deactivate_title;
	var dir_datasize	= scripts_vars.dir_datasize;
	var data_size_in_kb	= scripts_vars.data_size_in_kb;
	var loder_html	= '<div class="docdirect-site-wrap"><div class="docdirect-loader"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';

	//Update User schedules
	jQuery(document).on('click','.update-schedules',function(e){
		e.preventDefault();
		var $this 	= jQuery(this);

		var serialize_data	= jQuery('#form-docschedule').serialize();
		var dataString = serialize_data+'&action=docdirect_update_schedules';

		jQuery('body').append(loder_html);
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
				}
			}
		});
		return false;

	});

	/*********************************************************
	 * @Profile Uploader
	 * @User profile image update code
	 ********************************************************/
	//Uploader Handler
	var uploader = new plupload.Uploader({
		browse_button: 'upload-profile-avatar',          // this can be an id of a DOM element or the DOM element itself
		file_data_name: 'docdirect_uploader',
		container: 'plupload-container',
		multi_selection : false,
		multipart_params : {
			"type" : "profile_image",
		},
		url: scripts_vars.ajaxurl + "?action=docdirect_image_uploader&nonce=" + docdirect_upload_nounce,
		filters: {
			mime_types : [
				{ title : file_upload_title, extensions : "jpg,jpeg,gif,png" }
			],
			max_file_size: data_size_in_kb,
			prevent_duplicates: true
		}

	});

	 uploader.init();

	 // Process Duraing Upload
	 uploader.bind('FilesAdded', function(up, files) {
		var html = '';
		var profileThumb = "";
		plupload.each(files, function(file) {
			//Do something duraing upload
		});
		up.refresh();
		uploader.start();
	});

	/* File percentage */
	uploader.bind('UploadProgress', function(up, file) {
		jQuery('.tg-docimg .user-avatar').find('.avatar-percentage').remove();
		jQuery('.user-avatar').append('<span class="avatar-percentage">'+file.percent+"%</span>");
	});

	/* In case of error */
	uploader.bind('Error', function( up, err ) {
		jQuery('#errors-log').html(err.message);
	});

	//On Complete Uplaod
	uploader.bind('FileUploaded', function ( up, file, ajax_response ) {
		var response = $.parseJSON( ajax_response.response );
		if ( response.success ) {
			jQuery('.tg-docimg .user-avatar').find('.avatar-percentage').remove();
			jQuery('.tg-docimg .user-avatar').find('img').attr('src', response.url);
			jQuery('.tg-docprofile-img').find('img').attr('src', response.url);
		} else {
			console.log ( response );
		}
	});



	/*********************************************************
	 * @Profile Banner
	 * @User profile banner image update code
	 ********************************************************/
	//Uploader Handler
	var banner_uploader = new plupload.Uploader({
		browse_button: 'upload-profile-banner',          // this can be an id of a DOM element or the DOM element itself
		file_data_name: 'docdirect_uploader',
		container: 'plupload-container',
		multi_selection : false,
		multipart_params : {
			"type" : "profile_banner",
		},
		url: scripts_vars.ajaxurl + "?action=docdirect_image_uploader&nonce=" + docdirect_upload_nounce,
		filters: {
			mime_types : [
				{ title : file_upload_title, extensions : "jpg,jpeg,gif,png" }
			],
			max_file_size: data_size_in_kb,
			prevent_duplicates: true
		}

	});

	 banner_uploader.init();

	 // Process Duraing Upload
	 banner_uploader.bind('FilesAdded', function(up, files) {
		var html = '';
		var profileThumb = "";
		plupload.each(files, function(file) {
			//Do something duraing upload
		});
		up.refresh();
		banner_uploader.start();
	});

	/* File percentage */
	banner_uploader.bind('UploadProgress', function(up, file) {
		jQuery('.tg-docimg .user-banner').find('.banner-percentage').remove();
		jQuery('.user-banner').append('<span class="banner-percentage">'+file.percent+"%</span>");
	});

	/* In case of error */
	banner_uploader.bind('Error', function( up, err ) {
		jQuery('#errors-log').html(err.message);
	});

	//On Complete Uplaod
	banner_uploader.bind('FileUploaded', function ( up, file, ajax_response ) {
		var response = $.parseJSON( ajax_response.response );
		if ( response.success ) {
			jQuery('.tg-docimg .user-banner').find('.banner-percentage').remove();
			jQuery('.tg-docimg .user-banner').find('img').attr('src', response.url);
		} else {
			console.log ( response );
		}
	});

	/*********************************************************
	 * @Email Uploader
	 * @User profile image update code
	 ********************************************************/
	//Uploader Handler
	var email_uploader = new plupload.Uploader({
		browse_button: 'upload-email',          // this can be an id of a DOM element or the DOM element itself
		file_data_name: 'docdirect_uploader',
		container: 'plupload-container',
		multi_selection : false,
		multipart_params : {
			"type" : "email_image",
		},
		url: scripts_vars.ajaxurl + "?action=docdirect_image_uploader&nonce=" + docdirect_upload_nounce,
		filters: {
			mime_types : [
				{ title : file_upload_title, extensions : "jpg,jpeg,gif,png" }
			],
			max_file_size: data_size_in_kb,
			prevent_duplicates: true
		}

	});

	 email_uploader.init();

	 // Process Duraing Upload
	 email_uploader.bind('FilesAdded', function(up, files) {
		var html = '';
		var profileThumb = "";
		plupload.each(files, function(file) {
			//Do something duraing upload
		});
		up.refresh();
		email_uploader.start();
	});

	/* File percentage */
	email_uploader.bind('UploadProgress', function(up, file) {
		jQuery('.tg-docimg .user-email').find('.avatar-percentage').remove();
		jQuery('.user-email').append('<span class="avatar-percentage">'+file.percent+"%</span>");
	});

	/* In case of error */
	email_uploader.bind('Error', function( up, err ) {
		jQuery('#errors-log').html(err.message);
	});

	//On Complete Uplaod
	email_uploader.bind('FileUploaded', function ( up, file, ajax_response ) {
		var response = $.parseJSON( ajax_response.response );
		if ( response.success ) {
			jQuery('.tg-docimg .user-email').find('.avatar-percentage').remove();
			jQuery('.tg-docimg .user-email').find('img').attr('src', response.url);
		} else {
			console.log ( response );
		}
	});

	/*********************************************************
	 * @Gallery Images Uploader
	 * @Gallery images update code
	 ********************************************************/

	$( "#gallery-sortable-container" ).sortable({
		revert: 100,
		placeholder: "sortable-placeholder",
		cursor: "move"
	});
	/* initialize uploader */
	var uploaderArguments = {
		browse_button: 'attach-gallery',          // this can be an id of a DOM element or the DOM element itself
		file_data_name: 'docdirect_uploader',
		container: 'plupload-container',
		drop_element: 'drag-and-drop',
		multipart_params : {
			"type" : "user_gallery",
		},
		url: scripts_vars.ajaxurl + "?action=docdirect_image_uploader&nonce=" + docdirect_upload_nounce,
		filters: {
			mime_types : [
				{ title : file_upload_title, extensions : "jpg,jpeg,gif,png" }
			],
			max_file_size: data_size_in_kb,
			prevent_duplicates: true
		}
	};

	var GalleryUploader = new plupload.Uploader( uploaderArguments );
	GalleryUploader.init();

	/* Run after adding file */
	GalleryUploader.bind('FilesAdded', function(up, files) {
		var html = '';
		var galleryThumbnail = "";
		plupload.each(files, function(file) {
			 galleryThumbnail += '<li class="gallery-item gallery-thumb-item" id="thumb-' + file.id + '">' + '' + '</li>';
		});

		jQuery('.doc-user-gallery').append(galleryThumbnail);
		up.refresh();
		GalleryUploader.start();
	});

	/* Run during upload */
	GalleryUploader.bind('UploadProgress', function(up, file) {
		jQuery(".doc-user-gallery #thumb-" + file.id).html('<span class="gallery-percentage">' + file.percent + "%</span>");
	});


	/* In case of error */
	GalleryUploader.bind('Error', function( up, err ) {
		jQuery('#errors-log-gallery').html(err.message);
	});


	/* If files are uploaded successfully */
	GalleryUploader.bind('FileUploaded', function ( up, file, ajax_response ) {
		var response = $.parseJSON( ajax_response.response );
		if ( response.success ) {
			var load_gallery = wp.template( 'load-gallery' );
			var _thumb	= load_gallery(response);
			jQuery("#thumb-" + file.id).html(_thumb);
			jQuery('.doc-user-gallery .tg-img-hover a').unbind('click');
			//bindThumbnailEvents();  // bind click event with newly added gallery thumb
		} else {
			// log response object
			console.log ( response );
		}
	});

	//Delete Gallery Image
	jQuery(document).on('click','.doc-user-gallery .tg-img-hover a',function(e){
		e.preventDefault();
		var $this 	= jQuery(this);
		$this.parents('.gallery-item').remove();
	});

	//Update User Password
	jQuery('.form-resetpassword').on('click','.do-change-password',function(e){
		e.preventDefault();
		var $this 	= jQuery(this);

		var dataString = jQuery('.form-resetpassword').serialize()+'&action=docdir_change_user_password';
		jQuery('body').append(loder_html);
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: dataString,
			dataType:"json",
			success: function(response) {
				jQuery('body').find('.docdirect-site-wrap').remove();
				if( response.type == 'error' ) {
					jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 7000});
				} else{
					$this.parents('.tg-docimg').find('img').attr('src', response.avatar);
					jQuery('.tg-docprofile-img').find('img').attr('src', response.avatar);
					$this.parents('.tg-docimg').find('.del-avatar').hide();
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 7000});
				}
			}
		});
		return false;
	});

	//Delete Avatar
	jQuery('.tg-docimg').on('click','.del-avatar',function(e){
		e.preventDefault();
		var $this 	= jQuery(this);
		var dataString = 'action=docdir_delete_avatar';
		jQuery('body').append(loder_html);
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
					$this.parents('.tg-docimg').find('img').attr('src', response.avatar);
					jQuery('.tg-docprofile-img').find('img').attr('src', response.avatar);
					$this.parents('.tg-docimg').find('.del-avatar').hide();
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
				}
			}
		});
		return false;
	});

	//Delete banner
	jQuery('.tg-docimg').on('click','.del-banner',function(e){
		e.preventDefault();
		var _this 	= jQuery(this);
		var dataString = 'action=docdir_delete_user_banner';
		jQuery('body').append(loder_html);
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
					_this.parents('.tg-docimg').find('.user-banner img').attr('src', response.avatar);
					_this.parents('.tg-docimg').find('.del-banner').hide();
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
				}
			}
		});
		return false;
	});

	//Delete Email Logo
	jQuery('.tg-docimg').on('click','.del-email',function(e){
		e.preventDefault();
		var $this 	= jQuery(this);
		var dataString = 'action=docdir_delete_email_logo';
		jQuery('body').append(loder_html);
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
					$this.parents('.tg-docimg').find('img').attr('src', response.avatar);
					$this.parents('.tg-docimg').find('.del-email').hide();
					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000});
				}
			}
		});
		return false;
	});


	//Add Awards
	jQuery(document).on('click','.add-new-awards',function(){
		var load_awards = wp.template( 'load-awards' );
		var counter	= jQuery( '.awards_wrap > tbody' ).length;
		var load_awards	= load_awards(counter);
		jQuery( '.awards_wrap' ).append(load_awards);

		//init date
		//jQuery('.award_datepicker').datetimepicker({
		//  format:'Y-m-d',
		//  timepicker:false
		//});
	});;

	//init date
	jQuery('.award_datepicker').datetimepicker({
	  format:'Y-m-d',
	  timepicker:false
	});

	//Delete Awards
	jQuery(document).on('click','.award-action .delete-me',function(){
		var _this	= jQuery(this);
		$.confirm({
			'title': scripts_vars.delete_award,
			'message': scripts_vars.delete_award_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						_this.parents('.awards_item').remove();
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

	//Edit Awards
	jQuery(document).on('click','.award-action .edit-me',function(){
		jQuery('.award-data').hide();
		jQuery(this).parents('.awards_item').find('.award-data').toggle();
	});


	//Add Educations
	jQuery(document).on('click','.add-new-educations',function(){
		var load_educations = wp.template( 'load-educations' );
		var counter	= jQuery( '.educations_wrap > tbody' ).length;
		var load_educations	= load_educations(counter);
		jQuery( '.educations_wrap' ).append(load_educations);

		jQuery('.edu_start_date_'+counter).datetimepicker({
		   format:'Y-m-d',
		  onShow:function( ct ){
		   this.setOptions({
			maxDate:jQuery('.edu_end_date_'+counter).val()?jQuery('.edu_end_date_'+counter).val():false
		   })
		  },
		  timepicker:false
		 });
		 jQuery('.edu_end_date_'+counter).datetimepicker({
		  format:'Y-m-d',
		  onShow:function( ct ){
		   this.setOptions({
			minDate:jQuery('.edu_start_date_'+counter).val()?jQuery('.edu_start_date_'+counter).val():false
		   })
		  },
		  timepicker:false
		 });

	});

	//Delete Education
	jQuery(document).on('click','.education-action .delete-me',function(){
		var _this	= jQuery(this);
		$.confirm({
			'title': scripts_vars.delete_education,
			'message': scripts_vars.delete_education_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						_this.parents('.educations_item').remove();
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

	//Add Experience
	jQuery(document).on('click','.add-new-experiences',function(){
		var load_educations = wp.template( 'load-experiences' );
		var counter	= jQuery( '.experiences_wrap > tbody' ).length;
		var load_educations	= load_educations(counter);
		jQuery( '.experiences_wrap' ).append(load_educations);

		jQuery('.exp_start_date_'+counter).datetimepicker({
		   format:'Y-m-d',
		  onShow:function( ct ){
		   this.setOptions({
			maxDate:jQuery('.exp_end_date_'+counter).val()?jQuery('.exp_end_date_'+counter).val():false
		   })
		  },
		  timepicker:false
		 });
		 jQuery('.exp_end_date_'+counter).datetimepicker({
		  format:'Y-m-d',
		  onShow:function( ct ){
		   this.setOptions({
			minDate:jQuery('.exp_start_date_'+counter).val()?jQuery('.exp_start_date_'+counter).val():false
		   })
		  },
		  timepicker:false
		 });

	});

	//Delete Experience
	jQuery(document).on('click','.experience-action .delete-me',function(){
		var _this	= jQuery(this);
		$.confirm({
			'title': scripts_vars.delete_experience,
			'message': scripts_vars.delete_experience_message,
			'buttons': {
				'Yes': {
					'class': 'blue',
					'action': function () {
						_this.parents('.experiences_item').remove();
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

	//Awards Sortable
	jQuery( ".awards_wrap" ).sortable({
		cursor: "move"
	});

	//Education Sortable
	jQuery( ".educations_wrap" ).sortable({
		cursor: "move"
	});

	//Experience Sortable
	jQuery( ".experiences_wrap" ).sortable({
		cursor: "move"
	});

	//Edit Education
	jQuery(document).on('click','.education-action .edit-me',function(){
		jQuery('.education-data').hide();
		jQuery(this).parents('tr').next('tr').find('.education-data').toggle();
	});

	//Edit Experience
	jQuery(document).on('click','.experience-action .edit-me',function(){
		jQuery('.experience-data').hide();
		jQuery(this).parents('tr').next('tr').find('.experience-data').toggle();
	});


	//Do Process Account Settings
	jQuery(document).on('click','.process-account-settings',function(e){
		e.preventDefault();

		tinyMCE.triggerSave();
		var $this 	= jQuery(this);
		var serialize_data	= jQuery('.do-account-setitngs').serialize();
		var dataString = serialize_data+'&action=docdirect_account_settings';

		jQuery('body').append(loder_html);
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
					if( response.education ){
						var append_educations = wp.template( 'append-educations' );
						var load_educations	= append_educations(response.education);
						jQuery( '.educations_wrap' ).html(load_educations);
					}
					if( response.awards ){
						var append_awards = wp.template( 'append-awards' );
						var append_awards	= append_awards(response.awards);
						jQuery( '.awards_wrap' ).html(append_awards);
					}

					if( response.experience ){
						var append_experiences = wp.template( 'append-experiences' );
						var load_experiences	= append_experiences(response.experience);
						jQuery( '.experiences_wrap' ).html(load_experiences);
					}

					jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 5000,position: 'top-right',});
				}
			}
		});
		return false;
	});


	_.toDate = function(epoch, format, locale) {
		var date = new Date(epoch),
			format = format || 'dd/mm/YY',
			locale = locale || 'en'
			dow = {};

		dow.en = [
			'Sunday',
			'Monday',
			'Tuesday',
			'Wednesday',
			'Thursday',
			'Friday',
			'Saturday'
		];

		var formatted = format
			.replace('D', dow[locale][date.getDay()])
			.replace('dd', ("0" + date.getDate()).slice(-2))
			.replace('mm', ("0" + (date.getMonth() + 1)).slice(-2))
			.replace('yyyy', date.getFullYear())
			.replace('yy', (''+date.getFullYear()).slice(-2))
			.replace('hh', date.getHours())
			.replace('mn', date.getMinutes());

		return formatted;
	}

	/* ---------------------------------------
     Login Ajax
     --------------------------------------- */
	jQuery('.form-deleteaccount').on('click', '.do-process-account', function (event) {
		event.preventDefault();
		var $this	= jQuery(this);
		var process_type	= $this.data('action');

		if( process_type == 'deleteme' ){
			var message	= delete_message;
			var title	= delete_title;
		} else if( process_type == 'deactivateme' ){
			var message	= deactivate;
			var title	= deactivate_title;
		} else{
			var message	= delete_message;
			var title	= delete_title;
		}


		if( process_type == 'activateme' ){
			jQuery('body').append(loder_html);
			jQuery.ajax({
				type: "POST",
				url: scripts_vars.ajaxurl,
				data: jQuery('.form-deleteaccount').serialize() +'&process='+process_type+'&action=docdirect_process_acount',
				dataType: "json",
				success: function (response) {
					jQuery('body').find('.docdirect-site-wrap').remove();
					jQuery('.login-message').show();
					if (response.type == 'success') {
						jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 7000,position: 'top-right',});
						window.location.reload();
					} else {
						jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 7000});
					}
				}
		   });
		} else {
			$.confirm({
				'title': title,
				'message': message,
				'buttons': {
					'Yes': {
						'class': 'blue',
						'action': function () {
							jQuery('body').append(loder_html);
							jQuery.ajax({
								type: "POST",
								url: scripts_vars.ajaxurl,
								data: jQuery('.form-deleteaccount').serialize() +'&process='+process_type+'&action=docdirect_process_acount',
								dataType: "json",
								success: function (response) {
									jQuery('body').find('.docdirect-site-wrap').remove();
									jQuery('.login-message').show();
									if (response.type == 'success') {
										jQuery.sticky(response.message, {classList: 'success', speed: 200, autoclose: 7000,position: 'top-right',});
										window.location.reload();
									} else {
										jQuery.sticky(response.message, {classList: 'important', speed: 200, autoclose: 7000});
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
		}
	});

	/* ---------------------------------------
     Renew Package
     --------------------------------------- */
	jQuery(document).on('click', '.do-process-payment', function (event) {
		event.preventDefault();
		var $this	= jQuery(this);
		jQuery('body').append(loder_html);
		jQuery('.login-message').html('').hide();
		jQuery.ajax({
			type: "POST",
			url: scripts_vars.ajaxurl,
			data: jQuery('.renew-package').serialize() + '&action=docdirect_do_process_subscription',
			dataType: "json",
			success: function (response) {
				jQuery('body').find('.docdirect-site-wrap').remove();
				if (response.type == 'success') {
					if ( response.payment_type == 'bank' ) {
						jQuery('.gateways-settings').html(response.form_data);
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
										'action': 'docdirect_complete_stripe_payment',
										'first_name': obj.first_name,
										'last_name': obj.last_name,
										'username': obj.username,
										'email': obj.email,
										'useraddress': obj.useraddress,
										'order_no': obj.order_no,
										'user_identity': obj.user_identity,
										'package_id': obj.package_id,
										'package_name': obj.package_name,
										'gateway': obj.gateway,
										'type': obj.type,
										'payment_type': obj.payment_type,
										'process': '',
										'name': obj.name,
										'amount': obj.amount,
										'total_amount': obj.total_amount,
										'token': token,
									},
									dataType: "json",
									success: function(response) {
										jQuery('body').find('.docdirect-site-wrap').remove();
										handler.close();
										if (response.type == 'success') {
											jQuery('.notification_wrap').find('.notification_text').html(response.message);
											jQuery('.notification_wrap').find('.notification_text').addClass('alert alert-success').show();

										} else if (response.type == 'error') {
											jQuery('.notification_wrap').find('.notification_text').addClass('alert alert-danger').show();
                    						jQuery('.notification_wrap').find('.notification_text').html(response.message);
										}
									}
								});
							}
						});

						handler.open({
						  name: obj.name,
						  description: obj.description,
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


						/*jQuery(window).on('popstate', function() {
							handler.close();
						});*/
					} else{
						jQuery('body').append( response.form_data );
					}

				} else {

				}
			}
	   });
	});

	/* ---------------------------------------
    Language Choosen
     --------------------------------------- */
	var config = {
      '.chosen-select'           : {},
      '.chosen-select-deselect'  : {allow_single_deselect:false},
      '.chosen-select-no-single' : {disable_search_threshold:10},
      '.chosen-select-no-results': {no_results_text:'Oops, nothing found!'},
      '.chosen-select-width'     : {width:"95%"}
    }

    for (var selector in config) {
      $(selector).chosen(config[selector]);
    }

	/* ---------------------------------------
    Language Search
     --------------------------------------- */
	var config = {
      '.language-selelct'           : {},
      '.chosen-select-deselect'  : {allow_single_deselect:true},
      '.chosen-select-no-single' : {disable_search_threshold:10},
      '.chosen-select-no-results': {no_results_text:'Oops, nothing found!'},
      '.chosen-select-width'     : {width:"95%"}
    }

    for (var selector in config) {
      $(selector).chosen(config[selector]);
    }

	/* ---------------------------------------
    	Location Choosen
     --------------------------------------- */
	 var config = {
      '.locations-select'           : {},
      '.chosen-select-deselect'  : {allow_single_deselect:false},
      '.chosen-select-no-single' : {disable_search_threshold:10},
      '.chosen-select-no-results': {no_results_text:'Oops, nothing found!'},
      '.chosen-select-width'     : {width:"95%"}
    }

	for (var selector in config) {
      $(selector).chosen(config[selector]);
    }

	//Package active
	jQuery('.packages-payments').on('click','.selected-package',function(){
		jQuery(this).parents('.packages-payments').find('.tg-packages').removeClass('active');
		jQuery(this).parents('.tg-packages').addClass('active');
	});

	//User active
	jQuery(document).on('click','.active-user-type',function(){
		jQuery(this).parents('.form-group').find('.tg-packages').removeClass('active');
		jQuery(this).addClass('active');

		jQuery('.user-types').show();
		if( jQuery(this).hasClass('visitor-type') ){
			jQuery('.user-types').hide();
		}
	});

	//Emails Tabs
	jQuery(document).on('click','.doc-tab-link',function(){

		if( jQuery(this).hasClass('active') ){
			jQuery(this).removeClass('active');
		} else{
			jQuery(this).addClass('active');
		}

		jQuery(this).parents('.booking-email-wrap').find('.tab-data').slideToggle('slow');
	});

	//Update Email settings
	jQuery(document).on('click','.update-email-settings',function(e){
		e.preventDefault();
		tinyMCE.triggerSave();
		var $this 	= jQuery(this);
		var serialize_data	= jQuery('.email-settings').serialize();
		var dataString = serialize_data+'&action=docdir_update_booking_settings';
		jQuery('body').append(loder_html);
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
				}
			}
		});
		return false;
	});

});

/* ---------------------------------------
 Confirm Box
 --------------------------------------- */
(function ($) {

		$.confirm = function (params) {

			if ($('#confirmOverlay').length) {
				// A confirm is already shown on the page:
				return false;
			}

			var buttonHTML = '';
			$.each(params.buttons, function (name, obj) {

				// Generating the markup for the buttons:

				buttonHTML += '<a href="#" class="button ' + obj['class'] + '">' + name + '<span></span></a>';

				if (!obj.action) {
					obj.action = function () {
					};
				}
			});

			var markup = [
				'<div id="confirmOverlay">',
				'<div id="confirmBox">',
				'<h1>', params.title, '</h1>',
				'<p>', params.message, '</p>',
				'<div id="confirmButtons">',
				buttonHTML,
				'</div></div></div>'
			].join('');

			$(markup).hide().appendTo('body').fadeIn();

			var buttons = $('#confirmBox .button'),
					i = 0;

			$.each(params.buttons, function (name, obj) {
				buttons.eq(i++).click(function () {

					// Calling the action attribute when a
					// click occurs, and hiding the confirm.

					obj.action();
					$.confirm.hide();
					return false;
				});
			});
		}

		$.confirm.hide = function () {
			$('#confirmOverlay').fadeOut(function () {
				$(this).remove();
			});
		}

})(jQuery);