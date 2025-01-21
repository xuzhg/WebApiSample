namespace WKBSample
{
    partial class AddPoint
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
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            mTextBox = new TextBox();
            zTextBox = new TextBox();
            yTextBox = new TextBox();
            xTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 121);
            label4.Name = "label4";
            label4.Size = new Size(21, 15);
            label4.TabIndex = 15;
            label4.Text = "M:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 82);
            label3.Name = "label3";
            label3.Size = new Size(17, 15);
            label3.TabIndex = 16;
            label3.Text = "Z:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 49);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 17;
            label2.Text = "Y:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 20);
            label1.Name = "label1";
            label1.Size = new Size(17, 15);
            label1.TabIndex = 14;
            label1.Text = "X:";
            // 
            // mTextBox
            // 
            mTextBox.Location = new Point(69, 118);
            mTextBox.Name = "mTextBox";
            mTextBox.Size = new Size(163, 23);
            mTextBox.TabIndex = 10;
            mTextBox.Text = "NaN";
            // 
            // zTextBox
            // 
            zTextBox.Location = new Point(69, 82);
            zTextBox.Name = "zTextBox";
            zTextBox.Size = new Size(163, 23);
            zTextBox.TabIndex = 11;
            zTextBox.Text = "NaN";
            // 
            // yTextBox
            // 
            yTextBox.Location = new Point(69, 49);
            yTextBox.Name = "yTextBox";
            yTextBox.Size = new Size(163, 23);
            yTextBox.TabIndex = 12;
            yTextBox.Text = "0.0";
            // 
            // xTextBox
            // 
            xTextBox.Location = new Point(69, 12);
            xTextBox.Name = "xTextBox";
            xTextBox.Size = new Size(163, 23);
            xTextBox.TabIndex = 13;
            xTextBox.Text = "0.0";
            // 
            // okButton
            // 
            okButton.Location = new Point(164, 161);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 18;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(69, 161);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 18;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // AddPoint
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(251, 196);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(mTextBox);
            Controls.Add(zTextBox);
            Controls.Add(yTextBox);
            Controls.Add(xTextBox);
            Name = "AddPoint";
            Text = "AddPoint";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox mTextBox;
        private TextBox zTextBox;
        private TextBox yTextBox;
        private TextBox xTextBox;
        private Button okButton;
        private Button cancelButton;
    }
}