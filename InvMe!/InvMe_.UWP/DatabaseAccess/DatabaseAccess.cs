using InvMe.BLL.DatabaseAccess;
using InvMe_.UWP.DatabaseAccess;
using System;
using System.Threading.Tasks;
using InvMe.DAL.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseAccess))]
namespace InvMe_.UWP.DatabaseAccess
{
    public class DatabaseAccess : IDatabaseAccess
    {
        #region attr

        ServiceReference1.DatabaseServiceClient client =
                new ServiceReference1.DatabaseServiceClient(ServiceReference1.DatabaseServiceClient.EndpointConfiguration.BasicHttpsBinding_IDatabaseService);
        
        #endregion

        #region DeleteFunctions

        public async Task<bool> DeleteEvent(Events events)
        {
            return await client.DeleteEventAsync(events);
        }

        public async Task<bool> DeleteFriend(Friends friend)
        {
            return await client.DeleteFriendAsync(friend);
        }

        public async Task<bool> DeleteHashtag(HashtagsM hastag)
        {
            return await client.DeleteHashtagAsync(hastag);
        }

        public async Task<bool> DeleteUser(User user)
        {
            return await client.DeleteUserAsync(user);
        }

        public async Task<bool> DeleteAttended(Attended attend)
        {
            return await client.DeleteAttendedAsync(attend);
        }

        #endregion

        #region GetFunctions

        public async Task<ObservableCollection<Events>> GetEvent()
        {
            return await client.GetEventAsync();
        }

        public async Task<ObservableCollection<User>> GetUser()
        {
            return await client.GetUserAsync();
        }

        public async Task<ObservableCollection<Friends>> GetFriend()
        {
            return await client.GetFriendAsync();
        }
        public async Task<ObservableCollection<HashtagsM>> GetHashtag()
        {
            return await client.GetHashtagAsync();
        }
        public async Task<ObservableCollection<Attended>> GetAttended()
        {
            return await client.GetAttendedAsync();
        }

        #endregion

        #region GetElementById

        public async Task<Events> GetEventByID(int ID)
        {
            return await client.GetEventByIDAsync(ID);
        }

        public async Task<Friends> GetFriendByID(int ID)
        {
            return await client.GetFriendByIDAsync(ID);
        }

        public async Task<HashtagsM> GetHashtagByID(int ID)
        {
            return await client.GetHashtagByIDAsync(ID);
        }

        public async Task<User> GetUserByID(int ID)
        {
            return await client.GetUserByIDAsync(ID);
        }

        public async Task<User> GetUserByEMAIL(string EMAIL)
        {
            return await client.GetUserByEMAILAsync(EMAIL);
        }

        public async Task<ObservableCollection<Attended>> GetAttendedByID(int ID)
        {
            return await client.GetAttendedByIDAsync(ID);
        }
        public async Task<ObservableCollection<Attended>> GetAttendedByEventID(int ID)
        {
            return await client.GetAttendedByEventIDAsync(ID);
        }
        #endregion

        #region InsertFunctions

        public async Task<string> InserFriend(Friends friend)
        {
            return await client.InsertFriendsAsync(friend);
        }

        public async Task<int> InsertEventAsync(Events events)
        {
            return await client.InsertEventsAsync(events);
        }

        public async Task<string> InsertHashtag(HashtagsM hastag)
        {
            return await client.InsertHashtagsMAsync(hastag);
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            return await client.InsertUserAsync(user);
        }

        public async Task<bool> InsertAttendedAsync(Attended attend)
        {
            return await client.InsertAttendedAsync(attend);
        }

        #endregion

        #region UpdateFunctions

        public async Task<bool> UpdateEvent(int ID, Events events)
        {
            return await client.UpdateEventAsync(ID,events);
        }

        public async Task<bool> UpdateFriend(int ID, Friends friend)
        {
            return await client.UpdateFriendAsync(ID, friend);
        }

        public async Task<bool> UpdateHashtag(int ID, HashtagsM hastag)
        {
            return await client.UpdateHashtagAsync(ID, hastag);
        }

        public async Task<bool> UpdateUser(int ID, User user)
        {
            return await client.UpdateUserAsync(ID, user);
        }
        
        #endregion
    }
}
