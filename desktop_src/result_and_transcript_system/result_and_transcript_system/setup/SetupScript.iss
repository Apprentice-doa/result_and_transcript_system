; -- ResultandTrascriptProcessingSystem.iss --
; Setup script
;my custom perl code/directive
[Code]
#define MyAppName "RTPS Result Software"
#define MyAppVersion "1.0.Beta"
#define MyAppPublisher "Olaye, E."
#define MyAppURL "https://www.software.keminkreative.site.com/"
#define MyAppExeName "RTPS Result Soft Setup.exe"
#define MyDir "RTPS Result Soft"

[Setup]
PrivilegesRequired=admin
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={commonpf}\{#MyDir}
DefaultGroupName={#MyDir}
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2
SolidCompression=yes
OutputDir="RTPS Result Soft Setup\"

AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}


;optional to run as admin
;[Run]
;Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,MyApp}"; Flags: runascurrentuser nowait postinstall skipifsilent; Check: returnTrue()



[InstallDelete]
Type: files; Name: {app}\{#MyAppExeName}

[Files]
;main program
Source: "..\bin\debug\result_and_transcript_system.exe"; DestDir: "{app}"
Source: "..\bin\debug\result_and_transcript_system.exe.config"; DestDir: "{app}"
;Database
Source: "..\bin\debug\db\db.mdb"; DestDir: "{app}\db"

;dlls
Source: "..\bin\debug\ExcelDataReader.DataSet.dll"; DestDir: "{app}"
Source: "..\bin\debug\ExcelDataReader.dll"; DestDir: "{app}"
Source: "..\bin\debug\Excel.Helper.dll"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.dll"; DestDir: "{app}"
Source: "..\bin\debug\Excel.Helper.dll"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.dll"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.OOXML.dll"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.OpenXml4Net.dll"; DestDir: "{app}"

Source: "..\bin\debug\NPOI.OpenXmlFormats.dll"; DestDir: "{app}"
Source: "..\bin\debug\MySql.Data.dll"; DestDir: "{app}"

Source: "..\bin\debug\stdole.dll"; DestDir: "{app}"
  Source: "..\bin\debug\Microsoft.VisualStudio.TextManager.Interop.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.VisualStudio.TextManager.Interop.8.0.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.VisualStudio.Shell.Interop.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.VisualStudio.Shell.Interop.8.0.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.VisualStudio.OLE.Interop.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.MSXML.dll"; DestDir: "{app}"
Source: "..\bin\debug\EnvDTE.dll"; DestDir: "{app}"

Source: "..\bin\debug\Microsoft.ReportViewer.ProcessingObjectModel.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.ReportViewer.DataVisualization.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.ReportViewer.Common.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.ReportViewer.Design.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.ReportViewer.WinForms.dll"; DestDir: "{app}"
Source: "..\bin\debug\Microsoft.SqlServer.Types.dll"; DestDir: "{app}"

;pdb and xml todo
Source: "..\bin\debug\ExcelDataReader.DataSet.pdb"; DestDir: "{app}"
Source: "..\bin\debug\ExcelDataReader.pdb"; DestDir: "{app}"

Source: "..\bin\debug\ExcelDataReader.DataSet.xml"; DestDir: "{app}"
Source: "..\bin\debug\Excel.Helper.xml"; DestDir: "{app}"
Source: "..\bin\debug\ExcelDataReader.xml"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.OOXML.xml"; DestDir: "{app}"
Source: "..\bin\debug\NPOI.xml"; DestDir: "{app}"

 ;help files
Source: "..\bin\debug\help\guide.html"; DestDir: "{app}\help\guide.html"
Source: "..\bin\debug\help\js\bootstrap.min.js"; DestDir: "{app}\help\js\bootstrap.min.js"


Source: "..\bin\debug\help\css\bootstrap.min.css"; DestDir: "{app}\help\css\bootstrap.min.css"
Source: "..\bin\debug\help\css\bootstrap.css"; DestDir: "{app}\help\css\bootstrap.css"
Source: "..\bin\debug\help\css\style.css"; DestDir: "{app}\help\css\style.css"
Source: "..\bin\debug\help\css\sticky-menu.css"; DestDir: "{app}\help\css\sticky-menu.css"


Source: "..\bin\debug\help\images\banner.jpg"; DestDir: "{app}\help\images\banner.jpg"
Source: "..\bin\debug\help\images\banner2.jpg"; DestDir: "{app}\help\images\banner2.jpg"
Source: "..\bin\debug\help\images\banner3.jpg"; DestDir: "{app}\help\images\banner3.jpg"
Source: "..\bin\debug\help\images\header.svg"; DestDir: "{app}\help\images\header.svg"
Source: "..\bin\debug\help\images\feature.svg"; DestDir: "{app}\help\images\feature.svg"
Source: "..\bin\debug\help\images\content.svg"; DestDir: "{app}\help\images\content.svg"
Source: "..\bin\debug\help\images\troubleshoot_updateable_query.jpg"; DestDir: "{app}\help\images\troubleshoot_updateable_query.jpg"
;samples
Source: "..\bin\debug\samples\sample_broadsheet.xlsm"; DestDir: "{app}\samples"
Source: "..\bin\debug\samples\Sample_result.xlsx"; DestDir: "{app}\samples"
;Source: "..\bin\debug\samples\sample_GPA_DMI.xlsx"; DestDir: "{userdocs}\{#Mydir}\samples"
Source: "..\bin\debug\samples\Students_CCT_1_2018_2019.xlsx"; DestDir: "{userdocs}\{#Mydir}\samples"
Source: "..\bin\debug\samples\Sample_registration_extracted_from_ACCESS_2018_2019.xlsx"; DestDir: "{userdocs}\{#Mydir}\samples"





;templates
Source: "..\bin\debug\templates\result.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\broadsheet.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\broadsheet.xlsm"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\broadsheet_plain.xlsx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\broadsheet - Copy3.xlsm"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\gpa.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\senate.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
 Source: "..\bin\debug\templates\courses.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\students.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"
Source: "..\bin\debug\templates\registration.xltx"; DestDir: "{userdocs}\{#Mydir}\templates"

;images
Source: "..\bin\debug\photos\ENG1503585.jpg"; DestDir: "{userdocs}\{#Mydir}\photos"
Source: "..\bin\debug\photos\photo_female.jpg"; DestDir: "{userdocs}\{#Mydir}\photos"
Source: "..\bin\debug\scans\result_2018_2019_ecp281.jpg"; DestDir: "{userdocs}\{#Mydir}\scans"


;Save templates in prog path as backup
;templates
Source: "..\bin\debug\templates\result.xltx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\broadsheet.xltx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\broadsheet.xlsm"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\broadsheet_plain.xlsx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\broadsheet - Copy3.xlsm"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\gpa.xltx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\senate.xltx"; DestDir: "{app}\templates"
 Source: "..\bin\debug\templates\courses.xltx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\students.xltx"; DestDir: "{app}\templates"
Source: "..\bin\debug\templates\registration.xltx"; DestDir: "{app}\templates"

;save copy in prog path
Source: "..\bin\debug\samples\sample_broadsheet.xlsm"; DestDir: "{app}\samples"
;Source: "..\bin\debug\samples\sample_result.xltx"; DestDir: "{app}\samples"
;broadsheets
Source: "..\bin\debug\broadsheets\broadsheet_saved.xlsm"; DestDir: "{userdocs}\{#Mydir}\broadsheets"
Source: "..\bin\debug\results\sample_result.xlsx"; DestDir: "{userdocs}\{#Mydir}\results"

;{usercf}
;The path to the current user's Common Files directory. 
;Only Windows 7 and later supports {usercf}; if used on previous Windows versions, 
;it will translate to the same directory as {localappdata}\Programs\Common.

;---------------------------------------------------
;lets put some stuff here so that they are writable
;{userdocs} & {commondocs}
;The path to the My Documents folder.

;{userappdata} & {commonappdata}
;The path to the Application Data folder.

;{localappdata}
;The path to the local (nonroaming) Application Data folder.
;

;Test thumbnails
;Source: "..\bin\debug\images\thumbnails\hd-01.jpg"; DestDir: "{userdocs}\images\thumbnails"


[Icons]
Name: "{group}\ResultandTrascriptProcessingSystem"; Filename: "{app}\result_and_transcript_system.exe"
