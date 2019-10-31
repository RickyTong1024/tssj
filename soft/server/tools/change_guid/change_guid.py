# -*- coding: utf-8 -*-
import pymysql
import struct
import time
from warnings import filterwarnings
filterwarnings('ignore', category = pymysql.Warning)

tables = [
    {
        'acc_t':['guid', ['guid']],
        'boss_t':['guid', ['guid']],
        'equip_t':['guid', ['guid', 'player_guid', 'role_guid']],
        'global_t':['guid', ['guid']],
        'gtool_t':['guid', ['guid']],
        'guild_t':['guid', ['guid', 'mission', 'pvp_guild']],
        'guild_arrange_t':['guid', ['guid']],
        'guild_box_t':['guid', ['guid', 'guild_guid']],
        'guild_event_t':['guid', ['guid', 'guild_guid']],
        'guild_fight_t':['guid', ['guid', 'guild_guid', 'target_guild_guid']],
        'guild_list_t':['guid', ['guid']],
        'guild_member_t':['guid', ['guid', 'guild_guid', 'player_guid']],
        'guild_message_t':['guid', ['guid', 'guild_guid']],
        'guild_mission_t':['guid', ['guid', 'guild_guid']],
        'guild_red_t':['guid', ['guid', 'guild_guid']],
        'huodong_t':['guid', ['guid']],
        'huodong_entry_t':['guid', ['guid', 'huodong_guid']],
        'huodong_player_t':['guid', ['guid', 'huodong_guid', 'player_guid']],
        'pet_t':['guid', ['guid', 'player_guid', 'role_guid']],
        'player_t':['guid', ['guid', 'pet_on', 'guild']],
        'post_t':['pid', ['guid', 'receiver_guid']],
        'rank_t':['guid', ['guid']],
        'recharge_heitao_t':['guid', ['player_guid']],
        'role_t':['guid', ['guid', 'player_guid', 'pet']],
        'social_t':['guid', ['guid', 'player_guid', 'target_guid']],
        'sport_t':['guid', ['guid', 'player_guid', 'other_guid']],
        'sport_list_t':['guid', ['guid']],
        'treasure_t':['guid', ['guid', 'player_guid', 'role_guid']],
        'treasure_list_t':['guid', ['guid']],
        'treasure_report_t':['guid', ['guid', 'player_guid', 'other_guid']],
    },
    ##########################################
    {
        'boss_t':['guid', ['player_guids', 'player_rank_guids']],
        'global_t':['guid', ['guild_pvp_ranks']],
        'guild_t':['guid', ['member_guids', 'event_guids', 'message_guids', 'apply_guids', 'red_guids', 'box_guids', 'pvp_guilds']],
        'guild_arrange_t':['guid', ['player_guids', 'player_zguids', 'guild_fights']],
        'guild_box_t':['guid', ['reward_guids']],
        'guild_fight_t':['guid', ['target_guids']],
        'guild_list_t':['guid', ['guild_guids']],
        'guild_mission_t':['guid', ['player_guids']],
        'guild_red_t':['guid', ['player_guid']],
        'huodong_t':['guid', ['entrys', 'player_guids', 'player_players', 'player_subs']],
        'huodong_entry_t':['guid', ['player_guids']],
        'huodong_player_t':['guid', ['args8']],
        'player_t':['guid', ['roles', 'equips', 'treasures', 'pets', 'ybq_guids', 'zhenxing', 'houyuan', 'sports', 'guild_applys', 'treasure_reports', 'yh_roles']],
        'rank_t':['guid', ['player_guid']],
        'role_t':['guid', ['zhuangbeis', 'treasures']],
        'social_t':['guid', ['invite_players']],
        'sport_list_t':['guid', ['player_guid']],
        'treasure_list_t':['guid', ['player_guid']],
    }
]

serverid = 1

db = pymysql.connect(user='root', passwd='root', db='tsjh0', host='127.0.0.1', use_unicode=True, charset='utf8')


def make_guid(guid):
    return guid & 0xFF000FFFFFFFFFFF | (serverid << 44 & 0x00FFF00000000000)


def change_single_table(data):
    return make_guid(data)


def change_multi_table(data):
    l = len(data)
    if l == 4:
        return data
    s = 'I%ds' % (l - 4)
    data_len, data = struct.unpack(s, data)
    l = len(data)
    guids = []
    for i in range(data_len):
        s = 'Q%ds' % (l - 8)
        guid, data = struct.unpack(s, data)
        l = len(data)
        guids.append(make_guid(guid))
    new_data = struct.pack('I', data_len)
    for i in range(data_len):
        l = len(new_data)
        s = '=%dsQ' % (l)
        new_data = struct.pack(s, new_data, guids[i])
    return new_data


change_table_func = [change_single_table, change_multi_table]


def change_table(tp, table, key, colomns):
    cur = db.cursor()
    try:
        s = key
        ss = ''
        for i in range(len(colomns)):
            s = s + ', ' + colomns[i]
            if i > 0:
                ss = ss + ', '
            ss = ss + colomns[i] + ' = sss'
        sql = "select %s from %s" % (s, table)
        print(sql)
        sql1 = "update %s set %s where %s = sss" % (table, ss, key)
        sql1 = sql1.replace('sss', '%s')
        print(sql1)
    
        cur.execute('SET NAMES UTF8')
        cur.execute("SET CHARACTER SET utf8;")
        cur.execute("SET character_set_connection=utf8;")
        cur.execute(sql)
        res = cur.fetchall()
        for i in range(len(res)):
            data_key = res[i][0]
            param = (data_key,)
            for j in range(len(colomns))[::-1]:
                data_c = res[i][j + 1]
                if data_c == None or data_c == 0:
                    param = (data_c,) + param
                else:
                    new_data_c = change_table_func[tp](data_c)
                    param = (new_data_c,) + param
            cur.execute(sql1, param)
    except Exception as e:
        print(e)
    finally:
        db.commit()
        cur.close()


def main():
    start = time.clock()
    for tp in range(2):
        for table in tables[tp]:
            print(table)
            key = tables[tp][table][0]
            colomns = tables[tp][table][1]
            change_table(tp, table, key, colomns)
            end = time.clock()
            print('Running time: %s Seconds' % (end - start))
    end = time.clock()
    print('Running time: %s Seconds' % (end - start))


if __name__ == '__main__':
    main()
