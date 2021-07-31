using NoteProject.BusinessLayer;
using NoteProject.Entities;
using NoteProject.Entities.Messages;
using NoteProject.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //CategoryController üzerinden gelen view talebi ve model
            //if(TempData["nm"] != null)
            //{
            //    return View(TempData["nm"] as List<Note>);
            //}
            NoteManager nm = new NoteManager();
            //List<Note> list =nm.GetAllNote().OrderByDescending(x=> x.ModifiedOn).ToList();
            List<Note> list = nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            return View(list);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index", "Home");
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if(res.Errors.Find(x=> x.Code== ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-89098";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["login"] = res.Result;    //Session'a kullanıcı bilgi saklama...
                return RedirectToAction("Index"); //Yönlendirme..
            }
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) //Model geçerli mi? Modele ait bütün property'ler için Attribute ve veri tipi kontorolü
            {
                #region username and email kontrol
                //bool HasError = false;
                //if (model.Username == "aaa")
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor");
                //    HasError = true;
                //}

                //if (model.Email == "aaa@gmail.com")
                //{
                //    ModelState.AddModelError("", "Email adresi kullanılıyor");
                //    HasError = true;
                //}

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count>0)
                //        return View(model);
                //}
                #endregion

                
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                //try
                //{
                //    user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{

                //    ModelState.AddModelError("", ex.Message); //Gelen mesajı olduğu gibi ModelError'a vericem.
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}

                return RedirectToAction("RegisterOk");
            }

           
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            //Kullanıcı aktivasyonu sağlanacak..
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}