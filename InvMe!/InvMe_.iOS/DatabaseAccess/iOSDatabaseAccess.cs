using System;
using InvMe.BLL.DatabaseAccess;
using InvMe.DAL.Model;
using System.Collections.ObjectModel;
using InvMe_.iOS.DatabaseAccess;
using System.Threading.Tasks;
using System.Data.SqlClient;

[assembly: Xamarin.Forms.Dependency(typeof(iOSDatabaseAccess))]
namespace InvMe_.iOS.DatabaseAccess
{
    public class iOSDatabaseAccess : IiOSDatabaseAccess
    {
        public static string ConnectionString { get; } = "Data Source=tcp:invme.database.windows.net,1433;Initial Catalog=invme;User ID=doniald@invme;Password=96kEHTPp2o0206;";

        #region sql_snipetts
        public string INSERT_USER_SQL { get; } =
            "INSERT INTO [dbo].[USER]" +
            "([EMAIL], [FIRSTNAME], [LASTNAME], [BORNDATE], [PROFILEPICTURE], [FACEBOOK], [PASSWORD]) " +
            "VALUES(" +
            "@EMAIL,@FIRSTNAME,@LASTNAME,@BORNDATE,@PROFILEPICTURE,@FACEBOOK,@PASSWORD)";

        public string INSERT_EVENTS_SQL { get; } =
            "INSERT INTO [dbo].[EVENTS]" +
            "([EVENTNAME], [FROM], [TO], [ONLINE], [TOWN], [PLACE], [HOWMANY], [MEETINGCORD], [PLACECORD]) " +
            "VALUES(" +
            "@EVENTNAME,@FROM,@TO,@ONLINE,@TOWN,@PLACE,@HOWMANY,@MEETINGCORD,@PLACECORD);SET @id = SCOPE_IDENTITY();";

        public string INSERT_HASHTAGS_SQL { get; } =
            "INSERT INTO [dbo].[HASHTAGS]" +
            "([UID], [HASHTAG], [TOWN]) " +
            "VALUES(" +
            "@UID,@HASHTAG,@TOWN)";

        public string INSERT_FRIENDS_SQL { get; } =
            "INSERT INTO [dbo].[FRIENDS]" +
            "([SUID], [GUID]) " +
            "VALUES(" +
            "@SUID,@GUID)";

        public string INSERT_ATTENDED_SQL { get; } =
           "INSERT INTO [dbo].[Attended]" +
           "([user_id], [event_id]) " +
           "VALUES(" +
           "@user_id,@event_id)";

        public string GET_FRIENDS_SQL { get; } =
            "SELECT * FROM [dbo].[FRIENDS]";

        public string GET_ATTENDED_SQL { get; } =
            "SELECT * FROM [dbo].[Attended]";

        public string GET_USER_SQL { get; } =
            "SELECT * FROM [dbo].[USER]";

        public string GET_HASHTAGS_SQL { get; } =
            "SELECT * FROM [dbo].[HASHTAGS]";

        public string GET_EVENTS_SQL { get; } =
            "SELECT * FROM [dbo].[EVENTS]";

        public string DELETE_EVENTS_SQL { get; } =
            "DELETE FROM [dbo].[EVENTS] WHERE ID = @id";

        public string DELETE_USER_SQL { get; } =
            "DELETE FROM [dbo].[USER] WHERE ID = @id;" +
            "DELETE FROM [dbo].[HASHTAGS] WHERE UID = @id;" +
            "DELETE FROM [dbo].[Attended] WHERE user_id = @id";

        public string DELETE_HASHTAG_SQL { get; } =
            "DELETE FROM [dbo].[HASHTAGS] WHERE ID = @id";

        public string DELETE_FRIENDS_SQL { get; } =
            "DELETE FROM [dbo].[FRIENDS] WHERE ID = @id";

        public string DELETE_ATTENDED_SQL { get; } =
            "DELETE FROM [dbo].[Attended] WHERE id = @id";

        public string GETBYID_FRIENDS_SQL { get; } =
            "SELECT * FROM [dbo].[FRIENDS] WHERE ID=@id";
        public string GET_ATTENDEDBYID_SQL { get; } =
                    "SELECT * FROM [dbo].[ATTENDED] WHERE user_id=@id";

