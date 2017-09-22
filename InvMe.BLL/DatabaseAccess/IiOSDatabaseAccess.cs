using InvMe.DAL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.BLL.DatabaseAccess
{
    public interface IiOSDatabaseAccess
    {
        #region insert
        bool InsertUser(User user);
        int InsertEvent(Events events);
        string InsertHashtag(HashtagsM hastag);
        string InsertFriend(Friends friend);
        bool InsertAttended(Attended attend);

#endregion

        #region DeleteFunctions

        bool DeleteUser(User user);
        bool DeleteEvent(Events events);
        bool DeleteHashtag(HashtagsM hastag);
        bool DeleteFriend(Friends friend);
        bool DeleteAttended(Attended attend);

        #endregion

        #region UpdateFunctions

        bool UpdateUser(int ID, User user);
        bool UpdateEvent(int ID, Events events);
        bool UpdateHashtag(int ID, HashtagsM hastag);
        bool UpdateFriend(int ID, Friends friend);

        #endregion

        #region GetFunctions

        ObservableCollection<User> GetUser();
        ObservableCollection<Events> GetEvent();
        ObservableCollection<HashtagsM> GetHashtag();
        ObservableCollection<Friends> GetFriend();
        ObservableCollection<Attended> GetAttended();
        #endregion

        #region GetFunctionsById

        User GetUserByID(int ID);
        User GetUserByEMAIL(string EMAIL);
        Events GetEventByID(int ID);
        HashtagsM GetHashtagByID(int ID);
        Friends GetFriendByID(int ID);
        ObservableCollection<Attended> GetAttendedByID(int ID);
        ObservableCollection<Attended> GetAttendedByEventID(int ID);

        #endregion
    }
}
