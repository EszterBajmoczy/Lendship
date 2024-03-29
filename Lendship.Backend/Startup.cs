using Lendship.Backend.Authentication;
using Lendship.Backend.Converters;
using Lendship.Backend.DTO.Authentication;
using Lendship.Backend.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Repositories;
using Lendship.Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Lendship.Backend
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
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddDbContext<LendshipDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // For Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<LendshipDbContext>()
                .AddDefaultTokenProviders();

            //redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["redis:connectionString"];
                //options.InstanceName = "lendship";
            });

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddHttpContextAccessor();

            //converters
            services.AddScoped<IAdvertisementConverter, AdvertisementConverter>();
            services.AddScoped<IAvailabilityConverter, AvailabilityConverter>();
            services.AddScoped<ICategoryConverter, CategoryConverter>();
            services.AddScoped<IPrivateUserConverter, PrivateUserConverter>();
            services.AddScoped<IConversationConverter, ConversationConverter>();
            services.AddScoped<IEvaluationAdvertiserConverter, EvaluationAdvertiserConverter>();
            services.AddScoped<IEvaluationLenderConverter, EvaluationLenderConverter>();
            services.AddScoped<IMessageConverter, MessageConverter>();
            services.AddScoped<INotificationConverter, NotificationConverter>();
            services.AddScoped<IReservationConverter, ReservationConverter>();
            services.AddScoped<IUserConverter, UserConverter>();

            //repositories
            services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IEvaluationRepository, EvaluationRepository>();
            services.AddScoped<IImageLocationRepository, ImageLocationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ISavedAdvertisementRepository, SavedAdvertisementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUsersAndConversationsRepository, UsersAndConversationsRepository>();
            services.AddScoped<IPrivateUserRepository, PrivateUsersRepository>();

            //services
            services.AddScoped<IAdvertisementService, AdvertisementService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IEvaluationService, EvaluationService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IPrivateUserService, PrivateUserService>();
            services.AddScoped<IInformationsService, InformationsService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IEvaluationCalculator, WeightedAverage>();
            services.AddScoped<IReputationCalculatorService, ReputationCalculatorService>();
            services.AddScoped<TokenValidator>();
            var serviceProvider = services.BuildServiceProvider();

            // Adding Authentication

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(serviceProvider.GetService<TokenValidator>());
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new CustomTokenValidationParameters(Configuration).GetTokenValidationParameters();
                });

            services.AddScoped<IProfileService, ProfileService>();
            services.AddControllers();

            

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Lendship.Backend",
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter �Bearer� [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowOrigin");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lendship.Backend v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
