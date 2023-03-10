using HashidsNet;
using LivroReceitas.API.Filtros;
using LivroReceitas.API.Filtros.Swagger;
using LivroReceitas.API.Filtros.UsuarioLogado;
using LivroReceitas.API.Middleware;
using LivroReceitas.API.WebSockets;
using LivroReceitas.Application;
using LivroReceitas.Application.Servicos.AutoMapper;
using LivroReceitas.Domain.Extension;
using LivroReceitas.Infra;
using LivroReceitas.Infra.AcessoRepositorio;
using LivroReceitas.Infra.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => 
{
	option.OperationFilter<HashidsOperationFilter>();
	option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Livro de receitas API", Version = "1.0" });
	option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		Scheme = "Bearer",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "JWT Authorization header utilizando o Bearer scheme. Example: \"Authorization: Bearer {token}\""
	});
	option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			System.Array.Empty<string>()
		}
	});
});

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options=> options.Filters.Add(typeof(FiltroDasExceptions)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg=>
{
	cfg.AddProfile(new AutoMapperConfig(provider.GetService<IHashids>()));

}).CreateMapper());


builder.Services.AddScoped<IAuthorizationHandler, UsuarioLogadoHandler>();
builder.Services.AddAuthorization(option =>
{
	option.AddPolicy("UsuarioLogado", policy => policy.Requirements.Add(new UsuarioLogadoRequirement()));
});

builder.Services.AddScoped<UsuarioAutenticadoAttribute>();


builder.Services.AddSignalR();
builder.Services.AddHealthChecks().AddDbContextCheck<Context>();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions 
{
	AllowCachingResponses= false,
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
		
	}
});


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualizarBD();

app.UseMiddleware<CultureMiddleware>();

app.MapHub<AdicionarConexao>("/addConexao");

app.Run();

void AtualizarBD()
{
	using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
	using var context = serviceScope.ServiceProvider.GetService<Context>();

	bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

	if(!databaseInMemory.HasValue || !databaseInMemory.Value)
	{
		var conexao = builder.Configuration.GetConnection();
		var db = builder.Configuration.GetNomeDB();
		Database.CriarDB(conexao, db);

		app.MigrateDb();
	}		
}

#pragma warning disable CA1050, S3903, S1118
public partial class Program { }
#pragma warning disable CA1050, S3903, S1118