using Accessibility;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using WKBSample.WBK;

namespace WKBSample
{
    public partial class Form1 : Form
    {
        private WKBConfig _config = new WKBConfig();

        private Geometry _geometry = null;

        private IWKBObject _wkbObject = null;


        public Form1()
        {
            InitializeComponent();

            toolTip1.SetToolTip(isoWkbcheckBox, "Enable ISO WKB extened Heade:\n +1000 for Z\n + 2000 for M\n + 3000 for ZM.");
            toolTip1.SetToolTip(spatialListBox, "Right click the mouse to add spatial types");
            spatialListBox.Items.Clear();

            InitContextMenu();

            wkbListView.Items.Add(new ListViewItem(new string[] { "abc", "efg" }));
            wkbListView.Items.Add(new ListViewItem(new string[] { "abc", "efg" }));
        }

        private ContextMenuStrip spatialContextMenu = new ContextMenuStrip();
        private ContextMenuStrip pointContextMenu = new ContextMenuStrip();
        private ContextMenuStrip lineStringContextMenu = new ContextMenuStrip();
        private ContextMenuStrip polygonContextMenu = new ContextMenuStrip();
        private ContextMenuStrip multiPointContextMenu = new ContextMenuStrip();
        private ContextMenuStrip multiLineStringMenu = new ContextMenuStrip();
        private ContextMenuStrip multiPolygonContextMenu = new ContextMenuStrip();

        private void InitContextMenu()
        {
            var menuItemEdit = new ToolStripMenuItem("Edit Point");
            menuItemEdit.Click += EditOnePointCoordinateMenuItem_Click;
            pointContextMenu.Items.Add(menuItemEdit);

            // linestring context Menu
            menuItemEdit = new ToolStripMenuItem("Add Point");
            menuItemEdit.Click += AddOnePointCoordinateMenuItem_Click;
            lineStringContextMenu.Items.Add(menuItemEdit);
            menuItemEdit = new ToolStripMenuItem("Add Empty Point");
            menuItemEdit.Click += AddEmptyPointMenuItem_Click;
            lineStringContextMenu.Items.Add(menuItemEdit);

            // polygon context Menu
            menuItemEdit = new ToolStripMenuItem("Add Ring");
            menuItemEdit.Click += AddLineStringMenuItem_Click;
            polygonContextMenu.Items.Add(menuItemEdit);

            // Multipoint context Menu
            menuItemEdit = new ToolStripMenuItem("Add Point");
            menuItemEdit.Click += AddOnePointCoordinateMenuItem_Click;
            multiPointContextMenu.Items.Add(menuItemEdit);
            menuItemEdit = new ToolStripMenuItem("Add Empty Point");
            menuItemEdit.Click += AddEmptyPointMenuItem_Click;
            multiPointContextMenu.Items.Add(menuItemEdit);

            // MultiLineString Context Menu
            menuItemEdit = new ToolStripMenuItem("Add LineString");
            menuItemEdit.Click += AddLineStringMenuItem_Click;
            multiLineStringMenu.Items.Add(menuItemEdit);

            // MultiPolygon context Menu
            menuItemEdit = new ToolStripMenuItem("Add Polygon");
            menuItemEdit.Click += AddPolygonMenuItem_Click;
            multiPolygonContextMenu.Items.Add(menuItemEdit);

            // Spatial context Menu
            var addPointMenuItem = new ToolStripMenuItem("Add Point");
            addPointMenuItem.Click += AddPointMenuItem_Click;

            var addLineStringMenuItem = new ToolStripMenuItem("Add LineString");
            addLineStringMenuItem.Click += AddLineStringMenuItem_Click;

            var addPolygonMenuItem = new ToolStripMenuItem("Add Polygon");
            addPolygonMenuItem.Click += AddPolygonMenuItem_Click;

            var addMultiPointMenuItem = new ToolStripMenuItem("Add MultiPoint");
            addMultiPointMenuItem.Click += AddMultiPointMenuItem_Click;

            var addMultiLineStringMenuItem = new ToolStripMenuItem("Add MultiLineString");
            addMultiLineStringMenuItem.Click += AddMultiLineStringMenuItem_Click;

            var addMultiPolygonMenuItem = new ToolStripMenuItem("Add MultiPolygon");
            addMultiPolygonMenuItem.Click += AddMultiPolygonMenuItem_Click;

            var addCollectionMenuItem = new ToolStripMenuItem("Add Collection");
            addCollectionMenuItem.Click += AddCollectionMenuItem_Click;

            spatialContextMenu.Items.AddRange([addPointMenuItem, addLineStringMenuItem, addPolygonMenuItem, addMultiPointMenuItem, addMultiLineStringMenuItem, addMultiPolygonMenuItem, addCollectionMenuItem]);
        }

