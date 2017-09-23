(function()
{
 "use strict";
 var Global,NCF,BPP,Client,WebSharper,UI,Next,Doc,IntelliFactory,Runtime,Utils,Concurrency,Remoting,AjaxRemotingProvider;
 Global=window;
 NCF=Global.NCF=Global.NCF||{};
 BPP=NCF.BPP=NCF.BPP||{};
 Client=BPP.Client=BPP.Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Utils=WebSharper&&WebSharper.Utils;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Client.Main=function()
 {
  return Doc.Element("div",[],[]);
 };
 Client.init=function(a)
 {
  Global.jQuery("[data-reportId]").each(Client.f);
 };
 Client.f=function(a,e)
 {
  var $1,$2;
  $1=e.getAttribute("data-groupId");
  $2=e.getAttribute("data-reportId");
  e.textContent=(((Runtime.Curried3(function($3,$4,$5)
  {
   return $3(Utils.toSafe($4)+" - "+Utils.toSafe($5));
  }))(Global.id))($1))($2);
 };
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
  var r,r$1,html,view,powerbi_settings,powerbi_conf,$1,$2;
  html=Global.jQuery("#embedReportHtml").text();
  view=Global.open();
  view.document.write(html);
  powerbi_settings=(r={},r.filterPaneEnabled=true,r.navContentPaneEnabled=true,r);
  powerbi_conf=(r$1={},r$1.type="report",r$1.tokenType=1,r$1.accessToken=embedToken,r$1.embedUrl=embedUrl,r$1.id=reportId,r$1.permissions=7,r$1.settings=powerbi_settings,r$1);
  try
  {
   $1=(view.init("NCF.BPP - "+name,powerbi_conf),false);
  }
  catch(err)
  {
   $1=true;
  }
  $2=$1;
  while($2)
   {
    Client.sleep(1);
    try
    {
     $1=(view.init("NCF.BPP - "+name,powerbi_conf),false);
    }
    catch(err$1)
    {
     $1=true;
    }
    $2=$1;
   }
 };
 Client.sleep=function(ms)
 {
  Global.setTimeout(Global.ignore,ms);
 };
}());
