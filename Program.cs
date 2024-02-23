using System.Text;
using BlogAspNet;
using BlogAspNet.Data;
using BlogAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();

void ConfigureAuthentication(WebApplicationBuilder webApplicationBuilder)
{
    Configuration.JwyKey = webApplicationBuilder.Configuration.GetValue<string>("jwtKey");
    var key = Encoding.ASCII.GetBytes(Configuration.JwyKey);
    webApplicationBuilder.Services
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}

void ConfigureMvc(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
}

void ConfigureServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddDbContext<BlogDataContext>();
    webApplicationBuilder.Services.AddTransient<TokenService>();    //Sempre criar um novo
    //builder.Services.AddScoped();                                 //Por transação
    //builder.Services.AddSingleton();                              //Um por App!
    webApplicationBuilder.Services.AddTransient<EmailService>();
}

void LoadConfiguration(WebApplication webApplication)
{
    
    Configuration.ApiKeyName = webApplication.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = webApplication.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}