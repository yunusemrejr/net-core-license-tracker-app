using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
 using MySqlDatabase; // Ensure you have the correct using statement for your namespace
using MaxMind.Db;
using System.Net;
 using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
 using System.Threading.Tasks;
 using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("LoggedInPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set the expiration time to 1 hour
    });


var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/wwwroot", // Optional URL path prefix for static files.
    OnPrepareResponse = context =>
    {
        if (context.File.Name.Equals("panel.html", StringComparison.OrdinalIgnoreCase))
        {
            // Prevent serving "panel.html"
            context.Context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Context.Response.ContentLength = 0;
            context.Context.Response.Body = Stream.Null;
        }
    }
});
app.MapGet("/", (HttpContext context) =>
{
    string htmlContent = File.ReadAllText("wwwroot/index.html");
    context.Response.ContentType = "text/html; charset=utf-8"; // Set the content type

    return htmlContent;
});

app.MapGet("/general.css", (HttpContext context) =>
{
    string styleContent = File.ReadAllText("wwwroot/general.css");
    context.Response.ContentType = "text/css"; // Set the content type to CSS

    return context.Response.WriteAsync(styleContent);
});
 
app.MapGet("/{imageName}.png", async (HttpContext context) =>
{
    // Get the requested image name from the route parameters
    string? imageName = context.Request.RouteValues["imageName"] as string;

    // Combine the image name with the path to the image directory
    string imagePath = Path.Combine("wwwroot", $"{imageName}.png");

    if (File.Exists(imagePath))
    {
        // Read the image file as bytes
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

        // Set the content type to image/png
        context.Response.ContentType = "image/png";

        // Write the image bytes to the response stream
        await context.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
    }
    else
    {
        // If the requested image doesn't exist, return a 404 Not Found response
        context.Response.StatusCode = 404;
    }
});

app.MapGet("/{imageName}.gif", async (HttpContext context) =>
{
    // Get the requested image name from the route parameters
    string? imageName = context.Request.RouteValues["imageName"] as string;

    // Combine the image name with the path to the image directory
    string imagePath = Path.Combine("wwwroot", $"{imageName}.gif");

    if (File.Exists(imagePath))
    {
        // Read the image file as bytes
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

        // Set the content type to image/png
        context.Response.ContentType = "image/gif";

        // Write the image bytes to the response stream
        await context.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
    }
    else
    {
        // If the requested image doesn't exist, return a 404 Not Found response
        context.Response.StatusCode = 404;
    }
});


app.MapGet("/general.js", (HttpContext context) =>
{
    string styleContent = File.ReadAllText("wwwroot/general.js");
    context.Response.ContentType = "application/javascript"; // Set the content type to JS

    return context.Response.WriteAsync(styleContent);
});
 app.MapGet("/analysis.js", (HttpContext context) =>
{
    string styleContent = File.ReadAllText("wwwroot/analysis.js");
    context.Response.ContentType = "application/javascript"; // Set the content type to JS

    return context.Response.WriteAsync(styleContent);
});

app.MapPost("/login", async (HttpContext context) =>
{
    try
    {
        // Retrieve form data from the POST request
        var form = await context.Request.ReadFormAsync();

        string? username = form["username"];
        string? password = form["password"];

        if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {

       
      
  var mySqlDatabase = new MySqlDatabase.MySqlDatabase("localhost", 3306, "root", "root", "SOME_DATABASE");

    if (mySqlDatabase.IsValidUser(username, password))
{
                    Console.WriteLine("User IS allowed in /panel AND NOW I'M REDIRECTING THERE...");

var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, username),
    // Add other claims as needed
};

var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

// After signing in, redirect to the intended page
context.Response.Redirect("/panel");
}
else
{
    // Display an error message or redirect to a login failure page
    context.Response.Redirect("/?wrong-credentials");
}

 }
 
    }
    catch (Exception ex)
    {
        // Handle any exceptions that may occur during the POST request
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"An error occurred: {ex.Message}");
    }
});



app.MapGet("/panel", context =>
{
    if (!context.User.Identity.IsAuthenticated)
    {

        context.Response.Redirect("/");
        Console.WriteLine("User NOT allowed in /panel but tried to access.");
        return Task.CompletedTask;
    }
    
    Console.WriteLine("User IS allowed in /panel AND successfully accessed.");

  var mySqlDatabase = new MySqlDatabase.MySqlDatabase("localhost", 3306, "root", "root", "SOME_DATABASE");
  string? json=mySqlDatabase.GetLicenseDataAsJson();
  
     string panelHtml = File.ReadAllText("wwwroot/panel.html");
         panelHtml = panelHtml.Replace("%LICENSES_DATA%", json);

    context.Response.ContentType = "text/html; charset=utf-8";
    return context.Response.WriteAsync(panelHtml);
}).RequireAuthorization("LoggedInPolicy");


