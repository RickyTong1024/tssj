# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
from tornado.httpclient import AsyncHTTPClient
import base_handler
import json

key = '2:s85wUBznpwbAUJqjSOL0uuCLWYkGkjW6Q9Awte4YEA8CQ8kQJqVoAdGU8DYspAs_:uAvFJgYSr55MN_0-K9nUoQ=='
proid = 'com.amazon.iapsamplev2.gold_medal'


class NotifyHandler(base_handler.BaseHandler):
    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def post(self):
        code = self.get_argument("code", None)
        userid = self.get_argument("userid", None)
        order_idd = self.get_argument("recepitid", None)

        if not code:
            self.response("error")
            return

        username, serverid, rid, ios_id = code.rsplit("_", 3)
        print(userid)
        print(order_idd)

        verify_result = yield self.verify(key, userid, order_idd, False)
        print("verify_result:", verify_result)
        if not isinstance(verify_result, dict):
            self.response("result error")
            return
        if verify_result.get("betaProduct", None) != False:
            self.response("betaproduct error")
            return

        try:
            rid = int(rid)
            if self.application.recharge.get_iosid(rid) != int(ios_id):
                print("invalid productid", rid, ios_id)
                self.response("iosid error")
                return
        except Exception as e:
            print(e)
            self.response("error")
            return

        res, ecode = yield self.deliver_final_old(username, serverid, order_idd, rid)
        if res == 0:
            self.response("success")
        else:
            self.response(ecode)

    @tornado.gen.coroutine
    def verify(self, developer_secret, user_id, receipt_id, issandbox):
        if issandbox:
            url_fmt = "http://localhost:8089/RVSSandbox/version/1.0/verifyReceiptId/developer/developerSecret/user/{userId}/receiptId/{receiptId}"
            url = url_fmt.format(userId=user_id,
                                 receiptId=receipt_id)
        else:
            url_fmt = 'http://appstore-sdk.amazon.com/version/1.0/verifyReceiptId/developer/{developerSecret}/user/{userId}/receiptId/{receiptId}'
            url = url_fmt.format(developerSecret=developer_secret,
                                 userId=user_id,
                                 receiptId=receipt_id)
        http_client = AsyncHTTPClient()
        try:
            respone = yield http_client.fetch(url)
            print("fetch")
        except Exception as e:
            print("http error")
            print(e)
            raise tornado.gen.Return(-1)

        try:
            data = json.loads(respone.body)
        except Exception as e:
            print("json error")
            print(e)
            raise tornado.gen.Return(-1)

        raise tornado.gen.Return(data)
