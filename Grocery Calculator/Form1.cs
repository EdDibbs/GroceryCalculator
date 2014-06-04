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

        private void Form1_Load(object sender, EventArgs e)
        {
            listItems.Columns.Add("Item Name", 200);
            listItems.Columns.Add("Cost", 50, HorizontalAlignment.Right);
            listItems.Columns.Add("Taxed?",50,HorizontalAlignment.Center);
            listItems.Columns.Add("Payers", 140);
            listItems.View = View.Details;
            listItems.GridLines = true;
            listItems.FullRowSelect = true;
            labelCost2.Text = "";

            GroceryItem newItem = new GroceryItem("Strawberries", 3.70m, false, 3);
            listItems.Items.Add(newItem.GetListItem());


            string[] item1 = new string[4];
            item1[0] = "Really long item name, like seriously what is this long?";
            item1[1] = "$5.83";
            item1[2] = "Y";
            item1[3] = "Ed, Matt, Mel, Mike";

            string[] item2 = new string[4];
            item2[0] = "Bananas";
            item2[1] = "$2.37";
            item2[2] = "N";
            item2[3] = "Matt, Mel";

            ListViewItem item = new ListViewItem(item1);
            listItems.Items.Add(item);
            item = new ListViewItem(item2);
            listItems.Items.Add(item);
        }

        private void listItems_Validated(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal totalCost = 0;

            foreach (ListViewItem item in listItems.Items)
            {
                decimal itemCost = decimal.Parse(item.SubItems[1].Text.Replace("$",""));
                bool taxable = (item.SubItems[2].Text == "Y");

                if (taxable) itemCost *= 1 + TaxRate;

                totalCost += itemCost;
            }
            
            labelCost2.Text = "$" + totalCost.ToString("F");
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {

        }


    }//class Form1

    public class GroceryItem
    {
        public GroceryItem(string name, decimal cost, bool taxed, int payers)
        {
            Name = name;
            Cost = cost;
            Taxed = taxed;
            Payers = payers;
        }

        public string Name { get; private set; }
        public decimal Cost { get; private set; }
        public bool Taxed { get; private set; }
        public int Payers { get; private set; }

        public ListViewItem GetListItem()
        {
            string[] itemString = new string[4];

            itemString[0] = Name;
            itemString[1] = "$" + Cost.ToString();

            if (Taxed) itemString[2] = "Y";
            else itemString[2] = "N";

            itemString[3] = ((Payer)Payers).ToString();

            return new ListViewItem(itemString);
        }
    }
}
