using System;
using System.Text;
using API.Data;
using API.Entities;
using API.Repositories;
using API.Services;
using API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration Config;

        public Startup(IConfiguration Config)
        {
            this.Config = Config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            String dbSocketDir = Environment.GetEnvironmentVariable("DB_SOCKET_PATH") ?? "/cloudsql";
            String instanceConnectionName = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME");
            services.AddDbContext<DataContext>(options =>
            {
                if(Environment.GetEnvironmentVariable("DB_USER") != null && Environment.GetEnvironmentVariable("DB_PASS") != null 
                && Environment.GetEnvironmentVariable("DB_NAME") != null && instanceConnectionName != null){
                    options.UseNpgsql($"Host={String.Format("{0}/{1}", dbSocketDir, instanceConnectionName)};Database={Environment.GetEnvironmentVariable("DB_NAME")};Username={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("DB_PASS")}");
                }else{
                    options.UseNpgsql(Config.GetConnectionString("DefaultConnection"));
                }
            });

            var jwtConfig = Config.GetSection("JwtConfig").Get<JwtConfig>();
            var emailConfig = Config.GetSection("EmailStrings").Get<EmailConfig>();
            services.AddSingleton(jwtConfig);

            


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidAudience = jwtConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            

            services.AddTransient<IGenericUserRepository<User>, UserRepository>();
            services.AddTransient<IGenericUserRepository<Admin>, AdminRepository>();
            services.AddTransient<IGenericUserRepository<Registrar>, RegistrarRepository>();
            services.AddTransient<IGenericUserRepository<Doctor>, DoctorRepository>();
            services.AddTransient<IGenericUserRepository<LabTechnician>, LabTechnicianRepository>();
            services.AddTransient<IGenericUserRepository<LabManager>, LabManagerRepository>();

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();

            services.AddTransient<IAppointmentRepository, AppointmentRepository>();

            services.AddTransient<IExaminationCodeRepository, ExaminationCodeRepository>();
            services.AddTransient<IPhysicalExaminationRepository, PhysicalExaminationRepository>();
            services.AddTransient<ILabExaminationRepository, LabExaminationRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IExaminationCodeService, ExaminationCodeService>();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddSingleton<IMailService>(x => 
                new MailService(emailConfig));

            services.AddTransient<IPhysicalExaminationService, PhysicalExaminationService>();
            services.AddTransient<ILabExaminationService, LabExaminationService>();

            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddTransient<DbSeeder>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseCors(builder => builder.WithOrigins("https://przychodnia---frontend-5yssyu7a2a-lz.a.run.app","https://localhost:3000","http://localhost:3000").AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
