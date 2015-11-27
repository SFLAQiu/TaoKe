
!function ($) {
	$(document).ready(function () {
	  
		function recalcHeight() {
			$('#lists > div').height(parseInt($(window).height()));
			$('.scrooly').scrooly('resize');
		}
		
		// menu langue
		var menulangOPEN = false;
		$(document).click(function() {
			if (menulangOPEN)
				$('#langues').removeClass('open');
		});
		$('#langues > span').click(function(e) {
			$('#langues').toggleClass('open');
			menulangOPEN = $('#langues').hasClass('open');
			return false;
		});
		
		// menu top: suppr du "active" au rollover
		$('#menu-top .nav > li:not(.active) > a').hover(function() {
			$('#menu-top .nav > li.active').addClass('inactive');
		}, function() {
			$('#menu-top .nav > li').removeClass('inactive');
		});
		// ...pareil pour les listes dans #filters_select
		$('#listes > li > a').hover(function() {
			$('#listes').addClass('inactive');
		}, function() {
			$('#listes').removeClass('inactive');
		});
		
		
		// tooltip
		$('a[data-toggle=tooltip], span[data-toggle=tooltip]').tooltip();
		
		// rollover sur bt-like
		$(document).on('mouseenter', '.bt-like', function() {
			$(this).parent().addClass('hover');
		}).on('mouseleave', '.like', function() {
			$(this).removeClass('hover');
		});
		
		
		


		// header3 fixed ou pas?
		$(window).on('load scroll', function() {
		    var rupture = $("#header1").height() + $("#header2").height() + $("#header3").height();
			var scrollTop = $(window).scrollTop();
			if( scrollTop>rupture )
				$('body:not(.headerfix)').addClass('headerfix');
			else {
				$('body.headerfix').removeClass('headerfix');
			}
			if (scrollTop > rupture)
			    $('#lists > div').css("top", scrollTop - rupture + $("#header3").height() - 35);
			else
			    $('#lists > div').css("top", 0);
			
		});



		// filtres: masquer si on sort du header3
		$('#header3').on('mouseleave', function() {
			if( $('#filters:visible').length )
				$('#bt-trier').trigger('click');
		});



		// listes
		$('.open-list').on('click', function() {
			
			// Trick to remove filters if block is openned 
			
			$('#filters').hide();
			
			if($('.fleche-filtre').hasClass('glyphicon-chevron-up')){
				
				$('.fleche-filtre').addClass('glyphicon-chevron-down');
				$('.fleche-filtre').removeClass('glyphicon-chevron-up');
				
			}
			
			
			if($('.fleche-liste').hasClass('glyphicon-chevron-up')){
				
				$('.fleche-liste').addClass('glyphicon-chevron-down');
				$('.fleche-liste').removeClass('glyphicon-chevron-up');
				
			}
			else{
				
				$('.fleche-liste').addClass('glyphicon-chevron-up');
				$('.fleche-liste').removeClass('glyphicon-chevron-down');
				
			}
			
			
			
			if( $('#filters:visible').length )
				$('#bt-trier-mask').trigger('click');
			
			/*var scrollTop = $(window).scrollTop();
			if( $('#lists:hidden').length && scrollTop>110)
				$("html, body").animate({scrollTop: '110px'});*/
			
			if( $('#lists:visible').length ) {
				$('#lists').hide();
				$('#neeed').removeClass('col-xs-9').addClass('col-xs-12');
				$('.big-liste > .prod').removeClass('col-sm-4').addClass('col-sm-3');
				
				$('.flw-list').removeClass('col-md-9').addClass('col-md-12');
				$('.flw-list').removeClass('col-md-offset-3');
				$('.flw-list > .userlist_box').removeClass('col-sm-4').addClass('col-sm-3');
				
				
			} else {
				$('#lists').show();
				$('#neeed').removeClass('col-xs-12').addClass('col-xs-9');
				$('.big-liste > .prod').removeClass('col-sm-3').addClass('col-sm-4');
				
				$('.flw-list').removeClass('col-md-12').addClass('col-md-9');
				$('.flw-list').addClass('col-md-offset-3');
				$('.flw-list > .userlist_box').removeClass('col-sm-3').addClass('col-sm-4');
				
				recalcHeight();
				$('.scrooly').scrooly('resize');
			}

			return false;
		});
		

		// filtres
		$('#bt-trier, #bt-trier-mask, #listes').on('click', function() {
						
			if( $('#lists:visible').length )
				$('#bt-lists > a').trigger('click');

			/*var scrollTop = $(window).scrollTop();
			if( $('#filters:hidden').length && scrollTop>110 )
				$("html, body").animate({scrollTop: '110px'});*/
			
			$('#filters').stop().slideToggle(310, function() {
				if( $('#filters:visible').length )
					$('#bt-trier-mask:hidden').fadeIn();
				else {
					$('#bt-trier-mask:visible').fadeOut();
				}
			});
			
			$('#lists').hide();
			if($('.fleche-liste').hasClass('glyphicon-chevron-up')){
				
				$('.fleche-liste').addClass('glyphicon-chevron-down');
				$('.fleche-liste').removeClass('glyphicon-chevron-up');
				
			}
			
			if($('.fleche-filtre').hasClass('glyphicon-chevron-up')){
				
				$('.fleche-filtre').addClass('glyphicon-chevron-down');
				$('.fleche-filtre').removeClass('glyphicon-chevron-up');
				
			}
			else{
				
				$('.fleche-filtre').addClass('glyphicon-chevron-up');
				$('.fleche-filtre').removeClass('glyphicon-chevron-down');
				
			}
			
			return false;
		});
		
		
		
		$('.scrooly').scrooly({
			step: 15,
			opacity: 0.5,
			speed: 200
		});

		$(window).on('resize', function() {
			$('#filters:visible').hide();
			recalcHeight();
		});
		recalcHeight();
		
		/** drag n drop **/
		if ($('body').hasClass('is-logged') && $('body').hasClass('is-own-products')) {
			var target = {
					list: null,
					product: null
				},
				$lists = $('#lists'),
				$phantom = null,
				$master = null;
	
			$(document).on('mouseenter', '#neeed .big-liste .prod', function() {
				if ($lists.is(':visible') && $('li[data-list]', $lists).length)
					$(this).addClass('hovered');
			}).on('mouseleave', '#neeed .big-liste .prod', function() {
				$(this).removeClass('hovered');
			}).on('mousedown', '#neeed .big-liste .prod', function() {
				if ($lists.is(':visible') && $('li[data-list]', $lists).length) {
					$master = $(this);
					target.product = $master.removeClass('hovered').addClass('dragged').data('product');
					return false;
				}
			});
	
			$(document).on({
				mouseup: function() {
					if (null !== $phantom) {
						$phantom.remove();
						if (null !== target.list) {
							var IdArt 	= $master.attr('data-product'),
								IdList 	= target.list;
							$.ajax({
								url: 'core/process/aru.post.php',
								type: "POST",
								data: { art_id: IdArt, lst_id: IdList, t: "quick_addto_list" },
								success: function() {
									// Yeah !!!
						
									var $count = $('li[data-list=' + IdList + '] .badge.nb', $lists),
										count = parseInt($count.text()) + 1;
									$count.text(count);
									$master.remove();
						
								}
							});
						} else
							$master.fadeIn();
					}
					$('.hovered').removeClass('hovered');
					$('.dragged').removeClass('dragged');
					$('body').removeClass('boo');
					$phantom = null;
					$master = null;
					onMouseDown = false;
				},
				mousemove: function(e) {
					if (null !== $master) {
						if (null === $phantom) {
							$phantom = $('<div id="phantom" />').html('<img src="' + $('.prod-img img', $master).attr('src') + '" />');
							$('body').append($phantom).addClass('boo');
							$master.fadeOut();
						}
						else {
							$phantom.stop(true, true).animate({
								top: e.pageY + 5,
								left: e.pageX + 5
							}, 50);
						}
					}
				}
			});
	
			$('li[data-list]', $lists).on({
				mouseenter: function() {
					if (null !== $phantom)
						target.list = parseInt($(this).addClass('hovered').data('list'));
				},
				mouseleave: function() {
					if (null !== $phantom) {
						target.list = null;
						$(this).removeClass('hovered');
					}
				}
			});
		}
		
	});
}(window.jQuery);