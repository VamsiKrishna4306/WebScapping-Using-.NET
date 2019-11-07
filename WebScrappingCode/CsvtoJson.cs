using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Canada_Job_Bank_Web_Scrapping1
{
    class CsvtoJson
    {
        public string ConvertCsvFileToJsonObject(string path)
        {
            // reading csv file data as new list object
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(path);
            // splitting datafields by delimiter 
            foreach (string line in lines)
                csv.Add(line.Split('|'));


            // getting field names from line 1
            var properties = lines[0].Split("|");

            var listObjResult = new List<Dictionary<string, string>>();
            // converting all lines to list of dictionary format
            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }
            // returning the serialized json object
            return JsonConvert.SerializeObject(listObjResult);
        }

    }
}
