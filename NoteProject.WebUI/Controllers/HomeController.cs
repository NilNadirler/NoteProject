using NoteProject.BusinessLayer;
using NoteProject.BusinessLayer.Results;
using NoteProject.Entities;
using NoteProject.Entities.Messages;
using NoteProject.Entities.ValueObjects;
using NoteProject.WebUI.ViewModels;
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
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        

        // GET: Home
        public ActionResult Index()
        {
            //CategoryController üzerinden gelen view talebi ve model
            //if(TempData["nm"] != null)
            //{
            //    return View(TempData["nm"] as List<Note>);
            //}
            //List<Note> list =nm.GetAllNote().OrderByDescending(x=> x.ModifiedOn).ToList();
            List<Note> list = noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            return View(list);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category cat = categoryManager.Find(x=> x.Id==id.Value);

            if (cat == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index", "Home");
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {

                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Güncellenemedi.",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }

                Session["login"] = res.Result; //Profil güncellendiği için session güncellendi.
                return RedirectToAction("ShowProfile");
            }

            return View(model);

        }

        public ActionResult DeleteProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi.",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }

            Session.Clear();
            return RedirectToAction("Index");
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
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
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


                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);

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

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/login"
                };

                notifyObj.Items.Add("Lütfen e-mail adresinize gönderilen aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğeni yapamazsınız.");

                return View("Ok", notifyObj);
            }


            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            //Kullanıcı aktivasyonu sağlanacak..
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesabınız Aktifleştirilmiştir.",
                RedirectingUrl = "/Home/login"
            };

            okNotifyObj.Items.Add("Hesabınızı aktifleştirildi.Artık not ekleyebilir ve beğeni yapabilirsiniz.");

            return View("Ok", okNotifyObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        //public ActionResult TestNotify()
        //{
        //    OkViewModel model = new OkViewModel()
        //    {
        //        Header = "Yönlendirme..",
        //        Title = "Ok Test",
        //        RedirectingTimeout = 30000,
        //        Items = new List<string>() { "Test Başarılı", "Test Başarılı 2" }
        //    };

        //    return View("Ok", model);
        //}
    }
}