app.MapGet("/about",(HttpContext context) =>{
string aboutHtml = File.ReadAllText("wwwroot/about.html");
context.Response.ContentType="text/html; charset=utf-8";
return aboutHtml;
});

app.MapGet("/data-analysis", context =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/");
        Console.WriteLine("User NOT allowed in /panel but tried to access.");
        return Task.CompletedTask;
    }

    var referrer = context.Request.Headers["Referer"].ToString();
    if(string.IsNullOrEmpty(referrer) || !referrer.Contains("/panel")){
        context.Response.Redirect("/");
        Console.WriteLine("User tried to access /data-analysis from an unauthorized path.");
        return Task.CompletedTask;
    }
 
     string analyzelHtml = File.ReadAllText("wwwroot/analyze.html");
 
    context.Response.ContentType = "text/html; charset=utf-8";
    return context.Response.WriteAsync(analyzelHtml);
}).RequireAuthorization("LoggedInPolicy");


static string?[] ConvertToJsonDataArray(SendEndpointClass data)
{
    return
    [
         data.ActionType,
        data.FirstName,
        data.LastName,
        data.EmailAddress,
        data.CompanyName,
        data.LicenseName,
        data.LicenseType,
        data.LicenseID,
        data.OldLicenseID,
        data.Quantity,
        data.LicenseExpirationDate,
        data.AspInvoiceDate,
        data.AspPaymentDueDate,
        data.YunusPaymentTerm,
        data.YunusPaymentAmountUSD,
        data.YunusPaymentStatus,
        data.PoNumber,
        data.YunusInvoiceNumber,
        data.Sector,
        data.SiOrEndUser,
        data.AspInvoice,
        data.AspPaymentTerm,
        data.CompanyMadePayment,
        data.EnteredIntoFinancialSystem,
        data.ForeignInstructionDate,
        data.Notes,
        data.SQL_ID
     ];
}

