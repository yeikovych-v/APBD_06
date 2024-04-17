using APBD_06.Model;
using APBD_06.Repository;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace APBD_06.Controller;

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