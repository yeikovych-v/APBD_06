using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers().AddXmlSerializerFormatters();
        builder.Services.AddScoped<IAnimalsRepository, SqlAnimalRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}


public interface IAnimalsRepository
{
    List<Animal> FindAll(string orderBy);
    void Add(Animal animal);
    bool Update(int id, Animal animal);
    bool Delete(int id);

}

public class SqlAnimalRepository(IConfiguration configuration) : IAnimalsRepository
{
    public List<Animal> FindAll(string orderBy)
    {
        Console.WriteLine("In Find All");
        Console.WriteLine($"{orderBy}");

        var animals = new List<Animal>();
        var sqlDataSource = configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(sqlDataSource);

        connection.Open();

        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animals ORDER BY {orderBy} ASC";

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            var idAnimal = Convert.ToInt32(reader["IdAnimal"]);
            var name = reader["Name"].ToString() ?? "";
            var description = reader["Description"].ToString() ?? "";
            var category = reader["Category"].ToString() ?? "";
            var area = reader["Area"].ToString() ?? "";

            var animal = new Animal(name, description, category, area)
            {
                IdAnimal = idAnimal
            };
            animals.Add(animal);
        }

        return animals;
    }

    public void Add(Animal animal)
    {
        Console.WriteLine("In Add");
        Console.WriteLine($"{animal}");
        var sqlDataSource = configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(sqlDataSource);

        connection.Open();

        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText =
            "INSERT INTO Animals (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        command.ExecuteNonQuery();
    }

    public bool Update(int id, Animal animal)
    {
        Console.WriteLine("In Update");
        Console.WriteLine($"Id: {id}, :: {animal}");
        var sqlDataSource = configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(sqlDataSource);

        connection.Open();

        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText =
            "UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
        command.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        command.ExecuteNonQuery();

        return true;
    }

    public bool Delete(int id)
    {
        Console.WriteLine("In Delete");
        Console.WriteLine($"Id: {id}");
        var sqlDataSource = configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(sqlDataSource);

        connection.Open();

        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText =
            "DELETE FROM Animals WHERE IdAnimal = @IdAnimal";
        command.Parameters.AddWithValue("@IdAnimal", id);

        command.ExecuteNonQuery();

        return true;
    }
}

public class Animal
{
    public Animal(string name, string description, string category, string area)
    {
        Name = name;
        Description = description;
        Category = category;
        Area = area;
    }

    public int IdAnimal { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }

    public override string ToString()
    {
        return $"Id: {IdAnimal}, Name: {Name}, Description: {Description}, Category: {Category}, Area: {Area}";
    }
}

[ApiController]
[Route("api/animals")]
public class AnimalController(IAnimalsRepository animalRepo)
{

    [HttpGet]
    public IResult GetAllAnimals(string orderBy = "")
    {
        Console.WriteLine("Inside GET");
        return Ok(animalRepo.FindAll(OrderOrDefault(orderBy)));
    }

    private string OrderOrDefault(string orderBy)
    {
        return orderBy is not ("Description" or "Category" or "Area") ? "Name" : orderBy;
    }

    [HttpPost]
    public IResult AddAnimal(Animal animal)
    {
        animalRepo.Add(animal);
        return Ok("Successfully added.");
    }

    [HttpPut]
    [Route("{id:int}")]
    public IResult UpdateAnimal(int id, Animal animal)
    {
        var updated = animalRepo.Update(id, animal);
        
        return updated ? Ok("Successfully updated.") : BadRequest("Animal with give id is not found.");
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public IResult DeleteAnimal(int id)
    {
        var deleted = animalRepo.Delete(id);
        
        return deleted ? Ok("Successfully deleted.") : BadRequest("Animal with give id is not found.");
    }
}