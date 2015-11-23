function EditProd(){
	
	$( ".add-to-list" ).unbind( "click" );
	
	$( ".add-to-list" ).click(function(e) {
				
		e.preventDefault();
		var ProdId 	= $(this).attr('data-prod'); 
		var UsrId 	= $(this).attr('data-usr');
		
		var CurrentElem = $(this);
		
		$.get("pages/template.add_to_list.php", {art_id: ProdId, usr_id: UsrId }).done(function(data) {
		
			bootbox.confirm(data, function(result) {
			        if(result)
			            $('#add_to_list').submit();
			            //$('#'+ProdId).hide();
			});
			
			 
			
		});
		
	});
}


function FollowProd(){
	
	$( ".add-product" ).unbind( "click" );
	
	$(".add-product").click(function(e) {
		
		e.preventDefault();
		
		var IdUsr 	= $(this).attr('data-usr');
		var IdPro 	= $(this).attr('data-prod');
		var Cas		= $(this).attr('data-cas');
		
		var CurrentElem = $(this); 
				
		$.post("core/process/aru.post.php", { t: 'quick_add_prod', cas : Cas, usr_id : IdUsr, art_id : IdPro } ).done(function(data) {
						
			$('#'+IdPro).hide(); 
			$(CurrentElem).unbind('mouseenter mouseleave click'); 
			
		});
		
	
	});
	
}

function UnFollowProd(){
	
	$(".remove-product").unbind( "click" );
	
	$(".remove-product").click(function(e) {
		
		e.preventDefault();
		
		var IdUsr 	= $(this).attr('data-usr');
		var IdPro 	= $(this).attr('data-prod');
		
		var CurrentElem = $(this);
		
		bootbox.confirm("Souhaitez vous supprimer ce produit ?", function(result) {
			
			if(result == true){
				
				$.post("core/process/aru.post.php", { t: 'quick_remove_prod', usr_id : IdUsr, art_id : IdPro } ).done(function(data) {
					
					var retour = data.replace(/\s+/, "");
					
					if(retour == 'false'){
						
						bootbox.alert("Impossible de supprimer ce produit", function() {});
						
					}
					else{
					
						$('#'+IdPro).hide(); 
					
					}
					
					 
					
				});
			
			}
		
		});
		
		return false;
		
	
	});
	
}

function RemoveFromList(){
	
	$( ".remove-list > span" ).unbind( "click" );
	
	$(".remove-list > span").click(function(e) {
				
		e.preventDefault();
				
		var IdList 	= $(this).parent().attr('data-list');
		var IdPro 	= $(this).parent().attr('data-prod');
		
		var CurrentElem = $(this);
		
				
		$.post("core/process/aru.post.php", { t: 'quick_remove_list', lst_id : IdList, art_id : IdPro } ).done(function(data) {
			
			var retour = data.replace(/\s+/, "");
			
			if(retour == 'false'){
				
				bootbox.alert("Impossible de supprimer ce produit", function() {});
				$(CurrentElem).unbind('mouseenter mouseleave click');
				
			}
			else{
			
				$('#'+IdPro).hide(); 
				$(CurrentElem).unbind('mouseenter mouseleave click');
			
			}
						 
			
		});
	
	
	});
	
}



function PriceFollow(){
	
	$( ".price-follow > span" ).unbind( "click" );
	
	$(".price-follow > span").click(function(e) {
				
		e.preventDefault();
		
		var IdUsr 	= $(this).parent().attr('data-usr');
		var IdPro 	= $(this).parent().attr('data-prod');
		var Datas 	= $(this); 
		
		$.post("core/process/aru.post.php", { t: 'price_follow', usr_id : IdUsr, art_id : IdPro } ).done(function(data) {

			$(Datas).parent().removeClass('flw-price').addClass('flwed-price');
			$(Datas).parent().removeClass('price-follow').addClass('price-unfollow');
			$(Datas).parent().parent().addClass('followed-price');
			
			$(Datas).unbind('mouseenter mouseleave click');
			PriceUnfollow();
			
		}); 
		
	
	}); 
	
}


