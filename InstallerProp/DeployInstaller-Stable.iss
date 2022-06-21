; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=Collapse Launcher
AppVersion=1.0.30.0
AppCopyright=2022 - neon-nyan
AppPublisher=neon-nyan
VersionInfoVersion=1.0.30.0
VersionInfoCompany=neon-nyan
VersionInfoDescription=Collapse Launcher - An advanced launcher for miHoYo's Games
VersionInfoCopyright=2022 - neon-nyan
VersionInfoProductName=Collapse Launcher
VersionInfoProductVersion=1.0.30.0
VersionInfoProductTextVersion=1.0.30.0
SolidCompression=True
Compression=lzma2/ultra64
InternalCompressLevel=ultra64
MinVersion=0,10.0.17763
DefaultDirName={autopf64}\Collapse Launcher\
DefaultGroupName=Collapse Launcher
UninstallDisplayName=Collapse Launcher
UninstallDisplayIcon={app}\CollapseLauncher.exe
WizardStyle=modern
WizardImageFile=..\InstallerProp\WizardBannerDesign.bmp
WizardSmallImageFile=..\InstallerProp\WizardBannerDesignSmall.bmp
DisableWelcomePage=False
ArchitecturesInstallIn64BitMode=x64
LicenseFile=..\LICENSE

[Icons]
Name: "{group}\Collapse Launcher\Collapse Launcher"; Filename: "{app}\CollapseLauncher.exe"; WorkingDir: "{app}"; IconFilename: "{app}\CollapseLauncher.exe"; IconIndex: 0
Name: "{userdesktop}\Collapse Launcher"; Filename: "{app}\CollapseLauncher.exe"; WorkingDir: "{app}"; IconFilename: "{app}\CollapseLauncher.exe"; IconIndex: 0

[Files]
Source: "..\..\CollapseLauncher-ReleaseRepo\stable\*"; DestDir: "{app}"; Flags: ignoreversion createallsubdirs recursesubdirs
