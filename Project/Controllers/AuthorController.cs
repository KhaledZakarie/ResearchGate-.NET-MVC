using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class AuthorController : Controller
    {

        public ActionResult Home()
        {
            return View();
        }

        // GET: Author/Registration
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Author/Registration
        [HttpPost]
        public ActionResult Registration(Author author)
        {
            try
            {
                // TODO: Add insert logic here
                string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                string extension = Path.GetExtension(author.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                author.ImagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                author.ImageFile.SaveAs(fileName);

                using (DBModels db = new DBModels())
                {
                    if (db.Authors.Any(x => x.Email == author.Email))
                    {
                        ViewBag.DuplicateMessage = "Email Already Exist";
                        return View("Registration", author);
                    }
                    else
                    {
                        db.Authors.Add(author);
                        db.SaveChanges();
                    }
                }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Registration Successful";
                return View("Registration", new Author());
            }
            catch
            {
                return View();
            }
        }


        // GET: Author/LogIn
        public ActionResult LogIn()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("MainPage", "Author", new { id = Session["username"].ToString() });//id -> Email
            }
            else
            {
                return View();
            }

        }

        // POST: Author/Login
        [HttpPost]
        public ActionResult LogIn(Author author)
        {
            using (DBModels db = new DBModels())
            {
                
                if (author.Email == "Admin" && author.Passward == "Admin")
                {
             


                    return RedirectToAction("Admin", "Author");//id -> Email
                }
                else
                {
                    var userLoggedIn = db.Authors.SingleOrDefault(x => x.Email == author.Email && x.Passward == author.Passward);
                    if (userLoggedIn != null)
                    {
                        //ViewBag.message = "LoggedIn";
                        //ViewBag.triedOnce = "yes";

                        Session["username"] = author.Email;


                        return RedirectToAction("MainPage", "Author", new { id = author.Email });//id -> Email
                                                                                                 //author.LoginErrorMsg = "Invalid Email or Password";
                                                                                                 //return View("LogIn", author);
                    }
                    else
                    {
                        author.LoginErrorMsg = "Invalid Email or Password";
                        //ViewBag.triedOnce = "yes";
                        return View("LogIn", author);
                        //Session["AuthorId"] = author.Id;
                        //Session["AuthorFName"] = author.FName;
                        //return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        public ActionResult MainPage(string id) //id -> Email
        {


            if (Session["username"] == null)
            {
                return RedirectToAction("LogIn", "Author");
            }
            else
            {
                ViewBag.username = Session["username"];
                using (DBModels db = new DBModels())
                {
                    return View(db.Authors.Where(x => x.Email == id).FirstOrDefault());
                }


            }

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            //int AuthorId = (int)Session["AuthorId"];
            return RedirectToAction("LogIn", "Author");
        }


        // GET: Author/Edit
        public ActionResult Edit(int id)
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Authors.Where(c => c.Id == id).FirstOrDefault());
            }

        }

        // POST: Author/Edit
        [HttpPost]
        public ActionResult Edit(int id, Author author)
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    db.Entry(author).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("MainPage", "Author", new { id = author.Email });
            }
            catch
            {

                return RedirectToAction("Edit", "Author", author);
            }
        }


        // GET: Author/Edit
        public ActionResult EditImage(int id)
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Authors.Where(c => c.Id == id).FirstOrDefault());
            }

        }

        // POST: Author/Edit
        [HttpPost]
        public ActionResult EditImage(int id, Author author)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                string extension = Path.GetExtension(author.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                author.ImagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                author.ImageFile.SaveAs(fileName);
                using (DBModels db = new DBModels())
                {
                    db.Entry(author).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("MainPage", "Author", new { id = author.Email });
            }
            catch
            {
                return View("EditImage", new Author());
            }
        }



        public ActionResult IndexAuthor()
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Authors.ToList());
            }
        }

        public ActionResult IndexPaper()
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.ToList());
            }
        }

        public ActionResult ViewPage(int id) //id -> author id
        {
                using (DBModels db = new DBModels())
                {
                    return View(db.Authors.Where(x => x.Id == id).FirstOrDefault());
                }

        }

        public ActionResult Admin()
        {
            return View();
        }

        // GET: Author/DeleteAuthorAdmin
        public ActionResult DeleteAuthorAdmin(int id) // id of author to delete
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Authors.Where(c => c.Id == id).FirstOrDefault());
            }
        }

        // POST: author/DeleteAuthorAdmin
        [HttpPost]
        public ActionResult DeleteAuthorAdmin(int id, Author author)// id of author to delete
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    var comment = db.CommentDBs.Where(c => c.AuthorId == id).FirstOrDefault();

                    while (comment != null)
                    {
                        db.CommentDBs.Remove(comment);
                        db.SaveChanges();
                        comment = db.CommentDBs.Where(c => c.AuthorId == id).FirstOrDefault();
                    }

                    var like = db.LikeDBs.Where(c => c.AuthorId == id).FirstOrDefault();
                    while (like != null)
                    {
                        db.LikeDBs.Remove(like);
                        db.SaveChanges();
                        like = db.LikeDBs.Where(c => c.AuthorId == id).FirstOrDefault();
                    }
                    var part = db.Participates.Where(c => c.AuthorId == id).FirstOrDefault();

                    while (part != null)
                    {
                        db.Participates.Remove(part);
                        db.SaveChanges();
                        part = db.Participates.Where(c => c.AuthorId == id).FirstOrDefault();
                    }
                    

                    

                    var paper = db.Papers.Where(c => c.PublisherId == id).FirstOrDefault();
                    while (paper != null)
                    {
                        var tt = paper.PaperId;
                        var commentt = db.CommentDBs.Where(c => c.PaperId == tt).FirstOrDefault();
                        while (commentt != null)
                        {
                            db.CommentDBs.Remove(commentt);
                            db.SaveChanges();
                            commentt = db.CommentDBs.Where(c => c.PaperId == tt).FirstOrDefault();
                        }

                        var likee = db.LikeDBs.Where(c => c.PaperId == tt).FirstOrDefault();
                        while (likee != null)
                        {
                            db.LikeDBs.Remove(likee);
                            db.SaveChanges();
                            likee = db.LikeDBs.Where(c => c.PaperId == tt).FirstOrDefault();
                        }
                        db.Papers.Remove(paper);
                        db.SaveChanges();
                        paper = db.Papers.Where(c => c.PublisherId == id).FirstOrDefault();
                    }
                    author = db.Authors.Where(c => c.Id == id).FirstOrDefault();
                    var pubId = author.Id;
                    db.Authors.Remove(author);
                    db.SaveChanges();
                    return RedirectToAction("IndexAuthor");
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Author/DeletePaperAdmin
        public ActionResult DeletePaperAdmin(int id) // id of paper to delete
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Papers.Where(c => c.PaperId == id).FirstOrDefault());
            }
        }

        // POST: author/DeletePaperAdmin
        [HttpPost]
        public ActionResult DeletePaperAdmin(int id, Paper paper)// id of paper to delete
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
                    return RedirectToAction("IndexPaper");
                }
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SearchAuthor(String search) //search -> email or name or university
        {
            using (DBModels db = new DBModels())
            {
                return View(db.Authors.Where(s=> s.FName.Contains(search) || s.LName.Contains(search) 
                    || s.Email.Contains(search) || s.University.Contains(search)).ToList());
            }

        }
    }
}
