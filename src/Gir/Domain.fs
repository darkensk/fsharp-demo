module Gir.Domain

type Product = {
    Id : int
    Name : string
    Price : float
    Img : string 
}

type CartEvent =
    | Add of productId : int
    | Remove of productId : int
    | Clear