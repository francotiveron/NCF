namespace NCF.BPP.PowerBI.JSExtension

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let PowerBIView = 
        Class "PowerBIView" |=> Inherits T<Window>

    let PowerBISettings =
        Pattern.Config "PowerBISettings" {
            Required = []
            Optional =
                [
                    "filterPaneEnabled" , T<bool>             
                    "navContentPaneEnabled", T<bool>
               ]
        }

    let PowerBIConfig =
        Pattern.Config "PowerBIConfig" {
            Required = []
            Optional =
                [
                    "type", T<string>
                    "tokenType", T<int>
                    "accessToken" , T<string>
                    "embedUrl", T<string>
                    "id", T<string>  
                    "permissions", T<int>
                    "settings", PowerBISettings.Type
                ]
        }

    let PowerBIViewClass = 
            PowerBIView |+> Instance [ "init" => T<string>?title * PowerBIConfig?embed_config ^-> T<unit>]

    let Assembly =
        Assembly [
            Namespace "NCF.BPP.PowerBI.JSExtension" [
                PowerBIConfig
                PowerBISettings
                PowerBIViewClass
                ]
        ]

(* github powerbi websharper demo
    let R1 = Resource "PowerBIResource" "scripts/powerbi.js"

    let PB = Class "powerbi"

    let PowerBISettings =
        Pattern.Config "PowerBISettings" {
            Required = []
            Optional =
                [
                    "filterPaneEnabled" , T<bool>             
                ]
        }

    let PowerBIConfig =
        Pattern.Config "PowerBIConfig" {
            Required = []
            Optional =
                [
                    "tokenType", T<int>
                    "accessToken" , T<string>
                    "embedUrl", T<string>
                    "type", T<string>
                    "id", T<string>  
                    "settings", PowerBISettings.Type
                ]
        }

    let PowerBIClass =
        PB
        |+> Static [
                "embed" => T<Dom.Element>?target * PowerBIConfig?config ^-> T<unit>
            ]

    let Assembly =
        Assembly [
            Namespace "NCF.BPP.PowerBI.JSExtension" [
                PowerBIConfig
                PowerBISettings
                PowerBIClass
            ]
            Namespace "NCF.BPP.PowerBI.JSExtension.Resources" [                
                R1
                |> fun r -> r.AssemblyWide()
            ] 
        ] |> Requires [R1]
*)
(*
    let I1 =
        Interface "I1"
        |+> [
                "test1" => T<string> ^-> T<string>
            ]

    let I2 =
        Generic -- fun t1 t2 ->
            Interface "I2"
            |+> [
                    Generic - fun m1 -> "foo" => m1 * t1 ^-> t2
                ]

    let C1 =
        Class "C1"
        |+> Instance [
                "foo" =@ T<int>
            ]
        |+> Static [
                Constructor (T<unit> + T<int>)
                "mem"   => (T<unit> + T<int> ^-> T<unit>)
                "test2" => (TSelf -* T<int> ^-> T<unit>) * T<string> ^-> T<string>
                "radius2" =? T<float>
                |> WithSourceName "R2"
            ]

    let Assembly =
        Assembly [
            Namespace "NCF.BPP.PowerBI.JSExtension" [
                 I1
                 I2
                 C1
            ]
        ]
*)
[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
