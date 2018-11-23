using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate;
using EM.Elsukov.DB.NHibernate.Interfaces;
using EM.Elsukov.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EM.Elsukov.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        NHNoteRepository notesRep;
        public NotesController()
        {
            notesRep = new NHNoteRepository();
        }

        [HttpGet]
        public ActionResult MyNotes()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllNotes()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult SearchPartial()
        {
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult GetAllNotes()
        {
            IEnumerable<Note> notes = notesRep.LoadBySorted("CreateDate");
            if (notes == null || notes.Count() == 0)
                return PartialView();
            return PartialView("AllNoteListPartial", notes);
        }

        [HttpPost]
        public PartialViewResult GetAllNotes(SearchModel searchModel)
        {
            if (!ModelState.IsValid || (searchModel.search == null & searchModel.sortBy == null))
                return PartialView(searchModel);
            var notes = notesRep.LoadLike(searchModel.search, searchModel.sortBy);
            return PartialView("AllNoteListPartial", notes);
        }

        [HttpGet]
        public PartialViewResult GetMyNotes()
        {
            IEnumerable<Note> notes = notesRep.LoadByUser(User.Identity.Name, "CreateDate");
            if (notes == null || notes.Count() == 0)
                return PartialView();
            return PartialView("MyNoteListPartial", notes);
        }
        [HttpPost]
        public ActionResult GetMyNotes(SearchModel searchModel)
        {
            if (!ModelState.IsValid || (searchModel.search == null & searchModel.sortBy == null))
                return PartialView(searchModel);
            var notes = notesRep.LoadLikeByUser(User.Identity.Name, searchModel.search, searchModel.sortBy);
            return PartialView("MyNoteListPartial", notes);
        }
        public ActionResult EditNotePatrial(int id)
        {
            var note = notesRep.LoadById(id);
            if (note != null)
                return PartialView(note);
            return HttpNotFound();
        }
    }
}