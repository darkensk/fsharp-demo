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

type ExtraInitSettings =
    { Language: Language
      Mode: CheckoutMode
      DifferentDeliveryAddress: CheckboxState
      SelectedPaymentMethod: SelectedPaymentMethod
      DisplayItems: bool
      RecurringPayments: CheckboxState
      SmsNewsletterSubscription: CheckboxState
      EmailNewsletterSubscription: CheckboxState }

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: bool }

type Market =
    | Sweden
    | Finland

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings
      Market: Market
      OrderReference: string }

let languageToString (language: Language) =
    match language with
    | English -> "English"
    | Swedish -> "Swedish"
    | Finnish -> "Finnish"
    | Norwegian -> "Norwegian"
    | Estonian -> "Estonian"
    | Danish -> "Danish"

let stringToLanguage (s: string) =
    match s with
    | "English" -> English
    | "Swedish" -> Swedish
    | "Finnish" -> Finnish
    | "Norwegian" -> Norwegian
    | "Estonian" -> Estonian
    | "Danish" -> Danish
    | _ -> English

let checkoutModeToString (mode: CheckoutMode) =
    match mode with
    | B2C -> "b2c"
    | B2B -> "b2b"

let stringToCheckoutMode (s: string) =
    match s with
    | "b2c" -> B2C
    | "b2b" -> B2B
    | _ -> B2C

let checkboxStateToString (checkboxState: CheckboxState) =
    match checkboxState with
    | Hidden -> "Hidden"
    | Checked -> "Checked"
    | Unchecked -> "Unchecked"

let stringToCheckboxState (s: string) =
    match s with
    | "Hidden" -> Hidden
    | "Checked" -> Checked
    | "Unchecked" -> Unchecked
    | _ -> Hidden

let paymentMethodsToString (pm: PaymentMethod) =
    match pm with
    | Loan -> "Loan"
    | Invoice -> "Invoice"
    | Card -> "Card"
    | Direct -> "Direct"
    | PayPal -> "PayPal"
    | Swish -> "Swish"
    | PartPayment -> "PartPayment"
    | PayOnDelivery -> "PayOnDelivery"

let selectedPaymentMethodToString (spm: SelectedPaymentMethod) =
    match spm with
    | Selected selectedPaymentMethod -> paymentMethodsToString selectedPaymentMethod
    | NotSelected -> ""

let stringToSelectedPaymentMethod (s: string) =
    match s with
    | "Loan" -> Selected Loan
    | "Invoice" -> Selected Invoice
    | "Card" -> Selected Card
    | "Direct" -> Selected Direct
    | "PayPal" -> Selected PayPal
    | "Swish" -> Selected Swish
    | "PartPayment" -> Selected PartPayment
    | "PayOnDelivery" -> Selected PayOnDelivery
    | _ -> NotSelected

let marketToString (m: Market) =
    match m with
    | Sweden -> "Sweden"
    | Finland -> "Finland"

let stringToMarket (s: string) =
    match s with
    | "Sweden" -> Sweden
    | "Finland" -> Finland
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
      EmailNewsletterSubscription = Hidden }

let defaultSettings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings
      Market = Sweden
      OrderReference = "TEST-AVARDA-DEMO-SHOP" }
