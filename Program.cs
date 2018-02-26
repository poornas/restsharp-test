using System;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
using Newtonsoft.Json;

namespace minio.netcoresampleconsoleapp
{


    /// <summary>
    /// This example creates a new bucket if it does not already exist, and uploads a file
    /// to the bucket.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var accessKey = "Q3AM3UQ867SPQQA43P2F";
            var secretKey = "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG";
            var client = new RestClient("https://play.minio.io:9000");
            client.Authenticator = new V4Authenticator(true,accessKey, secretKey);

            var request = new RestRequest("/", Method.GET);
            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Console.Out.WriteLine(content);
            LogRequest(client,response.Request, response);
            Console.ReadLine();
        }

        /// <summary>
        /// Logs the request sent to server and corresponding response 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="durationMs"></param>
        private static void LogRequest(RestClient restClient, IRestRequest request, IRestResponse response)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = restClient.BuildUri(request),
            };

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            Console.Out.WriteLine(string.Format("Request completed :- Request: {0}, Response: {1}",
                    JsonConvert.SerializeObject(requestToLog, Formatting.Indented),
                    JsonConvert.SerializeObject(responseToLog, Formatting.Indented)));
        }

    }


}