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
        public ActionResult AllNotes()
        {
            var m = new SearchModel();
            return View(m);
        }
        [HttpPost]
        public ActionResult AllNotes(SearchModel searchModel)
        {
            IEnumerable<Note> notes = notesRep.LoadLike(searchModel.search, searchModel.sortBy);
            return PartialView("NoteList", notes);
        }
        [HttpGet]
        public ActionResult MyNotes()
        {
            IEnumerable<Note> notes = notesRep.LoadByUserLogin(User.Identity.Name);
            return View("AllNotes", notes);
        }
    }
}