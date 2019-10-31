#! /usr/bin/env python
#coding=utf-8

import paramiko
import os
import threading
import time
import MySQLdb

port = 22
uname = "root"
passwd = "yymoon@39299911"

serverids = [] + range(1167, 1177+1)
servers = []
rootdir = "/root/app/work"

def upload(host, site, path1, path2):
    ssh = paramiko.Transport((host, 22))
    ssh.connect(username = uname, password = passwd)
    sftp = paramiko.SFTPClient.from_transport(ssh)
    print "upload start", host, site
    remote_dir = rootdir + str(site) + path2
    print remote_dir
    try:
        sftp.chdir(remote_dir)
    except Exception,e:
        print remote_dir, "not exsit"
        return
    for root, dirs, files in os.walk(path1):
        print root, len(dirs), "dirs", len(files), "files"
        for filespath in files:
            local_file = os.path.join(root,filespath)
            a = local_file.replace(path1, '')
            remote_file = os.path.join(remote_dir, a)
            remote_file = remote_file.replace('\\', '/')
            try:
                sftp.put(local_file, remote_file)
            except Exception,e:
                sftp.mkdir(os.path.split(remote_file)[0])
                sftp.put(local_file,remote_file)
        for name in dirs:
            local_path = os.path.join(root, name)
            a = local_path.replace(path1, '')
            remote_path = os.path.join(remote_dir, a)
            remote_path = remote_path.replace('\\', '/')
            try:
                sftp.mkdir(remote_path)
            except Exception,e:
                print e
    ssh.close()
    print "upload end", host, site

def make(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "make start", host, site
    remote_dir = rootdir + str(site) + "/server/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./makeex.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "make end", host, site

def start(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "start start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./start.sh', timeout = 3600)
    print stdout.read()
    time.sleep(1)
    ssh.close()
    print "start end", host, site

def stop(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "stop start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./stop.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "stop end", host, site

def stopex(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "stopex start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./stopex.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "stopex end", host, site

def cp(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "cp start", host, site
    remote_dir = rootdir + str(site) + "/server/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./cp.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "cp end", host, site

def chmod(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "chmod start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';chmod 777 updatebak.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "chmod end", host, site

def setff(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "setff start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./setff.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "setff end", host, site

def updatebak(host, site):
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(host, username = uname, password = passwd)

    print "updatebak start", host, site
    remote_dir = rootdir + str(site) + "/"
    print remote_dir
    stdin, stdout, stderr = ssh.exec_command('cd ' + remote_dir + ';./updatebak.sh', timeout = 3600)
    print stdout.read()
    ssh.close()
    print "updatebak end", host, site

def cprestart(host, site):
    stop(host, site)
    time.sleep(10)
    stopex(host, site)
    cp(host, site)
    start(host, site)

def stopbak(host, site):
    stop(host, site)
    time.sleep(10)
    stopex(host, site)
    updatebak(host, site)

def stopall(host, site):
    stop(host, site)
    time.sleep(10)
    stopex(host, site)

def cpstart(host, site):
    cp(host, site)
    start(host, site)

def thread_do(func, args):
    global servers
    ts = []
    jindex = 0
    while len(ts) > 0 or jindex < len(servers):
        if jindex < len(servers):
            a = (servers[jindex][0], servers[jindex][1]) + args
            t = threading.Thread(target=func, args=a)
            t.start()
            ts.append(t)
            jindex = jindex + 1
        if jindex < len(servers) and len(ts) < 10:
            continue
        ts[0].join()
        del ts[0]

def make_server():
    global servers
    global serverids
    db = MySQLdb.connect(user="root" ,passwd="1qaz2wsx@39299911" ,db="gttool", host="121.43.107.164", charset="utf8")
    cur = db.cursor()
    cur.execute("SET NAMES utf8");

    for i in range(len(serverids)):
        sql = "select host, site, merge from engine_server where serverid = %s"
        param = (serverids[i],)
        cur.execute(sql, param)
        res = cur.fetchall()
        num = len(res)
        if num == 1:
            if int(res[0][2]) == 0:
                servers.append([res[0][0], res[0][1]])
                print serverids[i]
    db.close()

    print servers

def main():
    make_server()
##    thread_do(upload, ("G:/BattleWomen1_Server/brance2018.05.08/soft/server/src/libgame/", "/server/src/libgame/",))
##    thread_do(upload, ("G:/BattleWomen1_Server/brance2018.05.08/soft/common/protocol/", "/common/protocol/",))
##    thread_do(upload, ("G:/BattleWomen1_Server/brance2018.05.08/soft/common/protocpp/", "/common/protocpp/",))
##    thread_do(upload, ("G:/BattleWomen1_Server/brance2018.05.08/soft/server/config/", "/server/config/",))
    thread_do(stop, ())

if __name__ == '__main__':
    iy = raw_input("Please input y:\n")
    if iy == "y":
        main()

