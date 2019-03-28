module NCF.BPP.Resources

    open WebSharper.Core.Resources
    open WebSharper.Web

    [<Require(typeof<WebSharper.JQuery.Resources.JQuery>)>]
    type BootStrapJS() =
        inherit BaseResource("scripts/bootstrap/bootstrap.min.js")
