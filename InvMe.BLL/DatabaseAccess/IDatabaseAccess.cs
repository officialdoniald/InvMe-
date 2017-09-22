using InvMe.DAL.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InvMe.BLL.DatabaseAccess
{
    public interface IDatabaseAccess
    {
        #region InsertFunctions

        Task<bool> InsertUserAsync(User user);
        Task<int> InsertEventAsync(Events events);
        Task<string> InsertHashtag(HashtagsM hastag);
        Task<string> InserFriend(Friends friend);
        Task<bool> InsertAttendedAsync(Attended attend);

        #endregion

        #region DeleteFunctions

        Task<bool> DeleteUser(User user);
        Task<bool> DeleteEvent(Events events);
        Task<bool> DeleteHashtag(HashtagsM hastag);
        Task<bool> DeleteFriend(Friends friend);
        Task<bool> DeleteAttended(Attended attend);

        #endregion

        #region UpdateFunctions

        Task<bool> UpdateUser(int ID, User user);
        Task<bool> UpdateEvent(int ID, Events events);
        Task<bool> UpdateHashtag(int ID, HashtagsM hastag);
        Task<bool> UpdateFriend(int ID, Friends friend);

        #endregion

        #region GetFunctions

        Task<ObservableCollection<User>> GetUser();
        Task<ObservableCollection<Events>> GetEvent();
        Task<ObservableCollection<HashtagsM>> GetHashtag();
        Task<ObservableCollection<Friends>> GetFriend();
        Task<ObservableCollection<Attended>> GetAttended();

        #endregion

        #region GetFunctionsById

        Task<User> GetUserByID(int ID);
        Task<User> GetUserByEMAIL(string EMAIL);
        Task<Events> GetEventByID(int ID);
        Task<HashtagsM> GetHashtagByID(int ID);
        Task<Friends> GetFriendByID(int ID);
        Task<ObservableCollection<Attended>> GetAttendedByID(int ID);
        Task<ObservableCollection<Attended>> GetAttendedByEventID(int ID);
        
        #endregion
    }
}
