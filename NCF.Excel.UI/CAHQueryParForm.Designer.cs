namespace NCF.Excel.Forms
{
    partial class CAHQueryParForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxUG = new System.Windows.Forms.CheckBox();
            this.checkBoxOPD = new System.Windows.Forms.CheckBox();
            this.checkBoxWinder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.CustomFormat = "dd/MMM/yyyy HH:mm:ss";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(15, 64);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerTo.TabIndex = 0;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.CustomFormat = "dd/MMM/yyyy HH:mm:ss";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(15, 25);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerFrom.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(15, 112);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(140, 112);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // checkBoxUG
            // 
            this.checkBoxUG.AutoSize = true;
            this.checkBoxUG.Checked = true;
            this.checkBoxUG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUG.Location = new System.Drawing.Point(15, 90);
            this.checkBoxUG.Name = "checkBoxUG";
            this.checkBoxUG.Size = new System.Drawing.Size(42, 17);
            this.checkBoxUG.TabIndex = 6;
            this.checkBoxUG.Text = "UG";
            this.checkBoxUG.UseVisualStyleBackColor = true;
            // 
            // checkBoxOPD
            // 
            this.checkBoxOPD.AutoSize = true;
            this.checkBoxOPD.Checked = true;
            this.checkBoxOPD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOPD.Location = new System.Drawing.Point(80, 90);
            this.checkBoxOPD.Name = "checkBoxOPD";
            this.checkBoxOPD.Size = new System.Drawing.Size(49, 17);
            this.checkBoxOPD.TabIndex = 7;
            this.checkBoxOPD.Text = "OPD";
            this.checkBoxOPD.UseVisualStyleBackColor = true;
            // 
            // checkBoxWinder
            // 
            this.checkBoxWinder.AutoSize = true;
            this.checkBoxWinder.Checked = true;
            this.checkBoxWinder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWinder.Location = new System.Drawing.Point(144, 90);
            this.checkBoxWinder.Name = "checkBoxWinder";
            this.checkBoxWinder.Size = new System.Drawing.Size(71, 17);
            this.checkBoxWinder.TabIndex = 8;
            this.checkBoxWinder.Text = "WINDER";
            this.checkBoxWinder.UseVisualStyleBackColor = true;
            // 
            // CAHQueryParForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(228, 148);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxWinder);
            this.Controls.Add(this.checkBoxOPD);
            this.Controls.Add(this.checkBoxUG);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.dateTimePickerTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CAHQueryParForm";
            this.Text = "Citect Alarm History Query Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxUG;
        private System.Windows.Forms.CheckBox checkBoxOPD;
        private System.Windows.Forms.CheckBox checkBoxWinder;
    }
}