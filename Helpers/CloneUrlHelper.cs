namespace GitHgMirror.Common.Helpers
{
    public static class CloneUrlHelper
    {
        public static string ConvertToUrl(string url, bool gitUrlIsHgUrl = false)
        {
            var passwordOrTokenAdded = url.LastIndexOf('@') > 0;
            url = passwordOrTokenAdded ? @"https:\\" + url.Substring(url.LastIndexOf('@') + 1) : url;

            return gitUrlIsHgUrl ? ConvertGitUrlToHgUrl(url) : url;
        }

        public static string ConvertGitUrlToHgUrl(string url)
        {
            if (url.EndsWith(".git"))
            {
                url = url.Remove(url.LastIndexOf(".git"));
            }

            if (url.StartsWith("git+"))
            {
                url = url.Substring("git+".Length);
            }

            return url;
        }
    }
}