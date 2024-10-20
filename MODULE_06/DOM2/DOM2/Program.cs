using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM2
{
    public interface IReportBuilder
    {
        void SetHeader(string header);
        void SetContent(string content);
        void SetFooter(string footer);
        Report GetReport();
    }

    public class Report
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }

        public void UpdateContent(string newContent)
        {
            this.Content = newContent;
        }

        public override string ToString()
        {
            return $"{Header}\n{Content}\n{Footer}";
        }
    }

    public class TextReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = "Заголовок: " + header;
        }

        public void SetContent(string content)
        {
            _report.Content = "Содержание: " + content;
        }

        public void SetFooter(string footer)
        {
            _report.Footer = "Подвал: " + footer;
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class HtmlReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = $"<h1>{header}</h1>";
        }

        public void SetContent(string content)
        {
            _report.Content = $"<p>{content}</p>";
        }

        public void SetFooter(string footer)
        {
            _report.Footer = $"<footer>{footer}</footer>";
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class XmlReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = $"<Заголовок>{header}</Заголовок>";
        }

        public void SetContent(string content)
        {
            _report.Content = $"<Содержание>{content}</Содержание>";
        }

        public void SetFooter(string footer)
        {
            _report.Footer = $"<Подвал>{footer}</Подвал>";
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class ReportDirector
    {
        public void ConstructReport(IReportBuilder builder)
        {
            builder.SetHeader("Пример отчета");
            builder.SetContent("Это пример содержимого отчета.");
            builder.SetFooter("Подвал отчета");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ReportDirector director = new ReportDirector();

            TextReportBuilder textBuilder = new TextReportBuilder();
            director.ConstructReport(textBuilder);
            Report textReport = textBuilder.GetReport();
            Console.WriteLine("Текстовый отчет:");
            Console.WriteLine(textReport);

            HtmlReportBuilder htmlBuilder = new HtmlReportBuilder();
            director.ConstructReport(htmlBuilder);
            Report htmlReport = htmlBuilder.GetReport();
            Console.WriteLine("\nHTML-отчет:");
            Console.WriteLine(htmlReport);

            XmlReportBuilder xmlBuilder = new XmlReportBuilder();
            director.ConstructReport(xmlBuilder);
            Report xmlReport = xmlBuilder.GetReport();
            Console.WriteLine("\nXML-отчет:");
            Console.WriteLine(xmlReport);

            textReport.UpdateContent("Это обновленное содержимое для текстового отчета.");
            Console.WriteLine("\nОбновленный текстовый отчет:");
            Console.WriteLine(textReport);
        }
    }
}