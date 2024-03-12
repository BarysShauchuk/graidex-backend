namespace Graidex.API.Hubs
{
    public interface INotificationsClient: ITeacherNotificationsClient, IStudentNotificationsClient
    {
        public Task ReceiveLoginNotification();
        public Task ReceiveNewVersionNotification();

        public Task ReceiveApplicationTestNotification(string message);
    }

    public interface ITeacherNotificationsClient
    {
        public Task ReceiveNewSubjectStudentNotification();
        public Task ReceiveTestOpensNotification(); // TODO: with bool IsHidden
        public Task ReceiveStudentStartedTestNotification();
        public Task ReceiveStudentSubmittedTestNotification();
        public Task ReceiveTestResultAutoCheckCompletedNotification();
    }

    public interface IStudentNotificationsClient
    {
        public Task ReceiveNewSubjectRequestNotification();
        public Task ReceiveTestOpensNotification();
        public Task ReceiveCanReviewResultNotification();
    }
}
