# -*- coding: utf-8 -*-

import time
import datetime
# import random
import struct
import httplib
import torndb
import paramiko
import smtplib
import logging
import os
from email.mime.text import MIMEText
from email.header import Header
from oss import oss_api
# import xml.etree as ET
import lxml.etree as ET
import rpc_pb2
import models
# from models import *
import opcodes

logging.basicConfig()
logger = logging.getLogger(__name__)


def sync_group(group, start_time, end_time):
    msg = rpc_pb2.tmsg_activity_group()
    msg.name = group.name
    msg.isjieri = group.isjieri
    msg.item_name1 = group.item_name1
    msg.item_name2 = group.item_name2
    msg.item_des1 = group.item_des1
    msg.item_des2 = group.item_des2
    activitys = group.activitys.all()
    for act in activitys:
        activityID = models.ActivityID.objects.get(type=act.type)
        activityID.aid = activityID.aid + 1
        activityID.save()

        tmsg = msg.activitys.add()
        tmsg.id = activityID.aid
        tmsg.start = int(time.mktime(start_time.timetuple()) * 1000)
        tmsg.end = int(time.mktime(end_time.timetuple()) * 1000)
        tmsg.kaifu_start = act.kaifu_start
        tmsg.kaifu_end = act.kaifu_end
        tmsg.name = act.huodong_name
        tmsg.kaikai_start = act.kaikai_start
        tmsg.type = act.atype
        tmsg.subtype = act.subtype
        tmsg.show = tmsg.start + act.start_show_day * 24 * 60 * 60 * 1000
        if act.end_show_day == 0:
            tmsg.noshow = tmsg.end
        else:
            tmsg.noshow = tmsg.start + act.end_show_day * 24 * 60 * 60 * 1000

        entrys = act.entrys.order_by("condition").order_by("order")
        for entry in entrys:
            reward = tmsg.rewards.add()
            reward.id = entry.aid
            reward.condition = entry.condition
            reward.arg1 = entry.arg1
            reward.arg2 = entry.arg2
            reward.arg3 = entry.arg3
            reward.arg4 = entry.arg4
            reward.arg5 = entry.arg5
            reward.show_time = tmsg.start
            if tmsg.subtype == 8:
                reward.show_time = tmsg.start + entry.arg3 * 24 * 60 * 60 * 1000
            if entry.duihuan11 != 0:
                reward.arg6.append(entry.duihuan11)
                reward.arg7.append(entry.duihuan12)
                reward.arg8.append(entry.duihuan13)
            if entry.duihuan21 != 0:
                reward.arg6.append(entry.duihuan21)
                reward.arg7.append(entry.duihuan22)
                reward.arg8.append(entry.duihuan23)
            if entry.type11 != 0:
                reward.types.append(entry.type11)
                reward.value1s.append(entry.value12)
                reward.value2s.append(entry.value13)
                reward.value3s.append(entry.value14)
            if entry.type21 != 0:
                reward.types.append(entry.type21)
                reward.value1s.append(entry.value22)
                reward.value2s.append(entry.value23)
                reward.value3s.append(entry.value24)
            if entry.type31 != 0:
                reward.types.append(entry.type31)
                reward.value1s.append(entry.value32)
                reward.value2s.append(entry.value33)
                reward.value3s.append(entry.value34)
            if entry.type41 != 0:
                reward.types.append(entry.type41)
                reward.value1s.append(entry.value42)
                reward.value2s.append(entry.value43)
                reward.value3s.append(entry.value44)
    s = msg.SerializeToString()
    sync_servers = ""
    all_servers = models.Server.objects.all()
    for server in all_servers:
        if int(server.merge) != 0:
            continue
        httpClient = None
        suc = "失败"
        error = ""
        try:
            httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
            headers = {'Content-type': 'text/xml;charset=UTF-8'}
            httpClient.request("POST", opcodes.op_url("TMSG_HUODONG"), s, headers)
            response = httpClient.getresponse()
            text = response.read()
            fmt = "i%ds" % (len(text) - 4)
            error_code, info = struct.unpack(fmt, text)
            if response.status / 100 == 2:
                if error_code == 0:
                    suc = "成功"
                elif error_code == -100:
                    error = "无效的活动时间段"
                elif error_code == -101:
                    error = "无效的活动奖励参数"
                elif error_code == -102:
                    error = "活动没到开服开启时间"
                elif error_code == -103:
                    error = "活动已过开服结束时间"
                else:
                    error = "同步错误"
            else:
                error = "同步失败"
        except Exception as e:
            print(e)
            error = "同步失败"
        finally:
            if httpClient:
                httpClient.close()
        sync_servers = sync_servers + "[" + server.name + ":" + suc + ":" + error + "]"

    history = models.ActivitySyncHistory(sync_infos=sync_servers,
                                         start_time=start_time.replace(tzinfo=None),
                                         end_time=end_time.replace(tzinfo=None),
                                         synctime=datetime.datetime.now().replace(tzinfo=None),
                                         group=group)
    history.save()


