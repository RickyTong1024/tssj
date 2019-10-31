#coding=utf-8

import sys 
sys.path.append("../common")
import config
import pull
import thrpool
from rpc_pb2 import *
import http.client
import json
import struct
import opcodes
import time

def pvp_push(tin):
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_PUSH"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def pvp_pull(tin):
    mg = tmsg_rep_pvp_match()
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_PULL"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def fight(tin):
    mg = tmsg_rep_pvp_fight()
    mg.text = b""
    mg.result = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_FIGHT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def reward(tin):
    mg = tmsg_rep_pvp_reward()
    rk = tmsg_rep_pvp_match()
    mg.ranks.CopyFrom(rk)
    mg.res = -1;
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_REWARD"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def team_enter(tin):
    mg = tmsg_rep_team_login()
    mg.res = -1
    mg.chenghao = 0
    mg.point = 0
    mg.duanwei = 0
    mg.dspoint = 0
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_TEAM_ENTER"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def team_invite(tin):
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_TEAM_INVITE"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def bingyuan_fight(tin):
    mg = tmsg_rep_bingyuan_fight()
    mg.bingjing = 0
    mg.point = 0
    mg.chenghao = 0
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_BINGYUAN_FIGHT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()
    pull.ioloop.instance().send_rep_msg(tin.req.id, info)
    
def bingyuan_buy(tin):
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_BINGYUAN_BUY"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def bingyuan_reward(tin):
    mg = tmsg_rep_pvp_reward()
    rk = tmsg_rep_pvp_match()
    mg.ranks.CopyFrom(rk)
    mg.res = -1;
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_BINGYUAN_REWARD"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def ds_fight(tin):
    mg = tmsg_rep_ds_fight()
    mg.xinpian = 0
    mg.point = 0
    mg.ciliao = 0
    mg.duanwei = 0
    mg.grank = -1
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_DS_FIGHT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def mofang_point(tin):
    mg = tmsg_rep_mofang_point()
    mg.point = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_MOFANG_POINT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)