function PriceUnfollow(){
	
	$( ".price-unfollow > span" ).unbind( "click" );
	
	$(".price-unfollow > span").click(function(e) {
						
		e.preventDefault();
		
		var IdUsr 	= $(this).parent().attr('data-usr');
		var IdPro 	= $(this).parent().attr('data-prod');
		var Datas 	= $(this);
		
		$.post("core/process/aru.post.php", { t: 'price_unfollow', usr_id : IdUsr, art_id : IdPro } ).done(function(data) {
			
			$(Datas).parent().removeClass('flwed-price').addClass('flw-price');			
			$(Datas).parent().removeClass('price-unfollow').addClass('price-follow');
			$(Datas).parent().parent().removeClass('followed-price');
			
			$(Datas).unbind('mouseenter mouseleave click');
			PriceFollow();
			
		});
	
	});
	
}


function Follow(){
	
	$( ".follow" ).unbind( "click" );
	
	$(".follow").click(function(e) {
		
		e.preventDefault();
		
		var SourceId	= $(this).attr('data-source');
		var CibleId		= $(this).attr('data-cible');
		var TypeOf 		= $(this).attr('data-type');
		
		var CurrentElem = $(this);
		
		$.post("core/process/aru.post.php", { t: 'follow', flw_source : SourceId, flw_cible : CibleId, flw_type : TypeOf } ).done(function(data) {
			
			$(CurrentElem).html('Suivi'); 
			$(CurrentElem).removeClass('follow').addClass('unfollow');
			
			$(CurrentElem).unbind('mouseenter mouseleave click');
			
			Unfollow(); 
			
		});
		
	});	 
	
	
}


function Unfollow(){
	
	$( ".unfollow" ).unbind( "click" );
	
	$(".unfollow").click(function(e) {
		
		e.preventDefault();
		
		var SourceId	= $(this).attr('data-source');
		var CibleId		= $(this).attr('data-cible');
		var TypeOf 		= $(this).attr('data-type');
		
		var CurrentElem = $(this);
		
		$.post("core/process/aru.post.php", { t: 'unfollow', flw_source : SourceId, flw_cible : CibleId, flw_type : TypeOf } ).done(function(data) {
			
			$(CurrentElem).html("Suivre"); 
			$(CurrentElem).removeClass('unfollow').addClass('follow');
			
			$(CurrentElem).unbind('mouseenter mouseleave click');
			
			Follow(); 
			
		});
		
	});	
	
	 
		
}


function ShareBox(){
	
	$( ".share-box" ).unbind( "click" );
	
	// Share box 
	$(".share-box").click(function(e) {
		
		e.preventDefault();
		
		var Id 			= $(this).attr('data-rel');
		var ShareType 	= $(this).attr('data-share');
		var ShareTxt 	= $(this).attr('data-txt');
		var ShareURL 	= $(this).attr('data-url'); 
		
		var CurrentElem = $(this);
		
		$.get("pages/template.share.php", { share: ShareType, id: Id, share_txt : ShareTxt, share_url : ShareURL }).done(function(data) {
		
			bootbox.alert(data, function() {}).find("div.modal-footer").addClass("hidden");
			
			$( "a.sharer" ).unbind( "click" );
			
			$('a.sharer').click(function(e){
				
				e.preventDefault();
				
				window.open(this.href, 'Neeed Sharer', config='height=320, width=600, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, directories=no, status=no, top=300, left=500 ');
				return false;
				
			});
			
		
		});
		
				
	});

	
}

function add_hashtag(){
	
	// Ajouter un hashtag
	// -------------------
	
	
	$('.add-hashtag').click(function(e){
		
		e.preventDefault();
		var ArtId = $(this).attr('data-rel');
		
		$.get("pages/template.add_hashtag.php", { art_id: ArtId }, function( data ) {
			
			
			bootbox.confirm(data, function(result) {
			        if(result)
			            $('#add_hashtag').submit();
			});
			
			
						
		});		
				
	});
	
}


function remove_hashtag(){
	
	
	$(".icn-remove").click(function(e) {
		
		e.preventDefault();
		
		var ArtId = $(this).attr('data-rel');
		var Elem  = $(this);
		var Pare  = $(this).parent().parent();     
				
		$.post("core/process/aru.post.php", { t : 'remove_hashtag', art_id: ArtId }, function( data ) {
			
			$(Elem).parent().remove(); 
			$(Pare).append('<a href="#" data-rel="' + ArtId + '" class="add-hashtag">Ajouter un hashtag</a>'); 
			
			add_hashtag();
						
		});
		
		
	});	
	
}


