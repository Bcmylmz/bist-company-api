using System.Xml;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using AllCompaniesAPI.Controllers;
using AllCompaniesAPI.Data;
using Microsoft.EntityFrameworkCore;
using AllCompaniesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DbExtensions;
using StackExchange.Redis;
using Hangfire.Storage;
using System.Text.Json;
public interface ICompaniesJob
{
    void ExecuteAsync();
}


public class CompaniesJob : ICompaniesJob
{
    private readonly IConnectionMultiplexer _redis;
    private readonly DataContext _context;
    public CompaniesJob(DataContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }
    
   


    public async void ExecuteAsync()
    {
        var models = new List<Company>();
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load("https://www.kap.org.tr/tr/bist-sirketler");
        HtmlNodeCollection comprows = doc.DocumentNode.SelectNodes("//div[contains(@class, 'comp-row')]");



        foreach (HtmlNode item in comprows)
        {
            string url = item.SelectSingleNode("./div[1]//a").Attributes["href"].Value;
            string code = item.SelectSingleNode("./div[1]").InnerText.Trim();
            string companyName = item.SelectSingleNode("./div[2]").InnerText.Trim();
            string city = item.SelectSingleNode("./div[3]").InnerText.Trim();
            string independentAuditingFirm = item.SelectSingleNode("./div[4]").InnerText.Trim();
            var model = new Company() { Url = url, Code = code, CompanyName = companyName, City = city, IndependentAuditingFirm = independentAuditingFirm };
            models.Add(model);


            
            
        }
        _context.MultipleAddOrUpdate(models);

        var db = _redis.GetDatabase();

        foreach (var model in models)
        {
            string jsonData = JsonSerializer.Serialize(model);
       
            await db.StringSetAsync(model.Code, jsonData);
            
        }

        Console.WriteLine("Successfully completed!");
    }
}

