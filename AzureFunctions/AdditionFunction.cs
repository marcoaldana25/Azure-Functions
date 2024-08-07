using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class AdditionFunction
    {
        private readonly ILogger _logger;

        public AdditionFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AdditionFunction>();
        }

        [Function("Addition")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData httpRequestData)
        {
            var response = httpRequestData
                .CreateResponse(System.Net.HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            try
            {
                var firstInteger = Convert.ToInt64(httpRequestData
                    .Query["first"]);

                var secondInteger = Convert.ToInt64(httpRequestData
                    .Query["second"]);

                response.WriteString($"Sum of integers in query string is: {firstInteger + secondInteger}");
            }
            catch (FormatException formatException)
            {
                var formatErrorMessage = $"Encountered an error when formatting the query string to an Int64 value: {formatException.Message}";

                _logger.LogError(formatErrorMessage, formatException);

                response
                    .WriteString(formatErrorMessage);
            }
            catch (OverflowException overflowException)
            {
                var overflowErrorMessage = $"Encounted an overflow error when casting the query string to an Int64 value: {overflowException.Message}";

                _logger
                    .LogError(overflowErrorMessage, overflowException);

                response
                    .WriteString(overflowErrorMessage);
            }

            return response;
        }
    }
}