function show_delete_hashtag(){
	
	$('.show-remove-hash').hover(function(e){
		$(this).children().show();
	}, function(e) {
    	$(this).children().hide();
	}); 
	
}


function Product_init(){
	
	EditProd();
	FollowProd();
	UnFollowProd();
	
	RemoveFromList();
	
	PriceFollow();
	PriceUnfollow();
	
	ShareBox();
	
	add_hashtag(); 
	remove_hashtag();
	show_delete_hashtag();
	
}



$(document).ready(function() {
	
	
	Follow();
	Unfollow(); 
	
	Product_init();


	// Login Box
	$(".login-box").click(function(e) {
		
		e.preventDefault();
		
		$.get("core/static/login.box.static.php").done(function(data) {
		
			bootbox.alert(data, function(){}).find("div.modal-footer").addClass("hidden");
		
		});
		
				
	});
	
	
	
	
	
	
				
	
	// NEW LIST CREATION 
	// -----------------
	
	
	$('.add-list').click(function(e){
		
		e.preventDefault();
		
		$.get("pages/template.add_list.php", function( data ) {
			
			
			bootbox.confirm(data, function(result) {
			        if(result)
			            $('#add_list').submit();
			});
			
			
						
		});		
				
	}); 
	

	
	$('.edit-list').click(function(e){
		
		e.preventDefault();
		var ListId = $(this).attr('data-rel'); 
				
		$.get("pages/template.edit_list.php", { lst_id: ListId } , function( data ) {
						
			
			bootbox.dialog({
				message: data,
				title: "Edition liste",
				buttons: {
					success: {
						label: "Sauver",
						className: "btn-default",
						callback: function() {
							$('#add_list').submit(); 
						}
					},
						danger: {
						label: "Supprimer!",
						className: "btn-danger pull-left",
						callback: function() {
							
							$.post("core/process/aru.post.php", {t: 'delete_list', lst_id: ListId}, function(data){ 
								location.reload();
							});
							
						}
					}
					
				}
			});			
			
			
						
		});
	
	});
	
	
	$('.delete-account').click(function(e){
		
		e.preventDefault();
		
		bootbox.dialog({
			message: "Etes vous sur de vouloir supprimer votre compte ?",
			title: "Suppression de votre compte",
			buttons: {
				success: {
					label: "Non non non non non !",
					className: "btn-default",
					callback: function() {
						
					}
				},
					danger: {
					label: "Supprimer!",
					className: "btn-danger pull-left",
					callback: function() {
						
						$.post("core/process/aru.post.php", {t: 'delete_account'}, function(data){ 
							window.location = '/auth/logout'; 							 
						});
						
					}
				}
				
			}
		});	
			
		
	});
	
	
	$('.neeed-js-snippet')
	
	.bind('dragstart',function( event ){
		$( this ).children().html('♥ I Neeed');
	})

	.bind('drag',function( event ){
		$( this ).children().html('Déposez moi dans votre barre de favoris');
	})
	
	.bind('dragend',function( event ){
    	$( this ).children().html('Ajouter à Neeed');
    });

	
	


	

});



(function(document,navigator,standalone) {
    // prevents links from apps from oppening in mobile safari
    // this javascript must be the first script in your <head>
    if ((standalone in navigator) && navigator[standalone]) {
        var curnode, location=document.location, stop=/^(a|html)$/i;
        document.addEventListener('click', function(e) {
            curnode=e.target;
            while (!(stop).test(curnode.nodeName)) {
                curnode=curnode.parentNode;
            }
            // Condidions to do this only on links to your own app
            // if you want all links, use if('href' in curnode) instead.
            if(
                'href' in curnode && // is a link
                (chref=curnode.href).replace(location.href,'').indexOf('#') && // is not an anchor
                (   !(/^[a-z\+\.\-]+:/i).test(chref) ||                       // either does not have a proper scheme (relative links)
                    chref.indexOf(location.protocol+'//'+location.host)===0 ) // or is in the same protocol and domain
            ) {
                e.preventDefault();
                location.href = curnode.href;
            }
        },false);
    }
})(document,window.navigator,'standalone');



