namespace Giraffe.Razor

[<AutoOpen>]
module Middleware =

#if NETCOREAPP3_1
    open Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
#else
    open Microsoft.AspNetCore.Mvc.Razor
#endif
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.FileProviders

    type IServiceCollection with

        member this.AddRazorEngine(viewsFolderPath: string) =
#if NETCOREAPP3_1
            this.Configure<MvcRazorRuntimeCompilationOptions>(fun (options: MvcRazorRuntimeCompilationOptions) ->
                options.FileProviders.Clear()
                options.FileProviders.Add(new PhysicalFileProvider(viewsFolderPath))).AddMvc().AddRazorRuntimeCompilation()
            |> ignore
#else
            this.Configure<RazorViewEngineOptions>(fun (options : RazorViewEngineOptions) ->
                    options.FileProviders.Clear()
                    options.FileProviders.Add(new PhysicalFileProvider(viewsFolderPath))).AddMvc()
            |> ignore
#endif
            this.AddAntiforgery()

        member this.AddRazorEngine() =
#if NETCOREAPP3_1
            this.AddMvc().AddRazorRuntimeCompilation() |> ignore
#else
            this.AddMvc() |> ignore
#endif
            this.AddAntiforgery()
