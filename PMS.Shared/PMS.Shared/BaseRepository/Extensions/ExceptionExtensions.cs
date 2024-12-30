using Newtonsoft.Json;
using System.Diagnostics;

namespace PMS.Shared.BaseRepository.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToJSON(this Exception ex)
        {
            var errorMessage = JsonConvert.SerializeObject(new { Error = ex.Message, InnerError = ex?.InnerException?.Message });
            return errorMessage;
        }

        public static string ExceptionFilePath(this Exception ex)
        {
            var stackTrace = new StackTrace(ex, true);
            foreach (var frame in stackTrace.GetFrames())
            {
                var filePath = frame?.GetFileName();
                if (!string.IsNullOrEmpty(filePath))
                {
                    return filePath;
                }
            }
            return string.Empty;
        }
    }
}