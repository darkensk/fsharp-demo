module Gir.App

open CompositionRoot
open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open System
open System.IO


let redirectHandler next (ctx:HttpContext) =
    let reffererUrl = ctx.Request.GetTypedHeaders().Referer.ToString()
    redirectTo false reffererUrl next ctx

let webApp (root:CompositionRoot) =
    choose [
        GET >=>
            choose [
                route "/cart/" >=> Cart.HttpHandlers.cartHandler root.CheckoutFrontendBundle root.GetPurchaseToken root.GetAllProducts
                route "/cart/clear" >=> Cart.HttpHandlers.clearCartHandler root.GetAllProducts >=> redirectTo false "/cart/"
                route "/cart/tbd" >=> Cart.HttpHandlers.reclaimHandler root.CheckoutBackendApiUrl root.CheckoutFrontendBundle root.GetPartnerAccessToken
                route "/cart/completed" >=> Cart.HttpHandlers.completedHandler >=> text "OK - CompletedCallback Successfull"
                subRoute "/product" (
                    choose [

                        subRoutef "/%i" (Products.HttpHandlers.detailHandler root.GetProductById)
                    ]
                )

                route "/" >=> Products.HttpHandlers.listHandler root.GetAllProducts
            ]
        POST >=>
            choose [
                routef "/product/%i/add" (fun i -> Cart.HttpHandlers.addToCartHandler i root.GetAllProducts >=> Cart.HttpHandlers.updateItemsHandler root.CheckoutBackendApiUrl root.GetPartnerAccessToken >=> redirectHandler )
                routef "/product/%i/remove" (fun i -> Cart.HttpHandlers.removeFromCartHandler i root.GetAllProducts >=> Cart.HttpHandlers.updateItemsHandler root.CheckoutBackendApiUrl root.GetPartnerAccessToken >=> redirectHandler )
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:5000")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore
    builder.WithOrigins("https://localhost:5001")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp root (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        //.UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseSession()
        .UseGiraffe(webApp root)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore
    services.AddAuthentication() |> ignore
    services.AddDataProtection() |> ignore
    services.AddSession() |> ignore
    services.AddMvc() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let cfg = 
        (ConfigurationBuilder())
            .AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables()
            .Build()

    let root = CompositionRoot.compose cfg

    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> (configureApp root))
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
