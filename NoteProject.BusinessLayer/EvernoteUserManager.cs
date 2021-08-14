using NoteProject.BusinessLayer.Abstract;
using NoteProject.BusinessLayer.Results;
using NoteProject.Common.Helpers;
using NoteProject.DataAccesslayer.EF;
using NoteProject.Entities;
using NoteProject.Entities.Messages;
using NoteProject.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.BusinessLayer
{
    public class EvernoteUserManager:ManagerBase<EvernoteUser>
    {
      
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            //Kullanıcı username kontrolü
            //Kullanıcı email kontrolü
            //Kayıt İşlemi
            //Aktivasyon email gönderimi

            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if (user != null)
            {
                if(user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı.");
                }

            }
            else
            {
                int dbResult = Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),                   
                    IsActive=false,
                    IsAdmin=false,
                    ProfileImageFilename="user_boy.png"
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Username == data.Username && x.Email == data.Email);

                    //TODO: aktivasyon maili atılacak
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}Home/UserActivate/{res.Result.ActivateGuid}";
                    //https:/localhost:44335/Home/userActivate/6032ceb3-9d08-49b5-a4ed-ddcc8bbe1b91
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'> tıklayınız. <a>.";

                    MailHelper.SendMail(body, res.Result.Email, "NoteProject Hesap Aktifleştirme");



                }
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Id == id);

            if(res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamdı.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş Kontrolü 
            //Hesap aktive edilmiş mi?

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ve şifre uyuşmuyor.");
            }

            return res;

        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if(db_user != null && db_user.Id == data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Username = data.Username;
            res.Result.Password = data.Password;

            if(string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if(Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }
            return res;

        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if(res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyActivate, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }
    }
}
