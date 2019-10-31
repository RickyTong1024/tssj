#!/usr/bin/env python  
#coding=utf-8

import tornado.httpserver
import tornado.ioloop
import tornado.web
import pymysql
import json

class set_handler(tornado.web.RequestHandler):
    @tornado.web.asynchronous
    def post(self):
        # 收到包，取出地址信息
        token = self.get_body_argument("token")
        serverid = self.get_body_argument("serverid")
        level = self.get_body_argument("level")

        print("token:%s    serverid:%s    level:%s" % (token, serverid, level))

        l = len(token)
        if l < 6 or l > 64:
            self.write("-1")
            self.finish()
            return

        l = len(serverid)
        if l < 1 or l > 10:
            self.write("-1")
            self.finish()
            return

        l = len(level)
        if l < 1 or l > 10:
            self.write("-1")
            self.finish()
            return
        
        serverid = int(serverid)
        level = int(level)
        self.application.ping()
        db = self.application.db
        cur = db.cursor()
        sql = "select id, serverid from storage where token = %s order by level asc, id asc"
        param = (token,)
        cur.execute(sql, param)
        res = cur.fetchall()

        flag = False
        for i in range(len(res)):
            if res[i][1] == serverid:
                flag = True
                sql = "update storage set level = %s where id = %s"
                param = (level, res[i][0],)
                cur.execute(sql, param)
                break
        if not flag:
            if len(res) >= 2:
                sql = "delete from storage where id = %s"
                param = (res[0][0],)
                cur.execute(sql, param)

            sql = "insert into storage (token, serverid, level) values (%s, %s, %s)"
            param = (token, serverid, level,)
            cur.execute(sql, param)
        
        self.write("0")
        self.finish()

class get_handler(tornado.web.RequestHandler):
    @tornado.web.asynchronous
    def post(self):
        # 收到包，取出地址信息
        token = self.get_body_argument("token")

        print("token:%s" % (token))

        l = len(token)
        if l < 6 or l > 64:
            self.write("-1")
            self.finish()
            return
        
        self.application.ping()
        db = self.application.db
        cur = db.cursor()
        sql = "select serverid, level from storage where token = %s order by level asc, id asc"
        param = (token,)
        cur.execute(sql, param)
        res = cur.fetchall()

        data = []
        for i in range(len(res)):
            data.append({"serverid":res[i][0], "level":res[i][1]})
        data = json.dumps(data)
        self.write(data)
        self.finish()

class Application(tornado.web.Application):  
    def __init__(self):  
        handlers = [
            (r"/set", set_handler),
            (r"/get", get_handler),
        ]
        tornado.web.Application.__init__(self, handlers)
        self.db = None
        
    def create_db(self):
        self.db = pymysql.connect(user='root' ,passwd='root' ,db='tsstorage', host='127.0.0.1')
        self.db.autocommit(1)
        
    def ping(self):
        try:
            self.db.ping()
        except Exception as e:
            self.create_db()

def main():
    http_server = tornado.httpserver.HTTPServer(Application())
    http_server.listen(10004)
    print('Welcome to the machine...')
    tornado.ioloop.IOLoop.instance().start()
            
if __name__ == '__main__':
    main()

