using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HKeInvestWebApplication.Code_File;

namespace HKeInvestWebApplicationTests.Code_File
{
    [TestFixture]
    class HKeInvestCodeTests
    {
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();

        [TestCase("EUR", "8.488", "USD", "7.791", 5.0, 5.45)]
        [TestCase("USD", "7.791", "JPY", ".065", 4.0, 479.45)]
        [TestCase("HKD", "1", "JPY", ".065", 3.0, 46.15)]
        [TestCase("USD", "7.791", "GBP", "11.1", 2.0, 1.4)]
        [TestCase("EUR", "8.488", "GBP", "11.1", 1.0, .76)]
        public void TestConvertCurrency(string fromCurrency, string fromCurrencyRate, string toCurrency, string toCurrencyRate, decimal value, decimal result)
        {
            decimal data = myHKeInvestCode.convertCurrency(fromCurrency, fromCurrencyRate, toCurrency, toCurrencyRate, value);
            Assert.AreEqual(data, result);
        }
    }
}