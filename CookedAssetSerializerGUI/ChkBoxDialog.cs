using CookedAssetSerializerGUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using CheckBox = System.Windows.Forms.CheckBox;

namespace CookedAssetSerializerGUI
{
    public partial class ChkBoxDialog2Bool : Form
    {
        public ChkBoxDialog2Bool(string dTitle, string DialogCaption, string chk1Caption, string chk2Caption)
        {
            InitializeComponent();
            Text = dTitle;
            lbBoolDialog.Text = DialogCaption;
            chkBool1.Text = chk1Caption;
            chkBool2.Text = chk2Caption;
            lbBoolDialog.Left = (this.ClientSize.Width - (lbBoolDialog.Width)) / 2;
            chkBool1.Left = (this.ClientSize.Width - (chkBool1.Width)) / 2;
            chkBool2.Left = (this.ClientSize.Width - (chkBool2.Width)) / 2;
        }

        public bool b1Dialog
        {
            get { return chkBool1.Checked; }
            set { chkBool1.Checked = value; }
        }


        public bool b2Dialog
        {
            get { return chkBool2.Checked; }
            set { chkBool2.Checked = value; }
        }


        public string chk1Caption;
        public string chk2Caption;
        public string DialogCaption;
        public string dTitle;

        private void chkBool1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBool1.Checked == true)
            {
                b1Dialog = true;
            }
            else
            {
                b1Dialog = false;
            }
        }

        private void chkBool2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBool2.Checked == true)
            {
                b2Dialog = true;
            }
            else
            {
                b2Dialog = false;
            }
        }
    }
}
