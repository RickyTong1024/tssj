#!/usr/bin/env python
# coding=utf-8

import pymysql

server_host = "121.43.107.164"
sid_min = 1000
sid_max = 1500


def get_server():

    db = pymysql.connect(server_host, 'root', '1qaz2wsx@39299911', 'gttool')
    cur = db.cursor()
    cur.execute("SET NAMES utf8")
    sql = "select name, host, serverid, site from engine_server where serverid >= %s and serverid < %s order by serverid"
    param = (sid_min, sid_max,)
    cur.execute(sql, param)
    res = cur.fetchall()
    for i in range(len(res)):
        s = '<server http="http://%s:%s/" tcp="%s" port="%s" id="%s" name="%s" state="1"/>' % (
            res[i][1], 8000 + res[i][3] * 10, res[i][1], 9001 + res[i][3] * 10, res[i][2], res[i][0])
        print(s)

    db.close()


def main():
    get_server()


if __name__ == '__main__':
    main()
