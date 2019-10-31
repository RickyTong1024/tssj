# -*- coding: utf-8 -*-

import sys
import rsa
from Crypto.PublicKey import RSA
from Crypto.Signature import PKCS1_v1_5 as pk
from Crypto.Hash import SHA
sys.path.append("./handlers")
sys.path.append("./help")
import pymysql
import tornado.httpserver
import tornado.ioloop
import config_help
import google_pay_handler
import apple_pay_handler
import amazon_pay_handler
import yijie_pay_handler
import sign_handler
import ali_pay_handler
import wechat_pay_handler
import bt_pay_handler


class Application(tornado.web.Application):
    def __init__(self):
        handlers = [
            (r"/notifyapple", apple_pay_handler.NotifyHandler),         # 苹果
            (r"/notifyamazon", amazon_pay_handler.NotifyHandler),       # 亚马逊
            (r"/notifygoogle", google_pay_handler.NotifyHandler),       # 谷歌
            (r"/notifyyijie", yijie_pay_handler.NotifyHandler),         # 易接
            (r"/notifysign", sign_handler.sign_handler),                # 支付宝签名
            (r"/notifyali", ali_pay_handler.NotifyHandler),             # 支付宝
            (r"/notifywechat", wechat_pay_handler.NotifyHandler),       # 微信
            (r"/notifybt", bt_pay_handler.NotifyHandler),               # BT版本
        ]
        tornado.web.Application.__init__(self, handlers)

        self.recharge = config_help.Recharge("./config/")
        self.recharge.parse()

        self.db = None
        self.gttool = None
        with open("./config/rsa_public_key.pem") as f:
            p = f.read()
            self.alipay_pubkey = rsa.PublicKey.load_pkcs1_openssl_pem(p)

        with open('./config//rsa_private_key_pkcs8.pem') as privatefile:
            p = privatefile.read()
            self.rsa_pri = RSA.importKey(p)

    def create_db(self):
        self.db = pymysql.connect(user='root', passwd='root', db='tsjhpay', host='127.0.0.1')
        self.db.autocommit(1)

    def ping(self):
        try:
            self.db.ping()
        except Exception as e:
            self.create_db()

    def create_gttool_db(self):
        self.gtool = pymysql.connect(user='root', passwd='root', db='gttool', host='127.0.0.1')
        self.gtool.autocommit(1)

    def gttool_ping(self):
        try:
            self.gtool.ping()
        except Exception as e:
            self.create_gttool_db()


def main():
    http_server = tornado.httpserver.HTTPServer(Application())
    http_server.listen(10002)
    print('Welcome to the machine...')
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    main()
