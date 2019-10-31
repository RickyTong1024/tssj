#!/usr/bin/python
# -*-coding:utf-8-*-

from django.contrib.auth import authenticate, login
from django import forms
# from django.db import connection
import models
import pymysql
import struct
import time
import httplib
import rpc_pb2
import random
import datetime
from django.conf import settings
import helper
import torndb
# from ftplib import FTP
# import ftplib
# import tarfile
import opcodes
from warnings import filterwarnings
filterwarnings('ignore', category=pymysql.Warning)


class LoginForm(forms.Form):
    username = forms.CharField(
        required=True,
        label='用户',
        min_length=4,
        max_length=16,
        error_messages={'required': '必选项'}
    )

    password = forms.CharField(
        required=True,
        label='密码',
        min_length=4,
        max_length=16,
        widget=forms.PasswordInput,
        error_messages={'required': '必选项'}
    )

    def __init__(self, request=None, *args, **kwargs):
        self.request = request
        self.user_cache = None
        super(LoginForm, self).__init__(*args, **kwargs)

    def clean(self):
        username = self.cleaned_data.get('username')
        password = self.cleaned_data.get('password')

        if username and password:
            self.user_cache = authenticate(username=username, password=password)
            # 如果成功返回对应的User对象，否则返回None(只是判断此用户是否存在，不判断是否is_active或者is_staff)
            if self.user_cache is None:
                raise forms.ValidationError(u"您输入的用户名或密码不正确!")
            elif not self.user_cache.is_active or not self.user_cache.is_staff:
                raise forms.ValidationError(u"您输入的用户名或密码不正确!")
            else:
                login(self.request, self.user_cache)
        return self.cleaned_data


class ServerlookPlayer():
    def __init__(self):
        self.name = ""
        self.num = 0
        self.recharge = 0
        self.huoyue = 0


class ServerlookItems():
    def __init__(self, index, user):
        self.index = index
        self.total = 1
        self.sers = []

        servers = models.Server.objects.filter(merge=0)
        if hasattr(user, 'profile'):
            user_profile = user.profile
            servers = models.Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                                                                           serverid__lt=user_profile.qudao1)

        num = len(servers)
        self.total = (num + 9) / 10
        num1 = index * 10 - 10
        num2 = index * 10 - 1

        for k in range(len(servers)):
            if k < num1:
                continue
            if k > num2:
                continue
            server = servers[k]
            if str(server.merge) != '0':
                continue
            db = None
            try:
                ser = ServerlookPlayer()
                ser.name = server.name
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                sql = "select count(*) from player_t"
                cur.execute(sql)
                res = cur.fetchall()
                if len(res) == 1:
                    ser.num = res[0][0]

                sql = "select sum(total_recharge) from player_t"
                cur.execute(sql)
                res = cur.fetchall()
                if len(res) == 1:
                    ser.recharge = res[0][0]

                sql = "select count(*) from player_t where player_t.last_login_time > %s" % (time.time() * 1000 - 24 * 60 * 60 * 1000)
                cur.execute(sql)
                res = cur.fetchall()
                if len(res) == 1:
                    ser.huoyue = res[0][0]

                self.sers.append(ser)
            finally:
                if db:
                    db.close()


class UserlookPlayer():
    def __init__(self):
        self.servername = ""
        self.guid = 0
        self.username = ""
        self.name = ""
        self.level = 1
        self.jewel = 0
        self.gold = 0
        self.yuanli = 0
        self.mission = 0
        self.mission_jy = 0
        self.recharge = 0
        self.zhouka_time = 0
        self.yuaka_time = 0


class UserlookForm(forms.Form):
    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )
    playername = forms.CharField(
        required=True,
        label='名字（模糊）',
        max_length=512,
        error_messages={'required': '必选项'}
    )
    playerchoice = forms.ChoiceField(
        required=True,
        label='类型',
        choices=(
            ("0", "名字"),
            ("1", "ID"),
        ),
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.player = []
        super(UserlookForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(UserlookForm, self).clean()
            server = models.Server.objects.get(name=self.cleaned_data['server_select'])
            db = None
            error = ''
            self.player = []
            pname = self.cleaned_data['playername']
            try:
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                if self.cleaned_data['playerchoice'] == '1':
                    sql = "select guid, username from acc_t where username like '%%%s%%'" % pname
                    cur.execute(sql)
                    res = cur.fetchall()
                    if len(res) > 10:
                        error = '数据过多'
                    elif len(res) == 0:
                        error = '没有该玩家'
                    else:
                        for i in range(len(res)):
                            guid = res[i][0]
                            sql = "select guid, name, level, jewel, gold, yuanli, mission, mission_jy, total_recharge, zhouka_time, yueka_time from player_t where guid = %s" % guid
                            cur.execute(sql)
                            res1 = cur.fetchall()
                            if len(res1) == 1:
                                player = UserlookPlayer()
                                player.servername = self.cleaned_data['server_select']
                                player.guid = res[i][0]
                                player.username = res[i][1]
                                player.name = res1[0][1]
                                player.level = res1[0][2]
                                player.jewel = res1[0][3]
                                player.gold = res1[0][4]
                                player.yuanli = res1[0][5]
                                player.mission = res1[0][6]
                                player.mission_jy = res1[0][7]
                                player.recharge = res1[0][8]
                                date = int(res1[0][9] / 1000)
                                date1 = int(res1[0][10] / 1000)
                                zhouka_dt = time.localtime(date)
                                player.zhouka_time = time.strftime("%Y-%m-%d %H:%M:%S", zhouka_dt)
                                yueka_dt = time.localtime(date1)
                                player.yueka_time = time.strftime("%Y-%m-%d %H:%M:%S", yueka_dt)
                                self.player.append(player)
                else:
                    sql = "select guid, name, level, jewel, gold, yuanli, mission, mission_jy, total_recharge, zhouka_time, yueka_time from player_t where name like '%%%s%%'" % pname
                    cur.execute(sql)
                    res = cur.fetchall()
                    if len(res) > 10:
                        error = '数据过多'
                    elif len(res) == 0:
                        error = '没有该玩家'
                    else:
                        for i in range(len(res)):
                            guid = res[i][0]
                            sql = "select username from acc_t where guid = %s"
                            param = (guid,)
                            cur.execute(sql, param)
                            res1 = cur.fetchall()
                            if len(res1) == 1:
                                player = UserlookPlayer()
                                player.servername = self.cleaned_data['server_select']
                                player.guid = res[i][0]
                                player.username = res1[0][0]
                                player.name = res[i][1]
                                player.level = res[i][2]
                                player.jewel = res[i][3]
                                player.gold = res[i][4]
                                player.yuanli = res[i][5]
                                player.mission = res[i][6]
                                player.mission_jy = res[i][7]
                                player.recharge = res[i][8]
                                date = int(res[i][9] / 1000)
                                date1 = int(res[i][10] / 1000)
                                zhouka_dt = time.localtime(date)
                                player.zhouka_time = time.strftime("%Y-%m-%d %H:%M:%S", zhouka_dt)
                                yueka_dt = time.localtime(date1)
                                player.yueka_time = time.strftime("%Y-%m-%d %H:%M:%S", yueka_dt)
                                self.player.append(player)
                            else:
                                error = '没有该玩家'
            except Exception as e:
                error = e
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)
        return self.cleaned_data


class HuobanItem():
    def __init__(self):
        self.id = 0
        self.level = 0
        self.jlevel = 0
        self.glevel = 0
        self.skill_level = 0


class HuobanForm(forms.Form):
    def __init__(self, *args, **kwargs):
        self.huoban = []
        self.guid = int(args[0])
        self.servername = args[1]
        super(HuobanForm, self).__init__(*args, **kwargs)

    def clean(self):
        self.cleaned_data = super(HuobanForm, self).clean()
        server = models.Server.objects.get(name=self.servername)
        db = None
        error = ''
        self.huoban = []
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select roles from player_t where guid = %s"
            param = (self.guid,)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) > 1:
                error = '内部错误'
            elif len(res) == 0:
                error = '没有该玩家'
            else:
                l = res[0][0]
                guids = []
                num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                for i in range(num):
                    guid, l = struct.unpack('Q%ds' % (len(l) - 8), l)
                    guids.append(guid)
                for i in range(len(guids)):
                    guid = guids[i]
                    sql = "select guid, template_id, level, jlevel, glevel, bskill_level from role_t where guid = %s"
                    param = (guid,)
                    cur.execute(sql, param)
                    res1 = cur.fetchall()
                    if len(res1) == 1:
                        huoban = HuobanItem()
                        huoban.id = res1[0][1]
                        huoban.level = res1[0][2]
                        huoban.jlevel = res1[0][3]
                        huoban.glevel = res1[0][4]
                        huoban.skill_level = res1[0][5]
                        self.huoban.append(huoban)
        except Exception as e:
            error = e
        finally:
            if db:
                db.close()
        if error != '':
            raise forms.ValidationError(error)
        return self.cleaned_data


class ZhuangbeiGzItem():
    def __init__(self):
        self.type = 0
        self.value = 0


class ZhuangbeiItem():
    def __init__(self):
        self.id = 0
        self.enhance = 0
        self.jilian = 0
        self.gz = []
        self.bs = []


class ZhuangbeiForm(forms.Form):
    def __init__(self, *args, **kwargs):
        self.zhuangbei = []
        self.guid = int(args[0])
        self.servername = args[1]
        super(ZhuangbeiForm, self).__init__(*args, **kwargs)

    def clean(self):
        self.cleaned_data = super(ZhuangbeiForm, self).clean()
        server = models.Server.objects.get(name=self.servername)
        db = None
        error = ''
        self.zhuangbei = []
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select equips from player_t where guid = %s"
            param = (self.guid,)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) > 1:
                error = '内部错误'
            elif len(res) == 0:
                error = '没有该玩家'
            else:
                l = res[0][0]
                guids = []
                num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                for i in range(num):
                    guid, l = struct.unpack('Q%ds' % (len(l) - 8), l)
                    guids.append(guid)
                for i in range(len(guids)):
                    guid = guids[i]
                    sql = "select guid, template_id, enhance, rand_ids, rand_values, stone, jilian from equip_t where guid = %s"
                    param = (guid,)
                    cur.execute(sql, param)
                    res1 = cur.fetchall()
                    if len(res1) == 1:
                        zhuangbei = ZhuangbeiItem()
                        zhuangbei.id = res1[0][1]
                        zhuangbei.enhance = res1[0][2]
                        zhuangbei.jilian = res1[0][6]
                        l = res1[0][3]
                        ris = []
                        num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                        for i in range(num):
                            ri, l = struct.unpack('i%ds' % (len(l) - 4), l)
                            ris.append(ri)
                        l = res1[0][4]
                        num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                        for i in range(num):
                            rv, l = struct.unpack('i%ds' % (len(l) - 4), l)
                            gz = ZhuangbeiGzItem()
                            gz.type = ris[i]
                            gz.value = rv
                            zhuangbei.gz.append(gz)
                        l = res1[0][5]
                        num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                        for i in range(num):
                            stone, l = struct.unpack('i%ds' % (len(l) - 4), l)
                            zhuangbei.bs.append(stone)
                        self.zhuangbei.append(zhuangbei)
        except Exception as e:
            error = e
        finally:
            if db:
                db.close()
        if error != '':
            raise forms.ValidationError(error)
        return self.cleaned_data


class BaowuItem():
    def __init__(self):
        self.guid = 0
        self.id = 0
        self.enhance = 0
        self.jilian = 0
        self.star = 0


class BaowuForm(forms.Form):
    def __init__(self, *args, **kwargs):
        self.baowu = []
        self.guid = int(args[0])
        self.servername = args[1]
        super(BaowuForm, self).__init__(*args, **kwargs)

    def clean(self):
        self.cleaned_data = super(BaowuForm, self).clean()
        server = models.Server.objects.get(name=self.servername)
        db = None
        error = ''
        self.baowu = []
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select treasures from player_t where guid = %s"
            param = (self.guid,)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) > 1:
                error = '内部错误'
            elif len(res) == 0:
                error = '没有该玩家'
            else:
                l = res[0][0]
                guids = []
                num, l = struct.unpack('i%ds' % (len(l) - 4), l)
                for i in range(num):
                    guid, l = struct.unpack('Q%ds' % (len(l) - 8), l)
                    guids.append(guid)
                for i in range(len(guids)):
                    guid = guids[i]
                    sql = "select guid, template_id, enhance, jilian from treasure_t where guid = %s"
                    param = (guid,)
                    cur.execute(sql, param)
                    res1 = cur.fetchall()
                    if len(res1) == 1:
                        baowu = BaowuItem()
                        baowu.id = res1[0][1]
                        baowu.enhance = res1[0][2]
                        baowu.jilian = res1[0][3]
                        self.baowu.append(baowu)
        except Exception as e:
            error = e
        finally:
            if db:
                db.close()
        if error != '':
            raise forms.ValidationError(error)
        return self.cleaned_data


