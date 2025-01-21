using Accessibility;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;
using WKBSample.WBK;

namespace WKBSample
{
    public partial class Form1 : Form
    {
        private IWKBObject _wkbObject = null;
        private int _srid = 4326;
        private string sridTextBoxText;

        public Form1()
        {
            InitializeComponent();

            toolTip1.SetToolTip(isoWkbcheckBox, "Enable ISO WKB extened Heade:\n +1000 for Z\n + 2000 for M\n + 3000 for ZM.");
            toolTip1.SetToolTip(spatialListBox, "Right click the mouse to add spatial types");
            wkbBitsTextBox.Text = "Please add spatial types in the spatial list box.";
            spatialListBox.Items.Clear();

            InitContextMenu();

            sridTextBox.Text = _srid.ToString();
            sridTextBoxText = sridTextBox.Text;
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
            wkbListView.Items.Clear();
            wkbBitsTextBox.Text = "";
            if (_wkbObject == null)
            {
                wkbBitsTextBox.Text = "Please add spatial types in the spatial list box.";
                return;
            }

            WKBConfig config = new WKBConfig
            {
                Order = littleEndianRadioButton.Checked ? ByteOrder.LittleEndian : ByteOrder.BigEndian,
                //SRID = int.Parse(sridTextBox.Text),
                SRID = _srid,
                IsoWKB = isoWkbcheckBox.Checked,
                HasM = mCheckBox.Checked,
                HasSRID = sridCheckBox.Checked,
                HasZ = zCheckBox.Checked,
            };

            IList<BitsInfo> bitsInfos = new List<BitsInfo>();

            _wkbObject.GetBits(bitsInfos, config, true);

            foreach (BitsInfo info in bitsInfos)
            {
                ListViewItem item = new ListViewItem([info.Bytes, info.Info, BitUtils.HexToBinary(info.Bytes)]);
                wkbListView.Items.Add(item);
            }

            wkbBitsTextBox.Text = string.Join("", bitsInfos.Select(c => c.Bytes));
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
            wkbListView.Items.Clear();
            wkbBitsTextBox.Text = "Please add spatial types in the spatial list box.";
        }

        private void isoWkbcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBits();
        }

        private void sridTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(sridTextBox.Text, out int srid))
            {
                MessageBox.Show($"SIRD value '{sridTextBox.Text}' is not a valid value. SRID should be an integer.");
                sridTextBox.Text = sridTextBoxText;
                return;
            }
            else
            {
                sridTextBoxText = sridTextBox.Text;
            }
        }

        private void sridTextBox_Leave(object sender, EventArgs e)
        {
            int srid = int.Parse(sridTextBox.Text);
            if (_srid != srid)
            {
                _srid = srid;
                UpdateBits();
            }
        }
    }
}
