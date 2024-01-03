using kanbanBoard;
using System.Text;
using kanban.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace kanban
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            }
           );

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            static string GenerateTokenKey(int keyLength)
            {
                byte[] randomBytes = new byte[keyLength];

                using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
                {
                    rngCrypto.GetBytes(randomBytes);
                }

                return BitConverter.ToString(randomBytes).Replace("-", "");
            }

            int keyLength = 32;

            // Cria uma chave criptograficamente segura
            string tokenKey = GenerateTokenKey(keyLength);
            var key = Encoding.ASCII.GetBytes(tokenKey);

            builder.Services.AddAuthentication
            (
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
             ).AddJwtBearer
             (
                x =>
                {
                    x.RequireHttpsMetadata = false; //quando for publicar esse projeto, essa linha precisa ser comentada, pois o false é usado apenas no ambiente de desenvolvimento.
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //Esses dois abaixo também precisam ser alterados no ambiente de produção. Tornam-se true e são usados para mitigação de invasores.
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                }
             );

            builder.Services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(tokenKey));
            builder.Services.AddDbContext<kanbanContext>(); //libera a injeção de dependencia

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins); //ADD Politica de segurança


            app.MapControllers();

            app.Run();
        }
    }
}