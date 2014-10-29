using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xo.Areas.Blog.Domain
{
    public class Post
    {
        [Obsolete("Runtime instantiation only.", error: true)]
        public Post() { }

        public Post(DateTime created, Guid createdById)
        {
            this.Created = this.Updated = created;
            this.CreatedById = this.UpdatedById = createdById;
        }

        public int Id { get; private set; }

        [Required, StringLength(250)]
        public string Title { get; set; }

        [Required, StringLength(16000)]
        public string Body { get; set; }

        [Required]
        [Display(Name = "Publication Date")]
        public DateTime? PublicationDate { get; set; }

        [Required, DataType("AuthorId")]
        [Display(Name = "Author")]
        public Guid? AuthorId { get; set; }
        [ScaffoldColumn(false)]
        public virtual Users.Domain.User Author { get; private set; }

        [Required]
        public PostStatus? Status { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Comment> Comments { get; set; }

        [Required, ScaffoldColumn(false)]
        public DateTime Created { get; private set; }
        [Required, ScaffoldColumn(false)]
        public Guid CreatedById { get; private set; }

        [Required, ScaffoldColumn(false)]
        public DateTime Updated { get; private set; }
        [Required, ScaffoldColumn(false)]
        public Guid UpdatedById { get; private set; }


        public override string ToString()
        {
            return Title;
        }
    }
}