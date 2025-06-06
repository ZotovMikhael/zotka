using Microsoft.AspNetCore.Mvc;
using KursovayaServer.Models;
using KursovayaServer.Services;

namespace KursovayaServer.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly DataService _dataService;

    public NotesController(DataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("{userId}")]
    public ActionResult<IEnumerable<Note>> GetUserNotes(string userId)
    {
        return Ok(_dataService.GetUserNotes(userId));
    }

    [HttpPost]
    public ActionResult<Note> AddNote(Note note)
    {
        var addedNote = _dataService.AddNote(note);
        return Ok(addedNote);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateNote(int id, Note note)
    {
        if (note.Id != id)
        {
            return BadRequest("Id в URL не совпадает с Id в теле запроса");
        }
        
        if (_dataService.UpdateNote(note))
            return Ok();
        return NotFound();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteNote(int id)
    {
        if (_dataService.DeleteNote(id))
            return Ok();
        return NotFound();
    }
} 