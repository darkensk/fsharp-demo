module Gir.App

open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authentication.Cookies
open System
open System.IO
open CompositionRoot


let redirectHandler next (ctx: HttpContext) =
    let refererUrl = ctx.Request.GetTypedHeaders().Referer.ToString()

    redirectTo false refererUrl next ctx

let webApp (root: CompositionRoot) =
    choose
        [ GET
          >=> choose
              [ route "/cart/"
                >=> Cart.HttpHandlers.cartHandler
                    root.CheckoutFrontendBundle
                    root.GetPurchaseToken
                    root.GetAllProducts
                    root.GetPartnerAccessToken
                    root.ReclaimPurchaseToken
                    root.PartnerShippingBundle
                route "/cart/clear"
                >=> Cart.HttpHandlers.clearCartHandler root.GetAllProducts
                >=> redirectTo false "/cart/"
                route "/cart/completed"
                >=> Cart.HttpHandlers.completedHandler root.CheckoutBackendApiUrl root.GetPartnerAccessToken
                >=> text "OK - CompletedCallback Successfull"
                route "/cart/sessionExpired"
                >=> Cart.HttpHandlers.sessionExpiredHandler
                    root.CheckoutBackendApiUrl
                    root.ApiPublicUrl
                    root.GetPartnerAccessToken
                >=> text "OK - Session Timed Out Callback Successfull"
                route "/pay-frame"
                >=> PayFrame.HttpHandlers.validationHandler root.PayFrameBundle
                >=> PayFrame.HttpHandlers.payFrameHandler root.PayFrameBundle root.PayFrameSiteKey root.PayFrameLanguage
                route "/settings/"
                >=> Settings.HttpHandlers.settingsHandler
                    root.PaymentWidgetBundle
                    root.EnabledMarkets
                    root.PartnerShippingBundle
                subRoute
                    "/product"
                    (choose
                        [ subRoutef
                              "/%i"
                              (Products.HttpHandlers.detailHandler
                                  root.GetPaymentWidgetToken
                                  root.GetPartnerAccessToken
                                  root.PaymentWidgetBundle
                                  root.GetProductById
                                  root.ApiPublicUrl) ])
                route "/" >=> Products.HttpHandlers.listHandler root.GetAllProducts
                subRoute
                    "/test"
                    (choose
                        [ subRoutef "/%s" (fun (purchaseToken: string) ->
                              Test.HttpHandlers.testCheckoutHandler
                                  root.CheckoutFrontendBundle
                                  purchaseToken
                                  root.PartnerShippingBundle) ]) ]
          POST
          >=> choose
              [ routef "/product/%i/add" (fun (productId: int) ->
                    Cart.HttpHandlers.addToCartValidationHandler root.CheckoutBackendApiUrl root.GetPartnerAccessToken
                    >=> Cart.HttpHandlers.addToCartHandler productId root.GetAllProducts
                    >=> Cart.HttpHandlers.updateItemsHandler
                        root.CheckoutBackendApiUrl
                        root.ApiPublicUrl
                        root.GetPartnerAccessToken
                    >=> redirectHandler)
                routef "/product/%i/remove" (fun (productId: int) ->
                    Cart.HttpHandlers.removeFromCartValidationHandler
                        root.CheckoutBackendApiUrl
                        root.GetPartnerAccessToken
                    >=> Cart.HttpHandlers.removeFromCartHandler productId root.GetAllProducts
                    >=> Cart.HttpHandlers.updateItemsHandler
                        root.CheckoutBackendApiUrl
                        root.ApiPublicUrl
                        root.GetPartnerAccessToken
                    >=> redirectHandler)
                routef "/product/%i/removeAll" (fun (productId: int) ->
                    Cart.HttpHandlers.removeAllFromCartHandler productId root.GetAllProducts
                    >=> Cart.HttpHandlers.updateItemsHandler
                        root.CheckoutBackendApiUrl
                        root.ApiPublicUrl
                        root.GetPartnerAccessToken
                    >=> redirectHandler)
                route "/test/" >=> Test.HttpHandlers.easterEggHandler
                route "/settings/save"
                >=> Settings.HttpHandlers.saveSettingsHandler
                >=> redirectTo false "/settings/"
                route "/be2be/fail"
                >=> setStatusCode 500
                >=> text "ERROR - Backend notification failed"
                route "/be2be/succeed" >=> text "OK - Backend notification received" ]
          setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader()
    |> ignore

    builder.WithOrigins("https://localhost:5001").AllowAnyMethod().AllowAnyHeader()
    |> ignore

let configureApp (root: CompositionRoot) (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseSession()
        .UseGiraffe(webApp root)

let cookieOptions =
    (fun (options: CookieAuthenticationOptions) ->
        options.Cookie.SameSite <- SameSiteMode.Strict
        options.Cookie.SecurePolicy <- CookieSecurePolicy.Always)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddAuthentication() |> ignore
    services.AddDataProtection() |> ignore
    services.AddSession() |> ignore
    services.AddMvc() |> ignore

    services.ConfigureApplicationCookie(Action<_> cookieOptions) |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder
        .AddFilter(fun (logLevel: LogLevel) -> logLevel.Equals LogLevel.Error)
        .AddConsole()
        .AddDebug()
    |> ignore

[<EntryPoint>]
let main _ =
    let cfg = (ConfigurationBuilder()).AddEnvironmentVariables().Build()

    let root = CompositionRoot.compose cfg

    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")

    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder>(configureApp root))
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()

    0
