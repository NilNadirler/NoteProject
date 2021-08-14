
namespace NoteProject.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153,
        UsernameAlreadyActivate=154,
        ActivateIdDoesNotExists =155,
        UserNotFound=156,
        ProfileCouldNotUpdated = 157,
        UserCouldNotRemove = 158,
        UserCouldNotFind = 159
    }
}
