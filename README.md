<<<<<<< HEAD
# VPet.Plugin.Demo

简体中文 | [English](./README_en.md) | [Spanish](./README_es.md)

由开发者制作的桌宠插件案例
* DemoClock: 给桌宠添加一个时钟显示的功能
* EdgeTTS: 桌宠说话的时候附带语音,使用EdgeTTS

## VPet.Plugin.DemoClock
给桌宠添加一个时钟显示的功能,这算是代码嵌入类型MOD的DEMO

![democlock](democlock.png)

参考本软件即可编写自己的代码MOD

### 功能

给桌宠添加一个可以显示时间的钟表

* 支持倒计时
* 支持正计时
* 支持番茄钟 (工作/休息)

每次使用番茄钟完成工作,还可以获得相应时间的 [金钱] 奖励

### 相关截图

菜单栏设置

![image-20230411135459116](README.assets/image-20230411135459116.png)

鼠标移近时突出显示状态

![image-20230411134850372](README.assets/image-20230411134850372.png)

待机时状态

![image-20230411134857271](README.assets/image-20230411134857271.png)

倒计时状态

![image-20230411135128676](README.assets/image-20230411135128676.png)

番茄钟:工作状态

![image-20230411134933108](README.assets/image-20230411134933108.png)

时间到

![image-20230411135345438](README.assets/image-20230411135345438.png)

游戏设置窗口

![image-20230411134959369](README.assets/image-20230411134959369.png)

## VPet.Plugin.EdgeTTS

桌宠说话的时候附带语音,使用EdgeTTS

![edgetts](edgetts.png)

参考本软件即可编写自己的代码MOD

### 功能

让桌宠说话的时候说出来

* 支持自定义讲述人和语音语调语速等

### 相关截图

游戏设置窗口

![image-20230411134959369](README.assets/image-20230411134959369.png)
=======
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
>>>>>>> 588b901da349e898aa89b6eb3d94aa6eed17e6ac
