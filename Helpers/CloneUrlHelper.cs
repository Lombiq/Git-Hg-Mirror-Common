namespace GitHgMirror.Common.Helpers
{
    public static class CloneUrlHelper
    {
        public static string ConvertToClickableUrl(string url)
        {
            var passwordOrTokenAdded = url.LastIndexOf('@') > 0;
            url = passwordOrTokenAdded ? @"https:\\" + url.Substring(url.LastIndexOf('@') + 1) : url;

            return url;
        }

        public static string GitUrlIsHgUrl(string url)
        {
            url = url.Remove(url.LastIndexOf(".git"));

            if (url.StartsWith("git+"))
            {
                url = url.Substring("git+".Length);
            }

            return url;
        }
    }
}