#!/usr/bin/env python
# coding=utf-8
import pymysql
import struct
#from name_en import gname


def seek_servers():
    db = pymysql.connect(user="root", passwd="1qaz2wsx@39299911", db="tsjh0", host="47.244.100.28", charset='utf8')
    cur = db.cursor()
    sql = "select guid, player_guid from sport_list_t"
    cur.execute(sql)
    res = cur.fetchall()
    print(type(res[1]))
    for i in range(len(res)):
        if res[i][0] & 0x00000FFFFFFFFFFF == 0:
            # pass
            nalflag = change_res(res[i])
            sql = "update sport_list_t set nalflag = %s"
            param = (nalflag,)
            cur.execute(sql, param)
    db.commit()


def change_res(res):
    player_guid = res[1]
    # len_player_guid, player_guid = struct.unpack('i%ds' % (len(player_guid) - 4), player_guid)
    len_player_guid,  = struct.unpack('i', player_guid[:4])
    print(len_player_guid)

    nalflag = b''
    # nalflag = struct.pack('i', 0)
    for i in range(len_player_guid):
        flag = struct.pack('i', 0)
        nalflag = nalflag + flag
    print(nalflag)
    # nalflag = nalflag - struct.pack('i', 0)

    return nalflag


def main():
    seek_servers()


if __name__ == '__main__':
    main()
