#!/usr/bin/env python
# coding=utf-8

import zmq
import time
import threading
from rpc_pb2 import *
import tornado.ioloop

context = None

inlock = threading.Lock()
outlock = threading.Lock()

inmsgq = []
outmsgq = []


class push_thread(threading.Thread):
    def __init__(self, addr):
        threading.Thread.__init__(self)

        self.push_socket = context.socket(zmq.PUSH)
        self.push_socket.connect(addr)
        print("push_thread start")

    def run(self):
        global outmsgq

        while True:
            flag = True
            outlock.acquire()
            tout = list(outmsgq)
            outmsgq = []
            outlock.release()
            l = len(tout)
            if l > 0:
                flag = False
            for i in range(l):
                pck_str = tout.pop()
                self.push_socket.send(pck_str, zmq.NOBLOCK)

            # 队列空，休息0.01秒
            if flag == True:
                time.sleep(0.01)


class pull_thread(threading.Thread):
    def __init__(self, addr):
        threading.Thread.__init__(self)

        self.pull_socket = context.socket(zmq.PULL)
        self.pull_socket.bind(addr)
        print("pull_thread start")

    def run(self):
        global inmsgq

        while True:
            # 收返回消息
            message = self.pull_socket.recv()
            try:
                # 解析返回包
                inmsg = rpc()
                inmsg.ParseFromString(message)

                # 加入队列
                inlock.acquire()
                inmsgq.append(inmsg)
                inlock.release()
            except Exception as e:
                print(e)


class ioloop():
    def __init__(self):
        global context
        context = zmq.Context(1)
        self.thread1 = None
        self.thread2 = None
        self.name = ""
        self.num = 0

    @classmethod
    def instance(cls):
        if not hasattr(cls, "_instance"):
            cls._instance = cls()
        return cls._instance

    def read_msg(self):
        global inmsgq

        inlock.acquire()
        tin = list(inmsgq)
        inmsgq = []
        inlock.release()

        return tin

    def send_req_msg(self, op, msg):
        global outmsgq

        num = self.num
        self.num = self.num + 1
        rpc_msg = rpc()
        rpc_msg.type = REQUESST
        rpc_msg.req.name = self.name
        rpc_msg.req.id = num
        rpc_msg.req.opcode = op
        rpc_msg.req.msg = msg
        s = rpc_msg.SerializeToString()
        outlock.acquire()
        outmsgq.append(s)
        outlock.release()
        return num

    def send_rep_msg(self, rid, msg):
        global outmsgq

        rpc_msg = rpc()
        rpc_msg.type = RESPONSE
        rpc_msg.rep.name = self.name
        rpc_msg.rep.id = rid
        if msg != "":
            rpc_msg.rep.msg = msg
        s = rpc_msg.SerializeToString()
        outlock.acquire()
        outmsgq.append(s)
        outlock.release()

    def start(self, name, pushaddr, pulladdr):
        self.name = name
        self.thread1 = push_thread(pushaddr)
        self.thread1.start()
        self.thread2 = pull_thread(pulladdr)
        self.thread2.start()
