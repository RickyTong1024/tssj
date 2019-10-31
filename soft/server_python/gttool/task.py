# -*- coding: utf-8 -*-

from fabric import api, context_managers
import os
import torndb
from oss import oss_api
import lxml.etree as ET
# import time

server_mysql_pass_word = "1qaz2wsx@39299911"
gtool_host = "127.0.0.1"
gtool_db = "gttool"
gtool_user = "root"
gtool_pwd = "root"

api.env.user = "root"
api.env.password = "yymoon@39299911"


@api.task
def deployee(path, sid, ip, site, host):
    with api.cd("/home/app"):
        api.run("rm -rf work%s" % site)

    with api.lcd(path):
        api.local("cp -rf work work%s" % site)
        api.local("mysql -h%s -uroot -p%s --default-character-set=utf8 tsjh%s < tsjh.sql" % (host, server_mysql_pass_word, site))

    with api.lcd(path + "/work%s" % site):
        api.local("python replace.py ./server/conf/game.json {{sid}} %s" % sid)
        api.local("python replace.py ./server/conf/server.json {{slot}} %s" % site)

        api.local("python replace.py ./gate1/config.py {{slot}} %s" % site)
        api.local("python replace.py ./gate2/config.py {{slot}} %s" % site)
        api.local("python replace.py ./gate3/config.py {{slot}} %s" % site)
        api.local("python replace.py ./gate4/config.py {{slot}} %s" % site)

        api.local("python replace.py ./libao/config.py {{slot}} %s" % site)

        api.local("python replace.py ./login/config.py {{slot}} %s" % site)

        api.local("python replace.py ./remote/config.py {{slot}} %s" % site)

        api.local("python replace.py ./start.sh {{slot}} %s" % site)
        api.local("python replace.py ./stop.sh {{slot}} %s" % site)
        api.local("python replace.py ./stopex.sh {{slot}} %s" % site)
        api.local("python replace.py ./updatebak.sh {{slot}} %s" % site)

        api.local("rm -rf replace.py")

    with api.lcd(path):
        f = "work%s" % site
        api.local("tar -czf %s.tar.gz %s" % (f, f))

        with context_managers.settings(warn_only=True):
            result = api.put("work%s.tar.gz" % site, "/home/app")
            if result.failed:
                api.abort("Error: put error")

            lmd5 = api.local("md5sum work%s.tar.gz" % site, capture=True).split(" ")[0]
            rmd5 = api.run("md5sum /home/app/work%s.tar.gz" % site).split(" ")[0]
            if lmd5 != rmd5:
                api.abort("Error: md5sum")

            api.local("rm -rf work%s.tar.gz" % site)
            api.local("rm -rf work%s" % site)

    with api.cd("/home/app"):
        api.run("tar -xzvf work%s.tar.gz" % site)
        api.run("rm -rf work%s.tar.gz" % site)


@api.task
def makee(site):
    with api.cd("/home/app"):
        api.run("chmod -R 777 work%s" % site)

    with api.cd("/home/app/work%s" % site):
        api.run("./setff.sh")

    with api.cd("/home/app/work%s/server" % site):
        api.run("./make.sh")

    with api.cd("/home/app/work%s/server/out" % site):
        result = api.run("ls -h")
        suc = False
        try:
            result.index("gs")
            result.index("chat")
            result.index("master")
            suc = True
        except Exception as e:
            print(e)

        if not suc:
            api.abort("make fail")


def get_guid(T, Q, I):
    num = (((int(T) << 56) & 0xFF00000000000000) | ((int(Q) << 44) & 0x00FFF00000000000) | (int(I) & 0x00000FFFFFFFFFFF))
    return int(num)


