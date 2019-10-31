# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
import base_handler
import hashlib
import hmac
import json


class NotifyHandler(base_handler.BaseHandler):
    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def get(self):
        all_args = {}
        all_args["sid"] = self.get_argument("sid", None)
        all_args["uid"] = self.get_argument("uid", None)
        all_args["orderno"] = self.get_argument("orderno", None)
        all_args["amount"] = self.get_argument("amount", None)
        all_args["status"] = self.get_argument("status", None)
        all_args["extinfo"] = self.get_argument("extinfo", None)
        all_args["nonce"] = self.get_argument("nonce", None)
        token = self.get_argument("token", None)

        args = {}
        for k, v in all_args.items():
            if v is not None:
                args[k] = v

        if make_bt_sign(args, "8b1b7ab788f0e56cb2dcc27d4f6428fd") != token:
            print("bt:signError")
            self.response("ERROR")
            return

        if args.get("status", "0") != "1":
            print("bt:statusError")
            self.response("ERROR")
            return

        # repeat order
        order_idd = "bt" + args.get("orderno", "empty")
        if order_idd == "btempty":
            print("bt:orderError")
            self.response("ERROR")
            return
        # 充值金额确认
        amount = int(args.get("amount"))
        print(amount)
        # if self.application.recharge.get_vip(rid) / 500 != amount:
        #     self.response("amounterror")
        #     return

        try:
            username, serverid, rid, huodongid, entryid = args.get("extinfo").rsplit("_", 4)
            print(username, serverid, rid, huodongid, entryid)
            rid = int(rid)
            serverid = int(serverid)
            huodongid = int(huodongid)
            entryid = int(entryid)
        except Exception as e:
            print("bt:", e)
            self.response("ERROR")
            return

        res, ecode = yield self.deliver_final(username, serverid, order_idd, rid, huodongid, entryid)
        if res == 0:
            re = json.dumps({"errno": "0", "errmsg": "success"})
        else:
            re = json.dumps({"errno": "1", "errmsg": ecode})
        self.response(re)


def make_bt_sign(args, appkey):
    sign = ''
    for k in args.keys():
        if(k != 'status'):
            sign = sign + str(args[k])
    m = hashlib.md5()
    m.update((sign + appkey).encode('utf8'))
    return m.hexdigest()
    # return m
