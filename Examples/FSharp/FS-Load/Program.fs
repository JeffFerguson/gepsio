open JeffFerguson.Gepsio

let LoadFromLocalFile =
    let xbrlDoc = new XbrlDocument()
    xbrlDoc.Load @"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml"

let LoadFromUrl =
    let xbrlDoc = new XbrlDocument()
    xbrlDoc.Load "http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml"

[<EntryPoint>]
let main argv =
    LoadFromLocalFile
    LoadFromUrl
    0 // return an integer exit code