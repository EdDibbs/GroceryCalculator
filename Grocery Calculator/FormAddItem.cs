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
        ListViewItem m_LastAddedItem;
        CheckBox[] m_Boxes;
        int m_NumPayers;

        public FormAddItem(Form parent)
        {
            InitializeComponent();
            txtName.Select();
            myParent = parent;
            m_LastAddedItem = null;
            
            m_NumPayers = ((Form1)myParent).NumPayers;
            m_Boxes = new CheckBox[m_NumPayers];

            for (int payer = 0; payer < m_NumPayers; payer++)
            {
                //set the text for the check boxes
                m_Boxes[payer] = new CheckBox();
                m_Boxes[payer].Text = ((Form1)myParent).Payers[payer];


                //set the position of the check boxes
                int xPos = 15 + ((payer / 4) * 107);
                int yPos = 151 + ((payer % 4) * 23);
                m_Boxes[payer].Location = new Point(xPos, yPos);


                this.Controls.Add(m_Boxes[payer]);
                
            }


        }
        public ListViewItem getLastItemAdded()
        {   
            return m_LastAddedItem;
        }

        //constructor for if we're actually editing an item
        public FormAddItem(Form parent, GroceryItem item) 
        {
            InitializeComponent();
            txtName.Select();
            myParent = parent;

            txtName.Text = item.Name;
            numQuant.Value = item.Quantity;
            txtCost.Text = item.Cost.ToString();

            //select and deselect the txtCost to cause validation to occur
            txtCost.Select();
            txtName.Select();
            boxTax.Checked = item.Taxed;

            int payerInt = item.Payers;

            //parse our payerInt bitstring to find which payers are paying
            for (int payerBit = 0; payerBit < m_NumPayers; payerBit++)
            {
                if ((payerInt & 1 << payerBit) != 0)
                {
                    m_Boxes[payerBit].Checked = true;
                }
            }


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

            int quantity = (int)numQuant.Value;

            if (txtCost.Text == "")
            {
                toolStripStatusLabel1.Text = "Enter an item cost!";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            //make sure at least one checkbox is checked
            bool oneChecked = false;
            foreach (CheckBox box in m_Boxes)
            {
                if (box.Checked)
                {
                    oneChecked = true;
                    break;
                }
            }
            if (!oneChecked)
            {
                toolStripStatusLabel1.Text = "Check at least one person!";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            decimal cost;
            decimal.TryParse(txtCost.Text.Substring(1), out cost);
            int payerInt = 0;

            for (int payer = 0; payer < m_NumPayers; payer++)
            {
                if (m_Boxes[payer].Checked)
                {
                    payerInt += (int)Math.Pow(2, payer);
                }
            }



            GroceryItem newItem = new GroceryItem(txtName.Text, cost, boxTax.Checked, payerInt, quantity);
            m_LastAddedItem = newItem.GetListItem(((Form1)myParent).Payers);
            ((Form1)myParent).AddItem(m_LastAddedItem);
            
            
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            txtName.Text = txtName.Text.Trim();
        }
    }
}
