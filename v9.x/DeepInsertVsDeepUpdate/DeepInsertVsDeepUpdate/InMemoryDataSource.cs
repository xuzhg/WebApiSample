namespace DeepInsertVsDeepUpdate
{
    // for simplicity, using in-memory data source without any checks
    public class InMemoryDataSource : IDataSource
    {
        private readonly List<AgentCardManifest> _manifests = new();
        private readonly List<AgentInstance> _agentInstances = new();

        public IList<AgentInstance> Instances => _agentInstances;

        public IList<AgentCardManifest> Manifests => _manifests;

        public void Init()
        {
            _manifests.Clear();
            _agentInstances.Clear();

            AgentInstance agent = new AgentInstance()
            {
                Id = 1,
                Name = "Peter",
                Manifest = new AgentCardManifest
                {
                    Id = 1,
                    DisplayName = "Manifest One"
                },
                Manifest2 = new AgentCardManifest
                {
                    Id = 21,
                    DisplayName = "Manifest2 One"
                }
            };
            _agentInstances.Add(agent);
            _manifests.Add(agent.Manifest);
            _manifests.Add(agent.Manifest2);

            agent = new AgentInstance()
            {
                Id = 11,
                Name = "Sam",
                Manifest = new AgentCardManifest
                {
                    Id = 2,
                    DisplayName = "Manifest Sam"
                },
                Manifest2 = new AgentCardManifest
                {
                    Id = 22,
                    DisplayName = "Manifest2 Sam"
                }
            };
            _agentInstances.Add(agent);
            _manifests.Add(agent.Manifest);
            _manifests.Add(agent.Manifest2);
        }

        public AgentInstance? GetInstance(int id)
        {
            return _agentInstances.FirstOrDefault(i => i.Id == id);
        }

        public AgentCardManifest? GetManifest(int cardId)
        {
            return _manifests.FirstOrDefault(m => m.Id == cardId);
        }

        public void AddManfest(AgentCardManifest manifest)
        {
            _manifests.Add(manifest);
        }

        public void AddAgentInstance(AgentInstance agentInstance)
        {
            int max = _agentInstances.Count == 0 ? 1 : _agentInstances.Max(a => a.Id);
            agentInstance.Id = max + 1;
            _agentInstances.Add(agentInstance);
        }
    }
}
