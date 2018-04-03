(function()
{
 "use strict";
 var Global,NCF,BPP,Client,WebSharper,Concurrency,Remoting,AjaxRemotingProvider,Strings;
 Global=window;
 NCF=Global.NCF=Global.NCF||{};
 BPP=NCF.BPP=NCF.BPP||{};
 Client=BPP.Client=BPP.Client||{};
 WebSharper=Global.WebSharper;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Strings=WebSharper&&WebSharper.Strings;
 Client.pbiLinkClicked=function(e,a)
 {
  var b;
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   var div,$1,$2,$3,$4;
   div=e.parentElement;
   $1=div.getAttribute("data-pbiType");
   $2=div.getAttribute("data-groupId");
   $3=div.getAttribute("data-resourceId");
   $4=div.getAttribute("data-embedUrl");
   Global.jQuery("*").css("cursor","progress");
   return Concurrency.Bind(Client.getEmbedToken($2,$3),function(a$1)
   {
    Global.jQuery("*").css("cursor","default");
    return a$1.$==1?(Global.alert(a$1.$0),Concurrency.Zero()):(Client.openEmbedPage($1,e.textContent,$4,$3,a$1.$0),Concurrency.Zero());
   });
  })),null);
 };
 Client.getEmbedToken=function(gId,rId)
 {
  var b;
  b=null;
  return Concurrency.Delay(function()
  {
   return(new AjaxRemotingProvider.New()).Async("NCF.BPP:NCF.BPP.Server.getEmbedTokenAsync:-483179291",[gId,rId]);
  });
 };
 Client.openEmbedPage=function(pbiType,name,embedUrl,id,embedToken)
 {
  var html;
  html=Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Global.jQuery("#embedReportHtml").text(),"${{rplcType}}",pbiType),"${{rplcTitle}}","NCF.BPP - "+name),"${{rplcEmbedToken}}",embedToken),"${{rplcEmbedUrl}}",embedUrl),"${{rplcId}}",id);
  Global.open().document.write(html);
 };
}());
