using System.Net;
using Newtonsoft.Json;

var test = new List<string>();
var option = new ParallelOptions { MaxDegreeOfParallelism = 4 };

Parallel.For(0, 1000, (i, loopState) =>
{
    //Console.WriteLine(i);
    //Console.WriteLine(loopState);
    string functionName = "api/Booking/Booking";

    var postData = new
    {
        OperatorUserId = 168905,
        PurchaseOrderId = 316513,
        TeacherId = 2854,
        StudentId = 129603,
        StartTime = "2022-05-18T04:00:00",
        EndTime = "2022-05-18T04:50:00",
        Date = "2022-05-18T00:00:00",
        MaxCount = 1,
        TeachingType = 4,
    };
    string jsonString = JsonConvert.SerializeObject(postData);

    CallStudentCenterAPI("POST", functionName, jsonString);
});

//concurrenQueue();

//var qoo = new Queue<string>();
//qoo.Enqueue("i am coming");
//qoo.Dequeue();

void CallStudentCenterAPI(string method, string action, string jsonString)
{
    string val = "";
    if (method == "GET" && jsonString != null && jsonString.Length != 0)
    {
        val = "?";
        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        foreach (KeyValuePair<string, string> item in values)
        {
            val += string.Format("{0}={1}&", item.Key, item.Value);
        }
        val = val.Remove(val.Length - 1);
    }

    string url = "https://localhost:44328/" + action + val;
    try
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        httpWebRequest.PreAuthenticate = true;
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = method;
        string AccessToken = "Your Token Example:eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.xxx.xxx";
        httpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        if (jsonString != null && method == "POST")
        {
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonString);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(result);
            //return result;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}