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

type CartState =
    { Items: CartItem list }

type InitializePaymentResponse =
    { PurchaseId: string
      Jwt: string }
