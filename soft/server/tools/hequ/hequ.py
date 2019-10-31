#!/usr/bin/env python  
#coding=utf-8

import MySQLdb
import struct
import time

he = [3886, 3887]

server1 = []
server2 = []

guids1 = []
guids2 = []

def get_server():
    global server1
    global server2

    db = MySQLdb.connect(user='root' ,passwd='1qaz2wsx@39299911' ,db='gttool', host='120.131.3.168', charset="utf8")
    cur = db.cursor()
    cur.execute("SET NAMES utf8");
    
    sql = "select host, site from engine_server where serverid = %s"
    param = (he[0],)
    cur.execute(sql, param)
    res = cur.fetchall()
    dateb = 'tsjh%s' % (res[0][1],)
    server1 = [res[0][0], 'root', '1qaz2wsx@39299911', dateb, he[0]]
    print server1

    sql = "select host, site from engine_server where serverid = %s"
    param = (he[1],)
    cur.execute(sql, param)
    res = cur.fetchall()
    dateb = 'tsjh%s' % (res[0][1],)
    server2 = [res[0][0], 'root', '1qaz2wsx@39299911', dateb, he[1]]
    print server2

    db.close()

def get_guid(T, Q, I):
    num = (((long(T) << 56) & 0xFF00000000000000) | ((long(Q) << 44) & 0x00FFF00000000000) | (long(I) & 0x00000FFFFFFFFFFF))
    return long(num)
         
def clear_db(server, guids):
    db = MySQLdb.connect(user=server[1] ,passwd=server[2] ,db=server[3], host=server[0], charset="utf8")
    cur = db.cursor()
    cur.execute("SET NAMES utf8");
    
    sql = "select guid from player_t where level <= 10 and total_recharge = 0"
    cur.execute(sql)
    res = cur.fetchall()
    for i in range(len(res)):
        guids.append(res[i][0])
    print sql

    sql = "select guid from player_t where level <= 15 and last_login_time <= %s and total_recharge = 0"
    param = (int(time.time() * 1000) - 86400000 * 7,)
    cur.execute(sql, param)
    res = cur.fetchall()
    for i in range(len(res)):
        if not res[i][0] in guids:
            guids.append(res[i][0])
    print sql

    sql = "select guid from player_t where level <= 20 and last_login_time <= %s and total_recharge = 0"
    param = (int(time.time() * 1000) - 86400000 * 30,)
    cur.execute(sql, param)
    res = cur.fetchall()
    for i in range(len(res)):
        if not res[i][0] in guids:
            guids.append(res[i][0])
    print sql
    
    print 'guid num:', len(guids)

    sql = "create temporary table tmp_player (guid bigint not null)"
    cur.execute(sql)
    sql = "insert into tmp_player (guid) values (%s)"
    param1 = []
    for i in range(len(guids)):
        p = (guids[i],)
        param1.append(p)
    cur.executemany(sql, param1)

    num = 0
    sql = "delete acc_t from acc_t, tmp_player where acc_t.guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete equip_t from equip_t, tmp_player where equip_t.player_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete player_t from player_t, tmp_player where player_t.guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete post_t from post_t, tmp_player where post_t.receiver_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete treasure_t from treasure_t, tmp_player where treasure_t.player_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete role_t from role_t, tmp_player where role_t.player_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete pet_t from pet_t, tmp_player where pet_t.player_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete social_t from social_t, tmp_player where social_t.player_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    num = 0
    sql = "delete social_t from social_t, tmp_player where social_t.target_guid = tmp_player.guid"
    num = cur.execute(sql)
    print sql
    print 'run num:', num

    for kg in range(0, 11):
        sql = "delete rank_t from rank_t where guid = %s" % get_guid(17, server[4], kg)
        cur.execute(sql)
        print sql

    sql = "delete from boss_t"
    cur.execute(sql)
    print sql

    sql = "delete from treasure_list_t"
    cur.execute(sql)
    print sql

    db.close()

def insert_db(table_name, mt_num = 5000, auto_col = ''):
    db1 = MySQLdb.connect(user=server1[1] ,passwd=server1[2] ,db=server1[3], host=server1[0], charset="utf8")
    db2 = MySQLdb.connect(user=server2[1] ,passwd=server2[2] ,db=server2[3], host=server2[0], charset="utf8")
    cur1 = db1.cursor()
    cur1.execute("SET NAMES utf8");
    cur2 = db2.cursor()
    cur2.execute("SET NAMES utf8");

    csql = "select column_name from information_schema.columns where table_name = '%s' and table_schema = '%s'" % (table_name, server2[3])
    cur2.execute(csql)
    cres = cur2.fetchall()

    index = 0
    while 1:
        sql = "select * from " + table_name + " limit %s, %s"
        param = (index, mt_num,)
        cur2.execute(sql, param)
        res = cur2.fetchall()
        num = len(res)
        index = index + num
        if num > 0:
            sql11 = ""
            sql12 = ""
            c = 0
            auto_index = -1
            for i in range(len(cres)):
                if cres[i][0] == auto_col:
                    auto_index = i
                    continue
                if c > 0:
                    sql11 = sql11 + ", "
                    sql12 = sql12 + ", "
                sql11 = sql11 + cres[i][0]
                sql12 = sql12 + "%s"
                c = c + 1
            sql1 = "insert into " + table_name + "(" + sql11 + ") values (" + sql12 + ")"
            param1 = []
            for i in range(num):
                p = ()
                for j in range(len(res[i])):
                    if j == auto_index:
                        continue
                    p = p + (res[i][j],)
                param1.append(p)
            cur1.executemany(sql1, param1)
        if num != mt_num:
            break
    print 'insert', table_name, 'num:', index
    db1.close()
    db2.close()

