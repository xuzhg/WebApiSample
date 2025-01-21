using WKBSample.WBK;

namespace WKBSample;

public partial class AddPoint : Form
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }
    public double M { get; private set; }

    public AddPoint()
    {
        InitializeComponent();
    }

    internal AddPoint(WKBPoint pt)
        : this()
    {
        xTextBox.Text = double.IsNaN(pt.X) ? "NaN" : pt.X.ToString();
        yTextBox.Text = double.IsNaN(pt.Y) ? "NaN" : pt.Y.ToString();
        zTextBox.Text = double.IsNaN(pt.Z) ? "NaN" : pt.Z.ToString();
        mTextBox.Text = double.IsNaN(pt.M) ? "NaN" : pt.M.ToString();
    }

    private void okButton_Click(object sender, EventArgs e)
    {
        // X
        if (!GetValue(xTextBox, out double x))
        {
            MessageBox.Show($"'{xTextBox.Text}' in 'x' text box is not a valid point value. X Ordinate is required and should be a valid double value. Please use 'N', 'NaN' or '-' for Empty X.");
            return;
        }

        X = x;

        // Y
        if (!GetValue(yTextBox, out double y))
        {
            MessageBox.Show($"'{yTextBox.Text}'  in 'y' text box  is not a valid point value. Y Ordinate is required and should be a valid double value. Please use 'N', 'NaN' or '-' for Empty Y.");
            return;
        }

        Y = y;

        // Z
        if (GetValue(zTextBox, out double z))
        {
            Z = z;
        }
        else
        {
            MessageBox.Show($"'{zTextBox.Text}' in Z Ordinate is not a valid point value. Please use 'N', 'NaN' or '-' for Empty Z.");
            return;
        }

        // M
        if (GetValue(mTextBox, out double m))
        {
            M = m;
        }
        else
        {
            MessageBox.Show($"'{mTextBox.Text}' in M Ordinate is not a valid point value. Please use 'N', 'NaN' or '-' for Empty M.");
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
        if (text.Equals("NaN", StringComparison.OrdinalIgnoreCase) ||
            text.Equals("N", StringComparison.OrdinalIgnoreCase) ||
            text.Equals("-", StringComparison.Ordinal))
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
}