def invite_code_create(tin):
    mg = tmsg_rep_invite_code()
    mg.res = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_INVITE_CODE_CREATE"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def invite_code_input(tin):
    mg = tmsg_rep_invite_code()
    mg.res = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_INVITE_CODE_INPUT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def invite_code_level(tin):
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_INVITE_CODE_LEVEL"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def invite_code_pull(tin):
    mg = tmsg_rep_invite_level()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_INVITE_CODE_PULL"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def ds_time_buy(tin):
    try:
        httpClient = http.client.HTTPConnection(config.team_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_DS_TIME_BUY"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def guild_fight_look_bushu(tin):
    mg = tmsg_rep_guild_look_bushu()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_LOOK_BUSHU"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_look_pipei(tin):
    mg = tmsg_rep_guild_fight_info()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_LOOK_PIPEI"), tin.req.msg, headers)
        response = httpClient.getresponse()
        print("111")
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        print(error_code)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_bushu(tin):
    mg = tmsg_rep_guild_pvp_bushu()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_BUSHU"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_look_jinrizhanji(tin):
    mg = tmsg_rep_guild_jinrizhanji()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_LOOK_JINRIZHANJI"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_fight(tin):
    mg = tmsg_rep_guild_fight()
    mg.res = -1
    mg.text = b""
    mg.result = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_FIGHT"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        print("123")
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        print(error_code)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_match(tin):
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_MATCH"), tin.req.msg, headers)
        response = httpClient.getresponse()
        print("456")
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        print(error_code)
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

def guild_fight_baoming(tin):
    mg = tmsg_rep_guild_pvp_baoming()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_BAOMING"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        print("789")
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        print(error_code)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_reward(tin):
    mg = tmsg_rep_guild_fight_reward()
    mg.stype = -1
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_REWARD"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        print("222")
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        print(error_code)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_target(tin):
    mg = tmsg_rep_guild_target_reward()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_TARGET"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def guild_fight_look_xiuzhan(tin):
    mg = tmsg_rep_guild_jinrizhanji()
    mg.res = False
    info = mg.SerializeToString()
    try:
        httpClient = http.client.HTTPConnection(config.pvp_host, timeout = 10)
        headers = {'Content-type':'text/xml;charset=UTF-8'}
        httpClient.request("POST", opcodes.op_url("TMSG_PVP_GUILD_LOOK_XIUZHAN"), tin.req.msg, headers)
        response = httpClient.getresponse()
        text = response.read()
        fmt = "i%ds" % (len(text) - 4)
        error_code, info = struct.unpack(fmt, text)
        if error_code != 0:
            info = mg.SerializeToString()
    except Exception as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()

    pull.ioloop.instance().send_rep_msg(tin.req.id, info)

def main():
    pull.ioloop.instance().start(config.myname, config.push_addr, config.pull_addr)
    thrpool.ThreadPool.instance().start(5)
    while 1:
        tin = pull.ioloop.instance().read_msg()
        for i in range(len(tin)):
            stin = tin[i]
            if stin.req.opcode == opcodes.op("PMSG_PVP_PUSH"):
                thrpool.ThreadPool.instance().add_task(pvp_push, stin)
            elif stin.req.opcode == opcodes.op("PMSG_PVP_PULL"):
                thrpool.ThreadPool.instance().add_task(pvp_pull, stin)
            elif stin.req.opcode == opcodes.op("PMSG_PVP_FIGHT"):
                thrpool.ThreadPool.instance().add_task(fight, stin)
            elif stin.req.opcode == opcodes.op("PMSG_PVP_REWARD"):
                thrpool.ThreadPool.instance().add_task(reward, stin)
            elif stin.req.opcode == opcodes.op("PMSG_TEAM_ENTER"):
                thrpool.ThreadPool.instance().add_task(team_enter, stin)
            elif stin.req.opcode == opcodes.op("PMSG_BINGYUAN_FIGHT"):
                thrpool.ThreadPool.instance().add_task(bingyuan_fight, stin)
            elif stin.req.opcode == opcodes.op("PMSG_SOCIAL_TEAM_INVITE"):
                thrpool.ThreadPool.instance().add_task(team_invite, stin)
            elif stin.req.opcode == opcodes.op("PMSG_BINGYUAN_BUY"):
                thrpool.ThreadPool.instance().add_task(bingyuan_buy, stin)
            elif stin.req.opcode == opcodes.op("PMSG_BINGYUAN_REWARD"):
                thrpool.ThreadPool.instance().add_task(bingyuan_reward, stin)
            elif stin.req.opcode == opcodes.op("PMSG_DS_FIGHT"):
                thrpool.ThreadPool.instance().add_task(ds_fight, stin)
            elif stin.req.opcode == opcodes.op("PMSG_MOFANG_POINT"):
                thrpool.ThreadPool.instance().add_task(mofang_point, stin)
            elif stin.req.opcode == opcodes.op("PMSG_SOCIAL_INVITE_CODE_CREATE"):
                thrpool.ThreadPool.instance().add_task(invite_code_create, stin)
            elif stin.req.opcode == opcodes.op("PMSG_SOCIAL_INVITE_CODE_INPUT"):
                thrpool.ThreadPool.instance().add_task(invite_code_input, stin)
            elif stin.req.opcode == opcodes.op("PMSG_SOCIAL_INVITE_CODE_LEVEL"):
                thrpool.ThreadPool.instance().add_task(invite_code_level, stin)
            elif stin.req.opcode == opcodes.op("PMSG_SOCIAL_INVITE_CODE_PULL"):
                thrpool.ThreadPool.instance().add_task(invite_code_pull, stin)
            elif stin.req.opcode == opcodes.op("PMSG_DS_TIME_BUY"):
                thrpool.ThreadPool.instance().add_task(ds_time_buy, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_LOOK_BUSHU"):
                thrpool.ThreadPool.instance().add_task(guild_fight_look_bushu, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_LOOK_PIPEI"):
                thrpool.ThreadPool.instance().add_task(guild_fight_look_pipei, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_BUSHU"):
                thrpool.ThreadPool.instance().add_task(guild_fight_bushu, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_LOOK_ZHANJI"):
                thrpool.ThreadPool.instance().add_task(guild_fight_look_jinrizhanji, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_FIGHT"):
                thrpool.ThreadPool.instance().add_task(guild_fight_fight, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_BUY"):
                thrpool.ThreadPool.instance().add_task(guild_fight_target, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_REWARD"):
                thrpool.ThreadPool.instance().add_task(guild_fight_reward, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_MATCH"):
                thrpool.ThreadPool.instance().add_task(guild_fight_match, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_BAOMING"):
                thrpool.ThreadPool.instance().add_task(guild_fight_baoming, stin)
            elif stin.req.opcode == opcodes.op("PMSG_GUILDFIGHT_LOOK_XIUZHAN"):
                thrpool.ThreadPool.instance().add_task(guild_fight_look_xiuzhan, stin)
        time.sleep(0.1)
    thrpool.ThreadPool.instance().wait()
            
if __name__ == '__main__':
    main()


