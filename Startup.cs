using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using MediatR;
using HomeAutomation.IdentityService.Services;
using HomeAutomation.IdentityService.ApiKeyAuthentication.Options;
using HomeAutomation.IdentityService.ApiKeyAuthentication.Extensions;
using HomeAutomation.IdentityService.ApiKeyAuthentication.Services;
using HomeAutomation.IdentityService.Features.Authentication.ClientAuthentication;

namespace HomeAutomation.IdentityService
{
    public class Startup
    {
        public static SymmetricSecurityKey SecurityKey; 

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var guid = Encoding.ASCII.GetBytes(Configuration.GetSection("AuthenticationGuid").Value);
            SecurityKey = new SymmetricSecurityKey(guid);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(AuthenticateClientCommand));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IApiKeyService, ApiKeyService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HomeAutomation IdentityService API", Version = "v1" });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
            .AddApiKeySupport(options => { });

            //var logDNAToken = Configuration.GetSection("LogDNAToken").Value;
            //var logDNAOptions = new LogDNAOptions(logDNAToken, LogLevel.Debug)
            //    .AddWebItems();

            //logDNAOptions.HostName = "HomeAutomationIdentityService";
            //services.AddLogging(loggingBuilder => loggingBuilder.AddLogDNA(logDNAOptions));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAny");
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeAutomation IdentityService API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
