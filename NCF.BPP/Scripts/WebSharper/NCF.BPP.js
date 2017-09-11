(function()
{
 "use strict";
 var Global,NCF,BPP,Client,WebSharper,UI,Next,Doc,AttrProxy,Seq;
 Global=window;
 NCF=Global.NCF=Global.NCF||{};
 BPP=NCF.BPP=NCF.BPP||{};
 Client=BPP.Client=BPP.Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Seq=WebSharper&&WebSharper.Seq;
 Client.Main=function(workspaces)
 {
  return Doc.Element("div",[AttrProxy.Create("class","container")],[Doc.Element("div",[AttrProxy.Create("class","panel-group"),AttrProxy.Create("id","accordion")],[Client.renderWorkspaces(workspaces)])]);
 };
 Client.renderWorkspaces=function(workspaces)
 {
  return Doc.Concat(Seq.mapi(Client.renderWorkspace,workspaces));
 };
 Client.renderWorkspace=function(i,w)
 {
  return Doc.Element("div",[AttrProxy.Create("class","panel panel-default")],[Doc.Element("div",[AttrProxy.Create("class","panel-heading")],[Doc.Element("h4",[AttrProxy.Create("class","panel-title")],[Doc.Element("a",[AttrProxy.Create("data-"+"toggle","collapse"),AttrProxy.Create("data-"+"parent","#accordion"),AttrProxy.Create("href",(function($1)
  {
   return function($2)
   {
    return $1("#collapse"+Global.String($2));
   };
  }(Global.id))(i+1))],[Doc.TextNode(w.name)])])]),Doc.Element("div",[AttrProxy.Create("class","panel-collapse collapse"),AttrProxy.Create("id",(function($1)
  {
   return function($2)
   {
    return $1("collapse"+Global.String($2));
   };
  }(Global.id))(i+1))],[Doc.Element("div",[AttrProxy.Create("class","panel-body")],[Client.renderReports(w.reports)])])]);
 };
 Client.renderReports=function(rs)
 {
  return Doc.Concat(Seq.map(Client.renderReport,rs));
 };
 Client.renderReport=function(r)
 {
  return Doc.Element("div",[],[r["public"]?Doc.Element("a",[],[Doc.TextNode(r.name)]):Doc.Element("p",[],[Doc.TextNode(r.name)])]);
 };
}());
