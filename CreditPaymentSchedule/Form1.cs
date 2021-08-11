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
    public partial class Form1 : Form
    {
        CreditInfo CI;
        DataTable DT;
        string fileNamePaymentSchedule;

        public Form1()
        {
            InitializeComponent();
            this.buttonCreate.Visible = false;
            this.buttonCreate.Enabled = false;
            CI = new CreditInfo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetDataForCalculations();

            MakeTableStructure();

            CreateGraph();
        }

        // получить данные для расчетов
        private void GetDataForCalculations()
        {
            Form2 F = new Form2();
            if (F.ShowDialog() == DialogResult.Cancel)
            {
                this.labelInfo.Text = "Нажмите <Новый> для создания нового графика";
                CI.empty = true;
                return;
            }

            CI.creditValue = Convert.ToDecimal(F.textBoxValue.Text);
            CI.creditTerm = Convert.ToInt32(F.comboBoxTerms.SelectedItem.ToString());
            CI.creditRate = F.ICT.rate;

            if (F.checkBoxFirstPay.Checked)
            {
                if (F.comboBoxFirstPay.SelectedIndex == 1)
                    CI.firstPay = CI.creditValue * F.ICT.pay;
                else
                    CI.firstPay = Convert.ToDecimal(F.textBoxFirstPay.Text);
            }

            if (F.checkBoxComis.Checked)
            {
                if (F.comboBoxComis.SelectedIndex == 1)
                    CI.onetimeComiss = CI.creditValue * F.ICT.comis;
                else
                    CI.onetimeComiss = Convert.ToDecimal(F.textBoxComis.Text);
            }

            CI.bodyPayTerm = Convert.ToInt32(F.comboBoxBodyPay.SelectedItem.ToString());
            CI.percentPayTerm = Convert.ToInt32(F.comboBoxPercentPay.SelectedItem.ToString());
            if (F.checkBoxComis.Checked)
                CI.comissionTime = Convert.ToInt32(F.comboBoxTermsComis.SelectedItem.ToString());

            CI.empty = false;
        }

        // cформировать структуры таблицы с графиком платежей
        private void MakeTableStructure()
        {
            DT = new System.Data.DataTable();
            DataColumn idPay = new DataColumn("Платеж", Type.GetType("System.Int32"));
            idPay.AutoIncrement = true;
            idPay.AutoIncrementSeed = 1;            

            DataColumn colRestPay = new DataColumn("Остаток", Type.GetType("System.Decimal"));
            DataColumn colBodyPay = new DataColumn("Тело кредита", Type.GetType("System.Decimal"));
            DataColumn colPercentPay = new DataColumn("Проценты", Type.GetType("System.Decimal"));
            DataColumn colComission = null;
            if (CI.onetimeComiss >= 1)
                colComission = new DataColumn("Комиссия", Type.GetType("System.Decimal"));
            DataColumn colTotalPay = new DataColumn("Итоговый платеж", Type.GetType("System.Decimal"));

            DT.Columns.Add(idPay);
            DT.Columns.Add(colRestPay);
            DT.Columns.Add(colBodyPay);
            DT.Columns.Add(colPercentPay);
            if (CI.onetimeComiss >= 1)
                DT.Columns.Add(colComission);
            DT.Columns.Add(colTotalPay);
        }

        // сформировать график платежей
        private void CreateGraph()
        {
            if (this.dataGridView1.DataSource != null) return;

            if (CI.empty)
            {
                MessageBox.Show("Не указаны параметры кредита\nДля их указания нажмите кнопку <Новый> и заполните поля");
                return;
            }

            decimal startPrice = CI.creditValue - CI.firstPay;
            decimal cred;
            decimal percent = 0;
            int tempComisTime = CI.comissionTime;
            decimal tempStartPrice = startPrice;

            decimal total = 0.0m;

            decimal sumCredit = startPrice;
            decimal sumPercent = 0.0m;
            decimal sumTotal = 0.0m;

            while (Math.Truncate(startPrice) > 0)
            {
                DataRow row = DT.NewRow();
                row[1] = string.Format("{0:N2}", startPrice);
                if (Convert.ToInt32(row[0].ToString()) % CI.bodyPayTerm == 0)
                {
                    cred = tempStartPrice / (CI.creditTerm / CI.bodyPayTerm);
                }
                else
                    cred = 0.0m;

                row[2] = string.Format("{0:N2}", cred);

                if (Convert.ToInt32(row[0].ToString()) % CI.percentPayTerm == 0)
                {
                    percent = startPrice * (CI.creditRate / 12);
                }
                else
                    percent = 0.0m;

                sumPercent += percent;
                row[3] = string.Format("{0:N2}", percent);


                if (CI.onetimeComiss >= 1) // если есть комиссия
                {
                    if (CI.comissionTime > 0)
                    {
                        row[4] = string.Format("{0:N2}", CI.onetimeComiss / tempComisTime);
                        total = cred + percent + (CI.onetimeComiss / tempComisTime);

                        row[5] = string.Format("{0:N2}", total);
                        CI.comissionTime--;
                    }
                    else
                    {
                        row[4] = 0.0;
                        total = cred + percent;
                        row[5] = string.Format("{0:N2}", total);
                    }
                }
                else
                {
                    total = cred + percent;
                    row[4] = string.Format("{0:N2}", total);
                }

                sumTotal += total;

                startPrice -= cred;
                this.DT.Rows.Add(row);
            }
            this.dataGridView1.DataSource = DT;

            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++) // в цикле скрываются нулевые значения
            {
                if (this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Red;

                if (this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[3].Style.ForeColor = Color.Blue;

                if (this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[4].Style.ForeColor = Color.Green;

            }

            this.labelSum.Text = string.Format("{0:N2}", CI.creditValue);
            this.labelFirstPay.Text = string.Format("{0:N2}", CI.firstPay);
            this.labelCredit.Text = string.Format("{0:N2}", sumCredit);
            this.labelPercent.Text = string.Format("{0:N2}", sumPercent);
            this.labelComis.Text = string.Format("{0:N2}", CI.onetimeComiss);
            this.labelTotalPayments.Text = string.Format("{0:N2}", sumTotal);
            this.labelOverpay.Text = string.Format("{0:N2}", sumTotal + CI.firstPay - CI.creditValue);
            this.labelPriceHikes.Text = string.Format("{0:N1}", (sumTotal + CI.firstPay - CI.creditValue) / CI.creditValue * 100);

            this.labelInfo.Text = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        // вернуть элементам формы значения по умолчанию
        private void DefaultValuesFormElements()
        {
            this.labelSum.Text = "";
            this.labelFirstPay.Text = "";
            this.labelCredit.Text = "";
            this.labelPercent.Text = "";
            this.labelComis.Text = "";
            this.labelTotalPayments.Text = "";
            this.labelOverpay.Text = "";
            this.labelPriceHikes.Text = "";
            this.labelInfo.Text = "";
        }

        public class CreditInfo
        {
            public decimal creditValue; // сумма кредита
            public int creditTerm; // срок кредита
            public decimal creditRate; // процентная ставка
            public decimal firstPay; // первоначальный платеж
            public decimal onetimeComiss; // единоразовая комиссия        
            public int bodyPayTerm; // очередность уплаты тела кредита
            public int percentPayTerm; // очередность уплаты процентов
            public int comissionTime; // срок уплаты единоразовой комиссии
            public bool empty; // заполнены ли все поля

            public CreditInfo(decimal v = 1000, int cT = 1, decimal cR = 10, decimal fP = 0, decimal oTC = 0, int bPT = 1, int pPT = 1, int cmT = 0, bool em = true)
            {
                this.creditValue = v;
                this.creditTerm = cT;
                this.creditRate = cR;
                this.firstPay = fP;
                this.onetimeComiss = oTC;
                this.bodyPayTerm = bPT;
                this.percentPayTerm = pPT;
                this.comissionTime = cmT;
                this.empty = em;
            }
        }
    }
}