app.MapPost("/send", async (HttpContext context) =>
{
    string ActionType = context.Request.Query["type"].ToString();
    var response = new { Status = "Error", Message = "An error occurred" };

    try
    {
        if (context.Request.ContentType != null && context.Request.ContentType.ToLower().Contains("application/json"))
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var jsonArray = JsonConvert.DeserializeObject<List<List<string>>>(requestBody);

            if (jsonArray != null && jsonArray.All(item => item.Count == 2))
            {
                var jsonData = MapJsonArrayToEndpointClass(jsonArray);
                jsonData.ActionType = ActionType;
                      // Convert jsonData to a string array for SaveData
        string?[] fields = ConvertToJsonDataArray(jsonData);

       var mySqlDb = new MySqlDatabase.MySqlDatabase("localhost", 3306, "root", "root", "SOME_DATABASE");

                int result = mySqlDb.SaveData(fields);

 if (result > 0)
        {
            response = new { Status = "Success", Message = "Data processed and saved successfully" };
        }
        else
        {
            response = new { Status = "Error", Message = "Failed to save data" };
        }

 
              
                // Add your data processing logic here, for example:
                // SaveToDatabase(jsonData);
            }
            else
            {
                response = new { Status = "Error", Message = "Invalid JSON format" };
            }
        }
        else
        {
            response = new { Status = "Error", Message = "Incorrect Content-Type" };
        }
    }
    catch (Exception ex)
    {
        response = new { Status = "Error", Message = $"Exception: {ex.Message}" };
    }
    finally
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

static SendEndpointClass MapJsonArrayToEndpointClass(List<List<string>> jsonArray)
{
    var endpointData = new SendEndpointClass();

    foreach (var item in jsonArray)
    {
        string key = item[0];
        string value = item[1];

switch (key)
{
    case "txtAd":
        endpointData.FirstName = value;
        break;
    case "txtSoyad":
        endpointData.LastName = value;
        break;
    case "txtEmail":
        endpointData.EmailAddress = value;
        break;
    case "txtFirma":
        endpointData.CompanyName = value;
        break;
    case "cmbLisansIsmi":
        endpointData.LicenseName = value;
        break;
    case "cmbLisansTuru":
        endpointData.LicenseType = value;
        break;
    case "txtLisansId":
        endpointData.LicenseID = value;
        break;
    case "txtOldLisansId":
        endpointData.OldLicenseID = value;
        break;
    case "txtAdet":
        endpointData.Quantity = value;
        break;
    case "dtpLisansBitisTarihi":
        endpointData.LicenseExpirationDate = value;
        break;
    case "dtpAspInvoiceDate":
        endpointData.AspInvoiceDate = value;
        break;
    case "dtpAspVadeBitisTarihi":
        endpointData.AspPaymentDueDate = value;
        break;
    case "txtYunusVade":
        endpointData.YunusPaymentTerm = value;
        break;
    case "txtYunusPayi":
        endpointData.YunusPaymentAmountUSD = value;
        break;
    case "txtPoNumber":
        endpointData.PoNumber = value;
        break;
    case "txtYunusInvoiceNo":
        endpointData.YunusInvoiceNumber = value;
        break;
    case "cmbSektor":
        endpointData.Sector = value;
        break;
    case "cmbSiEndUser":
        endpointData.SiOrEndUser = value;
        break;
    case "txtAspInvoice":
        endpointData.AspInvoice = value;
        break;
    case "txtAspVadeSuresi":
        endpointData.AspPaymentTerm = value;
        break;
    case "dtpYurtdisiTalimatTarihi":
        endpointData.ForeignInstructionDate = value;
        break;
    case "txtNot":
        endpointData.Notes = value;
        break;
    case "cmbYunusOdemeDurumu":
        endpointData.YunusPaymentStatus = value;
        break;
    case "cmbFirmaOdemesi":
        endpointData.CompanyMadePayment = value;
        break;
    case "cmbFinansSistemi":
        endpointData.EnteredIntoFinancialSystem = value;
        break;
    case "SQL_ID":
        endpointData.SQL_ID = value;
        break;
    default:
        // Optionally handle unknown keys
        Console.WriteLine($"Unknown key: {key}");
        break;
}
    }

    return endpointData;
}



app.MapGet("/{*path}", (HttpContext context) =>
{
    string htmlContent = File.ReadAllText("wwwroot/404.html");
    context.Response.ContentType = "text/html; charset=utf-8"; // Set the content type

    return htmlContent;
});
 

  
var allowedCountries = new List<string> { "US" }; // Only allow traffic from USA
    app.UseMiddleware<CountryRestrictionMiddleware>(allowedCountries);

 app.Run();
 

public class CountryRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _allowedCountries;

    public CountryRestrictionMiddleware(RequestDelegate next, List<string> allowedCountries)
    {
        _next = next;
        _allowedCountries = allowedCountries;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            string? visitorIpAddress = context.Connection.RemoteIpAddress?.ToString();
            string? visitorCountry = GetCountryFromIpAddress(visitorIpAddress);

            if (!string.IsNullOrEmpty(visitorCountry) && !_allowedCountries.Contains(visitorCountry))
            {
                Console.WriteLine("Forbidden COUNTRY!!!!");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                 context.Response.Redirect("https://www.yunusemrevurgun.com");

                await context.Response.WriteAsync("Access from your country is not allowed.");
                return;
             }else{
                                Console.WriteLine("ACCEPTED ALLOWED COUNTRY!!!!");

             }
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., invalid IP or database errors.
            // Log the error or handle it as needed.
            Console.WriteLine($"Error: {ex.Message}");
        }

        await _next(context);
    }

private string GetCountryFromIpAddress(string ipAddress)
{
    try
    {
            using var reader = new DatabaseReader("GeoLite2-Country.mmdb");
            var response = reader.Country(IPAddress.Parse(ipAddress));
            return response.Country.IsoCode;
        }
    catch (AddressNotFoundException)
    {
        // Handle cases where the IP address is not found in the database
        return null;
    }
    catch (Exception ex)
    {
        // Handle other exceptions, e.g., database errors
        Console.WriteLine($"Error: {ex.Message}");
        return null;
    }
}
}


public class SendEndpointClass
{
    public string? ActionType { get; set; }

     public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? CompanyName { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseType { get; set; }
    public string? LicenseID { get; set; }
    public string? OldLicenseID { get; set; }
    public string? Quantity { get; set; }
    public string? LicenseExpirationDate { get; set; }
    public string? AspInvoiceDate { get; set; }
    public string? AspPaymentDueDate { get; set; }
    public string? YunusPaymentTerm { get; set; }
    public string? YunusPaymentAmountUSD { get; set; }
    public string? YunusPaymentStatus { get; set; }
    public string? PoNumber { get; set; }
    public string? YunusInvoiceNumber { get; set; }
    public string? Sector { get; set; }
    public string? SiOrEndUser { get; set; }
    public string? AspInvoice { get; set; }
    public string? AspPaymentTerm { get; set; }
    public string? CompanyMadePayment { get; set; }
    public string? EnteredIntoFinancialSystem { get; set; }
    public string? ForeignInstructionDate { get; set; }
    public string? Notes { get; set; }
        public string? SQL_ID { get; set; }

}