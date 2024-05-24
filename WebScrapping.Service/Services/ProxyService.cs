using System.Net;
using OpenQA.Selenium.Chrome;
using SharedLibrary;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace WebScrapping.Service.Services;

public class ProxyService:IDisposable
{
    private readonly ProxyServer _proxyServer;
    private ChromeDriver _driver;

    public ProxyService()
    {
        _proxyServer = new ProxyServer();
        _proxyServer.CertificateManager.CertificateEngine = Titanium.Web.Proxy.Network.CertificateEngine.DefaultWindows;
        _proxyServer.CertificateManager.EnsureRootCertificate();
        _proxyServer.BeforeRequest += OnRequest;
        _proxyServer.BeforeResponse += OnResponse;

        var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Parse(ProxyConfig.ProxyAddress), ProxyConfig.ProxyPort, true);
        _proxyServer.AddEndPoint(explicitEndPoint);

        var httpClientHandler = new HttpClientHandler
        {
            Proxy = new WebProxy(),
            UseProxy = true
        };
    }

    public void Start()
    {
        _proxyServer.Start();
        Console.WriteLine("Proxy server started...");
        //SendHttpRequestThroughProxy().Wait();
    }
    
    public void Stop()
    {
        Console.WriteLine("Stopping proxy server...");
        _proxyServer.Stop();
        _proxyServer.BeforeRequest -= OnRequest;
        _proxyServer.BeforeResponse -= OnResponse;
        Console.WriteLine("Proxy server stopped.");
    }
    
    private Task OnRequest(object sender, SessionEventArgs e)
    {
        Console.WriteLine($"outgoing request {e.HttpClient.Request.Url}");
        return Task.CompletedTask;
    }

    private Task OnResponse(object sender, SessionEventArgs e)
    {
        if (e.HttpClient.Response.HeaderText.Contains("application/json"))
        {
            Console.WriteLine(e.HttpClient.Request.Url);
        }
        return Task.CompletedTask;
    }

    // private async Task SendHttpRequestThroughProxy()
    // {
    //     var options = new ChromeOptions();
    //     options.AddArguments("--proxy-server=http://localhost:12345", "--headless");
    //     var path = @"C:\Users\Lenovo\RiderProjects\ProxyTweak\ProxyTweak\chromedriver123.exe";
    //     var chromeDriverService = ChromeDriverService.CreateDefaultService(path);
    //     chromeDriverService.HideCommandPromptWindow = true;
    //
    //     _driver = new ChromeDriver(chromeDriverService, options);
    //
    //     _driver.Navigate().GoToUrl("https://www.trendyol.com/erkek-t-shirt-x-g2-c73?pi=2");
    //
    //     Stop();
    // }


    public void Dispose()
    {
        _proxyServer.Dispose();
        _driver.Dispose();
    }

}