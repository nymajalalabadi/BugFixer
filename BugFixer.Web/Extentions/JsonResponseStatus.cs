using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Extentions
{
    public class JsonResponseStatus
    {
        public static JsonResult Success()
        {
            return new JsonResult(new { status = "Success" });
        }

        public static JsonResult Error()
        {
            return new JsonResult(new { status = "Error" });
        }
    }
}
