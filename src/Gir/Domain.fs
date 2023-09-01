module Gir.Domain

type Product =
    { ProductId: int
      Name: string
      Price: decimal
      Img: string }

type CartEvent =
    | Add of productId: int
    | Remove of productId: int
    | Clear

type CartItem =
    { Id: int
      Qty: int
      ProductDetail: Product }

type CartState = { Items: CartItem list }

type InitializePaymentResponse = { PurchaseId: string; Jwt: string }

type PaymentWidgetState =
    { PaymentId: string; WidgetJwt: string }

type Language =
    | English
    | Swedish
    | Finnish
    | Norwegian
    | Estonian
    | Danish
    | Slovak
    | Czech
    | Polish
    | Latvian
    | German
    | Austrian

type CheckoutMode =
    | B2C
    | B2B

type CheckboxState =
    | Hidden
    | Checked
    | Unchecked

type PaymentMethod =
    | Loan
    | Invoice
    | Card
    | Direct
    | PayPal
    | Swish
    | PartPayment
    | PayOnDelivery

type SelectedPaymentMethod =
    | Selected of selectedPaymentMethod: PaymentMethod
    | NotSelected

type BackendNotificationState =
    | NotSet
    | ShouldSucceed
    | ShouldFail

type AgeValidation =
    | Disabled
    | Enabled of ageValidationLimit: int

type ExtraInitSettings =
    { Language: Language
      Mode: CheckoutMode
      DifferentDeliveryAddress: CheckboxState
      SelectedPaymentMethod: SelectedPaymentMethod
      DisplayItems: bool
      RecurringPayments: CheckboxState
      SmsNewsletterSubscription: CheckboxState
      EmailNewsletterSubscription: CheckboxState
      BackendNotification: BackendNotificationState
      EnableB2BLink: bool
      EnableCountrySelector: bool
      ShowThankYouPage: bool
      AgeValidation: AgeValidation
      EmailInvoice: CheckboxState
      UseCustomTermsAndConditionsUrl: bool
      UseCustomIntegrityConditionsUrl: bool
      HideUnsupportedRecurringPaymentMethods: bool
      UseCustomSmsNewsletterSubscriptionText: bool
      UseCustomEmailNewsletterSubscriptionText: bool
      SkipEmailZipEntry: bool }

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: bool
      IncludePaymentFeeInTotalPrice: bool
      ShippingOptionChangedCallbackEnabled: bool
      PaymentMethodChangedCallbackEnabled: bool
      ModeChangedCallbackEnabled: bool
      HideAvardaLogo: bool }

type Market =
    | Sweden
    | Finland
    | Norway
    | Denmark
    | Slovakia
    | Czechia
    | Poland
    | Latvia
    | Estonia
    | International
    | Germany
    | Austria

type PaymentWidgetSettings = { Enabled: bool; CustomStyles: bool }

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings
      Market: Market
      OrderReference: string
      PaymentWidgetSettings: PaymentWidgetSettings }

let languageToString =
    function
    | English -> "English"
    | Swedish -> "Swedish"
    | Finnish -> "Finnish"
    | Norwegian -> "Norwegian"
    | Estonian -> "Estonian"
    | Danish -> "Danish"
    | Slovak -> "Slovak"
    | Czech -> "Czech"
    | Polish -> "Polish"
    | Latvian -> "Latvian"
    | German -> "German"
    | Austrian -> "Austrian"

let languageToIsoCode =
    function
    | English -> "en"
    | Swedish -> "sv"
    | Finnish -> "fi"
    | Norwegian -> "nb"
    | Estonian -> "et"
    | Danish -> "da"
    | Slovak -> "sk"
    | Czech -> "cs"
    | Polish -> "pl"
    | Latvian -> "lv"
    | German -> "de"
    | Austrian -> "de"


let stringToLanguage =
    function
    | "English" -> English
    | "Swedish" -> Swedish
    | "Finnish" -> Finnish
    | "Norwegian" -> Norwegian
    | "Estonian" -> Estonian
    | "Danish" -> Danish
    | "Slovak" -> Slovak
    | "Czech" -> Czech
    | "Polish" -> Polish
    | "Latvian" -> Latvian
    | "German" -> German
    | "Austrian" -> Austrian
    | _ -> English

let checkoutModeToString =
    function
    | B2C -> "b2c"
    | B2B -> "b2b"

let stringToCheckoutMode =
    function
    | "b2c" -> B2C
    | "b2b" -> B2B
    | _ -> B2C

let checkboxStateToString =
    function
    | Hidden -> "Hidden"
    | Checked -> "Checked"
    | Unchecked -> "Unchecked"