def sync_activity():
    timelines = models.ActivityTime.objects.filter(startime__lte=datetime.datetime.now().replace(tzinfo=None))
    for tl in timelines:
        if tl.sync:
            continue
        groups = tl.activitygroup_set.all()
        for g in groups:
            sync_group(g, tl.startime, tl.endtime)
        tl.sync = True
        tl.save()


def tongji_normal():
    logdb = torndb.Connection(host="127.0.0.1", database="tslog", user="root", password="root", time_zone="+8:00")
    last_day = datetime.date.today() - datetime.timedelta(days=1)
    last_day_str = last_day.strftime("%Y-%m-%d")

    ##
    res = logdb.query('select COUNT(DISTINCT player_guid) from login_t where value2 = 1 and DATE(dt) = %s', last_day_str)
    new_user_num = res[0]["COUNT(DISTINCT player_guid)"]

    ##
    res = logdb.query("select COUNT(DISTINCT player_guid) from login_t where DATE(dt) = %s", last_day_str)
    huo_user_num = res[0]["COUNT(DISTINCT player_guid)"]

    ##
    res = logdb.query("select sum(value3),COUNT(DISTINCT player_guid),COUNT(player_guid) from login_t where value2 = 100 and DATE(dt) = %s", last_day_str)
    total_rmb = res[0]["sum(value3)"] if res[0]["sum(value3)"] else 0
    total_user_num = res[0]["COUNT(DISTINCT player_guid)"]
    total_num = res[0]["COUNT(player_guid)"]
    logdb.close()

    ##
    dpu = float(total_rmb) / float(huo_user_num)
    dpu = '%.2f' % dpu
    dpu = float(dpu)
    aku = (float(total_rmb) / float(total_user_num)) if total_user_num != 0 else 0
    aku = '%.2f' % aku
    aku = float(aku)

    tongji_data = models.TongjiNormal(new_user_num=new_user_num,
                                      huo_user_num=huo_user_num,
                                      total_rmb=total_rmb,
                                      total_user_num=total_user_num,
                                      total_num=total_num,
                                      dpu=dpu,
                                      aku=aku,
                                      dt=last_day)
    tongji_data.save()


