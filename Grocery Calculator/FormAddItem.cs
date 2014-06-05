using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grocery_Calculator
{
    public partial class FormAddItem : Form
    {
        Form myParent;
        public FormAddItem(Form parent)
        {
            InitializeComponent();
            txtName.Select();
            myParent = parent;
        }

        public FormAddItem(Form parent, GroceryItem item)
        {
            InitializeComponent();
            txtName.Select();
            myParent = parent;

            txtName.Text = item.Name;
            txtCost.Text = item.Cost.ToString();

            //select and deselect the txtCost to cause validation to occur
            txtCost.Select();
            txtName.Select();
            boxTax.Checked = item.Taxed;

            int payers = item.Payers;
            string payersString = ((Payer)payers).ToString();
            if (payersString.Contains("Ed"))
                boxEd.Checked = true;
            if (payersString.Contains("Matt"))
                boxMatt.Checked = true;
            if (payersString.Contains("Mel"))
                boxMel.Checked = true;
            if (payersString.Contains("Mike"))
                boxMike.Checked = true;


        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            decimal Cost;
            if (txtCost.Text == "")
                return;

            if (!decimal.TryParse(txtCost.Text, System.Globalization.NumberStyles.Currency, 
                System.Globalization.CultureInfo.CurrentCulture, out Cost))
            {
                toolStripStatusLabel1.Text = "Please enter in the format: ###.##";
                txtCost.SelectAll();
                System.Media.SystemSounds.Exclamation.Play();
                e.Cancel = true;
            }
            else
            {
                toolStripStatusLabel1.Text = "";
                txtCost.Text = "$" + Cost.ToString();
                
            }


        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar != (char)Keys.Back
            //    || e.KeyChar != (char)Keys.Left
            //    || e.KeyChar != (char)Keys.Right
            //    || e.KeyChar != (char)Keys.OemPeriod)
            //{
            //    e.Handled = !char.IsNumber(e.KeyChar);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                toolStripStatusLabel1.Text = "Enter an item name!";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            if (txtCost.Text == "")
            {
                toolStripStatusLabel1.Text = "Enter an item cost!";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            //make sure at least one checkbox is checked
            if (!boxEd.Checked && !boxMatt.Checked && !boxMel.Checked && !boxMatt.Checked)
            {
                toolStripStatusLabel1.Text = "Check at least one person!";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            decimal cost;
            decimal.TryParse(txtCost.Text.Substring(1), out cost);
            int payers = 0;
            if (boxEd.Checked) payers += (int)Payer.Ed;
            if (boxMatt.Checked) payers += (int)Payer.Matt;
            if (boxMel.Checked) payers += (int)Payer.Mel;
            if (boxMike.Checked) payers += (int)Payer.Mike;



            GroceryItem newItem = new GroceryItem(txtName.Text, cost, boxTax.Checked, payers);

            ((Form1)myParent).AddItem(newItem.GetListItem());
            this.Close();
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            txtName.Text = txtName.Text.Trim();
        }
    }
}
