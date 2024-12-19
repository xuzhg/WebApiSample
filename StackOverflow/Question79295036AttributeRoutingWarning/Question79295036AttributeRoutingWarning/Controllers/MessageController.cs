using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Question79295036AttributeRoutingWarning.Models;

namespace Question79295036AttributeRoutingWarning.Controllers;

// [Authorize]
//[Route("odata")]
public class MessageController : ODataController
{
    //[HttpPost("GetTagMessages")]
    [HttpPost]
    public async Task<IActionResult> GetTagMessages(ODataActionParameters parameters)
    {

        //var messages = await _db.MessageTags.Where(x => tagIds.Any(t => t == x.TagId))
        //    .AsNoTracking().ToListAsync();
        // var mapped = _mapper.Map<List<Message>, List<MessageViewModel>>(messages);
        var tagIds = parameters["tagIds"] as IEnumerable<int>;

        var mapped = tagIds.Select(i => new MessageViewModel { Id = i }).ToList();
        return Ok(mapped);
    }

    //[HttpPost("GetTagMessages()")]
    //public async Task<IActionResult> GetTagMessages([FromBody] IList<int> tagIds)
    //{

    //    //var messages = await _db.MessageTags.Where(x => tagIds.Any(t => t == x.TagId))
    //    //    .AsNoTracking().ToListAsync();
    //    // var mapped = _mapper.Map<List<Message>, List<MessageViewModel>>(messages);

    //    var mapped = tagIds.Select(i => new MessageViewModel { Id = i }).ToList();
    //    return Ok(mapped);
    //}

}