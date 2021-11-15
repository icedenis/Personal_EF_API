using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Personal_EF_API.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Personal_EF_API.Data.Mappings;
using Personal_EF_API.Interfaces;
using Personal_EF_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Personal_EF_API
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //here i deleted the emeil token request and we add here the Role base as well .AddRoles<IdentityRole>()
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //start
            //Here we add Cores to be able to connect to the API FROM other mashiens 
            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            });
            //Here i define Automapper i need 2 packages Automapper and Automapper.Miscrsoft.Dependency...
            services.AddAutoMapper(typeof(Maps));
            //here i add the JWT TOKEN
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience =Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    
                };
                });

            //here we add Swagger services c is the Token
            services.AddSwaggerGen(c => {
                //here is swagger version 1

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Denis Personal_EF_API",
                    Version = "v1",
                    Description = "This is Bachelor Project API for Personal use"
                });

                // here we add XML Document for Swagger I can reuse it for all project Documenation when i enable on the build properties the documentation
                var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xpath = Path.Combine(AppContext.BaseDirectory, xfile);
                c.IncludeXmlComments(xpath);

            });
            // here we add Nlog Sevices
            services.AddSingleton<ILoggerService, LoggerService>();

            //here i add my Iauthor repository and Author services to connect the database 
            services.AddScoped<IAuthoRepository, AuthorRepository>();
            //here i add book repository
            services.AddScoped<IBookRepository, BookRepository>();
            // end




            // Here we add this  .AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); cuz m.b the book lsit is empty 
            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            ,UserManager<IdentityUser> user_manager
            ,RoleManager<IdentityRole> role_manager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Swagger initiation to use it 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Denis Personal EF API");
                c.RoutePrefix = "";
            });

            //here add to Cores Policys
            app.UseCors("CorsPolicy");
            //here i use .Wait its good to work around async methods
            CoreData.Core(user_manager, role_manager).Wait();
            //here i add my core data test users


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // here i change endpoint from Pages to Controllers
                endpoints.MapControllers();
            });
        }
    }
}
