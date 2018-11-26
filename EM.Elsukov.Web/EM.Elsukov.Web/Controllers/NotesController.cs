using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate;
using EM.Elsukov.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace EM.Elsukov.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        NHNoteRepository notesRep;
        NHUserRepository usersRep;
        public NotesController()
        {
            notesRep = new NHNoteRepository();
            usersRep = new NHUserRepository();
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
            if (!ModelState.IsValid)
                return PartialView(searchModel);
            if (searchModel.sortBy == null)
                searchModel.sortBy = "CreateDate";
            var notes = notesRep.LoadLike(searchModel.search, searchModel.sortBy);
            return PartialView("AllNoteListPartial", notes);
        }

        [HttpGet]
        public ActionResult GetMyNotes()
        {
            IEnumerable<Note> notes = notesRep.LoadByUser(User.Identity.Name, "CreateDate");
            if (notes == null || notes.Count() == 0)
                return HttpNotFound();
            return PartialView("MyNoteListPartial", notes);
        }

        [HttpPost]
        public PartialViewResult GetMyNotes(SearchModel searchModel)
        {
            if (!ModelState.IsValid)
                return PartialView(searchModel);
            if (searchModel.sortBy == null)
                searchModel.sortBy = "CreateDate";
            var notes = notesRep.LoadLikeByUser(User.Identity.Name, searchModel.search, searchModel.sortBy);
            return PartialView("MyNoteListPartial", notes);
        }

        [HttpGet]
        public ActionResult EditNotePartial(long id)
        {
            var note = notesRep.LoadById(id);
            if (note == null || note.User.Login != User.Identity.Name)
                return HttpNotFound();
            var editNoteModel = new EditNoteModel()
            {
                Id = note.Id,
                Title = note.Title,
                Text = note.Text,
                Status = note.Status
            };
            return PartialView(editNoteModel);
        }

        [HttpPost]
        public ActionResult EditNotePartial(EditNoteModel editNote)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return Json(new { errors = errors });
            }
            var note = notesRep.LoadById(editNote.Id);
            if (note == null || note.User.Login != User.Identity.Name)
                return Json(new { errors = new List<string>() { { "Заметка не найдена" } } });

            Response.StatusCode = (int)HttpStatusCode.OK;

            note.Text = editNote.Text;
            note.Status = editNote.Status;
            notesRep.Save(note);

            return PartialView(editNote);
        }

        [HttpGet]
        public bool deleteNote(long id)
        {
            var note = notesRep.LoadById(id);
            if (note == null || note.User.Login != User.Identity.Name)
                return false;
            notesRep.Delete(note);
            return true;
        }
        [HttpGet]
        public bool restoreNote(long id)
        {
            var note = notesRep.LoadById(id);
            if (note == null || note.User.Login != User.Identity.Name)
                return false;
            notesRep.restoreNote(note);
            return true;
        }

        [HttpGet]
        public PartialViewResult CreateNotePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNotePartial(CreateNoteModel createNoteModel)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            if (!ModelState.IsValid)
            {
                return PartialView(createNoteModel);
            }

            User user = usersRep.LoadByLogin(User.Identity.Name);

            if (user == null)
            {
                return PartialView(createNoteModel);
            }

            byte[] bytess = new byte[createNoteModel.File.InputStream.Length];
            createNoteModel.File.InputStream.Read(bytess, 0, bytess.Length);

            var postedFile = new MemoryPostedFile(bytess, createNoteModel.File.FileName,createNoteModel.File.ContentType);

            var uploadedFile = ObjectToByteArray(postedFile);

            Note note = new Note()
            {
                Title = createNoteModel.Title,
                Text = createNoteModel.Text,
                Tags = createNoteModel.Tags,
                Status = (NoteStatus)createNoteModel.Status,
                User = user,
                BinaryFile = uploadedFile
            };

            if (notesRep.SaveByProc(note))
                Response.StatusCode = (int)HttpStatusCode.OK;

            return PartialView();
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            var note = notesRep.LoadById(id);
            MemoryPostedFile objFile = (MemoryPostedFile)ByteArrayToObject(note.BinaryFile);

            return new FileContentResult(objFile.fileBytes, objFile.ContentType);
        }

        private byte[] ObjectToByteArray(HttpPostedFileBase File)
        {
            MemoryStream target = new MemoryStream();
            File.InputStream.CopyTo(target);
            return target.ToArray();
        }
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        public byte[] fileBytes { get; }

        public MemoryPostedFile(byte[] fileBytes, string fileName = null, string ContentType = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.ContentType = ContentType;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override string ContentType { get; }

        public override Stream InputStream { get; }
    }
}