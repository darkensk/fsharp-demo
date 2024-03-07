module Gir.Domain


type ShippingParameters =
    { Height: int
      Length: int
      Width: int
      Weight: int
      Attributes: string list }

type Product =
    { ProductId: int
      Name: string
      Price: decimal
      Img: string
      BigImg: string
      ShippingParameters: ShippingParameters }

type CheckoutItem =
    { Name: string
      Price: decimal
      Quantity: int
      ShippingParameters: ShippingParameters option }

type CartEvent =
    | Add of productId: int * quantity: int
    | Remove of productId: int
    | RemoveAll of productId: int
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

type Extras =
    { ExtraTermsAndConditions: string option }

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: bool
      IncludePaymentFeeInTotalPrice: bool
      ShippingOptionChangedCallbackEnabled: bool
      PaymentMethodChangedCallbackEnabled: bool
      ModeChangedCallbackEnabled: bool
      HideAvardaLogo: bool
      Extras: Extras }

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

type PaymentWidgetSettings = { Enabled: bool }

type AdditionalFeatures = { PartnerShippingEnabled: bool }

type AprWidgetSettings = { Enabled: bool }

type SharedWidgetSettings = { CustomStyles: bool }

type ShippingSettings =
    { IncludeShippingParameters: bool
      IncludeDefaultShippingItem: bool }

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings
      Market: Market
      OrderReference: string
      PaymentWidgetSettings: PaymentWidgetSettings
      AdditionalFeatures: AdditionalFeatures
      AprWidgetSettings: AprWidgetSettings
      SharedWidgetSettings: SharedWidgetSettings
      ShippingSettings: ShippingSettings }

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

let defaultExtraCheckoutFlags: ExtraCheckoutFlags =
    { DisableFocus = false
      BeforeSubmitCallbackEnabled = false
      DeliveryAddressChangedCallbackEnabled = false
      CustomStyles = false
      IncludePaymentFeeInTotalPrice = false
      ShippingOptionChangedCallbackEnabled = false
      PaymentMethodChangedCallbackEnabled = false
      ModeChangedCallbackEnabled = false
      HideAvardaLogo = false
      Extras = { ExtraTermsAndConditions = None } }


let defaultExtraInitSettings: ExtraInitSettings =
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

let defaultPaymentWidgetSettings: PaymentWidgetSettings = { Enabled = false }

let defaultAprWidgetSettings: AprWidgetSettings = { Enabled = false }

let defaultAdditionalFeatures: AdditionalFeatures =
    { PartnerShippingEnabled = false }

let defaultSharedWidgetSettings: SharedWidgetSettings = { CustomStyles = false }

let defaultShippingSettings: ShippingSettings =
    { IncludeShippingParameters = false
      IncludeDefaultShippingItem = false }

let defaultSettings: Settings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings
      Market = Sweden
      OrderReference = "TEST-AVARDA-DEMO-SHOP"
      PaymentWidgetSettings = defaultPaymentWidgetSettings
      AdditionalFeatures = defaultAdditionalFeatures
      AprWidgetSettings = defaultAprWidgetSettings
      SharedWidgetSettings = defaultSharedWidgetSettings
      ShippingSettings = defaultShippingSettings }

type ExtraIdentifiers = { OrderReference: string }

type B2BStep =
    | B2BInitialized
    | B2BEnterCompanyInfo
    | B2BCompanyAddressInfo
    | B2BWaitingForSwish
    | B2BRedirectedToDirectPaymentBank
    | B2BRedirectedToNets
    | B2BWaitingForBankId
    | B2BRedirectedToTupas
    | B2BCompleted
    | B2BHandledByMerchant
    | B2BTimedOut
    | B2BCanceled
    | B2BRedirectedToNetsEident
    | B2BRedirectedToVipps
    | B2BUnknownStep

let stringToB2BStep =
    function
    | "Initialized" -> B2BInitialized
    | "EnterCompanyInfo" -> B2BEnterCompanyInfo
    | "CompanyAddressInfo" -> B2BCompanyAddressInfo
    | "WaitingForSwish" -> B2BWaitingForSwish
    | "RedirectedToDirectPaymentBank" -> B2BRedirectedToDirectPaymentBank
    | "RedirectedToNets" -> B2BRedirectedToNets
    | "WaitingForBankId" -> B2BWaitingForBankId
    | "RedirectedToTupas" -> B2BRedirectedToTupas
    | "Completed" -> B2BCompleted
    | "HandledByMerchant" -> B2BHandledByMerchant
    | "TimedOut" -> B2BTimedOut
    | "Canceled" -> B2BCanceled
    | "RedirectedToNetsEident" -> B2BRedirectedToNetsEident
    | "RedirectedToVipps" -> B2BRedirectedToVipps
    | _ -> B2BUnknownStep

type B2CStep =
    | B2CInitialized
    | B2CEmailZipEntry
    | B2CSsnEntry
    | B2CPhoneNumberEntry
    | B2CPhoneNumberEntryForKnownCustomer
    | B2CPersonalInfoWithoutSsn
    | B2CPersonalInfo
    | B2CWaitingForSwish
    | B2CRedirectedToDirectPaymentBank
    | B2CRedirectedToNets
    | B2CWaitingForBankId
    | B2CRedirectedToTupas
    | B2CCompleted
    | B2CTimedOut
    | B2CHandledByMerchant
    | B2CAwaitingCreditApproval
    | B2CRedirectedToNetsEident
    | B2CCanceled
    | B2CRedirectedToVipps
    | B2CUnknownStep

let stringToB2CStep =
    function
    | "Initialized" -> B2CInitialized
    | "EmailZipEntry" -> B2CEmailZipEntry
    | "SsnEntry" -> B2CSsnEntry
    | "PhoneNumberEntry" -> B2CPhoneNumberEntry
    | "PhoneNumberEntryForKnownCustomer" -> B2CPhoneNumberEntryForKnownCustomer
    | "PersonalInfoWithoutSsn" -> B2CPersonalInfoWithoutSsn
    | "PersonalInfo" -> B2CPersonalInfo
    | "WaitingForSwish" -> B2CWaitingForSwish
    | "RedirectedToDirectPaymentBank" -> B2CRedirectedToDirectPaymentBank
    | "RedirectedToNets" -> B2CRedirectedToNets
    | "WaitingForBankId" -> B2CWaitingForBankId
    | "RedirectedToTupas" -> B2CRedirectedToTupas
    | "Completed" -> B2CCompleted
    | "TimedOut" -> B2CTimedOut
    | "HandledByMerchant" -> B2CHandledByMerchant
    | "AwaitingCreditApproval" -> B2CAwaitingCreditApproval
    | "RedirectedToNetsEident" -> B2CRedirectedToNetsEident
    | "Canceled" -> B2CCanceled
    | "RedirectedToVipps" -> B2CRedirectedToVipps
    | _ -> B2CUnknownStep

type CurrentB2BStep = { Current: B2BStep }

type CurrentB2CStep = { Current: B2CStep }

type B2BData = { Step: CurrentB2BStep }

type B2CData = { Step: CurrentB2CStep }


type PaymentStatus =
    { PurchaseId: string
      ExtraIdentifiers: ExtraIdentifiers
      Mode: string
      B2B: B2BData option
      B2C: B2CData option }

type PurchaseIdentifiers =
    { PurchaseToken: string
      PurchaseId: string }
