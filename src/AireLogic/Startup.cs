using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AireLogic.Dto;
using AireLogic.Models;
using AireLogic.Repositories;
using AireLogic.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AireLogic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<IssueContext>(options => options.UseSqlServer(@"Server=127.0.0.1,1433;Database=airelogic;User Id=sa;Password=Passw0rd;"));
            // services.AddDbContext<PersonContext>(options => options.UseSqlServer(@"Server=127.0.0.1,1433;Database=airelogic;User Id=sa;Password=Passw0rd;"));
            services.AddDbContext<IssueContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddDbContext<PersonContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<IPersonRepository, PersonDbRepository>();
            services.AddScoped<IIssueRepository, IssueDbRepository>();
            services.AddMvc()
                    .AddJsonOptions(options => { options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors("AllowAllOrigins");

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Issue, IssueDto>().ReverseMap();
                mapper.CreateMap<Issue, IssueCreateDto>().ReverseMap();
                mapper.CreateMap<Issue, IssueUpdateDto>().ReverseMap();
                mapper.CreateMap<Issue, IssueViewModel>().ReverseMap();
                mapper.CreateMap<Person, PersonDto>().ReverseMap();
                mapper.CreateMap<Person, PersonCreateDto>().ReverseMap();
                mapper.CreateMap<Person, PersonUpdateDto>().ReverseMap();
                mapper.CreateMap<Person, PersonViewModel>().ReverseMap();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
