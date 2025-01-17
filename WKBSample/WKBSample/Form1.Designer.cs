namespace WKBSample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            sridCheckBox = new CheckBox();
            mCheckBox = new CheckBox();
            zCheckBox = new CheckBox();
            groupBox1 = new GroupBox();
            sridTextBox = new TextBox();
            label5 = new Label();
            bigEndianRadioButton = new RadioButton();
            isoWkbcheckBox = new CheckBox();
            littleEndianRadioButton = new RadioButton();
            spatialListBox = new ListBox();
            wkbBytesListBox = new ListBox();
            wkbBitsTextBox = new TextBox();
            toolTip1 = new ToolTip(components);
            clearAllBtn = new Button();
            label1 = new Label();
            wkbListView = new ListView();
            hexColumnHeader = new ColumnHeader();
            detailsColumnHeader = new ColumnHeader();
            binaryColumnHeader = new ColumnHeader();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // sridCheckBox
            // 
            sridCheckBox.AutoSize = true;
            sridCheckBox.Checked = true;
            sridCheckBox.CheckState = CheckState.Checked;
            sridCheckBox.Location = new Point(20, 37);
            sridCheckBox.Name = "sridCheckBox";
            sridCheckBox.Size = new Size(50, 19);
            sridCheckBox.TabIndex = 7;
            sridCheckBox.Text = "SRID";
            sridCheckBox.UseVisualStyleBackColor = true;
            sridCheckBox.CheckedChanged += sridCheckBox_CheckedChanged;
            // 
            // mCheckBox
            // 
            mCheckBox.AutoSize = true;
            mCheckBox.Checked = true;
            mCheckBox.CheckState = CheckState.Checked;
            mCheckBox.Location = new Point(20, 87);
            mCheckBox.Name = "mCheckBox";
            mCheckBox.Size = new Size(37, 19);
            mCheckBox.TabIndex = 7;
            mCheckBox.Text = "M";
            mCheckBox.UseVisualStyleBackColor = true;
            mCheckBox.CheckedChanged += mCheckBox_CheckedChanged;
            // 
            // zCheckBox
            // 
            zCheckBox.AutoSize = true;
            zCheckBox.Checked = true;
            zCheckBox.CheckState = CheckState.Checked;
            zCheckBox.Location = new Point(20, 62);
            zCheckBox.Name = "zCheckBox";
            zCheckBox.Size = new Size(33, 19);
            zCheckBox.TabIndex = 7;
            zCheckBox.Text = "Z";
            zCheckBox.UseVisualStyleBackColor = true;
            zCheckBox.CheckedChanged += zCheckBox_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(sridTextBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(bigEndianRadioButton);
            groupBox1.Controls.Add(isoWkbcheckBox);
            groupBox1.Controls.Add(littleEndianRadioButton);
            groupBox1.Controls.Add(sridCheckBox);
            groupBox1.Controls.Add(zCheckBox);
            groupBox1.Controls.Add(mCheckBox);
            groupBox1.Location = new Point(12, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(515, 163);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // sridTextBox
            // 
            sridTextBox.Location = new Point(329, 110);
            sridTextBox.Name = "sridTextBox";
            sridTextBox.Size = new Size(163, 23);
            sridTextBox.TabIndex = 9;
            sridTextBox.Text = "4326";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(278, 111);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 10;
            label5.Text = "SRID:";
            // 
            // bigEndianRadioButton
            // 
            bigEndianRadioButton.AutoSize = true;
            bigEndianRadioButton.Location = new Point(405, 62);
            bigEndianRadioButton.Name = "bigEndianRadioButton";
            bigEndianRadioButton.Size = new Size(78, 19);
            bigEndianRadioButton.TabIndex = 1;
            bigEndianRadioButton.Text = "BigEndian";
            bigEndianRadioButton.UseVisualStyleBackColor = true;
            bigEndianRadioButton.CheckedChanged += bigEndianRadioButton_CheckedChanged;
            // 
            // isoWkbcheckBox
            // 
            isoWkbcheckBox.AutoSize = true;
            isoWkbcheckBox.Checked = true;
            isoWkbcheckBox.CheckState = CheckState.Checked;
            isoWkbcheckBox.Location = new Point(20, 112);
            isoWkbcheckBox.Name = "isoWkbcheckBox";
            isoWkbcheckBox.Size = new Size(72, 19);
            isoWkbcheckBox.TabIndex = 8;
            isoWkbcheckBox.Text = "ISO WKB";
            isoWkbcheckBox.UseVisualStyleBackColor = true;
            isoWkbcheckBox.CheckedChanged += isoWkbcheckBox_CheckedChanged;
            // 
            // littleEndianRadioButton
            // 
            littleEndianRadioButton.AutoSize = true;
            littleEndianRadioButton.Checked = true;
            littleEndianRadioButton.Location = new Point(405, 36);
            littleEndianRadioButton.Name = "littleEndianRadioButton";
            littleEndianRadioButton.Size = new Size(87, 19);
            littleEndianRadioButton.TabIndex = 0;
            littleEndianRadioButton.TabStop = true;
            littleEndianRadioButton.Text = "LittleEndian";
            littleEndianRadioButton.UseVisualStyleBackColor = true;
            littleEndianRadioButton.CheckedChanged += littleEndianRadioButton_CheckedChanged;
            // 
            // spatialListBox
            // 
            spatialListBox.Font = new Font("Segoe UI", 12F);
            spatialListBox.FormattingEnabled = true;
            spatialListBox.ItemHeight = 21;
            spatialListBox.Items.AddRange(new object[] { "1", "2" });
            spatialListBox.Location = new Point(1343, 84);
            spatialListBox.Name = "spatialListBox";
            spatialListBox.Size = new Size(452, 466);
            spatialListBox.TabIndex = 12;
            spatialListBox.MouseDown += spatialListBox_MouseDown;
            // 
            // wkbBytesListBox
            // 
            wkbBytesListBox.FormattingEnabled = true;
            wkbBytesListBox.ItemHeight = 15;
            wkbBytesListBox.Items.AddRange(new object[] { "1", "2" });
            wkbBytesListBox.Location = new Point(12, 325);
            wkbBytesListBox.Name = "wkbBytesListBox";
            wkbBytesListBox.Size = new Size(247, 454);
            wkbBytesListBox.TabIndex = 12;
            // 
            // wkbBitsTextBox
            // 
            wkbBitsTextBox.Location = new Point(12, 785);
            wkbBitsTextBox.Multiline = true;
            wkbBitsTextBox.Name = "wkbBitsTextBox";
            wkbBitsTextBox.Size = new Size(987, 61);
            wkbBitsTextBox.TabIndex = 13;
            // 
            // clearAllBtn
            // 
            clearAllBtn.Font = new Font("Segoe UI", 12F);
            clearAllBtn.Location = new Point(1521, 809);
            clearAllBtn.Name = "clearAllBtn";
            clearAllBtn.Size = new Size(233, 37);
            clearAllBtn.TabIndex = 4;
            clearAllBtn.Text = "Clear All";
            clearAllBtn.UseVisualStyleBackColor = true;
            clearAllBtn.Click += clearAllBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(79, 301);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 15;
            label1.Text = "Standard WKB";
            // 
            // wkbListView
            // 
            wkbListView.Columns.AddRange(new ColumnHeader[] { hexColumnHeader, detailsColumnHeader, binaryColumnHeader });
            wkbListView.GridLines = true;
            wkbListView.Location = new Point(376, 301);
            wkbListView.Name = "wkbListView";
            wkbListView.Size = new Size(809, 290);
            wkbListView.TabIndex = 16;
            wkbListView.UseCompatibleStateImageBehavior = false;
            wkbListView.View = View.Details;
            // 
            // hexColumnHeader
            // 
            hexColumnHeader.Text = "Hex";
            hexColumnHeader.Width = 120;
            // 
            // detailsColumnHeader
            // 
            detailsColumnHeader.Text = "Details";
            detailsColumnHeader.Width = 200;
            // 
            // binaryColumnHeader
            // 
            binaryColumnHeader.Text = "Binary";
            binaryColumnHeader.TextAlign = HorizontalAlignment.Right;
            binaryColumnHeader.Width = 450;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1952, 854);
            Controls.Add(wkbListView);
            Controls.Add(label1);
            Controls.Add(wkbBitsTextBox);
            Controls.Add(wkbBytesListBox);
            Controls.Add(spatialListBox);
            Controls.Add(groupBox1);
            Controls.Add(clearAllBtn);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckBox sridCheckBox;
        private CheckBox mCheckBox;
        private CheckBox zCheckBox;
        private GroupBox groupBox1;
        private ListBox spatialListBox;
        private ListBox wkbBytesListBox;
        private TextBox wkbBitsTextBox;
        private RadioButton bigEndianRadioButton;
        private RadioButton littleEndianRadioButton;
        private Label label5;
        private TextBox sridTextBox;
        private ToolTip toolTip1;
        private Button clearAllBtn;
        private Label label1;
        private CheckBox isoWkbcheckBox;
        private ListView wkbListView;
        private ColumnHeader hexColumnHeader;
        private ColumnHeader detailsColumnHeader;
        private ColumnHeader binaryColumnHeader;
    }
}
