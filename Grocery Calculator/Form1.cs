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
    [Flags]
    public enum Payer { Ed = 0x01, Matt = 0x02, Mel = 0x04, Mike = 0x08}
    
    

    public partial class Form1 : Form
    {
        public decimal TaxRate = 0.07725m;

        public Form1()
        {
            InitializeComponent();

        }
        public void AddItem(ListViewItem item)
        {
            listItems.Items.Add(item);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listItems.Columns.Add("Item Name", 150);
            listItems.Columns.Add("#", 50, HorizontalAlignment.Center);
            listItems.Columns.Add("Cost", 50, HorizontalAlignment.Right);
            listItems.Columns.Add("Taxed?",50,HorizontalAlignment.Center);
            listItems.Columns.Add("Payers", 140);
            listItems.View = View.Details;
            listItems.GridLines = true;
            listItems.FullRowSelect = true;
            labelCost2.Text = "";
            lblEdCost.Text = "";
            lblMattCost.Text = "";
            lblMelCost.Text = "";
            lblMikeCost.Text = "";

            GroceryItem newItem = new GroceryItem("Strawberries", 3.70m, false, 3);
            listItems.Items.Add(newItem.GetListItem());
            newItem = new GroceryItem("Dr. Pepper", 2.50m, true, (int)Payer.Ed);
            listItems.Items.Add(newItem.GetListItem());
            newItem = new GroceryItem("Pork", 8.35m, false, 15);
            listItems.Items.Add(newItem.GetListItem());
            newItem = new GroceryItem("Mountain Dew", 2.50m, true, (int)Payer.Mike);
            listItems.Items.Add(newItem.GetListItem());

            //string[] item1 = new string[4];
            //item1[0] = "Really long item name, like seriously what is this long?";
            //item1[1] = "$15.83";
            //item1[2] = "Y";
            //item1[3] = "Ed, Matt, Mel, Mike";

            //string[] item2 = new string[4];
            //item2[0] = "Bananas";
            //item2[1] = "$2.37";
            //item2[2] = "N";
            //item2[3] = "Matt, Mel";

            //ListViewItem item = new ListViewItem(item1);
            //listItems.Items.Add(item);
            //item = new ListViewItem(item2);
            //listItems.Items.Add(item);
        }

        private void listItems_Validated(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal totalCost = 0;
            decimal EdTotal = 0, MattTotal = 0, MelTotal = 0, MikeTotal = 0;

            foreach (ListViewItem item in listItems.Items)
            {
                decimal itemCost = decimal.Parse(item.SubItems[2].Text.Replace("$",""));
                bool taxable = (item.SubItems[3].Text == "Y");

                if (taxable) itemCost *= 1 + TaxRate;

                totalCost += itemCost;
            }
            
            labelCost2.Text = "$" + totalCost.ToString("F");

            //loop through all the items
            for (int i = 0; i < listItems.Items.Count; i++)
            {
                ListViewItem item = listItems.Items[i];
                GroceryItem gItem = new GroceryItem(item);

                //calculate the cost of the item after tax
                decimal itemCost = gItem.Cost;
                if (gItem.Taxed)
                    itemCost *= 1 + TaxRate;
                
                //divide the cost of the item among all the people paying for it
                bool EdPay = false, MattPay = false, MelPay = false, MikePay = false;
                string payersString = ((Payer)gItem.Payers).ToString();
                int payerCount = 0;

                if (payersString.Contains("Ed"))
                {
                    payerCount++;
                    EdPay = true;
                }
                if (payersString.Contains("Matt"))
                {
                    payerCount++;
                    MattPay = true;
                }
                if (payersString.Contains("Mike"))
                {
                    payerCount++;
                    MikePay = true;
                }
                if (payersString.Contains("Mel"))
                {
                    payerCount++;
                    MelPay = true;
                }

                
                decimal splitCost = itemCost / payerCount;

                if (EdPay) EdTotal += splitCost;
                if (MattPay) MattTotal += splitCost;
                if (MelPay) MelTotal += splitCost;
                if (MikePay) MikeTotal += splitCost;

            }

            decimal sumOfIndividualCosts = EdTotal + MattTotal + MelTotal + MikeTotal;
            if (sumOfIndividualCosts > totalCost 
                /*|| sumOfIndividualCosts < totalCost - 1*/)
            {
                MessageBox.Show("Math went wrong somewhere. The sum of each person's share is "
                    + sumOfIndividualCosts.ToString() + " but it should equal the total cost, ie "
                    + totalCost.ToString() + ". We're off by $" 
                    + (Math.Abs(sumOfIndividualCosts - totalCost).ToString() )  );
            }

            

            lblEdCost.Text = "$"   + Math.Round(EdTotal,2).ToString();
            lblMattCost.Text = "$" + Math.Round(MattTotal,2).ToString();
            lblMelCost.Text = "$"  + Math.Round(MelTotal, 2).ToString();
            lblMikeCost.Text = "$" + Math.Round(MikeTotal, 2).ToString();


        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            FormAddItem addItemForm = new FormAddItem(this);
            addItemForm.ShowDialog();
            
        }

        private void labelCost2_TextChanged(object sender, EventArgs e)
        {
            //if we're out of the window's view we should nudge over
            if (labelCost2.Right > this.Width)
            {
                labelCost2.Left = this.Width - labelCost2.Width;
            }
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItems.Count == 0)
            {
                statusLabel.Text = "Must select an item to edit.";
                return;
            }
            ListViewItem selectedItem = listItems.SelectedItems[0];
            GroceryItem selectedGroceryItem = new GroceryItem (selectedItem);
            FormAddItem addItemForm = new FormAddItem(this, selectedGroceryItem);

            addItemForm.ShowDialog();
            
            listItems.Items.Remove(selectedItem);

            
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItems.Count != 0)
            {
                listItems.Items.Remove(listItems.SelectedItems[0]);
            }

            button1_Click(null, null);
        }


    }//class Form1

    public class GroceryItem
    {
        public GroceryItem(string name, decimal cost, bool taxed, int payers, int quantity = 1)
        {
            Name = name;
            Cost = cost;
            Quantity = quantity;
            Taxed = taxed;
            Payers = payers;
        }

        public GroceryItem(ListViewItem item)
        {
            Name = item.SubItems[0].Text;
            Quantity = int.Parse(item.SubItems[1].Text);
            Cost = decimal.Parse(item.SubItems[2].Text.Replace("$", ""));
            Taxed = (item.SubItems[3].Text == "Y");
            Payers = 0;
            string payersString = item.SubItems[4].Text;

            if (payersString.Contains("Ed"))
                Payers += (int)Payer.Ed;
            if (payersString.Contains("Matt"))
                Payers += (int)Payer.Matt;
            if (payersString.Contains("Mel"))
                Payers += (int)Payer.Mel;
            if (payersString.Contains("Mike"))
                Payers += (int)Payer.Mike;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Cost { get; private set; }
        public bool Taxed { get; private set; }
        public int Payers { get; private set; }

        public ListViewItem GetListItem()
        {
            string[] itemString = new string[5];

            itemString[0] = Name;
            itemString[1] = Quantity.ToString();
            itemString[2] = "$" + Cost.ToString();

            if (Taxed) itemString[3] = "Y";
            else itemString[3] = "N";

            itemString[4] = ((Payer)Payers).ToString();

            return new ListViewItem(itemString);
        }
    }
}
