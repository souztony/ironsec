var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger sempre ligado
app.UseSwagger();
app.UseSwaggerUI();

// Middlewares
app.UseAuthorization();

// Mapear controllers
app.MapControllers();

app.Run();