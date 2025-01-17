using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WKBSample.WBK;

namespace WKBSample
{
    public partial class AddPoint : Form
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double? Z { get; private set; }
        public double? M { get; private set; }

        public AddPoint()
        {
            InitializeComponent();
        }

        internal AddPoint(WKBPoint pt)
            : this()
        {
            if (pt.IsEmpty)
            {
                emptyCheckBox.Checked = true;
            }

            xTextBox.Text = double.IsNaN(pt.X) ? "NaN" : pt.X.ToString();
            yTextBox.Text = double.IsNaN(pt.Y) ? "NaN" : pt.Y.ToString();
            zTextBox.Text = pt.Z == null ? "null" : pt.Z.Value.ToString();
            mTextBox.Text = pt.M == null ? "null" : pt.M.Value.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (emptyCheckBox.Checked)
            {
                X = double.NaN;
                Y = double.NaN;
                Z = null;
                M = null;
                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }

            // X
            if (!GetValue(xTextBox, out double x))
            {
                MessageBox.Show($"'{xTextBox.Text}' in 'x' text box is not a valid point value. X Ordinate is required and should be a valid double value.");
                return;
            }

            X = x;

            // Y
            if (!GetValue(yTextBox, out double y))
            {
                MessageBox.Show($"'{yTextBox.Text}'  in 'y' text box  is not a valid point value. Y Ordinate is required and should be a valid double value.");
                return;
            }

            Y = y;

            // Z
            if (GetValue(zTextBox, out double? z))
            {
                Z = z;
            }
            else
            {
                MessageBox.Show($"'{zTextBox.Text}' in Z Ordinate is not a valid point value. Z is optional, please leave it empty, or 'Nan' or 'null' for empty Z.");
                return;
            }

            // M?
            if (GetValue(mTextBox, out double? m))
            {
                M = m;
            }
            else
            {
                MessageBox.Show($"'{mTextBox.Text}' in M Ordinate is not a valid point value. M is optional, please leave it empty, or 'Nan' or 'null' for empty M.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private static bool GetValue(TextBox textBox, out double value)
        {
            value = double.NaN;
            string text = textBox.Text;
            if (text.Equals("NaN", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (double.TryParse(text, out double d))
            {
                value = d;
                return true;
            }

            return false;
        }

        private static bool GetValue(TextBox textBox, out double? value)
        {
            value = null;
            string text = textBox.Text;
            if (string.IsNullOrEmpty(text) || text.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (text.Equals("NaN", StringComparison.OrdinalIgnoreCase))
            {
                value = double.NaN;
                return true;
            }

            if (double.TryParse(text, out double d))
            {
                value = d;
                return true;
            }

            return false;
        }

        private void emptyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (emptyCheckBox.Checked)
            {
                xTextBox.Enabled = false;
                yTextBox.Enabled = false;
                zTextBox.Enabled = false;
                mTextBox.Enabled = false;
            }
            else
            {
                xTextBox.Enabled = true;
                yTextBox.Enabled = true;
                zTextBox.Enabled = true;
                mTextBox.Enabled = true;
            }
        }
    }
}
