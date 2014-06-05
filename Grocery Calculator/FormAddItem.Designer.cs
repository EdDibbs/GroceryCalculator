namespace Grocery_Calculator
{
    partial class FormAddItem
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.boxTax = new System.Windows.Forms.CheckBox();
            this.boxEd = new System.Windows.Forms.CheckBox();
            this.boxMatt = new System.Windows.Forms.CheckBox();
            this.boxMel = new System.Windows.Forms.CheckBox();
            this.boxMike = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Item Cost:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Who\'s paying? :";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(82, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(122, 20);
            this.txtName.TabIndex = 0;
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(82, 37);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(122, 20);
            this.txtCost.TabIndex = 1;
            this.txtCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCost_KeyPress);
            this.txtCost.Validating += new System.ComponentModel.CancelEventHandler(this.textBox2_Validating);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 235);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(236, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // boxTax
            // 
            this.boxTax.AutoSize = true;
            this.boxTax.Location = new System.Drawing.Point(10, 67);
            this.boxTax.Name = "boxTax";
            this.boxTax.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.boxTax.Size = new System.Drawing.Size(87, 17);
            this.boxTax.TabIndex = 2;
            this.boxTax.Text = "Taxable Item";
            this.boxTax.UseVisualStyleBackColor = true;
            // 
            // boxEd
            // 
            this.boxEd.AutoSize = true;
            this.boxEd.Location = new System.Drawing.Point(102, 97);
            this.boxEd.Name = "boxEd";
            this.boxEd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.boxEd.Size = new System.Drawing.Size(39, 17);
            this.boxEd.TabIndex = 3;
            this.boxEd.Text = "Ed";
            this.boxEd.UseVisualStyleBackColor = true;
            // 
            // boxMatt
            // 
            this.boxMatt.AutoSize = true;
            this.boxMatt.Location = new System.Drawing.Point(102, 120);
            this.boxMatt.Name = "boxMatt";
            this.boxMatt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.boxMatt.Size = new System.Drawing.Size(47, 17);
            this.boxMatt.TabIndex = 4;
            this.boxMatt.Text = "Matt";
            this.boxMatt.UseVisualStyleBackColor = true;
            // 
            // boxMel
            // 
            this.boxMel.AutoSize = true;
            this.boxMel.Location = new System.Drawing.Point(102, 143);
            this.boxMel.Name = "boxMel";
            this.boxMel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.boxMel.Size = new System.Drawing.Size(43, 17);
            this.boxMel.TabIndex = 5;
            this.boxMel.Text = "Mel";
            this.boxMel.UseVisualStyleBackColor = true;
            // 
            // boxMike
            // 
            this.boxMike.AutoSize = true;
            this.boxMike.Location = new System.Drawing.Point(102, 166);
            this.boxMike.Name = "boxMike";
            this.boxMike.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.boxMike.Size = new System.Drawing.Size(49, 17);
            this.boxMike.TabIndex = 6;
            this.boxMike.Text = "Mike";
            this.boxMike.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Add Item";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormAddItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 257);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.boxMike);
            this.Controls.Add(this.boxMel);
            this.Controls.Add(this.boxMatt);
            this.Controls.Add(this.boxEd);
            this.Controls.Add(this.boxTax);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtCost);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormAddItem";
            this.Text = "Add Item";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox boxTax;
        private System.Windows.Forms.CheckBox boxEd;
        private System.Windows.Forms.CheckBox boxMatt;
        private System.Windows.Forms.CheckBox boxMel;
        private System.Windows.Forms.CheckBox boxMike;
        private System.Windows.Forms.Button button1;
    }
}