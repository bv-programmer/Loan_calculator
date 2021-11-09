using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditPaymentSchedule
{
    public static class IndividualCreditTerms
    {
        private static decimal creditvalue; // сумма кредита
        private static int creditTerm; // срок кредита
        private static decimal rate; // процентная ставка
        private static decimal firstPay; // первоначальный платеж
        private static decimal comission; // единоразовая комиссия        
        private static int creditBodyPayTerm; // очередность уплаты тела кредита
        private static int creditPercentPayTerm; // очередность уплаты процентов
        private static int comissionPayTime; // срок уплаты единоразовой комиссии
        private static bool isEmptyLines; // заполнены ли все поля

        public static decimal CreditValue
        {
            get
            {
                return creditvalue;
            }
            set
            {
                creditvalue = value;
            }
        }
        public static int CreditTerm
        {
            get
            {
                return creditTerm;
            }
            set
            {
                creditTerm = value;
            }
        }
        public static decimal Rate
        {
            get
            {
                return rate;
            }
            set
            {
                rate = value;
            }
        }
        public static decimal FirstPay
        {
            get
            {
                return firstPay;
            }
            set
            {
                firstPay = value;
            }
        }
        public static decimal Comission
        {
            get
            {
                return comission;
            }
            set
            {
                comission = value;
            }
        }
        public static int CreditBodyPayTerm
        {
            get
            {
                return creditBodyPayTerm;
            }
            set
            {
                creditBodyPayTerm = value;
            }
        }
        public static int CreditPercentPayTerm
        {
            get
            {
                return creditPercentPayTerm;
            }
            set
            {
                creditPercentPayTerm = value;
            }
        }
        public static int ComissionPayTime
        {
            get
            {
                return comissionPayTime;
            }
            set
            {
                comissionPayTime = value;
            }
        }
        public static bool IsEmptyLines
        {
            get
            {
                return isEmptyLines;
            }
            set
            {
                isEmptyLines = value;
            }
        }
    }
}
