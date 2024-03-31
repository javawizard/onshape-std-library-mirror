# onshape-std-library-mirror

This is an automatically updated mirror of the [Onshape FeatureScript standard
library](https://cad.onshape.com/documents/12312312345abcabcabcdeff). (You'll need an Onshape account to access that
document; you can [sign up for one for free here](https://www.onshape.com/en/sign-up).)

It includes version history going back to when the standard library was first released [in
2014](https://github.com/javawizard/onshape-std-library-mirror/commit/b3fb8f9f415bce86817d65f98ec90e6ef543a543).

**There are three branches:**

- [main](https://github.com/javawizard/onshape-std-library-mirror/tree/main): This is the standard library as-is,
  without modifications
- [without-versions](https://github.com/javawizard/onshape-std-library-mirror/tree/without-versions): Version numbers
  have been removed; this cuts down significantly on diff noise since version numbers in imports are bumped throughout
  the standard library each release
- [readme](https://github.com/javawizard/onshape-std-library-mirror/tree/readme): The branch containing the
  authoritative copy of this README. It's automatically merged into the other two branches whenever the importer
  notices that it's changed.

## Automatic updates

[onshape-std-library-importer](https://github.com/javawizard/onshape-std-library-importer) keeps this repo up to date
automatically. It checks for new standard library releases at the top of the hour and pushes them to the `main` and
`without-versions` branches. It also checks for updates to the `readme` branch and merges them into `main` and
`without-versions`.

## Feedback

Any changes you'd like to see? Additional branches with transformations applied? Want to see
`onshape-std-library-importer` generalized to support arbitrary Onshape documents? Feel free to open an issue and let
me know!

## Is this legal?

Yep! Onshape's standard library is licensed under the MIT license which permits copying (and modifying) provided the
license text is included [with the
copy](https://github.com/javawizard/onshape-std-library-mirror/blob/main/LICENSE.txt).

## Disclaimer

The usual applies: `onshape-std-library-mirror` and `onshape-std-library-importer` are projects by
[@javawizard](https://github.com/javawizard), **not by Onshape.** All trademarks are owned by their respective owners.
