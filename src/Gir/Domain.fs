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
      ShowThankYouPage: bool }

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: bool }

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

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings
      Market: Market
      OrderReference: string }

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
    | _ -> Sweden

let defaultExtraCheckoutFlags =
    { DisableFocus = false
      BeforeSubmitCallbackEnabled = false
      DeliveryAddressChangedCallbackEnabled = false
      CustomStyles = false }

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
      ShowThankYouPage = true }

let defaultSettings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings
      Market = Sweden
      OrderReference = "TEST-AVARDA-DEMO-SHOP" }

type ExtraIdentifiers = { OrderReference: string }

type PaymentStatus =
    { PurchaseId: string
      ExtraIdentifiers: ExtraIdentifiers
      Mode: string }
