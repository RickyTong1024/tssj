# -*- coding: utf-8 -*-
import tornado.httpserver
import tornado.ioloop
import tornado.web
from tornado.options import define, options
from twilio.rest import Client

define('port', default=10006)


class VoiceHandler(tornado.web.RequestHandler):
    def get(self):
        client = Client(self.application.account_sid,
                        self.application.auth_token)

        try:
            call = client.calls.create(
                url='http://demo.twilio.com/docs/voice.xml',
                to='+8615221569451',
                from_='+12019039130'
            )
        except Exception as e:
            self.write(e)
            return

        self.write(call.sid)


class Application(tornado.web.Application):
    def __init__(self):
        handlers = [
            (r'/voice', VoiceHandler),
        ]
        tornado.web.Application.__init__(self, handlers)

        self.account_sid = 'ACb010924570237ed71302c43b87d7751f'
        self.auth_token = 'a831afe99f8567a8ed05360f15114051'


def main():
    tornado.options.parse_command_line()
    http_server = tornado.httpserver.HTTPServer(Application())
    http_server.listen(options.port)
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    main()
