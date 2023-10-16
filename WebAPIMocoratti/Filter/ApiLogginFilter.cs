using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIMocoratti.Filter
{
    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger<ApiLogginFilter> _logger;

        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation("###################################");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### Executado -> OnActionExecuted");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation($"###ModelState ->  {context.ModelState.IsValid}");
            _logger.LogInformation("###################################");
        }
    }
}