using System;
using System.Net.Http;

namespace CheckRunningTime
{
    class Program
    {
        static void Main(string[] args)
        {
            var startAt = DateTime.Now;
            try
            {
                while (true)
                {
                    HttpClient client = new HttpClient();
                    var result = client.GetAsync("https://bringauto.azurewebsites.net/").Result;
                    result.EnsureSuccessStatusCode();
                    Console.WriteLine($"{DateTime.Now} -  {result.Content.ReadAsStringAsync().Result} : Total minutes = {(DateTime.Now - startAt).TotalMinutes}");
                }
            }
            catch
            {
                Console.WriteLine($"{startAt} - {DateTime.Now} : Total minutes = {(DateTime.Now - startAt).TotalMinutes}");
            }
        }
    }
}
