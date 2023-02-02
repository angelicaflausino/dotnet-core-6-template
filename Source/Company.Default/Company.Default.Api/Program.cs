using Company.Default.Infra.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.IdentityModel.Tokens.Jwt;

namespace Company.Default.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string[] scopes = builder.Configuration.GetValue<string>("DowstreamApi:Scopes")?.Split(' ');

            //This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            //Add Microsoft Identity Platform (AAD v2.0) support to protect this Api
            //builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
            
            //Configure Web API calls Microsoft.Graph
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd")
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph(builder.Configuration.GetSection("Graph"))
                .AddInMemoryTokenCaches();
                            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Register Project Services
            // Register Infrastructure
            builder.Services.AddInfrastructure();
            //Register Core
            builder.Services.AddCore();
            //Register Cloud
            builder.Services.AddCloud(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}