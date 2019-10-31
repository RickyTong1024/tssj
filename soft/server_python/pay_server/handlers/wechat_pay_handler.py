# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
import base_handler
import xmltodict
import hashlib


class NotifyHandler(base_handler.BaseHandler):
    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def post(self):
        try:
            args = xmltodict.parse(self.request.body)['xml']
            sign = args.pop("sign")
        except Exception as e:
            print(e)
            print(self.request.body)
            self.response("invalid xml data")
            return

        if args.get("return_code", "empty") != "SUCCESS":
            print(args.get("return_msg", "unknown error"))
            self.response("return_code is not SUCCESS")
            return

        if args.get("result_code", "empty") != "SUCCESS":
            print(args.get("err_code_des", "unknown error"))
            self.response("result_code is not SUCCESS")
            return

        if make_wepay_sign(args, "yinyuewangluo123YYMOONWANGLUO123") != sign:
            print(args, sign)
            self.response("invalid sign")
            return

        # repeat order
        odd_id = "wx" + args.get("transaction_id")
        try:
            username, serverid, rid, sku, huodong_id, entry_id = args.get("attach").rsplit("_", 5)
            serverid = int(serverid)
            rid = int(rid)
            huodong_id = int(huodong_id)
            entry_id = int(entry_id)
        except Exception as e:
            print(e)
            self.response(e)
            return

        res, ecode = yield self.deliver_final(username, serverid, odd_id, rid, huodong_id, entry_id)
        if res == 0:
            self.response("OK")
        else:
            self.response(ecode)

    def response(self, error="OK"):
        ret_tml = """
                    <xml>
                    <return_code><![CDATA[{0}]]></return_code>
                    <return_msg><![CDATA[{1}]]></return_msg>
                    </xml>
                    """
        ret_result = ret_tml.format("FAIL" if error != 'OK' else 'SUCCESS', error)
        print(ret_result)
        self.write(ret_result)
        self.finish()


def make_wepay_sign(args, appkey):
    sign = "&".join(k + "=" + args[k] for k in sorted(args.keys()))
    sign = sign + "&" + "key=" + appkey
    m = hashlib.md5()
    m.update(sign.encode('utf8'))
    return m.hexdigest().upper()
