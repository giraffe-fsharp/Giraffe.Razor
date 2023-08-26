namespace Giraffe.Razor

module RazorEngine =

    open System
    open System.IO
    open System.Collections.Generic
    open Microsoft.AspNetCore.Http
    open Microsoft.AspNetCore.Mvc
    open Microsoft.AspNetCore.Mvc.Abstractions
    open Microsoft.AspNetCore.Mvc.ModelBinding
    open Microsoft.AspNetCore.Mvc.Razor
    open Microsoft.AspNetCore.Mvc.Rendering
    open Microsoft.AspNetCore.Mvc.ViewFeatures
    open Microsoft.AspNetCore.Routing

    let private extractRouteData (path : string) =
        // Normalize nulls
        let templatePath = path + ""

        // Split path into segments and reverse the orders
        let segments =
            templatePath.Split('/', '\\')
            |> List.ofSeq
            |> List.rev

        let routeValues =
            seq {
                for i in 1..segments.Length do
                    match i with
                    | 1 -> yield "action", segments.[0]
                    | 2 -> yield "controller", segments.[1]
                    | 3 -> yield "area", segments.[2]
                    | x -> yield sprintf "token-%d" (x), segments.[x - 1]
            }

        // Create RouteData Object using Values Created
        let routeData = RouteData()

        for (key,value) in routeValues do
            routeData.Values.Add(key, value)

        routeData

    let renderView (razorViewEngine       : IRazorViewEngine)
                   (modelMetadataProvider : IModelMetadataProvider)
                   (tempDataDict          : ITempDataDictionary)
                   (httpContext           : HttpContext)
                   (viewName              : string)
                   (model                 : 'T option)
                   (viewData              : IDictionary<string, obj> option)
                   (modelState            : ModelStateDictionary option) =
        task {
            let routeData = extractRouteData(viewName)
            let templateName = routeData.Values.["action"].ToString()

            let actionContext    = ActionContext(httpContext, routeData, ActionDescriptor())
            let viewEngineResult = razorViewEngine.FindView(actionContext, templateName, true)

            match viewEngineResult.Success with
            | false ->
                let locations = String.Join(" ", viewEngineResult.SearchedLocations)
                return Error (sprintf "Could not find view with the name '%s'. Looked in %s." templateName locations)
            | true ->
                let view      = viewEngineResult.View
                let viewModel = defaultArg model Unchecked.defaultof<'T>
                let viewModelState = defaultArg modelState (ModelStateDictionary())
                let viewDataDict =
                    ViewDataDictionary<'T>(
                        modelMetadataProvider,
                        viewModelState,
                        Model = viewModel)
                if (viewData.IsSome) then
                    viewData.Value
                    |> Seq.iter (fun x -> viewDataDict.Add x)
                let htmlHelperOptions  = HtmlHelperOptions()
                use output = new StringWriter()
                let viewContext = ViewContext(actionContext, view, viewDataDict, tempDataDict, output, htmlHelperOptions)
                do! view.RenderAsync(viewContext)
                tempDataDict.Save()
                return Ok (output.ToString())
        }
