namespace NCF.Excel

open System.Runtime.InteropServices
open ExcelDna.Integration.CustomUI
open CitectAlarmHistory
open NCF.Excel.UI

//using Office = NetOffice.OfficeApi;
//using Excel = NetOffice.ExcelApi;
//using NetOffice.ExcelApi.Enums;
//using NetOffice.ExcelApi.Tools.Utils;

[<ComVisible(true)>]
type RibbonController() = 
    inherit ExcelRibbon()
    override this.GetCustomUI(ribbonId) =
        @"
        <customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>
            <ribbon>
            <tabs>
                <tab id='NCF' label='NCF'>
                    <group id='CitectAlarms' label='Citect Alarms'>
                        <button id='queryButton' imageMso='NewPage' onAction='QueryLast24h' label='Last 24h' size='large' />
                        <menu id='queryMenu' imageMso='NewPageLayout' description='Query Citect Alarm History' label='Query' size='large'>
                            <button id='queryByTime' onAction='QueryByTime' label='By Time...' />
                            <button id='queryByZone' onAction='QueryByZone' label='By Zone...' />
                        </menu>
                    </group>
                </tab>
            </tabs>
            </ribbon>
        </customUI>
        "
    member this.QueryByZone(control : IRibbonControl) =
       let ok, ug, opd, wi, tFrom, tTo = UI.GetQueryParameters(true)
       if ok then buildNewSheetFromQuery (Query (ug, opd, wi, tFrom, tTo)) else ()

    member this.QueryByTime(control : IRibbonControl) =
       let ok, _, _, _, tFrom, tTo = UI.GetQueryParameters()
       if ok then buildNewSheetFromQuery (ByTime (tFrom, tTo)) else ()

    member this.QueryLast24h(control : IRibbonControl) =
       buildNewSheetFromQuery Last24H
