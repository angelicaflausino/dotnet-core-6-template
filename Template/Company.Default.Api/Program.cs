using $ext_safeprojectname$.Domain.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace $safeprojectname$
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Configure custom API settings
            ApiOptions.ConfigureApiOptions(builder.Configuration);

            //Azure AD Authentication
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            builder.Services.AddAuthenticationAzureWithMicrosoftGraph(builder.Configuration);

            //Application Insights
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddLogging();
            
            builder.Services.AddControllers();

            //Register Project Services
            builder.Services.AddInfrastructure();
            builder.Services.AddCore();
            builder.Services.AddCloud(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.CustomSchemaIds(type => type.FullName);
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api Name", Version = "v1" });
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}