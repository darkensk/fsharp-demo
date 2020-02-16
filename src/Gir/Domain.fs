module Gir.Domain

type Product = {
    Id : int
    Name : string
    Price : float
    Img : string 
}

type CartEvent =
    | Add
    | Remove
    | Clear