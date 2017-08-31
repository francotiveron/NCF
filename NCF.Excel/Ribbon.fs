namespace NCF.Excel

open System.Runtime.InteropServices
open ExcelDna.Integration.CustomUI
open System.Reflection

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
                        <button id='queryCAHLast24hButton' imageMso='NewPage' onAction='QueryCAHLast24h' label='Last 24h' size='large' />
                        <menu id='queryCAHMenu' imageMso='NewPageLayout' description='Query Citect Alarm History' label='Query' size='large'>
                            <button id='queryCAHByTimeButton' onAction='QueryCAHByTime' label='By Time...' />
                            <button id='queryCAHByZoneButton' onAction='QueryCAHByZone' label='By Zone...' />
                        </menu>
                    </group>
                    <group id='Resources' label='Resources'>
                        <button id='abputButton' imageMso='Info' onAction='About' label='About'/>
                    </group>
                </tab>
            </tabs>
            </ribbon>
        </customUI>
        "
    member this.QueryCAHByZone(control : IRibbonControl) = CAH.getByZone()
    member this.QueryCAHByTime(control : IRibbonControl) = CAH.getByTime()
    member this.QueryCAHLast24h(control : IRibbonControl) = CAH.getLast24h()
    member this.About(control : IRibbonControl) = UI.About(Assembly.GetExecutingAssembly().GetName().Version.ToString());
