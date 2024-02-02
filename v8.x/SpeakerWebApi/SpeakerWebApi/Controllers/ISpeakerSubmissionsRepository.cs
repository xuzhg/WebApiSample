using Microsoft.AspNetCore.OData.Deltas;
using SpeakerWebApi.Models;

namespace SpeakerWebApi.Controllers
{
    public interface ISpeakerSubmissionsRepository
    {
        Task<SpeakerSubmissionResource> UpdateSubmissionAsync(int classId,
            int studentId,
            int speakerSubmissionId,
            Delta<SpeakerSubmissionResource> speakerSubmissionDelta);
    }

    public class SpeakerSubmissionsRepository : ISpeakerSubmissionsRepository
    {
        public async Task<SpeakerSubmissionResource> UpdateSubmissionAsync(int classId, int studentId, int speakerSubmissionId,
            Delta<SpeakerSubmissionResource> speakerSubmissionDelta)
        {
            SpeakerSubmissionResource speakerSubmissionResource;

            if (classId == 1)
            {
                speakerSubmissionResource = new SpeakerSubmissionResource
                {
                    Id = classId + studentId,
                    RehearsalReport = new CoachSummaryData
                    {
                        VisionCritiqueFrames = new SpeakerSubmissionResource
                        {
                            Id = classId + studentId + speakerSubmissionId,
                            RehearsalReport = new CoachSummaryData
                            {
                                VisionCritiqueFrames = new List<int> { 1, 2, 3 }
                            }
                        }
                    }
                };
            }
            else if (classId == 2)
            {
                speakerSubmissionResource = new SpeakerSubmissionResource
                {
                    Id = classId + studentId,
                    RehearsalReport = new CoachSummaryData
                    {
                        VisionCritiqueFrames = new Dictionary<string, object>
                        {
                            { "p1", 1 },
                            { "p2", "a string" }
                        }
                    }
                };
            }
            else if (classId == 3)
            {
                speakerSubmissionResource = new SpeakerSubmissionResource
                {
                    Id = classId + studentId,
                    RehearsalReport = new CoachSummaryData
                    {
                        VisionCritiqueFrames = classId * studentId
                    }
                };
            }
            else
            {
                speakerSubmissionResource = new SpeakerSubmissionResource
                {
                    Id = classId + studentId,
                    RehearsalReport = new CoachSummaryData
                    {
                        VisionCritiqueFrames = GetDeltaObject(speakerSubmissionDelta)
                    }
                };
            }

            return await Task.FromResult(speakerSubmissionResource);
        }

        private static object GetDeltaObject(IDelta delta)
        {
            IDictionary<string, object> values = new Dictionary<string, object>();
            foreach (var item in delta.GetChangedPropertyNames())
            {
                delta.TryGetPropertyValue(item, out object value);
                if (value == null)
                {
                    values[item] = null;
                    continue;
                }

                Type valueType = value.GetType();

                if (value is IDelta nestedDelta)
                {
                    values[item] = GetDeltaObject(nestedDelta);
                }
                else
                {
                    values[item] = value;
                }
            }

            return values;
        }
    }
}
