using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Xo.Areas.Blog.Domain
{
    public class Comment
    {
        [Obsolete("Runtime instantiation only.", error: true)]
        public Comment() { }

        public Comment(DateTime created, Guid createdById)
        {
            this.Posted = created;
            this.PostedById = createdById;
        }

        public int Id { get; private set; }

        [Required, StringLength(2000), DataType(DataType.MultilineText)]
        public string Body { get; private set; }

        public DateTime? Posted { get; private set; }

        public Guid PostedById { get; private set; }
        public Identity.Domain.Identity PostedBy { get; private set; }
    }
}
