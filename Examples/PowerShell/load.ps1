Add-Type -Path '..\..\JeffFerguson.Gepsio\bin\Debug\netstandard2.0\JeffFerguson.Gepsio.dll'

function LoadFromLocalFile()
{
    $ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path
    $xbrlDoc = new-object "JeffFerguson.Gepsio.XbrlDocument"
    $xbrlDoc.Load($ScriptDir + '..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml')
}

function LoadFromUrl()
{
    $xbrlDoc = new-object "JeffFerguson.Gepsio.XbrlDocument"
    $xbrlDoc.Load('http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml')
}

LoadFromLocalFile
LoadFromUrl