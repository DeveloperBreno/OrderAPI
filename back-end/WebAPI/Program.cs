using Insfraestrutura.Configuracoes;
using Microsoft.EntityFrameworkCore;
using Dominio.Interfaces.Genericos;
using Insfraestrutura.Repositorio.Genericos;
using Dominio.Interfaces;
using Insfraestrutura.Repositorio;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Aplicacao.Interfaces;
using Aplicacao.Aplicacoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Recupera a string de conexão do appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext ao contêiner de serviços
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

// Adiciona os serviços de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Contexto>()
    .AddDefaultTokenProviders();

// Interface e repositório
builder.Services.AddScoped(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped(typeof(INoticia), typeof(RepositorioNoticia));
builder.Services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

// Serviço domínio
builder.Services.AddScoped<IServicoNoticia, ServicoNoticia>();

// Interface aplicação
builder.Services.AddScoped<IAplicacaoNoticia, AplicacaoNoticia>();
builder.Services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

// JWT
// muda no usuarioController tambem
var key = "Secret_Key-12345678_Secret_Key-12345678";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Teste.Securiry.Bearer",
            ValidAudience = "Teste.Securiry.Bearer",
            IssuerSigningKey = JwtSecurityKey.Create(key)
        };

        option.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(b => b.WithOrigins("https://google.com", "https://microsoft.com"));

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