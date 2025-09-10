using Application.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Preceptor")]
public class EventsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EventsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var events = await _unitOfWork.EventRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EventDto>>(events));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        var eventItem = await _unitOfWork.EventRepository.GetByIdAsync(id);
        if (eventItem == null) return NotFound();
        return Ok(_mapper.Map<EventDto>(eventItem));
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(EventDto eventDto)
    {
        var eventItem = _mapper.Map<Event>(eventDto);
        await _unitOfWork.EventRepository.AddAsync(eventItem);
        await _unitOfWork.Complete();
        return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, _mapper.Map<EventDto>(eventItem));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEvent(int id, EventDto eventDto)
    {
        if (id != eventDto.Id) return BadRequest();

        var eventItem = await _unitOfWork.EventRepository.GetByIdAsync(id);
        if (eventItem == null) return NotFound();

        _mapper.Map(eventDto, eventItem);
        _unitOfWork.EventRepository.Update(eventItem);
        await _unitOfWork.Complete();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var eventItem = await _unitOfWork.EventRepository.GetByIdAsync(id);
        if (eventItem == null) return NotFound();

        _unitOfWork.EventRepository.Remove(eventItem);
        await _unitOfWork.Complete();
        return NoContent();
    }
}