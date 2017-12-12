Release Notes
=============

## 0.1.0-beta-300

Upgraded to latest `Giraffe 0.1.0-beta-5**`, which has a new dependency on `Giraffe.Tasks` from NuGet.

#### New features

- Added `validateAntiforgeryToken` http handler for verifying anti forgery tokens.

## 0.1.0-beta-200

#### Breaking changes

- Renamed the `Giraffe.Razor.Engine` module to `RazorEngine` and moved it under the namespace `Giraffe.Razor`. If you were using the `renderRazorView` function to render individual razor views then you have to change the function call to `RazorEngine.renderView` now.

(Most Giraffe users will probably not bet affected if they were using the `razorView` or `razorHtmlView` http handlers.)

## 0.1.0-beta-110 and before

Previous releases of this library were documented in [Giraffe's release notes](https://github.com/giraffe-fsharp/Giraffe/blob/master/RELEASE_NOTES.md).