        private void EditOnePointCoordinateMenuItem_Click(object sender, EventArgs e)
        {
            ListBoxWKBItem item = spatialListBox.SelectedItem as ListBoxWKBItem;
            WKBPoint wkbPoint = (WKBPoint)item.WkbObject;

            AddPoint addPointDialog = new AddPoint(wkbPoint);
            DialogResult result = addPointDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                wkbPoint.X = addPointDialog.X;
                wkbPoint.Y = addPointDialog.Y;
                wkbPoint.Z = addPointDialog.Z;
                wkbPoint.M = addPointDialog.M;

                UpdateWKB();
            }
        }

        private void AddEmptyPointMenuItem_Click(Object sender, EventArgs e)
        {
            WKBPoint aNewPoint = new WKBPoint();
            ListBoxWKBItem item = spatialListBox.SelectedItem as ListBoxWKBItem;
            if (item.WkbObject.SpatialType == SpatialType.MultiPoint)
            {
                WKBMultiPoint mp = (WKBMultiPoint)(item.WkbObject);
                mp.Points.Add(aNewPoint);
            }
            else if (item.WkbObject.SpatialType == SpatialType.LineString)
            {
                WKBLineString ls = (WKBLineString)(item.WkbObject);
                ls.Points.Add(aNewPoint);
            }

            UpdateWKB();
        }

