# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
import base_handler
import base_ex_handler
import time


class NotifyHandler(base_ex_handler.BaseExHandler):
    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def post(self):
        username = self.get_argument("username", None)
        serverid = self.get_argument("serverid", None)
        rid = self.get_argument("rid", None)
        code = self.get_argument("code", None)
        huodong_id = self.get_argument("huodong_id", None)
        entry_id = self.get_argument("entry_id", None)

        if not username or not serverid or not rid or not code:
            self.response("error")
            return

        verify_result = yield self.verify_apple(code, username, serverid, rid, False)
        if verify_result[0] == 21007:
            verify_result = yield self.verify_apple(code, username, serverid, rid, True)

        if verify_result[0] != 0:
            self.response("error")
            return
        try:
            rid = int(rid)
            serverid = int(serverid)
            huodong_id = int(huodong_id)
            entry_id = int(entry_id)
            product_id = int(verify_result[2])
            buy_date_ms = int(verify_result[3])
            if self.application.recharge.get_iosid(rid) != product_id:
                self.response("error")
                return
            if int(time.time() * 1000) > buy_date_ms + 3600000:
                print("invalid buy_date_ms", buy_date_ms)
                self.response("error")
                return
        except Exception as e:
            print(e)
            self.response("error")
            return

        order_idd = verify_result[1]
        res, ecode = yield self.deliver_final(username, serverid, order_idd, rid, huodong_id, entry_id)
        if res == 0:
            self.response("success")
        else:
            self.response(ecode)
