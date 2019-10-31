# -*- coding: utf-8 -*-

import base_handler
import datetime
import urllib.parse
import tornado.gen
from tornado.httpclient import AsyncHTTPClient
import base_handler
import json
import base64


class BaseExHandler(base_handler.BaseHandler):
    client_id = None
    client_secret = None
    access_token = None
    refresh_token = None
    access_token_create_time = None
    access_token_expire_time = None

    def __init__(self, *args, **kwargs):
        super(base_handler.BaseHandler, self).__init__(*args, **kwargs)

    @tornado.gen.coroutine
    def verify_google(self, product_id, purchase_token, package_name):
        url_fmt = 'https://www.googleapis.com/androidpublisher/v2/applications/{packageName}/purchases/products/{productId}/tokens/{token}'
        url = url_fmt.format(packageName=package_name,
                             productId=product_id,
                             token=purchase_token)

        acctoken = yield self.get_token_google()
        params = {"access_token": acctoken}
        url += "?" + urllib.parse.urlencode(params)

        http_client = AsyncHTTPClient()
        try:
            respone = yield http_client.fetch(url)
        except Exception as e:
            print(e)
            raise tornado.gen.Return(-1)

        try:
            data = json.loads(respone.body)
        except Exception as e:
            print(e)
            raise tornado.gen.Return(-1)

        raise tornado.gen.Return(data)

    @tornado.gen.coroutine
    def get_token_google(self):
        self.need_get_access_token = False
        if self.access_token:
            now = datetime.datetime.now()
            if now >= self.access_token_expire_time:
                self.need_get_access_token = True
        else:
            self.need_get_access_token = True

        if not self.need_get_access_token:
            raise tornado.gen.Return(self.access_token)

        url = 'https://accounts.google.com/o/oauth2/token'
        headers = {"Content-type": "application/x-www-form-urlencoded"}
        body = dict(
            grant_type='refresh_token',
            client_id=self.client_id,
            client_secret=self.client_secret,
            refresh_token=self.refresh_token,
        )
        body = urllib.parse.urlencode(body)
        http_client = AsyncHTTPClient()
        try:
            rsp = yield http_client.fetch(url, method="POST",
                                          headers=headers,
                                          body=body)
            jdata = json.loads(rsp.body)
        except Exception as e:
            print(e)
            raise tornado.gen.Return("error")

        if 'access_token' in jdata:
            self.access_token = jdata['access_token']
            self.access_token_create_time = datetime.datetime.now()
            self.access_token_expire_time = self.access_token_create_time + datetime.timedelta(
                seconds=jdata['expires_in'] * 2 / 3
            )
            raise tornado.gen.Return(self.access_token)
        else:
            raise tornado.gen.Return("error")

    @tornado.gen.coroutine
    def verify_apple(self, code, username, serverid, rid, issandbox):
        url = "https://sandbox.itunes.apple.com/verifyReceipt" if issandbox else "https://buy.itunes.apple.com/verifyReceipt"
        res = -1
        orderid = ""
        product_id = ""
        buy_date_ms = ""
        errMsg = ""
        try:
            headers = {"Content-type": "application/json"}
            body = json.dumps({"receipt-data": code}).encode('utf8')
            http_client = AsyncHTTPClient()
            response = yield http_client.fetch(url, method="POST",
                                               headers=headers,
                                               body=body)
            content = json.loads(response.body)
            res = content['status']
            if res == 0:
                orderid = content["receipt"]["transaction_id"]
                product_id = content["receipt"]["product_id"]
                buy_date_ms = content["receipt"]["purchase_date_ms"]
        except Exception as e:
            print(e)
            errMsg = e
            res = -1

        raise tornado.gen.Return((res, orderid, product_id, buy_date_ms))
