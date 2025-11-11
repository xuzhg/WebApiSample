using Microsoft.AspNetCore.OData.Extensions;

namespace DeepInsertVsDeepUpdate
{
    public interface IDataSource
    {
        IList<AgentInstance> Instances { get; }

        IList<AgentCardManifest> Manifests { get; }

        AgentInstance? GetInstance(int id);

        AgentCardManifest? GetManifest(int cardId);

        void AddManfest(AgentCardManifest agentInstance);

        void AddAgentInstance(AgentInstance agentInstance);
    }
}
