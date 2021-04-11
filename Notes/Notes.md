# Dom Tool Notes

## Files

Game:
G:\Games\steamapps\common\Dominions5

Tool and data:
C:\Users\toryb\Documents\Domininions Tool

Saved games:
C:\Users\toryb\AppData\Roaming\Dominions5\savedgames\

Map to use:
C:\Users\toryb\AppData\Roaming\Dominions5\maps\Arena


## Docs

https://illwinter.com/dom5/mapmanual.html
https://illwinter.com/dom5/techmanual.html
https://illwinter.com/dom5/modmanual.html
https://illwinter.com/dom5/eventmanual.html


## 2h files

late_phlegra_4.2h
						  v Dom version
0102 0444 4f4d 63b9 0100 2802 0000 0000
						 v Nation id
0000 0000 0000 ffff ffff 6600 0000 0000
0000 0000 0000 212a 3823 203d 2b3c 4f1e
b22b c016 7800 0000 0002 3100 0000 0000
00bd 0100 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 00ff ffff
ff9d 006e 00ff ffff ff00 007c 0200 0000
				v Pretender monster id
0000 0000 0000 009d 0000 0000 0000 0000
0000 00be 0766 00ff ffff ff00 0000 0000
v Name 
S p  o o  k y    M  o n  s t  e r
1c3f 2020 2436 6f02 2021 3c3b 2a3d 4f1c
0900 00ff ffff ffff ffff ffff ffff ffff
ffff ffff ffff ff00 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 00ff ffff ffff ffff ffff
ff00 0000 0000 0000 0500 0000 0000 0303
0300 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0231 00fd fdfd
0000 4f00 0000 0000 0000 0000 0000 0000
0000 0000 0000 001c 3f20 2024 366f 0220
213c 3b2a 3d4f 4f4f ac0b 7b00 0000 4d37


### Name

0e A
0d B
0c C
0b D
0a E
09 F
08 G
07 H
06 I
05 J
04 K
03 L
02 M
01 N
00 O
1f P
1e Q
1d R
1c S
1b T
1a U
19 V
18 W
17 X
16 Y
15 Z
4f ?
d6 ?
06 ?



## Plan

- Open Arena map into Unity scene (hardcode first, late load province positions etc.)
- Parse monsters.txt and sprites (ignore everything but name and ID at start)
- Typing letters start search, updating with suggestions along the way.
- Implemented adding/removing commanders, as well as:
	- Units, with number of units (should be easily adjustable)
	- Magic powers
	- Items (Requires parsing items.txt)
- Implement choosing nations
- Export
	- (NiceToHave) Specify game-name, warning if savedgames already contain game-name
	- Create/change Arena.map file
	- Create 2h files for selected nations (HOW??)
	- Create folder in savedgames, move 2h files into it 
	- REMEMBER: Rename 2h files, and remove "_##"
- Run game with Command Line Options
	- Dominions5.exe --newgame Arenaye --mapfile Arena.map --era 3 && Dominions5.exe Arenaye 