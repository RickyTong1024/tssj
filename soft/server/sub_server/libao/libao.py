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
import time


def run(tin):
    inmsg = tmsg_req_libao()
    inmsg.ParseFromString(tin.req.msg)
    code = inmsg.code
    pt = inmsg.pt
    use = inmsg.use
    username = inmsg.username
    serverid = inmsg.serverid

    res = 0
    reward = []
    pc = 0
    chongzhi = 0
    httpClient = None
    try:
        httpClient = http.client.HTTPConnection(config.url)
        params = json.dumps({"code": code, "pt": pt, "use": use, "username": username, "serverid": serverid})
        print(params)
        headers = {'Content-type': 'text/xml;charset=UTF-8'}
        httpClient.request("POST", "/libao", params, headers)
        response = httpClient.getresponse()
        print(response.status)
        if response.status != 200:
            res = -1
        else:
            r = response.read()
            print(r)
            rjson = json.loads(r)
            res = rjson['res']
            reward = rjson['reward']
            pc = rjson['pc']
            chongzhi = rjson["chongzhi"]
    except Exception as e:
        print(e)
        res = -1
    finally:
        if httpClient:
            httpClient.close()

    outmsg = tmsg_rep_libao()
    outmsg.res = res
    outmsg.pc = pc
    outmsg.chongzi = chongzhi
    for i in range(len(reward)):
        outmsg.types.append(reward[i][0])
        outmsg.value1.append(reward[i][1])
        outmsg.value2.append(reward[i][2])
        outmsg.value3.append(reward[i][3])
    s = outmsg.SerializeToString()
    pull.ioloop.instance().send_rep_msg(tin.req.id, s)


def main():
    pull.ioloop.instance().start(config.myname, config.push_addr, config.pull_addr)
    thrpool.ThreadPool.instance().start(5)
    while 1:
        tin = pull.ioloop.instance().read_msg()
        for i in range(len(tin)):
            thrpool.ThreadPool.instance().add_task(run, tin[i])
        time.sleep(0.1)
    thrpool.ThreadPool.instance().wait()


if __name__ == '__main__':
    main()
