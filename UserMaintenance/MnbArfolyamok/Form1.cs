using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MnbArfolyamok.MNBService;
using MnbArfolyamok.Entities;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;

namespace MnbArfolyamok
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = Rates;
            cbCurrencies.DataSource = Currencies;

            LoadCurrencies();
            SetupChart();

            dtpStart.Value = new DateTime(2020, 01, 01);
            dtpEnd.Value = new DateTime(2020, 06, 30);

            if (Currencies.Contains("EUR"))
            {
                cbCurrencies.SelectedItem = "EUR";
            }

            RefreshData();
        }

        private void LoadCurrencies()
        {
            try
            {
                var response = mnbService.GetCurrencies(new GetCurrenciesRequestBody());
                var xml = new XmlDocument();
                xml.LoadXml(response.GetCurrenciesResult);

                Currencies.Clear();
                var nodes = xml.GetElementsByTagName("Curr");
                foreach (XmlNode node in nodes)
                {
                    Currencies.Add(node.InnerText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: " + ex.Message);
            }
        }

        private void RefreshData()
        {
            if (cbCurrencies.SelectedItem == null) return;

            Rates.Clear();

            var requestBody = new GetExchangeRatesRequestBody()
            {
                currencyNames = cbCurrencies.SelectedItem.ToString(),
                startDate = dtpStart.Value.ToString("yyyy-MM-dd"),
                endDate = dtpEnd.Value.ToString("yyyy-MM-dd")
            };

            try
            {
                var response = mnbService.GetExchangeRates(requestBody);
                ProcessXml(response.GetExchangeRatesResult);
                chartRateData.DataBind();
            }
            catch { }
        }

        private void ProcessXml(string xmlString)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlString);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                if (element.ChildNodes.Count == 0) continue;
                var childElement = (XmlElement)element.ChildNodes[0];

                rate.Currency = childElement.GetAttribute("curr");

                var unitStr = childElement.GetAttribute("unit").Replace(",", ".");
                var unit = decimal.Parse(unitStr, System.Globalization.CultureInfo.InvariantCulture);

                var valueStr = childElement.InnerText.Replace(",", ".");
                var value = decimal.Parse(valueStr, System.Globalization.CultureInfo.InvariantCulture);

                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
                Rates.Add(rate);
            }
        }

        private void SetupChart()
        {
            chartRateData.DataSource = Rates;
            if (chartRateData.Series.Count > 0)
            {
                var series = chartRateData.Series[0];
                series.ChartType = SeriesChartType.Line;
                series.XValueMember = "Date";
                series.YValueMembers = "Value";
                series.BorderWidth = 2;
            }

            if (chartRateData.Legends.Count > 0)
                chartRateData.Legends[0].Enabled = false;

            if (chartRateData.ChartAreas.Count > 0)
            {
                var ca = chartRateData.ChartAreas[0];
                ca.AxisX.MajorGrid.Enabled = false;
                ca.AxisY.MajorGrid.Enabled = false;
                ca.AxisY.IsStartedFromZero = false;
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e) => RefreshData();
        private void dtpEnd_ValueChanged(object sender, EventArgs e) => RefreshData();
        private void cbCurrencies_SelectedIndexChanged(object sender, EventArgs e) => RefreshData();

    }
}