@api.task
def startee(host, sid, site, huodong, recharge):
    print(host, sid, site, huodong)
    with api.cd("/home/app/work%s" % site):
        api.run("./start.sh && sleep 5")
        result = api.run("ps -ef | grep work%s" % site)
        try:
            result.index("python2.7 -u http.py work%s" % site)
            result.index("python2.7 -u libao.py work%s" % site)
            result.index("python2.7 -u login.py work%s" % site)
            result.index("python2.7 -u remote.py work%s" % site)
            result.index("./gs gs1 ../conf work%s" % site)
        except Exception as e:
            print(e)
            api.run("./stop.sh")
            raise "start server error"

    try:
        gtool = torndb.Connection(host=gtool_host, database=gtool_db, user=gtool_user, password=gtool_pwd, time_zone="+8:00")
        sql = "select houtai_name, ptid from engine_serverstart where serverid = %s" % sid
        result = gtool.get(sql)
        if not result:
            raise "empty"
        sql = "insert into engine_server (id,name,host,serverid,site,merge,ptid) VALUES(0,'%s','%s',%s,%s,%s,%s)" % (result["houtai_name"], host, sid, site, 0, result["ptid"])
        gtool.insert(sql)
    except Exception as e:
        print(e)
        raise "get houtai_name error"

    try:
        if huodong != "127.0.0.1":
            huodongtool = torndb.Connection(host=huodong, database="gttool", user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
            sql = "insert into engine_server (id,name,host,serverid,site,merge,ptid) VALUES(0,'%s','%s',%s,%s,%s,0)" % (result["houtai_name"], host, sid, site, 0)
            huodongtool.insert(sql)
            huodongtool.close()
    except Exception as e:
        print(e)
        raise "huodong houtai error"

    try:
        if recharge != "127.0.0.1":
            rechargegtool = torndb.Connection(host=recharge, database="gttool", user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
            sql = "insert into engine_server (id,name,host,serverid,site,merge,ptid) VALUES(0,'%s','%s',%s,%s,%s,0)" % (result["houtai_name"], host, sid, site, sid)
            rechargegtool.insert(sql)
            rechargegtool.close()
    except Exception as e:
        print(e)
        raise "recharge houtai error"


def download_oss(osss, path):
    api = oss_api.OssAPI("oss-cn-hangkong.aliyuncs.com", "5EhND1dOrdtSR4Vo", "nEeWqyWFWwkbHtb0hMuFNbN8nzLrYp")

    headers = {}
    # content_type = "text/HTML"

    try:
        res = api.get_object_to_file("xzn2", osss, path, headers)
        if (res.status / 100) != 2:
            return False
    except Exception as e:
        print(e)
        return False

    return True


def upload_oss(osss, path):
    api = oss_api.OssAPI("oss-cn-hangkong.aliyuncs.com", "5EhND1dOrdtSR4Vo", "nEeWqyWFWwkbHtb0hMuFNbN8nzLrYp")
    headers = {}
    content_type = "text/HTML"
    try:
        res = api.put_object_from_file("xzn2", osss, path, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception as e:
        print(e)
        return False

    return True


@api.task
def uploadex(osss, path, host, site, sid):
    print(osss, path, host, site, sid)
    result = download_oss(osss, path)
    if not result:
        raise "download fail"

    servername = ""
    hebing = 0
    zhuserver = 0
    try:
        gtool = torndb.Connection(host=gtool_host, database=gtool_db, user=gtool_user, password=gtool_pwd, time_zone="+8:00")
        sql = "select servername,hebing,zhuserver from engine_serverstart where serverid = %s" % sid
        result = gtool.get(sql)
        if not result:
            raise "empty servername"
        servername = result["servername"]
        hebing = result["hebing"]
        zhuserver = result["zhuserver"]
    except Exception as e:
        print(e)
        raise "servername wrong"

    if hebing:
        try:
            uploadex1(osss, path, host, site, sid)
        except Exception as e:
            print(e)
            raise "serverlist wrong"
    else:
        try:
            print("start haha")
            serverList = ET.parse(path)
            serverRoot = serverList.getroot()
            newServer = ET.Element("server")
            newServer.tail = "\n"
            newServer.set("http", "http://%s:80%s0/" % (host, site))
            newServer.set("tcp", host)
            newServer.set("port", "90%s1" % (site))
            if zhuserver:
                newServer.set("id", str(sid))
            else:
                newServer.set("id", str(int(sid) / 100))
            newServer.set("name", servername.encode("utf-8").decode("utf-8"))
            newServer.set("state", "1")
            serverRoot.append(newServer)
            serverList.write(path, encoding="utf-8", xml_declaration=True)
        except Exception as e:
            print(e)
            raise "serverlist wrong"

        if not upload_oss(osss, path):
            raise "upload fail"


@api.task
def uploadex1(osss, path, host, site, sid):
    print(osss, path, host, site, sid)
    result = download_oss(osss, path)
    if not result:
        raise "download fail"

    servername = ""
    pt = ""
    channel = ""
    zhuserver = 0
    try:
        gtool = torndb.Connection(host=gtool_host, database=gtool_db, user=gtool_user, password=gtool_pwd, time_zone="+8:00")
        sql = "select servername,pt,channel,zhuserver from engine_serverstart where serverid = %s" % sid
        result = gtool.get(sql)
        if not result:
            raise "empty servername"
        servername = result["servername"]
        pt = result["pt"]
        channel = result["channel"]
        zhuserver = result["zhuserver"]
    except Exception as e:
        print(e)
        raise "servername wrong"

    try:
        node = None
        serverList = ET.parse(path)
        serverRoot = serverList.getroot()
        nodePlatform = serverRoot.findall("platform")
        for p in nodePlatform:
            if p.get("name") == pt:
                node = p
                break
        if node is not None:
            nodeChannel = node.findall("chanel")
            node = None
            for c in nodeChannel:
                if c.get("name") == channel:
                    node = c
                    break
        if node is not None:
            node = node.find("serverlist")

        if node is None:
            raise "read xml wrong"

        newServer = ET.Element("server")
        newServer.tail = "\n"
        newServer.set("http", "http://%s:80%s0/" % (host, site))
        newServer.set("tcp", host)
        newServer.set("port", "90%s1" % (site))
        if zhuserver:
            newServer.set("id", str(sid))
        else:
            newServer.set("id", str(int(sid) / 100))
        newServer.set("name", servername.encode("utf-8").decode("utf-8"))
        newServer.set("state", "1")
        node.append(newServer)
        serverList.write(path, encoding="utf-8", xml_declaration=True)
    except Exception as e:
        print(e)
        raise "serverlist wrong"

    if not upload_oss(osss, path):
        raise "upload fail"


@api.task
def do(remote, path, sid, ip, site):
    host_host = "root@%s:22" % remote
    api.env.host_string = host_host
    try:
        deployee(path, sid, ip, site, remote)
        makee(site)
        os._exit(0)
    except Exception as e:
        print(e)
        os._exit(1)


@api.task
def start(remote, sid, site, osss, path, huodong, recharge):
    host_host = "root@%s:22" % remote
    api.env.host_string = host_host
    try:
        startee(remote, sid, site, huodong, recharge)
        uploadex(osss, path, remote, site, sid)
        os._exit(0)
    except Exception as e:
        print(e)
        os._exit(1)


@api.task
def onlyupload(remote, sid, site, osss, path, huodong):
    host_host = "root@%s:22" % remote
    api.env.host_string = host_host
    try:
        uploadex(osss, path, remote, site, sid)
        os._exit(0)
    except Exception as e:
        print(e)
        os._exit(1)
