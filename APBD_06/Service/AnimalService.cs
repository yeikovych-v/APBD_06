using APBD_06.Model;

namespace APBD_06.Service;

public class AnimalService
{
    public List<Animal> SortByName(IEnumerable<Animal> toSort)
    {
        return toSort.OrderBy(animal => animal.Name).ToList();
    }
    
    public List<Animal> SortByDesc(IEnumerable<Animal> toSort)
    {
        return toSort.OrderBy(animal => animal.Description).ToList();
    }
    
    public List<Animal> SortByCategory(IEnumerable<Animal> toSort)
    {
        return toSort.OrderBy(animal => animal.Category).ToList();
    }
}