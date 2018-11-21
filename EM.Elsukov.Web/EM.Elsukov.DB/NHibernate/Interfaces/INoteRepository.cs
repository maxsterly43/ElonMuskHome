using EM.Elsukov.DB.Models;
using System.Collections.Generic;

namespace EM.Elsukov.DB.NHibernate.Interfaces
{
    public interface INoteRepository
    {
        Note LoadByTitle(string name);

        IEnumerable<Note> LoadBySorted(string filed);

        IEnumerable<Note> LoadByUserLogin(string login, string filedSort);
    }
}