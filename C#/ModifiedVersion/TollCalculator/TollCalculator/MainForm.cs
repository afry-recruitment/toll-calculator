using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;


namespace TollCalculator
{
    public partial class Form1 : Form
    {
        int fee=0;
        private List<DateTime> dateTimes = new List<DateTime>();
        int selectedIndex;

        public Form1()
        {
            InitializeComponent();
            InitializeGUI();
        }

        public void InitializeGUI()
        {
            this.Text = "Toll Calculator";

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss tt";
            dateTimePicker1.ShowUpDown = true;

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss tt";
            dateTimePicker2.ShowUpDown = true;

            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy-MM-dd HH:mm:ss tt";
            dateTimePicker3.ShowUpDown = true;

            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "yyyy-MM-dd HH:mm:ss tt";
            dateTimePicker4.ShowUpDown = true;

            dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker5.CustomFormat = "yyyy-MM-dd HH:mm:ss tt";
            dateTimePicker5.ShowUpDown = true;

            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;
            dateTimePicker4.Enabled = false;
            dateTimePicker5.Enabled = false;

            comboBoxVehicle.DataSource = Enum.GetValues(typeof(VehicleType));
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            TollCalculator tollCalculator = new TollCalculator();

            OtherVehicles otherVehicle = new OtherVehicles();

            //  first check if a Vehicle is Selected
            if (comboBoxVehicle.SelectedIndex == -1)
            {
                lbltotalFee.Text = "Nothing was selected!";
            }
            else  // then check the number of fees to send right amount of fee dates
            {
                checkSelectedNumberOfFees();
                string input = comboBoxVehicle.SelectedItem.ToString();
                if (input.Equals("Car"))
                {
                    Car car = new Car();

                    fee = tollCalculator.GetTollFee(car, dateTimes);
                }
                else
                {
                    if (input.Equals("Motorbike"))
                    {
                        // Motorbike class can be removed and be added as OtherVehicle since it is included in the Toll Free Vehicles
                        //Motorbike motorbike = new Motorbike();
                        //fee = tollCalculator.GetTollFee(motorbike, dateTimes);

                        otherVehicle.VehicleType = "Motorbike";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);
                    }
                    else if (input.Equals("Tractor"))
                    {
                        otherVehicle.VehicleType = "Tractor";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);

                    }
                    else if (input.Equals("Emergency"))
                    {
                        otherVehicle.VehicleType = "Emergency";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);
                    }
                    else if (input.Equals("Diplomat"))
                    {
                        otherVehicle.VehicleType = "Diplomat";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);

                    }
                    else if (input.Equals("Foreign"))
                    {
                        otherVehicle.VehicleType = "Foreign";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);

                    }
                    else if (input.Equals("Military"))
                    {
                        otherVehicle.VehicleType = "Military";
                        fee = tollCalculator.GetTollFee(otherVehicle, dateTimes);
                    }

                }
                lbltotalFee.Text = "Total fee: " + fee+" for "+ input;
                writeResultToTextbox();
            }

        }

        /*
         * Check selected number of fees
         * First remove all the elements from the dateTime list then populate the list based on the number of fees
         */
        private void checkSelectedNumberOfFees()
        {
            dateTimes.Clear();

            if (selectedIndex == 0 || selectedIndex == 1)
            {
                var parsedDate1 = DateTime.Parse(dateTimePicker1.Value.ToString());
                dateTimes.Add(parsedDate1);

            }
            else if (selectedIndex == 2)
            {
                var parsedDate1 = DateTime.Parse(dateTimePicker1.Value.ToString());
                var parsedDate2 = DateTime.Parse(dateTimePicker2.Value.ToString());
                dateTimes.Add(parsedDate1);
                dateTimes.Add(parsedDate2);
            }
            else if (selectedIndex == 3)
            {
                var parsedDate1 = DateTime.Parse(dateTimePicker1.Value.ToString());
                var parsedDate2 = DateTime.Parse(dateTimePicker2.Value.ToString());
                var parsedDate3 = DateTime.Parse(dateTimePicker3.Value.ToString());
                dateTimes.Add(parsedDate1);
                dateTimes.Add(parsedDate2);
                dateTimes.Add(parsedDate3);
            }
            else if (selectedIndex == 4)
            {
                var parsedDate1 = DateTime.Parse(dateTimePicker1.Value.ToString());
                var parsedDate2 = DateTime.Parse(dateTimePicker2.Value.ToString());
                var parsedDate3 = DateTime.Parse(dateTimePicker3.Value.ToString());
                var parsedDate4 = DateTime.Parse(dateTimePicker4.Value.ToString());
                dateTimes.Add(parsedDate1);
                dateTimes.Add(parsedDate2);
                dateTimes.Add(parsedDate3);
                dateTimes.Add(parsedDate4);
            }
            else if (selectedIndex == 5)
            {
                var parsedDate1 = DateTime.Parse(dateTimePicker1.Value.ToString());
                var parsedDate2 = DateTime.Parse(dateTimePicker2.Value.ToString());
                var parsedDate3 = DateTime.Parse(dateTimePicker3.Value.ToString());
                var parsedDate4 = DateTime.Parse(dateTimePicker4.Value.ToString());
                var parsedDate5 = DateTime.Parse(dateTimePicker5.Value.ToString());
                dateTimes.Add(parsedDate1);
                dateTimes.Add(parsedDate2);
                dateTimes.Add(parsedDate3);
                dateTimes.Add(parsedDate4);
                dateTimes.Add(parsedDate5);
            }
        }
        /*
         * Check the selected Index and write the result to textBox result
         */
        private void writeResultToTextbox()
        {
            if (selectedIndex == 0 || selectedIndex == 1)
            {
                textBoxResult.Text = "First fee at: "+dateTimePicker1.Value.ToString();
            }
            else if(selectedIndex == 2)
            {
                textBoxResult.Text = "First fee at: " + dateTimePicker1.Value.ToString() + Environment.NewLine
                    +"Second fee at: "+dateTimePicker2.Value.ToString();
            }
            else if (selectedIndex == 3)
            {
                textBoxResult.Text = "First fee at: " + dateTimePicker1.Value.ToString() + Environment.NewLine
                   + "Second fee at: " + dateTimePicker2.Value.ToString() + Environment.NewLine +
                   "Third fee at: " + dateTimePicker3.Value.ToString() ;
            }
            else if (selectedIndex == 4)
            {
                textBoxResult.Text = "First fee at: " + dateTimePicker1.Value.ToString() + Environment.NewLine
                   + "Second fee at: " + dateTimePicker2.Value.ToString() + Environment.NewLine +
                   "Third fee at: " + dateTimePicker3.Value.ToString() + Environment.NewLine +
                   "Fourth fee at: " + dateTimePicker4.Value.ToString();
            }
            else if (selectedIndex == 5)
            {
                textBoxResult.Text = "First fee at: " + dateTimePicker1.Value.ToString() + Environment.NewLine
                   + "Second fee at: " + dateTimePicker2.Value.ToString() + Environment.NewLine +
                   "Third fee at: " + dateTimePicker3.Value.ToString() + Environment.NewLine +
                   "Fourth fee at: " + dateTimePicker4.Value.ToString() + Environment.NewLine +
                   "Fifth fee at: " + dateTimePicker5.Value.ToString() ;
            }
        }

        /*
         * Reset the fee, result textbox and total fee label 
         */
        private void btnReset_Click(object sender, EventArgs e)
        {
            fee = 0;
            lbltotalFee.Text = "Total fee: "+fee;
            textBoxResult.Text = "";
        }

        private void comboBoxNbrOfFees_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = comboBoxNbrOfFees.SelectedIndex+1;
            if (selectedIndex == 1)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;
                dateTimePicker5.Enabled = false;

            }
            else if(selectedIndex == 2)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;
                dateTimePicker5.Enabled = false;
            }
            else if(selectedIndex == 3)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = false;
                dateTimePicker5.Enabled = false;
            }
            else if(selectedIndex == 4)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
                dateTimePicker5.Enabled = false;
            }
            else if(selectedIndex == 5)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
                dateTimePicker5.Enabled = true;
            }
        }
    }
}