def merge_db():
    db1 = MySQLdb.connect(user=server1[1] ,passwd=server1[2] ,db=server1[3], host=server1[0], charset="utf8")
    db2 = MySQLdb.connect(user=server2[1] ,passwd=server2[2] ,db=server2[3], host=server2[0], charset="utf8")
    cur1 = db1.cursor()
    cur1.execute("SET NAMES utf8");
    cur2 = db2.cursor()
    cur2.execute("SET NAMES utf8");

    #gtool
    sql = "select guid, num from gtool_t limit 1"
    cur1.execute(sql)
    res = cur1.fetchall()
    guid = res[0][0]
    num = res[0][1]
    cur2.execute(sql)
    res = cur2.fetchall()
    if res[0][1] > num:
        num = res[0][1]
    sql = "update gtool_t set num = %s where guid = %s"
    param = (num, guid,)
    cur1.execute(sql, param)
    print 'merge gtool'

    #global
    sql = "select pttq_vip_id, pttq_player_name, czjh_count from global_t"
    cur1.execute(sql)
    res = cur1.fetchall()
    pttq_vip_id_str1 = res[0][0]
    pttq_player_name_str1 = res[0][1]
    czjh_count_str1 = res[0][2]
    pttq_count1 = struct.unpack('i', pttq_vip_id_str1[:4])
    cur2.execute(sql)
    res = cur2.fetchall()
    pttq_vip_id_str2 = res[0][0]
    pttq_player_name_str2 = res[0][1]
    czjh_count_str2 = res[0][2]
    pttq_count2 = struct.unpack('i', pttq_vip_id_str2[:4])
    if pttq_count2 > pttq_count1:
        sql = "update global_t set pttq_vip_id = %s, pttq_player_name = %s"
        param = (pttq_vip_id_str2, pttq_player_name_str2,)
        cur1.execute(sql, param)
    sql = "update global_t set czjh_count = %s"
    ddcount = czjh_count_str1 + czjh_count_str2
    param = (ddcount,)
    cur1.execute(sql, param)
    print 'merge global'

    #rank
    for k in range(11, 18):
        ranklist = []
        rguid = get_guid(17, server1[4], k)
        for m in range(2):
            res = None
            if m == 0:
                guid = get_guid(17, server1[4], k)
                sql = "select player_guid, player_name, player_level, player_bf, value, player_template, \
                player_vip, player_achieve, player_huiyi, player_chenghao from rank_t where guid = %s"
                param = (guid,)
                cur1.execute(sql, param)
                res = cur1.fetchall()
            else:
                guid = get_guid(17, server2[4], k)
                sql = "select player_guid, player_name, player_level, player_bf, value, player_template, \
                player_vip, player_achieve, player_huiyi, player_chenghao from rank_t where guid = %s"
                param = (guid,)
                cur2.execute(sql, param)
                res = cur2.fetchall()
            if len(res) > 0:
                l, guids_str = struct.unpack("i%ds" % (len(res[0][0]) - 4), res[0][0])
                l, name_str = struct.unpack("i%ds" % (len(res[0][1]) - 4), res[0][1])
                l, level_str = struct.unpack("i%ds" % (len(res[0][2]) - 4), res[0][2])
                l, bf_str = struct.unpack("i%ds" % (len(res[0][3]) - 4), res[0][3])
                l, val_str = struct.unpack("i%ds" % (len(res[0][4]) - 4), res[0][4])
                l, tid_str = struct.unpack("i%ds" % (len(res[0][5]) - 4), res[0][5])
                l, vip_str = struct.unpack("i%ds" % (len(res[0][6]) - 4), res[0][6])
                l, achieve_str = struct.unpack("i%ds" % (len(res[0][7]) - 4), res[0][7])
                l, huiyi_str = struct.unpack("i%ds" % (len(res[0][8]) - 4), res[0][8])
                l, chenghao_str = struct.unpack("i%ds" % (len(res[0][9]) - 4), res[0][9])
                print "rank", l
                for i in range(l):
                    guid, guids_str = struct.unpack("Q%ds" % (len(guids_str) - 8), guids_str)
                    namelen, name_str = struct.unpack("i%ds" % (len(name_str) - 4), name_str)
                    name, name_str = struct.unpack("%ds%ds" % (namelen, len(name_str) - namelen), name_str)
                    level, level_str = struct.unpack("i%ds" % (len(level_str) - 4), level_str)
                    bf, bf_str = struct.unpack("i%ds" % (len(bf_str) - 4), bf_str)
                    val, val_str = struct.unpack("i%ds" % (len(val_str) - 4), val_str)
                    tid, tid_str = struct.unpack("i%ds" % (len(tid_str) - 4), tid_str)
                    vip, vip_str = struct.unpack("i%ds" % (len(vip_str) - 4), vip_str)
                    achieve, achieve_str = struct.unpack("i%ds" % (len(achieve_str) - 4), achieve_str)
                    huiyi, huiyi_str = struct.unpack("i%ds" % (len(huiyi_str) - 4), huiyi_str)
                    chenhao, chenghao_str = struct.unpack("i%ds" % (len(chenghao_str) - 4), chenghao_str)
                    ranklist.append([val,guid,name,level,bf,tid,vip,achieve,huiyi,chenhao])
        ranklist.sort(key = lambda x:x[0], reverse = True)

        rankliststr = ["", "", "", "", "", "", "", "", "", ""]
        for i in range(len(rankliststr)):
            rankliststr[i] = struct.pack("i", len(ranklist))
        for rl in ranklist:
            rankliststr[0] = rankliststr[0] + struct.pack("Q", rl[1])
            rankliststr[1] = rankliststr[1] + struct.pack("i", len(rl[2]))
            rankliststr[1] = rankliststr[1] + struct.pack("%ds" % len(rl[2]), rl[2])
            rankliststr[2] = rankliststr[2] + struct.pack("i", rl[3])
            rankliststr[3] = rankliststr[3] + struct.pack("i", rl[4])
            rankliststr[4] = rankliststr[4] + struct.pack("i", rl[0])
            rankliststr[5] = rankliststr[5] + struct.pack("i", rl[5])
            rankliststr[6] = rankliststr[6] + struct.pack("i", rl[6])
            rankliststr[7] = rankliststr[7] + struct.pack("i", rl[7])
            rankliststr[8] = rankliststr[8] + struct.pack("i", rl[8])
            rankliststr[9] = rankliststr[9] + struct.pack("i", rl[9])
        sql = "update rank_t set player_guid = %s, player_name = %s, player_level = %s, \
        player_bf = %s, value = %s, player_template = %s, player_vip = %s, player_achieve = %s, \
        player_huiyi = %s, player_chenghao = %s where guid = %s"
        param = (rankliststr[0], rankliststr[1], rankliststr[2], rankliststr[3], rankliststr[4],
                 rankliststr[5], rankliststr[6], rankliststr[7], rankliststr[8], rankliststr[9], rguid,)
        cur1.execute(sql, param)
        print "rank total", len(ranklist)

    #pvp
    for j in range(3):
        pvplist = [[],[],[],[],[],[],[],[],[],[],[],[],[]]
        pvpliststr = ["", "", "", "", "", "", "", "", "", "", "", "", ""]

        guid = get_guid(26, server1[4], j)
        sql = "select player_guids, player_targets, player_names, player_templates, player_servers, player_bfs, \
        player_pvps, player_wins, player_vips, player_achieves, player_guanghuans, player_dress, player_chenghaos from pvp_list_t where guid = %s"
        param = (guid,)
        cur1.execute(sql, param)
        res = cur1.fetchall()
        l, guids_str = struct.unpack("i%ds" % (len(res[0][0]) - 4), res[0][0])
        l, targets_str = struct.unpack("i%ds" % (len(res[0][1]) - 4), res[0][1])
        l, name_str = struct.unpack("i%ds" % (len(res[0][2]) - 4), res[0][2])
        l, tem_str = struct.unpack("i%ds" % (len(res[0][3]) - 4), res[0][3])
        l, server_str = struct.unpack("i%ds" % (len(res[0][4]) - 4), res[0][4])
        l, bf_str = struct.unpack("i%ds" % (len(res[0][5]) - 4), res[0][5])
        l, pvp_str = struct.unpack("i%ds" % (len(res[0][6]) - 4), res[0][6])
        l, win_str = struct.unpack("i%ds" % (len(res[0][7]) - 4), res[0][7])
        l, vip_str = struct.unpack("i%ds" % (len(res[0][8]) - 4), res[0][8])
        l, achieve_str = struct.unpack("i%ds" % (len(res[0][9]) - 4), res[0][9])
        l, guanghuan_str = struct.unpack("i%ds" % (len(res[0][10]) - 4), res[0][10])
        l, dress_str = struct.unpack("i%ds" % (len(res[0][11]) - 4), res[0][11])
        l, chenghao_str = struct.unpack("i%ds" % (len(res[0][12]) - 4), res[0][12])
        print "pvp server1", l
        for i in range(l):
            guid, guids_str = struct.unpack("Q%ds" % (len(guids_str) - 8), guids_str)
            pvplist[0].append(guid)
            target, targets_str = struct.unpack("Q%ds" % (len(targets_str) - 8), targets_str)
            pvplist[1].append(target)
            namelen, name_str = struct.unpack("i%ds" % (len(name_str) - 4), name_str)
            name, name_str = struct.unpack("%ds%ds" % (namelen, len(name_str) - namelen), name_str)
            pvplist[2].append(name)
            tem, tem_str = struct.unpack("i%ds" % (len(tem_str) - 4), tem_str)
            pvplist[3].append(tem)
            server, server_str = struct.unpack("i%ds" % (len(server_str) - 4), server_str)
            pvplist[4].append(server)
            bf, bf_str = struct.unpack("i%ds" % (len(bf_str) - 4), bf_str)
            pvplist[5].append(bf)
            pvp, pvp_str = struct.unpack("i%ds" % (len(pvp_str) - 4), pvp_str)
            pvplist[6].append(pvp)
            win, win_str = struct.unpack("i%ds" % (len(win_str) - 4), win_str)
            pvplist[7].append(win)
            vip, vip_str = struct.unpack("i%ds" % (len(vip_str) - 4), vip_str)
            pvplist[8].append(vip)
            achieve, achieve_str = struct.unpack("i%ds" % (len(achieve_str) - 4), achieve_str)
            pvplist[9].append(achieve)
            guang, guanghuan_str = struct.unpack("i%ds" % (len(guanghuan_str) - 4), guanghuan_str)
            pvplist[10].append(guang)
            dress, dress_str = struct.unpack("i%ds" % (len(dress_str) - 4), dress_str)
            pvplist[11].append(dress)
            chenhao, chenghao_str = struct.unpack("i%ds" % (len(chenghao_str) - 4), chenghao_str)
            pvplist[12].append(chenhao)

        guid1 = get_guid(26, server2[4], j)
        param = (guid1,)
        cur2.execute(sql,param)
        res = cur2.fetchall()
        l1, guids_str = struct.unpack("i%ds" % (len(res[0][0]) - 4), res[0][0])
        l1, targets_str = struct.unpack("i%ds" % (len(res[0][1]) - 4), res[0][1])
        l1, name_str = struct.unpack("i%ds" % (len(res[0][2]) - 4), res[0][2])
        l1, tem_str = struct.unpack("i%ds" % (len(res[0][3]) - 4), res[0][3])
        l1, server_str = struct.unpack("i%ds" % (len(res[0][4]) - 4), res[0][4])
        l1, bf_str = struct.unpack("i%ds" % (len(res[0][5]) - 4), res[0][5])
        l1, pvp_str = struct.unpack("i%ds" % (len(res[0][6]) - 4), res[0][6])
        l1, win_str = struct.unpack("i%ds" % (len(res[0][7]) - 4), res[0][7])
        l1, vip_str = struct.unpack("i%ds" % (len(res[0][8]) - 4), res[0][8])
        l1, achieve_str = struct.unpack("i%ds" % (len(res[0][9]) - 4), res[0][9])
        l1, guanghuan_str = struct.unpack("i%ds" % (len(res[0][10]) - 4), res[0][10])
        l1, dress_str = struct.unpack("i%ds" % (len(res[0][11]) - 4), res[0][11])
        l1, chenghao_str = struct.unpack("i%ds" % (len(res[0][12]) - 4), res[0][12])
        print "pvp server2", l1
        for i in range(l1):
            guid, guids_str = struct.unpack("Q%ds" % (len(guids_str) - 8), guids_str)
            if guid in pvplist[0]:
                assert("error")
            pvplist[0].append(guid)
            target, targets_str = struct.unpack("Q%ds" % (len(targets_str) - 8), targets_str)
            pvplist[1].append(target)
            namelen, name_str = struct.unpack("i%ds" % (len(name_str) - 4), name_str)
            name, name_str = struct.unpack("%ds%ds" % (namelen, len(name_str) - namelen), name_str)
            pvplist[2].append(name)
            tem, tem_str = struct.unpack("i%ds" % (len(tem_str) - 4), tem_str)
            pvplist[3].append(tem)
            server, server_str = struct.unpack("i%ds" % (len(server_str) - 4), server_str)
            pvplist[4].append(server)
            bf, bf_str = struct.unpack("i%ds" % (len(bf_str) - 4), bf_str)
            pvplist[5].append(bf)
            pvp, pvp_str = struct.unpack("i%ds" % (len(pvp_str) - 4), pvp_str)
            pvplist[6].append(pvp)
            win, win_str = struct.unpack("i%ds" % (len(win_str) - 4), win_str)
            pvplist[7].append(win)
            vip, vip_str = struct.unpack("i%ds" % (len(vip_str) - 4), vip_str)
            pvplist[8].append(vip)
            achieve, achieve_str = struct.unpack("i%ds" % (len(achieve_str) - 4), achieve_str)
            pvplist[9].append(achieve)
            guang, guanghuan_str = struct.unpack("i%ds" % (len(guanghuan_str) - 4), guanghuan_str)
            pvplist[10].append(guang)
            dress, dress_str = struct.unpack("i%ds" % (len(dress_str) - 4), dress_str)
            pvplist[11].append(dress)
            chenhao, chenghao_str = struct.unpack("i%ds" % (len(chenghao_str) - 4), chenghao_str)
            pvplist[12].append(chenhao)

        llen = l + l1
        for i in range(13):
            pvpliststr[i] = struct.pack("i", llen)
        for i in range(llen):
            pvpliststr[0] = pvpliststr[0] + struct.pack("Q", pvplist[0][i])
            pvpliststr[1] = pvpliststr[1] + struct.pack("Q", pvplist[1][i])
            pvpliststr[2] = pvpliststr[2] + struct.pack("i", len(pvplist[2][i]))
            pvpliststr[2] = pvpliststr[2] + struct.pack("%ds" % len(pvplist[2][i]), pvplist[2][i])
            pvpliststr[3] = pvpliststr[3] + struct.pack("i", pvplist[3][i])
            pvpliststr[4] = pvpliststr[4] + struct.pack("i", pvplist[4][i])
            pvpliststr[5] = pvpliststr[5] + struct.pack("i", pvplist[5][i])
            pvpliststr[6] = pvpliststr[6] + struct.pack("i", pvplist[6][i])
            pvpliststr[7] = pvpliststr[7] + struct.pack("i", pvplist[7][i])
            pvpliststr[8] = pvpliststr[8] + struct.pack("i", pvplist[8][i])
            pvpliststr[9] = pvpliststr[9] + struct.pack("i", pvplist[9][i])
            pvpliststr[10] = pvpliststr[10] + struct.pack("i", pvplist[10][i])
            pvpliststr[11] = pvpliststr[11] + struct.pack("i", pvplist[11][i])
            pvpliststr[12] = pvpliststr[12] + struct.pack("i", pvplist[12][i])

        sql = "update pvp_list_t set player_guids = %s, player_targets = %s, player_names = %s, \
        player_templates = %s, player_servers = %s, player_bfs = %s, player_pvps = %s, player_wins = %s, \
        player_vips = %s, player_achieves = %s, player_guanghuans = %s, player_dress = %s, player_chenghaos = %s where guid = %s"
        param = (pvpliststr[0], pvpliststr[1], pvpliststr[2], pvpliststr[3], pvpliststr[4],
                 pvpliststr[5], pvpliststr[6], pvpliststr[7], pvpliststr[8], pvpliststr[9], pvpliststr[10], pvpliststr[11], pvpliststr[12], guid,)
        cur1.execute(sql, param)
        print "pvp total", llen
    print "merge pvp"

    #bingyuan
    bingyuanlist = []
    bingyuanlist_str = ""
    # server1
    bingyuanguid = get_guid(26, server1[4], 201)
    sql = "select player_guids from pvp_list_t where guid = %s"
    param = (bingyuanguid,)
    cur1.execute(sql, param)
    res = cur1.fetchall()
    l, bingyuanguids_str = struct.unpack("i%ds" % (len(res[0][0]) - 4), res[0][0])
    print "bingyuan server1", l
    for i in range(l):
        guid, bingyuanguids_str = struct.unpack("Q%ds" % (len(bingyuanguids_str) - 8), bingyuanguids_str)
        bingyuanlist.append(guid)
    # server2
    bingyuanguid2 = get_guid(26, server2[4], 201)
    sql = "select player_guids from pvp_list_t where guid = %s"
    param = (bingyuanguid2,)
    cur2.execute(sql, param)
    res = cur2.fetchall()
    l1, bingyuanguids_str = struct.unpack("i%ds" % (len(res[0][0]) - 4), res[0][0])
    print "bingyuan server2", l1
    for i in range(l1):
        guid, bingyuanguids_str = struct.unpack("Q%ds" % (len(bingyuanguids_str) - 8), bingyuanguids_str)
        bingyuanlist.append(guid)
    bingyuanlist_str = struct.pack("i", l + l1)
    for g in bingyuanlist:
        bingyuanlist_str = bingyuanlist_str + struct.pack("Q", g)
    sql = "update pvp_list_t set player_guids = %s where guid = %s"
    param = (bingyuanlist_str,bingyuanguid,)
    cur1.execute(sql, param)
    print "bingyuan total", len(bingyuanlist)

    #huodong
    sql = "select guid, id, player_guids, player_levels, player_players, player_subs from huodong_t"
    cur1.execute(sql)
    res1 = cur1.fetchall()
    for item in res1:
        huodong_player_guids = []
        huodong_player_levels = []
        huodong_player_players = []
        huodong_player_subs = []

        sql1 = "select player_guids, player_levels, player_players, player_subs from huodong_t where id = %s"
        param1 = (item[1],)
        cur2.execute(sql1, param1)
        res2 = cur2.fetchall()
        player_guid1_str = res2[0][0]
        player_level1_str = res2[0][1]
        player_player1_str = res2[0][2]
        player_sub1_str = res2[0][3]
        l, player_guid1_str = struct.unpack('i%ds' % (len(player_guid1_str) - 4), player_guid1_str)
        l, player_level1_str = struct.unpack('i%ds' % (len(player_level1_str) - 4), player_level1_str)
        l1, player_player1_str = struct.unpack('i%ds' % (len(player_player1_str) - 4), player_player1_str)
        l1, player_sub1_str = struct.unpack('i%ds' % (len(player_sub1_str) - 4), player_sub1_str)
        for i in range(l):
            guid1, player_guid1_str = struct.unpack('Q%ds' % (len(player_guid1_str) - 8), player_guid1_str)
            huodong_player_guids.append(guid1)

            level1, player_level1_str = struct.unpack('i%ds' % (len(player_level1_str) - 4), player_level1_str)
            huodong_player_levels.append(level1)
        for i in range(l1):
            player1, player_player1_str = struct.unpack('Q%ds' % (len(player_player1_str) - 8), player_player1_str)
            huodong_player_players.append(player1)

            sub1, player_sub1_str = struct.unpack('Q%ds' % (len(player_sub1_str) - 8), player_sub1_str)
            huodong_player_subs.append(sub1)
        print "huodong id 1", item[1], l, l1

        player_guid1_str = item[2]
        player_level1_str = item[3]
        player_player1_str = item[4]
        player_sub1_str = item[5]
        l, player_guid1_str = struct.unpack('i%ds' % (len(player_guid1_str) - 4), player_guid1_str)
        l, player_level1_str = struct.unpack('i%ds' % (len(player_level1_str) - 4), player_level1_str)
        l1, player_player1_str = struct.unpack('i%ds' % (len(player_player1_str) - 4), player_player1_str)
        l1, player_sub1_str = struct.unpack('i%ds' % (len(player_sub1_str) - 4), player_sub1_str)
        for i in range(l):
            guid1, player_guid1_str = struct.unpack('Q%ds' % (len(player_guid1_str) - 8), player_guid1_str)
            if guid1 in huodong_player_guids:
                assert("error")
            huodong_player_guids.append(guid1)

            level1, player_level1_str = struct.unpack('i%ds' % (len(player_level1_str) - 4), player_level1_str)
            huodong_player_levels.append(level1)
        for i in range(l1):
            player1, player_player1_str = struct.unpack('Q%ds' % (len(player_player1_str) - 8), player_player1_str)
            if player1 in huodong_player_players:
                assert("error")
            huodong_player_players.append(player1)

            sub1, player_sub1_str = struct.unpack('Q%ds' % (len(player_sub1_str) - 8), player_sub1_str)
            huodong_player_subs.append(sub1)
        print "hudong id 2", item[1], l, l1

        huodong_player_guids_str = huodong_player_levels_str = struct.pack("i", len(huodong_player_guids))
        for i in range(len(huodong_player_guids)):
            huodong_player_guids_str = huodong_player_guids_str + struct.pack("Q", huodong_player_guids[i])
            huodong_player_levels_str = huodong_player_levels_str + struct.pack("i", huodong_player_levels[i])
        huodong_player_player_str = huodong_player_sub_str = struct.pack("i", len(huodong_player_players))
        for i in range(len(huodong_player_players)):
            huodong_player_player_str = huodong_player_player_str + struct.pack("Q", huodong_player_players[i])
            huodong_player_sub_str = huodong_player_sub_str + struct.pack("Q", huodong_player_subs[i])

        sql6 = "update huodong_t set player_guids = %s, player_levels = %s, player_players = %s, player_subs = %s where guid = %s"
        param6 = (huodong_player_guids_str,huodong_player_levels_str,huodong_player_player_str,huodong_player_sub_str,item[0])
        cur1.execute(sql6, param6)
        print sql6

        sql10 = "select guid, id from huodong_t where type = 200 or type = 10"
        cur1.execute(sql10)
        res20 = cur1.fetchall()
        for iditem in res20:
            guid20 = iditem[0]
            id20 = iditem[1]
            sqll11 = "select guid from huodong_t where id = %s"
            param11 = (id20,)
            cur2.execute(sqll11,param11,)
            res21 = cur2.fetchall()
            sql12 = "update huodong_player_t set huodong_guid = %s where huodong_guid = %s"
            param12 = (guid20,res21[0][0],)
            cur2.execute(sql12, param12,)
            print sql12
        

        sql2 = "select id, player_guids, player_arg1s, player_arg2s, guid from huodong_entry_t where huodong_guid = %s"
        param2 = (item[0],)
        cur1.execute(sql2, param2)
        res3 = cur1.fetchall()
        for item1 in res3:
            entry_player_guids = []
            entry_arg1s = []
            entry_arg2s = []

            sql4 = "select player_guids, player_arg1s, player_arg2s from huodong_entry_t where id = %s"
            param3 = (item1[0],)
            cur2.execute(sql4, param3)
            res4 = cur2.fetchall()
            player_guid2_str = res4[0][0]
            player_arg1_str = res4[0][1]
            player_arg2_str = res4[0][2]
            l, player_guid2_str = struct.unpack('i%ds' % (len(player_guid2_str) - 4), player_guid2_str)
            l, player_arg1_str = struct.unpack('i%ds' % (len(player_arg1_str) - 4), player_arg1_str)
            l, player_arg2_str = struct.unpack('i%ds' % (len(player_arg2_str) - 4), player_arg2_str)
            print "huodong entry1 id", item1[0], l
            for i in range(l):
                guid2, player_guid2_str = struct.unpack('Q%ds' % (len(player_guid2_str) - 8), player_guid2_str)
                entry_player_guids.append(guid2)

                arg1, player_arg1_str = struct.unpack('i%ds' % (len(player_arg1_str) - 4), player_arg1_str)
                entry_arg1s.append(arg1)

                arg2, player_arg2_str = struct.unpack('i%ds' % (len(player_arg2_str) - 4), player_arg2_str)
                entry_arg2s.append(arg2)

            player_guid2_str = item1[1]
            player_arg1_str = item1[2]
            player_arg2_str = item1[3]
            l, player_guid2_str = struct.unpack('i%ds' % (len(player_guid2_str) - 4), player_guid2_str)
            l, player_arg1_str = struct.unpack('i%ds' % (len(player_arg1_str) - 4), player_arg1_str)
            l, player_arg2_str = struct.unpack('i%ds' % (len(player_arg2_str) - 4), player_arg2_str)
            print "huodong entry2 id", item1[0], l
            for i in range(l):
                guid2, player_guid2_str = struct.unpack('Q%ds' % (len(player_guid2_str) - 8), player_guid2_str)
                if guid2 in entry_player_guids:
                    assert("error")
                entry_player_guids.append(guid2)

                arg1, player_arg1_str = struct.unpack('i%ds' % (len(player_arg1_str) - 4), player_arg1_str)
                entry_arg1s.append(arg1)

                arg2, player_arg2_str = struct.unpack('i%ds' % (len(player_arg2_str) - 4), player_arg2_str)
                entry_arg2s.append(arg2)

            entry_player_guids_str = entry_arg1s_str = entry_arg2s_str = struct.pack("i", len(entry_player_guids))
            for i in range(len(entry_player_guids)):
                entry_player_guids_str = entry_player_guids_str + struct.pack("Q", entry_player_guids[i])
                entry_arg1s_str = entry_arg1s_str + struct.pack("i", entry_arg1s[i])
                entry_arg2s_str = entry_arg2s_str + struct.pack("i", entry_arg2s[i])

            sql5 = "update huodong_entry_t set player_guids = %s, player_arg1s = %s, player_arg2s = %s where guid = %s"
            param5 = (entry_player_guids_str,entry_arg1s_str,entry_arg2s_str,item1[4])
            cur1.execute(sql5, param5)
            #print sql5
    print "merge huodong"
        
    #guild_list
    guids = []
    sql = "select guild_guids from guild_list_t"
    cur1.execute(sql)
    res = cur1.fetchall()
    guidstr = res[0][0]
    l, guidstr = struct.unpack('i%ds' % (len(guidstr) - 4), guidstr)
    for i in range(l):
        guid, guidstr = struct.unpack('Q%ds' % (len(guidstr) - 8), guidstr)
        guids.append(guid)
    cur2.execute(sql)
    res = cur2.fetchall()
    guidstr = res[0][0]
    l, guidstr = struct.unpack('i%ds' % (len(guidstr) - 4), guidstr)
    for i in range(l):
        guid, guidstr = struct.unpack('Q%ds' % (len(guidstr) - 8), guidstr)
        guids.append(guid)
    l = len(guids)
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('Q', guids[i])
    sql = "update guild_list_t set guild_guids = %s"
    param = (s, )
    cur1.execute(sql, param)
    print 'merge guild_list'

    #sport_list
    sql = "delete from sport_list_t where guid = %s"
    param = (get_guid(11, server1[4], 1),)
    cur1.execute(sql, param)
    print sql, param
    param = (get_guid(11, server2[4], 1),)
    cur2.execute(sql, param)
    print sql, param

    param = (get_guid(11, server1[4], 100),)
    cur1.execute(sql, param)
    print sql, param
    param = (get_guid(11, server2[4], 100),)
    cur2.execute(sql, param)
    print sql, param
    
    sports = [[], [], [], [], [], [], [], [], []]
    sql = "select * from sport_list_t limit 1"
    cur1.execute(sql)
    res1 = cur1.fetchall()
    cur2.execute(sql)
    res2 = cur2.fetchall()
    print sql

    rr1 = res1[0]
    rr2 = res2[0]
    sz1 = [[], [], [], [], [], [], [], [], []]
    sz2 = [[], [], [], [], [], [], [], [], []]
    #player_guid
    tmp = rr1[1]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    print "sport1 len:", l
    for i in range(l):
        c, tmp = struct.unpack('Q%ds' % (len(tmp) - 8), tmp)
        sz1[0].append(c)
    tmp = rr2[1]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    print "sport2 len:", l
    for i in range(l):
        c, tmp = struct.unpack('Q%ds' % (len(tmp) - 8), tmp)
        sz2[0].append(c)

    #player_template
    tmp = rr1[2]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[1].append(c)
    tmp = rr2[2]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[1].append(c)

    #player_name
    tmp = rr1[3]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        cc, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        c, tmp = struct.unpack('%ds%ds' % (cc, len(tmp) - cc), tmp)
        sz1[2].append(c)
    tmp = rr2[3]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        cc, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        c, tmp = struct.unpack('%ds%ds' % (cc, len(tmp) - cc), tmp)
        sz2[2].append(c)

    #player_level
    tmp = rr1[4]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[3].append(c)
    tmp = rr2[4]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[3].append(c)

    #player_bat_eff
    tmp = rr1[5]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[4].append(c)
    tmp = rr2[5]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[4].append(c)

    #player_isnpc
    tmp = rr1[6]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[5].append(c)
    tmp = rr2[6]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[5].append(c)

    #player_vip
    tmp = rr1[7]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[6].append(c)
    tmp = rr2[7]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[6].append(c)

    #player_achieve
    tmp = rr1[8]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[7].append(c)
    tmp = rr2[8]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[7].append(c)

    #player_chenghao
    tmp = rr1[9]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz1[8].append(c)
    tmp = rr2[9]
    l, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
    for i in range(l):
        c, tmp = struct.unpack('i%ds' % (len(tmp) - 4), tmp)
        sz2[8].append(c)

    index = 0
    while 1:
        flag = False
        if index < len(sz1[0]):
            flag = True
            if sz1[5][index] == 1 and index < len(sports[0]):
                pass
            elif sz1[5][index] == 1 and len(sports[0]) >= 5000:
                pass
            elif sz1[5][index] == 0 and sz1[0][index] in guids1:
                pass
            else:
                for i in range(9):
                    sports[i].append(sz1[i][index])
        if index < len(sz2[0]):
            flag = True
            if sz2[5][index] == 1 and index < len(sports[0]):
                pass
            elif sz2[5][index] == 1 and len(sports[0]) >= 5000:
                pass
            elif sz2[5][index] == 0 and sz2[0][index] in guids2:
                pass
            else:
                for i in range(9):
                    sports[i].append(sz2[i][index])
        if not flag:
            break
        index = index + 1

    print "sport len:", len(sports[0])
        
    ss = []
    l = len(sports[0])
    #player_guid
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('Q', sports[0][i])
    ss.append(s)
    
    #player_template
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[1][i])
    ss.append(s)

    #player_name
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', len(sports[2][i]))
        s = s + struct.pack('%ds' % len(sports[2][i]), sports[2][i])
    ss.append(s)

    #player_level
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[3][i])
    ss.append(s)

    #player_bat_eff
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[4][i])
    ss.append(s)

    #player_isnpc
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[5][i])
    ss.append(s)

    #player_vip
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[6][i])
    ss.append(s)

    #player_achieve
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[7][i])
    ss.append(s)

    #player_chenghao
    s = struct.pack('i', l)
    for i in range(l):
        s = s + struct.pack('i', sports[8][i])
    ss.append(s)

    sql = "update sport_list_t set player_guid = %s, player_template = %s, player_name = %s, player_level = %s, player_bat_eff = %s, player_isnpc = %s, \
    player_vip = %s, player_achieve = %s, player_chenghao = %s"
    param = (ss[0], ss[1], ss[2], ss[3], ss[4], ss[5], ss[6], ss[7], ss[8])
    cur1.execute(sql, param)
    print 'merge sport_list'

    db1.close()
    db2.close()
    
def main():
    get_server()
    clear_db(server1, guids1)
    clear_db(server2, guids2)
    merge_db()
    insert_db('acc_t')
    insert_db('equip_t', 10000)
    insert_db('guild_box_t')
    insert_db('guild_red_t')
    insert_db('guild_event_t')
    insert_db('guild_member_t')
    insert_db('guild_message_t')
    insert_db('guild_mission_t')
    insert_db('guild_t')
    insert_db('player_t', 1000)
    insert_db('post_t', 5000, 'pid')
    insert_db('recharge_heitao_t', 5000, 'guid')
    insert_db('role_t')
    insert_db('social_t')
    insert_db('sport_t')
    insert_db('treasure_t')
    insert_db('treasure_report_t')
    insert_db('huodong_player_t')
    insert_db('pet_t')
        
if __name__ == '__main__':
    iy = raw_input("Please input y:\n")
    if iy == "y":
        main()