def tongji_liucun():
    logdb = torndb.Connection(host="127.0.0.1", database="tslog", user="root", password="root", time_zone="+8:00")
    now_day = datetime.date.today() - datetime.timedelta(days=1)
    now_day_str = now_day.strftime("%Y-%m-%d")

    countDay = 30
    start_day = now_day - datetime.timedelta(days=29)
    while start_day < now_day:
        if countDay == 30 or countDay == 15 or countDay == 7 \
                or countDay == 6 or countDay == 5 or countDay == 4 \
                or countDay == 3 or countDay == 2:
            start_day_str = start_day.strftime("%Y-%m-%d")
            res = logdb.query('select COUNT(DISTINCT player_guid) from login_t where value2 = 1 and DATE(dt) = %s', start_day_str)
            new_user_num = res[0]["COUNT(DISTINCT player_guid)"]

            sql = "select sum(value3) from login_t where value2 = 100 and DATE(dt) >= %s and DATE(dt) <= %s and player_guid in \
                    (select player_guid from login_t where value2 = 1 and DATE(dt) = %s)"
            res = logdb.query(sql, start_day_str, now_day_str, start_day_str)
            ltv = res[0]["sum(value3)"] if res[0]["sum(value3)"] else 0

            sql = """select COUNT(DISTINCT player_guid) from login_t where value2 = 2 and DATE(dt) = %s and player_guid in
                     (select player_guid from login_t where value2 = 1 and DATE(dt) = %s)
                  """
            res = logdb.query(sql, now_day_str, start_day_str)
            liucun_user_num = res[0]["COUNT(DISTINCT player_guid)"]

            liucun = float(liucun_user_num) / float(new_user_num) if new_user_num != 0 else 0
            liucun = '%.3f' % liucun
            liucun = float(liucun)

            tongji_data = None
            try:
                tongji_data = models.TongjiLiucun.objects.get(dt=start_day)
            except Exception:
                tongji_data = models.TongjiLiucun(dt=start_day,
                                                  total_num=new_user_num,
                                                  liucun_2ltv=0,
                                                  liucun_2rate=0.0,
                                                  liucun_3ltv=0,
                                                  liucun_3rate=0.0,
                                                  liucun_4ltv=0,
                                                  liucun_4rate=0.0,
                                                  liucun_5ltv=0,
                                                  liucun_5rate=0.0,
                                                  liucun_6ltv=0,
                                                  liucun_6rate=0.0,
                                                  liucun_7ltv=0,
                                                  liucun_7rate=0.0,
                                                  liucun_15ltv=0,
                                                  liucun_15rate=0.0,
                                                  liucun_30ltv=0,
                                                  liucun_30rate=0.0)
            if tongji_data:
                if countDay == 2:
                    tongji_data.liucun_2ltv = ltv
                    tongji_data.liucun_2rate = liucun
                elif countDay == 3:
                    tongji_data.liucun_3ltv = ltv
                    tongji_data.liucun_3rate = liucun
                elif countDay == 4:
                    tongji_data.liucun_4ltv = ltv
                    tongji_data.liucun_4rate = liucun
                elif countDay == 5:
                    tongji_data.liucun_5ltv = ltv
                    tongji_data.liucun_5rate = liucun
                elif countDay == 6:
                    tongji_data.liucun_6ltv = ltv
                    tongji_data.liucun_6rate = liucun
                elif countDay == 7:
                    tongji_data.liucun_7ltv = ltv
                    tongji_data.liucun_7rate = liucun
                elif countDay == 15:
                    tongji_data.liucun_15ltv = ltv
                    tongji_data.liucun_15rate = liucun
                elif countDay == 30:
                    tongji_data.liucun_30ltv = ltv
                    tongji_data.liucun_30rate = liucun

                tongji_data.save()

        countDay = countDay - 1
        start_day = start_day + datetime.timedelta(days=1)


def sync_server():
    timelines = models.ServerStart.objects.filter(opentime__lte=datetime.datetime.now().replace(tzinfo=None))
    for tl in timelines:
        if tl.sync:
            continue
        tl.sync = True
        tl.save()
        res = 0
        if tl.zhuserver:
            res = sync_server_sub(tl)
        else:
            res = deploy_server_sub(tl)
        if res == -1:
            tl.sync_error = "start server fail"
        elif res == -2:
            tl.sync_error = "add server fail"
        elif res == -3:
            tl.sync_error = "add huodong server fail"
        elif res == -4:
            tl.sync_error = "download oss fail"
        elif res == -5:
            tl.sync_error = "gen serverlist fail"
        elif res == -6:
            tl.sync_error = "upload oss fail"
        elif res == 0:
            tl.sync_error = "sync success"
            tl.openstat = 4
        else:
            tl.sync_error = "unknow error"
        tl.save()


