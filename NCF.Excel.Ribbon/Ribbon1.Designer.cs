namespace NCF.Excel.Ribbon
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonTab NCF;
            Microsoft.Office.Tools.Ribbon.RibbonMenu queryMenu;
            Microsoft.Office.Tools.Ribbon.RibbonButton queryButton;
            this.CitectAlarms = this.Factory.CreateRibbonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            NCF = this.Factory.CreateRibbonTab();
            queryMenu = this.Factory.CreateRibbonMenu();
            queryButton = this.Factory.CreateRibbonButton();
            NCF.SuspendLayout();
            this.CitectAlarms.SuspendLayout();
            this.SuspendLayout();
            // 
            // NCF
            // 
            NCF.Groups.Add(this.CitectAlarms);
            NCF.Label = "NCF";
            NCF.Name = "NCF";
            // 
            // CitectAlarms
            // 
            this.CitectAlarms.Items.Add(queryMenu);
            this.CitectAlarms.Items.Add(queryButton);
            this.CitectAlarms.Label = "Citect Alarms";
            this.CitectAlarms.Name = "CitectAlarms";
            // 
            // queryMenu
            // 
            queryMenu.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            queryMenu.Description = "Query Citect Alarm History";
            queryMenu.Items.Add(this.button1);
            queryMenu.Label = "Query";
            queryMenu.Name = "queryMenu";
            queryMenu.OfficeImageId = "AccountingFormat";
            queryMenu.ShowImage = true;
            // 
            // button1
            // 
            this.button1.Label = "button1";
            this.button1.Name = "button1";
            this.button1.ShowImage = true;
            // 
            // queryButton
            // 
            queryButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            queryButton.Label = "Last 24h";
            queryButton.Name = "queryButton";
            queryButton.ShowImage = true;
            queryButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.QueryLast24h);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(NCF);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            NCF.ResumeLayout(false);
            NCF.PerformLayout();
            this.CitectAlarms.ResumeLayout(false);
            this.CitectAlarms.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup CitectAlarms;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1 {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