let backendNotificationStateToString =
    function
    | NotSet -> "NotSet"
    | ShouldSucceed -> "ShouldSucceed"
    | ShouldFail -> "ShouldFail"

let stringToCheckboxState =
    function
    | "Hidden" -> Hidden
    | "Checked" -> Checked
    | "Unchecked" -> Unchecked
    | _ -> Hidden

let stringToBackendNotificationState =
    function
    | "NotSet" -> NotSet
    | "ShouldSucceed" -> ShouldSucceed
    | "ShouldFail" -> ShouldFail
    | _ -> NotSet

let paymentMethodsToString =
    function
    | Loan -> "Loan"
    | Invoice -> "Invoice"
    | Card -> "Card"
    | Direct -> "Direct"
    | PayPal -> "PayPal"
    | Swish -> "Swish"
    | PartPayment -> "PartPayment"
    | PayOnDelivery -> "PayOnDelivery"

let selectedPaymentMethodToString =
    function
    | Selected selectedPaymentMethod -> paymentMethodsToString selectedPaymentMethod
    | NotSelected -> ""

let stringToSelectedPaymentMethod =
    function
    | "Loan" -> Selected Loan
    | "Invoice" -> Selected Invoice
    | "Card" -> Selected Card
    | "Direct" -> Selected Direct
    | "PayPal" -> Selected PayPal
    | "Swish" -> Selected Swish
    | "PartPayment" -> Selected PartPayment
    | "PayOnDelivery" -> Selected PayOnDelivery
    | _ -> NotSelected

let marketToString =
    function
    | Sweden -> "Sweden"
    | Finland -> "Finland"
    | Norway -> "Norway"
    | Denmark -> "Denmark"
    | Slovakia -> "Slovakia"
    | Czechia -> "Czechia"
    | Poland -> "Poland"
    | Latvia -> "Latvia"
    | Estonia -> "Estonia"
    | International -> "International"
    | Germany -> "Germany"
    | Austria -> "Austria"


let stringToMarket =
    function
    | "Sweden" -> Sweden
    | "Finland" -> Finland
    | "Norway" -> Norway
    | "Denmark" -> Denmark
    | "Slovakia" -> Slovakia
    | "Czechia" -> Czechia
    | "Poland" -> Poland
    | "Latvia" -> Latvia
    | "Estonia" -> Estonia
    | "International" -> International
    | "Germany" -> Germany
    | "Austria" -> Austria
    | _ -> Sweden

let stringToAgeValidation (limit: string) =
    let mutable intvalue = 0

    match limit with
    | "" -> Disabled
    | _ ->
        if System.Int32.TryParse(limit, &intvalue) then
            if intvalue <= 0 then Disabled else Enabled intvalue
        else
            Disabled

let ageValidationToString =
    function
    | Enabled limit -> limit |> string
    | Disabled -> ""

let defaultExtraCheckoutFlags =
    { DisableFocus = false
      BeforeSubmitCallbackEnabled = false
      DeliveryAddressChangedCallbackEnabled = false
      CustomStyles = false
      IncludePaymentFeeInTotalPrice = false
      ShippingOptionChangedCallbackEnabled = false
      PaymentMethodChangedCallbackEnabled = false
      ModeChangedCallbackEnabled = false
      HideAvardaLogo = false }


let defaultExtraInitSettings =
    { Language = English
      Mode = B2C
      DifferentDeliveryAddress = Hidden
      SelectedPaymentMethod = NotSelected
      DisplayItems = true
      RecurringPayments = Hidden
      SmsNewsletterSubscription = Hidden
      EmailNewsletterSubscription = Hidden
      BackendNotification = NotSet
      EnableB2BLink = false
      EnableCountrySelector = false
      ShowThankYouPage = true
      AgeValidation = Disabled
      EmailInvoice = Hidden
      UseCustomTermsAndConditionsUrl = true
      UseCustomIntegrityConditionsUrl = true
      HideUnsupportedRecurringPaymentMethods = false
      UseCustomSmsNewsletterSubscriptionText = false
      UseCustomEmailNewsletterSubscriptionText = false
      SkipEmailZipEntry = false }

let defaultPaymentWidgetSettings: PaymentWidgetSettings =
    { Enabled = false
      CustomStyles = false }

let defaultSettings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings
      Market = Sweden
      OrderReference = "TEST-AVARDA-DEMO-SHOP"
      PaymentWidgetSettings = defaultPaymentWidgetSettings }

type ExtraIdentifiers = { OrderReference: string }

type PaymentStatus =
    { PurchaseId: string
      ExtraIdentifiers: ExtraIdentifiers
      Mode: string }