        private void AddOnePointCoordinateMenuItem_Click(object sender, EventArgs e)
        {
            AddPoint addPointDialog = new AddPoint();
            DialogResult result = addPointDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                WKBPoint aNewPoint = new WKBPoint
                {
                    X = addPointDialog.X,
                    Y = addPointDialog.Y,
                    Z = addPointDialog.Z,
                    M = addPointDialog.M
                };

                ListBoxWKBItem item = spatialListBox.SelectedItem as ListBoxWKBItem;
                if (item.WkbObject.SpatialType == SpatialType.MultiPoint)
                {
                    WKBMultiPoint mp = (WKBMultiPoint)(item.WkbObject);
                    mp.Points.Add(aNewPoint);
                }
                else if (item.WkbObject.SpatialType == SpatialType.LineString)
                {
                    WKBLineString ls = (WKBLineString)(item.WkbObject);
                    ls.Points.Add(aNewPoint);
                }

                UpdateWKB();
            }
        }

        private void AddPointMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.Point);
        }

        private void AddLineStringMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.LineString);
        }

        private void AddPolygonMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.Polygon);
        }

        private void AddMultiPointMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.MultiPoint);
        }

        private void AddMultiLineStringMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.MultiLineString);
        }

        private void AddMultiPolygonMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.MultiPolygon);
        }

        private void AddCollectionMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSpatialType(SpatialType.Collection);
        }

        private void AddNewSpatialType(SpatialType spatialType)
        {
            IWKBObject wkbObject = null;
            switch (spatialType)
            {
                case SpatialType.Point:
                    wkbObject = new WKBPoint();
                    break;
                case SpatialType.LineString:
                    wkbObject = new WKBLineString();
                    break;
                case SpatialType.Polygon:
                    wkbObject = new WKBPolygon();
                    break;
                case SpatialType.MultiPoint:
                    wkbObject = new WKBMultiPoint();
                    break;
                case SpatialType.MultiLineString:
                    wkbObject = new WKBMultiLineString();
                    break;
                case SpatialType.MultiPolygon:
                    wkbObject = new WKBMultiPolygon();
                    break;
                case SpatialType.Collection:
                default:
                    wkbObject = new WKBCollection();
                    break;
            }

            if (_wkbObject == null)
            {
                _wkbObject = wkbObject;
            }
            else
            {
                ListBoxWKBItem item = spatialListBox.SelectedItem as ListBoxWKBItem;
                if (item.WkbObject.SpatialType == SpatialType.Collection)
                {
                    WKBCollection parentCollect = (WKBCollection)item.WkbObject;
                    parentCollect.Items.Add(wkbObject);
                }
                else if (item.WkbObject.SpatialType == SpatialType.MultiPolygon && spatialType == SpatialType.Polygon)
                {
                    WKBMultiPolygon parentMultiPolygon = (WKBMultiPolygon)item.WkbObject;
                    parentMultiPolygon.Polygons.Add((WKBPolygon)wkbObject);
                }
                else if (item.WkbObject.SpatialType == SpatialType.MultiLineString && spatialType == SpatialType.LineString)
                {
                    WKBMultiLineString parentMultiLineString = (WKBMultiLineString)item.WkbObject;
                    parentMultiLineString.LineStrings.Add((WKBLineString)wkbObject);
                }
                else if (item.WkbObject.SpatialType == SpatialType.MultiPoint && spatialType == SpatialType.Point)
                {
                    WKBMultiPoint parentMultiPoint = (WKBMultiPoint)item.WkbObject;
                    parentMultiPoint.Points.Add((WKBPoint)wkbObject);
                }
                else if (item.WkbObject.SpatialType == SpatialType.LineString && spatialType == SpatialType.Point)
                {
                    WKBLineString parentLineString = (WKBLineString)item.WkbObject;
                    parentLineString.Points.Add((WKBPoint)wkbObject);
                }
                else if (item.WkbObject.SpatialType == SpatialType.Polygon && spatialType == SpatialType.LineString)
                {
                    WKBPolygon parentPolygon = (WKBPolygon)item.WkbObject;
                    parentPolygon.Rings.Add((WKBLineString)wkbObject);
                }
                else
                {
                    MessageBox.Show($"Something wrong! Try to add '{spatialType}' on '{item.WkbObject.SpatialType}'");
                    return;
                }
            }

            UpdateWKB();
        }


        private void Reset()
        {
            _wkbObject = null;
        }

        private void spatialListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (_wkbObject == null)
            {
                spatialContextMenu.Show(Cursor.Position);
                spatialContextMenu.Visible = true;
                return;
            }

            var index = spatialListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches && index == spatialListBox.SelectedIndex)
            {
                ListBoxWKBItem item = spatialListBox.Items[index] as ListBoxWKBItem;
                item.ContextMenuStrip.Show(Cursor.Position);
                item.ContextMenuStrip.Visible = true;
            }
            //else
            //{
            //    collectionRoundMenuStrip.Visible = false;
            //}
        }

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

        private void pointSpatialTypebtn_Click(object sender, EventArgs e)
        {
            //_geometry = 
            _wkbObject = new WKBPoint();
            spatialListBox.Items.Add(new ListBoxWKBItem { WkbObject = _wkbObject, ContextMenuStrip = pointContextMenu });
        }

        private void multiPointBtn_Click(object sender, EventArgs e)
        {
            _wkbObject = new WKBMultiPoint();
            spatialListBox.Items.Add(new ListBoxWKBItem { WkbObject = _wkbObject, ContextMenuStrip = multiPointContextMenu });
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void sridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }

        private void zCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }

        private void mCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }

        private void littleEndianRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }

        private void bigEndianRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }


        private void UpdateWKB()
        {
            spatialListBox.Items.Clear();

            InsertItemToListBox(_wkbObject, 0);

            UpdateBits();
        }

        private void UpdateBits()
        {
            wkbBytesListBox.Items.Clear();
            wkbBitsTextBox.Text = "";
            if (_wkbObject == null)
            {
                return;
            }

            //IList<string> byteLines = new List<string>();
            //StringBuilder sb = new StringBuilder();

            //WriteItemToByteListBox(_wkbObject, byteLines, sb, true);

            //foreach (var line in byteLines)
            //{
            //    wkbBytesListBox.Items.Add(line);
            //}
            //wkbBitsTextBox.Text = sb.ToString();

            WKBConfig config = new WKBConfig
            {
                Order = littleEndianRadioButton.Checked ? ByteOrder.LittleEndian : ByteOrder.BigEndian,
                SRID = int.Parse(sridTextBox.Text),
                IsoWKB = isoWkbcheckBox.Checked,
                HasM = mCheckBox.Checked,
                HasSRID = sridCheckBox.Checked,
                HasZ = zCheckBox.Checked,
            };

            wkbListView.Items.Clear();
            IList<BitsInfo> bitsInfos = new List<BitsInfo>();
     //       WriteItemToByteListBox(_wkbObject, bitsInfos, config, true);

            _wkbObject.GetBits(bitsInfos, config, true);

            foreach (BitsInfo info in bitsInfos)
            {
                ListViewItem item = new ListViewItem([info.Bytes, info.Info, BitUtils.HexToBinary(info.Bytes)]);
                wkbListView.Items.Add(item);

                string newBytes = info.Bytes.PadRight(20);

                string listBox = $"{newBytes} - {info.Info}";
                wkbBytesListBox.Items.Add(listBox);
            }

            wkbBitsTextBox.Text = string.Join("", bitsInfos.Select(c => c.Bytes));
        }

        private void WriteItemToByteListBox(IWKBObject wkbObject, IList<BitsInfo> byteLines, WKBConfig config, bool handSrid)
        {
            if (wkbObject == null)
            {
                return;
            }

            switch (wkbObject.SpatialType)
            {
                case SpatialType.Point:
                    wkbObject.GetBits(byteLines, config, handSrid);
                    break;

                case SpatialType.LineString:

                    break;

                case SpatialType.Polygon:

                    break;

                case SpatialType.MultiPoint:

                    break;

                case SpatialType.MultiLineString:


                    break;

                case SpatialType.MultiPolygon:

                    break;

                case SpatialType.Collection:

                    break;
            }
        }

        private void WriteItemToByteListBox(IWKBObject wkbObject, IList<string> byteLines, StringBuilder sb, bool handSrid)
        {
            if (wkbObject == null)
            {
                return;
            }

            switch (wkbObject.SpatialType)
            {
                case SpatialType.Point:
                    GetBits((WKBPoint)wkbObject, byteLines, sb, handSrid);
                    break;

                case SpatialType.LineString:


                    break;

                case SpatialType.Polygon:

                    break;

                case SpatialType.MultiPoint:

                    break;

                case SpatialType.MultiLineString:


                    break;

                case SpatialType.MultiPolygon:

                    break;

                case SpatialType.Collection:

                    break;
            }
        }

        private void GetBits(WKBPoint point, IList<string> byteLines, StringBuilder sb, bool handleSrid)
        {
            InsertByteOrder(byteLines, sb);
            byte[] headerBytes = GetHeader(SpatialType.Point, handleSrid);
            string headerStr = ToHex(headerBytes);

            string zHeader = "";
            if (zCheckBox.Checked)
            {
                if (isoWkbcheckBox.Checked)
                {
                    zHeader = "Z(ISO)";
                }
                else
                {
                    zHeader = "Z";
                }
            }

            string mHeader = "";
            if (mCheckBox.Checked)
            {
                if (isoWkbcheckBox.Checked)
                {
                    mHeader = "M(ISO)";
                }
                else
                {
                    mHeader = "M";
                }
            }

            byteLines.Add(headerStr + "          - Point " + zHeader + " " + mHeader);
            sb.Append(headerStr);


            InsertSRID(byteLines, sb, handleSrid);

            string xBits = GetDouble(point.X);
            byteLines.Add(xBits + $"  - x({point.X})");
            sb.Append(xBits);

            string yBits = GetDouble(point.Y);
            byteLines.Add(yBits + $"  - y({point.Y})");
            sb.Append(yBits);

            if (zCheckBox.Checked)
            {
                string zBits = GetDouble(point.Z);
                byteLines.Add(zBits + $"  - z({point.Z})");
                sb.Append(zBits);
            }

            if (mCheckBox.Checked)
            {
                string mBits = GetDouble(point.M);
                byteLines.Add(mBits + $"  - m({point.M})");
                sb.Append(mBits);
            }
        }

        private string GetDouble(double? d)
        {
            double dValue = d ?? double.NaN;
            if (littleEndianRadioButton.Checked)
            {
                return ToHex(BitConverter.GetBytes(dValue));
            }
            else
            {
                double newD = BitUtils.ReverseByteOrder(dValue);
                return ToHex(BitConverter.GetBytes(newD));
            }
        }

        private string GetDouble(double d)
        {
            if (littleEndianRadioButton.Checked)
            {
                return ToHex(BitConverter.GetBytes(d));
            }
            else
            {
                double newD = BitUtils.ReverseByteOrder(d);
                return ToHex(BitConverter.GetBytes(newD));
            }
        }

        private void InsertSRID(IList<string> byteLines, StringBuilder sb, bool handleSrid)
        {
            if (handleSrid)
            {
                if (sridCheckBox.Checked)
                {
                    int srid = int.Parse(sridTextBox.Text);

                    byte[] bytes;
                    if (littleEndianRadioButton.Checked)
                    {
                        bytes = BitConverter.GetBytes(srid);
                    }
                    else
                    {
                        int newValue = BinaryPrimitives.ReverseEndianness(srid);
                        bytes = BitConverter.GetBytes(newValue);
                    }

                    string sridStr = ToHex(bytes);
                    byteLines.Add(sridStr + $"          - SIRD ({srid})");
                    sb.Append(sridStr);
                }
            }
        }


        private void InsertByteOrder(IList<string> byteLines, StringBuilder sb)
        {
            byteLines.Add(littleEndianRadioButton.Checked ?
                "01                - LittleEndian" :
                "00                - BigEndian");

            sb.Append(littleEndianRadioButton.Checked ? "01" : "00");
        }

        private byte[] GetHeader(SpatialType type, bool handleSrid)
        {
            uint intSpatialType = (uint)type & 0xff;

            if (zCheckBox.Checked)
            {
                if (isoWkbcheckBox.Checked)
                {
                    intSpatialType += 1000;
                }

                intSpatialType |= 0x80000000;
            }

            if (mCheckBox.Checked)
            {
                if (isoWkbcheckBox.Checked)
                {
                    intSpatialType += 2000;
                }

                intSpatialType |= 0x40000000;
            }

            if (handleSrid)
            {
                if (sridCheckBox.Checked)
                {
                    intSpatialType |= 0x20000000;
                }
            }

            byte[] bytes = BitConverter.GetBytes(intSpatialType);
            if (littleEndianRadioButton.Checked)
            {
                return bytes;
            }
            else
            {
                uint newValue = BinaryPrimitives.ReverseEndianness(intSpatialType);
                return BitConverter.GetBytes(newValue);
            }
        }

        private void InsertItemToListBox(IWKBObject wkbObject, int indent)
        {
            if (wkbObject == null)
            {
                return;
            }

            switch (wkbObject.SpatialType)
            {
                case SpatialType.Point:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = pointContextMenu });
                    break;

                case SpatialType.LineString:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = lineStringContextMenu });
                    WKBLineString wkbLineString = wkbObject as WKBLineString;
                    foreach (var item in wkbLineString.Points)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }

                    break;

                case SpatialType.Polygon:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = polygonContextMenu });
                    WKBPolygon wkbPolygon = wkbObject as WKBPolygon;
                    foreach (var item in wkbPolygon.Rings)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }
                    break;

                case SpatialType.MultiPoint:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = multiPointContextMenu });
                    WKBMultiPoint wKBMultiPoint = wkbObject as WKBMultiPoint;
                    foreach (var item in wKBMultiPoint.Points)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }
                    break;

                case SpatialType.MultiLineString:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = multiLineStringMenu });
                    WKBMultiLineString wKBMultiLineString = wkbObject as WKBMultiLineString;
                    foreach (var item in wKBMultiLineString.LineStrings)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }

                    break;

                case SpatialType.MultiPolygon:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = multiPolygonContextMenu });
                    WKBMultiPolygon wKBMultiPolygon = wkbObject as WKBMultiPolygon;
                    foreach (var item in wKBMultiPolygon.Polygons)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }

                    break;

                case SpatialType.Collection:
                    spatialListBox.Items.Add(new ListBoxWKBItem { Indent = indent, WkbObject = wkbObject, ContextMenuStrip = spatialContextMenu });
                    WKBCollection collection = wkbObject as WKBCollection;
                    foreach (var item in collection.Items)
                    {
                        InsertItemToListBox(item, indent + 1);
                    }
                    break;
            }
        }

        private void clearAllBtn_Click(object sender, EventArgs e)
        {
            spatialListBox.Items.Clear();
            _wkbObject = null;
            wkbBytesListBox.Items.Clear();
            wkbListView.Items.Clear();
            wkbBitsTextBox.Text = "Please add spatial types in the spatial list box.";
        }

        public static string ToHex(byte[] bytes)
        {
            var buf = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                buf.Append(ToHexDigit((b >> 4) & 0x0F));
                buf.Append(ToHexDigit(b & 0x0F));
            }
            return buf.ToString();
        }

        private static char ToHexDigit(int n)
        {
            if (n < 0 || n > 15)
                throw new ArgumentException("Nibble value out of range: " + n);
            if (n <= 9)
                return (char)('0' + n);
            return (char)('A' + (n - 10));
        }

        private void isoWkbcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }
    }
}
