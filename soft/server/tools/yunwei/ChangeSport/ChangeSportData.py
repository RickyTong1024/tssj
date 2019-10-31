#!/usr/bin/env python  
#coding=utf-8
import MySQLdb
import struct
from name_en import gname

def seek_servers():    
    db = MySQLdb.connect(user="root", passwd="1qaz2wsx@39299911", db="tsjh0", host="47.75.28.116", charset='utf8')
    cur = db.cursor()
    cur.execute("SET NAMES utf8")
    sql = "select guid, player_name, player_isnpc from sport_list_t"
    cur.execute(sql)
    res = cur.fetchall()
    for i in range(len(res)):
        if res[i][0] & 0x00000FFFFFFFFFFF == 0:
            new_player_name = change_res(res[i])
            sql = "update sport_list_t set player_name = %s"
            param = (new_player_name,)
            cur.execute(sql, param)
    db.close()

def change_res(res):
    names = []
    player_names = res[1]
    len_player_names, player_names = struct.unpack('i%ds' % (len(player_names) - 4), player_names)
    for i in range(len_player_names):
        len_name, player_names = struct.unpack('i%ds' % (len(player_names) - 4), player_names)
        name, player_names = struct.unpack('%ds%ds' % (len_name, len(player_names) - len_name), player_names)
        names.append(name)

    isnpcs = []
    player_isnpcs = res[2]
    len_player_isnpcs, player_isnpcs = struct.unpack('i%ds' % (len(player_isnpcs) - 4), player_isnpcs)
    for i in range(len_player_isnpcs):
        player_isnpc, player_isnpcs = struct.unpack('i%ds' % (len(player_isnpcs) - 4), player_isnpcs)
        isnpcs.append(player_isnpc)

    tlen = len_player_names
    if len_player_names > len_player_isnpcs:
        tlen = len_player_isnpcs

    for i in range(tlen):
        if isnpcs[i]  == 1:
            names[i] = gname.get_random_name()
    
    new_player_names = struct.pack('i', len_player_names)
    for i in range(len_player_names):
        len_name = len(names[i])
        player_name = struct.pack('i%ds' % len_name, len_name, names[i])
        new_player_names = new_player_names + player_name
    
    return new_player_names
    
        
def main():
    seek_servers()
    
if __name__ == '__main__':
    main()
