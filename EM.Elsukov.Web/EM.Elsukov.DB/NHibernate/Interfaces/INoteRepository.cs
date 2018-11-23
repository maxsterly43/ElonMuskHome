using EM.Elsukov.DB.Models;
using System.Collections.Generic;

namespace EM.Elsukov.DB.NHibernate.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> LoadBySorted(string filed);

        IEnumerable<Note> LoadByUser(string login, string filedSort);

        IEnumerable<Note> LoadLike(string search, string sort);

        IEnumerable<Note> LoadLikeByUser(string login, string search, string sort);

        Note LoadById(long id);

        void SaveByProc(Note note);
    }
}