class BaowuAllForm(forms.Form):
    def __init__(self, *args, **kwargs):
        self.baowu = []
        self.guid = int(args[0])
        self.servername = args[1]
        super(BaowuAllForm, self).__init__(*args, **kwargs)

    def clean(self):
        self.cleaned_data = super(BaowuAllForm, self).clean()
        server = models.Server.objects.get(name=self.servername)
        db = None
        error = ''
        self.baowu = []
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select guid, template_id, enhance, jilian, star from treasure_t where role_guid = 0 and player_guid = %s"
            param = (self.guid,)
            cur.execute(sql, param)
            res = cur.fetchall()
            for ibaowu in res:
                baowu = BaowuItem()
                baowu.guid = ibaowu[0]
                baowu.id = ibaowu[1]
                baowu.enhance = ibaowu[2]
                baowu.jilian = ibaowu[3]
                baowu.star = ibaowu[4]
                self.baowu.append(baowu)
        except Exception as e:
            error = e
        finally:
            if db:
                db.close()
        if error != '':
            raise forms.ValidationError(error)
        return self.cleaned_data


class DaojuForm(forms.Form):
    def __init__(self, *args, **kwargs):
        self.items = []
        self.guid = int(args[0])
        self.servername = args[1]
        super(DaojuForm, self).__init__(*args, **kwargs)

    def clean(self):
        self.cleaned_data = super(DaojuForm, self).clean()
        server = models.Server.objects.get(name=self.servername)
        db = None
        error = ''
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select item_ids, item_amount from player_t where guid = %s"
            param = (self.guid,)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) == 1:
                item_ids = res[0][0]
                item_nums = res[0][1]
                count, = struct.unpack("i", item_ids[:4])
                item_ids = item_ids[4:]
                item_nums = item_nums[4:]
                for i in range(count):
                    guid, = struct.unpack("i", item_ids[:4])
                    item_ids = item_ids[4:]
                    num, = struct.unpack("i", item_nums[:4])
                    item_nums = item_nums[4:]
                    rds = str(2) + " " + str(guid) + " " + str(num) + " " + str(0) + " "
                    self.items.append(rds)
        except Exception as e:
            error = e
        finally:
            if db:
                db.close()
        if error != '':
            raise forms.ValidationError(error)
        return self.cleaned_data


class MailItem():
    def __init__(self):
        self.type1 = 0
        self.value11 = 0
        self.value12 = 0
        self.type2 = 0
        self.value21 = 0
        self.value22 = 0
        self.type3 = 0
        self.value31 = 0
        self.value32 = 0

    def get_strings(self):
        num = 0
        ctype = ""
        cvalue1 = ""
        cvalue2 = ""
        cvalue3 = ""
        if self.type1 > 0 and self.value11 > 0 and self.value12 > 0:
            num = num + 1
            ctype += struct.pack("i", self.type1)
            cvalue1 += struct.pack("i", self.value11)
            cvalue2 += struct.pack("i", self.value12)
            cvalue3 += struct.pack("i", 0)
        if self.type2 > 0 and self.value21 > 0 and self.value22 > 0:
            num = num + 1
            ctype += struct.pack("i", self.type2)
            cvalue1 += struct.pack("i", self.value21)
            cvalue2 += struct.pack("i", self.value22)
            cvalue3 += struct.pack("i", 0)
        if self.type3 > 0 and self.value31 > 0 and self.value32 > 0:
            num = num + 1
            ctype += struct.pack("i", self.type3)
            cvalue1 += struct.pack("i", self.value31)
            cvalue2 += struct.pack("i", self.value32)
            cvalue3 += struct.pack("i", 0)
        ctype = struct.pack("i", num) + ctype
        cvalue1 = struct.pack("i", num) + cvalue1
        cvalue2 = struct.pack("i", num) + cvalue2
        cvalue3 = struct.pack("i", num) + cvalue3

        return ctype, cvalue1, cvalue2, cvalue3


