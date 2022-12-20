using System.Windows.Forms;

namespace CookedAssetSerializerGUI
{
    partial class ChkBoxDialog2Bool
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
            this.btnYes = new System.Windows.Forms.Button();
            this.chkBool1 = new System.Windows.Forms.CheckBox();
            this.chkBool2 = new System.Windows.Forms.CheckBox();
            this.btnNo = new System.Windows.Forms.Button();
            this.lbBoolDialog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnYes
            // 
            this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(73, 102);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(86, 29);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // chkBool1
            // 
            this.chkBool1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.chkBool1.AutoSize = true;
            this.chkBool1.Location = new System.Drawing.Point(155, 53);
            this.chkBool1.Name = "chkBool1";
            this.chkBool1.Size = new System.Drawing.Size(15, 14);
            this.chkBool1.TabIndex = 1;
            this.chkBool1.UseVisualStyleBackColor = true;
            this.chkBool1.CheckedChanged += new System.EventHandler(this.chkBool1_CheckedChanged);
            // 
            // chkBool2
            // 
            this.chkBool2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.chkBool2.AutoSize = true;
            this.chkBool2.Location = new System.Drawing.Point(155, 76);
            this.chkBool2.Name = "chkBool2";
            this.chkBool2.Size = new System.Drawing.Size(15, 14);
            this.chkBool2.TabIndex = 2;
            this.chkBool2.UseVisualStyleBackColor = true;
            this.chkBool2.CheckedChanged += new System.EventHandler(this.chkBool2_CheckedChanged);
            // 
            // btnNo
            // 
            this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(165, 102);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(86, 29);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // lbBoolDialog
            // 
            this.lbBoolDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbBoolDialog.AutoSize = true;
            this.lbBoolDialog.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbBoolDialog.Location = new System.Drawing.Point(0, 9);
            this.lbBoolDialog.Name = "lbBoolDialog";
            this.lbBoolDialog.Size = new System.Drawing.Size(0, 20);
            this.lbBoolDialog.TabIndex = 4;
            // 
            // ChkBoxDialog2Bool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 141);
            this.Controls.Add(this.lbBoolDialog);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.chkBool2);
            this.Controls.Add(this.chkBool1);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChkBoxDialog2Bool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnYes;
        private CheckBox chkBool1;
        private CheckBox chkBool2;
        private Button btnNo;
        private Label lbBoolDialog;
    }
}