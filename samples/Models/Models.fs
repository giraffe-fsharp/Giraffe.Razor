namespace Models

// ---------------------------------
// Models
// ---------------------------------

[<CLIMutable>]
type Person =
    {
        Name : string
    }

[<CLIMutable>]
type CreatePerson =
    {
        Name    : string
        CheckMe : bool
    }