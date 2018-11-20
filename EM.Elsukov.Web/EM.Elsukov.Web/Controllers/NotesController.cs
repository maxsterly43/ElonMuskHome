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
        NHNoteRepository notes;
        NHUserRepository users;
        public NotesController()
        {
            notes = new NHNoteRepository();
            users = new NHUserRepository();
        }
        [HttpGet]
        public ActionResult AllNotes()
        {
            return View(notes.GetAll());
        }
    }
}