Release Notes
=============

## 1.3.0

Upgraded to Giraffe `2.0.0` and the latest ASP.NET Core `2.1.*` NuGet packages.

## 1.2.0

Added support for view folders (see [#1](https://github.com/giraffe-fsharp/Giraffe.Razor/issues/1), [#3](https://github.com/giraffe-fsharp/Giraffe.Razor/issues/3)).

## 1.1.0

Upgraded to Giraffe `1.1.0`.

## 1.0.1

Changed copyright notice in NuGet package.

## 1.0.0

Upgraded to latest Giraffe `1.0.0` release.

## 0.1.0-beta-310

Upgraded to latest Giraffe `0.1.0-beta-6**` release.

## 0.1.0-beta-300

Upgraded to latest Giraffe `0.1.0-beta-5**`, which has a new dependency on `Giraffe.Tasks` from NuGet.

#### New features

- Added `validateAntiforgeryToken` http handler for verifying anti forgery tokens.

## 0.1.0-beta-200

#### Breaking changes

- Renamed the `Giraffe.Razor.Engine` module to `RazorEngine` and moved it under the namespace `Giraffe.Razor`. If you were using the `renderRazorView` function to render individual razor views then you have to change the function call to `RazorEngine.renderView` now.

(Most Giraffe users will probably not bet affected if they were using the `razorView` or `razorHtmlView` http handlers.)

## 0.1.0-beta-110 and before

Previous releases of this library were documented in [Giraffe's release notes](https://github.com/giraffe-fsharp/Giraffe/blob/master/RELEASE_NOTES.md).