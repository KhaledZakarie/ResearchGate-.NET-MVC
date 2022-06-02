using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.ViewModels
{
    public class LIkeAndComment
    {
        public Author author { get; set; }
        public Paper paper { get; set; }
        public LikeDB like { get; set; }
        public CommentDB comment { get; set; }
    }
}