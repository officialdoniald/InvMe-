using System;
using InvMe.BLL.Labels.ILabels;

namespace InvMe.BLL.Labels.English
{
    public class Cimkek : ICimkek
    {
        public string GetApplicationName()
        {
            return "InvMe!";
        }

        public string GetDeleteAccount()
        {
            return "Delete my account";
        }

        public string GetEmail()
        {
            return "E-Mail";
        }

        public string GetFirstName()
        {
            return "First name";
        }

        public string GetForgotPassword()
        {
            return "Forgot password";
        }

        public string GetLastName()
        {
            return "Last name";
        }

        public string GetLogout()
        {
            return "Logout";
        }

        public string GetMyFriends()
        {
            return "My friends";
        }

        public string GetPassword()
        {
            return "Password";
        }

        public string GetPasswordAgain()
        {
            return "Password again";
        }

        public string GetProfile()
        {
            return "Profile";
        }

        public string GetSelectPhoto()
        {
            return "Album";
        }

        public string GetSelectPictureFromAlbum()
        {
            return "Select photo from album or take a new photo";
        }

        public string GetSendEmail()
        {
            return "Send " + GetEmail();
        }

        public string GetSignIn()
        {
            return "Sign In";
        }

        public string GetSignUp()
        {
            return "Sign Up";
        }

        public string GetTakePicture()
        {
            return "Take a photo";
        }

        public string GetWrongPasswordOrEmail()
        {
            return "Wrong " + GetEmail() + " or password!";
        }

        public string GetBorn()
        {
            return "Born date";
        }

        public string GetCamera()
        {
            return "Camera";
        }

        public string GetWarning()
        {
            return "Warning";
        }

        public string GetOK()
        {
            return "OK";
        }

        public string GetCancel()
        {
            return "Cancel";
        }

        public string GetWrongPassword()
        {
            return "Wrong password!";
        }

        public string GetNotTheSamePasswords()
        {
            return "Passwords aren't the same!";
        }

        public string GetEmailExists()
        {
            return "E-Mail already exists!";
        }

        public string GetSuccess()
        {
            return "Success";
        }

        public string GetMakesurepass()
        {
            return "Make sure, that your password is between 6 and 17 character!";
        }

        public string GetSomethingWentWrong()
        {
            return "Something went wrong, please check back soon...";
        }
        
        public string GetInvalidEmail()
        {
            return "Make sure, that this " + GetEmail() + " is valid!";
        }

        public string GetWrongEmail()
        {
            return "We can't find this " + GetEmail() + ".";
        }

        public string GetSentEmail()
        {
            return "Check your " + GetEmail() + "!";
        }

        public string GetUpdateProfile()
        {
            return "Update profile";
        }

        public string GetUpdate()
        {
            return "Update";
        }

        public string GetNewPassword()
        {
            return "New password";
        }

        public string GetUpdatePassword()
        {
            return "Update password";
        }

        public string GetImpress()
        {
            return "Impress";
        }

        public string GetUpdatePicture()
        {
            return "Update your profile picture";
        }

        public string GetEventName()
        {
            return "Event name";
        }

        public string GetEventTown()
        {
            return "Town";
        }

        public string GetOnline()
        {
            return "Online";
        }

        public string GetSearch()
        {
            return "Search";
        }

        public string GetAddHashtag()
        {
            return "Add hashtag";
        }

        public string GetRequired()
        {
            return "(required)";
        }

        public string GetCreateEvenet()
        {
            return "Create Event";
        }

        public string GetEventDescription()
        {
            return "Event description";
        }

        public string GetEventFrom()
        {
            return "When will the event start?" + GetRequired();
        }

        public string GetEventTo()
        {
            return "When will be the event ended?" + GetRequired();
        }

        public string GetNoMatter()
        {
            return "No matter how long it takes";
        }

        public string GetOnlineEvent()
        {
            return "Online event";
        }

        public string GetPinOnTheMapTheMeetingPoint()
        {
            return "Pin on the map the meeting place";
        }

        public string GetPinOnTheMapThePlacePoint()
        {
            return "Pin on the map the event place";
        }

        public string GetEventPlace()
        {
            return "Place";
        }

        public string GetEnyoneCanAttend()
        {
            return "Anyone can attend";
        }

        public string GetHowManyPersonCanAttend()
        {
            return "How many person can attend?";
        }

        public string GetCreateEvent()
        {
            return "Create event";
        }

        public string GetJoinedEvents()
        {
            return "Joined events";
        }

        public string GetWhoAttended()
        {
            return "Who will attend";
        }

        public string GetLoading()
        {
            return "Loading...";
        }

        public string GetUserDescription()
        {
            return "User description";
        }

        public string GetLostConnection()
        {
            return "Lost internet connection...";
        }

        public string GetNoFacebookAccount()
        {
            return "Not found this Facebook account!";
        }

        public string GetFillEverything()
        {
            return "Complete all the fields!";
        }

        public string GetAddedToYourHashtags()
        {
            return "Added to your hashtags!";
        }

        public string GetFillTheHashtagEntry()
        {
            return "You must fill the hashtag field!";
        }

        public string GetFillTheTownEntry()
        {
            return "You must fill the town field!";
        }

        public string GetDeleteFromYourHashtags()
        {
            return "Deleted from your hashtags!";
        }

        public string GetDeletedAcoountSuccessful()
        {
            return "You have deleted your account!";
        }
        
        public string GetSuccessfulUnsubscribed()
        {
            return "You have successful unsubscribed!";
        }

        public string GetChoosePhoto()
        {
            return "You must choose a photo!";
        }

        public string GetNoPicking()
        {
            return "No picking available, please try again later!";
        }

        public string GetYouHaceChangedyourPhoto()
        {
            return "You have changed your profile picture!";
        }

        public string GetHowmanyIsANumber()
        {
            return "How many person is a number!";
        }

        public string GetSuccessfulCreatedTheEvent()
        {
            return "You have created this event!";
        }

        public string GetBiggerEndDateThenTheBeginDate()
        {
            return "You must choose a bigger end date then the begin date!";
        }

        public string GetBiggerBeginDateThenTheCurrentDate()
        {
            return "You must choose a bigger begin date then the current date!";
        }

        public string GetBiggerEndDateThenTheCurrentDate()
        {
            return "You must choose a bigger end date then the current date!";
        }

        public string GetSuccessfulAttendToThisEvent()
        {
            return "You have successful attended to this event!";
        }
        
    }
}
