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
using Dominio.Interfaces.Filas;
using Insfraestrutura.Filas;
using RabbitMQ.Client;
using WebAPI.Controllers.v1;

var builder = WebApplication.CreateBuilder(args);

// Adiciona a pol�tica CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Recupera a string de conex�o do appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext ao cont�iner de servi�os
builder.Services.AddDbContext<Contexto>(options =>
    options.UseNpgsql(connectionString));

// Adiciona os servi�os de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Contexto>()
    .AddDefaultTokenProviders();

// Interface e reposit�rio
builder.Services.AddScoped(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped(typeof(INoticia), typeof(RepositorioNoticia));
builder.Services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

// Servi�o dom�nio
builder.Services.AddScoped<IServicoNoticia, ServicoNoticia>();

// Interface aplica��o
builder.Services.AddScoped<IAplicacaoNoticia, AplicacaoNoticia>();
builder.Services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

// logs
builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<UsuarioController>));

builder.Services.AddSignalR();

// Configurar RabbitMQ
try
{
    builder.Services.AddSingleton<IConnection>(sp =>
    {
        var factory = new ConnectionFactory
        {
            HostName = "127.0.0.1:4369",
            UserName = "guest",
            Password = "guest",
        };
        return factory.CreateConnection();
    });

}
catch (Exception e)
{
    Console.WriteLine("Message: " + e.Message);
    Console.WriteLine("InnerException: " + e.InnerException);
    Console.WriteLine("StackTrace: " + e.StackTrace);
}

builder.Services.AddScoped<IInsereNaFila, InserirNaFila>();

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
builder.Services.AddLogging();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chathub"); // Replace ChatHub with your Hub class

app.UseCors("AllowAll");

//app.UseRequestTimeout(TimeSpan.FromSeconds(30)); // Adicione o middleware de tempo limite aqui

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();