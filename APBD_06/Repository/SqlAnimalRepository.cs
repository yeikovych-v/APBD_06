using System.Data.SqlClient;
using APBD_06.Model;

namespace APBD_06.Repository;

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