        public string GET_ATTENDEDBYEVENTID_SQL { get; } =
                    "SELECT * FROM [dbo].[ATTENDED] WHERE event_id=@id";

        public string GETBYID_USER_SQL { get; } =
            "SELECT * FROM [dbo].[USER] WHERE ID=@id";

        public string GETBYEMAIL_USER_SQL { get; } =
            "SELECT * FROM [dbo].[USER] WHERE EMAIL=@EMAIL";

        public string GETBYID_HASHTAGS_SQL { get; } =
            "SELECT * FROM [dbo].[HASHTAGS] WHERE UID=@id";

        public string GETBYID_EVENTS_SQL { get; } =
            "SELECT * FROM [dbo].[EVENTS] WHERE ID=@id";

        public string UPDATE_EVENTS_SQL { get; } =
            "UPDATE EVENTS SET " +
            "EVENTNAME=@EVENTNAME," +
            "DESCRIPTION=@DESCRIPTION," +
            "FROM=@FROM," +
            "TO=@TO," +
            "ONLINE=@ONLINE," +
            "TOWN=@TOWN," +
            "PLACE=@PLACE," +
            "MDESCRIPTION=@MDESCRIPTION," +
            "HOWMANY=@HOWMANY" +
            "MEETINGCORD=@MEETINGCORD," +
            "PLACECORD=@PLACECORD" +
            " WHERE ID=@ID";

        public string UPDATE_USER_SQL { get; } =
            "UPDATE [dbo].[USER] SET " +
            "EMAIL=@EMAIL," +
            "FIRSTNAME=@FIRSTNAME," +
            "LASTNAME=@LASTNAME," +
            "BORNDATE=@BORNDATE," +
            "PROFILEPICTURE=@PROFILEPICTURE," +
            "FACEBOOK=@FACEBOOK," +
            "PASSWORD=@PASSWORD" +
            " WHERE ID=@ID";

        public string UPDATE_HASHTAGS_SQL { get; } =
           "UPDATE HASHTAGS SET " +
           "UID=@UID," +
           "HASHTAG=@HASHTAG," +
           "TOWN=@TOWN" +
           " WHERE ID=@ID";

        public string UPDATE_FRIENDS_SQL { get; } =
           "UPDATE FRIENDS SET " +
           "SUID=@SUID," +
           "GUID=@GUID" +
           " WHERE ID=@ID";

        #endregion

        #region DeleteFunctions

