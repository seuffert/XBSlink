;NSIS Modern User Interface
;Welcome/Finish Page Example Script
;Written by Joost Verburg

;--------------------------------
  !include "MUI2.nsh"
  !include "FileFunc.nsh"
  !include "VersionCompare.nsh"
  !system "xbslink_version_info.exe"
  !include "XBSlink_version.txt"
;--------------------------------
;General

  ;Name and file
  Name "XBSlink"
  OutFile "XBSlink_setup_${Version}.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\XBSlink"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\XBSlink" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

  SetCompressor /SOLID lzma
  
  Icon "..\XBSlink\XBSlink.ico"
  
  Caption "XBSlink Setup - v${Version}"    
  
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "XBSlink_license.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH

  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Version Info

  VIProductVersion ${Version}
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "XBSlink"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "A Xbox360 System Link Proxy"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Oliver Seuffert"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright Oliver Seuffert"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "XBSlink"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" ${Version}

;--------------------------------
;Installer Sections

; The stuff to install
Section "XBSlink (required)" XBSlink

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File /r "..\XBSlink\bin\Release\*"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\XBSlink "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\XBSlink" "DisplayName" "XBSlink"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\XBSlink" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\XBSlink" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\XBSlink" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

;--------------------------------

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts" StartMenuShortcuts

  CreateDirectory "$SMPROGRAMS\XBSlink"
  CreateShortCut "$SMPROGRAMS\XBSlink\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\XBSlink\XBSlink.lnk" "$INSTDIR\XBSlink.exe" "" "$INSTDIR\XBSlink.exe" 0
  
SectionEnd

  Var WINPCAP_NAME ; DisplayName from WinPcap installation
  Var WINPCAP_VERSION ; DisplayVersion from WinPcap installation

;--------------------------------
Section "WinPCap Capture Library" WinPCap
  SetOutPath $TEMP
  File "WinPcap_4_1_3.exe"
  ReadRegStr $WINPCAP_NAME HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\WinPcapInst" "DisplayName"
  IfErrors lbl_winpcap_do_install
  ReadRegStr $WINPCAP_VERSION HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\WinPcapInst" "DisplayVersion"
  StrCmp $WINPCAP_VERSION "" lbl_winpcap_do_install ; WinPcap is really old(?) or installed improperly.
	${VersionCompare} $WINPCAP_VERSION "4.1.0.2980" $1 ; WinPcap 4.1.3
	StrCmp $1 "1" lbl_winpcap_do_install
	goto lbl_winpcap_dont_install 
lbl_winpcap_do_install:    
  ExecWait '"$TEMP\WinPcap_4_1_3.exe"'
lbl_winpcap_dont_install:  
  Delete "$TEMP\WinPcap_4_1_3.exe"
SectionEnd
;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\XBSlink"
  DeleteRegKey HKLM SOFTWARE\XBSlink

  ;ADD YOUR OWN FILES HERE...  
  Delete "$INSTDIR\Uninstall.exe"
  Delete $INSTDIR\*

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\XBSlink\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\XBSlink"
  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\XBSlink"

SectionEnd


LangString DESC_XBSlink ${LANG_ENGLISH} "The core components of XBSlink."
LangString DESC_StartMenuShortcuts ${LANG_ENGLISH} "Shortcuts to XBSlink in the Startmenu."
LangString DESC_WinPCap ${LANG_ENGLISH} "The WinPCap Packet Capture Library.(see http://www.winpcap.org for more information)"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${XBSlink} $(DESC_XBSlink)
  !insertmacro MUI_DESCRIPTION_TEXT ${StartMenuShortcuts} $(DESC_StartMenuShortcuts)
  !insertmacro MUI_DESCRIPTION_TEXT ${WinPCap} $(DESC_WinPCap)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
