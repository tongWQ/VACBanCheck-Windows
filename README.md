# VACBanCheck
输入疑似挂逼个人steam主页（非自定义链接）查询封禁记录
## 原理说明
利用Steam Web API https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/ 查询该链接的steam账号持有者是否有被记录在案的steam封禁（包括但不限于VAC、游戏开发者封禁、社区封禁和交易封禁）记录
<br>使用Visual Studio 2015开发
<br>
## 系统要求
.NET Framework 3.5（Windows 7 及更新版的系统无需另外安装）<br>
NewtonSoft JSON.NET（用于解析收到的json文件）
<br>
## 直接运行
运行VACBanCheck/BanCheckWindows/bin/Debug 或Release 目录下的BanCheckWindows.exe<br>
注意：Newtonsoft.Json.dll必须与exe在同一目录
