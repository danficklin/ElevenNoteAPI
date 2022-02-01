using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models;
using ElevenNote.Models.Note;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize, Route("api/[controller]"), ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpGet, ProducesResponseType(typeof(IEnumerable<NoteListItem>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }
        [HttpPost, ProducesResponseType(typeof(string), 200), ProducesResponseType(typeof(string), 400), ProducesResponseType(typeof(ModelStateDictionary), 400) ]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(await _noteService.CreateNoteAsync(request))
                return Ok("Note created successfully.");
            return BadRequest("Note could not be created.");
        }
        [HttpGet("{noteId:int}"), ProducesResponseType(typeof(NoteDetail), 200)]
        public async Task<IActionResult> GetNoteById([FromRoute] int noteId)
        {
            var detail = await _noteService.GetNoteByIdAsync(noteId);
            return detail is not null ? Ok(detail) : NotFound();
        }
        [HttpPut, ProducesResponseType(typeof(ModelStateDictionary), 400), ProducesResponseType(typeof(NoteUpdate), 200)]
        public async Task<IActionResult> UpdateNoteById([FromBody] NoteUpdate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return await _noteService.UpdateNoteAsync(request) 
                ? Ok("Note updated successfully") 
                : BadRequest("Note could not be updated.");
        }
        [HttpDelete("{noteId:int}"), ProducesResponseType(typeof(string), 200), ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> DeleteNote([FromRoute] int noteId)
        {
            return await _noteService.DeleteNoteAsync(noteId) 
                ? Ok($"Note {noteId} was deleted successfully.") 
                : BadRequest($"Note {noteId} could not be deleted.");
        }
    }
}