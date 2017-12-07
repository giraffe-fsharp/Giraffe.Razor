Release Notes
=============

## 0.1.0-beta-200

#### Breaking changes

- Renamed the `Giraffe.Razor.Engine` module into `RazorEngine` under the namespace `Giraffe.Razor`. If you were manually rendering razor views then you'd have to change the function call `renderRazorView` into `RazorEngine.renderView` now. Most Giraffe users are probably not affected by this change if they used the `razorView` or `razorHtmlView` http handlers.

## 0.1.0-beta-110 and before

Previous releases were documented in the Giraffe [RELEASE_NOTES.md](https://github.com/giraffe-fsharp/Giraffe/blob/master/RELEASE_NOTES.md).