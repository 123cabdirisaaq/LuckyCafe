using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe
{
    public partial class Form1 : Form
    {
        const double TEA_COST = 1.00;
        const double COFFEE_COST = 2.00;
        const double JUICE_COST = 3.00;

        double finalAmount;
        double discountValue;
        int luckyNumber;
        string prizeMessage;

        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstOrderList.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            try
            {
                StreamWriter fileWriter = File.AppendText("orders.txt");

                foreach (object order in lstOrderList.Items)
                {
                    fileWriter.WriteLine(order.ToString());
                }

                fileWriter.WriteLine(lblTotalPrice.Text);
                fileWriter.WriteLine(lblLuckyMessage.Text);

                fileWriter.Close();

                MessageBox.Show("Order saved.");
            }
            catch
            {
                MessageBox.Show("Error saving the file.");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("orders.txt") == false)
                {
                    MessageBox.Show("No file found.");
                    return;
                }

                lstOrderList.Items.Clear();

                StreamReader fileReader = File.OpenText("orders.txt");

                while (fileReader.EndOfStream == false)
                {
                    string record = fileReader.ReadLine();

                    lstOrderList.Items.Add(record);
                }

                fileReader.Close();
            }
            catch
            {
                MessageBox.Show("Error loading the file.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();

            txtQuantity.Clear();

            lstOrderList.Items.Clear();

            lblTotalPrice.Text = "";

            lblLuckyMessage.Text = "";

            chkStudentMember.Checked = false;

            finalAmount = 0;

            prizeMessage = "";
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string customer = txtCustomerName.Text.Trim();

                if (customer == "")
                {
                    MessageBox.Show("Enter customer name.");
                    return;
                }

                foreach (char character in customer)
                {
                    if (!char.IsLetter(character) && character != ' ')
                    {
                        MessageBox.Show("Customer name must contain letters only.");
                        txtCustomerName.Clear();
                        txtCustomerName.Focus();
                        return;
                    }
                }

                int drinkQuantity = Convert.ToInt32(txtQuantity.Text);

                if (drinkQuantity < 1)
                {
                    MessageBox.Show("Enter quantity greater than zero.");
                    return;
                }

                double selectedPrice = 0;
                string selectedDrink = "";

                if (rdoTea.Checked == true)
                {
                    selectedPrice = TEA_COST;
                    selectedDrink = "Tea";
                }
                else if (rdoCoffee.Checked == true)
                {
                    selectedPrice = COFFEE_COST;
                    selectedDrink = "Coffee";
                }
                else if (rdoJuice.Checked == true)
                {
                    selectedPrice = JUICE_COST;
                    selectedDrink = "Juice";
                }
                else
                {
                    MessageBox.Show("Choose a drink.");
                    return;
                }

                lstOrderList.Items.Clear();

                Random randomGenerator = new Random();

                int prizeNumber = 0;

                for (int index = 1; index <= drinkQuantity; index++)
                {
                    int randomNumber = randomGenerator.Next(1, 51);

                    lstOrderList.Items.Add(index + ") " + customer + " - " +
                        selectedDrink + " - Lucky: " + randomNumber + " - $" +
                        selectedPrice.ToString("F2"));

                    if (randomNumber == 7 || randomNumber == 17)
                    {
                        prizeNumber = randomNumber;
                    }
                }

                double originalAmount = selectedPrice * drinkQuantity;

                double savedAmount = 0;

                if (chkStudentMember.Checked == true)
                {
                    savedAmount = originalAmount * 0.10;
                }

                finalAmount = originalAmount - savedAmount;

                lstOrderList.Items.Add("------------------------");

                if (chkStudentMember.Checked == true)
                {
                    lstOrderList.Items.Add("Member Discount: 10%");
                }
                else
                {
                    lstOrderList.Items.Add("Member Discount: 0%");
                }

                lstOrderList.Items.Add("Total Before Discount: $" +
                    originalAmount.ToString("F2"));

                lstOrderList.Items.Add("Discount Amount: $ " +
                    savedAmount.ToString("F2"));

                lstOrderList.Items.Add("Total After Discount: $" +
                    finalAmount.ToString("F2"));

                if (prizeNumber != 0)
                {
                    prizeMessage = "You got a Free Cookie! (" +
                        prizeNumber + ")";
                }
                else
                {
                    prizeMessage = "No prize this time.";
                }

                lstOrderList.Items.Add("Lucky Message: " + prizeMessage);

                lblTotalPrice.Text = "Total Price: $" + finalAmount.ToString("F2");

                lblLuckyMessage.Text = prizeMessage;
            }
            catch
            {
                MessageBox.Show("Please enter a valid quantity.");
            }
        }
    }
}