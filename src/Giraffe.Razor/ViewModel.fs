namespace Giraffe.Razor

[<AutoOpen>]
module Types =

  type ViewData = (string * obj) seq

  type ViewModel<'T> = 
      | Model of 'T
      | ViewDataAndModel of ViewData * 'T