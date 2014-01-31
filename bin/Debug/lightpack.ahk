#SingleInstance force

;Automatic Profile switching
;If VLC is the active window, it will switch to the VLC profile, if not it's switches back to "Lightpack" profile, which is my default 16:9 profile.
;2 or more app can use the same Profile -> if WinActive("VLC") or WinActive("GOM Player")   -> in this case, GOM player
;To get the correct window title, you need Autoit3 Windows spy, which is installed along with Autohotkey.
/*SetTitleMatchMode, 2
#Persistent
SetTimer, Timer, 1000 ;checks to current window every 1 sec (1000 miliseconds)
return
timer:
	if WinActive("VLC") or WinActive("GOM Player")
		Run, Lightpack_cmd.exe "Profile" "VLC", %A_ScriptDir%, Hide
	Else
		Run, Lightpack_cmd.exe "Profile" "Lightpack", %A_ScriptDir%, Hide
return
*/

;Hotkeys: http://www.autohotkey.com/docs/Hotkeys.htm
;Examples:
;^!F9 -> Control+Alt+F9
;#F1 -> Windows key + F1
;!+a -> Alt + Shift + a

+!F9::  ;This hotkey toggles ON and OFF the device (Shift + Alt + F9)
	toggle := !toggle
	if toggle
		Run, Lightpack_cmd.exe "ON", %A_ScriptDir%, Hide
	else
		Run, Lightpack_cmd.exe "OFF", %A_ScriptDir%, Hide
return


+!F10:: ;Device ON (Shift + Alt + F10)
	Run, Lightpack_cmd.exe "ON", %A_ScriptDir%, Hide
return


+!F11:: ;Device OFF (Shift + Alt + F11)
	Run, Lightpack_cmd.exe "OFF", %A_ScriptDir%, Hide
return


;Dedicated hotkeys for profiles

+!F5:: ;Default Profile (Shift + Alt + F5)
	Run, Lightpack_cmd.exe "Profile" "Lightpack", %A_ScriptDir%, Hide
	splashGUi("Default", "Blue") ; If you want visual feedback for profile switching
return


+!F6:: ;VLC profile (Shift + Alt + F6)
	Run, Lightpack_cmd.exe "Profile" "VLC", %A_ScriptDir%, Hide
	splashGUi("VLC", "Red") ; If you want visual feedback for profile switching
return


splashGUi(text,color) {
  CustomColor = EEAA99  ; Can be any RGB color (it will be made transparent below).
  Gui +LastFound +AlwaysOnTop -Caption +ToolWindow  ; +ToolWindow avoids a taskbar button and an alt-tab menu item.
  Gui, Font, s50  ; Set a large font size (32-point).
  Gui, Add, Text, c%Color%, %text%
  Gui, Color, 808080
  WinSet, TransColor, 808080
  Gui, Show, xCenter yCenter NoActivate  ; NoActivate avoids deactivating the currently active window.
  Sleep, 1000
  Gui, Destroy
}
