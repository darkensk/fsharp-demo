module Gir.Domain


type Product =
    { ProductId: int
      Name: string
      Price: float
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

let languageToString language =
    match language with
    | English -> "English"
    | Swedish -> "Swedish"
    | Finnish -> "Finnish"
    | Norwegian -> "Norwegian"
    | Estonian -> "Estonian"
    | Danish -> "Danish"

let stringToLanguage s =
    match s with
    | "English" -> English
    | "Swedish" -> Swedish
    | "Finnish" -> Finnish
    | "Norwegian" -> Norwegian
    | "Estonian" -> Estonian
    | "Danish" -> Danish
    | _ -> English

type CheckoutMode =
    | B2C
    | B2B

let checkoutModeToString mode =
    match mode with
    | B2C -> "b2c"
    | B2B -> "b2b"

let stringToCheckoutMode s =
    match s with
    | "b2c" -> B2C
    | "b2b" -> B2B
    | _ -> B2C

type CheckboxState =
    | Hidden
    | Checked
    | Unchecked

let checkboxStateToString checkboxState =
    match checkboxState with
    | Hidden -> "Hidden"
    | Checked -> "Checked"
    | Unchecked -> "Unchecked"

let stringToCheckboxState s =
    match s with
    | "Hidden" -> Hidden
    | "Checked" -> Checked
    | "Unchecked" -> Unchecked
    | _ -> Hidden

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

let paymentMethodsToString pm =
    match pm with
    | Loan -> "Loan"
    | Invoice -> "Invoice"
    | Card -> "Card"
    | Direct -> "Direct"
    | PayPal -> "PayPal"
    | Swish -> "Swish"
    | PartPayment -> "PartPayment"
    | PayOnDelivery -> "PayOnDelivery"

let selectedPaymentMethodToString selectedPaymentMethod =
    match selectedPaymentMethod with
    | Selected selectedPaymentMethod -> paymentMethodsToString selectedPaymentMethod
    | NotSelected -> ""


let stringToSelectedPaymentMethod s =
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


type ExtraInitSettings =
    { Language: Language
      Mode: CheckoutMode
      DifferentDeliveryAddress: CheckboxState
      SelectedPaymentMethod: SelectedPaymentMethod
      DisplayItems: bool
      RecurringPayments: CheckboxState
      SmsNewsletterSubscription: CheckboxState
      EmailNewsletterSubscription: CheckboxState
      EmailInvoice: CheckboxState }

type CustomStyles =
    | Set of customStyles: string
    | NotSet

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: CustomStyles }

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings }

let defaultExtraCheckoutFlags =
    { DisableFocus = true
      BeforeSubmitCallbackEnabled = false
      DeliveryAddressChangedCallbackEnabled = false
      CustomStyles = NotSet }

let defaultExtraInitSettings =
    { Language = English
      Mode = B2C
      DifferentDeliveryAddress = Hidden
      SelectedPaymentMethod = NotSelected
      DisplayItems = true
      RecurringPayments = Hidden
      SmsNewsletterSubscription = Hidden
      EmailNewsletterSubscription = Hidden
      EmailInvoice = Hidden }

let defaultSettings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings }
