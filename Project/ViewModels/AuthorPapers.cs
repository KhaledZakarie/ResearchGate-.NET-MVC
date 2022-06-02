using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.ViewModels
{
    public class AuthorPapers
    {
        public Participate participate { get; set; }
        public IEnumerable<Author> AuthorsList { get; set; }
    }
}