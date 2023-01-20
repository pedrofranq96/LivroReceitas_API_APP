using LivroReceitas.API.Filtros;
using LivroReceitas.Application;
using LivroReceitas.Application.Servicos.AutoMapper;
using LivroReceitas.Domain.Extension;
using LivroReceitas.Infra;
using LivroReceitas.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositorio(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options=> options.Filters.Add(typeof(FiltroDasExceptions)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg=>
{
	cfg.AddProfile(new AutoMapperConfig());

}).CreateMapper());

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualizarBD();

app.Run();

void AtualizarBD()
{
	var conexao = builder.Configuration.GetConnection();
	var db = builder.Configuration.GetNomeDB();
	Database.CriarDB(conexao, db);

	app.MigrateDb();
}
