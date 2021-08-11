using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditPaymentSchedule
{
    public partial class Form2 : Form
    {
        public IndividualCreditTerms ICT;
        private bool flagValue, flagRate, flagFirstPay, flagComis;

        public Form2()
        {
            InitializeComponent();
            
            flagValue = false;
            flagRate = false;
            flagFirstPay = false;
            flagComis = false;
            this.comboBoxFirstPay.Enabled = false;
            this.textBoxFirstPay.Enabled = false;
            this.comboBoxComis.Enabled = false;
            this.textBoxComis.Enabled = false;
            this.label6.Enabled = false;
            this.comboBoxTermsComis.Enabled = false;

            int step = 12;
            for (int i = 0; i < 75; i += 3)
            {
                this.comboBoxTerms.Items.Add(step + i);
            }
            ICT = new IndividualCreditTerms();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void checkBoxFirstPay_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxFirstPay_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkBoxComis_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxComis_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxValue_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBoxRate_Leave(object sender, EventArgs e)
        {
            
        }

        private void comboBoxTerms_Leave(object sender, EventArgs e)
        {
            
        }

        private void comboBoxBodyPay_Leave(object sender, EventArgs e)
        {
            
        }

        private void comboBoxPercentPay_Leave(object sender, EventArgs e)
        {
            
        }

        private void comboBoxTermsComis_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBoxFirstPay_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBoxComis_Leave(object sender, EventArgs e)
        {
            
        }

        private void comboBox_Click(object sender, EventArgs e) // распахивание комбобокса при клике на нем
        {
           
        }
    }

    // хранить индивидуальные параметры кредита введенные в текстбоксы
    public class IndividualCreditTerms
    {
        public decimal rate { get; set; }
        public decimal pay { get; set; }
        public decimal comis { get; set; }
    }
}