def sync_server_sub(server):
    try:
        ssh = paramiko.SSHClient()
        ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
        ssh.connect(server.host, username="root", password="yymoon@39299911")
        cmd = 'cd ' + '/home/app/work' + str(server.site) + "/" + ';./start.sh && sleep 10'
        ssh.exec_command(cmd)
        time.sleep(2)
        cmd = 'ps -ef | grep work%s' % server.site
        stdin, stdout, stderr = ssh.exec_command(cmd)
        result = stdout.readlines()
        has_gs = True
        has_remote = True
        for res in result:
            if res.find("./gs gs1 ../conf") != -1:
                has_gs = True
            if res.find("python -u remote.py") != -1:
                has_remote = True

        if has_gs and has_remote:
            pass
        else:
            post(server.host, server.serverid, "start server fail")
            return -1
    except Exception as e:
        post(server.host, server.serverid, e)
        return -1

    time.sleep(5)

    try:
        pt = models.ServerList.objects.get(id=server.ptid)
        new_server = models.Server(name=server.houtai_name,
                                   host=server.host,
                                   serverid=server.serverid,
                                   site=server.site,
                                   merge=0)
        new_server.save()
    except Exception as e:
        post(server.host, server.serverid, e)
        return -2

    try:
        if pt.huodong_path != "127.0.0.1":
                huodongtool = torndb.Connection(host=pt.huodong_path, database="gttool", user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
                sql = "insert into engine_server (id,name,host,serverid,site,merge) VALUES(0,'%s','%s',%s,%s,%s)" % (server.houtai_name, server.host, server.serverid, server.site, 0)
                huodongtool.insert(sql)
    except Exception as e:
        post(server.host, server.serverid, e)
        return -3

    try:
        if pt.recharge_path != "127.0.0.1":
            rechargegtool = torndb.Connection(host=pt.recharge_path, database="gttool", user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
            sql = "insert into engine_server (id,name,host,serverid,site,merge) VALUES(0,'%s','%s',%s,%s,%s)" % (server.houtai_name, server.host, server.serverid, server.site, server.serverid)
            rechargegtool.insert(sql)
    except Exception as e:
        post(server.host, server.serverid, e)
        return -3

    server_list_name = "/tmp/serverlist.xml"
    if server.hebing:
        server_list_name = "/tmp/config_yymoon.xml"

    result = download_oss(pt.osspath, server_list_name)
    if not result:
        post(server.host, server.serverid, "download oss error")
        return -4

    if server.hebing:
        try:
            node = None
            serverList = ET.parse(server_list_name)
            serverRoot = serverList.getroot()
            nodePlatform = serverRoot.findall("platform")
            for p in nodePlatform:
                if p.get("name") == server.pt:
                    node = p
                    break
            if node is not None:
                nodeChannel = node.findall("chanel")
                node = None
                for c in nodeChannel:
                    if c.get("name") == server.channel:
                        node = c
                        break
            if node is not None:
                node = node.find("serverlist")

            if node is None:
                post(server.host, server.serverid, "read hebing xml wrong")
                return -5

            newServer = ET.Element("server")
            newServer.tail = "\n"
            newServer.set("http", "http://%s:80%s0/" % (server.host, server.site))
            newServer.set("tcp", server.host)
            newServer.set("port", "90%s1" % (server.site))
            newServer.set("id", str(server.serverid))
            newServer.set("name", server.servername)
            newServer.set("state", "1")
            node.append(newServer)
            serverList.write(server_list_name, encoding="utf-8", xml_declaration=True)
        except Exception as e:
            post(server.host, server.serverid, e)
            return -5
    else:
        try:
            serverList = ET.parse(server_list_name)
            serverRoot = serverList.getroot()
            newServer = ET.Element("server")
            newServer.tail = "\n"
            newServer.set("http", "http://%s:80%s0/" % (server.host, server.site))
            newServer.set("tcp", server.host)
            newServer.set("port", "90%s1" % (server.site))
            newServer.set("id", str(server.serverid))
            newServer.set("name", server.servername)
            newServer.set("state", "1")
            serverRoot.append(newServer)
            serverList.write(server_list_name, encoding="utf-8", xml_declaration=True)
        except Exception as e:
            post(server.host, server.serverid, e)
            return -5

    if not upload_oss(pt.osspath, server_list_name):
        post(server.host, server.serverid, "upload oss error")
        return -6

    try:
        sync_activity_server(server.serverid)
    except Exception as e:
        post(server.host, server.serverid, e)

    os.remove(server_list_name)
    post(server.host, server.serverid, "success")
    return 0


def post(host, sid, error):
    mail_host = "smtp.126.com"
    mail_user = "xiaobang29@126.com"
    mail_pass = "45630066CCdd"
    sender = 'xiaobang29@126.com'
    receivers = ['xiaobang29@126.com']
    subject = '%s %s %s' % (host, sid, error)
    message = MIMEText(subject, 'plain', 'utf-8')
    message['From'] = Header("xiaobang29@126.com", 'utf-8')
    message['To'] = Header("xiaobang29@126.com", 'utf-8')

    subject = '%s %s %s' % (host, sid, error)
    message['Subject'] = Header(subject, 'utf-8')

    try:
        smtpObj = smtplib.SMTP()
        smtpObj.connect(mail_host, 25)
        smtpObj.login(mail_user, mail_pass)
        smtpObj.sendmail(sender, receivers, message.as_string())
    except Exception as e:
        print(e)


def download_oss(osss, path):
    api = oss_api.OssAPI("oss-cn-hongkong.aliyuncs.com", "LTAIZR9Nn6vxMjoC", "7ekqhZYtDOQ6BlfhsbubOsGDvU4Usm")

    headers = {}
    # content_type = "text/HTML"

    try:
        res = api.get_object_to_file("hktsjh", osss, path, headers)
        if (res.status / 100) != 2:
            return False
    except Exception as e:
        print(e)
        return False

    return True


def upload_oss(osss, path):
    api = oss_api.OssAPI("oss-cn-hongkong.aliyuncs.com", "LTAIZR9Nn6vxMjoC", "7ekqhZYtDOQ6BlfhsbubOsGDvU4Usm")
    headers = {}
    content_type = "text/HTML"
    try:
        res = api.put_object_from_file("hktsjh", osss, path, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception as e:
        print(e)
        return False

    return True


def deploy_server_sub(server):
    try:
        pt = models.ServerList.objects.get(id=server.ptid)
    except Exception as e:
        post(server.host, server.serverid, e)
        return -2

    server_list_name = "/tmp/serverlist.xml"
    if server.hebing:
        server_list_name = "/tmp/config_yymoon.xml"

    result = download_oss(pt.osspath, server_list_name)
    if not result:
        post(server.host, server.serverid, "download oss error")
        return -4

    if server.hebing:
        try:
            node = None
            serverList = ET.parse(server_list_name)
            serverRoot = serverList.getroot()
            nodePlatform = serverRoot.findall("platform")
            for p in nodePlatform:
                if p.get("name") == server.pt:
                    node = p
                    break
            if node is not None:
                nodeChannel = node.findall("chanel")
                node = None
                for c in nodeChannel:
                    if c.get("name") == server.channel:
                        node = c
                        break
            if node is not None:
                node = node.find("serverlist")

            if node is None:
                post(server.host, server.serverid, "read hebing xml wrong")
                return -5

            newServer = ET.Element("server")
            newServer.tail = "\n"
            newServer.set("http", "http://%s:80%s0/" % (server.host, server.site))
            newServer.set("tcp", server.host)
            newServer.set("port", "90%s1" % (server.site))
            newServer.set("id", str(server.serverid / 100))
            newServer.set("name", server.servername)
            newServer.set("state", "1")
            node.append(newServer)
            serverList.write(server_list_name, encoding="utf-8", xml_declaration=True)
        except Exception as e:
            post(server.host, server.serverid, e)
            return -5
    else:
        try:
            serverList = ET.parse(server_list_name)
            serverRoot = serverList.getroot()
            newServer = ET.Element("server")
            newServer.tail = "\n"
            newServer.set("http", "http://%s:80%s0/" % (server.host, server.site))
            newServer.set("tcp", server.host)
            newServer.set("port", "90%s1" % (server.site))
            newServer.set("id", str(server.serverid / 100))
            newServer.set("name", server.servername)
            newServer.set("state", "1")
            serverRoot.append(newServer)
            serverList.write(server_list_name, encoding="utf-8", xml_declaration=True)
        except Exception as e:
            post(server.host, server.serverid, e)
            return -5

    if not upload_oss(pt.osspath, server_list_name):
        post(server.host, server.serverid, "upload oss error")
        return -6

    os.remove(server_list_name)
    post(server.host, server.serverid, "success")
    return 0


def tongji_qudao_normal():
    last_day = datetime.date.today() - datetime.timedelta(days=1)
    last_day_str = last_day.strftime("%Y-%m-%d")
    all_qudao = models.TongjiQudao.objects.all()
    logdb = torndb.Connection(host="127.0.0.1", database="tslog", user="root", password="root", time_zone="+8:00")

    for qudao in all_qudao:
        ##
        res = logdb.query('select COUNT(DISTINCT player_guid) from login_t where pt = %s and DATE(dt) = %s and value2 = 1', qudao.qudao_name, last_day_str)
        new_user_num = res[0]["COUNT(DISTINCT player_guid)"]

        ##
        res = logdb.query("select COUNT(DISTINCT player_guid) from login_t where pt = %s and DATE(dt) = %s", qudao.qudao_name, last_day_str)
        huo_user_num = res[0]["COUNT(DISTINCT player_guid)"]

        ##
        res = logdb.query("select sum(value3),COUNT(DISTINCT player_guid),COUNT(player_guid) from login_t where pt = %s and DATE(dt) = %s and value2 = 100", qudao.qudao_name, last_day_str)
        total_rmb = res[0]["sum(value3)"] if res[0]["sum(value3)"] else 0
        total_user_num = res[0]["COUNT(DISTINCT player_guid)"]
        total_num = res[0]["COUNT(player_guid)"]
        logdb.close()

        ##
        dpu = float(total_rmb) / float(huo_user_num) if huo_user_num != 0 else 0
        dpu = '%.2f' % dpu
        dpu = float(dpu)

        aku = (float(total_rmb) / float(total_user_num)) if total_user_num != 0 else 0
        aku = '%.2f' % aku
        aku = float(aku)

        tongji_data = models.TongjiQudaoServer(new_user_num=new_user_num,
                                               huo_user_num=huo_user_num,
                                               total_rmb=total_rmb,
                                               total_user_num=total_user_num,
                                               total_num=total_num,
                                               dpu=dpu,
                                               aku=aku,
                                               dt=last_day,
                                               group=qudao)
        tongji_data.save()

    logdb.close()


def tongji_qudao_liucun():
    logdb = torndb.Connection(host="127.0.0.1", database="tslog", user="root", password="root", time_zone="+8:00")
    now_day = datetime.date.today() - datetime.timedelta(days=1)
    now_day_str = now_day.strftime("%Y-%m-%d")
    all_qudao = models.TongjiQudao.objects.all()

    for qudao in all_qudao:
        countDay = 30
        start_day = now_day - datetime.timedelta(days=29)
        while start_day < now_day:
            if countDay == 30 or countDay == 15 or countDay == 7 \
                    or countDay == 6 or countDay == 5 or countDay == 4 \
                    or countDay == 3 or countDay == 2:
                start_day_str = start_day.strftime("%Y-%m-%d")
                res = logdb.query('select COUNT(DISTINCT player_guid) from login_t where pt = %s and DATE(dt) = %s and value2 = 1', qudao.qudao_name, start_day_str)
                new_user_num = res[0]["COUNT(DISTINCT player_guid)"]

                sql = "select sum(value3) from login_t where pt = %s and value2 = 100 and DATE(dt) >= %s and DATE(dt) <= %s and player_guid in \
                        (select player_guid from login_t where pt = %s and value2 = 1 and DATE(dt) = %s)"
                res = logdb.query(sql, qudao.qudao_name, start_day_str, now_day_str, qudao.qudao_name, start_day_str)
                ltv = res[0]["sum(value3)"] if res[0]["sum(value3)"] else 0

                sql = """select COUNT(DISTINCT player_guid) from login_t where pt = %s and value2 = 2 and DATE(dt) = %s and player_guid in
                        (select player_guid from login_t where pt = %s and value2 = 1 and DATE(dt) = %s)
                        """
                res = logdb.query(sql, qudao.qudao_name, now_day_str, qudao.qudao_name, start_day_str)
                liucun_user_num = res[0]["COUNT(DISTINCT player_guid)"]

                liucun = float(liucun_user_num) / float(new_user_num) if new_user_num != 0 else 0
                liucun = '%.3f' % liucun
                liucun = float(liucun)

                tongji_data = None
                try:
                    tongji_data = models.TongjiQudaoLicun.objects.get(dt=start_day, group=qudao)
                except Exception:
                    tongji_data = models.TongjiQudaoLicun(dt=start_day,
                                                          total_num=new_user_num,
                                                          liucun_2ltv=0,
                                                          liucun_2rate=0.0,
                                                          liucun_3ltv=0,
                                                          liucun_3rate=0.0,
                                                          liucun_4ltv=0,
                                                          liucun_4rate=0.0,
                                                          liucun_5ltv=0,
                                                          liucun_5rate=0.0,
                                                          liucun_6ltv=0,
                                                          liucun_6rate=0.0,
                                                          liucun_7ltv=0,
                                                          liucun_7rate=0.0,
                                                          liucun_15ltv=0,
                                                          liucun_15rate=0.0,
                                                          liucun_30ltv=0,
                                                          liucun_30rate=0.0,
                                                          group=qudao)
                if tongji_data:
                    if countDay == 2:
                        tongji_data.liucun_2ltv = ltv
                        tongji_data.liucun_2rate = liucun
                    elif countDay == 3:
                        tongji_data.liucun_3ltv = ltv
                        tongji_data.liucun_3rate = liucun
                    elif countDay == 4:
                        tongji_data.liucun_4ltv = ltv
                        tongji_data.liucun_4rate = liucun
                    elif countDay == 5:
                        tongji_data.liucun_5ltv = ltv
                        tongji_data.liucun_5rate = liucun
                    elif countDay == 6:
                        tongji_data.liucun_6ltv = ltv
                        tongji_data.liucun_6rate = liucun
                    elif countDay == 7:
                        tongji_data.liucun_7ltv = ltv
                        tongji_data.liucun_7rate = liucun
                    elif countDay == 15:
                        tongji_data.liucun_15ltv = ltv
                        tongji_data.liucun_15rate = liucun
                    elif countDay == 30:
                        tongji_data.liucun_30ltv = ltv
                        tongji_data.liucun_30rate = liucun

                    tongji_data.save()

            countDay = countDay - 1
            start_day = start_day + datetime.timedelta(days=1)

    logdb.close()


def sync_activity_server(sid):
    startime1 = datetime.date.today()
    startime = datetime.datetime(startime1.year, startime1.month, startime1.day, 0, 0, 0)
    timelines = models.ActivityTime.objects.filter(isnew=True)
    for tl in timelines:
        endtime = startime + datetime.timedelta(days=tl.endday)
        groups = tl.activitygroup_set.all()
        for g in groups:
            sync_group_server(g, startime, endtime, sid)


def sync_group_server(group, start_time, end_time, sid):
    msg = rpc_pb2.tmsg_activity_group()
    msg.name = group.name
    msg.isjieri = group.isjieri
    msg.item_name1 = group.item_name1
    msg.item_name2 = group.item_name2
    msg.item_des1 = group.item_des1
    msg.item_des2 = group.item_des2
    activitys = group.activitys.all()
    for act in activitys:
        activityID = models.ActivityID.objects.get(type=act.type)
        activityID.aid = activityID.aid + 1
        activityID.save()

        tmsg = msg.activitys.add()
        tmsg.id = activityID.aid
        tmsg.start = int(time.mktime(start_time.timetuple()) * 1000)
        tmsg.end = int(time.mktime(end_time.timetuple()) * 1000)
        tmsg.kaifu_start = act.kaifu_start
        tmsg.kaifu_end = act.kaifu_end
        tmsg.name = act.huodong_name
        tmsg.kaikai_start = act.kaikai_start
        tmsg.type = act.atype
        tmsg.subtype = act.subtype
        tmsg.show = tmsg.start + act.start_show_day * 24 * 60 * 60 * 1000
        if act.end_show_day == 0:
            tmsg.noshow = tmsg.end
        else:
            tmsg.noshow = tmsg.start + act.end_show_day * 24 * 60 * 60 * 1000

        entrys = act.entrys.order_by("condition").order_by("order")
        for entry in entrys:
            reward = tmsg.rewards.add()
            reward.id = entry.aid
            reward.condition = entry.condition
            reward.arg1 = entry.arg1
            reward.arg2 = entry.arg2
            reward.arg3 = entry.arg3
            reward.arg4 = entry.arg4
            reward.arg5 = entry.arg5
            reward.show_time = tmsg.start
            if tmsg.subtype == 8:
                reward.show_time = tmsg.start + entry.arg3 * 24 * 60 * 60 * 1000
            if entry.duihuan11 != 0:
                reward.arg6.append(entry.duihuan11)
                reward.arg7.append(entry.duihuan12)
                reward.arg8.append(entry.duihuan13)
            if entry.duihuan21 != 0:
                reward.arg6.append(entry.duihuan21)
                reward.arg7.append(entry.duihuan22)
                reward.arg8.append(entry.duihuan23)
            if entry.type11 != 0:
                reward.types.append(entry.type11)
                reward.value1s.append(entry.value12)
                reward.value2s.append(entry.value13)
                reward.value3s.append(entry.value14)
            if entry.type21 != 0:
                reward.types.append(entry.type21)
                reward.value1s.append(entry.value22)
                reward.value2s.append(entry.value23)
                reward.value3s.append(entry.value24)
            if entry.type31 != 0:
                reward.types.append(entry.type31)
                reward.value1s.append(entry.value32)
                reward.value2s.append(entry.value33)
                reward.value3s.append(entry.value34)
            if entry.type41 != 0:
                reward.types.append(entry.type41)
                reward.value1s.append(entry.value42)
                reward.value2s.append(entry.value43)
                reward.value3s.append(entry.value44)
    s = msg.SerializeToString()
    sync_servers = ""
    all_servers = models.Server.objects.filter(serverid=sid)
    for server in all_servers:
        if int(server.merge) != 0:
            continue
        httpClient = None
        suc = "失败"
        error = ""
        try:
            httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
            headers = {'Content-type': 'text/xml;charset=UTF-8'}
            httpClient.request("POST", opcodes.op_url("TMSG_HUODONG"), s, headers)
            response = httpClient.getresponse()
            text = response.read()
            fmt = "i%ds" % (len(text) - 4)
            error_code, info = struct.unpack(fmt, text)
            if response.status / 100 == 2:
                if error_code == 0:
                    suc = "成功"
                elif error_code == -100:
                    error = "无效的活动时间段"
                elif error_code == -101:
                    error = "无效的活动奖励参数"
                elif error_code == -102:
                    error = "活动没到开服开启时间"
                elif error_code == -103:
                    error = "活动已过开服结束时间"
                else:
                    error = "同步错误"
            else:
                error = "同步失败"
        except Exception as e:
            print(e)
            error = "同步失败"
        finally:
            if httpClient:
                httpClient.close()
        sync_servers = sync_servers + "[" + server.name + ":" + suc + ":" + error + "]"

    history = models.ActivitySyncHistory(sync_infos=sync_servers,
                                         start_time=start_time.replace(tzinfo=None),
                                         end_time=end_time.replace(tzinfo=None),
                                         synctime=datetime.datetime.now().replace(tzinfo=None),
                                         group=group)
    history.save()


def tongji_normal_huoyue():
    logdb = torndb.Connection(host="127.0.0.1", database="tslog", user="root", password="root", time_zone="+8:00")
    last_day = datetime.date.today() - datetime.timedelta(days=1)
    last_day_str = last_day.strftime("%Y-%m-%d")

    res = logdb.query("select serverid, COUNT(DISTINCT player_guid) from login_t where DATE(dt) = %s GROUP BY serverid", last_day_str)
    for r in res:
        tongji_data = models.TongjiNormalHuoyue(
                               huo_user_num=r["COUNT(DISTINCT player_guid)"],
                               serverid=r["serverid"],
                               dt=last_day)
        tongji_data.save()
