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
    public class NotesController : Controller
    {
        NHNoteRepository notesRep;
        IEnumerable<Note> notes;
        public NotesController()
        {
            notesRep = new NHNoteRepository();
        }
        [HttpGet]
        public ActionResult AllNotes()
        {
            notes = notesRep.GetAll();
            return View(notes);
        }
    }
}