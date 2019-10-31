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
        print("here")
        all_args = {}
        all_args["app"] = self.get_argument("app", None)
        all_args["cbi"] = self.get_argument("cbi", None)
        all_args["ct"] = self.get_argument("ct", None)
        all_args["fee"] = self.get_argument("fee", None)
        all_args["pt"] = self.get_argument("pt", None)
        all_args["sdk"] = self.get_argument("sdk", None)
        all_args["ssid"] = self.get_argument("ssid", None)
        all_args["st"] = self.get_argument("st", None)
        all_args["tcd"] = self.get_argument("tcd", None)
        all_args["uid"] = self.get_argument("uid", None)
        all_args["ver"] = self.get_argument("ver", None)
        sign = self.get_argument("sign", None)

        args = {}
        for k, v in all_args.items():
            if v is not None:
                args[k] = v
        print(sign)
        print(args)
        print(make_yijie_sign(args, "RM4LB3TRXQWCSK5VQHZY5FGBT2A1J0SG"))

        if make_yijie_sign(args, "RM4LB3TRXQWCSK5VQHZY5FGBT2A1J0SG") != sign:
            print("yijie:signError")
            self.response("ERROR")
            return
        if args.get("st", "0") != "1":
            print("yijie:statusError")
            self.response("ERROR")
            return

        # repeat order
        order_idd = "yijie" + args.get("tcd", "empty")
        if order_idd == "yijieempty":
            print("yijie:orderError")
            self.response("ERROR")
            return

        try:
            username, serverid, rid, huodongid, entryid = args.get("cbi").rsplit("_", 4)
            rid = int(rid)
            serverid = int(serverid)
            huodongid = int(huodongid)
            entryid = int(entryid)
        except Exception as e:
            print("yijie:", e)
            self.response("ERROR")
            return

        res, ecode = yield self.deliver_final(username, serverid, order_idd, rid, huodongid, entryid)
        if res == 0:
            self.response("SUCCESS")
        else:
            self.response(ecode)


def make_yijie_sign(args, appkey):
    sign = "&".join(k + "=" + str(args[k]) for k in sorted(args.keys()))
    m = hashlib.md5()
    m.update((sign + appkey).encode('utf8'))
    return m.hexdigest()
