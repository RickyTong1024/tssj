#!/usr/bin/python
# -*-coding:utf-8-*-

import os
# import json
import time
import datetime
import pymysql
from oss import oss_api
from django.conf import settings
from models import Server
# from ftplib import FTP
import ftplib


def make_dir(path):
    is_exists = os.path.exists(path)
    if not is_exists:
        os.makedirs(path)


def oss_upload_text_from_file(filename, path=None):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    content_type = "text/HTML"
    filepath = ""
    if path:
        filepath = path
    else:
        filepath = settings.STATIC_ROOT + "/huodong/" + filename

    try:
        res = api.put_object_from_file("yymoon", filename, filepath, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_upload_text_from_fp(bucket, filename, fp):
    api = oss_api.OssAPI("oss-cn-hongkong.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    content_type = "text/HTML"

    try:
        res = api.put_object_from_fp(bucket, filename, fp, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_upload_png_from_file(filename, path=None):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    content_type = "image/png"
    filepath = ""
    if path:
        filepath = path
    else:
        filepath = settings.STATIC_ROOT + "/" + filename

    try:
        res = api.put_object_from_file("yymoon", filename, filepath, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_upload_png_from_fp(filename, fp):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    content_type = "image/png"

    try:
        res = api.put_object_from_fp("yymoon", filename, fp, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_upload_wav_from_fp(filename, fp):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    content_type = "audio/wav"

    try:
        res = api.put_object_from_fp("yymoon", filename, fp, content_type, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_upload_string_from_string(bucket, dir, content):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    # headers = {}
    content_type = "text/HTML"

    try:
        res = api.put_object_from_string(bucket, dir, content, content_type)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def oss_download_string(block, dir, filename):
    api = oss_api.OssAPI("oss.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    # content_type = "text/HTML"

    try:
        res = api.get_object_to_file(block, dir, settings.STATIC_ROOT + "/" + filename, headers)
        if (res.status / 100) != 2:
            return None

        f = open(settings.STATIC_ROOT + "/" + filename, "r")
        con = ""
        for ff in f:
            con = con + ff
        f.close()
        return con
    except Exception as e:
        print(e)
        if f:
            f.close()
        return None

    return None


def oss_download_file(block, dir, filename):
    api = oss_api.OssAPI("oss-cn-hongkong.aliyuncs.com", settings.ALIYUN_ID, settings.ALIYUN_KEY)

    headers = {}
    # content_type = "text/HTML"

    try:
        res = api.get_object_to_file(block, dir, settings.STATIC_ROOT + "/" + filename, headers)
        if (res.status / 100) != 2:
            return False
    except Exception:
        return False

    return True


def ftp_download_file(sftp_address, sftp_username, sftp_password, dir, path):
    print(sftp_address, sftp_username, sftp_password, dir, path)
    ftp = ftplib.FTP()
    fp = open(settings.STATIC_ROOT + "/" + path, 'wb')
    try:
        ftp.connect(sftp_address, 21)
        ftp.login(sftp_username, sftp_password)
        bufsize = 1024
        ftp.dir()
        ftp.retrbinary('RETR ' + dir, fp.write, bufsize)
        ftp.set_debuglevel(0)
    except Exception as e:
        print("ftp error")
        print(e)
        return False
    finally:
        fp.close()

    return True


def ftp_upload_file(sftp_address, sftp_username, sftp_password, dir, path):
    ftp = ftplib.FTP()
    # path = "./" + path
    fp = open(path, 'rb')
    try:
        ftp.connect(sftp_address, 21)
        ftp.login(sftp_username, sftp_password)

        bufsize = 1024

        ftp.storbinary('STOR ' + dir, fp, bufsize)
        ftp.set_debuglevel(0)
    except Exception as e:
        print(e)
        print("upload ftp error")
        return False
    finally:
        fp.close()
        ftp.quit()

    return True


rmb_rids = {20: 30, 30: 648, 40: 648, 50: 328, 60: 328, 70: 198, 80: 198, 90: 98,
            100: 98, 110: 30, 120: 30, 130: 6, 140: 6}


def get_rmb(rid):
    global rmb_rids
    if rid in rmb_rids:
        return rmb_rids[rid]
    else:
        return 0


def get_server_charge_today(recharge):
    if recharge.date != datetime.date.today():
        return

    total_recharge = 0
    total_count = 0
    total_player = 0
    nowtime = time.strftime("%Y-%m-%d")
    servers = Server.objects.all()
    for server in servers:
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select rid from recharge_heitao_t where DATE(dt) = '%s'" % nowtime
            cur.execute(sql)
            result = cur.fetchall()
            for val in result:
                total_recharge += get_rmb(val[0])

            sql = "select count(player_guid), count(DISTINCT(player_guid)) from recharge_heitao_t where DATE(dt) = '%s'" % nowtime
            cur.execute(sql)
            result = cur.fetchall()
            total_count += result[0][0]
            total_player += result[0][1]
        finally:
            if db:
                db.close()

    recharge.total_charge = total_recharge
    recharge.total_count = total_count
    recharge.total_player = total_player


def get_server_charge_todayex(recharge):
    total_recharge = 0
    total_count = 0
    total_player = 0
    nowtime = time.strftime("%Y-%m-%d")
    servers = Server.objects.all()
    for server in servers:
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select (SUM(if(rid=20, 30, 0))+ SUM(if(rid=25, 60, 0))+SUM(if(rid=30, 648, 0))+SUM(if(rid=40, 648, 0))+SUM(if(rid=50, 328, 0))+SUM(if(rid=60, 328, 0))+SUM(if(rid=70, 198, 0))+SUM(if(rid=80, 198, 0)) \
                    +SUM(if(rid=90, 98, 0))+SUM(if(rid=100, 98, 0))+SUM(if(rid=110, 30, 0))+SUM(if(rid=120, 30, 0))+SUM(if(rid=130, 6, 0))+SUM(if(rid=140, 6, 0))) as total from recharge_heitao_t where DATE(dt) = '%s'" % nowtime
            cur.execute(sql)
            result = cur.fetchall()
            total_recharge += result[0][0]

            sql = "select count(player_guid), count(DISTINCT(player_guid)) from recharge_heitao_t where DATE(dt) = '%s'" % nowtime
            cur.execute(sql)
            result = cur.fetchall()
            total_count += result[0][0]
            total_player += result[0][1]
        finally:
            if db:
                db.close()

    recharge.total_charge = total_recharge
    recharge.total_count = total_count
    recharge.total_player = total_player


def find_work_dir(path):
    return "0"


def get_guid_server_by_global(guid):
    serverid = ((int(guid) >> 44) & 0x0000000000000FFF)
    return serverid
