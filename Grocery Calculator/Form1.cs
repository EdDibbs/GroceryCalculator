﻿using System;
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
        public bool SavedSinceModified = true;

        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog1.FileName = "Grocery List.txt";
            openFileDialog1.Filter = "Text files(*.txt)|*.txt";

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

            //GroceryItem newItem = new GroceryItem("Strawberries", 3.70m, false, 3);
            //listItems.Items.Add(newItem.GetListItem());
            //newItem = new GroceryItem("Dr. Pepper", 2.50m, true, (int)Payer.Ed);
            //listItems.Items.Add(newItem.GetListItem());
            //newItem = new GroceryItem("Pork", 8.35m, false, 15);
            //listItems.Items.Add(newItem.GetListItem());
            //newItem = new GroceryItem("Mountain Dew", 2.50m, true, (int)Payer.Mike);
            //listItems.Items.Add(newItem.GetListItem());


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
                int quantity = int.Parse(item.SubItems[1].Text);
                if (taxable) itemCost *= 1 + TaxRate;
                itemCost *= quantity;
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

                
                decimal splitCost = (itemCost * gItem.Quantity) / payerCount;

                if (EdPay) EdTotal += splitCost;
                if (MattPay) MattTotal += splitCost;
                if (MelPay) MelTotal += splitCost;
                if (MikePay) MikeTotal += splitCost;

            }

            decimal sumOfIndividualCosts = EdTotal + MattTotal + MelTotal + MikeTotal;
            if (sumOfIndividualCosts > totalCost + 1
                || sumOfIndividualCosts < totalCost - 1)
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
            SavedSinceModified = false;
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

            //Get the item to be edited and send it back to the AddItem form
            ListViewItem selectedItem = listItems.SelectedItems[0];
            int selectedItemIndex = selectedItem.Index;
            GroceryItem selectedGroceryItem = new GroceryItem (selectedItem);
            FormAddItem addItemForm = new FormAddItem(this, selectedGroceryItem);

            DialogResult result = addItemForm.ShowDialog();
            
            if (result == System.Windows.Forms.DialogResult.OK)
                listItems.Items.Remove(selectedItem);

            //since our new item is in the last position of the list,
            //move it back to where it was
            ListViewItem editedItem = addItemForm.getLastItemAdded();

            listItems.Items.Remove(editedItem);
            listItems.Items.Insert(selectedItemIndex, editedItem);

            SavedSinceModified = false;
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            int index = 0; 
            if (listItems.SelectedItems.Count != 0)
            {
                index = listItems.Items.IndexOf(listItems.SelectedItems[0]);
                listItems.Items.Remove(listItems.SelectedItems[0]);
            }
            

            //listItems.Items[index].Selected = true;
            button1_Click(null, null);

            SavedSinceModified = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            saveFile(saveFileDialog1.FileName);    

            
        }

        private void saveFile(string filepath)
        {
            //open the file stream
            System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);
            string line;

            //loop through all the items
            for (int i = 0; i < listItems.Items.Count; i++)
            {
                line = "";
                ListViewItem item = listItems.Items[i];

                for (int j = 0; j < item.SubItems.Count; j++)
                {
                    line += item.SubItems[j].Text;
                    line += "\t";
                }
                //print out each item
                file.WriteLine(line);
            }

            file.Close();
            SavedSinceModified = true;
        }

        private void loadFile(string filepath)
        {
            //open the file stream
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            string line;
            string[] entries = new string[5];

            //clear our current list
            listItems.Items.Clear();

            //loop through all the items
            while (!file.EndOfStream)
            {
                line = file.ReadLine();
                entries = line.Split('\t');

                string name = entries[0];

                int quantity;
                int.TryParse(entries[1], out quantity);

                decimal cost;
                decimal.TryParse( entries[2].TrimStart('$'), out cost );

                bool taxed;
                if (entries[3] == "Y") taxed = true;
                else taxed = false;

                string payersString = entries[4];
                int payersInt = 0;

                if (payersString.Contains("Ed"))
                {
                    payersInt += (int)Payer.Ed;
                }
                if (payersString.Contains("Matt"))
                {
                    payersInt += (int)Payer.Matt;
                }
                if (payersString.Contains("Mel"))
                {
                    payersInt += (int)Payer.Mel;
                }
                if (payersString.Contains("Mike"))
                {
                    payersInt += (int)Payer.Mike;
                }


                GroceryItem item = new GroceryItem(name, cost, taxed, payersInt, quantity);
                AddItem(item.GetListItem());
            }


            file.Close();
            

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SavedSinceModified)
            {
                var promptResult = MessageBox.Show("Save file?", "Do you want to save your current list first?"
                    , MessageBoxButtons.YesNoCancel);

                if (promptResult == DialogResult.Cancel)
                { return; }
                else if (promptResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(null, null);
                }

            }

            DialogResult result = openFileDialog1.ShowDialog();
            loadFile(openFileDialog1.FileName);

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

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
