namespace Graidex.Application.Notifications.SubjectRequests.Accepted
{
    public class SubjectRequestAcceptedData
    {
        public required string SubjectTitle { get; set; }
        public int SubjectId { get; set; }
        public required string StudentEmail { get; set; }
    }
}