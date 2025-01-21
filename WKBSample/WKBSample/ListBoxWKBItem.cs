using System.Text;
using WKBSample.WBK;

namespace WKBSample;

internal class ListBoxWKBItem
{
    public int Indent { get; set; }
    public IWKBObject WkbObject { get; set; }
    public ContextMenuStrip ContextMenuStrip { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(Indent * 4 + 2);
        for (int i = 0; i < Indent; i++)
        {
            if (i != Indent - 1)
            {
                sb.Append("    ");
            }
            else
            {
                sb.Append("    |-");
            }
        }

        return $"{sb.ToString()} {WkbObject.ToString()}";
    }
}