class MailOneForm(forms.Form):
    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )
    accname = forms.CharField(
        required=True,
        label='玩家id 每个一行',
        max_length=4096,
        widget=forms.Textarea,
        error_messages={'required': '必选项'}
    )
    title = forms.CharField(
        required=True,
        label='标题',
        max_length=15,
        error_messages={'required': '必选项'}
    )
    text = forms.CharField(
        required=True,
        label='内容',
        max_length=512,
        widget=forms.Textarea,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        super(MailOneForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            ctype, cvalue1, cvalue2, cvalue3 = self.items.get_strings()
            self.cleaned_data = super(MailOneForm, self).clean()
            server = models.Server.objects.get(name=self.cleaned_data['server_select'])
            accnames = self.cleaned_data['accname']
            accnames = accnames.split('\r\n')

            db = None
            error = ''
            try:
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                tguids = []
                for i in range(len(accnames)):
                    sql = "select guid from acc_t where username = %s and serverid = %s"
                    param = (accnames[i], server.serverid)
                    cur.execute(sql, param)
                    res = cur.fetchall()
                    if len(res) == 1:
                        tguids.append(res[0][0])

                sql = "insert into post_t (guid, receiver_guid, sender_date, title, text, sender_name, type, value1, value2, value3) values (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s)"
                params = []
                for i in range(len(tguids)):
                    param = (0, tguids[i], time.time() * 1000, self.cleaned_data['title'], self.cleaned_data['text'], 'Nova', ctype, cvalue1, cvalue2, cvalue3,)
                    params.append(param)
                    print(param)
                cur.executemany(sql, params)
                db.commit()
                self.suc = True
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)
        return self.cleaned_data


class MailAllForm(forms.Form):
    server_select = forms.ModelMultipleChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        widget=forms.CheckboxSelectMultiple,
        error_messages={'required': '必选项'}
    )
    level = forms.IntegerField(
        required=True,
        label='等级限定',
        min_value=5,
        max_value=999,
        error_messages={'required': '必选项'}
    )
    title = forms.CharField(
        required=True,
        label='标题',
        max_length=15,
        error_messages={'required': '必选项'}
    )
    text = forms.CharField(
        required=True,
        label='内容',
        max_length=512,
        widget=forms.Textarea,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        super(MailAllForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            ctype, cvalue1, cvalue2, cvalue3 = self.items.get_strings()
            self.cleaned_data = super(MailAllForm, self).clean()
            fails = []
            for i in range(len(self.cleaned_data['server_select'])):
                server = models.Server.objects.get(name=self.cleaned_data['server_select'][i])
                db = None
                try:
                    db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                    cur = db.cursor()
                    sql = "select guid from player_t where level >= %s and serverid = %s"
                    param = (self.cleaned_data['level'], server.serverid)
                    cur.execute(sql, param)
                    res = cur.fetchall()
                    params = []
                    for j in range(len(res)):
                        param = (0, res[j][0], time.time() * 1000, self.cleaned_data['title'], self.cleaned_data['text'], 'Nova', ctype, cvalue1, cvalue2, cvalue3,)
                        params.append(param)
                    sql = "insert into post_t (guid, receiver_guid, sender_date, title, text, sender_name, type, value1, value2, value3) values (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s)"
                    cur.executemany(sql, params)
                    db.commit()
                    self.suc = True
                except Exception:
                    fails.append(server.name)
                finally:
                    if db:
                        db.close()
            if len(fails) > 0:
                s = '部分服务器内部错误：'
                for i in range(len(fails)):
                    s = s + fails[i] + " "
                raise forms.ValidationError(s)
        return self.cleaned_data


class GonggaoForm(forms.Form):
    server_select = forms.ModelMultipleChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        widget=forms.CheckboxSelectMultiple,
        error_messages={'required': '必选项'}
    )
    text = forms.CharField(
        required=True,
        label='内容',
        max_length=512,
        widget=forms.Textarea,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(GonggaoForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(GonggaoForm, self).clean()
            fails = []
            for i in range(len(self.cleaned_data['server_select'])):
                server = models.Server.objects.get(name=self.cleaned_data['server_select'][i])
                if int(server.merge) != 0:
                    continue
                msg = rpc_pb2.tmsg_gonggao()
                msg.gonggao = self.cleaned_data['text']
                msg.serverid = str(server.serverid)
                s = msg.SerializeToString()
                httpClient = None
                try:
                    httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
                    headers = {'Content-type': 'text/xml;charset=UTF-8'}
                    httpClient.request("POST", opcodes.op_url("TMSG_GONGGAO"), s, headers)
                    response = httpClient.getresponse()
                    print(response)
                    self.suc = True
                except Exception:
                    fails.append(server.name)
                finally:
                    if httpClient:
                        httpClient.close()
            if len(fails) > 0:
                s = '部分服务器内部错误：'
                for i in range(len(fails)):
                    s = s + fails[i] + " "
                raise forms.ValidationError(s)
        return self.cleaned_data


class DingdanStruct():
    def __init__(self):
        self.orderno = ""
        self.rid = 0
        self.dt = ""


class DingdanForm(forms.Form):
    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )
    orderno = forms.CharField(
        required=True,
        label='订单号',
        max_length=30,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.dingdan = None
        super(DingdanForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(DingdanForm, self).clean()
            server = models.Server.objects.get(name=self.cleaned_data['server_select'])
            db = None
            error = ''
            try:
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                sql = "select orderno, rid, dt from recharge_heitao_t where orderno = %s"
                param = (self.cleaned_data['orderno'],)
                cur.execute(sql, param)
                res = cur.fetchall()
                if len(res) == 1:
                    self.dingdan = DingdanStruct()
                    self.dingdan.orderno = res[0][0]
                    self.dingdan.rid = res[0][1]
                    self.dingdan.dt = res[0][2].strftime("%Y-%m-%d %H:%M:%S")
                elif len(res) == 0:
                    error = '没有该订单号'
                else:
                    error = '订单号不唯一'
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)
        return self.cleaned_data


class MoniForm(forms.Form):
    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )
    accname = forms.CharField(
        required=True,
        label='玩家id',
        max_length=512,
        error_messages={'required': '必选项'}
    )

    yueka = forms.ChoiceField(
        required=True,
        label='类型',
        choices=(
            ("130", "普通"),
            ("20", "300钻月卡"),
            ("25", "600钻高级月卡"),
            ("30", "6480钻+1620钻（赠送)-首冲"),
            ("40", "6480钻+1620钻（赠送)"),
            ("50", "3280钻+600钻（赠送）-首冲"),
            ("60", "3280钻+600钻（赠送）"),
            ("70", "1980钻+240钻（赠送）-首冲"),
            ("80", "1980钻+240钻（赠送）"),
            ("90", "980钻+80钻（赠送）-首冲"),
            ("100", "980钻+80钻（赠送）"),
            ("105", "600钻+30钻（赠送）"),
            ("110", "300钻+15钻（赠送））-首冲"),
            ("120", "300钻+15钻（赠送）"),
            ("130", "60钻-首冲"),
            ("140", "60钻"),
        ),
        error_messages={'required': '必选项'},
    )

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(MoniForm, self).clean()
            server = models.Server.objects.get(name=self.cleaned_data['server_select'])
            accname = self.cleaned_data['accname']
            db = None
            error = ''
            try:
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                sql = "select guid from acc_t where username = %s and serverid = %s"
                param = (accname, server.serverid)
                cur.execute(sql, param)
                res = cur.fetchall()
                if len(res) != 1:
                    error = '没有该玩家'
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)

            msg = rpc_pb2.tmsg_req_recharge_heitao()
            msg.uid = accname
            msg.sid = str(server.serverid)
            msg.order = "moni"
            msg.pid = int(self.cleaned_data['yueka'])
            msg.huodong_id = 0
            msg.entry_id = 0
            msg.count = 0
            s = msg.SerializeToString()
            httpClient = None
            try:
                httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
                headers = {'Content-type': 'text/xml;charset=UTF-8'}
                httpClient.request("POST", opcodes.op_url("TMSG_RECHARGE_HEITAO"), s, headers)
                response = httpClient.getresponse()
                print(response)
                self.suc = True
            except Exception:
                forms.ValidationError("内部错误")
            finally:
                if httpClient:
                    httpClient.close()
        return self.cleaned_data


class FenghaoForm(forms.Form):
    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )
    accname = forms.CharField(
        required=True,
        label='玩家id',
        max_length=512,
        error_messages={'required': '必选项'}
    )
    fenghao_time = forms.IntegerField(
        required=True,
        label='封号时间(小时)',
        min_value=0,
        max_value=99999999,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(FenghaoForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(FenghaoForm, self).clean()
            server = models.Server.objects.get(name=self.cleaned_data['server_select'])
            db = None
            error = ''
            guid = 0
            try:
                db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
                cur = db.cursor()
                sql = "select guid from acc_t where username = %s and serverid = %s"
                param = (self.cleaned_data['accname'], server.serverid)
                cur.execute(sql, param)
                res = cur.fetchall()
                if len(res) == 1:
                    guid = res[0][0]
                    sql = "update acc_t set fenghao_time = %s where guid = %s"
                    t = self.cleaned_data['fenghao_time']
                    if t > 0:
                        t = time.time() * 1000 + t * 3600000
                    param = (t, guid,)
                    cur.execute(sql, param)
                    db.commit()
                elif len(res) == 0:
                    error = '没有该玩家'
                else:
                    error = '存在多个相同名字的玩家'
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)

            msg = rpc_pb2.tmsg_kick()
            msg.guid = guid
            s = msg.SerializeToString()
            httpClient = None
            try:
                httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
                headers = {'Content-type': 'text/xml;charset=UTF-8'}
                httpClient.request("POST", opcodes.op_url("TMSG_KICK"), s, headers)
                response = httpClient.getresponse()
                print(response)
                self.suc = True
            except Exception:
                forms.ValidationError("内部错误")
            finally:
                if httpClient:
                    httpClient.close()
        return self.cleaned_data


class LibaoItem():
    def __init__(self, code, name, pc, num, max_num, types, value1s, value2s, value3s, time, gongxiang, chongzhi):
        self.code = code
        self.name = name
        self.pc = pc
        self.num = num
        self.max_num = max_num
        self.gongxiang = gongxiang
        self.time = time
        self.reward = ""

        l, types = struct.unpack('i%ds' % (len(types) - 4), types)
        type_arr = []
        for i in range(l):
            j, types = struct.unpack('i%ds' % (len(types) - 4), types)
            type_arr.append(j)
        l, value1s = struct.unpack('i%ds' % (len(value1s) - 4), value1s)
        value1_arr = []
        for i in range(l):
            j, value1s = struct.unpack('i%ds' % (len(value1s) - 4), value1s)
            value1_arr.append(j)
        l, value2s = struct.unpack('i%ds' % (len(value2s) - 4), value2s)
        value2_arr = []
        for i in range(l):
            j, value2s = struct.unpack('i%ds' % (len(value2s) - 4), value2s)
            value2_arr.append(j)
        l, value3s = struct.unpack('i%ds' % (len(value3s) - 4), value3s)
        value3_arr = []
        for i in range(l):
            j, value3s = struct.unpack('i%ds' % (len(value3s) - 4), value3s)
            value3_arr.append(j)

        if chongzhi > 0:
            type_arr.append(100)
            value1_arr.append(chongzhi)
            value2_arr.append(0)
            value3_arr.append(0)
        for i in range(len(type_arr)):
            self.reward = self.reward + str(type_arr[i]) + " " + str(value1_arr[i]) + " " + str(value2_arr[i]) + " " + str(value3_arr[i]) + " "


class LibaoItems():
    def __init__(self, index):
        self.index = index
        self.total = 1
        self.libaos = []
        num = 10
        db = None
        try:
            db = pymysql.connect(user='root', passwd='root', db='tslibao', host='127.0.0.1', charset='utf8')
            cur = db.cursor()
            sql = "select count(*) from libao_type"
            cur.execute(sql)
            res = cur.fetchall()
            if len(res) == 1:
                self.total = (res[0][0] + num - 1) / num
                if self.total == 0:
                    self.total = 1

            index = (index - 1) * num
            sql = "select * from libao_type limit %s, %s"
            param = (index, num,)
            cur.execute(sql, param)
            res = cur.fetchall()
            for i in range(len(res)):
                code = res[i][0]
                lnum = 0
                sql = "select count(*) from libao where type = %s and used = 0"
                param = (code,)
                cur.execute(sql, param)
                res1 = cur.fetchall()
                if len(res1) == 1:
                    lnum = res1[0][0]
                litem = LibaoItem(res[i][0], res[i][1], res[i][2], lnum, res[i][3], res[i][4], res[i][5], res[i][6], res[i][7], res[i][8].strftime("%Y-%m-%d %H:%M:%S"), res[i][9], res[i][10])
                self.libaos.append(litem)
        finally:
            if db:
                db.close()


class ClibaoForm(forms.Form):
    libao_type = forms.CharField(
        required=True,
        label='礼包类型',
        min_length=4,
        max_length=4,
        error_messages={'required': '必选项'}
    )
    libao_name = forms.CharField(
        required=True,
        label='礼包描述',
        max_length=256,
        error_messages={'required': '必选项'}
    )
    libao_pc = forms.IntegerField(
        required=True,
        label='礼包批次',
        min_value=0,
        max_value=50000,
        error_messages={'required': '必选项'}
    )
    libao_num = forms.IntegerField(
        required=True,
        label='礼包数量',
        min_value=1,
        max_value=50000,
        error_messages={'required': '必选项'}
    )

    libao_chongzhi = forms.ChoiceField(
        required=True,
        label='充值礼包',
        choices=(
            ("0", "不是"),
            ("30", "648元"),
            ("50", "328元"),
            ("70", "198元"),
            ("90", "98元"),
            ("110", "30元"),
            ("130", "6元"),
            ("20", "300钻月卡"),
            ("25", "600钻月卡"),
            ("1000", "5次648"),
            ("2000", "10次648"),
        ),
        error_messages={'required': '必选项'},
        )

    libao_gx = forms.ChoiceField(
        required=True,
        label='是否是共享礼包',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        self.type = ""
        super(ClibaoForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            ctype, cvalue1, cvalue2, cvalue3 = self.items.get_strings()
            self.cleaned_data = super(ClibaoForm, self).clean()
            if self.cleaned_data['libao_gx'] == '1':
                self.cleaned_data['libao_num'] = 1
            db = None
            error = ''
            try:
                db = pymysql.connect(user='root', passwd='root', db='tslibao', host='127.0.0.1', charset='utf8')
                cur = db.cursor()
                sql = "select code from libao_type where code = %s"
                param = (self.cleaned_data['libao_type'],)
                cur.execute(sql, param)
                res = cur.fetchall()
                if len(res) > 0:
                    error = '存在该类型的礼包'
                else:
                    sql = "insert into libao_type (code, name, pc, num, chongzhi, gongxiang, type, value1, value2, value3, dt) values (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, now())"
                    param = (self.cleaned_data['libao_type'], self.cleaned_data['libao_name'], self.cleaned_data['libao_pc'], self.cleaned_data['libao_num'], self.cleaned_data["libao_chongzhi"], self.cleaned_data['libao_gx'], ctype, cvalue1, cvalue2, cvalue3,)
                    cur.execute(sql, param)
                    db.commit()

                    zm = "23456789AaBbCcDdEeFfGgHhGgKkLlMmNnPpQqRrSsTtUuVvWwXxYyZz"
                    codes = []
                    t = 0
                    while t < self.cleaned_data['libao_num']:
                        s = self.cleaned_data['libao_type']
                        for i in range(10):
                            a = random.randint(0, len(zm) - 1)
                            s = s + zm[a]
                        s = s.upper()
                        flag = False
                        for i in range(len(codes)):
                            if codes[i] == s:
                                flag = True
                                break
                        if flag:
                            continue
                        codes.append(s)
                        t = t + 1

                    sql = "insert into libao (code, type, used, wexin) values (%s, %s, 0, 0)"
                    params = []
                    for i in range(len(codes)):
                        param = (codes[i], self.cleaned_data['libao_type'],)
                        params.append(param)
                    cur.executemany(sql, params)
                    db.commit()

                    s = settings.STATIC_ROOT + "/libao/%s.txt" % self.cleaned_data['libao_type']
                    f = open(s, "w")
                    for i in range(len(codes)):
                        f.write(codes[i])
                        f.write("\n")
                    f.close()

                    self.type = self.cleaned_data['libao_type']
                    self.suc = True
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)
        return self.cleaned_data


class DlibaoForm(forms.Form):
    libao_type = forms.CharField(
        required=True,
        label='礼包类型',
        min_length=4,
        max_length=4,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(DlibaoForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        else:
            self.cleaned_data = super(DlibaoForm, self).clean()
            db = None
            error = ''
            try:
                db = pymysql.connect(user='root', passwd='root', db='tslibao', host='127.0.0.1', charset='utf8')
                cur = db.cursor()
                sql = "delete from libao_type where code = %s"
                param = (self.cleaned_data['libao_type'],)
                cur.execute(sql, param)

                sql = "delete from libao where type = %s"
                param = (self.cleaned_data['libao_type'],)
                cur.execute(sql, param)
                db.commit()

                self.suc = True
            except Exception:
                raise forms.ValidationError('内部错误')
            finally:
                if db:
                    db.close()
            if error != '':
                raise forms.ValidationError(error)
        return self.cleaned_data


# ##################################活动(huodong)#############################################
class ActivityGroupForm(forms.Form):
    name = forms.CharField(
        required=True,
        label='活动组描述',
        max_length=40,
        error_messages={'required': '必选项'}
    )

    isjieri = forms.ChoiceField(
        required=True,
        label='是否是节日活动',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    jieri_item1 = forms.CharField(
        required=False,
        label='节日祝福图标名',
        max_length=40,
    )

    jieri_des1 = forms.CharField(
        required=False,
        label='节日祝福描述',
        max_length=80,
        widget=forms.Textarea,
    )

    jieri_item2 = forms.CharField(
        required=False,
        label='节日卡片图标名',
        max_length=40,
    )

    jieri_des2 = forms.CharField(
        required=False,
        label='节日卡片描述',
        max_length=80,
        widget=forms.Textarea,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(ActivityGroupForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityGroupForm, self).clean()

        now_time = datetime.datetime.now().replace(tzinfo=None)
        activityGroup = models.ActivityGroup(name=self.cleaned_data['name'],
                                             create_time=now_time,
                                             isjieri=int(self.cleaned_data["isjieri"]),
                                             item_name1=self.cleaned_data["jieri_item1"],
                                             item_name2=self.cleaned_data["jieri_item2"],
                                             item_des1=self.cleaned_data["jieri_des1"],
                                             item_des2=self.cleaned_data["jieri_des2"])
        activityGroup.save()

        self.suc = True
        return self.cleaned_data


class ActivityModifyGroupNameForm(forms.Form):
    name = forms.CharField(
        required=True,
        label='新的名字',
        max_length=40,
        error_messages={'required': '必选项'}
    )

    jieri_item1 = forms.CharField(
        required=False,
        label='新的节日祝福图标名',
        max_length=40,
    )

    jieri_des1 = forms.CharField(
        required=False,
        label='新的节日祝福描述',
        max_length=80,
    )

    jieri_item2 = forms.CharField(
        required=False,
        label='新的节日卡片图标名',
        max_length=40,
    )

    jieri_des2 = forms.CharField(
        required=False,
        label='新的节日卡片描述',
        max_length=80,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "group" in arg:
                self.id = arg["group"]
        super(ActivityModifyGroupNameForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityModifyGroupNameForm, self).clean()

        group = models.ActivityGroup.objects.get(id=self.id)

        if self.cleaned_data["name"]:
            group.name = self.cleaned_data["name"]
        if self.cleaned_data["jieri_item1"]:
            group.item_name1 = self.cleaned_data["jieri_item1"]
        if self.cleaned_data["jieri_des1"]:
            group.item_des1 = self.cleaned_data["jieri_des1"]
        if self.cleaned_data["jieri_item2"]:
            group.item_name2 = self.cleaned_data["jieri_item2"]
        if self.cleaned_data["jieri_des2"]:
            group.item_des2 = self.cleaned_data["jieri_des2"]
        group.save()

        self.suc = True
        return self.cleaned_data


class ActivityTimeLineForm(forms.Form):
    start = forms.DateField(
        required=True,
        label='活动开始时间',
        widget=forms.DateInput,
        help_text=u"格式:2015-06-12，新服活动可以填一个2025年",
        error_messages={'required': '必选项'},
    )

    end = forms.IntegerField(
        required=True,
        min_value=1,
        label='活动持续天数',
        error_messages={'required': '必选项'},
    )

    gp = forms.IntegerField(
        required=True,
        min_value=0,
        label='组',
        error_messages={'required': '必选项'},
    )

    isnew = forms.ChoiceField(
        required=True,
        label='是否是新服活动',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        help_text=u"如果是新服活动会在新服开启的时候自动开启，否则没有任何效果(仅限提前配好，自动开启的服务器)",
        error_messages={'required': '必选项'},
        )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(ActivityTimeLineForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityTimeLineForm, self).clean()

        start_time = self.cleaned_data['start']
        end_time = start_time + datetime.timedelta(days=self.cleaned_data['end'])
        enddayd = self.cleaned_data['end']

        isnew = int(self.cleaned_data["isnew"])

        if isnew:
            timeline = models.ActivityTime(startime=datetime.datetime.fromtimestamp(time.mktime(start_time.timetuple())), endtime=datetime.datetime.fromtimestamp(time.mktime(end_time.timetuple())), sync=True, isnew=True, endday=enddayd, gp=self.cleaned_data['gp'])
            timeline.save()
        else:
            timeline = models.ActivityTime(startime=datetime.datetime.fromtimestamp(time.mktime(start_time.timetuple())), endtime=datetime.datetime.fromtimestamp(time.mktime(end_time.timetuple())), sync=False, isnew=False, endday=enddayd, gp=self.cleaned_data['gp'])
            timeline.save()

        self.suc = True
        return self.cleaned_data


class ActivityTimeLine():
    def __init__(self, id, synctime, endtime, day, isnew, gp):
        self.id = id
        self.starttime = synctime
        self.endtime = endtime
        self.names = []
        self.endday = day
        self.isnew = isnew
        self.gp = gp

    def add_name(self, name, id):
        self.names.append([name, id])

    def resize(self):
        while len(self.names) < 5:
            self.names.append(["", 0])


class ActivityTimeLineAddGroupForm(forms.Form):
    huodong_select = forms.ModelMultipleChoiceField(
        required=True,
        label='已创建的活动组',
        queryset=models.ActivityGroup.objects.all(),
        widget=forms.CheckboxSelectMultiple(attrs={'inline': True}),
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "timeline" in arg:
                self.id = arg["timeline"]
        super(ActivityTimeLineAddGroupForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityTimeLineAddGroupForm, self).clean()

        if len(self.cleaned_data['huodong_select']) > 5:
            raise forms.ValidationError('一次最多选5个')

        timeline = models.ActivityTime.objects.get(id=self.id)
        for i in range(len(self.cleaned_data['huodong_select'])):
            group = models.ActivityGroup.objects.get(name=self.cleaned_data['huodong_select'][i])
            group.timelines.add(timeline)

        self.suc = True
        return self.cleaned_data


class ActivityGroupSyncForm(forms.Form):
    server_select = forms.ModelMultipleChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.filter(merge="0"),
        widget=forms.CheckboxSelectMultiple(attrs={'inline': True}),
        error_messages={'required': '必选项'}
    )

    start = forms.DateField(
        label='活动开始时间',
        widget=forms.DateInput,
        help_text=u"格式:2015-06-12",
        error_messages={'required': '必选项'},
    )

    end = forms.DateField(
        label='活动结束时间',
        widget=forms.DateInput,
        help_text=u"格式:2015-06-25",
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "group" in arg:
                self.id = arg["group"]
        super(ActivityGroupSyncForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityGroupSyncForm, self).clean()

        start_time = self.cleaned_data['start']
        end_time = self.cleaned_data['end']

        group = models.ActivityGroup.objects.get(id=self.id)
        msg = rpc_pb2.tmsg_activity_group()
        msg.name = group.name
        msg.isjieri = group.isjieri
        msg.item_name1 = group.item_name1
        msg.item_name2 = group.item_name2
        msg.item_des1 = group.item_des1
        msg.item_des2 = group.item_des2
        activitys = group.activitys.all()
        for act in activitys:
            activityID = models.ActivityID.objects.get(type=act.type)
            activityID.aid = activityID.aid + 1
            activityID.save()

            tmsg = msg.activitys.add()
            tmsg.id = activityID.aid
            tmsg.start = int(time.mktime(start_time.timetuple()) * 1000)
            tmsg.end = int(time.mktime(end_time.timetuple()) * 1000)
            tmsg.kaifu_start = act.kaifu_start
            tmsg.kaifu_end = act.kaifu_end
            tmsg.name = act.huodong_name
            tmsg.kaikai_start = act.kaikai_start
            tmsg.type = act.atype
            tmsg.subtype = act.subtype
            tmsg.show = tmsg.start + act.start_show_day * 24 * 60 * 60 * 1000
            if act.end_show_day == 0:
                tmsg.noshow = tmsg.end
            else:
                tmsg.noshow = tmsg.start + act.end_show_day * 24 * 60 * 60 * 1000

            entrys = act.entrys.order_by("condition").order_by("order")
            for entry in entrys:
                reward = tmsg.rewards.add()
                reward.id = entry.aid
                reward.condition = entry.condition
                reward.arg1 = entry.arg1
                reward.arg2 = entry.arg2
                reward.arg3 = entry.arg3
                reward.arg4 = entry.arg4
                reward.arg5 = entry.arg5
                reward.show_time = tmsg.start
                if tmsg.subtype == 8:
                    reward.show_time = tmsg.start + entry.arg3 * 24 * 60 * 60 * 1000
                if entry.duihuan11 != 0:
                    reward.arg6.append(entry.duihuan11)
                    reward.arg7.append(entry.duihuan12)
                    reward.arg8.append(entry.duihuan13)
                if entry.duihuan21 != 0:
                    reward.arg6.append(entry.duihuan21)
                    reward.arg7.append(entry.duihuan22)
                    reward.arg8.append(entry.duihuan23)
                if entry.type11 != 0:
                    reward.types.append(entry.type11)
                    reward.value1s.append(entry.value12)
                    reward.value2s.append(entry.value13)
                    reward.value3s.append(entry.value14)
                if entry.type21 != 0:
                    reward.types.append(entry.type21)
                    reward.value1s.append(entry.value22)
                    reward.value2s.append(entry.value23)
                    reward.value3s.append(entry.value24)
                if entry.type31 != 0:
                    reward.types.append(entry.type31)
                    reward.value1s.append(entry.value32)
                    reward.value2s.append(entry.value33)
                    reward.value3s.append(entry.value34)
                if entry.type41 != 0:
                    reward.types.append(entry.type41)
                    reward.value1s.append(entry.value42)
                    reward.value2s.append(entry.value43)
                    reward.value3s.append(entry.value44)
        s = msg.SerializeToString()
        sync_servers = ""
        for i in range(len(self.cleaned_data['server_select'])):
            server = models.Server.objects.get(name=self.cleaned_data['server_select'][i])
            if int(server.merge) != 0:
                continue
            httpClient = None
            suc = "失败"
            error = ""
            try:
                httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
                headers = {'Content-type': 'text/xml;charset=UTF-8'}
                httpClient.request("POST", opcodes.op_url("TMSG_HUODONG"), s, headers)
                response = httpClient.getresponse()
                text = response.read()
                fmt = "i%ds" % (len(text) - 4)
                error_code, info = struct.unpack(fmt, text)
                if response.status / 100 == 2:
                    if error_code == 0:
                        suc = "成功"
                    elif error_code == -100:
                        error = "无效的活动时间"
                    elif error_code == -101:
                        error = "无效的活动奖励参数"
                    else:
                        error = "同步错误"
                else:
                    error = "同步失败"
            except Exception as e:
                print(e)
                error = "同步失败"
            finally:
                if httpClient:
                    httpClient.close()
            sync_servers = sync_servers + "[" + server.name + ":" + suc + ":" + error + "]"

        history = models.ActivitySyncHistory(sync_infos=sync_servers,
                                             start_time=datetime.datetime.fromtimestamp(time.mktime(start_time.timetuple())),
                                             end_time=datetime.datetime.fromtimestamp(time.mktime(end_time.timetuple())),
                                             synctime=datetime.datetime.now().replace(tzinfo=None),
                                             group=group)
        history.save()

        self.suc = True
        return self.cleaned_data


class ActivityForm(forms.Form):
    huodong_def = {"1": ("充值返利", 30000, 3),
                   "2": ("活跃活动", 40000, 4),
                   "3": ("单笔充值", 50000, 5),
                   "4": ("登录送礼", 60000, 6),
                   "5": ("折扣贩售", 70000, 7),
                   "6": ("道具兑换", 80000, 8),
                   "7": ("日期登录", 90000, 9),
                   "8": ("幸运探宝", 100000, 20),
                   "9": ("充值翻牌", 110000, 21),
                   "10": ("时装转盘", 120000, 22),
                   "11": ("太空漫游", 130000, 23),
                   "12": ("九宫魔方", 140000, 24),
                   "13": ("月卡基金", 150000, 10),
                   "14": ("直冲活动", 160000, 11),
                   }

    select = forms.ChoiceField(
        required=True,
        label='选择活动类型',
        choices=(
            ("1", "充值返利"),
            ("2", "活跃活动"),
            ("3", "单笔充值"),
            ("4", "登录送礼"),
            ("5", "折扣贩售"),
            ("6", "道具兑换"),
            ("7", "日期登录"),
            ("8", "幸运探宝"),
            ("9", "充值翻牌"),
            ("10", "时装转盘"),
            ("11", "太空漫游"),
            ("12", "九宫魔方"),
            ("13", "月卡基金"),
            ("14", "直冲活动"),
        ),
        error_messages={'required': '必选项'},
    )

    huodong_name = forms.CharField(
        required=True,
        label='活动名称',
        max_length=40,
        error_messages={'required': '必选项'}
    )

    kaifu_start = forms.IntegerField(
        required=True,
        label='最小等级',
        min_value=1,
        error_messages={'required': '必选项'},
    )

    kaifu_end = forms.IntegerField(
        required=True,
        label='最大等级',
        min_value=1,
        error_messages={'required': '必选项'},
    )

    kaikai_start = forms.IntegerField(
        required=True,
        label='创建角色几天后开始(初始0)',
        min_value=0,
        error_messages={'required': '必选项'},
    )

    start_show_day = forms.ChoiceField(
        required=True,
        label='活动开始(活动时间内)',
        choices=(
            ("0", "第一天"),
            ("3", "第四天"),
        ),
        error_messages={'required': '必选项'},
    )

    end_show_day = forms.ChoiceField(
        required=True,
        label='活动结束(活动时间内)',
        choices=(
            ("0", "结束"),
            ("3", "第三天"),
        ),
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "group" in arg:
                self.id = arg["group"]
        super(ActivityForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityForm, self).clean()

        activityGroup = models.ActivityGroup.objects.get(id=self.id)

        activityID = None
        try:
            activityID = models.ActivityID.objects.get(type=int(self.cleaned_data['select']))
        except Exception:
            activityID = models.ActivityID(aid=self.huodong_def[self.cleaned_data['select']][1],
                                           type=int(self.cleaned_data['select']))
            activityID.save()

        entryID = None
        try:
            entryID = models.EntryID.objects.get(eid=1)
        except Exception:
            entryID = models.EntryID(eid=1, entry_id=100)
            entryID.save()

        huodong_type_type = self.huodong_def[self.cleaned_data['select']][2]
        huodong_subtype_type = self.huodong_def[self.cleaned_data['select']][2]
        if activityGroup.isjieri == 1:
            if huodong_type_type == 8:
                if self.cleaned_data["start_show_day"] != '0':
                    raise forms.ValidationError('节日兑换活动，必须从第一天开始')
                if self.cleaned_data["end_show_day"] != '0':
                    raise forms.ValidationError('节日兑换活动，必须选结束')
            huodong_type_type = 100
        else:
            if huodong_type_type >= 20 and huodong_type_type < 30:
                huodong_type_type = 200

            if self.cleaned_data["start_show_day"] != '0':
                raise forms.ValidationError('非节日活动，必须是第一天开始')

            if self.cleaned_data["end_show_day"] != '0':
                raise forms.ValidationError('非节日活动，必须是结束')

        activity = models.Activity(name=self.huodong_def[self.cleaned_data['select']][0],
                                   type=int(self.cleaned_data['select']),
                                   atype=huodong_type_type,
                                   subtype=huodong_subtype_type,
                                   aid=self.huodong_def[self.cleaned_data['select']][1],
                                   kaifu_start=self.cleaned_data['kaifu_start'],
                                   kaifu_end=self.cleaned_data['kaifu_end'],
                                   huodong_name=self.cleaned_data['huodong_name'],
                                   kaikai_start=self.cleaned_data['kaikai_start'],
                                   start_show_day=int(self.cleaned_data["start_show_day"]),
                                   end_show_day=int(self.cleaned_data["end_show_day"]),
                                   group=activityGroup)
        activity.save()

        self.suc = True
        return self.cleaned_data


class ActivityModifyForm(forms.Form):
    huodong_name = forms.CharField(
        required=False,
        label='活动名字',
        max_length=40,
    )

    kaifu_start = forms.IntegerField(
        required=False,
        label='最小等级',
        min_value=1,
    )

    kaifu_end = forms.IntegerField(
        required=False,
        label='最大等级',
        min_value=1,
    )

    kaikai_start = forms.IntegerField(
        required=False,
        label='创建角色几天后开始(初始0)',
        min_value=0,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityModifyForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityModifyForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        if self.cleaned_data["huodong_name"]:
            activity.huodong_name = self.cleaned_data["huodong_name"]
        if self.cleaned_data["kaifu_start"]:
            activity.kaifu_start = self.cleaned_data["kaifu_start"]
        if self.cleaned_data["kaifu_end"]:
            activity.kaifu_end = self.cleaned_data["kaifu_end"]
        if self.cleaned_data["kaikai_start"] or self.cleaned_data["kaikai_start"] == 0:
            activity.kaikai_start = self.cleaned_data["kaikai_start"]

        activity.save()

        self.suc = True
        return self.cleaned_data


class ActivityCopyForm(forms.Form):
    huodong_select = forms.ModelChoiceField(
        required=True,
        label='活动组',
        queryset=models.ActivityGroup.objects.all(),
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityCopyForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityCopyForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        activityGroup = models.ActivityGroup.objects.get(name=self.cleaned_data['huodong_select'])

        newActivity = models.Activity(name=activity.name,
                                      type=activity.type,
                                      atype=activity.atype,
                                      subtype=activity.subtype,
                                      aid=activity.aid,
                                      kaifu_start=activity.kaifu_start,
                                      kaifu_end=activity.kaifu_end,
                                      huodong_name=activity.huodong_name,
                                      kaikai_start=activity.kaikai_start,
                                      start_show_day=activity.start_show_day,
                                      end_show_day=activity.end_show_day,
                                      group=activityGroup)
        newActivity.save()

        entrys = activity.entrys.all()
        for entry in entrys:
            entryID = models.EntryID.objects.get(eid=1)
            entryID.entry_id = entryID.entry_id + 1
            entryID.save()
            newEntry = models.Entry(aid=entryID.entry_id,
                                    order=entry.order,
                                    condition=entry.condition,
                                    arg1=entry.arg1,
                                    arg2=entry.arg2,
                                    arg3=entry.arg3,
                                    arg4=entry.arg4,
                                    arg5=entry.arg5,
                                    arg6=entry.arg6,
                                    arg7=entry.arg7,
                                    duihuan11=entry.duihuan11,
                                    duihuan12=entry.duihuan12,
                                    duihuan13=entry.duihuan13,
                                    duihuan21=entry.duihuan21,
                                    duihuan22=entry.duihuan22,
                                    duihuan23=entry.duihuan23,
                                    type11=entry.type11,
                                    value12=entry.value12,
                                    value13=entry.value13,
                                    value14=entry.value14,
                                    type21=entry.type21,
                                    value22=entry.value22,
                                    value23=entry.value23,
                                    value24=entry.value24,
                                    type31=entry.type31,
                                    value32=entry.value32,
                                    value33=entry.value33,
                                    value34=entry.value34,
                                    type41=entry.type41,
                                    value42=entry.value42,
                                    value43=entry.value43,
                                    value44=entry.value44,
                                    activity=newActivity)
            newEntry.save()

        self.suc = True
        return self.cleaned_data


class ActivityRewardItem():
    def __init__(self):
        self.type1 = 0
        self.value11 = 0
        self.value12 = 0
        self.type2 = 0
        self.value21 = 0
        self.value22 = 0
        self.type3 = 0
        self.value31 = 0
        self.value32 = 0
        self.type4 = 0
        self.value41 = 0
        self.value42 = 0


class ActivityRewardDesc():
    choices = {0: "无",
               1: "钻石",
               2: "人民币",
               3: "天数",
               4: "道具",
               5: "日期",
               100: "主线副本通关次数",
               101: "魔王讨伐次数",
               102: "竞技场胜利次数",
               103: "夺宝次数",
               104: "命运指针次数",
               105: "幸运探宝积分",
               106: "精英副本通关次数",
               107: "军团副本挑战次数",
               108: "猎人大会挑战次数",
               109: "冰原获奖次数",
               110: "银河转盘积分",
               111: "太空漫游积分",
               112: "九宫魔方积分",
               113: "太空运输拦截飞船次数",
               114: "太空运输护送企业号次数",
               115: "合成橙色饰品个数",
               116: "合成红色饰品个数",
               117: "伙伴商店刷新次数",
               118: "普通/精英副本重置次数",
               }

    def __init__(self, entry):
        self.id = entry.id
        self.aid = entry.aid
        self.order = entry.order
        self.condition = self.choices.get(entry.condition, "无")
        self.arg1 = entry.arg1
        self.arg2 = entry.arg2
        self.arg3 = entry.arg3
        self.arg4 = entry.arg4
        self.arg5 = entry.arg5
        self.reward = ""
        self.duihuan = ""
        if entry.duihuan11 != 0:
            self.duihuan = self.duihuan + str(entry.duihuan11) + " " + str(entry.duihuan12) + " " + str(entry.duihuan13) + " " + str(0) + " "
        if entry.duihuan21 != 0:
            self.duihuan = self.duihuan + str(entry.duihuan21) + " " + str(entry.duihuan22) + " " + str(entry.duihuan23) + " " + str(0) + " "

        if entry.type11 != 0:
            self.reward = self.reward + str(entry.type11) + " " + str(entry.value12) + " " + str(entry.value13) + " " + str(entry.value14) + " "
        if entry.type21 != 0:
            self.reward = self.reward + str(entry.type21) + " " + str(entry.value22) + " " + str(entry.value23) + " " + str(entry.value24) + " "
        if entry.type31 != 0:
            self.reward = self.reward + str(entry.type31) + " " + str(entry.value32) + " " + str(entry.value33) + " " + str(entry.value34) + " "
        if entry.type41 != 0:
            self.reward = self.reward + str(entry.type41) + " " + str(entry.value42) + " " + str(entry.value43) + " " + str(entry.value44) + " "


class ActivityCzflRewardForm(forms.Form):
    '''充值返利'''

    arg1 = forms.IntegerField(
        required=True,
        label='人民币',
        min_value=1,
        help_text=u'每档达成条件所需人民币',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityCzflRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityCzflRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg2"],
                             condition=2,
                             arg1=self.cleaned_data["arg1"],
                             arg2=0,
                             arg3=0,
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityDbczRewardForm(forms.Form):
    '''单笔充值'''

    arg1 = forms.IntegerField(
        required=True,
        label='iosid',
        min_value=1,
        help_text=u'每档达成条件所需iosid',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.IntegerField(
        required=True,
        label='领取次数',
        min_value=1,
        help_text=u'每档可领取的次数',
        error_messages={'required': '必选项'}
    )

    arg3 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'}
    )

    arg4 = forms.IntegerField(
        required=True,
        label='显示钻石',
        min_value=1,
        help_text=u'繁体版显示钻石',
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityDbczRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityDbczRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg3"],
                             condition=2,
                             arg1=self.cleaned_data["arg1"],
                             arg2=self.cleaned_data["arg2"],
                             arg3=self.cleaned_data["arg4"],
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityDlslRewardForm(forms.Form):
    '''登录送礼'''

    arg1 = forms.IntegerField(
        required=True,
        label='累计天数',
        min_value=1,
        error_messages={'required': '必选项'}
    )

    arg3 = forms.ChoiceField(
        required=True,
        label='是否N选1',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    arg2 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityDlslRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityDlslRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg2"],
                             condition=3,
                             arg1=self.cleaned_data["arg1"],
                             arg2=int(self.cleaned_data["arg3"]),
                             arg3=0,
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityZkfsRewardForm(forms.Form):
    '''折扣贩售'''

    arg1 = forms.IntegerField(
        required=True,
        label='钻石数量',
        min_value=1,
        help_text=u'每档折扣兑换所需钻石数',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.IntegerField(
        required=True,
        label='兑换次数',
        min_value=1,
        help_text=u'每档折扣兑换最大兑换数',
        error_messages={'required': '必选项'}
    )

    arg3 = forms.ChoiceField(
        required=True,
        label='是否N选1',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    arg4 = forms.IntegerField(
        required=True,
        label='折扣',
        min_value=1,
        max_value=100,
        help_text=u'百分比',
        error_messages={'required': '必选项'},
        )

    arg5 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'},
        )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityZkfsRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityZkfsRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg5"],
                             condition=1,
                             arg1=self.cleaned_data["arg1"],
                             arg2=self.cleaned_data["arg2"],
                             arg3=int(self.cleaned_data["arg3"]),
                             arg4=self.cleaned_data["arg4"],
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityDjdhRewardForm(forms.Form):
    '''道具兑换'''

    arg1 = forms.IntegerField(
        required=True,
        label='兑换次数',
        min_value=1,
        help_text=u'每档道具兑换最大兑换数',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.ChoiceField(
        required=True,
        label='是否N选1',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    arg3 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'},
        )

    arg4 = forms.ChoiceField(
        required=False,
        label='节日活动中第几天开始显示',
        choices=(
            ("0", "第一天"),
            ("3", "第四天"),
        ),
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.duihuans = None
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityDjdhRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityDjdhRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg3"],
                             condition=4,
                             arg1=self.cleaned_data["arg1"],
                             arg2=self.cleaned_data["arg2"],
                             arg3=int(self.cleaned_data["arg4"]) if self.cleaned_data["arg4"] else 0,
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=self.duihuans.type1,
                             duihuan12=self.duihuans.value11,
                             duihuan13=self.duihuans.value12,
                             duihuan21=self.duihuans.type2,
                             duihuan22=self.duihuans.value21,
                             duihuan23=self.duihuans.value22,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityHyhdRewardForm(forms.Form):
    '''活跃活动'''

    arg1 = forms.IntegerField(
        required=True,
        label='达成次数',
        min_value=1,
        help_text=u'每档需要满足条件数',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.ChoiceField(
        required=True,
        label='条件',
        choices=(
            ("100", "副本"),
            ("101", "魔王讨伐"),
            ("102", "竞技场胜利"),
            ("103", "夺宝"),
            ("104", "命运指针"),
            ("105", "幸运探宝"),
            ("106", "精英"),
            ("107", "军团副本"),
            ("108", "猎人大会"),
            ("109", "决战冰原奖励次数"),
            ("110", "银河转盘积分"),
            ("111", "太空漫游积分"),
            ("112", "九宫魔方积分"),
            ("113", "太空运输拦截飞船次数"),
            ("114", "太空运输护送企业号次数"),
            ("115", "合成橙色饰品个数"),
            ("116", "合成红色饰品个数"),
            ("117", "伙伴商店刷新次数"),
            ("118", "普通/精英副本重置次数"),
        ),
        error_messages={'required': '必选项'},
        )

    arg3 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'},
        )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityHyhdRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityHyhdRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg3"],
                             condition=int(self.cleaned_data["arg2"]),
                             arg1=self.cleaned_data["arg1"],
                             arg2=0,
                             arg3=0,
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityRqdlRewardForm(forms.Form):
    '''日期登录'''

    arg1 = forms.IntegerField(
        required=True,
        label='年',
        min_value=2016,
        help_text=u'登录年',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.IntegerField(
        required=True,
        label='月',
        min_value=1,
        max_value=12,
        help_text=u'登录月',
        error_messages={'required': '必选项'}
    )

    arg3 = forms.IntegerField(
        required=True,
        label='日',
        min_value=1,
        max_value=31,
        help_text=u'登录日',
        error_messages={'required': '必选项'}
    )

    arg4 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'},
        )

    arg5 = forms.ChoiceField(
        required=True,
        label='是否N选1',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
        )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityRqdlRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityRqdlRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg4"],
                             condition=5,
                             arg1=self.cleaned_data["arg1"],
                             arg2=self.cleaned_data["arg2"],
                             arg3=self.cleaned_data["arg3"],
                             arg4=int(self.cleaned_data["arg5"]),
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


class ActivityZchdRewardForm(forms.Form):
    '''直冲活动'''

    arg1 = forms.IntegerField(
        required=True,
        label='rid',
        min_value=1,
        help_text=u'每档达成条件所需rid',
        error_messages={'required': '必选项'}
    )

    arg2 = forms.IntegerField(
        required=True,
        label='领取次数',
        min_value=1,
        help_text=u'每档可领取的次数',
        error_messages={'required': '必选项'}
    )

    arg3 = forms.IntegerField(
        required=True,
        label='奖励VIP经验',
        min_value=1,
        help_text=u'每档可领取的VIP经验',
        error_messages={'required': '必选项'}
    )

    arg4 = forms.IntegerField(
        required=True,
        label='显示顺序',
        min_value=1,
        help_text=u'数字越小,客户端显示档位越靠前',
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        for arg in args:
            if "activity" in arg:
                self.id = arg["activity"]
        super(ActivityZchdRewardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ActivityZchdRewardForm, self).clean()

        activity = models.Activity.objects.get(id=self.id)
        entryID = models.EntryID.objects.get(eid=1)
        entryID.entry_id = entryID.entry_id + 1
        entryID.save()
        entry = models.Entry(aid=entryID.entry_id,
                             order=self.cleaned_data["arg4"],
                             condition=2,
                             arg1=self.cleaned_data["arg1"],
                             arg2=self.cleaned_data["arg2"],
                             arg3=self.cleaned_data["arg3"],
                             arg4=0,
                             arg5=0,
                             arg6="",
                             arg7="",
                             duihuan11=0,
                             duihuan12=0,
                             duihuan13=0,
                             duihuan21=0,
                             duihuan22=0,
                             duihuan23=0,
                             type11=self.items.type1,
                             value12=self.items.value11,
                             value13=self.items.value12,
                             value14=0,
                             type21=self.items.type2,
                             value22=self.items.value21,
                             value23=self.items.value22,
                             value24=0,
                             type31=self.items.type3,
                             value32=self.items.value31,
                             value33=self.items.value32,
                             value34=0,
                             type41=self.items.type4,
                             value42=self.items.value41,
                             value43=self.items.value42,
                             value44=0,
                             activity=activity)
        entry.save()

        self.suc = True
        return self.cleaned_data


##########################################################################
class ResourceFormat():
    op_type = {1000: "装备碎片合成",
               1001: "装备分解",
               1002: "装备改造",
               1003: "装备升星",
               1004: "装备精炼",
               1005: "装备重生",
               1006: "装备扩充",
               1007: "装备强化",
               1008: "装备熔炼",

               2000: "伙伴抽卡",
               2001: "伙伴突破",
               2002: "伙伴进阶",
               2003: "伙伴兑换",
               2004: "伙伴技能",
               2005: "伙伴重生",
               2006: "伙伴充能",
               2007: "伙伴碎片",
               2008: "伙伴分解",
               2009: "伙伴约会",
               2010: "伙伴升品",
               2011: "伙伴命运指针",
               2012: "伙伴回忆激活",
               2013: "伙伴回忆改运",
               2014: "伙伴回忆翻盘",
               2015: "伙伴碎片改造",
               2016: "伙伴队形升级",
               2017: "光环重生",
               2018: "光环升级",
               2019: "被动技能",
               2020: "宠物升级",
               2021: "宠物进阶",
               2022: "宠物沈星",
               2023: "宠物兑换",
               2024: "宠物分解",
               2025: "宠物重生",
               2026: "回忆升星",
               2027: "回忆升星重置",

               3000: "宝物扩充",
               3001: "宝物精炼",
               3002: "宝物合成",
               3003: "宝物强化",
               3004: "宝物重生",
               3005: "宝物免战",
               3006: "宝物免战购买",
               3007: "宝物战斗",
               3008: "宝物扫荡",
               3009: "宝物熔炼",
               3010: "宝物铸造",
               3011: "宝物一键扫荡",
               3012: "宝物升星",

               4000: "商店购买",
               4001: "商店刷新",
               4002: "物品出售",
               4003: "物品使用",
               4004: "改造石购买",
               4005: "家族商店奖励",
               4006: "星河秘境商店奖励",
               4007: "竞技场商店奖励",
               4008: "家族限时商店",
               4009: "物品合成",
               4010: "回忆商店",
               4011: "猎人商店",
               4012: "幸运商店",
               4013: "探宝目标",
               4014: "探宝商店",
               4015: "冰原商店",
               4016: "冰原目标",
               4017: "家族商店EX",
               4018: "充值翻牌商店",
               4019: "宠物商店",
               4020: "宠物商店刷新",
               4021: "魔方商店",

               5000: "副本战斗",
               5001: "副本扫荡",
               5002: "副本购买",
               5003: "副本星数奖励",
               5004: "副本首次通关奖励",
               5005: "副本奇遇挑战",

               5100: "竞技场战斗",
               5101: "竞技场奖励",
               5102: "竞技场扫荡",

               5200: "伙伴副本",
               5201: "伙伴副本刷新",

               5300: "星河秘境副本",
               5301: "星河秘境重置",
               5302: "星河秘境奖励",
               5303: "星河秘境秘宝",
               5304: "星河秘境三星",
               5305: "星河秘境扫荡",

               5400: "押镖刷新",
               5401: "押镖鼓舞",
               5402: "押镖召唤",
               5403: "押镖加速",
               5404: "押镖立即结束",
               5405: "押镖抢夺",
               5406: "押镖奖励",

               5500: "金币副本",

               5600: "boss日常目标",
               5601: "boss副本",

               5700: "猎人大会购买",
               5701: "猎人大会战斗",
               5702: "猎人大会刷新",
               5703: "猎人大会任务",

               5800: "冰原购买",
               5801: "冰原战斗",

               5900: "大师联赛购买",
               5901: "大师联赛战斗",
               5902: "大师联赛目标",

               6000: "军团膜拜",
               6001: "军团创建",
               6002: "军团弹劾",
               6003: "军团副本购买",
               6004: "军团试练奖励",
               6005: "军团过关奖励",
               6006: "军团副本",
               6007: "军团科技学习",
               6008: "军团科技升级",
               6009: "军团红包",
               6010: "红包成就",
               6011: "红包抢夺",
               6012: "家族改名",

               7000: "GM",
               7001: "任务",
               7002: "目标",
               7003: "目标进度",
               7004: "首冲",
               7005: "vip奖励",
               7006: "在线奖励",
               7007: "签到",
               7008: "礼包",
               7009: "邮件",
               7010: "点金",
               7011: "升级",
               7012: "充值",
               7013: "签到30天",
               7014: "赠送能量",
               7015: "时装解锁",
               7016: "时装成就解锁",
               7017: "换名字",

               8000: "充值计划",
               8001: "充值计划购买",
               8002: "普天同庆",
               8003: "开服购买",
               8004: "开服奖励",
               8005: "vip每日礼包",
               8006: "每周礼包",
               8007: "充值计划人数",

               9000: "充值返利",
               9001: "单笔充值",
               9002: "登录送礼",
               9003: "折扣贩售",
               9004: "活跃活动",
               9005: "日期登录",
               9006: "道具兑换",
               9007: "探宝活动",
               9008: "充值翻牌",
               9009: "充值翻牌重置",
               9010: "时装转盘",
               9011: "太空探索",
               9012: "九宫魔方",
               }

    def __init__(self, value2, value3, value4, value5, dt, type):
        self.value2 = value2
        self.value3 = value3
        self.value4 = self.op_type.get(value4, "未知")
        self.value5 = "获取" if value5 == 0 else "消耗"
        self.dt = dt
        if type != 0:
            self.reward = str(type) + " " + str(value2) + " " + str(value3) + " " + str(0) + " "


class jewelformat(ResourceFormat, object):
    def __init__(self, value2, value3, value4, value5, dt, type, sum, cate):
        super(jewelformat, self).__init__(value2, value3, value4, value5, dt, type)
        self.sum = sum
        self.cate = cate


class ViewResourceLogForm(forms.Form):
    '''查询服务器资源日志'''

    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    userid = forms.CharField(
        required=True,
        label='玩家guid',
        max_length=512,
    )

    restype = forms.ChoiceField(
        required=True,
        label='资源类型',
        choices=(
            ("1", "金币"),
            ("2", "钻石"),
            ("5", "战魂"),
            ("6", "合金"),
            ("7", "原力"),
            ("10", "贡献"),
            ("13", "魔王勋章"),
            ("14", "荣誉点"),
            ("20", "幸运点"),
            ("21", "回忆结晶"),
            ("22", "猎人勋章"),
            ("24", "冰晶"),
        ),
        error_messages={'required': '必选项'},
        )

    resget = forms.ChoiceField(
        required=True,
        label='获取/消耗',
        choices=(
            ("2", "两者"),
            ("0", "获取"),
            ("1", "消耗"),
        ),
        error_messages={'required': '必选项'},
        )

    query_date = forms.DateField(
        required=False,
        label='日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        super(ViewResourceLogForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ViewResourceLogForm, self).clean()

        server_select = models.Server.objects.get(name=self.cleaned_data['server'])

        try:
            if self.cleaned_data['resget'] == '2':
                if self.cleaned_data['query_date']:
                    sql = "select value3, value4, value5, dt from resource_t where player_guid = %s and serverid = '%s' and value2 = %s and DATE(dt) = '%s'" % \
                        (self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['restype']), self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value3, value4, value5, dt from resource_t where player_guid = %s and serverid = '%s' and value2 = %s" % \
                        (self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['restype']),)
            else:
                if self.cleaned_data['query_date']:
                    sql = "select value3, value4, value5, dt from resource_t where player_guid = %s and serverid = '%s' and value2 = %s and value5 = %s and DATE(dt) = '%s'" % \
                        (self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['restype']), int(self.cleaned_data['resget']), self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value3, value4, value5, dt from resource_t where player_guid = %s and serverid = '%s' and value2 = %s and value5 = %s" % \
                        (self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['restype']), int(self.cleaned_data['resget']),)

            db = torndb.Connection(host=settings.LOG_QUERY_HOST, database=settings.LOG_QUERY_DB,
                                   user=settings.LOG_QUERY_ROOT, password=settings.LOG_QUERY_PSD, time_zone="+8:00")

            res = db.query(sql)
            for item in res:
                f = ResourceFormat(0, item["value3"], item["value4"], item["value5"], item["dt"], 0)
                self.desc.append(f)
            self.suc = True
        except Exception:
            pass
        finally:
            if db:
                db.close()

        return self.cleaned_data


class ViewItemLogForm(forms.Form):
    '''查询道具资源日志'''

    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    userid = forms.CharField(
        required=True,
        label='玩家guid',
        max_length=512,
    )

    itemtype = forms.ChoiceField(
        required=True,
        label='获取/消耗',
        choices=(
            ("2", "道具"),
            ("4", "装备"),
            ("6", "饰品"),
            ("3", "伙伴"),
        ),
        error_messages={'required': '必选项'},
        )

    itemget = forms.ChoiceField(
        required=True,
        label='获取/消耗',
        choices=(
            ("2", "两者"),
            ("0", "获取"),
            ("1", "消耗"),
        ),
        error_messages={'required': '必选项'},
        )

    query_date = forms.DateField(
        required=False,
        label='日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        self.item_idids = 0
        for arg in args:
            if "item_id" in arg:
                self.item_idids = arg["item_id"]
        super(ViewItemLogForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ViewItemLogForm, self).clean()

        server_select = models.Server.objects.get(name=self.cleaned_data['server'])

        db = "item_t"
        if self.cleaned_data['itemtype'] == '4':
            db = "equip_t"
        elif self.cleaned_data['itemtype'] == '6':
            db = "treasure_t"
        elif self.cleaned_data['itemtype'] == '3':
            db = "role_t"

        try:
            if self.cleaned_data['itemget'] == '2' and self.item_idids != 0:
                if self.cleaned_data['query_date']:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value2 = %s and DATE(dt) = '%s'" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, self.item_idids, self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value2 = %s" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, self.item_idids,)
            elif self.cleaned_data['itemget'] == '2' and self.item_idids == 0:
                if self.cleaned_data['query_date']:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and DATE(dt) = '%s'" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s'" % \
                        (db, self.cleaned_data['userid'], server_select.serverid,)
            elif self.item_idids != 0:
                if self.cleaned_data['query_date']:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value2 = %s and value5 = %s and DATE(dt) = '%s'" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, self.item_idids, int(self.cleaned_data['itemget']), self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value2 = %s and value5 = %s" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, self.item_idids, int(self.cleaned_data['itemget']),)
            else:
                if self.cleaned_data['query_date']:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value5 = %s and DATE(dt) = '%s'" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['itemget']), self.cleaned_data['query_date'].strftime("%Y-%m-%d"),)
                else:
                    sql = "select value2, value3, value4, value5, dt from %s where player_guid = %s and serverid = '%s' and value5 = %s" % \
                        (db, self.cleaned_data['userid'], server_select.serverid, int(self.cleaned_data['itemget']),)

            dbcon = torndb.Connection(host=settings.LOG_QUERY_HOST, database=settings.LOG_QUERY_DB,
                                      user=settings.LOG_QUERY_ROOT, password=settings.LOG_QUERY_PSD, time_zone="+8:00")

            res = dbcon.query(sql)
            for item in res:
                f = ResourceFormat(item["value2"], item["value3"], item["value4"], item["value5"], item["dt"], int(self.cleaned_data['itemtype']))
                self.desc.append(f)
            self.suc = True
        except Exception:
            pass
        finally:
            if dbcon:
                dbcon.close()

        return self.cleaned_data


class ViewJewelLogForm(forms.Form):
    '''查询钻石资源'''
    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )
    itemget = forms.ChoiceField(
        required=True,
        label='获取/消耗',
        choices=(
            ("0", "获取"),
            ("1", "消耗"),
        ),
        error_messages={'required': '必选项'},
    )
    query_date = forms.DateField(
        required=False,
        label='日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        super(ViewJewelLogForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ViewJewelLogForm, self).clean()
        server_select = models.Server.objects.get(name=self.cleaned_data['server'])
        try:
            db = torndb.Connection(host=settings.LOG_QUERY_HOST, database=settings.LOG_QUERY_DB,
                                   user=settings.LOG_QUERY_ROOT, password=settings.LOG_QUERY_PSD, time_zone="+8:00")

            sql = "select value4, value5, dt, sumCate, round((sumCate/sumCount)*100,2) as cate \
                   from (select value4, value5, dt, sum(value3) as sumCate from resource_t where DATE(dt) = '%s' and serverid = '%s' and value2 = 2 and value5 = '%s' group by value4) as a, \
                   (select sum(value3) as sumCount from resource_t where DATE(dt) = '%s' and serverid = '%s' and value2 = 2 and value5 = '%s') as b" % \
                  (self.cleaned_data['query_date'].strftime("%Y-%m-%d"), server_select.serverid, int(self.cleaned_data['itemget']),
                   self.cleaned_data['query_date'].strftime("%Y-%m-%d"), server_select.serverid, int(self.cleaned_data['itemget']),)

            res = db.query(sql)

            for item in res:
                f = jewelformat(2, 0, item['value4'], item['value5'], item['dt'], 0, item['sumCate'], item['cate'])
                self.desc.append(f)
            self.suc = True
        except Exception as e:
            print(Exception, ":", e)
            pass
        finally:
            if db:
                db.close()
        return self.cleaned_data


class PlayerLevelForm(forms.Form):
    '''玩家等级分布查询'''
    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        super(PlayerLevelForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(PlayerLevelForm, self).clean()

        server_select = models.Server.objects.get(name=self.cleaned_data['server'])
        # Time = time.time()
        try:
            db = torndb.Connection(host=server_select.host, database=server_select.dbbase,
                                   user=server_select.dbuser, password=server_select.dbpasswd, time_zone="+8:00")

            sql = "select lev, seven, fourt, thirty, sum, r_num, r_sum \
                   from (select level as lev from player_t where serverid = '%s' group by level) as d LEFT JOIN \
                   (select level, count(level) as seven from player_t where (unix_timestamp(now()) * 1000 - last_login_time) >= 604800000 and serverid = '%s' group by level) as a on a.level = d.lev LEFT JOIN \
                   (select level, count(level) as fourt from player_t where (unix_timestamp(now()) * 1000 - last_login_time) >= 1209600000 and serverid = '%s' group by level) as b on b.level = d.lev LEFT JOIN \
                   (select level, count(level) as thirty from player_t where (unix_timestamp(now()) * 1000 - last_login_time) >= 2592000000 and serverid = '%s' group by level) as c on c.level = d.lev LEFT JOIN \
                   (select level, count(level) as sum from player_t where serverid = '%s' group by level) as e on e.level = d.lev LEFT JOIN \
                   (select level, count(level) as r_num from player_t where total_recharge > 0 and serverid = '%s' group by level) as f on f.level = d.lev LEFT JOIN \
                   (select level, sum(total_recharge) as r_sum from player_t where serverid = '%s' group by level) as g on g.level = d.lev" % \
                  (server_select.serverid, server_select.serverid, server_select.serverid, server_select.serverid, server_select.serverid, server_select.serverid, server_select.serverid,)
            res = db.query(sql)
            for item in res:
                self.desc.append(item)
            self.suc = True
        except Exception as e:
            print(Exception, ":", e)
        finally:
            if db:
                db.close()
        return self.cleaned_data


class PlayerdataForm(forms.Form):
    '''玩家等级/充值前十名'''

    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        super(PlayerdataForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(PlayerdataForm, self).clean()

        server_select = models.Server.objects.get(name=self.cleaned_data['server'])
        try:
            db = torndb.Connection(host=server_select.host, database=server_select.dbbase,
                                   user=server_select.dbuser, password=server_select.dbpasswd, time_zone="+8:00")
            sql = "(select * from(select a.username as usname, b.name as name, b.level as lev from (select guid as gd, username from acc_t where serverid = '%s') as a INNER JOIN \
                  (select * from player_t where serverid = '%s' ORDER BY level DESC LIMIT 10) as b on b.guid = a.gd) as e ORDER BY lev DESC LIMIT 10) UNION ALL \
                  (select * from(select c.username as usname1,d.name as name1, d.total_recharge as total from (select guid as gd, username from acc_t where serverid = '%s') as c INNER JOIN \
                  (select * from player_t where serverid = '%s' ORDER BY total_recharge DESC LIMIT 10) as d on d.guid = c.gd) as f ORDER BY total DESC LIMIT 10)" % \
                  (server_select.serverid, server_select.serverid, server_select.serverid, server_select.serverid,)
            res = db.query(sql)

            item = [dict() for i in range(20)]
            for i in range(0, len(res)):
                if i == 10:
                    break
                item[i]['usname'] = res[i]['usname']
                item[i]['name'] = res[i]['name']
                item[i]['lev'] = res[i]['lev']
            for i in range(10, len(res)):
                j = i - 10
                item[j]['usname1'] = res[i]['usname']
                item[j]['name1'] = res[i]['name']
                item[j]['total'] = res[i]['lev']
            self.desc = item
            self.suc = True
        except Exception as e:
            print(Exception, ":", e)
            pass
        finally:
            if db:
                db.close()
        return self.cleaned_data


class QueryPasswardForm(forms.Form):
    '''查询密码'''

    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    userid = forms.CharField(
        required=True,
        label='玩家账号',
        max_length=40,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.uname = ""
        self.pwd = ""
        self.goolge = ""
        self.old_username = ""
        super(QueryPasswardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(QueryPasswardForm, self).clean()

        res = None
        try:
            db = torndb.Connection(host='47.52.210.125', database='tsuser', user='root', password='1qaz2wsx@39299911', time_zone='+8:00')
            res = db.get("select username, password,google,old_username from user where username = %s", self.cleaned_data['userid'])
            if res:
                self.uname = res["username"]
                self.pwd = res["password"]
                self.goolge = res["google"]
                self.old_username = res["old_username"]
                self.suc = True
            else:
                res = db.get("select username, password,google,old_username from user where old_username = %s", self.cleaned_data['userid'])
                if res:
                    self.uname = res["username"]
                    self.pwd = res["password"]
                    self.goolge = res["google"]
                    self.old_username = res["old_username"]
                    self.suc = True
                else:
                    res = db.get("select username, password,google,old_username from user where google = %s", self.cleaned_data['userid'])
                    if res:
                        self.uname = res["username"]
                        self.pwd = res["password"]
                        self.goolge = res["google"]
                        self.old_username = res["old_username"]
                        self.suc = True
        except Exception:
            pass
        finally:
            if db:
                db.close()

        return self.cleaned_data


class QueryRechargeForm(forms.Form):
    '''查询充值情况'''

    itemget = forms.ChoiceField(
        required=True,
        label='查询区域',
        choices=(
            ("0", "游戏服"),
            ("1", "礼包服"),
            ("2", "充值服"),
        ),
        help_text=u"游戏服:查询游戏服充值情况/礼包服:查询礼包是否被使用/充值服:查询充值记录",
        error_messages={'required': '必选项'},
        )

    server = forms.ModelChoiceField(
        required=False,
        label='选择服务器(可选)',
        queryset=models.Server.objects.all(),
    )

    userid = forms.CharField(
        required=False,
        label='玩家账号(可选)',
        max_length=40,
    )

    libaoid = forms.CharField(
        required=False,
        label='礼包码(可选)',
        max_length=40,
    )

    query_date = forms.DateField(
        required=False,
        label='充值日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.recharges = []
        self.libaos = ""
        super(QueryRechargeForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(QueryRechargeForm, self).clean()

        if self.cleaned_data['itemget'] == '0':
            server_select = models.Server.objects.get(name=self.cleaned_data['server'])
            account = None
            try:
                db = torndb.Connection(host=server_select.host, database="tsjh" + str(server_select.site), user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
                account = db.get("select guid from acc_t where username = %s and serverid = %s", self.cleaned_data['userid'], server_select.serverid)
                if account:
                    if self.cleaned_data['libaoid']:
                        if self.cleaned_data['query_date']:
                            sql = "select rid, orderno, dt from recharge_heitao_t where player_guid = %s and orderno = '%s' and DATE(dt) = '%s'" % (account["guid"], "libao" + self.cleaned_data['libaoid'], self.cleaned_data['query_date'].strftime("%Y-%m-%d"))
                        else:
                            sql = "select rid, orderno, dt from recharge_heitao_t where player_guid = %s and orderno = '%s'" % (account["guid"], "libao" + self.cleaned_data['libaoid'])
                    else:
                        if self.cleaned_data['query_date']:
                            sql = "select rid, orderno, dt from recharge_heitao_t where player_guid = %s and DATE(dt) = '%s'" % (account["guid"], self.cleaned_data['query_date'].strftime("%Y-%m-%d"))
                        else:
                            sql = "select rid, orderno, dt from recharge_heitao_t where player_guid = %s" % (account["guid"])
                    self.recharges = db.query(sql)
            except Exception:
                pass
            finally:
                if db:
                    db.close()
        elif self.cleaned_data['itemget'] == '1':
            try:
                db = torndb.Connection(host="127.0.0.1", database="libao", user="root", password="root", time_zone="+8:00")
                ress = db.get("select used from libao where code = '%s'" % self.cleaned_data['libaoid'])
                if ress:
                    self.libaos = ress["used"]
            except Exception:
                pass
            finally:
                if db:
                    db.close()
        elif self.cleaned_data['itemget'] == '3':
            db = "role_t"

        if account:
            try:
                db = torndb.Connection(host='121.40.80.198', database='tsuser', user='root', password='1qaz2wsx@39299911', time_zone='+8:00')
                res = db.get("select username, password from user where username = %s", self.cleaned_data['userid'])
                self.uname = res["username"]
                self.pwd = res["password"]
                self.suc = True
            except Exception:
                pass
            finally:
                if db:
                    db.close()

        return self.cleaned_data


class UploadServerlistForm(forms.Form):
    '''上传服务器列表'''

    server = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    userid = forms.CharField(
        required=True,
        label='服务器名字',
        max_length=40,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(QueryPasswardForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(QueryPasswardForm, self).clean()

        server_select = models.Server.objects.get(name=self.cleaned_data['server'])
        account = None
        try:
            db = torndb.Connection(host=server_select.host, database="tsjh" + str(server_select.site), user="root", password="1qaz2wsx@39299911", time_zone="+8:00")
            account = db.get("select guid from acc_t where username = %s and serverid = %s", self.cleaned_data['userid'], server_select.serverid)
        except Exception:
            pass
        finally:
            if db:
                db.close()

        if account:
            try:
                db = torndb.Connection(host='121.40.80.198', database='tsuser', user='root', password='1qaz2wsx@39299911', time_zone='+8:00')
                res = db.get("select username, password from user where username = %s", self.cleaned_data['userid'])
                self.uname = res["username"]
                self.pwd = res["password"]
                self.suc = True
            except Exception:
                pass
            finally:
                if db:
                    db.close()

        return self.cleaned_data


class QueueGonggaolist(forms.Form):
    '''下载公告'''

    pingtai = forms.ModelChoiceField(
        required=True,
        label='选择平台',
        queryset=models.GonggaoPlatform.objects.all(),
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.con = "无"
        self.suc = False
        super(QueueGonggaolist, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(QueueGonggaolist, self).clean()

        gpf = models.GonggaoPlatform.objects.get(gname=self.cleaned_data['pingtai'])
        self.suc = helper.oss_download_file(gpf.fqudao, gpf.fname, "gonggao.txt")
        return self.cleaned_data


class ModifyGonggaolist(forms.Form):
    '''上传公告'''

    pingtai = forms.ModelChoiceField(
        required=True,
        label='选择平台',
        queryset=models.GonggaoPlatform.objects.all(),
        error_messages={'required': '必选项'}
    )

    file1 = forms.FileField(
        required=True,
        label=u"公告文本",
        help_text=u"文件名字为gonggao.txt",
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(ModifyGonggaolist, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ModifyGonggaolist, self).clean()

        ff = self.cleaned_data["file1"]
        if ff.name != "gonggao.txt":
            raise forms.ValidationError('文件名不正确')
        if not ff:
            raise forms.ValidationError('无效文件')

        gpf = models.GonggaoPlatform.objects.get(gname=self.cleaned_data['pingtai'])
        self.suc = helper.oss_upload_text_from_fp(gpf.fqudao, gpf.fname, ff.file)
        return self.cleaned_data


class ModifyPlayerInfo(forms.Form):
    '''修改信息'''

    server_select = forms.ModelChoiceField(
        required=True,
        label='选择服务器',
        queryset=models.Server.objects.all(),
        empty_label=None,
        error_messages={'required': '必选项'}
    )

    accname = forms.CharField(
        required=True,
        label='玩家id',
        max_length=512,
        error_messages={'required': '必选项'}
    )

    yueka = forms.ChoiceField(
        required=True,
        label='是否踢下线',
        choices=(
            ("0", "否"),
            ("1", "是"),
        ),
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.items = None
        self.suc = False
        super(ModifyPlayerInfo, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ModifyPlayerInfo, self).clean()

        server = models.Server.objects.get(name=self.cleaned_data['server_select'])
        accname = self.cleaned_data['accname']
        db = None
        error = ''
        guid = 0
        try:
            db = pymysql.connect(user=server.dbuser, passwd=server.dbpasswd, db=server.dbbase, host=server.host, charset='utf8')
            cur = db.cursor()
            sql = "select guid from acc_t where username = %s and serverid = %s"
            param = (accname, server.serverid)
            cur.execute(sql, param)
            res = cur.fetchall()
            if len(res) != 1:
                error = '没有该玩家'
            else:
                guid = res[0][0]
        except Exception:
            raise forms.ValidationError('内部错误')
        finally:
            if db:
                db.close()

        if error != '':
            raise forms.ValidationError(error)

        ctype = self.items.type1
        cvalue1 = self.items.value11
        cvalue2 = self.items.value12

        msg = rpc_pb2.tmsg_fight_treasure()
        msg.guid = guid
        msg.template_id = int(ctype)
        msg.enhance = int(cvalue1)
        msg.star = int(cvalue2)
        msg.star_exp = int(self.cleaned_data['yueka'])
        s = msg.SerializeToString()
        httpClient = None
        try:
            httpClient = httplib.HTTPConnection(server.host + ":" + str(server.port))
            headers = {'Content-type': 'text/xml;charset=UTF-8'}
            httpClient.request("POST", opcodes.op_url("TMSG_HUODONG_MODIFY"), s, headers)
            response = httpClient.getresponse()
            print(response)
            self.suc = True
        except Exception:
            forms.ValidationError("内部错误")
        finally:
            if httpClient:
                httpClient.close()
        '''
        try:
            data_log = ChangeLog(guid=guid, username=accname, serverid=server.serverid,
                                 template=ctype, enhance=cvalue1, star=cvalue2, star_exp=self.cleaned_data['yueka'],
                                 dt=datetime.datetime.now().replace(tzinfo=None))
            data_log.save()
        except Exception:
            pass
        '''

        if error != '':
            raise forms.ValidationError(error)

        return self.cleaned_data


class ServerTimeLineForm(forms.Form):
    pt_select = forms.ModelChoiceField(
        required=True,
        label='选择平台',
        queryset=models.ServerList.objects.filter(zhuserver=1),
        empty_label=None,
        error_messages={'required': '必选项'}
    )

    start = forms.DateTimeField(
        required=True,
        label='开服时间',
        widget=forms.DateTimeInput,
        help_text=u"格式:2016-06-12 11:00",
        error_messages={'required': '必选项'},
    )

    servername = forms.CharField(
        required=True,
        label='服务器名字',
        max_length=30,
        error_messages={'required': '必选项'}
    )

    houtainame = forms.CharField(
        required=True,
        label='后台名字',
        max_length=30,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        super(ServerTimeLineForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ServerTimeLineForm, self).clean()

        pt = self.cleaned_data['pt_select']
        allservers = pt.servers.all()
        choose_server = None
        choose_site = -1
        for server in allservers:
            if server.site0 == 0:
                choose_site = 0
            elif server.site1 == 0:
                choose_site = 1
            elif server.site2 == 0:
                choose_site = 2
            elif server.site3 == 0:
                choose_site = 3
            elif server.site4 == 0:
                choose_site = 4
            elif server.site5 == 0:
                choose_site = 5
            elif server.site6 == 0:
                choose_site = 6
            elif server.site7 == 0:
                choose_site = 7
            elif server.site8 == 0:
                choose_site = 8
            elif server.site9 == 0:
                choose_site = 9
            if choose_site != -1:
                choose_server = server
                break

        if not choose_server:
            raise forms.ValidationError('找不到可开服的服务器')

        has_valid_server_server = True
        try:
            valid_server_site = "tsjh" + str(choose_site)
            valid_server = torndb.Connection(host=choose_server.host, database=valid_server_site, user="root", password="1qaz2wsx@39299911", time_zone='+8:00')
            valid_server_res = valid_server.query("select TABLE_NAME  from information_schema.TABLES where table_schema = '%s'" % valid_server_site)
            valid_server_res_list = [res["TABLE_NAME"] for res in valid_server_res]
            if 'global_t' in valid_server_res_list:
                valid_global_t = valid_server.get("select guid from global_t")
                if valid_global_t:
                    gtool_server = torndb.Connection(host="127.0.0.1", database="gttool", user="root", password="root", time_zone='+8:00')
                    exist_serverid = helper.get_guid_server_by_global(valid_global_t["guid"])
                    sql = "select id from engine_server where serverid = %s and merge = 0" % exist_serverid
                    exist_serverid_exist = gtool_server.get(sql)
                    if exist_serverid_exist:
                        has_valid_server_server = False
        except Exception as e:
            if valid_server:
                valid_server.close()
            raise forms.ValidationError(e)

        if not has_valid_server_server:
            raise forms.ValidationError('找不到可开服的服务器')

        pt.serverid = pt.serverid + 1
        pt.save()
        newServer = models.ServerStart(opentime=datetime.datetime.fromtimestamp(time.mktime(self.cleaned_data['start'].timetuple())),
                                       openflag=False,
                                       host=choose_server.host,
                                       serverid=pt.serverid,
                                       version=pt.version,
                                       site=choose_site,
                                       hostip=choose_server.hostin,
                                       servername=self.cleaned_data['servername'],
                                       houtai_name=self.cleaned_data['houtainame'],
                                       sync=False,
                                       sync_error="",
                                       openstat=0,
                                       ptid=pt.id,
                                       zhuserver=True,
                                       pt=pt.pt,
                                       channel=pt.qudao,
                                       hebing=pt.hebing)
        newServer.save()
        if choose_site == 0:
            choose_server.site0 = 1
        elif choose_site == 1:
            choose_server.site1 = 1
        elif choose_site == 2:
            choose_server.site2 = 1
        elif choose_site == 3:
            choose_server.site3 = 1
        elif choose_site == 4:
            choose_server.site4 = 1
        elif choose_site == 5:
            choose_server.site5 = 1
        elif choose_site == 6:
            choose_server.site6 = 1
        elif choose_site == 7:
            choose_server.site7 = 1
        elif choose_site == 8:
            choose_server.site8 = 1
        elif choose_site == 9:
            choose_server.site9 = 1
        choose_server.save()
        self.suc = True

        same_qudaos = models.ServerList.objects.filter(osspt=pt.osspt).exclude(qudao=pt.qudao, pt=pt.pt)
        if same_qudaos:
            xuhao = 0
            for qu in same_qudaos:
                xuhao += 1
                newServer1 = models.ServerStart(opentime=datetime.datetime.fromtimestamp(time.mktime(self.cleaned_data['start'].timetuple())),
                                                openflag=False,
                                                host=newServer.host,
                                                serverid=newServer.serverid * 100 + xuhao,
                                                version=newServer.version,
                                                site=newServer.site,
                                                hostip=newServer.hostip,
                                                servername=newServer.servername,
                                                houtai_name=newServer.houtai_name,
                                                sync=False,
                                                sync_error="",
                                                openstat=2,
                                                ptid=qu.id,
                                                zhuserver=False,
                                                pt=qu.pt,
                                                channel=qu.qudao,
                                                hebing=qu.hebing)
                newServer1.save()

        return self.cleaned_data


class ServerTimeLineModifyNameForm(forms.Form):
    start = forms.DateTimeField(
        required=False,
        label='开服时间',
        widget=forms.DateTimeInput,
        help_text=u"格式:2016-06-12 11:00",
    )

    servername = forms.CharField(
        required=False,
        label='服务器名字',
        max_length=30,
    )

    houtainame = forms.CharField(
        required=False,
        label='后台名字',
        max_length=30,
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        for arg in args:
            if "id" in arg:
                self.id = arg["id"]
        super(ServerTimeLineModifyNameForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ServerTimeLineModifyNameForm, self).clean()

        serverstart = models.ServerStart.objects.get(id=self.id)

        if self.cleaned_data["start"] is not None:
            serverstart.opentime = datetime.datetime.fromtimestamp(time.mktime(self.cleaned_data['start'].timetuple()))
        if self.cleaned_data["servername"] is not None and self.cleaned_data["servername"] != "":
            serverstart.servername = self.cleaned_data["servername"]
        if self.cleaned_data["houtainame"] is not None and self.cleaned_data["houtainame"] != "":
            serverstart.houtai_name = self.cleaned_data["houtainame"]

        serverstart.save()
        self.suc = True
        return self.cleaned_data


class Huodongformat(object):
    def __init__(self, value1, value2, value3):
        self.value1 = value1
        self.value2 = value2
        self.value3 = value3


class ViewHuodongLogForm(forms.Form):
    '''查询活动资源'''
    itemget = forms.ChoiceField(
        required=True,
        label='活动类型',
        choices=(
            ("9000", "充值返利"),
            ("9001", "单笔充值"),
            ("9002", "登陆送礼"),
            ("9003", "折扣贩售"),
            ("9004", "活跃活动"),
            ("9006", "道具兑换"),
            ("9007", "探宝活动"),
            ("9010", "时装转盘"),
            ("9011", "太空漫游"),
            ("9012", "九宫魔方"),
        ),
        error_messages={'required': '必选项'},
    )
    query_date_start = forms.DateField(
        required=False,
        label='开始日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
        error_messages={'required': '必选项'},
    )
    query_date_end = forms.DateField(
        required=False,
        label='结束日期(可选)',
        widget=forms.DateInput,
        help_text=u"格式:2016-06-12",
        error_messages={'required': '必选项'},
    )

    def __init__(self, *args, **kwargs):
        self.suc = False
        self.desc = []
        super(ViewHuodongLogForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(ViewHuodongLogForm, self).clean()

        try:
            db = torndb.Connection(host=settings.LOG_QUERY_HOST, database=settings.LOG_QUERY_DB,
                                   user=settings.LOG_QUERY_ROOT, password=settings.LOG_QUERY_PSD, time_zone="+8:00")

            if self.cleaned_data['query_date_start'] and self.cleaned_data['query_date_end']:
                sql = "select value3, COUNT(player_guid), COUNT(DISTINCT player_guid) from huodong_t where value2 = %s and DATE(dt) >= '%s' and DATE(dt) <= '%s' GROUP BY value3" % \
                        (self.cleaned_data['itemget'], self.cleaned_data['query_date_start'].strftime("%Y-%m-%d"), self.cleaned_data['query_date_end'].strftime("%Y-%m-%d"))
            elif self.cleaned_data['query_date_start']:
                sql = "select value3, COUNT(player_guid), COUNT(DISTINCT player_guid) from huodong_t where value2 = %s and DATE(dt) >= '%s' GROUP BY value3" % \
                        (self.cleaned_data['itemget'], self.cleaned_data['query_date_start'].strftime("%Y-%m-%d"))
            elif self.cleaned_data['query_date_end']:
                sql = "select value3, COUNT(player_guid), COUNT(DISTINCT player_guid) from huodong_t where value2 = %s and DATE(dt) <= '%s' GROUP BY value3" % \
                        (self.cleaned_data['itemget'], self.cleaned_data['query_date_end'].strftime("%Y-%m-%d"))
            else:
                sql = "select value3, COUNT(player_guid), COUNT(DISTINCT player_guid) from huodong_t where value2 = %s GROUP BY value3" % \
                        (self.cleaned_data['itemget'])

            print(sql)
            res = db.query(sql)

            for item in res:
                f = Huodongformat(item['value3'], item['COUNT(player_guid)'], item['COUNT(DISTINCT player_guid)'])
                self.desc.append(f)
            self.suc = True
        except Exception as e:
            print(Exception, ":", e)
        finally:
            if db:
                db.close()
        return self.cleaned_data
