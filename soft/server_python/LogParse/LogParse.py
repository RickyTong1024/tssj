#!/usr/bin/env python
# coding=utf-8

import time
import datetime
import torndb
import os
import shutil

src_dir = "{{deploy_path}}/log_server/new_log"
target_dir = "{{deploy_path}}/log_server/old_log"


def check_list_len(item, size):
    if len(item) != size:
        return False
    return True


class Parse():
    db_list = {}
    db_name = {'0': "resource_t", '1': "item_t", '2': "role_t", '3': "equip_t",
               '4': "treasure_t", '5': "login_t", '6': "huodong_t", '7': "statistics_t"}

    def __init__(self, host, db, user, pwd):
        self.db = torndb.Connection(host=host, database=db, user=user, password=pwd, time_zone="+8:00")

    def close(self):
        self.db.close()

    def backup(self):
        now_day = datetime.datetime.now()
        if now_day.weekday() != 4:
            return
        try:
            now_day_str = now_day.strftime("%Y-%m-%d")
            flag = self.db.get("select * from backup_t where DATE(dt) = %s", now_day_str)
            if flag:
                return
            self.db.insert("INSERT INTO `backup_t` (id, dt) VALUES (0, NOW())")
        except Exception, e:
            print e
            return

        print "start backup"
        timeformat = "%Y%m%d%H%M%S"
        sqlbackupformat = "mysqldump -uroot -proot tslog > /root/app/old_log/backup/tslog%s.sql"
        nowdate = time.strftime(timeformat)
        sqlval = sqlbackupformat % nowdate
        try:
            result = 0
            if result == 0:
                self.db.execute("TRUNCATE TABLE resource_t")
                self.db.execute("TRUNCATE TABLE item_t")
                self.db.execute("TRUNCATE TABLE role_t")
                self.db.execute("TRUNCATE TABLE equip_t")
                self.db.execute("TRUNCATE TABLE treasure_t")
                self.db.execute("TRUNCATE TABLE huodong_t")
                self.db.execute("TRUNCATE TABLE statistics_t")
        except Exception, e:
            print e

    def insert(self, msg):
        if self.db_name.has_key(msg[3]) == False:
            print "msg type is unknown", msg
            return

        if self.db_list.has_key(msg[3]):
            self.db_list[msg[3]].append(msg)
        else:
            self.db_list[msg[3]] = [msg]

        cache = self.db_list[msg[3]]
        if len(cache) == 100:
            self.db_list[msg[3]] = []
            cache_value = [tuple(x) for x in cache]
            sql = "INSERT INTO `%s`" % self.db_name[msg[3]] + \
                " (id,username,serverid,player_guid,value1,value2,value3,value4,value5,pt,dt) VALUES(0,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)"
            self.db.insertmany(sql, cache_value)

    def parse(self, path):
        self.db_list = {}
        with open(path, "r") as f:
            lines = f.readlines()
            for line in lines:
                try:
                    item = line.split("@", 4)
                    if check_list_len(item, 5):
                        msg = item[4].split("|")
                        if check_list_len(msg, 9):
                            msg[8] = msg[8].strip("\n")
                            t = item[0].split(".")
                            if check_list_len(t, 2):
                                msg.append(t[0])
                                self.insert(msg)
                            else:
                                print "parse time error", line
                        else:
                            print "parse msg error", line
                    else:
                        print "parse line error", line
                except Exception, e:
                    print e

        for key, val in self.db_list.items():
            cache = val
            cache_value = [tuple(x) for x in cache]
            sql = "INSERT INTO `%s`" % self.db_name[key] + \
                " (id,username,serverid,player_guid,value1,value2,value3,value4,value5,pt,dt) VALUES(0,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)"
            self.db.insertmany(sql, cache_value)


if __name__ == '__main__':
    p = Parse("127.0.0.1", "tslog", "root", "root")
    p.backup()
    move_file = []
    for d in os.listdir(src_dir):
        f = os.path.join(src_dir, d)
        if os.path.isfile(f):
            if d == "log":
                continue
            else:
                p.parse(f)
                move_file.append(f)

    target_dir_sub = target_dir + "/" + datetime.datetime.now().strftime("%Y%m%d") + "/" + \
        datetime.datetime.now().strftime("%Y%m%d%H")
    if not os.path.exists(target_dir_sub):
        os.makedirs(target_dir_sub)

    for f in move_file:
        shutil.move(f, target_dir_sub)

    p.close()
