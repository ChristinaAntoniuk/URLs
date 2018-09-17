using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class UrlDetailsController : Controller
    {
        // GET: UrlDetails
        public async Task<ActionResult> Index(URL model)
        {
            string[] urls = model.URLsSet.Split('\n');
            model.URLs = new string[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                model.URLs[i] = urls[i].Trim('\r', '\n');
            }
            model = await GetInfo(model);
            return View("../Home/UrlDetails", model);
        }

        static async Task<URL> GetInfo(URL model)
        {
            HttpClient client = new HttpClient();
            var tasksString = new List<Task<string>>();
            var tasksCodes = new List<Task<HttpResponseMessage>>();
            foreach (string url in model.URLs) { 

                tasksString.Add(client.GetStringAsync(url));
                tasksCodes.Add(client.GetAsync(url));

            }   
            try
            {
                await Task.WhenAll(tasksString.ToArray());
                await Task.WhenAll(tasksCodes.ToArray());
            }
            catch { }
            model.Titles = new string[model.URLs.Length];
            model.ResStatusCodes = new string[model.URLs.Length];

            for (int i = 0; i < model.URLs.Length; i++)
            {
                if (!tasksString[i].IsFaulted)
                    {
                        model.Titles[i] = Regex.Match(tasksString[i].Result, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                        model.ResStatusCodes[i] = tasksCodes[i].Result.StatusCode.ToString();
                    }
                else
                    {
                        model.Titles[i] = "Faulted";
                        model.ResStatusCodes[i] = "Faulted";
                    }
            }
            return model;
        }
    }
}