        public bool DeleteEvent(Events events)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        DELETE_EVENTS_SQL
                        , conn))
                    {
                        cmd.Parameters.AddWithValue("@id", events.id);
                        if (cmd.ExecuteNonQuery() == 1) return true;
                        else return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteFriend(Friends friend)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                       DELETE_FRIENDS_SQL
                        , conn))
                    {
                        cmd.Parameters.AddWithValue("@id", friend.id);
                        if (cmd.ExecuteNonQuery() == 1) return true;
                        else return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAttended(Attended attend)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                       DELETE_ATTENDED_SQL
                        , conn))
                    {
                        cmd.Parameters.AddWithValue("@id", attend.id);
                        if (cmd.ExecuteNonQuery() == 1) return true;
                        else return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteHashtag(HashtagsM hastag)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        DELETE_HASHTAG_SQL
                        , conn))
                    {
                        cmd.Parameters.AddWithValue("@id", hastag.id);
                        if (cmd.ExecuteNonQuery() == 1) return true;
                        else return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(DELETE_USER_SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.id);
                        if (cmd.ExecuteNonQuery() == 1) return true;
                        else return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region GetFunctions

        public ObservableCollection<Events> GetEvent()
        {
            ObservableCollection<Events> events = new ObservableCollection<Events>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_EVENTS_SQL, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Events even = new Events();

                                even.id = reader.GetInt32(reader.GetOrdinal("id"));
                                even.EVENTNAME = reader.GetString(reader.GetOrdinal("EVENTNAME"));
                                try
                                {
                                    even.DESCRIPTION = reader.GetString(reader.GetOrdinal("DESCRIPTION"));
                                }
                                catch (Exception)
                                {
                                    even.DESCRIPTION = null;
                                }
                                try
                                {
                                    even.FROM = reader.GetString(reader.GetOrdinal("FROM"));
                                }
                                catch (Exception)
                                {
                                    even.FROM = DateTimeOffset.Now.ToString("f");
                                }
                                try
                                {
                                    even.TO = reader.GetString(reader.GetOrdinal("TO"));
                                }
                                catch (Exception)
                                {
                                    even.TO = DateTimeOffset.Now.ToString("f");
                                }
                                try
                                {
                                    even.ONLINE = reader.GetInt32(reader.GetOrdinal("ONLINE"));
                                }
                                catch (Exception)
                                {
                                    even.ONLINE = 0;
                                }
                                try
                                {
                                    even.TOWN = reader.GetString(reader.GetOrdinal("TOWN"));
                                }
                                catch (Exception)
                                {
                                    even.TOWN = null;
                                }
                                try
                                {
                                    even.PLACE = reader.GetString(reader.GetOrdinal("PLACE"));
                                }
                                catch (Exception)
                                {
                                    even.PLACE = null;
                                }
                                try
                                {
                                    even.MDESCRIPTION = reader.GetString(reader.GetOrdinal("MDESCRIPTION"));
                                }
                                catch (Exception)
                                {
                                    even.MDESCRIPTION = null;
                                }
                                try
                                {
                                    even.HOWMANY = reader.GetInt32(reader.GetOrdinal("HOWMANY"));
                                }
                                catch (Exception)
                                {
                                    even.HOWMANY = 1;
                                }
                                try
                                {
                                    even.MEETINGCORD = reader.GetString(reader.GetOrdinal("MEETINGCORD"));
                                }
                                catch (Exception)
                                {
                                    even.MEETINGCORD = null;
                                }
                                try
                                {
                                    even.PLACECORD = reader.GetString(reader.GetOrdinal("PLACECORD"));
                                }
                                catch (Exception)
                                {
                                    even.PLACECORD = null;
                                }

                                events.Add(even);
                            }
                        }
                    }
                }
                return events;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<Friends> GetFriend()
        {
            ObservableCollection<Friends> friends = new ObservableCollection<Friends>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_FRIENDS_SQL, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Friends friend = new Friends();

                                friend.id = reader.GetInt32(reader.GetOrdinal("id"));
                                friend.SUID = reader.GetInt32(reader.GetOrdinal("SUID"));
                                friend.GUID = reader.GetInt32(reader.GetOrdinal("GUID"));

                                friends.Add(friend);
                            }
                        }
                    }
                }
                return friends;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<Attended> GetAttended()
        {
            ObservableCollection<Attended> attended = new ObservableCollection<Attended>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_ATTENDED_SQL, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Attended attend = new Attended();

                                attend.id = reader.GetInt32(reader.GetOrdinal("id"));
                                attend.user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
                                attend.event_id = reader.GetInt32(reader.GetOrdinal("event_id"));

                                attended.Add(attend);
                            }
                        }
                    }
                }
                return attended;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<HashtagsM> GetHashtag()
        {
            ObservableCollection<HashtagsM> hashtags = new ObservableCollection<HashtagsM>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_HASHTAGS_SQL, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HashtagsM hashtag = new HashtagsM();

                                hashtag.id = reader.GetInt32(reader.GetOrdinal("id"));
                                hashtag.UID = reader.GetInt32(reader.GetOrdinal("UID"));
                                hashtag.HASHTAG = reader.GetString(reader.GetOrdinal("HASHTAG"));
                                hashtag.TOWN = reader.GetString(reader.GetOrdinal("TOWN"));

                                hashtags.Add(hashtag);
                            }
                        }
                    }
                }
                return hashtags;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<User> GetUser()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_USER_SQL, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                User user = new User();

                                user.id = reader.GetInt32(reader.GetOrdinal("id"));
                                user.EMAIL = reader.GetString(reader.GetOrdinal("EMAIL"));
                                user.FIRSTNAME = reader.GetString(reader.GetOrdinal("FIRSTNAME"));
                                user.LASTNAME = reader.GetString(reader.GetOrdinal("LASTNAME"));
                                user.BORNDATE = reader.GetDateTime(reader.GetOrdinal("BORNDATE"));
                                user.PROFILEPICTURE = reader.GetString(reader.GetOrdinal("PROFILEPICTURE"));
                                
                                try
                                {
                                    user.FACEBOOK = reader.GetString(reader.GetOrdinal("FACEBOOK"));
                                }
                                catch (Exception)
                                {
                                    user.FACEBOOK = null;
                                }

                                user.PASSWORD = reader.GetString(reader.GetOrdinal("PASSWORD"));

                                users.Add(user);
                            }
                        }
                    }
                }
                return users;
            }
            catch (Exception e)
            {
                string ass = e.Message;
                return null;
            }
        }

        #endregion

        #region GetElementByID

        public Events GetEventByID(int ID)
        {
            Events even = new Events();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    GETBYID_EVENTS_SQL
                    , conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                even.id = reader.GetInt32(reader.GetOrdinal("id"));
                                even.EVENTNAME = reader.GetString(reader.GetOrdinal("EVENTNAME"));
                                try
                                {
                                    even.DESCRIPTION = reader.GetString(reader.GetOrdinal("DESCRIPTION"));
                                }
                                catch (Exception)
                                {
                                    even.DESCRIPTION = null;
                                }
                                try
                                {
                                    even.FROM = reader.GetString(reader.GetOrdinal("FROM"));
                                }
                                catch (Exception)
                                {
                                    even.FROM = DateTimeOffset.Now.ToString("f");
                                }
                                try
                                {
                                    even.TO = reader.GetString(reader.GetOrdinal("TO"));
                                }
                                catch (Exception)
                                {
                                    even.TO = DateTimeOffset.Now.ToString("f");
                                }
                                try
                                {
                                    even.ONLINE = reader.GetInt32(reader.GetOrdinal("ONLINE"));
                                }
                                catch (Exception)
                                {
                                    even.ONLINE = 0;
                                }
                                try
                                {
                                    even.TOWN = reader.GetString(reader.GetOrdinal("TOWN"));
                                }
                                catch (Exception)
                                {
                                    even.TOWN = null;
                                }
                                try
                                {
                                    even.PLACE = reader.GetString(reader.GetOrdinal("PLACE"));
                                }
                                catch (Exception)
                                {
                                    even.PLACE = null;
                                }
                                try
                                {
                                    even.MDESCRIPTION = reader.GetString(reader.GetOrdinal("MDESCRIPTION"));
                                }
                                catch (Exception)
                                {
                                    even.MDESCRIPTION = null;
                                }
                                try
                                {
                                    even.HOWMANY = reader.GetInt32(reader.GetOrdinal("HOWMANY"));
                                }
                                catch (Exception)
                                {
                                    even.HOWMANY = 1;
                                }
                                try
                                {
                                    even.MEETINGCORD = reader.GetString(reader.GetOrdinal("MEETINGCORD"));
                                }
                                catch (Exception)
                                {
                                    even.MEETINGCORD = null;
                                }
                                try
                                {
                                    even.PLACECORD = reader.GetString(reader.GetOrdinal("PLACECORD"));
                                }
                                catch (Exception)
                                {
                                    even.PLACECORD = null;
                                }
                            }
                        }
                    }
                }
                return even;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Friends GetFriendByID(int ID)
        {
            Friends friend = new Friends();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    GETBYID_FRIENDS_SQL
                    , conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                friend.id = reader.GetInt32(reader.GetOrdinal("id"));
                                friend.SUID = reader.GetInt32(reader.GetOrdinal("SUID"));
                                friend.GUID = reader.GetInt32(reader.GetOrdinal("GUID"));
                            }
                        }
                    }
                }
                return friend;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public HashtagsM GetHashtagByID(int ID)
        {
            HashtagsM hashtag = new HashtagsM();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    GETBYID_HASHTAGS_SQL
                    , conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                hashtag.id = reader.GetInt32(reader.GetOrdinal("id"));
                                hashtag.UID = reader.GetInt32(reader.GetOrdinal("UID"));
                                hashtag.HASHTAG = reader.GetString(reader.GetOrdinal("HASHTAG"));
                                hashtag.TOWN = reader.GetString(reader.GetOrdinal("TOWN"));
                            }
                        }
                    }
                }
                return hashtag;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User GetUserByID(int ID)
        {
            User user = new User();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    GETBYID_USER_SQL
                    , conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@id", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                user.id = reader.GetInt32(reader.GetOrdinal("id"));
                                user.EMAIL = reader.GetString(reader.GetOrdinal("EMAIL"));
                                user.FIRSTNAME = reader.GetString(reader.GetOrdinal("FIRSTNAME"));
                                user.LASTNAME = reader.GetString(reader.GetOrdinal("LASTNAME"));
                                user.BORNDATE = reader.GetDateTime(reader.GetOrdinal("BORNDATE"));
                                user.PROFILEPICTURE = reader.GetString(reader.GetOrdinal("PROFILEPICTURE"));

                                try
                                {
                                    user.FACEBOOK = reader.GetString(reader.GetOrdinal("FACEBOOK"));
                                }
                                catch (Exception)
                                {
                                    user.FACEBOOK = null;
                                }
                                user.PASSWORD = reader.GetString(reader.GetOrdinal("PASSWORD"));
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User GetUserByEMAIL(string EMAIL)
        {
            User user = new User();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    GETBYEMAIL_USER_SQL
                    , conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@EMAIL", EMAIL);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                user.id = reader.GetInt32(reader.GetOrdinal("id"));
                                user.EMAIL = reader.GetString(reader.GetOrdinal("EMAIL"));
                                user.FIRSTNAME = reader.GetString(reader.GetOrdinal("FIRSTNAME"));
                                user.LASTNAME = reader.GetString(reader.GetOrdinal("LASTNAME"));
                                user.BORNDATE = reader.GetDateTime(reader.GetOrdinal("BORNDATE"));
                                user.PROFILEPICTURE = reader.GetString(reader.GetOrdinal("PROFILEPICTURE"));
                                
                                try
                                {
                                    user.FACEBOOK = reader.GetString(reader.GetOrdinal("FACEBOOK"));
                                }
                                catch (Exception)
                                {
                                    user.FACEBOOK = null;
                                }
                                user.PASSWORD = reader.GetString(reader.GetOrdinal("PASSWORD"));
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<Attended> GetAttendedByID(int ID)
        {
            ObservableCollection<Attended> attended = new ObservableCollection<Attended>();

            try
            {

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_ATTENDEDBYID_SQL, conn))
                {
                    cmd.Parameters.AddWithValue("@id", ID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Attended attend = new Attended();

                                attend.id = reader.GetInt32(reader.GetOrdinal("id"));
                                attend.user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
                                attend.event_id = reader.GetInt32(reader.GetOrdinal("event_id"));

                                attended.Add(attend);
                            }
                        }
                    }
                }
                return attended;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ObservableCollection<Attended> GetAttendedByEventID(int ID)
        {
            ObservableCollection<Attended> attended = new ObservableCollection<Attended>();

            try
            {

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(GET_ATTENDEDBYEVENTID_SQL, conn))
                {
                    cmd.Parameters.AddWithValue("@id", ID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Attended attend = new Attended();

                                attend.id = reader.GetInt32(reader.GetOrdinal("id"));
                                attend.user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
                                attend.event_id = reader.GetInt32(reader.GetOrdinal("event_id"));

                                attended.Add(attend);
                            }
                        }
                    }
                }
                return attended;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region InsertFunctions

        public bool InsertUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(INSERT_USER_SQL, conn);

                    cmd.Parameters.Add(
                        new SqlParameter("@EMAIL", user.EMAIL)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@FIRSTNAME", user.FIRSTNAME)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@LASTNAME", user.LASTNAME)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@BORNDATE", (object)user.BORNDATE ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.DateTime
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@PROFILEPICTURE", (object)user.PROFILEPICTURE ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@FACEBOOK", (object)user.FACEBOOK ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@PASSWORD", user.PASSWORD)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertEvent(Events events)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(INSERT_EVENTS_SQL, conn);

                    cmd.Parameters.Add(
                        new SqlParameter("@EVENTNAME", events.EVENTNAME)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@HOWMANY", events.HOWMANY)
                        {
                            SqlDbType = System.Data.SqlDbType.Int
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@ONLINE", events.ONLINE)
                        {
                            SqlDbType = System.Data.SqlDbType.Int
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@FROM", events.FROM)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@TO", events.TO)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@TOWN", events.TOWN)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@PLACE", events.PLACE)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@MEETINGCORD", events.MEETINGCORD)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@PLACECORD", events.PLACECORD)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );

                    var idpar = new SqlParameter("@id", DBNull.Value)
                    {
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    cmd.Parameters.Add(idpar);

                    try
                    {
                        conn.Open();

                        cmd.CommandTimeout = 600;

                        cmd.ExecuteNonQuery();

                        events.id = (int)idpar.Value;

                        return events.id;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string InsertFriend(Friends friends)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(INSERT_FRIENDS_SQL, conn);

                    cmd.Parameters.Add(
                        new SqlParameter("@SUID", friends.SUID)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@GUID", friends.GUID)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );

                    cmd.ExecuteNonQuery();

                    return "";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public bool InsertAttended(Attended attended)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(INSERT_ATTENDED_SQL, conn);

                    cmd.Parameters.Add(
                        new SqlParameter("@user_id", attended.user_id)
                        {
                            SqlDbType = System.Data.SqlDbType.Int
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@event_id", attended.event_id)
                        {
                            SqlDbType = System.Data.SqlDbType.Int
                        }
                     );

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string InsertHashtag(HashtagsM hashtags)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(INSERT_HASHTAGS_SQL, conn);

                    cmd.Parameters.Add(
                        new SqlParameter("@HASHTAG", hashtags.HASHTAG)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@UID", hashtags.UID)
                        {
                            SqlDbType = System.Data.SqlDbType.Int
                        }
                     );
                    cmd.Parameters.Add(
                        new SqlParameter("@TOWN", hashtags.TOWN)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar
                        }
                     );

                    cmd.ExecuteNonQuery();

                    return "";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        #region UpdateFunctions

        public bool UpdateEvent(int ID, Events events)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand(
                            UPDATE_EVENTS_SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@EVENTNAME", events.EVENTNAME);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", events.DESCRIPTION);
                        cmd.Parameters.AddWithValue("@FROM", events.FROM);
                        cmd.Parameters.AddWithValue("@TO", events.TO);
                        cmd.Parameters.AddWithValue("@ONLINE", events.ONLINE);
                        cmd.Parameters.AddWithValue("@TOWN", events.TOWN);
                        cmd.Parameters.AddWithValue("@PLACE", events.PLACE);
                        cmd.Parameters.AddWithValue("@MDESCRIPTION", events.MDESCRIPTION);
                        cmd.Parameters.AddWithValue("@HOWMANY", events.HOWMANY);
                        cmd.Parameters.AddWithValue("@MEETINGCORD", events.HOWMANY);
                        cmd.Parameters.AddWithValue("@PLACECORD", events.HOWMANY);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 1) return true; else return false;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool UpdateFriend(int ID, Friends friend)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand(
                            UPDATE_FRIENDS_SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@SUID", friend.SUID);
                        cmd.Parameters.AddWithValue("@GUID", friend.GUID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 1) return true; else return false;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool UpdateHashtag(int ID, HashtagsM hastag)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand(
                            UPDATE_HASHTAGS_SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@UID", hastag.UID);
                        cmd.Parameters.AddWithValue("@HASHTAG", hastag.HASHTAG);
                        cmd.Parameters.AddWithValue("@TOWN", hastag.TOWN);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 1) return true; else return false;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool UpdateUser(int ID, User user)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd =
                        new SqlCommand(
                            UPDATE_USER_SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@EMAIL", user.EMAIL);
                        cmd.Parameters.AddWithValue("@FIRSTNAME", user.FIRSTNAME);
                        cmd.Parameters.AddWithValue("@LASTNAME", user.LASTNAME);
                        cmd.Parameters.AddWithValue("@BORNDATE", user.BORNDATE);
                        cmd.Parameters.AddWithValue("@PROFILEPICTURE", user.PROFILEPICTURE);
                        cmd.Parameters.AddWithValue("@FACEBOOK", user.FACEBOOK);
                        cmd.Parameters.AddWithValue("@PASSWORD", user.PASSWORD);

                        int rows = cmd.ExecuteNonQuery();

                        //if (rows == 1) return true; else return false;
                        if (rows == 1) return true;
                        return false;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        #endregion
    }
}