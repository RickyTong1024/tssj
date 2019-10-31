# -*- coding: utf-8 -*-

import tornado.web
import tornado.gen
import base_ex_handler


class NotifyHandler(base_ex_handler.BaseExHandler):
    def __init__(self, *args, **kwargs):
        super(base_ex_handler.BaseExHandler, self).__init__(*args, **kwargs)
        self.client_id = '527958469177-8o48vvh4ah7b76n3hjbbrlg6n4grutgh.apps.googleusercontent.com'
        self.client_secret = '67l4QE7FSkFc3klfEAR9vHRz'
        self.refresh_token = '1/Z82D85-5vXIz2t0itz12Xm-DXTP8tkvUehBqN-ObRL4'
        self.package_name = 'com.calling.angel'

    @tornado.web.asynchronous
    @tornado.gen.coroutine
    def post(self):
        self.dbg_print()

        packageName = self.get_argument("packageName", None)
        productId = self.get_argument("productId", None)
        purchase_token = self.get_argument("purchase_token", None)

        if not packageName or not productId or not purchase_token:
            self.response("one of the packageName , productId, purchase_token  is error ")
            print("one of the packageName , productId, purchase_token  is error ")
            return

        if packageName != self.package_name:
            self.response("the packageName wrong")
            print("the packageName wrong")
            return

        gdata = yield self.verify_google(productId, purchase_token, packageName)

        if not isinstance(gdata, dict):
            self.response("the gata is not contain dict")
            print("the gata is not contain dict")
            return

        if gdata.get("purchaseState", None) != 0:
            self.response("gdata purchaseState is null")
            print("gdata purchaseState is null")
            return

        if gdata.get("orderId", None) is None:
            self.response("gdata orderId si null")
            print("gdata orderId si null")
            return

        order_idd = str(gdata.get("orderId"))
        try:
            username, serverid, rid, huodong_id, entry_id = gdata.get("developerPayload").rsplit("_", 4)
            rid = int(rid)
            huodong_id = int(huodong_id)
            entry_id = int(entry_id)
            if self.application.recharge.get_iosid(rid) != int(productId):
                print("invalid productid", rid, productId)
                self.response("iosid error")
                print("iosid error")
                return
        except Exception as e:
            print(e)
            self.response("iosid  try error")
            print("iosid  try error")
            return

        res, ecode = yield self.deliver_final(username, serverid, order_idd, rid, huodong_id, entry_id)
        if res == 0:
            self.response("success")
        else:
            self.response(ecode)
