# -*- coding: utf-8 -*-

import tornado.httpserver
import tornado.ioloop
import tornado.web
from Crypto.PublicKey import RSA
from Crypto.Signature import PKCS1_v1_5 as pk
from Crypto.Hash import SHA
import base64
import urllib.parse


class sign_handler(tornado.web.RequestHandler):
    connection_closed = False
    db = None

    @tornado.web.asynchronous
    def post(self):
        body = self.request.body
        h = SHA.new(body)
        signer = pk.new(self.application.rsa_pri)
        signn = signer.sign(h)
        signn = base64.b64encode(signn)
        signn = urllib.parse.quote(signn, "")
        self.write(signn)
        self.finish()

    def on_connection_close(self):
        self.connection_closed = True
