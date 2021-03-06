#How to Download All Your SSRS Report Definitions (RDL files) Using PowerShell
#Written by belle Posted March 28, 2011 at 9:44 pm
##Here’s a short PowerShell script that :
#1. Connects to your report server
#2. Creates the same folder structure you have in your Report Server
#3. Download all the SSRS Report Definition (RDL) files into their respective folders
#In addition to backing up your Source Project, your ReportServer database, or good old RSScripter (see http://sqlserver-indo.org/blogs/mca/archive/2009/03/08/extract-and-transfer-rdl-files-from-ssrs.aspx) this is just another way you can “backup” or archive your reports.
#note this is tested on PowerShell v2 and SSRS 2008 R2
[void][System.Reflection.Assembly]::LoadWithPartialName("System.Xml.XmlDocument");
[void][System.Reflection.Assembly]::LoadWithPartialName("System.IO");
 
$ReportServerUri = "http://machete/Reportserver/ReportService2005.asmx";
$Proxy = New-WebServiceProxy -Uri $ReportServerUri -Namespace SSRS.ReportingService2005 -UseDefaultCredential ;
 
#check out all members of $Proxy
#$Proxy | Get-Member
#http://msdn.microsoft.com/en-us/library/aa225878(v=SQL.80).aspx
 
#second parameter means recursive
$items = $Proxy.ListChildren("/", $true) | `
         select Type, Path, ID, Name | `
         Where-Object {$_.type -eq "Report"};
 
#create a new folder where we will save the files
#PowerShell datetime format codes http://technet.microsoft.com/en-us/library/ee692801.aspx
 
#create a timestamped folder, format similar to 2011-Mar-28-0850PM
$folderName = Get-Date -format "yyyy-MMM-dd-hhmmtt";
$fullFolderName = "C:\Temp\" + $folderName;
[System.IO.Directory]::CreateDirectory($fullFolderName) | out-null
 
foreach($item in $items)
{
    #need to figure out if it has a folder name
    $subfolderName = split-path $item.Path;
    $reportName = split-path $item.Path -Leaf;
    $fullSubfolderName = $fullFolderName + $subfolderName;
    if(-not(Test-Path $fullSubfolderName))
    {
        #note this will create the full folder hierarchy
        [System.IO.Directory]::CreateDirectory($fullSubfolderName) | out-null
    }
 
    $rdlFile = New-Object System.Xml.XmlDocument;
    [byte[]] $reportDefinition = $null;
    $reportDefinition = $Proxy.GetReportDefinition($item.Path);
 
    #note here we're forcing the actual definition to be 
    #stored as a byte array
    #if you take out the @() from the MemoryStream constructor, you'll 
    #get an error
    [System.IO.MemoryStream] $memStream = New-Object System.IO.MemoryStream(@(,$reportDefinition));
    $rdlFile.Load($memStream);
 
    $fullReportFileName = $fullSubfolderName + "\" + $item.Name +  ".rdl";
    #Write-Host $fullReportFileName;
    $rdlFile.Save( $fullReportFileName);
 
}
