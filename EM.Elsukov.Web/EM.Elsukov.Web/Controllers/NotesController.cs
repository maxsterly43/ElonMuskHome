using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate;
using EM.Elsukov.DB.NHibernate.Interfaces;
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
        public ActionResult AllNotes(string search)
        {
            string s = search;
            IEnumerable<Note> notes = notesRep.GetAll();
            return View(notes);
        }
        [HttpGet]
        public ActionResult MyNotes()
        {
            IEnumerable<Note> notes = notesRep.LoadByUserLogin(User.Identity.Name);
            return View("AllNotes", notes);
        }
    }
}