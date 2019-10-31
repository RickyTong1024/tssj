#!/usr/bin/env python
# coding=utf-8
import MySQLdb
import struct
import time

player_level = 10
player_login_time = 15
player_recharge = [0, 99999999]
# 选择服务器的范围
server_ranges = [0, 10000]
server_names = {}
usernames = ['zh4558599',]


def servers_list():
    global server_names
    db = MySQLdb.connect(user="root", passwd="1qaz2wsx@39299911", db="gttool", host="121.43.107.164", charset='utf8')
    cur = db.cursor()
    sql = "select serverid, name from engine_server"
    cur.execute(sql)
    serverids = cur.fetchall()
    for serverid in serverids:
        server_names[str(serverid[0])] = serverid[1]
    sql = "select host, site from engine_server where merge = '0' and serverid >= %s and serverid <= %s"
    param = (server_ranges[0], server_ranges[1],)
    cur.execute(sql, param)
    servers = cur.fetchall()
    num = 0
    for server in servers:
        print "start", server[0], server[1]
        seek_player(server[0], server[1])
        num = num + 1
        print "end", num

    db.close()


player_data = {}


def seek_player(host, site):
    global player_data
    try:
        db = MySQLdb.connect(user="root", passwd="1qaz2wsx@39299911", db='tsjh' + str(site), host=host, charset='utf8')
        cur = db.cursor()
        ttime = int(time.time() * 1000)
        sql = "select player_t.name, player_t.level, player_t.last_login_time, player_t.total_recharge, acc_t.username, acc_t.serverid from player_t left join acc_t on player_t.guid = acc_t.guid where player_t.level >= %s and player_t.last_login_time <= %s and player_t.total_recharge >= %s and player_t.total_recharge <= %s"
        param = (player_level, ttime, player_recharge[0], player_recharge[1])
        cur.execute(sql, param)
        players = cur.fetchall()
        for player in players:
            name = player[0]
            level = player[1]
            last_login_time = player[2]
            total_recharge = player[3]
            username = player[4]
            serverid = player[5]

            if username in player_data:
                if player_data[username]['level'] < level:
                    player_data[username]['name'] = name
                    player_data[username]['level'] = level
                    player_data[username]['last_login_time'] = last_login_time
                    player_data[username]['total_recharge'] = total_recharge
                    player_data[username]['serverid'] = serverid
            else:
                player_data[username] = {}
                player_data[username]['name'] = name
                player_data[username]['level'] = level
                player_data[username]['last_login_time'] = last_login_time
                player_data[username]['total_recharge'] = total_recharge
                player_data[username]['serverid'] = serverid
        db.close()
    except:
        pass


user_data = {}


def seek_account():
    f = open('user.txt', 'r')
    for s in f.readlines():
        ss = s.split('@|@')
        username = ss[0]
        if username in player_data:
            user_data[username] = {}
            user_data[username]['name'] = player_data[username]['name']
            user_data[username]['level'] = player_data[username]['level']
            user_data[username]['last_login_time'] = player_data[username]['last_login_time']
            user_data[username]['total_recharge'] = player_data[username]['total_recharge']
            user_data[username]['serverid'] = player_data[username]['serverid']

    f.close()


def display_server_name(serverid):
    return server_names[serverid]


def display_time(timenum):
    t = time.time() - int(timenum / 1000)
    t = t / 86400 + 1
    return "%d" % (t,)


def main():
    copy_user()
    seek_wanted()


def seek_wanted():
    servers_list()
    print "len player_data", len(player_data)
    seek_account()
    print "len user_data", len(user_data)

    f = open('out.txt', 'w')
    s = "用户名\t角色名\t等级\t离线天数\t充值钻石\t服务器id\n"
    f.write(s)
    for username in user_data:
        s = "%s\t%s\t%d\t%s\t%d\t%s\n" % (username,
                                          user_data[username]['name'], user_data[username]['level'], display_time(
                                              user_data[username]['last_login_time']),
                                          user_data[username]['total_recharge'], display_server_name(user_data[username]['serverid']),)
        f.write(s.encode("utf-8"))
    f.close()


def copy_user():
    db = MySQLdb.connect(user="root", passwd="1qaz2wsx@39299911", db="tsuser", host="121.40.80.198", charset='utf8')
    cur = db.cursor()

    sql = "select user.username from user where user.username = %s"
    param = usernames
    cur.execute(sql, param)
    users = cur.fetchall()

    f = open('user.txt', 'w')
    for user in users:
        username = user[0]
        s = "%s@|@\n" % (username)
        f.write(s)
    f.close()
    db.close()


if __name__ == '__main__':
    main()
# 先在表中 查找 是否含有 这个用户名
    # copy_user()
