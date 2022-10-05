using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Parsing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, decimal> exchange = new Dictionary<string, decimal>();
        public decimal ConvertMoney(decimal countFirstMoney,string nameFirstMoney, string nameSecondMoney)
        {
            return countFirstMoney * exchange[nameFirstMoney] / exchange[nameSecondMoney];
        }

        public MainWindow()
        {
            InitializeComponent();
            HttpClient httpClient = new HttpClient();   
            Task<HttpResponseMessage> tasks = httpClient.GetAsync("https://www.nbrb.by/statistics/rates/ratesdaily.asp");
            ClassParse clasParser = new ClassParse();
            HtmlParser htmalParser = new HtmlParser();
            IHtmlDocument ihtmlDocument = htmalParser.ParseDocumentAsync(tasks.Result.Content.ReadAsStringAsync().Result).Result;
            List<DataMoney> dataMoney = clasParser.Parse(ihtmlDocument);
            foreach (var a in dataMoney)
            {
                exchange.Add(a.alphabeticCurrencyCode + " " + a.name, a.moneyBelarus / a.foreignMoney);
            }
            comboBox1.ItemsSource = exchange.Keys;
            comboBox2.ItemsSource = exchange.Keys;
            comboBox1.Text = dataMoney[0].alphabeticCurrencyCode+" "+ dataMoney[0].name;
            comboBox2.Text = dataMoney[1].alphabeticCurrencyCode + " " + dataMoney[1].name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox2.Text = Convert.ToString(ConvertMoney(Convert.ToDecimal(textBox1.Text), comboBox1.Text, comboBox2.Text));
            }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!decimal.TryParse(e.Text, out decimal num))
            {
                e.Handled = true;
            }
            
        }
    }
}
