using EM.Elsukov.DB.Models;
using System.Collections.Generic;

namespace EM.Elsukov.DB.NHibernate.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> LoadBySorted(string filed);

        IEnumerable<Note> LoadByUserLogin(string login, string filedSort);

        IEnumerable<Note> LoadLike(string search, string sort);

        void SaveByProc(Note note);

        Note LoadByTitle(string title);
    }
}