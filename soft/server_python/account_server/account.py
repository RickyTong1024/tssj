#!/usr/bin/env python
# coding=utf-8

import tornado.httpserver
import tornado.ioloop
import tornado.web
import pymysql
import json
import config
import smtplib
from email.mime.text import MIMEText
from email.utils import formataddr
import re
import json


class base_handler(tornado.web.RequestHandler):
    _argl = []
    _args = {}

    def dbug(self):
        print(self.__class__.__name__, self.request.body)

    def check_args(self):
        for i in range(len(self._argl)):
            name = self._argl[i]
            arg = self.get_body_argument(name)
            self._args[name] = arg
            if name == "username":
                l = len(arg)
                if l < 6 or l > 128:
                    # 长度问题
                    print("len error -3")
                    self.response(-3)
                    return False

            if name == "old_username":
                l = len(arg)
                if l > 128:
                    # 长度问题
                    print("len error -3")
                    self.response(-3)
                    return False

            if name == "password":
                l = len(arg)
                if l < 6 or l > 14:
                    # 长度问题
                    print("len error -3")
                    self.response(-3)
                    return False

            if name == "old_password":
                l = len(arg)
                if l > 14:
                    # 长度问题
                    print("len error -3")
                    self.response(-3)
                    return False

            if name == "mail":
                if re.match(r'^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$', arg) == None:
                    # 邮箱格式问题
                    print("email error")
                    self.response(-4)
                    return False
        return True

    def get_db(self):
        self.application.ping()
        return self.application.db

    def make_token(self, pid):
        token = str(pid)
        while len(token) < 8:
            token = "0" + token
        token = "ns" + token
        return token

    def response(self, res, param=None):
        data = {"res": res}
        if param:
            data.update(param)
        data = json.dumps(data)
        self.write(data)
        self.finish()


