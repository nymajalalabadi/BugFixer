namespace BugFixer.Application.Statics
{
    public class PathTools
    {
        #region User

        public static readonly string DefaultUserAvatar = "DefaultAvatar.png";

        public static readonly string UserAvatarServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/user/orgin/");
        public static readonly string UserAvatarPath = "/content/user/orgin/";


        public static readonly string UserAvatarServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/user/thumb/");
        public static readonly string UserAvatarThumb = "/content/user/thumb/";

        #endregion


        #region site

        public static string SiteAddress = "https://localhost:7109";

        #endregion


        #region Ckeditor

        public static readonly string EditorImageServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/ckeditor/");
        public static readonly string EditorImagePath = "/content/ckeditor/";

        #endregion
    }
}
