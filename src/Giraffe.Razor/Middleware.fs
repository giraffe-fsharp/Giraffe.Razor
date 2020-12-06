namespace Giraffe.Razor

[<AutoOpen>]
module Middleware =

    open Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.FileProviders

    type IServiceCollection with

        member this.AddRazorEngine(viewsFolderPath: string, persistFileProviders: bool) =
            this.Configure<MvcRazorRuntimeCompilationOptions>(fun (options: MvcRazorRuntimeCompilationOptions) ->
                if not persistFileProviders then
                    options.FileProviders.Clear()
                else
                    ()
                options.FileProviders.Add(new PhysicalFileProvider(viewsFolderPath))).AddMvc().AddRazorRuntimeCompilation()
            |> ignore
            this.AddAntiforgery()

        member this.AddRazorEngine(viewsFolderPath: string) =
            this.AddRazorEngine(viewsFolderPath, false)

        member this.AddRazorEngine() =
            this.AddMvc().AddRazorRuntimeCompilation() |> ignore
            this.AddAntiforgery()
