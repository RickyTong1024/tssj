#!/usr/bin/env python
# coding=utf-8

import sys
sys.path.append("../common")
import config
import pull
import thrpool
from rpc_pb2 import *
import http.client
import json
import urllib.parse
import time


def get_official(addr, uid, token):      # 谷歌 苹果 官网
    errno = -1
    httpClient = None
    try:
        params = urllib.parse.urlencode({'token': uid, 'password': token})
        headers = {"Content-type": "application/x-www-form-urlencoded", "Accept": "text/plain"}
        print(params)
        httpClient = http.client.HTTPConnection(addr)
        httpClient.request("POST", "/check", params, headers)
        response = httpClient.getresponse()
        if response.status != 200:
            errno = -1
        else:
            r = response.read()
            r = json.loads(r)
            errno = r["res"]
    except Exception as e:
        print(e)
        errno = -1
    finally:
        if httpClient:
            httpClient.close()
    return errno


def get_http_yijie(addr, uid, token, sdk, app):     # 易接
    errno = -1
    httpClient = None
    uin = uid
    try:
        sdk, uin = uid.split("_", 1)
    except:
        uin = uid
    try:
        print(uin, sdk)
        params = "sdk=" + sdk + "&app=" + app + "&uin=" + uin + "&sess=" + token
        httpClient = http.client.HTTPConnection(addr)
        httpClient.request("GET", "/login/check.html?" + params)
        response = httpClient.getresponse()
        if response.status != 200:
            errno = -1
        else:
            r = response.read().decode('utf8')
            if r == "0":
                errno = 0
            else:
                errno = -1
    except Exception as e:
        print(e)
        errno = -1
    finally:
        if httpClient:
            httpClient.close()
    return errno


def get_official_BT(addr, uid, token):      # 超VBT版本
    errno = -1
    httpClient = None
    print(uid)
    print(token)
    try:
        params = "uid=" + uid + "&token=" + token
        http_client = http.client.HTTPConnection(addr)
        http_client.request("GET", "/xzn/auth?" + params)
        response = http_client.getresponse()
        print(response)
        if response.status != 200:
            print("response.status != 200")
            errno = -1
        else:
            r = response.read()
            r = json.loads(r)
            errno = r["errno"]
            errmsg = r["errmsg"]
            print(errno)
            print(errmsg)
    except Exception as e:
        print(e)
        errno = -1
    finally:
        if http_client:
            http_client.close()
    return errno


def run(tin):
    inmsg = tmsg_req_login_heitao()
    inmsg.ParseFromString(tin.req.msg)
    pt = inmsg.pt
    errno = 0
    print(pt)
    if pt == "official_yijie":
        # 易接
        errno = get_http_yijie("sync.1sdk.cn", inmsg.uid, inmsg.token, '09CE2B99C22E6D06', '32EECB02-B48B1D6E')
    elif pt == "official_BT":
        # 超VBT版本
        errno = get_official_BT("smi.51sfsy.com", inmsg.uid, inmsg.token)
    else:
        # 官网、苹果、谷歌
        errno = get_official(config.url, inmsg.uid, inmsg.token)
    if errno == 0:
        print("校验成功")
    else:
        print("校验失败")
    print(errno)
    outmsg = tmsg_rep_login_heitao()
    outmsg.errres = errno
    outmsg.errmsg = ""
    s = outmsg.SerializeToString()
    pull.ioloop.instance().send_rep_msg(tin.req.id, s)


def main():
    pull.ioloop.instance().start(config.myname, config.push_addr, config.pull_addr)
    thrpool.ThreadPool.instance().start(20)
    while 1:
        tin = pull.ioloop.instance().read_msg()
        for i in range(len(tin)):
            thrpool.ThreadPool.instance().add_task(run, tin[i])
        time.sleep(0.1)
    thrpool.ThreadPool.instance().wait()


if __name__ == '__main__':
    main()
