using GraphiQl;
using GraphQL.Server;
using GraphQL.Types;
using GraphQlProject.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.AddGraphQL(options =>
{
    options.EnableMetrics = false;
}).AddSystemTextJson();

builder.Services.AddDbContext<GraphQLDbContext>(option => option.UseSqlServer(@"Data Source= (localdb)\MSSQLLocalDB;Initial Catalog=GraphQLDb;Integrated Security = True"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();

// Create database if it does not exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GraphQLDbContext>();

    dbContext.Database.EnsureCreated();
}



app.Run();
