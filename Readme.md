# Git-hg Mirror Common readme



[Orchard CMS](http://orchardproject.net/) module serving as the frontend of the two-way Git-Mercurial repository syncing service [Git-hg Mirror](https://githgmirror.com).  The service component is [Git-Hg Mirror Daemon](https://github.com/Lombiq/Git-Hg-Mirror-Daemon).

This is a C# project that you'll need Visual Studio to work with. Commits in the master/default branch represent deployments, i.e. the latest commit in that branch shows the version currently running in production.

The project's source is available in two public source repositories, automatically mirrored in both directions with Git-hg Mirror itself:

- [https://bitbucket.org/Lombiq/git-hg-mirror-daemon-common](https://bitbucket.org/Lombiq/git-hg-mirror-daemon-common) (Mercurial repository)
- [https://github.com/Lombiq/Git-hg-Mirror-Common](https://github.com/Lombiq/Git-hg-Mirror-Common) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**. Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](https://lombiq.com/). Commercial-grade support is available through Lombiq.


## Developer overview

To work with the module locally you'll need to put it in an Orchard solution among the modules in a folder named exactly "GitHgMirror.Common". You'll need the same Orchard version as it's stated in the Module.txt file.

After enabling the module you'll see the same UI to create mirroring configurations than on githgmirror.com (note that it won't exactly look the same since the theme component of the website is not open source, since it's of little use to anybody else).