using Microsoft.Reporting.NETCore;
using RdlcWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace RdlcWebApi.Services
{
    public class ReportService
    {
        public byte[] GenerateReportAsync(string reportName, string reportType)
        {
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("RdlcWebApi.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

            byte[] result = null;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");
            if (File.Exists(rdlcFilePath))
            {
                // Create a Stream to read the RDLC file
                using (Stream reportDefinition = File.OpenRead(rdlcFilePath))
                {
                    var report = new LocalReport();
                    report.LoadReportDefinition(reportDefinition);
                    // prepare data for report
                    List<UserDto> userList = new List<UserDto>();

                    var user1 = new UserDto { FirstName = "jp", LastName = "jan", Email = "jp@gm.com", Phone = "+976666661111" };
                    var user2 = new UserDto { FirstName = "jp2", LastName = "jan", Email = "jp2@gm.com", Phone = "+976666661111" };

                    userList.Add(user1);
                    userList.Add(user2);

                    report.DataSources.Add(new ReportDataSource("dsUsers", userList));

                     report.SetParameters(new[] { new ReportParameter("dataStart", "Parameter value") });

                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    result = report.Render(RenderType(reportType));
                   
                }
            }
            return result;
        }

      

        private string RenderType(string reportType)
        {
            var renderType = "";

            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    renderType = "PDF";
                    break;
                case "XLS":
                    renderType = "Excel";
                    break;
                case "WORD":
                    renderType = "Word";
                    break;
            }

            return renderType;
        }

    }
}
