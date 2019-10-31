# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
import base_handler
import urllib
from tornado.httpclient import AsyncHTTPClient


class NotifyHandler(base_handler.BaseHandler):
    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def post(self):
        all_args = {}
        all_args["notify_time"] = self.get_argument("notify_time", None)
        all_args["notify_type"] = self.get_argument("notify_type", None)
        all_args["notify_id"] = self.get_argument("notify_id", None)
        all_args["app_id"] = self.get_argument("app_id", None)
        all_args["trade_no"] = self.get_argument("trade_no", None)
        all_args["out_trade_no"] = self.get_argument("out_trade_no", None)
        all_args["out_biz_no"] = self.get_argument("out_biz_no", None)
        all_args["buyer_id"] = self.get_argument("buyer_id", None)
        all_args["buyer_logon_id"] = self.get_argument("buyer_logon_id", None)
        all_args["seller_id"] = self.get_argument("seller_id", None)
        all_args["seller_email"] = self.get_argument("seller_email", None)
        all_args["trade_status"] = self.get_argument("trade_status", None)
        all_args["total_amount"] = self.get_argument("total_amount", None)
        all_args["receipt_amount"] = self.get_argument("receipt_amount", None)
        all_args["invoice_amount"] = self.get_argument("invoice_amount", None)
        all_args["buyer_pay_amount"] = self.get_argument("buyer_pay_amount", None)
        all_args["point_amount"] = self.get_argument("point_amount", None)
        all_args["refund_fee"] = self.get_argument("refund_fee", None)
        all_args["send_back_fee"] = self.get_argument("send_back_fee", None)
        all_args["subject"] = self.get_argument("subject", None)
        all_args["body"] = self.get_argument("body", None)
        all_args["gmt_create"] = self.get_argument("gmt_create", None)
        all_args["gmt_payment"] = self.get_argument("gmt_payment", None)
        all_args["gmt_refund"] = self.get_argument("gmt_refund", None)
        all_args["gmt_close"] = self.get_argument("gmt_close", None)
        all_args["fund_bill_list"] = self.get_argument("fund_bill_list", None)
        sign_type = self.get_argument("sign_type", None)
        sign = self.get_argument("sign", None)

        if sign_type is None or sign_type.upper() != "RSA":
            print("sign_type is not rsa")
            self.response("failure")
            return

        args = {}
        for k, v in all_args.items():
            if v is not None:
                args[k] = v
        print(args)

        if args.get("app_id", "empty") != "2016090501851430":
            print("appid is wrong")
            self.response("failure")
            return

        if args.get("trade_status", "emtpy") != "TRADE_SUCCESS" and args.get("trade_status", "emtpy") != "TRADE_FINISHED":
            print("trade_status is not success", args.get("trade_status", "emtpy"))
            self.response("success")
            return

        # repeat order
        check = yield self.order_verify(args.get("notify_id"))
        if check != 0:
            print("order verify error")
            self.response("failure")
            return

        order_idd = args.get("trade_no")
        try:
            username, serverid, rid, sku, huodong_id, entry_id = args.get("body").rsplit("_", 5)
            serverid = int(serverid)
            rid = int(rid)
            huodong_id = int(huodong_id)
            entry_id = int(entry_id)
        except Exception as e:
            print(e)
            self.response("failure")
            return

        res, ecode = yield self.deliver_final(username, serverid, order_idd, rid, huodong_id, entry_id)
        if res == 0:
            self.response("success")
        else:
            self.response(ecode)

    @tornado.gen.coroutine
    def order_verify(self, notifyid):
        url = "https://mapi.alipay.com/gateway.do"
        args = {}
        args["service"] = "notify_verify"
        args["partner"] = "2088811685407064"
        args["notify_id"] = notifyid
        url += "?" + urllib.parse.urlencode(args)

        http_client = AsyncHTTPClient()
        try:
            response = yield http_client.fetch(url)
        except:
            raise tornado.gen.Return(-1)

        if response.error:
            raise tornado.gen.Return(-1)

        if response.body.decode('utf8') != "true":
            print(response.body)
            raise tornado.gen.Return(-1)

        raise tornado.gen.Return(0)
