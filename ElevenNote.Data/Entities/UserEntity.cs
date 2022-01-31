using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNote.Data.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public List<NoteEntity> Notes { get; set; }
    }
}