# VPet.Plugin.ChatGPTPlus
A more advanced version of the ChatGPT Demo plugin for vpet from https://github.com/LorisYounger/VPet.Plugin.Demo/blob/main/README_en.md

## To run the windows version (will publish to steam later)  :
(you can create empty folders `VPet.Plugin.Demo` and `VPet.Plugin.ChatGPTPlus`)
1. put `6525_ChatGPTPlus` into `your_folder\VPet.Plugin.Demo\VPet.Plugin.ChatGPTPlus` 
2. Open admin cmd into `your_folder\VPet\VPet-Simulator.Windows`
3. Run `mklink /d "mod\6525_ChatGPTPlus" "..\..\..\VPet.Plugin.Demo\VPet.Plugin.ChatGPTPlus\6525_ChatGPTPlus"`

## Setup
Add this to your initialisation text :
```
Your answers depends on your status
Depending on our conversation, send me updates for your status in {brackets}. Add to your reply for exemples : {food+40}, {drink-10}, {feeling+20}, {starmina-40}, {health+30}
Start an action when you see fit by adding to your reply {go:action}
```
You can access this text in settings>Chat Settings>Open ChatGPT Settings
![image](https://github.com/jere344/VPet.Plugin.ChatGPTPlus/assets/86294972/9974c122-e4f4-4a9d-9b14-377799b89db9)
