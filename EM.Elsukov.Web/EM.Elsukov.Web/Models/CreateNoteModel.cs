using EM.Elsukov.DB.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EM.Elsukov.Web.Models
{
    public class CreateNoteModel
    {
        public CreateNoteModel() { }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Содержание")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Теги")]
        public string Tags { get; set; }

        [Display(Name = "Прикрепить файл")]
        public HttpPostedFileBase File { get; set; }

        [Required(ErrorMessage ="Необходимо выбрать один из вариантов")]
        public NoteStatus? Status { get; set; }
    }
}