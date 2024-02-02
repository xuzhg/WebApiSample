namespace SpeakerWebApi.Models
{
    public class Class
    {
        public int Id { get; set; }
        public List<Student> Students { get; }
    }

    public class Student
    {
        public int Id { get; set; }

        public List<SpeakerSubmissionResource> SpeakerSubmissions { get; }
    }

    public class SpeakerSubmissionResource
    {
        public int Id { get; set; }

        public CoachSummaryData RehearsalReport { get; set; }
    }

    public class CoachSummaryData
    {
        public object VisionCritiqueFrames { get; set; }
    }
}
