using NewMark.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;
using NewMark.WebApi.Model;
using Microsoft.Extensions.Options;
using Azure;
using Azure.Storage.Blobs;

namespace NewMark.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        //private static readonly List<Property> Property;
        private readonly ILogger<PropertyController> _logger;
        private readonly AzBlob _azBlob;

        public PropertyController(ILogger<PropertyController> logger, IOptions<AzBlob> azBlob)
        {
            _logger = logger;
            _azBlob = azBlob.Value;
        }

        /// <summary>
        /// Get all items asynchronously.
        /// </summary>
        /// <returns>A list of items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetProperty()
        {
            try
            {
                // Simulating async operation       
                _logger.LogInformation("GetProperty API accessed.");

                if (string.IsNullOrEmpty(_azBlob.BlobURL))
                {
                        _logger.LogError("Azure Blob URL not found or empty !!");
                    return NotFound("Azure Blob URL not found or empty !!"); 
                }
                if (string.IsNullOrEmpty(_azBlob.SasToken))
                {
                        _logger.LogError("SAS Token for Azure Blob URL not found or empty !!");
                    return NotFound("SAS Token for Azure Blob URL not found or empty !!");
                }
                var items = await Task.Run(() => GetPropertyDataFromAzBlob(_azBlob.BlobURL, _azBlob.SasToken));

                    if (items == null || !items.Any())
                    {
                            _logger.LogError("No Apartment found.");
                        return NotFound("No Apartment found.");
                    }
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown {0}", ex.Message);
                return StatusCode(500, new { Message = "An internal server error occurred. Please try again later." });
            }
        }

        private async Task<IList<Property>> GetPropertyDataFromAzBlob(string azBlobUri, string sasToken)
        {
            try
            {
                // Simulating async operation       
                _logger.LogInformation("Start : Azure Blob API-GetPropertyDataFromAzBlob accessed.");

                var client = new BlobClient(new Uri(azBlobUri), new AzureSasCredential(sasToken));

                var queryResponse = await client.DownloadContentAsync();
             
                string jsonString = queryResponse.Value.Content.ToString();

                List<Property> properties = JsonConvert.DeserializeObject<Property[]>(jsonString).ToList();

                _logger.LogInformation("End : Azure Blob API-GetPropertyDataFromAzBlob Completed.");
                return properties;

            }
            catch (Exception ex)
            {
                throw new Exception("custom error");
            }
        }
    }

}
