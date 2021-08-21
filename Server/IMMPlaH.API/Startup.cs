using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMMPlaH.DataAccess;
using IMMPlaH.Services;
using IMMPlaH.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IMMPlaH.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IPatientService, PatientService>();

            services.AddScoped<ICaregiverService, CaregiverService>();

            services.AddScoped<IMedicationService, MedicationService>();

            services.AddScoped<ICaregiverPatientsService, CaregiverPatientsService>();

            services.AddScoped<IMedicationPlanService, MedicationPlanService>();

            services.AddScoped<IConsumerService, ConsumerService>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors();

            services.AddGrpc(); //A3

            //services.AddHostedService<ConsumerService>();  A2
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); endpoints.MapGrpcService<MedPlanGRPCService>(); });  //endpoints.MapGrpc de la tema 3
        }
    }

}
