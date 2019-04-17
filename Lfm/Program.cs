using System;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Lfm
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlWeb html=new HtmlWeb();

            HtmlDocument doc = html.Load(Constants.Path);


            var items = doc.DocumentNode.SelectSingleNode("//td[@class='chartlist-name']//span");

            var info = items.InnerText.Split(new[]{" ","\n"},StringSplitOptions.RemoveEmptyEntries);
           

            Console.WriteLine(string.Join(" ",info));



        }
    }
}
