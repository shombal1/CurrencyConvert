using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Parsing
{


    public class ClassParse
    {
        private string ClearString(string text) =>text.Replace("\n", String.Empty).Replace(" ", String.Empty);

        private void GetNameAndResizeDataMony(List<DataMoney> dataMoney, IHtmlDocument htmlDocument)
        {
            IEnumerable<AngleSharp.Dom.IElement> elements = htmlDocument.QuerySelectorAll("td").Where(item => item.ClassName != null && item.ClassName.Contains("curName"));
            foreach(var i in elements)
            {
                dataMoney.Add(new DataMoney() { name = i.TextContent.Replace("\n",string.Empty).Trim() } );
            }
        }
        private void GetMoneyBelarus(List<DataMoney> dataMoney, IHtmlDocument htmlDocument)
        {
            IEnumerable<AngleSharp.Dom.IElement> elements = htmlDocument.QuerySelectorAll("td").Where(item => item.ClassName != null && item.ClassName.Contains("curCours"));
            for (int i = 0; i < elements.Count(); i++)
            {
                dataMoney[i].moneyBelarus =Convert.ToDecimal( elements.ToList()[i].TextContent);
            }
        }
        private void GetForeignMoneyAndAlphabeticCurrencyCode(List<DataMoney> dataMoney, IHtmlDocument htmlDocument)
        {
            IEnumerable<AngleSharp.Dom.IElement> elements = htmlDocument.QuerySelectorAll("td").Where(item => item.ClassName != null && item.ClassName.Contains("curAmount"));
            int z = 0;
            foreach(var i in elements)
            {
                i.TextContent = ClearString(i.TextContent);
                for(int j=0;j<i.TextContent.Length;j++)
                {
                    if(!(i.TextContent[j] >='0' && i.TextContent[j] <='9' || i.TextContent[j]==','))
                    {
                        dataMoney[z].foreignMoney=Convert.ToDecimal(i.TextContent.Substring(0,j));
                        dataMoney[z].alphabeticCurrencyCode = i.TextContent.Substring(j, i.TextContent.Length - j);
                        break;
                    }
                }
                z++;
            }
        }

        public List<DataMoney> Parse(IHtmlDocument htmlDocument)
        {
            List<DataMoney> dataMoney = new List<DataMoney>();
            GetNameAndResizeDataMony(dataMoney, htmlDocument);
            GetMoneyBelarus(dataMoney, htmlDocument);
            GetForeignMoneyAndAlphabeticCurrencyCode(dataMoney, htmlDocument);
            //System.Collections.Generic.IEnumerable<AngleSharp.Dom.IElement> t = htmlDocument.QuerySelectorAll("td").Where(i=> i.ClassName !=null && i.ClassName.Contains("curName") );//curCours; curAmount,curName
            //List<string> result= new List<string>();
            //foreach (var item in t)
            //{
            //    result.Add(item.TextContent.Replace("\n",string.Empty).Replace(" ",String.Empty));
            //}
            return dataMoney;
        }
    }
}
