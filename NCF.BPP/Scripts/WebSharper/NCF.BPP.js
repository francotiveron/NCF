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
 Client.reportClicked=function(e,a)
 {
  var b;
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   var div,$1,rId,embedUrl;
   div=e.parentElement;
   $1=div.getAttribute("data-groupId");
   rId=div.getAttribute("data-reportId");
   embedUrl=div.getAttribute("data-embedUrl");
   Global.jQuery("*").css("cursor","progress");
   return Concurrency.Bind(Client.getEmbedToken($1,rId),function(a$1)
   {
    Global.jQuery("*").css("cursor","default");
    return a$1.$==1?(Global.alert(a$1.$0),Concurrency.Zero()):(Client.openReport(e.textContent,embedUrl,rId,a$1.$0),Concurrency.Zero());
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
 Client.openReport=function(name,embedUrl,reportId,embedToken)
 {
  var html;
  html=Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Global.jQuery("#embedReportHtml").text(),"${{rplcTitle}}","NCF.BPP - "+name),"${{rplcEmbedToken}}",embedToken),"${{rplcEmbedUrl}}",embedUrl),"${{rplcReportId}}",reportId);
  Global.open().document.write(html);
 };
}());
