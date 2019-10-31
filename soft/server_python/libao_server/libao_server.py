#!/usr/bin/env python
# coding=utf-8

import sys
import importlib
import tornado.httpserver
import tornado.ioloop
import tornado.web
import pymysql
import struct
import json


class libao_handler(tornado.web.RequestHandler):
    connection_closed = False
    db = None

    def post(self):
        # 收到包，取出地址信息
        data = json.loads(self.request.body)
        code = data["code"]
        pt = data["pt"]
        use = '1'
        if "use" in data:
            use = data["use"]
        print(code, pt, use)
        res, reward, pc, chongzhi, wexin = self.check(code, pt, use)
        ds = {"res": res, "reward": reward, "pc": pc, "chongzhi": chongzhi}
        self.write(json.dumps(ds))
        self.finish()

    def check(self, code, pt, use):
        codeq = code[:4]
        self.application.ping()
        cur = self.application.db.cursor()
        sql = "select code, pc, type, value1, value2, value3, chongzhi, gongxiang from libao_type where code = '%s'" % (
            codeq)
        cur.execute(sql)
        data = cur.fetchall()
        print(data)
        print(len(data))
        if len(data) != 1:
            print("没有该礼包码")
            return -1, [], 0, 0, 0
        data = data[0]
        pc = data[1]
        chongzhi = data[6]
        types = data[2]
        gongxiang = data[7]
        l, types = struct.unpack('i%ds' % (len(types) - 4), types)
        type_arr = []
        for i in range(l):
            j, types = struct.unpack('i%ds' % (len(types) - 4), types)
            type_arr.append(j)
        value1 = data[3]
        l, value1 = struct.unpack('i%ds' % (len(value1) - 4), value1)
        value1_arr = []
        for i in range(l):
            j, value1 = struct.unpack('i%ds' % (len(value1) - 4), value1)
            value1_arr.append(j)
        value2 = data[4]
        l, value2 = struct.unpack('i%ds' % (len(value2) - 4), value2)
        value2_arr = []
        for i in range(l):
            j, value2 = struct.unpack('i%ds' % (len(value2) - 4), value2)
            value2_arr.append(j)
        value3 = data[5]
        l, value3 = struct.unpack('i%ds' % (len(value3) - 4), value3)
        value3_arr = []
        for i in range(l):
            j, value3 = struct.unpack('i%ds' % (len(value3) - 4), value3)
            value3_arr.append(j)
        reward = []
        for i in range(len(type_arr)):
            reward.append([type_arr[i], value1_arr[i], value2_arr[i], value3_arr[i]])

        sql = "select code, used, wexin from libao where code = '%s'" % (code)
        cur.execute(sql)
        data = cur.fetchall()
        if len(data) != 1:
            print("没有该礼包码")
            return -1, [], 0, 0, 0
        data = data[0]
        if not gongxiang:
            if data[1] == 1:
                print("已被使用")
                return -3, [], 0, 0, 0
        if use == '1':
            sql = "update libao set used = 1 where code = '%s'" % (code)
            cur.execute(sql)
        if use == '2':
            sql = "update libao set wexin = 1 where code = '%s'" % (code)
            cur.execute(sql)
        print(reward)
        return 0, reward, pc, chongzhi, data[2]

    def on_connection_close(self):
        self.connection_closed = True

class Application(tornado.web.Application):
    def __init__(self):
        handlers = [
            (r"/libao", libao_handler)
        ]
        tornado.web.Application.__init__(self, handlers)

    def create_db(self):
        self.db = pymysql.connect(user='root', passwd='root', db='tslibao', host='127.0.0.1')
        self.db.autocommit(1)

    def ping(self):
        try:
            self.db.ping()
        except Exception as e:
            self.create_db()

def main():
    http_server = tornado.httpserver.HTTPServer(Application(), xheaders=True)
    http_server.listen(10003)
    print('Welcome to the machine...')
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    main()
