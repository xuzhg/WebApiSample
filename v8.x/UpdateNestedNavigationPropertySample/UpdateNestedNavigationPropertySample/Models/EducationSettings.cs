namespace UpdateNestedNavigationPropertySample.Models
{
    public class EducationSettings
    {
        public bool SubmissionAnimationDisabled { get; set; }

        public IList<EducationGradingCategory> GradingCategories { get; set; }
    }
}
