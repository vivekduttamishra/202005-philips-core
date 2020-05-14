using HelloMVC.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC.Controllers
{
    public class MultiplicationTableController: Controller
    {
        public ContentResult Table1(int id,int? highestMultiple)
        {
            int max = 10;
            if (highestMultiple != null)
                max = highestMultiple.Value;

            var number = id;

            var html = new StringBuilder();
            html.AppendFormat("<html><head><title>Multiplication Table of {0}</title></head>", number);
            html.AppendFormat("<body><h1>Multiplication Table of {0}</h1></body>", number);

            html.AppendFormat("<table class='table table-responsive' ><thead><tr>");
            html.AppendFormat("<th>Number</th><th>X</th><th>Result</th>");
            html.AppendFormat("</tr></thead>");

            for(int i = 1; i <= max; i++)
            {
                var result = number * i;
                html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",number,i,result);
            }

            html.AppendFormat("</table></body></html>");

            return Content(html.ToString());


        }


        public ViewResult Table2(int id, int? highestMultiple)
        {
            int max = 10;
            if (highestMultiple != null)
                max = highestMultiple.Value;

            var number = id;

            var html = new StringBuilder();
            
            html.AppendFormat("<h1>Multiplication Table of {0}</h1>", number);
            html.AppendFormat("<table class='table table-responsive' ><thead><tr>");
            html.AppendFormat("<th>Number</th><th>X</th><th>Result</th>");
            html.AppendFormat("</tr></thead>");
            for (int i = 1; i <= max; i++)
            {
                var result = number * i;
                html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", number, i, result);
            }
            html.AppendFormat("</table>");

            return View(html); //generated table as the model

        }

        public ViewResult Table3(int id, int? highestMultiple)
        {
            int max = 10;
            if (highestMultiple != null)
                max = highestMultiple.Value;

            var number = id;

            //Generate the table for given number
            ViewBag.Max = max;
            return View(number); //generated table as the model


        }

        public ViewResult Table4(int id, int? highestMultiple)
        {
            var table = new MultiplicationTable()
            {
                Number = id,
                Max = 10
            };

           
            if (highestMultiple != null)
                table.Max = highestMultiple.Value;

            return View(table); //generated table as the model

        }




    }
}