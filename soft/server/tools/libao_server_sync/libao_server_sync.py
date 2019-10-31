# encoding=utf-8
import pymysql

# host_ = '122.112.227.206'
host_ = '116.62.105.66'

db1 = pymysql.connect(user='root', passwd='1qaz2wsx@39299911', db='gttool', host='121.43.107.164', charset='utf8')
db1.autocommit(1)
cur1 = db1.cursor()
# sql1 = "select name, host, serverid, site from engine_server"
sql1 = 'select name, host, serverid, site from engine_server where serverid > 1000 and serverid < 1500'
cur1.execute(sql1)
res = cur1.fetchall()
print(res)

db2 = pymysql.connect(user='root', passwd='1qaz2wsx@39299911', db='tslibao', host='120.26.96.80', charset='utf8')
db2.autocommit(1)
cur2 = db2.cursor()
sql2 = 'select serverid from engine_server'
cur2.execute(sql2)
res2 = cur2.fetchall()
cnt = list()
for i in range(len(res2)):
    cnt.append(res2[i][0])
print(cnt)

for i in range(len(res)):
    if res[i][2] not in cnt:
        sql2 = 'insert into engine_server set name = %s, host = %s, serverid = %s, site = %s'
        param = (res[i][0], res[i][1], res[i][2], res[i][3],)
        cur2.execute(sql2, param)
        print(res[i])

'''
sql2 = 'insert into engine_server set name = %s, host = %s, serverid = %s, site = %s'
for i in range(len(res)):
    param = (res[i][0], res[i][1], res[i][2], res[i][3],)
    cur2.execute(sql2, param)
    '''

'''

db2 = pymysql.connect(user='root', passwd='1qaz2wsx@39299911', db='tslibao_web', host=host_, charset='utf8')
# db2 = pymysql.connect(user='root', passwd='root', db='tsjh2', host='127.0.0.1', charset='utf8')
db2.autocommit(1)
cur2 = db2.cursor()
for i in range(len(res)):
    sql2 = "update engine_server set name = sss, host = sss, site = sss where serverid = %s" % (res[i][2])
    sql2 = sql2.replace('sss', '%s')
    print(sql2)
    param = (res[i][0], res[i][1], res[i][3],)
    cur2.execute(sql2, param)
'''
