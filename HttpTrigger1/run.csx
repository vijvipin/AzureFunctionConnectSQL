#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string name = req.Query["name"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    name = name ?? data?.name;

    //var connectionString  = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
    var connectionString  = "Server=tcp:functestservr.database.windows.net,1433;Initial Catalog=functiontest;Persist Security Info=False;User ID=vijadmin;Password=Myadmin@235;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    using(var connection = new SqlConnection(connectionString))
        {
            //Opens Azure SQL DB connection.
            connection.Open();
            var querytext = "insert into blobfileInformation (fileFirstName) values ('DefaultFile')";
            log.LogInformation(querytext);

            using (SqlCommand cmd = new SqlCommand(querytext, connection))
        {
            // Execute the command and log the # rows affected.
            var rows = await cmd.ExecuteNonQueryAsync();
            //log.LogInformation($"{rows} rows were updated");
        }

        }

    return name != null
        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
}
