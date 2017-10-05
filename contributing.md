# Welcome
Thank you for considering contributing to Gepsio! It's people like you that make Gepsio such a great library.

Following these guidelines helps to communicate that you respect the time of the developers managing and developing this open source project. In return, they should reciprocate that respect in addressing your issue, assessing changes, and helping you finalize your pull requests.

Gepsio is an open source project and we love to receive contributions from our community — you! There are many ways to contribute, from writing tutorials or blog posts, improving the documentation, submitting bug reports and feature requests or writing code which can be incorporated into Gepsio itself.
# Getting in Touch
The project would love to hear from *you*! 

Feel free to reach the project administrators via social media or email. The project maintains a [Wiki page](https://github.com/JeffFerguson/gepsio/wiki) which lists the various ways to reach the project administrators.
# Branching Strategy and Feature/Fix Workflow
Gepsio uses the [GitFlow](http://nvie.com/posts/a-successful-git-branching-model/) branching strategy. In this strategy, there are two long-lived branches:

 * `master`
 * `develop`

The `master` branch contains the code that has been published out as a NuGet package. The `develop` branch contains the code currently under development and ready for a future release. New code is developed under the `develop` branch, and only the Release-to-NuGet workflow will update `master` with the contents of `develop`. The `master` branch is never directly accessed.

If you would like to modify the code and present a pull request, please do the following:

1. Create a feature/fix branch off of the `develop` branch.
2. Perform your work on the feature/fix branch. The name of the branch doesn't matter, as long as it is branched off of the `develop` branch.
3. Present the feature/fix branch for consideration as a pull request.
4. When the pull request is accepted, then the work will be pulled into the `develop` branch.

When a release is imminent, the project administrators will merge the `develop` branch into the `master` branch, and the code in the `master` branch will be labeled and readied for release as a NuGet package.

# Unit Tests
Pull requests should include unit tests. Pull requests that address a logged issue should include at least one unit test that execrcises the issue.

The solution includes a project called `JeffFerguson.Gepsio.Test`. This project includes a subdirectory called `IssueTests`. Unit tests that exercise an issue should be placed within this subdirectory. Each issue should have their own subdirectory beneath the `IssueTests` subdirectory, and the subdirectory should be named with the issue number being exercised by the unit test.

For example, the solution contains a unit test to exercise Issue 1. The `JeffFerguson.Gepsio.Test` project includes a subfolder called `\IssueTests\1`, and the subdirectory includes a unit test to exercise Issue 1.

# Thank You
Thank you once again for your willingness to make a contribution to the project!