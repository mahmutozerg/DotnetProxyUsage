using System.Net;
using OpenQA.Selenium.Chrome;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

var proxyServer = new ProxyServer();

proxyServer.CertificateManager.CertificateEngine = Titanium.Web.Proxy.Network.CertificateEngine.DefaultWindows; 
proxyServer.CertificateManager.EnsureRootCertificate();

var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Parse("127.0.0.1"), 12345, true);
proxyServer.AddEndPoint(explicitEndPoint);
proxyServer.Start();

proxyServer.BeforeRequest += OnRequest;
proxyServer.BeforeResponse += OnResponse;

await SendHttpRequestThroughProxy();



Task OnRequest(object sender, SessionEventArgs e)
{
    return Task.CompletedTask;
}

// Handle responses.
Task OnResponse(object sender, SessionEventArgs e)
{
    if (e.HttpClient.Response.HeaderText.Contains("application/json"))
    {
        Console.WriteLine(e.HttpClient.Request.Url);
    }
    return Task.CompletedTask;
}
void StopProxyServer(ProxyServer proxyServer)
{
    Console.WriteLine("Stopping proxy server...");
    proxyServer.Stop();
    proxyServer.BeforeRequest -= OnRequest;
    proxyServer.BeforeResponse -= OnResponse;
    Console.WriteLine("Proxy server stopped.");
}

async Task SendHttpRequestThroughProxy()
{
    var options = new ChromeOptions();
    options.AddArguments("--proxy-server=http://localhost:12345","--headless");
    var path = @"C:\Users\Lenovo\RiderProjects\ProxyTweak\ProxyTweak\chromedriver123.exe";
    var chromeDriverService = ChromeDriverService
        .CreateDefaultService(path);
    chromeDriverService.HideCommandPromptWindow = true;

    var driver = new ChromeDriver(chromeDriverService,options);

    // Perform your Selenium actions
    driver.Navigate().GoToUrl("https://www.trendyol.com/erkek-t-shirt-x-g2-c73?pi=2");

    StopProxyServer(proxyServer);
}