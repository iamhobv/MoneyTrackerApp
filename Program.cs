
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyTrackerApp.Data;
using MoneyTrackerApp.Models;
using MoneyTrackerApp.Interfaces;
using MoneyTrackerApp.Services;
using System.Diagnostics;
using AutoMapper;
using Hangfire;

namespace MoneyTrackerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IGeneralRepository<Category>, GeneralRepository<Category>>();
            builder.Services.AddScoped<IGeneralRepository<Expense>, GeneralRepository<Expense>>();
            builder.Services.AddScoped<IGeneralRepository<Income>, GeneralRepository<Income>>();
            builder.Services.AddScoped<IGeneralRepository<RecurringTransaction>, GeneralRepository<RecurringTransaction>>();
            builder.Services.AddScoped<IIncomeServices, IncomeSrevices>();
            builder.Services.AddScoped<IExpenseServices, ExpenseServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<IRecurringTransactionServices, RecurringTransactionServices>();

            builder.Services.AddDbContext<DatabaseContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("CS"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .LogTo(log => Debug.WriteLine(log), LogLevel.Information);
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<DatabaseContext>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = builder.Configuration["JWT:Iss"],
                        ValidateAudience = false,
                        ValidAudience = builder.Configuration["JWT:Aud"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    };
                });


            builder.Services.AddHangfire(h => h.UseSqlServerStorage(builder.Configuration.GetConnectionString("CS")));

            builder.Services.AddHangfireServer();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);


            var app = builder.Build();

            MapperServices.Mapper = app.Services.GetService<IMapper>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard("/hangfireDashboard");

            //RecurringJob.AddOrUpdate("TestJob",
            //    () => Console.WriteLine("hello testing hangfire RecurringJob"), Cron.Minutely());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
