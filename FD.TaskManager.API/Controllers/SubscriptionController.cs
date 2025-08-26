using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Shared.Dto_s.Subscription;

namespace FD.TaskManager.API.Controllers
{
    [Route("api/subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubPlanManager _subPlanManager;
        public SubscriptionController(SubPlanManager subPlanManager)
        {
            _subPlanManager = subPlanManager ??
                throw new ArgumentNullException(nameof(subPlanManager));
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddSubscriptionDto addSubscriptionDto)
        {
            try
            {
                var result = await _subPlanManager.AddAsync(addSubscriptionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error accured: {ex}");
                return NotFound(ex.Message);
            }
        }
        [HttpGet("getsubscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var result = await _subPlanManager.GetSubscriptionsAsync();
            return result is not null ? Ok(result) : NotFound();
        }
        [HttpGet("getsubscription")]
        public async Task<IActionResult> GetSubscription(Guid? id, Guid? tenantId)
        {
            var result = await _subPlanManager.GetSubscriptionAsync(id, tenantId);
            return result is not null ? Ok(result) : NotFound();
        }
        [HttpPatch("updatesubscription")]
        public async Task<IActionResult> UpdateSubscriptino(Guid id, UpdateSubscriptionDto updateSubscriptionDto)
        {
            try
            {
                var result = await _subPlanManager.UpdateSubscriptinoAsync(id, updateSubscriptionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error accured while handling the request: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
        [HttpGet("getsubscriptionhistories")]
        public async Task<IActionResult> GetSubscriptionHistories(string? searchQuery)
        {
            var result = await _subPlanManager.GetSubscriptionHistoriesAsync(searchQuery);
            return result is not null ? Ok(result) : NotFound();
        }
        [HttpGet("getsubscriptionhistorieswithtenantid")]
        public async Task<IActionResult> GetSubscriptionHistoriesWithTenantId(Guid id)
        {
            var result = await _subPlanManager.GetSubscriptionHistoriesWithTenantIdAsync(id);
            return result is not null ? Ok(result) : NotFound();
        }
    }
}
