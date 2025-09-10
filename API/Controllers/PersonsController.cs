using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonsController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonsController(PersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetPerson(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonDto createPersonDto)
    {
        var person = await _personService.CreatePersonAsync(createPersonDto);
        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePerson(int id, PersonDto personDto)
    {
        if (id != personDto.Id) return BadRequest();
        var result = await _personService.UpdatePersonAsync(id, personDto);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerson(int id)
    {
        var result = await _personService.DeletePersonAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}