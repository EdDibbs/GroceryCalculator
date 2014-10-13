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

    public partial class Form1 : Form
    {
        const int MAX_PAYERS = 8;
        public int NumPayers = 0;
        public const decimal TaxRate = 0.07725m;
        private bool SavedSinceModified = true;
        public string[] Payers = new string[MAX_PAYERS]; //string of their names
        private Label[] lblPayersTotalNames = new Label[MAX_PAYERS];
        private Label[] lblPayersTotalCost  = new Label[MAX_PAYERS];

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
            lblTotalCost.Text = "";

            for (int i = 0; i < MAX_PAYERS; i++)
            {
                Payers[i] = "";
                lblPayersTotalNames[i] = new Label();
                lblPayersTotalNames[i].Text = "[Name here]"; //"Foo:"
                lblPayersTotalNames[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                lblPayersTotalNames[i].Height = 17;

                //windows form location calculation
                int nameX = 369 - ((i / 4) * 92);
                int nameY = 297 + ((i % 4) * 18);
                lblPayersTotalNames[i].Location = new Point(nameX, nameY);

                this.Controls.Add(lblPayersTotalNames[i]);

                lblPayersTotalCost[i] = new Label();
                lblPayersTotalCost[i].Text = ""; //"$xxxx.xx
                lblPayersTotalCost[i].Location = new Point(nameX + 42, nameY);

                this.Controls.Add(lblPayersTotalCost[i]);

            }

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
            decimal totalCost = 0; //total grocery bill total
            decimal[] PayerTotal = new decimal[MAX_PAYERS]; //each payers individual contribution to the bill
            for (int i = 0; i < MAX_PAYERS; i++ )
            {
                PayerTotal[i] = 0;
            }


            //go through each item in our grocery list, calculate individual totals
            foreach (ListViewItem item in listItems.Items)
            {
                decimal itemCost = decimal.Parse(item.SubItems[2].Text.Replace("$", ""));
                bool taxable = (item.SubItems[3].Text == "Y");
                int quantity = int.Parse(item.SubItems[1].Text);
                if (taxable) itemCost *= 1 + TaxRate;
                itemCost *= quantity;
                totalCost += itemCost;
            }
            
            lblTotalCost.Text = "$" + totalCost.ToString("F");

            //loop through all the items
            for (int i = 0; i < listItems.Items.Count; i++)
            {
                ListViewItem item = listItems.Items[i];
                GroceryItem gItem = new GroceryItem(item, Payers);
                
                //calculate the cost of the item after tax

                decimal itemCost = gItem.Cost;
                
                if (gItem.Taxed)
                    itemCost *= 1 + TaxRate;

                bool[] payerPaying = new bool[MAX_PAYERS];
                for (int j = 0; j < MAX_PAYERS; j++)
                {
                    payerPaying[j] = false;
                }


                int iPayers = gItem.Payers;
                int payerCount = 0;

                //calculate the number of people paying for this item by
                //checking the number of 1 bits in the gItem.Payers int
                for (int payerBit = 0; payerBit < MAX_PAYERS; payerBit++)
                {
                    if ((iPayers & 1 << payerBit) != 0)
                    {
                        //if this payer is paying for this item
                        payerCount++;
                        payerPaying[payerBit] = true;
                    }
                }
              
  

                //total cost of the item divided by the number of payers paying for it
                decimal splitCost = (itemCost * gItem.Quantity) / payerCount;


                for (int payer = 0; payer < MAX_PAYERS; payer++)
                {
                    if (payerPaying[payer])
                    {
                        PayerTotal[payer] += splitCost;
                    }
                }

            }



            decimal sumOfIndividualCosts = 0;
            for (int payer = 0; payer < NumPayers; payer++)
            {
                sumOfIndividualCosts += PayerTotal[payer];

                lblPayersTotalCost[payer].Text = "$" + Math.Round(PayerTotal[payer], 2).ToString();
            }

            if (sumOfIndividualCosts > totalCost + 1
                || sumOfIndividualCosts < totalCost - 1)
            {
                MessageBox.Show("Math went wrong somewhere. The sum of each person's share is "
                    + sumOfIndividualCosts.ToString() + " but it should equal the total cost, ie "
                    + totalCost.ToString() + ". We're off by $" 
                    + (Math.Abs(sumOfIndividualCosts - totalCost).ToString() )  );
            }



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
            if (lblTotalCost.Right > this.Width)
            {
                lblTotalCost.Left = this.Width - lblTotalCost.Width;
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
            GroceryItem selectedGroceryItem = new GroceryItem (selectedItem, Payers);
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
            string line = "";


            //put a list of all our payer names as the first line
            //so we can have our names on our next load
            foreach (string name in Payers)
            {
                line += name + ",";
            }
            line.TrimEnd(',');

            file.WriteLine(line);

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

            //read in our payer names
            line = file.ReadLine();
            Payers = line.Split(',');
            NumPayers = Payers.Length;


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

                for (int i = 0; i < NumPayers; i++)
                {
                    payersInt += (int)Math.Pow(2, i);
                }


                GroceryItem item = new GroceryItem(name, cost, taxed, payersInt, quantity);
                AddItem(item.GetListItem(Payers));
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

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripTextBox1.Text == "")
                {
                    //error
                }
                else if (toolStripTextBox1.Text.Contains(","))
                {
                    //error
                }
                else if (NumPayers >= MAX_PAYERS)
                {
                    //error
                    MessageBox.Show("NO");
                }
                else
                {
                    Payers[NumPayers] = toolStripTextBox1.Text;

                    lblPayersTotalCost[NumPayers].Text = "";
                    lblPayersTotalNames[NumPayers].Text = toolStripTextBox1.Text;
                    lblPayersTotalNames[NumPayers].Show();

                    toolStripTextBox1.Clear();
                    addToolStripMenuItem.HideDropDown();
                    
                    NumPayers++;

                }
            }


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

        public GroceryItem(ListViewItem item, String[] PayerNames)
        {
            Name = item.SubItems[0].Text;
            Quantity = int.Parse(item.SubItems[1].Text);
            Cost = decimal.Parse(item.SubItems[2].Text.Replace("$", ""));
            Taxed = (item.SubItems[3].Text == "Y");
            Payers = 0;
            string payersString = item.SubItems[4].Text;


            for (int i = 0; i < PayerNames.Length; i++)
            {
                if (PayerNames[i].Length > 0)
                    Payers += (int)Math.Pow(2, i);
            }
            //if (payersString.Contains(Payers[payer].))
            //    Payers += (int)Payer.Ed;
            //if (payersString.Contains("Matt"))
            //    Payers += (int)Payer.Matt;
            //if (payersString.Contains("Mel"))
            //    Payers += (int)Payer.Mel;
            //if (payersString.Contains("Mike"))
            //    Payers += (int)Payer.Mike;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Cost { get; private set; }
        public bool Taxed { get; private set; }
        public int Payers { get; private set; }

        //you have to pass PayerNames to GetListItem so we know how to convert
        //the bitflag into each payer's name
        public ListViewItem GetListItem(string[] PayerNames)
        {
            string[] itemString = new string[5];

            itemString[0] = Name;
            itemString[1] = Quantity.ToString();
            itemString[2] = "$" + Cost.ToString();

            if (Taxed) itemString[3] = "Y";
            else itemString[3] = "N";

            string payerString = "";
            

            for (int payerBit = 0; payerBit < PayerNames.Count(); payerBit++)
            {
                if (PayerNames[payerBit].Length > 0)
                {
                    if ((Payers & 1 << payerBit) != 0)
                    {
                        //if this payer is paying for this item
                        payerString += PayerNames[payerBit] + ", ";
                    }
                }
                else
                    break;
            }
            payerString.TrimEnd(',', ' '); // we got an extra ", " at the end
            itemString[4] = payerString;

            return new ListViewItem(itemString);
        }
    }
}
