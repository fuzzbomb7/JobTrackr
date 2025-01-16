using JobTrackrApi;
using JobTrackrApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.Authority = builder.Configuration.GetValue<string>("Auth0:Authority");
	options.Audience = builder.Configuration.GetValue<string>("Auth0:Audience");
	options.TokenValidationParameters = new TokenValidationParameters
	{
		NameClaimType = ClaimTypes.NameIdentifier
	};
});

builder.Services.AddAuthorization();

SocketsHttpHandler socketsHttpHandler = new SocketsHttpHandler();
socketsHttpHandler.PooledConnectionLifetime = TimeSpan.FromMinutes(5);
builder.Services.AddSingleton<SocketsHttpHandler>(socketsHttpHandler);

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
	SocketsHttpHandler socketsHttpHandler = serviceProvider.GetRequiredService<SocketsHttpHandler>();
	CosmosClientOptions cosmosClientOptions = new CosmosClientOptions()
	{
		HttpClientFactory = () => new HttpClient(socketsHttpHandler, disposeHandler: false)
	};

	return new CosmosClient(builder.Configuration.GetConnectionString("Cosmos"), cosmosClientOptions);
});

builder.Services.AddSingleton<ApplyTrackContext>();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

ApplicationEndpoints.Map(app);

app.Run();

