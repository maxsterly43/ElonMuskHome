using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Elsukov.DB.Models
{
    public class File : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string Path { get; set; }

        public virtual string ContentType { get; set; }

        public virtual string Name { get; set; }

    }
}