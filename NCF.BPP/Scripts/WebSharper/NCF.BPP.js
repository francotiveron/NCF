(function()
{
 "use strict";
 var Global,NCF,BPP,Client,WebSharper,Concurrency,powerbi,UI,Next,Doc,AttrProxy;
 Global=window;
 NCF=Global.NCF=Global.NCF||{};
 BPP=NCF.BPP=NCF.BPP||{};
 Client=BPP.Client=BPP.Client||{};
 WebSharper=Global.WebSharper;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 powerbi=Global.powerbi;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Client.Main=function(report)
 {
  var powerbi_target,powerbi_settings,r,powerbi_conf,r$1;
  function start_embed()
  {
   var b;
   Concurrency.Start((b=null,Concurrency.Delay(function()
   {
    powerbi.embed(powerbi_target.elt,powerbi_conf);
    return Concurrency.Zero();
   })),null);
  }
  powerbi_target=Doc.Element("div",[AttrProxy.Create("style","height:720px")],[]);
  powerbi_settings=(r={},r.filterPaneEnabled=true,r);
  powerbi_conf=(r$1={},r$1.tokenType=1,r$1.accessToken=report.AccessToken,r$1.type="report",r$1.embedUrl=report.EmbedUrl,r$1.id=report.ReportId,r$1.settings=powerbi_settings,r$1);
  return Doc.Element("div",[],[Doc.Button("Start embed powerbi",[AttrProxy.Create("class","btn btn-primary")],function()
  {
   start_embed();
  }),powerbi_target]);
 };
}());
