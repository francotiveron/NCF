(function()
{
 "use strict";
 var Global,WebSharper,Sitelets,PathUtil,Route,List,Router,Obj,RouterModule,ListArrayConverter,RouterOperators,SC$1,Strings,Seq,Collections,Map,List$1,Arrays,IntelliFactory,Runtime,FSharpMap,Utils,console,Unchecked,Nullable,Numeric,Lazy,Concurrency,$,Operators,Char,System,Guid,Slice;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Sitelets=WebSharper.Sitelets=WebSharper.Sitelets||{};
 PathUtil=Sitelets.PathUtil=Sitelets.PathUtil||{};
 Route=Sitelets.Route=Sitelets.Route||{};
 List=Sitelets.List=Sitelets.List||{};
 Router=Sitelets.Router=Sitelets.Router||{};
 Obj=WebSharper&&WebSharper.Obj;
 RouterModule=Sitelets.RouterModule=Sitelets.RouterModule||{};
 ListArrayConverter=RouterModule.ListArrayConverter=RouterModule.ListArrayConverter||{};
 RouterOperators=Sitelets.RouterOperators=Sitelets.RouterOperators||{};
 SC$1=Global.StartupCode$WebSharper_Sitelets$Router=Global.StartupCode$WebSharper_Sitelets$Router||{};
 Strings=WebSharper&&WebSharper.Strings;
 Seq=WebSharper&&WebSharper.Seq;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 List$1=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 FSharpMap=Collections&&Collections.FSharpMap;
 Utils=WebSharper&&WebSharper.Utils;
 console=Global.console;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Nullable=WebSharper&&WebSharper.Nullable;
 Numeric=WebSharper&&WebSharper.Numeric;
 Lazy=WebSharper&&WebSharper.Lazy;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 $=Global.jQuery;
 Operators=WebSharper&&WebSharper.Operators;
 Char=WebSharper&&WebSharper.Char;
 System=Global.System;
 Guid=System&&System.Guid;
 Slice=WebSharper&&WebSharper.Slice;
 PathUtil.WriteLink=function(s,q)
 {
  var query;
  query=q.get_IsEmpty()?"":"?"+PathUtil.WriteQuery(q);
  return"/"+PathUtil.Concat(s)+query;
 };
 PathUtil.WriteQuery=function(q)
 {
  function m(k,v)
  {
   return k+"="+v;
  }
  return Strings.concat("&",Seq.map(function($1)
  {
   return m($1[0],$1[1]);
  },Map.ToSeq(q)));
 };
 PathUtil.Concat=function(xs)
 {
  var sb,start;
  sb=[];
  start=true;
  List$1.iter(function(x)
  {
   if(!Strings.IsNullOrEmpty(x))
    {
     start?start=false:sb.push("/");
     sb.push(x);
    }
  },xs);
  return Strings.Join("",Arrays.ofSeq(sb));
 };
 Route=Sitelets.Route=Runtime.Class({
  ToLink:function()
  {
   return PathUtil.WriteLink(this.Segments,this.QueryArgs);
  }
 },null,Route);
 Route.FromHash=function(path,strict)
 {
  var m,h;
  m=path.indexOf("#");
  return m===-1?Route.get_Empty():(h=path.substring(m+1),strict!=null&&strict.$0?h===""||h==="/"?Route.get_Empty():Strings.StartsWith(h,"/")?Route.FromUrl(h.substring(1),{
   $:1,
   $0:true
  }):Route.Segment$2(h):Route.FromUrl(path.substring(m),{
   $:1,
   $0:false
  }));
 };
 Route.FromUrl=function(path,strict)
 {
  var p,m,i;
  p=(m=path.indexOf("?"),m===-1?[path,new FSharpMap.New([])]:[Strings.Substring(path,0,m),Route.ParseQuery(path.substring(m+1))]);
  i=Route.get_Empty();
  return Route.New(List$1.ofArray(Strings.SplitChars(p[0],["/"],strict!=null&&strict.$0?0:1)),p[1],i.FormData,i.Method,i.Body);
 };
 Route.WriteQuery=function(q)
 {
  return PathUtil.WriteQuery(q);
 };
 Route.ParseQuery=function(q)
 {
  return Map.OfArray(Arrays.ofSeq(Arrays.choose(function(kv)
  {
   var m,v;
   m=Strings.SplitChars(kv,["="],0);
   return m&&Arrays.length(m)===2?(v=Arrays.get(m,1),{
    $:1,
    $0:[Arrays.get(m,0),v]
   }):((function($1)
   {
    return function($2)
    {
     return $1("wrong format for query argument: "+Utils.toSafe($2));
    };
   }(function(s)
   {
    console.log(s);
   }))(kv),null);
  },Strings.SplitChars(q,["&"],0))));
 };
 Route.Combine=function(paths)
 {
  var method,body,queryArgs,formData,i,$1,paths$1,m,segments,l;
  paths$1=Arrays.ofSeq(paths);
  m=Arrays.length(paths$1);
  if(m===0)
   return Route.get_Empty();
  else
   if(m===1)
    return Arrays.get(paths$1,0);
   else
    {
     method=null;
     body=null;
     segments=[];
     queryArgs=new FSharpMap.New([]);
     formData=new FSharpMap.New([]);
     i=0;
     l=Arrays.length(paths$1);
     while(i<l)
      (function()
      {
       var p,m$1,m$2;
       p=Arrays.get(paths$1,i);
       m$1=p.Method;
       m$1!=null&&m$1.$==1?method=m$1:void 0;
       m$2=p.Body;
       m$2!=null&&m$2.$==1?body=m$2:void 0;
       queryArgs=Map.FoldBack(function(k,v,t)
       {
        return t.Add(k,v);
       },queryArgs,p.QueryArgs);
       formData=Map.FoldBack(function(k,v,t)
       {
        return t.Add(k,v);
       },formData,p.FormData);
       List$1.iter(function(a)
       {
        segments.push(a);
       },p.Segments);
       i=i+1;
      }());
     return Route.New(List$1.ofSeq(segments),queryArgs,formData,method,body);
    }
 };
 Route.Segment=function(s,m)
 {
  var i;
  i=Route.get_Empty();
  return Route.New(s,i.QueryArgs,i.FormData,m,i.Body);
 };
 Route.Segment$1=function(s)
 {
  var i;
  i=Route.get_Empty();
  return Route.New(s,i.QueryArgs,i.FormData,i.Method,i.Body);
 };
 Route.Segment$2=function(s)
 {
  var i;
  i=Route.get_Empty();
  return Route.New(List$1.ofArray([s]),i.QueryArgs,i.FormData,i.Method,i.Body);
 };
 Route.get_Empty=function()
 {
  return Route.New(List$1.T.Empty,new FSharpMap.New([]),new FSharpMap.New([]),null,null);
 };
 Route.New=function(Segments,QueryArgs,FormData,Method,Body)
 {
  return new Route({
   Segments:Segments,
   QueryArgs:QueryArgs,
   FormData:FormData,
   Method:Method,
   Body:Body
  });
 };
 List.startsWith=function(s,l)
 {
  var $1;
  switch(s.$==1?l.$==1?Unchecked.Equals(s.$0,l.$0)?($1=[l.$0,l.$1,s.$0,s.$1],1):2:2:0)
  {
   case 0:
    return{
     $:1,
     $0:l
    };
    break;
   case 1:
    return List.startsWith($1[3],$1[1]);
    break;
   case 2:
    return null;
    break;
  }
 };
 Router.op_Addition=function(a,b)
 {
  return Router.New(function(path)
  {
   return Seq.append(a.Parse(path),b.Parse(path));
  },a.Segment);
 };
 Router.op_Division=function(before,after)
 {
  return Router.New(function(path)
  {
   return Seq.collect(after.Parse,before.Parse(path));
  },Seq.append(before.Segment,after.Segment));
 };
 Router.FromString=function(name)
 {
  var parts,parts$1;
  parts=Strings.SplitChars(name,["/"],1);
  return parts.length==0?Router.New(function(path)
  {
   return[path];
  },[]):(parts$1=List$1.ofArray(parts),Router.New(function(path)
  {
   var m;
   m=List.startsWith(parts$1,path.Segments);
   return m!=null&&m.$==1?[Route.New(m.$0,path.QueryArgs,path.FormData,path.Method,path.Body)]:[];
  },[Route.Segment$1(parts$1)]));
 };
 Router.New=function(Parse,Segment)
 {
  return{
   Parse:Parse,
   Segment:Segment
  };
 };
 Router.op_Addition$1=function(a,b)
 {
  return Router.New$1(function(path)
  {
   return Seq.append(a.Parse(path),b.Parse(path));
  },function(value)
  {
   var m;
   m=a.Write(value);
   return m==null?b.Write(value):m;
  });
 };
 Router.op_Division$1=function(before,after)
 {
  return Router.New$1(function(path)
  {
   function m(p,x)
   {
    return Seq.map(function(p$1)
    {
     return[p$1,x];
    },after.Parse(p));
   }
   return Seq.collect(function($1)
   {
    return m($1[0],$1[1]);
   },before.Parse(path));
  },function(v)
  {
   var o;
   o=before.Write(v);
   return o==null?null:{
    $:1,
    $0:Seq.append(o.$0,after.Segment)
   };
  });
 };
 Router.op_Division$2=function(before,after)
 {
  return Router.New$1(function(path)
  {
   return Seq.collect(after.Parse,before.Parse(path));
  },function(v)
  {
   var o;
   o=after.Write(v);
   return o==null?null:{
    $:1,
    $0:Seq.append(before.Segment,o.$0)
   };
  });
 };
 Router.op_Division$3=function(before,after)
 {
  return Router.New$1(function(path)
  {
   function m(p,x)
   {
    function m$1(p$1,y)
    {
     return[p$1,[x,y]];
    }
    return Seq.map(function($1)
    {
     return m$1($1[0],$1[1]);
    },after.Parse(p));
   }
   return Seq.collect(function($1)
   {
    return m($1[0],$1[1]);
   },before.Parse(path));
  },function(t)
  {
   var $1,$2,$3;
   $1=before.Write(t[0]);
   $2=after.Write(t[1]);
   return $1!=null&&$1.$==1&&($2!=null&&$2.$==1&&($3=[$1.$0,$2.$0],true))?{
    $:1,
    $0:Seq.append($3[0],$3[1])
   }:null;
  });
 };
 Router.New$1=function(Parse,Write)
 {
  return{
   Parse:Parse,
   Write:Write
  };
 };
 ListArrayConverter=RouterModule.ListArrayConverter=Runtime.Class({
  WebSharper_Sitelets_RouterModule_IListArrayConverter$ToArray:function(l)
  {
   return Arrays.ofList(l);
  },
  WebSharper_Sitelets_RouterModule_IListArrayConverter$OfArray:function(a)
  {
   return List$1.ofArray(a);
  }
 },Obj,ListArrayConverter);
 ListArrayConverter.New=Runtime.Ctor(function()
 {
 },ListArrayConverter);
 RouterModule.Cast=function(tryParseCast,tryWriteCast,router)
 {
  return Router.New$1(function(path)
  {
   function c(p,v)
   {
    var m;
    m=tryParseCast(v);
    return m!=null&&m.$==1?{
     $:1,
     $0:[p,m.$0]
    }:null;
   }
   return Seq.choose(function($1)
   {
    return c($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   var o;
   o=tryWriteCast(value);
   return o==null?null:router.Write(o.$0);
  });
 };
 RouterModule.Unbox=function(tryUnbox,router)
 {
  return Router.New$1(function(path)
  {
   function c(p,v)
   {
    var m;
    m=tryUnbox(v);
    return m!=null&&m.$==1?{
     $:1,
     $0:[p,m.$0]
    }:null;
   }
   return Seq.choose(function($1)
   {
    return c($1[0],$1[1]);
   },router.Parse(path));
  },router.Write);
 };
 RouterModule.Box=function(tryUnbox,router)
 {
  return Router.New$1(function(path)
  {
   function m(p,v)
   {
    return[p,v];
   }
   return Seq.map(function($1)
   {
    return m($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   var o;
   o=tryUnbox(value);
   return o==null?null:router.Write(o.$0);
  });
 };
 RouterModule.List=function(item)
 {
  return RouterModule.Map(List$1.ofArray,Arrays.ofList,RouterModule.Array(item));
 };
 RouterModule.Option=function(item)
 {
  return Router.New$1(function(path)
  {
   var $1,m;
   m=path.Segments;
   function m$1(p,v)
   {
    return[p,{
     $:1,
     $0:v
    }];
   }
   switch(m.$==1?m.$0==="None"?($1=m.$1,0):m.$0==="Some"?($1=m.$1,1):2:2)
   {
    case 0:
     return[[Route.New($1,path.QueryArgs,path.FormData,path.Method,path.Body),null]];
     break;
    case 1:
     return Seq.map(function($2)
     {
      return m$1($2[0],$2[1]);
     },item.Parse(Route.New($1,path.QueryArgs,path.FormData,path.Method,path.Body)));
     break;
    case 2:
     return[];
     break;
   }
  },function(value)
  {
   var x,m,s;
   return value!=null&&value.$==1?(x=item.Write(value.$0),(m=(s=[Route.Segment$2("Some")],function(s$1)
   {
    return Seq.append(s,s$1);
   }),x==null?null:{
    $:1,
    $0:m(x.$0)
   })):{
    $:1,
    $0:[Route.Segment$2("None")]
   };
  });
 };
 RouterModule.Nullable=function(item)
 {
  return Router.New$1(function(path)
  {
   var m,$1;
   function m$1(p,v)
   {
    return[p,v];
   }
   m=path.Segments;
   return m.$==1&&(m.$0==="null"&&($1=m.$1,true))?[[Route.New($1,path.QueryArgs,path.FormData,path.Method,path.Body),null]]:Seq.map(function($2)
   {
    return m$1($2[0],$2[1]);
   },item.Parse(path));
  },function(value)
  {
   return value!=null?item.Write(Nullable.get(value)):{
    $:1,
    $0:[Route.Segment$2("null")]
   };
  });
 };
 RouterModule.Array=function(item)
 {
  return Router.New$1(function(path)
  {
   var m,m$1,o;
   function collect(l,path$1,acc)
   {
    function m$2(p,a)
    {
     return collect(l-1,p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return l===0?[[path$1,Arrays.ofList(List$1.rev(acc))]]:Seq.collect(function($1)
    {
     return m$2($1[0],$1[1]);
    },item.Parse(path$1));
   }
   m=path.Segments;
   return m.$==1?(m$1=(o=0,[Numeric.TryParseInt32(m.$0,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]),m$1[0]?collect(m$1[1],Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),List$1.T.Empty):[]):[];
  },function(value)
  {
   var parts;
   parts=Arrays.map(item.Write,value);
   return Arrays.forall(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.append([Route.Segment$2(Global.String(Arrays.length(value)))],Seq.collect(function(o)
    {
     return o.$0;
    },parts))
   }:null;
  });
 };
 RouterModule.Delay=function(getRouter)
 {
  var r;
  r=Lazy.Create(getRouter);
  return Router.New$1(function(path)
  {
   return r.f().Parse(path);
  },function(value)
  {
   return r.f().Write(value);
  });
 };
 RouterModule.Single=function(endpoint,route)
 {
  var parts,parts$1;
  parts=Strings.SplitChars(route,["/"],1);
  return parts.length==0?Router.New$1(function(path)
  {
   return[[path,endpoint]];
  },function(value)
  {
   return Unchecked.Equals(value,endpoint)?{
    $:1,
    $0:[]
   }:null;
  }):(parts$1=List$1.ofArray(parts),Router.New$1(function(path)
  {
   var m;
   m=List.startsWith(parts$1,path.Segments);
   return m!=null&&m.$==1?[[Route.New(m.$0,path.QueryArgs,path.FormData,path.Method,path.Body),endpoint]]:[];
  },function(value)
  {
   return Unchecked.Equals(value,endpoint)?{
    $:1,
    $0:[Route.Segment$1(parts$1)]
   }:null;
  }));
 };
 RouterModule.Table=function(mapping)
 {
  function m(v,s)
  {
   return RouterModule.MapTo(v,Router.FromString(s));
  }
  return RouterModule.Sum(Seq.map(function($1)
  {
   return m($1[0],$1[1]);
  },mapping));
 };
 RouterModule.Sum=function(routers)
 {
  var routers$1;
  routers$1=Arrays.ofSeq(routers);
  return Router.New$1(function(path)
  {
   return Seq.collect(function(r)
   {
    return r.Parse(path);
   },routers$1);
  },function(value)
  {
   return Seq.tryPick(function(r)
   {
    return r.Write(value);
   },routers$1);
  });
 };
 RouterModule.MapTo=function(value,router)
 {
  return Router.New$1(function(path)
  {
   return Seq.map(function(p)
   {
    return[p,value];
   },router.Parse(path));
  },function(v)
  {
   return Unchecked.Equals(v,value)?{
    $:1,
    $0:router.Segment
   }:null;
  });
 };
 RouterModule.Filter=function(predicate,router)
 {
  return Router.New$1(function(path)
  {
   function f(t)
   {
    return t[1];
   }
   return Seq.filter(function(x)
   {
    return predicate(f(x));
   },router.Parse(path));
  },function(value)
  {
   return predicate(value)?router.Write(value):null;
  });
 };
 RouterModule.TryMap=function(decode,encode,router)
 {
  return Router.New$1(function(path)
  {
   function c(p,v)
   {
    var o;
    o=decode(v);
    return o==null?null:{
     $:1,
     $0:[p,o.$0]
    };
   }
   return Seq.choose(function($1)
   {
    return c($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   var o;
   o=encode(value);
   return o==null?null:router.Write(o.$0);
  });
 };
 RouterModule.Map=function(decode,encode,router)
 {
  return Router.New$1(function(path)
  {
   function m(p,v)
   {
    return[p,decode(v)];
   }
   return Seq.map(function($1)
   {
    return m($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   return router.Write(encode(value));
  });
 };
 RouterModule.Embed=function(decode,encode,router)
 {
  return Router.New$1(function(path)
  {
   function m(p,v)
   {
    return[p,decode(v)];
   }
   return Seq.map(function($1)
   {
    return m($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   var o;
   o=encode(value);
   return o==null?null:router.Write(o.$0);
  });
 };
 RouterModule.Slice=function(decode,encode,router)
 {
  return Router.New$1(function(path)
  {
   function c(p,v)
   {
    var o;
    o=decode(v);
    return o==null?null:{
     $:1,
     $0:[p,o.$0]
    };
   }
   return Seq.choose(function($1)
   {
    return c($1[0],$1[1]);
   },router.Parse(path));
  },function(value)
  {
   return router.Write(encode(value));
  });
 };
 RouterModule.HashLink=function(router,endpoint)
 {
  return"#"+RouterModule.Link(router,endpoint);
 };
 RouterModule.Ajax=function(router,endpoint)
 {
  var m,path,settings,r,m$1,m$2,fd;
  m=RouterModule.Write(router,endpoint);
  return m!=null&&m.$==1?(path=m.$0,(settings=(r={},r.dataType="text",r),(m$1=path.Method,m$1!=null&&m$1.$==1?settings.type=m$1.$0:void 0,m$2=path.Body,m$2!=null&&m$2.$==1?(settings.contentType="application/json",settings.data=m$2.$0,settings.processData=false,path.Method==null?settings.type="POST":void 0):(!path.FormData.get_IsEmpty()?(fd=new Global.FormData(),Map.Iterate(function(k,v)
  {
   return fd.append(k,v);
  },path.FormData),settings.contentType=false,settings.data=fd,settings.processData=false):void 0,path.Method==null?settings.type="POST":void 0),Concurrency.FromContinuations(function(ok,err)
  {
   settings.success=function(res)
   {
    return ok(res);
   };
   settings.error=function(a,a$1,msg)
   {
    return err(Global.Error(msg));
   };
   $.ajax(path.ToLink(),settings);
  })))):Operators.FailWith("Failed to map endpoint to request");
 };
 RouterModule.Link=function(router,endpoint)
 {
  var m;
  m=RouterModule.Write(router,endpoint);
  return m==null?"":m.$0.ToLink();
 };
 RouterModule.TryLink=function(router,endpoint)
 {
  var m;
  m=RouterModule.Write(router,endpoint);
  return m==null?null:{
   $:1,
   $0:m.$0.ToLink()
  };
 };
 RouterModule.Write=function(router,endpoint)
 {
  var o;
  o=router.Write(endpoint);
  return o==null?null:{
   $:1,
   $0:Route.Combine(o.$0)
  };
 };
 RouterModule.Parse=function(router,path)
 {
  function c(path$1,value)
  {
   return path$1.Segments.$==0?{
    $:1,
    $0:value
   }:null;
  }
  return Seq.tryPick(function($1)
  {
   return c($1[0],$1[1]);
  },router.Parse(path));
 };
 RouterModule.FormData=function(item)
 {
  return Router.New$1(function(path)
  {
   function m(a,r)
   {
    return[path,r];
   }
   return Seq.map(function($1)
   {
    return m($1[0],$1[1]);
   },item.Parse(Route.New(path.Segments,path.FormData,path.FormData,path.Method,path.Body)));
  },function(value)
  {
   var o;
   o=item.Write(value);
   return o==null?null:{
    $:1,
    $0:Seq.map(function(p)
    {
     return Route.New(p.Segments,new FSharpMap.New([]),p.QueryArgs,p.Method,p.Body);
    },o.$0)
   };
  });
 };
 RouterModule.Body=function(deserialize,serialize)
 {
  return Router.New$1(function(path)
  {
   var m,o;
   m=(o=path.Body,o==null?null:deserialize(o.$0));
   return m!=null&&m.$==1?[[Route.New(path.Segments,path.QueryArgs,path.FormData,path.Method,null),m.$0]]:[];
  },function(value)
  {
   var i;
   return{
    $:1,
    $0:[(i=Route.get_Empty(),Route.New(i.Segments,i.QueryArgs,i.FormData,i.Method,{
     $:1,
     $0:serialize(value)
    }))]
   };
  });
 };
 RouterModule.Method=function(m)
 {
  var i;
  return Router.New(function(path)
  {
   var m$1,$1;
   m$1=path.Method;
   return m$1!=null&&m$1.$==1&&(m$1.$0===m&&($1=m$1.$0,true))?[path]:[];
  },[(i=Route.get_Empty(),Route.New(i.Segments,i.QueryArgs,i.FormData,{
   $:1,
   $0:m
  },i.Body))]);
 };
 RouterModule.QueryNullable=function(key,item)
 {
  return Router.New$1(function(path)
  {
   var m,newQa,i;
   function m$1(a,v)
   {
    return[Route.New(path.Segments,newQa,path.FormData,path.Method,path.Body),v];
   }
   m=path.QueryArgs.TryFind(key);
   return m!=null&&m.$==1?(newQa=path.QueryArgs.Remove(key),Seq.map(function($1)
   {
    return m$1($1[0],$1[1]);
   },item.Parse((i=Route.get_Empty(),Route.New(List$1.ofArray([m.$0]),i.QueryArgs,i.FormData,i.Method,i.Body))))):[[path,null]];
  },function(value)
  {
   var o,m,$1,i;
   return value!=null?(o=item.Write(Nullable.get(value)),o==null?null:{
    $:1,
    $0:(m=Route.Combine(o.$0).Segments,m.$==1&&(m.$1.$==0&&($1=m.$0,true))?[(i=Route.get_Empty(),Route.New(i.Segments,Map.OfArray(Arrays.ofSeq(List$1.ofArray([[key,$1]]))),i.FormData,i.Method,i.Body))]:[])
   }):{
    $:1,
    $0:[]
   };
  });
 };
 RouterModule.QueryOption=function(key,item)
 {
  return Router.New$1(function(path)
  {
   var m,newQa,i;
   function m$1(a,v)
   {
    return[Route.New(path.Segments,newQa,path.FormData,path.Method,path.Body),{
     $:1,
     $0:v
    }];
   }
   m=path.QueryArgs.TryFind(key);
   return m!=null&&m.$==1?(newQa=path.QueryArgs.Remove(key),Seq.map(function($1)
   {
    return m$1($1[0],$1[1]);
   },item.Parse((i=Route.get_Empty(),Route.New(List$1.ofArray([m.$0]),i.QueryArgs,i.FormData,i.Method,i.Body))))):[[path,null]];
  },function(value)
  {
   var o,m,$1,i;
   return value!=null&&value.$==1?(o=item.Write(value.$0),o==null?null:{
    $:1,
    $0:(m=Route.Combine(o.$0).Segments,m.$==1&&(m.$1.$==0&&($1=m.$0,true))?[(i=Route.get_Empty(),Route.New(i.Segments,Map.OfArray(Arrays.ofSeq(List$1.ofArray([[key,$1]]))),i.FormData,i.Method,i.Body))]:[])
   }):{
    $:1,
    $0:[]
   };
  });
 };
 RouterModule.Query=function(key,item)
 {
  return Router.New$1(function(path)
  {
   var m,newQa,i;
   function m$1(p,v)
   {
    return[Route.New(path.Segments,newQa,path.FormData,path.Method,path.Body),v];
   }
   m=path.QueryArgs.TryFind(key);
   return m!=null&&m.$==1?(newQa=path.QueryArgs.Remove(key),Seq.map(function($1)
   {
    return m$1($1[0],$1[1]);
   },item.Parse((i=Route.get_Empty(),Route.New(List$1.ofArray([m.$0]),i.QueryArgs,i.FormData,i.Method,i.Body))))):[];
  },function(value)
  {
   var o,m,$1,i;
   o=item.Write(value);
   return o==null?null:{
    $:1,
    $0:(m=Route.Combine(o.$0).Segments,m.$==1&&(m.$1.$==0&&($1=m.$0,true))?[(i=Route.get_Empty(),Route.New(i.Segments,Map.OfArray(Arrays.ofSeq(List$1.ofArray([[key,$1]]))),i.FormData,i.Method,i.Body))]:[])
   };
  });
 };
 RouterModule.CreateWithQuery=function(ser,des)
 {
  return Router.New$1(function(path)
  {
   return[[Route.New(List$1.T.Empty,path.QueryArgs,path.FormData,path.Method,path.Body),des(path.Segments,path.QueryArgs)]];
  },function(value)
  {
   var p,i;
   p=ser(value);
   return{
    $:1,
    $0:[(i=Route.get_Empty(),Route.New(p[0],p[1],i.FormData,i.Method,i.Body))]
   };
  });
 };
 RouterModule.Create=function(ser,des)
 {
  return Router.New$1(function(path)
  {
   return[[Route.New(List$1.T.Empty,path.QueryArgs,path.FormData,path.Method,path.Body),des(path.Segments)]];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$1(ser(value))]
   };
  });
 };
 RouterModule.New=function(route,link)
 {
  return{
   WebSharper_Sitelets_IRouter_1$Route:route,
   WebSharper_Sitelets_IRouter_1$Link:link
  };
 };
 RouterModule.Empty=function()
 {
  SC$1.$cctor();
  return SC$1.Empty;
 };
 RouterOperators.JSClass=function(ctor,partsAndFields,subClasses)
 {
  var fields;
  fields=Arrays.ofSeq(Seq.choose(function(p)
  {
   return p.$==1?{
    $:1,
    $0:[p.$0[0],p.$0[1]]
   }:null;
  },partsAndFields));
  return RouterOperators.Class(function(value)
  {
   function m(fn,opt)
   {
    var v;
    return opt?(v=value[fn],Unchecked.Equals(v,void 0)?null:{
     $:1,
     $0:v
    }):value[fn];
   }
   return Arrays.map(function($1)
   {
    return m($1[0],$1[1]);
   },fields);
  },function(fieldValues)
  {
   var o;
   function a(fn,opt)
   {
    return function(v)
    {
     if(opt)
     {
      if(v!=null&&v.$==1)
       o[fn]=v.$0;
     }
     else
      o[fn]=v;
    };
   }
   o=ctor();
   (((Runtime.Curried3(Arrays.iter2))(function($1,$2)
   {
    return(function($3)
    {
     return a($3[0],$3[1]);
    }($1))($2);
   }))(fields))(fieldValues);
   return o;
  },Arrays.map(function(p)
  {
   return p.$==1?{
    $:1,
    $0:p.$0[2]
   }:{
    $:0,
    $0:p.$0
   };
  },partsAndFields),subClasses);
 };
 RouterOperators.Class=function(readFields,createObject,partsAndFields,subClasses)
 {
  var partsAndFieldsList,thisClass;
  partsAndFieldsList=List$1.ofArray(partsAndFields);
  thisClass=Router.New$1(function(path)
  {
   function collect(fields,path$1,acc)
   {
    var t,m,$1;
    function m$1(p,a)
    {
     return collect(t,p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return fields.$==1?fields.$0.$==1?(t=fields.$1,Seq.collect(function($2)
    {
     return m$1($2[0],$2[1]);
    },fields.$0.$0.Parse(path$1))):(m=path$1.Segments,m.$==1&&(m.$0===fields.$0.$0&&($1=[m.$0,m.$1],true))?collect(fields.$1,Route.New($1[1],path$1.QueryArgs,path$1.FormData,path$1.Method,path$1.Body),acc):[]):[[path$1,createObject(Arrays.ofList(List$1.rev(acc)))]];
   }
   return collect(partsAndFieldsList,path,List$1.T.Empty);
  },function(value)
  {
   var fields,index,parts;
   fields=readFields(value);
   index=-1;
   parts=Arrays.map(function(a)
   {
    return a.$==1?(index=index+1,a.$0.Write(Arrays.get(fields,index))):{
     $:1,
     $0:[Route.Segment$2(a.$0)]
    };
   },partsAndFields);
   return Arrays.forall(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.collect(function(o)
    {
     return o.$0;
    },parts)
   }:null;
  });
  return subClasses.length==0?thisClass:RouterModule.Sum(Seq.append(subClasses,[thisClass]));
 };
 RouterOperators.JSUnion=function(t,cases)
 {
  function m(a,m$1,p,r)
  {
   return[m$1,p,r];
  }
  return RouterOperators.Union(function(value)
  {
   var constIndex;
   function p($1)
   {
    return $1!=null&&$1.$==1&&Unchecked.Equals(value,$1.$0);
   }
   constIndex=Seq.tryFindIndex(function($1)
   {
    return p($1[0]);
   },cases);
   return constIndex!=null&&constIndex.$==1?constIndex.$0:value.$;
  },function(tag,value)
  {
   return Arrays.init(Arrays.length((Arrays.get(cases,tag))[3]),function(i)
   {
    return value["$"+Global.String(i)];
   });
  },function(tag,fieldValues)
  {
   var o,m$1,$1;
   o=t==null?{}:new t();
   m$1=Arrays.get(cases,tag);
   return($1=m$1[0],$1!=null&&$1.$==1)?m$1[0].$0:(o.$=tag,Seq.iteri(function(i,v)
   {
    o["$"+Global.String(i)]=v;
   },fieldValues),o);
  },Arrays.map(function($1)
  {
   return m($1[0],$1[1],$1[2],$1[3]);
  },cases));
 };
 RouterOperators.Union=function(getTag,readFields,createCase,cases)
 {
  return Router.New$1(function(path)
  {
   function m(i,a)
   {
    var $1,$2,m$1,p,m$2;
    function collect(fields,path$1,acc)
    {
     var t;
     function m$3(p$1,a$1)
     {
      return collect(t,p$1,new List$1.T({
       $:1,
       $0:a$1,
       $1:acc
      }));
     }
     return fields.$==1?(t=fields.$1,Seq.collect(function($3)
     {
      return m$3($3[0],$3[1]);
     },fields.$0.Parse(path$1))):[[path$1,createCase(i,Arrays.ofList(List$1.rev(acc)))]];
    }
    return($1=path.Method,($2=a[0],$1!=null&&$1.$==1?$2!=null&&$2.$==1?$1.$0===$2.$0:true:$2!=null&&$2.$==1?false:true))?(m$1=List.startsWith(List$1.ofArray(a[1]),path.Segments),m$1==null?[]:(p=m$1.$0,(m$2=List$1.ofArray(a[2]),m$2.$==0?[[Route.New(p,path.QueryArgs,path.FormData,path.Method,path.Body),createCase(i,[])]]:collect(m$2,Route.New(p,path.QueryArgs,path.FormData,path.Method,path.Body),List$1.T.Empty)))):[];
   }
   return Seq.collect(function($1)
   {
    return m($1[0],$1[1]);
   },Seq.indexed(cases));
  },function(value)
  {
   var tag,p,fields,casePath,fieldParts;
   function m(v,f)
   {
    return f.Write(v);
   }
   tag=getTag(value);
   p=Arrays.get(cases,tag);
   fields=p[2];
   casePath=[Route.Segment(List$1.ofArray(p[1]),p[0])];
   return fields&&Arrays.length(fields)===0?{
    $:1,
    $0:casePath
   }:(fieldParts=(((Runtime.Curried3(Arrays.map2))(m))(readFields(tag,value)))(fields),Arrays.forall(function(o)
   {
    return o!=null;
   },fieldParts)?{
    $:1,
    $0:Seq.append(casePath,Seq.collect(function(o)
    {
     return o.$0;
    },fieldParts))
   }:null);
  });
 };
 RouterOperators.JSRecord=function(t,fields)
 {
  function m(a,a$1,r)
  {
   return r;
  }
  return RouterOperators.Record(function(value)
  {
   function m$1(fn,opt,a)
   {
    var v;
    return opt?(v=value[fn],Unchecked.Equals(v,void 0)?null:{
     $:1,
     $0:v
    }):value[fn];
   }
   return Arrays.map(function($1)
   {
    return m$1($1[0],$1[1],$1[2]);
   },fields);
  },function(fieldValues)
  {
   var o;
   function a(fn,opt,a$1)
   {
    return function(v)
    {
     if(opt)
     {
      if(v!=null&&v.$==1)
       o[fn]=v.$0;
     }
     else
      o[fn]=v;
    };
   }
   o=t==null?{}:new t();
   (((Runtime.Curried3(Arrays.iter2))(function($1,$2)
   {
    return(function($3)
    {
     return a($3[0],$3[1],$3[2]);
    }($1))($2);
   }))(fields))(fieldValues);
   return o;
  },Arrays.map(function($1)
  {
   return m($1[0],$1[1],$1[2]);
  },fields));
 };
 RouterOperators.Record=function(readFields,createRecord,fields)
 {
  var fieldsList;
  fieldsList=List$1.ofArray(fields);
  return Router.New$1(function(path)
  {
   function collect(fields$1,path$1,acc)
   {
    var t;
    function m(p,a)
    {
     return collect(t,p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return fields$1.$==1?(t=fields$1.$1,Seq.collect(function($1)
    {
     return m($1[0],$1[1]);
    },fields$1.$0.Parse(path$1))):[[path$1,createRecord(Arrays.ofList(List$1.rev(acc)))]];
   }
   return collect(fieldsList,path,List$1.T.Empty);
  },function(value)
  {
   var parts;
   function m(v,r)
   {
    return r.Write(v);
   }
   parts=(((Runtime.Curried3(Arrays.map2))(m))(readFields(value)))(fields);
   return Arrays.forall(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.collect(function(o)
    {
     return o.$0;
    },parts)
   }:null;
  });
 };
 RouterOperators.JSTuple=function(items)
 {
  return RouterOperators.Tuple(function(value)
  {
   return Arrays.init(Arrays.length(items),function(i)
   {
    return value[i];
   });
  },Global.id,items);
 };
 RouterOperators.Tuple=function(readItems,createTuple,items)
 {
  return Router.New$1(function(path)
  {
   function collect(elems,path$1,acc)
   {
    var t;
    function m(p,a)
    {
     return collect(t,p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return elems.$==1?(t=elems.$1,Seq.collect(function($1)
    {
     return m($1[0],$1[1]);
    },elems.$0.Parse(path$1))):[[path$1,createTuple(Arrays.ofList(List$1.rev(acc)))]];
   }
   return collect(List$1.ofArray(items),path,List$1.T.Empty);
  },function(value)
  {
   var parts;
   function m(v,r)
   {
    return r.Write(v);
   }
   parts=(((Runtime.Curried3(Arrays.map2))(m))(readItems(value)))(items);
   return Arrays.forall(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.collect(function(o)
    {
     return o.$0;
    },parts)
   }:null;
  });
 };
 RouterOperators.rDateTime=function()
 {
  SC$1.$cctor();
  return SC$1.rDateTime;
 };
 RouterOperators.rWildcardList=function(item)
 {
  return Router.New$1(function(path)
  {
   var m;
   function collect(path$1,acc)
   {
    function m$1(p,a)
    {
     return collect(p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return path$1.Segments.$==0?[[path$1,List$1.rev(acc)]]:Seq.collect(function($1)
    {
     return m$1($1[0],$1[1]);
    },item.Parse(path$1));
   }
   m=path.Segments;
   return m.$==1?collect(Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),List$1.T.Empty):[[path,List$1.T.Empty]];
  },function(value)
  {
   var parts;
   parts=List$1.map(item.Write,value);
   return List$1.forAll(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.collect(function(o)
    {
     return o.$0;
    },parts)
   }:null;
  });
 };
 RouterOperators.rWildcardArray=function(item)
 {
  return Router.New$1(function(path)
  {
   var m;
   function collect(path$1,acc)
   {
    function m$1(p,a)
    {
     return collect(p,new List$1.T({
      $:1,
      $0:a,
      $1:acc
     }));
    }
    return path$1.Segments.$==0?[[path$1,Arrays.ofList(List$1.rev(acc))]]:Seq.collect(function($1)
    {
     return m$1($1[0],$1[1]);
    },item.Parse(path$1));
   }
   m=path.Segments;
   return m.$==1?collect(Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),List$1.T.Empty):[[path,[]]];
  },function(value)
  {
   var parts;
   parts=Arrays.map(item.Write,value);
   return Arrays.forall(function(o)
   {
    return o!=null;
   },parts)?{
    $:1,
    $0:Seq.collect(function(o)
    {
     return o.$0;
    },parts)
   }:null;
  });
 };
 RouterOperators.rWildcard=function()
 {
  SC$1.$cctor();
  return SC$1.rWildcard;
 };
 RouterOperators.rBool=function()
 {
  SC$1.$cctor();
  return SC$1.rBool;
 };
 RouterOperators.rSingle=function()
 {
  SC$1.$cctor();
  return SC$1.rSingle;
 };
 RouterOperators.rUInt64=function()
 {
  SC$1.$cctor();
  return SC$1.rUInt64;
 };
 RouterOperators.rInt64=function()
 {
  SC$1.$cctor();
  return SC$1.rInt64;
 };
 RouterOperators.rUInt=function()
 {
  SC$1.$cctor();
  return SC$1.rUInt;
 };
 RouterOperators.rUInt16=function()
 {
  SC$1.$cctor();
  return SC$1.rUInt16;
 };
 RouterOperators.rInt16=function()
 {
  SC$1.$cctor();
  return SC$1.rInt16;
 };
 RouterOperators.rByte=function()
 {
  SC$1.$cctor();
  return SC$1.rByte;
 };
 RouterOperators.rSByte=function()
 {
  SC$1.$cctor();
  return SC$1.rSByte;
 };
 RouterOperators.rDouble=function()
 {
  SC$1.$cctor();
  return SC$1.rDouble;
 };
 RouterOperators.rInt=function()
 {
  SC$1.$cctor();
  return SC$1.rInt;
 };
 RouterOperators.rGuid=function()
 {
  SC$1.$cctor();
  return SC$1.rGuid;
 };
 RouterOperators.rChar=function()
 {
  SC$1.$cctor();
  return SC$1.rChar;
 };
 RouterOperators.rString=function()
 {
  SC$1.$cctor();
  return SC$1.rString;
 };
 RouterOperators.rRoot=function()
 {
  SC$1.$cctor();
  return SC$1.rRoot;
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  function pInt(x)
  {
   var m,o;
   m=(o=0,[Numeric.TryParseInt32(x,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]);
   return m[0]?{
    $:1,
    $0:m[1]
   }:null;
  }
  SC$1.Empty=Router.New$1(function()
  {
   return[];
  },function()
  {
   return null;
  });
  SC$1.rRoot=Router.New(function(path)
  {
   return[path];
  },[]);
  SC$1.rString=Router.New$1(function(path)
  {
   var m;
   m=path.Segments;
   return m.$==1?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),Global.decodeURIComponent(m.$0)]]:[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(value==null?"null":Global.encodeURIComponent(value))]
   };
  });
  SC$1.rChar=Router.New$1(function(path)
  {
   var m,$1;
   m=path.Segments;
   return m.$==1&&(m.$0.length===1&&($1=[m.$0,m.$1],true))?[[Route.New($1[1],path.QueryArgs,path.FormData,path.Method,path.Body),Char.Parse(Global.decodeURIComponent($1[0]))]]:[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.encodeURIComponent(value))]
   };
  });
  SC$1.rGuid=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=null,Guid.TryParse(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rInt=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseInt32(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rDouble=Router.New$1(function(path)
  {
   var m,res,$1;
   m=path.Segments;
   return m.$==1?(res=0,($1=Global.Number(m.$0),Global.isNaN($1)?false:(res=$1,true))?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rSByte=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseSByte(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rByte=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseByte(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rInt16=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseInt16(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rUInt16=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseUInt16(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rUInt=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseUInt32(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rInt64=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseInt64(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rUInt64=Router.New$1(function(path)
  {
   var m,res;
   m=path.Segments;
   return m.$==1?(res=0,Numeric.TryParseUInt64(m.$0,{
    get:function()
    {
     return res;
    },
    set:function(v)
    {
     res=v;
    }
   })?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rSingle=Router.New$1(function(path)
  {
   var m,res,$1;
   m=path.Segments;
   return m.$==1?(res=0,($1=Global.Number(m.$0),Global.isNaN($1)?false:(res=$1,true))?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),res]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(Global.String(value))]
   };
  });
  SC$1.rBool=Router.New$1(function(path)
  {
   var m,m$1,o;
   m=path.Segments;
   return m.$==1?(m$1=(o=null,[Numeric.TryParseBool(m.$0,{
    get:function()
    {
     return o;
    },
    set:function(v)
    {
     o=v;
    }
   }),o]),m$1[0]?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),m$1[1]]]:[]):[];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(value?"True":"False")]
   };
  });
  SC$1.rWildcard=Router.New$1(function(path)
  {
   return[[Route.New(List$1.T.Empty,path.QueryArgs,path.FormData,path.Method,path.Body),Strings.concat("/",path.Segments)]];
  },function(value)
  {
   return{
    $:1,
    $0:[Route.Segment$2(value)]
   };
  });
  SC$1.rDateTime=Router.New$1(function(path)
  {
   var m,h,$1,$2,$3,$4,$5,$6,$7;
   m=path.Segments;
   return m.$==1?(h=m.$0,h.length===19&&h[4]==="-"&&h[7]==="-"&&h[10]==="-"&&h[13]==="."&&h[16]==="."?($1=pInt(Slice.string(h,{
    $:1,
    $0:0
   },{
    $:1,
    $0:3
   })),($2=pInt(Slice.string(h,{
    $:1,
    $0:5
   },{
    $:1,
    $0:6
   })),($3=pInt(Slice.string(h,{
    $:1,
    $0:8
   },{
    $:1,
    $0:9
   })),($4=pInt(Slice.string(h,{
    $:1,
    $0:11
   },{
    $:1,
    $0:12
   })),($5=pInt(Slice.string(h,{
    $:1,
    $0:14
   },{
    $:1,
    $0:15
   })),($6=pInt(Slice.string(h,{
    $:1,
    $0:17
   },{
    $:1,
    $0:18
   })),$1!=null&&$1.$==1&&($2!=null&&$2.$==1&&($3!=null&&$3.$==1&&($4!=null&&$4.$==1&&($5!=null&&$5.$==1&&($6!=null&&$6.$==1&&($7=[$3.$0,$4.$0,$2.$0,$5.$0,$6.$0,$1.$0],true))))))?[[Route.New(m.$1,path.QueryArgs,path.FormData,path.Method,path.Body),(new Global.Date($7[5],$7[2]-1,$7[0],$7[1],$7[3],$7[4])).getTime()]]:[])))))):[]):[];
  },function(d)
  {
   var s,m;
   function pad2(x)
   {
    var s$1;
    s$1=Global.String(x);
    return s$1.length===1?"0"+s$1:s$1;
   }
   return{
    $:1,
    $0:[Route.Segment$2((s=Global.String((new Global.Date(d)).getFullYear()),(m=s.length,m===1?"000"+s:m===2?"00"+s:m===3?"0"+s:s))+"-"+pad2((new Global.Date(d)).getMonth()+1)+"-"+pad2((new Global.Date(d)).getDate())+"-"+pad2((new Global.Date(d)).getHours())+"."+pad2((new Global.Date(d)).getMinutes())+"."+pad2((new Global.Date(d)).getSeconds()))]
   };
  });
 };
}());
