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
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            //Kullanıcı username kontrolü
            //Kullanıcı email kontrolü
            //Kayıt İşlemi
            //Aktivasyon email gönderimi

            EvernoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email);
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
                int dbResult = repo_user.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),                   
                    IsActive=false,
                    IsAdmin=false
                });

                if (dbResult > 0)
                {
                    res.Result = repo_user.Find(x => x.Username == data.Username && x.Email == data.Email);

                    //TODO: aktivasyon maili atılacak
                    //layerResult.Result.ActivateGuid
                }
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş Kontrolü 
            //Hesap aktive edilmiş mi?

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);

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
    }
}
