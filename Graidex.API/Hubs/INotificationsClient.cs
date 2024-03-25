using Graidex.Application.Notifications.Authentication.Login;
using Graidex.Application.Notifications.SubjectRequests.Accepted;
using Graidex.Application.Notifications.SubjectRequests.Created;
using Graidex.Application.Notifications.TestResults.Created;
using Graidex.Application.Notifications.TestResults.ReviewedByTeacher;
using Graidex.Application.Notifications.TestResults.ShownToStudent;
using Graidex.Application.Notifications.TestResults.Submitted;

namespace Graidex.API.Hubs
{
    public interface INotificationsClient: ITeacherNotificationsClient, IStudentNotificationsClient
    {
        public Task ReceiveLoginNotification(NewLoginData data);
    }

    public interface ITeacherNotificationsClient
    {
        public Task ReceiveSubjectRequestAcceptedNotification(SubjectRequestAcceptedData data);
        // public Task ReceiveTestOpensNotification(); // scheduled
        public Task ReceiveStudentStartedTestNotification(TestResultCreatedData data);
        public Task ReceiveStudentSubmittedTestNotification(TestResultSubmittedData data);
    }

    public interface IStudentNotificationsClient
    {
        public Task ReceiveNewSubjectRequestNotification(SubjectRequestCreatedData data);
        // public Task ReceiveTestOpensNotification(); // scheduled
        public Task ReceiveTestResultShownToStudentNotification(TestResultShownToStudentData data);
        public Task ReceiveTestResultReviewedByTeacherNotification(TestResultReviewedByTeacherData data);
    }
}
