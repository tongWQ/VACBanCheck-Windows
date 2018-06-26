import urllib,urllib.request,sys,time
from urllib.request import Request
import json
import os
#https://partner.steamgames.com/doc/webapi/ISteamUser GetPlayerBans


def IsVACBanned(receivedJSON):
    try:
        strAll = json.loads(receivedJSON)
        VACBanned = strAll["players"][0]["VACBanned"]
        return VACBanned
    except Exception as e:
        print(e)
        return None


SteamUserAPIKey="DCCF6B72328C01D1EE708DA272F01327"
#获取自己的apikey https://steamcommunity.com/dev/apikey

apiurl="https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/"
steamids = input("输入疑似挂逼的steam ID\n")
#示例挂逼id 76561198118220136
url = apiurl +"?key="+SteamUserAPIKey +"&steamids="+steamids+","

print(url)
request = Request(url)
response = urllib.request.urlopen(url)
content=response.read()
if(content):
    print(content)
    vacBanned = IsVACBanned(content)
    if(vacBanned):
        print("这确实是一个被VAC封号的孤儿挂逼")
    else:
        print("此ID并未被VAC封号")


os.system("pause")