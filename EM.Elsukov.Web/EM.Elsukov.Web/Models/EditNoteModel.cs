using EM.Elsukov.DB.Models;
using System.ComponentModel.DataAnnotations;

namespace EM.Elsukov.Web.Models
{
    public class EditNoteModel
    {
        public EditNoteModel() { }

        [ScaffoldColumn(false)]
        public long Id { get; set; }
        [Display(Name = "Редактирование заметки:")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Заметка должна содержать текст")]
        [Display(Name = "Содержание заметки:")]
        public string Text { get; set; }

        [Required]
        public NoteStatus Status { get; set; }
    }
}