using APBD_06.Model;

namespace APBD_06.Repository;

public interface IAnimalsRepository
{
    List<Animal> FindAll(string orderBy);
    void Add(Animal animal);
    bool Update(int id, Animal animal);
    bool Delete(int id);

}