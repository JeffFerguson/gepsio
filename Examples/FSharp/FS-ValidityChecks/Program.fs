// Learn more about F# at http://fsharp.org

open System
open JeffFerguson.Gepsio

let CheckValidity (xbrlDoc : XbrlDocument) =
    if xbrlDoc.IsValid then
        Console.WriteLine "Congratulations! This document is valid according to the XBRL specification."
    else
        Console.WriteLine "This document is invalid according to the XBRL specification."
        for validationError in xbrlDoc.ValidationErrors do
            Console.WriteLine validationError.Message
    |> ignore

let LoadValidInstance =
    let xbrlDoc = new XbrlDocument()
    xbrlDoc.Load @"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml"
    CheckValidity xbrlDoc
    |> ignore

let LoadInvalidInstance =
    let xbrlDoc = new XbrlDocument()
    xbrlDoc.Load @"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-10-FootnoteFromOutOfScope.xml"
    CheckValidity xbrlDoc
    |> ignore

[<EntryPoint>]
let main argv =
    LoadValidInstance
    LoadInvalidInstance
    0 // return an integer exit code
