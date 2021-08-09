using DltaIngest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesEngine;
using RulesEngine.Models;
using RulesEngine.Extensions;
using DltaIngest.Repository;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DltaIngest.AppService
{
    public class DltaIngestionRuleDriver
    {

        public DltaIngestionRuleDriver(SqlRepository sqlRepository, IConfiguration config)
        {
            _sqlRepository = sqlRepository;
            _config = config;
        }

        public List<SqlDatabase> Databases { get; set; }


        List<Azure> _azureResources = new List<Azure>();

        private readonly SqlRepository _sqlRepository;
        private readonly IConfiguration _config;

        public void Run()
        {


            Log.Information("** Starting the Process **");

            var _sqlModel = new SqlDatabase();


            var _files = Directory.GetFiles(Directory.GetCurrentDirectory(), "SqlInputData.json", SearchOption.AllDirectories);
            if (_files == null || _files.Length == 0)
                throw new Exception("Input data not found.");

            var _inputfileData = File.ReadAllText(_files[0]);

            Log.Information("Collected the Input data ");

            List<SqlDatabase> _sqlModels = JsonConvert.DeserializeObject<List<SqlDatabase>>(_inputfileData);

            Log.Information("DeserializeObject  the Input data to object and total objects :{0}", _sqlModels.Count.ToString());

            foreach (var item in _sqlModels)
            {

                Log.Information("Iterating the collection with item :{0}", item.DatabaseId);

                var _strsqlModel = JsonConvert.SerializeObject(item);

                var converter = new ExpandoObjectConverter();

                Log.Information("Preparing Input parameters");
                dynamic input1 = JsonConvert.DeserializeObject<ExpandoObject>(_strsqlModel, converter);


                var inputs = new dynamic[]
                    {
                    input1,

                    };

                Log.Information($"Searching for rule file");

                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "SqlRules.json", SearchOption.AllDirectories);
                if (files == null || files.Length == 0)
                {
                    throw new Exception("Rules not found.");
                }
                Log.Information($"Rule file found");

                var fileData = File.ReadAllText(files[0]);
                var workflowRules = JsonConvert.DeserializeObject<List<WorkflowRules>>(fileData);

                var bre = new RulesEngine.RulesEngine(workflowRules.ToArray(), null);

                foreach (var workflow in workflowRules)
                {
                    Console.WriteLine($"Iterating over the workflow {workflow.WorkflowName}");

                    var resultList = bre.ExecuteAllRulesAsync(workflow.WorkflowName, item).Result;

                    resultList.OnSuccess((eventName) =>
                    {
                        Console.WriteLine($"{workflow.WorkflowName} evaluation resulted in success - {eventName} for {item.DatabaseId}");
                        updateResult(item, eventName);

                    });

                    resultList.OnFail(() =>
                    {
                        Console.WriteLine($"{workflow.WorkflowName} evaluation resulted in failure for {item.DatabaseId}");
                    });

                }

            }

            Log.Information("Saving to database ");
            _sqlRepository.InsertRecords<Azure>("Azure", _azureResources);

            Console.WriteLine("Done!!!");
        }

        public void updateResult(SqlDatabase basicSQLModel, string eventName)
        {
            Azure _azureResource = new Azure();


            var _sqlModel = _azureResources.Where(x => x.ParentID == basicSQLModel.ParentId).FirstOrDefault();



            if (_sqlModel is null)
            {
                _azureResource.Name = basicSQLModel.SqlServerName;
                _azureResource.ParentID = basicSQLModel.ParentId;
                _azureResource.IngestionTimeStamp = DateTime.Now;
                _azureResource.DeltaID = new Guid().ToString();
                _azureResource.MetaDataApplied = eventName;
                _azureResource.DltaMeta.Add(eventName);
                _azureResources.Add(_azureResource);
            }
            else
            {

                _sqlModel.DltaMeta.Add(eventName);
            }



            string filePath = @"c:\temp\update.txt";
            using (var streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(JsonConvert.SerializeObject(basicSQLModel).ToString());
                streamWriter.Close();
            }
        }

    }
}
