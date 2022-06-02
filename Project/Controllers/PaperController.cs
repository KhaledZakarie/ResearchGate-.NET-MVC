using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class PaperController : Controller
    {
        // GET: Paper
        /*public ActionResult Index()
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.ToList());
            }
                
        }*/
        /*public ActionResult IndexFK()
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Participates.ToList());
            }

        }*/



        // GET: Paper/PaperLoad
        public ActionResult PaperLoad(int id) //id -> the author(Publisher)
        {
            ViewBag.PublisherId = id;
            return View();
        }

        // POST: Paper/PaperLoad
        [HttpPost]
        public ActionResult PaperLoad(Paper paper)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(paper.FilePost.FileName);
                string extension = Path.GetExtension(paper.FilePost.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                paper.PaperFile = "~/File/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/File/"), fileName);
                paper.FilePost.SaveAs(fileName);
                using (DBModels db = new DBModels())
                {
                    ViewBag.PaperLoaded = paper.PaperId;
                    
                    db.Papers.Add(paper);
                    db.SaveChanges();
                    //Tuple<AuthorPapers, int> t = new Tuple<AuthorPapers, int>(db.Authors.ToList(), ViewBag.PaperLoaded);
                    //return View("Tag", new AuthorPapers { AuthorsList = db.Authors.ToList()});
                }
                //ModelState.Clear();
                return RedirectToAction("Tag", "Paper", new { id = paper.PaperId });
                //ViewBag.SuccessMessage = paper.PaperId;
                //return View("PaperLoad", new Paper());
            }
            catch
            {
                return View();
            }
        }

        // GET: Paper/Tag
        public ActionResult Tag(int id) //id -> Paper Id
        {
            using (DBModels db = new DBModels())
            {
                ViewBag.PaperId = id;
               
                var Publisher = db.Authors.ToList();
                AuthorPapers authorPapers = new AuthorPapers
                {
                    AuthorsList = Publisher
                };
                
                return View(authorPapers);
            }
            
        }

        // POST: Paper/Tag
        [HttpPost]
        public ActionResult Tag(Participate participate)
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    db.Participates.Add(participate);
                    db.SaveChanges();

                    //ModelState.Clear();
                    //ViewBag.SuccessTag = "The Participate has been added, Can add another Participate by adding the Author's name";
                    //return View("Tag", new AuthorPapers { AuthorsList = db.Authors.ToList() });
                    
                }
                return RedirectToAction("Tag", "Paper", new { id = participate.PaperId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Paper/YourPaper Paper for each author
        public ActionResult YourPaper(int id) //id -> the author(Publisher)
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.Where(p=>p.PublisherId == id).ToList());
            }
        }

        // GET: Paper/YourPaper Paper for each author
        public ActionResult YourPaperDelete(int id) //id -> the author(Publisher)
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.Where(p => p.PublisherId == id).ToList());
            }
        }



        public ActionResult Download(string Name)
        {
            
            var fileName = Name.Remove(0, 7);  //to remove "~/File/" from path --> file name
            string path = Server.MapPath("~/File/") + fileName; // full path
            
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        // GET: Paper/DeletePaper
        public ActionResult DeletePaper(int id) // id of paper to delete
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.Where(c => c.PaperId == id).FirstOrDefault());
            }
        }

        // POST: Paper/DeletePaper
        [HttpPost]
        public ActionResult DeletePaper(int id, Paper paper)// id of paper to delete
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    var comment = db.CommentDBs.Where(c => c.PaperId == id).FirstOrDefault();
                   
                    while (comment != null)
                    {
                        db.CommentDBs.Remove(comment);
                        db.SaveChanges();
                        comment = db.CommentDBs.Where(c => c.PaperId == id).FirstOrDefault();
                    }
                    
                    var like = db.LikeDBs.Where(c => c.PaperId == id).FirstOrDefault();
                    while (like != null)
                    {
                        db.LikeDBs.Remove(like);
                        db.SaveChanges();
                        like = db.LikeDBs.Where(c => c.PaperId == id).FirstOrDefault();
                    }
                    
                    paper = db.Papers.Where(c => c.PaperId == id).FirstOrDefault();
                    var pubId = paper.PublisherId;
                    db.Papers.Remove(paper);
                    db.SaveChanges();
                    return RedirectToAction("YourPaper", new { id = pubId });
                }
                
            }
            catch
            {
                return View("LogIn", "Author");
            }
        }

        // GET: Paper/Mentions Paper mentioned to each author
        public ActionResult Mentions(int id) //author id
        {
            using (DBModels db = new DBModels())
            {
                var person = db.Participates.Include("Paper").Where(p => p.AuthorId == id).ToList();
                return View(person);
            }
        }

        // GET: Paper/DeleteMentions of you
        public ActionResult DeleteMentions(int id) //author id
        {
            using (DBModels db = new DBModels())
            {
                var person = db.Participates.Include("Paper").Where(p => p.AuthorId == id).ToList();
                return View(person);
            }
        }

        // GET: Paper/Details Paper for each Paper
        public ActionResult Details(int author, int paper) //paper id
        {
           
                
            
            using (DBModels db = new DBModels())
            {
                var Em = Session["username"];
                var xrd = db.Authors.Where(c => c.Email == Em).SingleOrDefault();
                ViewBag.autthor = xrd.Id;
                ViewBag.papper = paper;

                LikeDB like = db.LikeDBs.SingleOrDefault(x=>x.PaperId==paper && x.AuthorId== xrd.Id);
                CommentDB comment = new CommentDB();
                var paperr = db.Papers.Include("Author").Where(x => x.PaperId == paper).FirstOrDefault();
                
                LIkeAndComment lIkeAndComment = new LIkeAndComment
                {
                    like = like,
                    comment = comment,
                    paper = paperr
                };
                return View(lIkeAndComment);
            }
            
        }
        [HttpPost]
        public ActionResult Details(int author, int paper, LIkeAndComment lIkeAndComment)//id-> PId
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    var Em = Session["username"];
                    var xrd = db.Authors.Where(c => c.Email == Em).SingleOrDefault();

                    var Cond = db.LikeDBs.SingleOrDefault(x => x.PaperId == paper && x.AuthorId == xrd.Id);
                    if (Cond == null)
                    {
                        db.LikeDBs.Add(lIkeAndComment.like);
                        db.SaveChanges();
                        //return RedirectToAction("Details", "Paper", new { author = author, paper = paper });
                    }
                    else
                    {
                        var remov = db.LikeDBs.SingleOrDefault(x => x.PaperId == paper && x.AuthorId == xrd.Id);

                        db.LikeDBs.Remove(remov);
                        db.SaveChanges();
                        db.LikeDBs.Add(lIkeAndComment.like);
                        db.SaveChanges();
                        //db.Entry(likeDB).State = EntityState.Modified;
                        //db.SaveChanges();
                        //return RedirectToAction("Details", "Paper", new { id = paper });

                    }
                    //db.Papers.Add(paper);
                    //db.SaveChanges();
                }
                if (lIkeAndComment.comment.Comment != null)
                {
                    using (DBModels db = new DBModels())
                    {
                        db.CommentDBs.Add(lIkeAndComment.comment);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Paper", new { author = author, paper = paper });

                    }
                }
                else
                {
                    return RedirectToAction("Details", "Paper", new { author = author, paper = paper });
                }
                

            }
            catch
            {
                return View();
            }
        }


        // GET: Paper/YourPaper Authors Mentioned in paper
        public ActionResult PaperTag(int id) //id -> the Paper id
        {
            
            using (DBModels db = new DBModels())
            {
                var Tag = db.Participates.Include("Author").Where(p=>p.PaperId==id).ToList();
                return View(Tag);
            }
        }

        // GET: Paper/DeleteTag
        public ActionResult DeleteTag(int id) //id-> PId
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Participates.Where(c => c.PId == id).FirstOrDefault());
            }
        }

        // POST: Paper/DeleteTag
        [HttpPost]
        public ActionResult DeleteTag(int id, Participate participate)//id-> PId
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    participate = db.Participates.Where(c => c.PId == id).FirstOrDefault();
                    var pubId = participate.AuthorId;
                    db.Participates.Remove(participate);
                    db.SaveChanges();
                    return RedirectToAction("Mentions", "Paper", new { id = pubId });
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Paper/LikeComment
        public ActionResult LikeComment(int author, int paper) //PaperAction = tuple (PaperId , AuthorId)
        {
                
            ViewBag.author = author;
            ViewBag.paper = paper;
            return View();
        }

        // POST: Paper/LikeComment
        [HttpPost]
        public ActionResult LikeComment(int author, int paper, LikeDB likeDB)//id-> PId
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    var Cond = db.LikeDBs.SingleOrDefault(x => x.PaperId == paper && x.AuthorId ==author);
                    if (Cond == null)
                    {
                        db.LikeDBs.Add(likeDB);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Paper", new { id = paper });
                    }
                    else
                    {
                        var remov = db.LikeDBs.SingleOrDefault(x => x.PaperId == paper && x.AuthorId == author);

                        db.LikeDBs.Remove(remov);
                        db.SaveChanges();
                        db.LikeDBs.Add(likeDB);
                        db.SaveChanges();
                        //db.Entry(likeDB).State = EntityState.Modified;
                        //db.SaveChanges();
                        return RedirectToAction("Details", "Paper", new { id = paper });
                    }
                    //db.Papers.Add(paper);
                    //db.SaveChanges();
                    
                }
                //return View();
            }
            catch
            {
                return RedirectToAction("LogIn", "Author");
            }
        }

        public ActionResult ViewComment(int id) //id -> the Paper id
        {

            using (DBModels db = new DBModels())
            {
                return View(db.CommentDBs.Include("Author").Where(c=>c.PaperId == id).ToList());
            }
        }

        public ActionResult ViewLikes(int id) //id -> the Paper id
        {

            using (DBModels db = new DBModels())
            {
                return View(db.LikeDBs.Include("Author").Where(c => c.PaperId == id).ToList());
            }
        }

        // GET: Paper/AddComment
        public ActionResult AddComment(int author, int paper) //PaperAction = tuple (PaperId , AuthorId)
        {
            ViewBag.authorr = author;
            ViewBag.paperr = paper;
            return View();
        }

        // POST: Paper/AddComment
        [HttpPost]
        public ActionResult AddComment(int author, int paper, CommentDB commentDB)//id-> PId
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                        db.CommentDBs.Add(commentDB);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Paper", new { id = paper });
                    
                }
                //return View();
            }
            catch
            {
                return RedirectToAction("LogIn", "Author");
            }
        }


    }
}
