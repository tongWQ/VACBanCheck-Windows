# VACBanCheck
输入疑似挂逼个人steam主页（非自定义链接）查询封禁记录
## 原理说明
利用Steam Web API https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/ 查询该链接的steam账号持有者是否有被记录在案的steam封禁（包括但不限于VAC、游戏开发者封禁、社区封禁和交易封禁）记录

## 系统要求
.NET Framework 3.5（Windows 7 及更新版的系统无需另外安装）
NewtonSoft JSON.NET（用于解析收到的json文件）
