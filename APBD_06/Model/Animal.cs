namespace APBD_06.Model;

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