#!/usr/bin/env python
# coding=utf-8

import sys
sys.path.append("../common")
import config
import pull
from rpc_pb2 import *
import tornado.httpserver
import tornado.ioloop
import tornado.web
import struct
import pyDes
import time


class self_handler(tornado.web.RequestHandler):
    connection_closed = False
    num = 0
    tmout_handle = None

    @tornado.web.asynchronous
    def post(self):
        # 收到包，取出地址信息
        uri = self.request.uri
        op = uri
        msg = self.request.body
        if len(op) >= 1:
            op = op[1:]
        else:
            self.write_info(-2, "argv error")
            return

        if not op.isdigit():
            self.write_info(-2, "argv error")
            return

        op = int(op)
        if op < 19900:
            try:
                msg = self.decode(msg)
            except Exception as e:
                self.write_info(-2, "argv error")
                return
        print(op, msg)
        self.num = pull.ioloop.instance().send_req_msg(op, msg)
        self.application.add_sh(self.num, self)
        self.tmout_handle = tornado.ioloop.IOLoop.instance().add_timeout(time.time() + config.time_out, callback=self.time_out)

    def time_out(self):
        if self.connection_closed:
            return

        self.tmout_handle = None
        self.write_info(-1, "timerout")

    def on_connection_close(self):
        self.connection_closed = True

    def write_info(self, res, text):
        if self.connection_closed:
            return

        if self.tmout_handle:
            tornado.ioloop.IOLoop.instance().remove_timeout(self.tmout_handle)

        self.application.del_sh(self.num)
        if type(text) == str:
            text = text.encode('utf-8')
        fm = "i%ds" % len(text)
        text1 = struct.pack(fm, res, text)

        self.write(text1)
        self.finish()

    def decode(self, info):
        k = pyDes.des('tsjhtsjh', pyDes.CBC, '51478543', pad=None, padmode=pyDes.PAD_PKCS5)
        info = k.decrypt(info, padmode=pyDes.PAD_PKCS5)
        return info


class Application(tornado.web.Application):
    def __init__(self):
        handlers = [
            (r"/.*", self_handler),
        ]
        tornado.web.Application.__init__(self, handlers)
        self.shq = {}

    def add_sh(self, num, sh):
        self.shq[num] = sh

    def del_sh(self, num):
        if num in self.shq:
            del self.shq[num]

    def do_sh(self):
        tin = pull.ioloop.instance().read_msg()
        for i in range(len(tin)):
            stin = tin[i]
            num = stin.rep.id
            if num in self.shq:
                code = stin.rep.error.code
                msg = stin.rep.msg
                if code != 0:
                    msg = stin.rep.error.text.encode('utf-8')
                self.shq[num].write_info(code, msg)
        tornado.ioloop.IOLoop.instance().add_timeout(time.time() + 0.1, callback=self.do_sh)


def main():
    pull.ioloop.instance().start(config.myname, config.push_addr, config.pull_addr)
    app = Application()
    http_server = tornado.httpserver.HTTPServer(app)
    http_server.listen(config.port)
    tornado.ioloop.IOLoop.instance().add_timeout(time.time() + 0.1, callback=app.do_sh)
    print('Welcome to the machine...')
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    main()
