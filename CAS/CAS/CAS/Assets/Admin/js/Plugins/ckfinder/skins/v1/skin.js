﻿/*
Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
For licensing, see license.txt or http://cksource.com/license/ckfinder
*/

CKFINDER.skins.add('v1',(function(){var a=['/Assets/Client/images/loaders/16x16.gif','/Assets/Client/images/loaders/32x32.gif','/Assets/Client/images/ckffolder.gif','/Assets/Client/images/ckffolderopened.gif'];if(CKFINDER.env.ie&&CKFINDER.env.version<7)a.push('/Assets/Client/images/sprites_ie6.png');return{preload:a,application:{css:['app.css']},fixMainContentWidth:1,fixMainContentWidthValue:-8,marginSidebarContainer:0,host:{intoHostPage:1,css:['host.css']}};})());(function(){CKFINDER.dialog?a():CKFINDER.on('dialogPluginReady',a);function a(){CKFINDER.dialog.on('resize',function(b){var c=b.data,d=c.width,e=c.height,f=c.dialog,g=f.parts.contents;if(c.skin!='v1')return;g.setStyles({width:d+'px',height:e+'px'});setTimeout(function(){var h=f.parts.dialog.getChild([0,0,0]),i=h.getChild(0),j=h.getChild(2);j.setStyle('width',i.$.offsetWidth+'px');j=h.getChild(7);j.setStyle('width',i.$.offsetWidth-28+'px');j=h.getChild(4);j.setStyle('height',i.$.offsetHeight-31-14+'px');j=h.getChild(5);j.setStyle('height',i.$.offsetHeight-31-14+'px');},100);});};})();
