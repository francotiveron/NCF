(function()
{
 "use strict";
 var Global,NCF,BPP,Client,WebSharper,UI,Next,Doc,AttrProxy;
 Global=window;
 NCF=Global.NCF=Global.NCF||{};
 BPP=NCF.BPP=NCF.BPP||{};
 Client=BPP.Client=BPP.Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Client.Main=function(report)
 {
  return Doc.Element("div",[],[Doc.Button("GO",[AttrProxy.Create("class","btn btn-primary")],function()
  {
   Client.fun1(report);
  })]);
 };
 Client.fun1=function(report)
 {
  var powerbi_settings,r,powerbi_conf,r$1,h,w;
  powerbi_settings=(r={},r.filterPaneEnabled=true,r);
  powerbi_conf=(r$1={},r$1.tokenType=1,r$1.accessToken=report.AccessToken,r$1.type="report",r$1.embedUrl=report.EmbedUrl,r$1.id=report.ReportId,r$1.settings=powerbi_settings,r$1);
  h=Global.jQuery("#embedReportHtml").text();
  w=Global.open();
  w.document.write(h);
  w.init(powerbi_conf);
 };
}());
