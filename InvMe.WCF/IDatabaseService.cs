using InvMe.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDatabaseService" in both code and config file together.
    [ServiceContract]
    public interface IDatabaseService
    {
        #region InsertFunctions

        [OperationContract]
        bool InsertUser(User user);

        [OperationContract]
        int InsertEvents(Events events);

        [OperationContract]
        string InsertFriends(Friends friends);

        [OperationContract]
        string InsertHashtagsM(HashtagsM hashtags);

        [OperationContract]
        bool InsertAttended(Attended attended);

        #endregion

        #region DeleteFunctions
        [OperationContract]
        bool DeleteUser(User user);
        [OperationContract]
        bool DeleteEvent(Events events);
        [OperationContract]
        bool DeleteHashtag(HashtagsM hastag);
        [OperationContract]
        bool DeleteFriend(Friends friend);
        [OperationContract]
        bool DeleteAttended(Attended attend);

        #endregion

        #region UpdateFunctions
        [OperationContract]
        bool UpdateUser(int ID, User user);
        [OperationContract]
        bool UpdateEvent(int ID, Events events);
        [OperationContract]
        bool UpdateHashtag(int ID, HashtagsM hastag);
        [OperationContract]
        bool UpdateFriend(int ID, Friends friend);

        #endregion

        #region GetFunctions
        [OperationContract]
        List<User> GetUser();
        [OperationContract]
        List<Events> GetEvent();
        [OperationContract]
        List<HashtagsM> GetHashtag();
        [OperationContract]
        List<Friends> GetFriend();
        [OperationContract]
        List<Attended> GetAttended();

        #endregion

        #region GetFunctionsById
        [OperationContract]
        User GetUserByID(int ID);
        [OperationContract]
        User GetUserByEMAIL(string EMAIL);
        [OperationContract]
        Events GetEventByID(int ID);
        [OperationContract]
        HashtagsM GetHashtagByID(int ID);
        [OperationContract]
        Friends GetFriendByID(int ID);
        [OperationContract]
        List<Attended> GetAttendedByID(int ID);
        [OperationContract]
        List<Attended> GetAttendedByEventID(int ID);

        #endregion
    }
}
