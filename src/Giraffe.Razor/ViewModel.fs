namespace Giraffe.Razor

open System.Collections.Generic
[<AutoOpen>]
module Types =

  type ViewData = IDictionary<string,obj>

  type ViewModel<'T>(?model: 'T, ?viewData: ViewData) = 
    let model = defaultArg model Unchecked.defaultof<'T>
    let viewData = defaultArg viewData (dict Seq.empty)
    member this.Model = model
    member this.ViewData = viewData