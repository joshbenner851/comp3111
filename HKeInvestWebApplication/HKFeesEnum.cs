using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKeInvestWebApplication
{
    public class HKFeesEnum
    {
        public HKFeesEnum(string amount, string transactionType,string securityType, string orderType)
        {
            float.TryParse(amount, out m_Amount);
            if (m_Amount == 0)
            {
                System.Console.WriteLine("Amount is below zero or null");
            }
            else if (securityType == "stock")
            {
                if (m_Amount >= thresholdAmt)
                {
                    switch (orderType)
                    {
                        case "market":
                            m_StockOrderTypeFee = .02;
                            break;
                        case "limit":
                        case "stop":
                            m_StockOrderTypeFee = .04;
                            break;
                        case "stop limit":
                            m_StockOrderTypeFee = .06;
                            break;
                    }
                }
                else
                {

                    switch (orderType)
                    {
                        case "market":
                            m_StockOrderTypeFee = 0.4;
                            break;
                        case "limit":
                        case "stop":
                            m_StockOrderTypeFee = 0.6;
                            break;
                        case "stop limit":
                            m_StockOrderTypeFee = .08;
                            break;
                    }
                }
            }
            else if (securityType == "bond" || securityType == "unit trust")
            {
                if (m_Amount >= thresholdAmt)
                {
                    if (transactionType == "buy")
                    {
                        m_BondTrustBuyFee = .03;
                    }
                    else
                    {
                        m_BondTrustSellFee = 100;
                    }
                }
                else
                {
                    if (transactionType == "buy")
                    {
                        m_BondTrustBuyFee = .05;
                    }
                    else
                    {
                        m_BondTrustSellFee = 50;
                    }
                }
            }
        }

        public int getStockFee()
        {
            return m_MinStockFee;
        }

        public double getStockOrderFee()
        {
            return m_StockOrderTypeFee;
        }

        public double getBondTrustBuyFee()
        {
            return m_BondTrustBuyFee;
        }

        public double getBondTrustSellFee()
        {
            return m_BondTrustSellFee;
        }

        private int m_MinStockFee;
        private double m_StockOrderTypeFee;
        private double m_BondTrustBuyFee;
        private int m_BondTrustSellFee;
        private float m_Amount;
        private const int thresholdAmt = 1000000; //The 
    }
}