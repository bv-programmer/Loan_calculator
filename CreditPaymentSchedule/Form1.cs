using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace CreditPaymentSchedule
{
    public partial class Form1 : Form
    {
        System.Data.DataTable DT;
        string fileNamePaymentSchedule;

        public Form1()
        {
            InitializeComponent();
            this.buttonCreate.Visible = false;
            this.buttonCreate.Enabled = false;
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
                IndividualCreditTerms.IsEmptyLines = true;
                return;
            }
        }

        // cформировать структуру таблицы с графиком платежей
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
            if (IndividualCreditTerms.Comission >= 1)
                colComission = new DataColumn("Комиссия", Type.GetType("System.Decimal"));
            DataColumn colTotalPay = new DataColumn("Итоговый платеж", Type.GetType("System.Decimal"));

            DT.Columns.Add(idPay);
            DT.Columns.Add(colRestPay);
            DT.Columns.Add(colBodyPay);
            DT.Columns.Add(colPercentPay);
            if (IndividualCreditTerms.Comission >= 1)
                DT.Columns.Add(colComission);
            DT.Columns.Add(colTotalPay);
        }

        // сформировать график платежей
        private void CreateGraph()
        {
            if (this.dataGridView1.DataSource != null) return;

            if (IndividualCreditTerms.IsEmptyLines)
            {
                MessageBox.Show("Не указаны параметры кредита\nДля их указания нажмите кнопку <Новый> и заполните поля");
                return;
            }

            decimal startPrice = IndividualCreditTerms.CreditValue - IndividualCreditTerms.FirstPay;
            decimal cred;
            decimal percent = 0;
            int comissionTime = IndividualCreditTerms.ComissionPayTime;
            int tempComisTime = comissionTime;
            decimal tempStartPrice = startPrice;

            decimal total = 0.0m;

            decimal sumCredit = startPrice;
            decimal sumPercent = 0.0m;
            decimal sumTotal = 0.0m;

            while (Math.Truncate(startPrice) > 0)
            {
                DataRow row = DT.NewRow();
                row[1] = string.Format("{0:N2}", startPrice);
                if (Convert.ToInt32(row[0].ToString()) % IndividualCreditTerms.CreditBodyPayTerm == 0)
                {
                    cred = tempStartPrice / (IndividualCreditTerms.CreditTerm / IndividualCreditTerms.CreditBodyPayTerm);
                }
                else
                    cred = 0.0m;

                row[2] = string.Format("{0:N2}", cred);

                if (Convert.ToInt32(row[0].ToString()) % IndividualCreditTerms.CreditPercentPayTerm == 0)
                {
                    percent = startPrice * (IndividualCreditTerms.Rate / 12);
                }
                else
                    percent = 0.0m;

                sumPercent += percent;
                row[3] = string.Format("{0:N2}", percent);

                
                if (IndividualCreditTerms.Comission >= 1) // если есть комиссия
                {                    
                    if (comissionTime > 0)
                    {
                        row[4] = string.Format("{0:N2}", IndividualCreditTerms.Comission / tempComisTime);
                        total = cred + percent + (IndividualCreditTerms.Comission / tempComisTime);

                        row[5] = string.Format("{0:N2}", total);
                        comissionTime--;
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

            // поменять цвета в ячейках таблицы
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {
                if (this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Red;

                if (this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[3].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[3].Style.ForeColor = Color.Blue;

                if (this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0,00" || this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0" || this.dataGridView1.Rows[i].Cells[4].Value.ToString() == "0.00")
                    this.dataGridView1.Rows[i].Cells[4].Style.ForeColor = Color.Green;

            }

            this.labelSum.Text = string.Format("{0:N2}", IndividualCreditTerms.CreditValue);
            this.labelFirstPay.Text = string.Format("{0:N2}", IndividualCreditTerms.FirstPay);
            this.labelCredit.Text = string.Format("{0:N2}", sumCredit);
            this.labelPercent.Text = string.Format("{0:N2}", sumPercent);
            this.labelComis.Text = string.Format("{0:N2}", IndividualCreditTerms.Comission);
            this.labelTotalPayments.Text = string.Format("{0:N2}", sumTotal);
            this.labelOverpay.Text = string.Format("{0:N2}", sumTotal + IndividualCreditTerms.FirstPay - IndividualCreditTerms.CreditValue);
            this.labelPriceHikes.Text = string.Format("{0:N1}", (sumTotal + IndividualCreditTerms.FirstPay - IndividualCreditTerms.CreditValue) / IndividualCreditTerms.CreditValue * 100);

            this.labelInfo.Text = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            CreateGraph();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (IndividualCreditTerms.IsEmptyLines)
            {
                MessageBox.Show("Не указаны параметры кредита\nГрафик не сформирован");
                return;
            }

            if (backgroundWorker1.IsBusy)
                return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Книга Excel 2003|*.xls|Книга Excel|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    fileNamePaymentSchedule = sfd.FileName;

                    progressBar1.Minimum = 0;
                    progressBar1.Value = 0;
                    backgroundWorker1.RunWorkerAsync(fileNamePaymentSchedule);
                }
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            lblFileCreate.Text = "Процесс создания файла";
            DefaultValuesFormElements();
            GetDataForCalculations();
            MakeTableStructure();
            CreateGraph();
        }

        // формирование документа EXCEL
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.DisplayAlerts = false; // отключить сообщения об ощибках
            Workbook wb = excel.Workbooks.Add(XlSheetType.xlWorksheet); // создание книги
            Worksheet ws = (Worksheet)excel.ActiveSheet; // создание листа

            const int ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET = 7;

            int endRange = DT.Rows.Count + ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET; // последняя строка графика платежей

            Range range = ws.get_Range("A" + ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, "F" + endRange); // получение диапазона ячеек под таблицей
            range.Cells.Font.Size = 14; // размер шрифта в таблице
            range.Cells.Font.Name = "Times New Roman"; // стиль шрифта в таблице

            // добавление рамок к таблице
            range.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
            range.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;
            range.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
            range.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
            range.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;

            // общая информация в шапке на странице
            Range range1 = ws.get_Range("A1", "F5");
            range1.Cells.Font.Size = 14;
            range1.Cells.Font.Name = "Times New Roman";
            ws.Cells[2, 2] = "Сумма:";
            ws.Cells[3, 2] = "Ставка:";
            ws.Cells[4, 2] = "Первоначальный взнос:";
            ws.Cells[2, 3] = this.labelSum.Text;
            string rt = (IndividualCreditTerms.Rate * 100).ToString() + "%";
            ws.Cells[3, 3] = rt;
            ws.Cells[4, 3] = this.labelFirstPay.Text;

            // создание колонок в таблице MS Excel
            Range range2 = ws.get_Range("A" + ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, "F" + ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET);
            range2.Cells.Font.FontStyle = "Bold";
            if (DT.Columns.Count > 5)
            {
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 1] = "Платеж";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 2] = "Остаток";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 3] = "Тело кредита";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 4] = "Проценты";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 5] = "Комиссия";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 6] = "Итоговый платеж";
            }
            else
            {
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 1] = "Платеж";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 2] = "Остаток";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 3] = "Тело кредита";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 4] = "Проценты";
                ws.Cells[ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET, 5] = "Итоговый платеж";
            }

            // заполнение таблицы MS Excel данными
            int currentRow = ROW_NUMBER_FIRST_TABLE_CELL_ON_SHEET;
            foreach (DataRow row in DT.Rows)
            {
                if (!backgroundWorker1.CancellationPending)
                {
                    currentRow++;

                    if (DT.Columns.Count > 5)
                    {
                        ws.Cells[currentRow, 1] = row["Платеж"].ToString();
                        ws.Cells[currentRow, 2] = Convert.ToDouble(row["Остаток"]);
                        ws.Cells[currentRow, 3] = Convert.ToDouble(row["Тело кредита"]);
                        ws.Cells[currentRow, 4] = Convert.ToDouble(row["Проценты"]);
                        ws.Cells[currentRow, 5] = Convert.ToDouble(row["Комиссия"]);
                        ws.Cells[currentRow, 6] = Convert.ToDouble(row["Итоговый платеж"]);
                    }
                    else
                    {
                        ws.Cells[currentRow, 1] = row["Платеж"].ToString();
                        ws.Cells[currentRow, 2] = Convert.ToDouble(row["Остаток"]);
                        ws.Cells[currentRow, 3] = Convert.ToDouble(row["Тело кредита"]);
                        ws.Cells[currentRow, 4] = Convert.ToDouble(row["Проценты"]);
                        ws.Cells[currentRow, 5] = Convert.ToDouble(row["Итоговый платеж"]);
                    }
                    backgroundWorker1.ReportProgress(currentRow * (100 / DT.Rows.Count));
                }
            }
            endRange++;

            // создать объединенную ячейку в конце графика
            Range range3 = ws.get_Range("A" + endRange, "B" + endRange);
            range3.Merge();
            range3.Cells.Font.Size = 14; // размер шрифта в таблице
            range3.Cells.Font.Name = "Times New Roman"; // стиль шрифта в таблице
            range3.Cells.Font.FontStyle = "Bold";
            range3.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
            range3.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;
            range3.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
            range3.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
            range3.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;

            // создать ячейки для вывода итоговых результатов графика
            Range range4 = ws.get_Range("C" + endRange, "F" + endRange);
            range4.Cells.Font.Size = 14; // размер шрифта в таблице
            range4.Cells.Font.Name = "Times New Roman"; // стиль шрифта в таблице
            range4.Cells.Font.FontStyle = "Bold";
            range4.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
            range4.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;
            range4.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
            range4.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
            range4.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;

            int lastRow = ++currentRow;
            if (DT.Columns.Count > 5)
            {
                ws.Cells[lastRow, 1] = "ИТОГО";
                ws.Cells[lastRow, 3] = this.labelCredit.Text;
                ws.Cells[lastRow, 4] = this.labelPercent.Text;
                ws.Cells[lastRow, 5] = this.labelComis.Text;
                ws.Cells[lastRow, 6] = this.labelTotalPayments.Text;
            }
            else
            {
                ws.Cells[lastRow, 1] = "ИТОГО";
                ws.Cells[lastRow, 3] = this.labelCredit.Text;
                ws.Cells[lastRow, 4] = this.labelPercent.Text;
                ws.Cells[lastRow, 5] = this.labelTotalPayments.Text; ;
            }

            excel.Columns.AutoFit();
            ws.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            excel.Visible = true;

            // сохранение документа MS Excel на компьютере
            ws.SaveAs(fileNamePaymentSchedule, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 100)
            {
                progressBar1.Value = e.ProgressPercentage;
                progressBar1.Update();
                lblFileCreate.Text = String.Format("Обработка...{0}", e.ProgressPercentage);
            }
            else
            {
                progressBar1.Value = 100;
                progressBar1.Update();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Thread.Sleep(100);
                lblFileCreate.Text = "Файл успешно создан";
                progressBar1.Value = 100;
            }
            Thread.Sleep(1000);
            progressBar1.Value = 0;
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
    }
}
