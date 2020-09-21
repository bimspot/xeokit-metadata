# Contribution Guide

## Code Style

Our C# style guidelines are the different from that of the industry
standard. We write C# code in a Swift-y way. Key deviations to 
Microsoft's style:

* no prefix for any identifiers (class, interface, type, etc.)
* properties and methods are `lowerCamelCase`
* braces are always at the end of the lines
* 2 spaces indent
* hard wrap at 80

The `bimspot.DotSettings` is available to import into Rider
(Preferences -> Manage Layers ->Import) and should be placed
in all solutions. The `cmd` + `opt` + `L` applies the style to the
current file by reformatting it.

## Version Control

Gitflow is used as a branching strategy. In short:

* `develop` is used for the ongoing development, always must be shippable.
* `feature` branches are created, when bigger changes are required
* `release` branches are used to test and roll-out new versions of the app
* `hotfix` is used for patching the production version
* `master` branch is always the current production version

* For example: feature/BIMSPOT-20-overall-done-state

## Git Commits

Git commit messages should follow the following rules:
* Be descriptive
* The commit message should be in the imperative form
* Start the message with a capital letter and do not end it with dot

## Documentation

All code pushed to develop shall be properly documented. Technology
specific tools generate documentation automatically (CD) for each
components. In addition, every service shall have a README.md file
in its root folder to explain the high-level concept and usage of the 
service.


## Further Conventions

We prefer implementing functionality in extension methods instead of large
utility classes.
