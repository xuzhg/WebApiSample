using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DeepInsertVsDeepUpdate.Controllers
{
    public class MyController : ODataController
    {
        private readonly IDataSource _db;

        public MyController(IDataSource db)
        {
            _db = db;
        }

        [HttpGet("odata/AgentInstances")]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Instances);
        }

        [HttpGet("odata/manifests")]
        [EnableQuery]
        public IActionResult GetManifests()
        {
            return Ok(_db.Manifests);
        }

        [HttpPost("odata/AgentInstances")]
        public IActionResult Post([FromBody] AgentInstance agentInstance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check the deep update parts:
            foreach (var metadata in agentInstance.DeepUpdateMetadata)
            {
                // just update
                int id = -1;
                if (metadata.PropertyName == "Manifest")
                {
                    id = agentInstance.Manifest.Id;
                }
                else if (metadata.PropertyName == "Manifest2")
                {
                    id = agentInstance.Manifest2.Id;
                }

                // Deep Insert for Manifest
                // Check if the Manifest with the same ODataId already exists
                var existingManifest = _db.GetManifest(id);
                if (existingManifest == null)
                {
                    return NotFound($"An AgentCardManifest with ODataId '{metadata.ODataId}' for navigation property '{metadata.PropertyName}' not existed.");
                }

                if (metadata.PropertyName == "Manifest")
                {
                    agentInstance.Manifest = existingManifest;
                }
                else if (metadata.PropertyName == "Manifest2")
                {
                    agentInstance.Manifest2 = existingManifest;
                }
            }

            // check the deep insert parts:
            if (!agentInstance.DeepUpdateMetadata.Any(c => c.PropertyName == "Manifest"))
            {
                // If agentCardId already exist in db, return 409 
                AgentCardManifest manifest = _db.GetManifest(agentInstance.Manifest.Id);
                if (manifest != null)
                {
                    return Conflict($"An new AgentCardManifest Instance with the same ID {agentInstance.Manifest.Id} for 'Manifest' already exists.");
                }

                _db.AddManfest(agentInstance.Manifest);
            }

            if (!agentInstance.DeepUpdateMetadata.Any(c => c.PropertyName == "Manifest2"))
            {
                // If agentCardId already exist in db, return 409 
                AgentCardManifest manifest = _db.GetManifest(agentInstance.Manifest2.Id);
                if (manifest != null)
                {
                    return Conflict($"An new AgentCardManifest Instance with the same ID {agentInstance.Manifest2.Id} for 'Manifest2' already exists.");
                }

                _db.AddManfest(agentInstance.Manifest2);
            }

            _db.AddAgentInstance(agentInstance);

            // eturn 200
            return Ok();
        }
    }
}
