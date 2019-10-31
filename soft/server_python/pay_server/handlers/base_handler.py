# -*- coding: utf-8 -*-

import sys
sys.path.append("../help")
import tornado.web
import tornado.gen
from tornado.httpclient import AsyncHTTPClient, HTTPRequest
import base_handler
import opcodes
import struct
from rpc_pb2 import *


class BaseHandler(tornado.web.RequestHandler):
    def dbg_print(self):
        print(self.request.body)

    @property
    def db(self):
        self.application.ping()
        return self.application.db

    @property
    def gttool(self):
        self.application.gttool_ping()
        return self.application.gtool

    def response(self, content):
        self.write(content)
        self.finish()

    @tornado.gen.coroutine
    def deliver_final(self, username, serverid, order_idd, rid, huodong_id, entry_id):
        cur = self.db.cursor()
        sql = "select id from pay_t where orderid = %s"
        param = (order_idd,)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 0:
            print("order dumplicated")
            raise tornado.gen.Return([-1, "order dumplicated"])

        delivery_res = yield self.order_delivery(username, serverid, order_idd, rid, huodong_id, entry_id)

        sql = "insert into pay_t (id, username, serverid, rid, orderid, res, dt) values (0, %s, %s, %s, %s, %s, NOW())"
        param = (username, serverid, rid, order_idd, delivery_res,)
        cur.execute(sql, param)

        if delivery_res != 0:
            raise tornado.gen.Return([-1, " the delivery_res is't zero"])
        raise tornado.gen.Return([0, ""])

    @tornado.gen.coroutine
    def order_delivery(self, username, serverid, orderno, pid, huodong_id, entry_id):
        msg = tmsg_req_recharge_heitao()
        msg.uid = username
        msg.sid = str(serverid)
        msg.order = orderno
        msg.pid = pid
        msg.count = self.application.recharge.get_rmb(pid)
        msg.huodong_id = huodong_id
        msg.entry_id = entry_id
        s = msg.SerializeToString()

        db = None
        host = ""
        port = 0
        cur = self.gttool.cursor()
        sql = "select host, site from engine_server where serverid = %s"
        param = (serverid,)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 0:
            port = int(res[0][1]) * 10 + 8000
            host = "http://" + res[0][0] + ':' + str(port)
        else:
            raise tornado.gen.Return(-1)
        error_code = -1
        http_client = AsyncHTTPClient()
        try:
            request = HTTPRequest(url=host + opcodes.op_url("TMSG_RECHARGE_HEITAO"), method="POST",
                                  headers={'Content-type': 'text/xml;charset=UTF-8'},
                                  body=s)
            response = yield http_client.fetch(request)
            fmt = "i%ds" % (len(response.body) - 4)
            error_code, info = struct.unpack(fmt, response.body)
            print(error_code, info)
        except Exception as e:
            print(e)
            raise tornado.gen.Return(-1)
        raise tornado.gen.Return(error_code)
