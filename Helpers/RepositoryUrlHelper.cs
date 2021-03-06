﻿using System;

namespace GitHgMirror.Common.Helpers
{
    public static class RepositoryUrlHelper
    {
        public static string RemoveAuthenticationDetails(string url)
        {
            var uri = new Uri(url);
            var uriBuilder = new UriBuilder(uri);

            uriBuilder.Scheme = "https";
            uriBuilder.Password = null;
            uriBuilder.UserName = null;

            if (uri.PathAndQuery.EndsWith(".git"))
            {
                uriBuilder.Path = uriBuilder.Path.Remove(uriBuilder.Path.LastIndexOf(".git"));
            }

            return uriBuilder.Uri.ToString();
        }
    }
}