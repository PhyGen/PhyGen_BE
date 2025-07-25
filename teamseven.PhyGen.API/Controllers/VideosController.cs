using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Services.OtherServices;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly SupabaseService _supabaseService;

        public VideosController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        /// <summary>
        /// Upload a video file to the server and return the public URL.
        /// </summary>
        /// <param name="request">Request containing the video file</param>
        /// <returns>URL of the uploaded video</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Upload video",
            Description = "Uploads a video file to Supabase storage and returns the public URL."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadRequest request)
        {
            if (request?.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "No video file provided." });
            }

            try
            {
                var url = await _supabaseService.UploadVideoAsync(request.File);
                return Ok(new UploadResponse { Url = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }


    /// <summary>
    /// Response model for uploaded video
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// Public URL of the uploaded video
        /// </summary>
        public string Url { get; set; }
    }
}
