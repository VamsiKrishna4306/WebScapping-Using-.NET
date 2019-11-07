using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace Canada_Job_Bank_Web_Scrapping1
{

    public class JobPostings
    {
        public String JobTitle { get; set; }
        public String Company { get; set; }
        public String Location { get; set; }

        IDictionary<string, string> Geography = new Dictionary<string, string>();
        public String PostedSalary { get; set; }
        public String PostDate { get; set; }
        public String JOblink { get; set; }



        public async void GetJobPostDetails(String url)
        {
            // creating new string builder object
            var csv = new StringBuilder();      
            // appending field names
            // csv.AppendLine("JobTitle,Company,Geography,PostedSalary,PostDate,JOblink");
            csv.AppendLine("JobTitle|Company|PostedSalary|PostDate|JOblink");
            // iterating through pages of the url
            for (int i = 1; i <= 10; i++)
            {
                // appending page number to url
                String urlPages = String.Concat(url, i);
                Console.WriteLine(urlPages);
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(urlPages);
                // Loading source html
                var htmlDosument = new HtmlDocument();
                htmlDosument.LoadHtml(html);

                // Extracting data from html nodes 
                var bodyMainHtml = htmlDosument.DocumentNode.Descendants("main")
                     .Where(node => node.GetAttributeValue("property", "")
                     .Equals("mainContentOfPage")).ToList();
                var resultlistWrapper = bodyMainHtml[0].Descendants("div")
                     .Where(node => node.GetAttributeValue("id", "")
                     .Equals("results-list-wrapper")).ToList();
                var resultlistContent = resultlistWrapper[0].Descendants("div")
                     .Where(node => node.GetAttributeValue("id", "")
                     .Equals("results-list-content")).ToList();
                var articles = resultlistContent[0].Descendants("article")
                    .ToList();

                // iterating through each job posting
                foreach (var article in articles)
                {
                    var JOblinkPart = article.Descendants("a").Select(node => node.Attributes["href"].Value).ToArray()[0];
                    JOblink = String.Concat("https://www.jobbank.gc.ca", JOblinkPart);

                    var JobDetails = article.Descendants("a").Where(node => node.GetAttributeValue("class", "")
                         .Equals("resultJobItem")).ToList();

                    JobTitle = JobDetails[0].Descendants("h3").ToList()[0].Descendants("span").Where(node => node.GetAttributeValue("class", "")
                                   .Equals("noctitle")).FirstOrDefault().InnerText.Replace("\t", "").Split("\n").ToArray()[1].Trim();
                    PostDate = JobDetails[0].Descendants("ul").ToList()[0].Descendants("li").Where(node => node.GetAttributeValue("class", "")
                                   .Equals("date")).FirstOrDefault().InnerText.Trim();
                    Company = JobDetails[0].Descendants("ul").ToList()[0].Descendants("li").Where(node => node.GetAttributeValue("class", "")
                                   .Equals("business")).FirstOrDefault().InnerText.Trim();
                    Location = JobDetails[0].Descendants("ul").ToList()[0].Descendants("li").Where(node => node.GetAttributeValue("class", "")
                                   .Equals("location")).FirstOrDefault().InnerText.Replace("Location", "").Trim();

                    String city = Location.Split("(")[0].Trim();
                    String province = Location.Split(" ")[1].Replace(")", "");

                    Geography.Add(city, province);

                    var Salary = JobDetails[0].Descendants("ul").ToList()[0].Descendants("li").Where(node => node.GetAttributeValue("class", "")
                                    .Equals("salary")).ToList()[0].Descendants("span").Where(node => node.GetAttributeValue("class", "")
                                    .Equals("salary-item")).DefaultIfEmpty().ToArray();

                    // if salary data not available ,printing "NO SALARY POSTED"
                    if (Salary[0] == null)
                    {
                        PostedSalary = "NO SALARY POSTED";
                    }
                    else
                    {
                        PostedSalary = Salary[0].InnerText;
                    }

                    // printing the data to the console

                    Console.WriteLine(JobTitle);
                    Console.WriteLine(Company);
                    Console.WriteLine(Location);
                    Console.WriteLine(PostedSalary);
                    Console.WriteLine(PostDate);
                    Console.WriteLine("https://www.jobbank.gc.ca/" + JOblink);
                    Console.WriteLine();
                    // Csv file writing
                    ;
                    // appending data to the  csv string builder
                    // var newLine = string.Format("{0},{1},{2},{3},{4},{5}", "\"" + JobTitle + "\"", Company, "\"" + "{{city:" +city+"},"+"{province:"+province+"}}"+ "\"" , "\"" + PostedSalary + "\"", "\"" + PostDate + "\"", JOblink);
                    var newLine = string.Format("{0}|{1}|{2}|{3}|{4}", JobTitle, Company, PostedSalary, PostDate, JOblink);
                    csv.AppendLine(newLine);
                    Geography.Clear();
                }

            }
            // appending all text from csv object to the file
            File.AppendAllText("D:/Bdat_Course_Material/Semister 2/Social data mining techniques/C#/C#_Canada_job_listings.csv", csv.ToString());

        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            // creating object for JobPostings class and running GetJobPostDetails method
            var jobpostdetails = new JobPostings();
            jobpostdetails.GetJobPostDetails("https://www.jobbank.gc.ca/jobsearch/jobsearch?d=50&fn=1123&fn=1225&fn=2171&fn=2172&fn=2281&mid=22437&term=science+data&page=");
            var jsonconverter = new CsvtoJson();
            // converting csv file to json
            var jsonobj = jsonconverter.ConvertCsvFileToJsonObject("D:/Bdat_Course_Material/Semister 2/Social data mining techniques/C#/C#_Canada_job_listings.csv");
            Console.WriteLine(jsonobj);
            System.IO.File.WriteAllText(@"D:/Bdat_Course_Material/Semister 2/Social data mining techniques/C#/C#_Canada_job_listings.json", jsonobj);
            //inserting data to Mongo DB
            var mongoDBConnection = new MongoDBCOnnection();
            mongoDBConnection.InsertData();
            Console.WriteLine("Mongo DB Insert Done");

        }


    }
}
