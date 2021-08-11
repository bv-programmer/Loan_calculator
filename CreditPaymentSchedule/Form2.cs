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
            if (this.checkBoxFirstPay.Checked)
                this.comboBoxFirstPay.Enabled = true;
            else
            {
                this.comboBoxFirstPay.SelectedIndex = -1;
                this.comboBoxFirstPay.Enabled = false;
                this.textBoxFirstPay.Enabled = false;
            }
        }

        private void comboBoxFirstPay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxFirstPay.SelectedIndex != -1)
                this.textBoxFirstPay.Enabled = true;
            else
                this.textBoxFirstPay.Enabled = false;
        }

        private void checkBoxComis_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxComis.Checked)
            {
                this.comboBoxComis.Enabled = true;
                this.label6.Enabled = true;
                this.comboBoxTermsComis.Enabled = true;
            }
            else
            {
                this.comboBoxComis.SelectedIndex = -1;
                this.comboBoxComis.Enabled = false;
                this.textBoxComis.Enabled = false;

                this.label6.Enabled = false;
                this.comboBoxTermsComis.Enabled = false;
            }
        }

        private void comboBoxComis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxComis.SelectedIndex != -1)
                this.textBoxComis.Enabled = true;
            else
                this.textBoxComis.Enabled = false;
        }

        private void textBoxValue_Leave(object sender, EventArgs e)
        {
            double d = 0.0;
            try
            {
                d = Convert.ToDouble(this.textBoxValue.Text);
                this.errorProvider1.SetError(this.textBoxValue, "");
                flagValue = false;
            }
            catch
            {
                this.errorProvider1.SetError(this.textBoxValue, "Не введено значение или ошибка ввода");
                flagValue = true;
            }
        }

        private void textBoxRate_Leave(object sender, EventArgs e)
        {
            double d = 0.0;
            try
            {
                string replace = this.textBoxRate.Text.Replace('.', ',');
                d = Convert.ToDouble(replace);
                this.errorProvider1.SetError(this.textBoxRate, "");
                flagRate = false;
            }
            catch
            {
                this.errorProvider1.SetError(this.textBoxRate, "Не введено значение или ошибка ввода");
                flagRate = true;
            }
        }

        private void comboBoxTerms_Leave(object sender, EventArgs e)
        {
            if (this.comboBoxTerms.Text == "выберите")
            {
                this.errorProvider1.SetError(this.comboBoxTerms, "Не указан срок кредитования");
            }
            else
            {
                this.errorProvider1.SetError(this.comboBoxTerms, "");
            }
        }

        private void comboBoxBodyPay_Leave(object sender, EventArgs e)
        {
            if (this.comboBoxBodyPay.Text == "выберите")
            {
                this.errorProvider1.SetError(this.comboBoxBodyPay, "Не указана периодичность оплаты");
            }
            else
            {
                this.errorProvider1.SetError(this.comboBoxBodyPay, "");
            }
        }

        private void comboBoxPercentPay_Leave(object sender, EventArgs e)
        {
            if (this.comboBoxPercentPay.Text == "выберите")
            {
                this.errorProvider1.SetError(this.comboBoxPercentPay, "Не указана периодичность оплаты");
            }
            else
            {
                this.errorProvider1.SetError(this.comboBoxPercentPay, "");
            }
        }

        private void comboBoxTermsComis_Leave(object sender, EventArgs e)
        {
            if (this.comboBoxTermsComis.Text == "выберите")
            {
                this.errorProvider1.SetError(this.comboBoxTermsComis, "Не указано количество платежей по комиссии");
            }
            else
            {
                this.errorProvider1.SetError(this.comboBoxTermsComis, "");
            }
        }

        private void textBoxFirstPay_Leave(object sender, EventArgs e)
        {
            double d = 0.0;
            try
            {
                string replace = this.textBoxFirstPay.Text.Replace('.', ',');
                d = Convert.ToDouble(replace);
                this.errorProvider1.SetError(this.textBoxFirstPay, "");
                flagFirstPay = false;
            }
            catch
            {
                this.errorProvider1.SetError(this.textBoxFirstPay, "Не введено значение или ошибка ввода");
                flagFirstPay = true;
            }
        }

        private void textBoxComis_Leave(object sender, EventArgs e)
        {
            double d = 0.0;
            try
            {
                string replace = this.textBoxComis.Text.Replace('.', ',');
                d = Convert.ToDouble(replace);
                this.errorProvider1.SetError(this.textBoxComis, "");
                flagComis = false;
            }
            catch
            {
                this.errorProvider1.SetError(this.textBoxComis, "Не введено значение или ошибка ввода");
                flagComis = true;
            }
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