class reg_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["username", "password"]
        if not self.check_args():
            return

        cur = self.get_db().cursor()
        sql = "select pid, password from user where username = %s"
        param = (self._args["username"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 1:
            # 未知问题
            print("unknow error -2")
            self.response(-2)
            return
        elif len(res) == 0:
            sql = "insert into user (username, password, is_reg) values (%s, %s, 0)"
            param = (self._args["username"], self._args["password"],)
            cur.execute(sql, param)
            pid = cur.lastrowid
            token = self.make_token(pid)
            # 注册成功
            print("register suc 1")
            self.response(1, {"token": token})
            return
        else:
            # 已存在账号
            print("has username error -1")
            self.response(-1)
            return


class login_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["username", "password"]
        if not self.check_args():
            return

        cur = self.get_db().cursor()
        sql = "select pid, password, is_reg from user where username = %s"
        param = (self._args["username"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 1:
            # 未知问题
            print("unknow error -2")
            self.response(-2)
            return
        elif len(res) == 0:
            # 未注册用户
            print("register error -4")
            self.response(-4)
            return
        else:
            if res[0][1] != self._args["password"]:
                # 密码错误
                print("password error -1")
                self.response(-1)
                return

        # 登陆成功
        print("login suc 0")
        token = self.make_token(res[0][0])
        is_reg = res[0][2]
        self.response(0, {"token": token, "is_reg": is_reg})


class chpwd_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["username", "old_password", "password"]
        if not self.check_args():
            return

        cur = self.get_db().cursor()
        sql = "select password from user where username = %s"
        param = (self._args["username"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 1:
            # 未知问题
            print("unknow error -2")
            self.response(-2)
            return
        elif len(res) == 0:
            # 未注册用户
            print("register error -4")
            self.response(-4)
            return
        else:
            if res[0][0] != self._args["old_password"]:
                # 密码错误
                print("password error -1")
                self.response(-1)
                return
            sql = "update user set password = %s where username = %s"
            param = (self._args["password"], self._args["username"], )
            cur.execute(sql, param)
        # 成功
        print("chpwd suc 0")
        self.response(0)


class bind_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["username", "password", "mail", "old_username", "old_password"]
        if not self.check_args():
            return

        cur = self.get_db().cursor()
        sql = "select mail from user where mail = %s"
        param = (self._args["mail"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 0:
            print("mail registered -5")
            # 邮箱已注册
            self.response(-5)
            return

        sql = "select password from user where username = %s"
        param = (self._args["username"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 0:
            # 账号已注册
            print("user register -7")
            self.response(-7)
            return

        if self._args["old_username"] == "":
            sql = "insert into user (username, password, mail, is_reg) values(%s, %s, %s, 1)"
            param = (self._args["username"], self._args["password"], self._args["mail"],)
            cur.execute(sql, param)
            pid = cur.lastrowid
            token = self.make_token(pid)
            # 注册成功
            print("register suc 1")
            self.response(1, {"token": token})
            return
        else:
            sql = "select password, is_reg from user where username = %s"
            param = (self._args["old_username"],)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) > 1:
                # 未知问题
                print("unknow error -2")
                self.response(-2)
                return
            elif len(res) == 0:
                sql = "insert into user (username, password, mail, is_reg) values(%s, %s, %s, 1)"
                param = (self._args["username"], self._args["password"], self._args["mail"],)
                cur.execute(sql, param)
                pid = cur.lastrowid
                token = self.make_token(pid)
                # 注册成功
                print("register suc 1")
                self.response(1, {"token": token})
                return
            else:
                if res[0][1] != 0:
                    # 账号已注册
                    print("user register -7")
                    self.response(-7)
                    return
                elif res[0][0] != self._args["old_password"]:
                    # 密码错误
                    print("password error -1")
                    self.response(-1)
                    return
                else:
                    sql = "update user set username = %s, password = %s, mail = %s, is_reg = 1 where username = %s"
                    param = (self._args["username"], self._args["password"],
                             self._args["mail"], self._args["old_username"],)
                    cur.execute(sql, param)
                    print("register suc 2")
                    self.response(2)
                    return


class recovery_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["mail", "lang"]
        if not self.check_args():
            return

        cur = self.get_db().cursor()
        sql = "select username, password from user where mail = %s"
        param = (self._args["mail"],)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 1:
            # 未知问题
            print("unknow error -2")
            self.response(-2)
            return
        elif len(res) == 0:
            # 邮箱未注册
            print("email not register -5")
            self.response(-4)
            return
        else:
            user = res[0][0]
            passwd = res[0][1]
            lang = 0
            if self._args["lang"] != "0":
                lang = 1
            try:
                text = config.mail_text[lang] % (user, passwd)
                msg = MIMEText(text, 'plain', 'utf-8')
                msg['from'] = formataddr(["yymoon", config.mail_sender])
                msg['To'] = formataddr(["FK", self._args["mail"]])
                msg['Subject'] = config.mail_title[lang]
                server = smtplib.SMTP_SSL("smtp.exmail.qq.com", 465, timeout=2)
                server.login(config.mail_sender, config.mail_pass)
                server.sendmail(config.mail_sender, [self._args["mail"], ], msg.as_string())
                server.quit()
                print("email send success")
                self.response(0)
                return
            except Exception as e:
                print(e)
                print("email send error")
                self.response(-1)
                return


class check_handler(base_handler):
    @tornado.web.asynchronous
    def post(self):
        self.dbug()
        self._argl = ["token", "password"]
        if not self.check_args():
            return

        token = self._args["token"]
        token = token[2:]
        token = int(token)
        cur = self.get_db().cursor()
        sql = "select password from user where pid = %s"
        param = (token,)
        cur.execute(sql, param)
        res = cur.fetchall()
        if len(res) > 1:
            # 未知问题
            print("unknow error -2")
            self.response(-2)
            return
        elif len(res) == 0:
            print("unknow username -1")
            self.response(-1)
            return
        else:
            if res[0][0] != self._args["password"]:
                print("password error -4")
                self.response(-4)
                return
            else:
                print("check success")
                self.response(0)
                return


class Application(tornado.web.Application):
    def __init__(self):
        handlers = [
            (r"/reg", reg_handler),
            (r"/login", login_handler),
            (r"/chpwd", chpwd_handler),
            (r"/bind", bind_handler),
            (r"/recovery", recovery_handler),
            (r"/check", check_handler),
        ]
        tornado.web.Application.__init__(self, handlers)
        self.db = None

    def create_db(self):
        self.db = pymysql.connect(user='root', passwd='root', db='tsuser', host='127.0.0.1')
        self.db.autocommit(1)

    def ping(self):
        try:
            self.db.ping()
        except Exception as e:
            self.create_db()


def main():
    http_server = tornado.httpserver.HTTPServer(Application())
    http_server.listen(10001)
    print('Welcome to the machine...')
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    main()
