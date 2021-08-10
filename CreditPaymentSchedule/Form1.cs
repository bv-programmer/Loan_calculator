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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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
