from django.db import models
from django.contrib.auth.models import User
# from django.db.models.signals import post_save


class MyUser(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE, related_name='profile')
    name = models.CharField("用户名", max_length=40)
    qudao1 = models.IntegerField("服务器id开始")
    qudao2 = models.IntegerField("服务器id结束")
    qudaon = models.CharField("渠道", max_length=40)
    tag = models.BooleanField()

    def __unicode__(self):
        return self.name


class Server(models.Model):
    name = models.CharField("服务器名", max_length=40)
    host = models.CharField("服务器地址", max_length=40)
    serverid = models.IntegerField("服务器id")
    site = models.IntegerField("物理位置")
    merge = models.IntegerField("合并区服")

    def __unicode__(self):
        return self.name

    class Meta:
        ordering = ["name"]

    @property
    def dbuser(self):
        return "root"

    @property
    def dbpasswd(self):
        return "1qaz2wsx@39299911"

    @property
    def dbbase(self):
        return "tsjh" + str(self.site)

    @property
    def port(self):
        return 8000 + 10 * self.site


class UseAccess(models.Model):
    #
    class Meta:
        permissions = (
            ("serverlook", "serverlook"),
            ("userlook", "userlook"),
            ("mail_one", "mail_one"),
            ("mail_all", "mail_all"),
            ("gonggao", "gonggao"),
            ("dingdan", "dingdan"),
            ("moni", "moni"),
            ("fenghao", "fenghao"),
            ("looklibao", "looklibao"),
            ("clibao", "clibao"),
            ("dlibao", "dlibao"),
            ("activity_group", "activity_group"),
            ("activity_modify", "activity_modify"),
            ("activity_modify_modify", "activity_modify_modify"),
            ("activity_group_modify_name", "activity_group_modify_name"),
            ("activity_sync", "activity_sync"),
            ("activity_group_delete", "activity_group_delete"),
            ("activity_reward_czfl", "activity_reward_czfl"),
            ("activity_reward_dbcz", "activity_reward_dbcz"),
            ("activity_reward_dlsl", "activity_reward_dlsl"),
            ("activity_reward_zkfs", "activity_reward_zkfs"),
            ("activity_reward_djdh", "activity_reward_djdh"),
            ("activity_reward_hyhd", "activity_reward_hyhd"),
            ("activity_reward_rqdl", "activity_reward_rqdl"),
            ("activity_reward_zchd", "activity_reward_zchd"),
            ("activity_reward_modify", "activity_reward_modify"),
            ("activity_delete", "activity_delete"),
            ("activity_copy", "activity_copy"),
            ("activity_reward_delete", "activity_reward_delete"),
            ("activity_time_line", "activity_time_line"),
            ("activity_time_line_add", "activity_time_line_add"),
            ("activity_time_line_delete", "activity_time_line_delete"),
            ("tongji_normal", "tongji_normal"),
            ("tongji_liucun", "tongji_liucun"),
            ("view_resource", "view_resource"),
            ("view_item", "view_item"),
            ("view_jewel", "view_jewel"),
            ("player_level", "player_level"),
            ("player_data", "player_data"),
            ("query_passward", "query_passward"),
            ("mail_show", "mail_show"),
            ("modify_player", "modify_player"),
            ("get_gonggao", "get_gonggao"),
            ("set_gonggao", "set_gonggao"),
            ("tongji_qudao_normal", "tongji_qudao_normal"),
            ("tongji_qudao_normal_xiangxi", "tongji_qudao_normal_xiangxi"),
            ("tongji_qudao_liucun_xiangxi", "tongji_qudao_liucun_xiangxi"),
            ("tongji_qudao_liucun", "tongji_qudao_liucun"),
            ("view_huodong", "view_huodong"),
            ("tongji_normal_huoyue", "tongji_normal_huoyue"),
        )


# ########################活动##################################
class ActivityTime(models.Model):
    """时间线"""

    startime = models.DateTimeField()
    endtime = models.DateTimeField()
    sync = models.BooleanField()
    isnew = models.BooleanField()
    endday = models.IntegerField()
    gp = models.IntegerField()


class ActivityGroup(models.Model):
    """活动组"""

    name = models.CharField(max_length=80)
    isjieri = models.IntegerField()
    item_name1 = models.CharField(max_length=80)
    item_name2 = models.CharField(max_length=80)
    item_des1 = models.CharField(max_length=80)
    item_des2 = models.CharField(max_length=80)
    create_time = models.DateTimeField()
    timelines = models.ManyToManyField(ActivityTime)

    def __unicode__(self):
        return self.name


class ActivityID(models.Model):
    """活动同步id"""

    aid = models.IntegerField()
    type = models.IntegerField()


class EntryID(models.Model):
    """奖励Id"""

    eid = models.IntegerField()
    entry_id = models.IntegerField()


class Activity(models.Model):
    """活动"""

    aid = models.IntegerField()
    type = models.IntegerField()
    atype = models.IntegerField()
    subtype = models.IntegerField()
    name = models.CharField(max_length=40)
    kaifu_start = models.IntegerField()
    kaifu_end = models.IntegerField()
    kaikai_start = models.IntegerField()
    huodong_name = models.CharField(max_length=40)
    start_show_day = models.IntegerField()
    end_show_day = models.IntegerField()
    group = models.ForeignKey(ActivityGroup, on_delete=models.CASCADE, related_name='activitys')


class Entry(models.Model):
    """奖励"""

    aid = models.IntegerField()
    order = models.IntegerField()
    condition = models.IntegerField()
    arg1 = models.IntegerField()
    arg2 = models.IntegerField()
    arg3 = models.IntegerField()
    arg4 = models.IntegerField()
    arg5 = models.IntegerField()
    arg6 = models.CharField(max_length=80)
    arg7 = models.CharField(max_length=40)
    duihuan11 = models.IntegerField()
    duihuan12 = models.IntegerField()
    duihuan13 = models.IntegerField()
    duihuan21 = models.IntegerField()
    duihuan22 = models.IntegerField()
    duihuan23 = models.IntegerField()
    type11 = models.IntegerField()
    value12 = models.IntegerField()
    value13 = models.IntegerField()
    value14 = models.IntegerField()
    type21 = models.IntegerField()
    value22 = models.IntegerField()
    value23 = models.IntegerField()
    value24 = models.IntegerField()
    type31 = models.IntegerField()
    value32 = models.IntegerField()
    value33 = models.IntegerField()
    value34 = models.IntegerField()
    type41 = models.IntegerField()
    value42 = models.IntegerField()
    value43 = models.IntegerField()
    value44 = models.IntegerField()
    activity = models.ForeignKey(Activity, on_delete=models.CASCADE, related_name='entrys')


class ActivitySyncHistory(models.Model):
    '''同步历史'''

    sync_infos = models.CharField(max_length=4096)
    start_time = models.DateTimeField()
    end_time = models.DateTimeField()
    synctime = models.DateTimeField()
    group = models.ForeignKey(ActivityGroup, on_delete=models.CASCADE, related_name='historys')

######################################################################


class TongjiNormal(models.Model):
    '''一般统计数据'''

    new_user_num = models.IntegerField()
    huo_user_num = models.IntegerField()
    total_rmb = models.IntegerField()
    total_user_num = models.IntegerField()
    total_num = models.IntegerField()
    dpu = models.FloatField()
    aku = models.FloatField()
    dt = models.DateField()


class TongjiLiucun(models.Model):
    '''留存统计数据'''

    dt = models.DateField()
    total_num = models.IntegerField()
    liucun_2ltv = models.IntegerField()
    liucun_2rate = models.FloatField()
    liucun_3ltv = models.IntegerField()
    liucun_3rate = models.FloatField()
    liucun_4ltv = models.IntegerField()
    liucun_4rate = models.FloatField()
    liucun_5ltv = models.IntegerField()
    liucun_5rate = models.FloatField()
    liucun_6ltv = models.IntegerField()
    liucun_6rate = models.FloatField()
    liucun_7ltv = models.IntegerField()
    liucun_7rate = models.FloatField()
    liucun_15ltv = models.IntegerField()
    liucun_15rate = models.FloatField()
    liucun_30ltv = models.IntegerField()
    liucun_30rate = models.FloatField()


class TongjiNormalServer(models.Model):
    '''一般服务器统计数据'''

    new_user_num = models.IntegerField()
    huo_user_num = models.IntegerField()
    total_rmb = models.IntegerField()
    total_user_num = models.IntegerField()
    total_num = models.IntegerField()
    dpu = models.FloatField()
    aku = models.FloatField()
    serverid = models.IntegerField()
    dt = models.DateField()


class TongjiQudao(models.Model):
    '''统计的渠道信息'''

    qudao_name = models.CharField("渠道", max_length=40)

    def __unicode__(self):
        return self.qudao_name


class TongjiQudaoLicun(models.Model):
    '''渠道留存'''

    dt = models.DateField()
    total_num = models.IntegerField()
    liucun_2ltv = models.IntegerField()
    liucun_2rate = models.FloatField()
    liucun_3ltv = models.IntegerField()
    liucun_3rate = models.FloatField()
    liucun_4ltv = models.IntegerField()
    liucun_4rate = models.FloatField()
    liucun_5ltv = models.IntegerField()
    liucun_5rate = models.FloatField()
    liucun_6ltv = models.IntegerField()
    liucun_6rate = models.FloatField()
    liucun_7ltv = models.IntegerField()
    liucun_7rate = models.FloatField()
    liucun_15ltv = models.IntegerField()
    liucun_15rate = models.FloatField()
    liucun_30ltv = models.IntegerField()
    liucun_30rate = models.FloatField()
    group = models.ForeignKey(TongjiQudao, on_delete=models.CASCADE, related_name='liucuns')


class TongjiQudaoServer(models.Model):
    '''渠道的统计数据'''

    new_user_num = models.IntegerField()
    huo_user_num = models.IntegerField()
    total_rmb = models.IntegerField()
    total_user_num = models.IntegerField()
    total_num = models.IntegerField()
    dpu = models.FloatField()
    aku = models.FloatField()
    dt = models.DateField()
    group = models.ForeignKey(TongjiQudao, on_delete=models.CASCADE, related_name='servers')


# *************************************************
class GonggaoPlatform(models.Model):
    '''公告平台'''

    gname = models.CharField(max_length=64)
    fname = models.CharField(max_length=64)
    fqudao = models.CharField(max_length=64)
    qudao = models.IntegerField("服务器id开始")

    def __unicode__(self):
        return self.gname


class PostLog(models.Model):
    '''邮件日志'''

    puser = models.CharField(max_length=1024)
    title = models.CharField(max_length=1024)
    text = models.CharField(max_length=1024)
    content = models.CharField(max_length=1024)
    reward = models.CharField(max_length=1024)
    jewel = models.IntegerField()
    suc = models.BooleanField()
    dt = models.DateTimeField()


class MoniLog(models.Model):
    '''模拟充值日志'''

    uname = models.CharField(max_length=1024)
    server = models.IntegerField()
    acc = models.CharField(max_length=1024)
    pid = models.IntegerField()
    suc = models.BooleanField()
    dt = models.DateTimeField()


class ServerStart(models.Model):
    '''开服时间线'''

    opentime = models.DateTimeField()
    openflag = models.BooleanField()
    host = models.CharField("服务器地址", max_length=128)
    serverid = models.IntegerField("服务器id")
    site = models.IntegerField("服务器物理位置")
    version = models.CharField("当前版本", max_length=64)
    hostip = models.CharField("内网服务器地址", max_length=128)
    openstat = models.IntegerField()
    servername = models.CharField("服务器名字", max_length=128)
    houtai_name = models.CharField("服务器后台名字", max_length=128)
    sync = models.BooleanField()
    sync_error = models.CharField("同步结果", max_length=64)
    ptid = models.IntegerField()
    zhuserver = models.BooleanField()
    pt = models.CharField("平台", max_length=64)
    channel = models.CharField("渠道", max_length=64)
    hebing = models.BooleanField()

    def __unicode__(self):
        return str(self.serverid) + self.servername


class ServerList(models.Model):
    '''开服平台'''

    pt = models.CharField("平台", max_length=64)
    qudao = models.CharField("渠道", max_length=64)
    version = models.CharField("当前版本", max_length=64)
    serverid = models.IntegerField("当前服务器ID")
    path = models.CharField("部署源代码路径", max_length=64)
    osspath = models.CharField("oss服务器列表路径", max_length=64)
    huodong_path = models.IPAddressField("活动地址")
    recharge_path = models.IPAddressField("充值地址")
    hebing = models.BooleanField("是否使用合并配置表")
    zhuserver = models.BooleanField("是否是需要配置服务器")
    osspt = models.CharField("分类平台", max_length=64)

    def __unicode__(self):
        return self.pt + ":" + self.qudao


class OpenServer(models.Model):
    '''开服服务器'''

    host = models.IPAddressField("外网地址")
    hostin = models.IPAddressField("内网地址")
    site0 = models.IntegerField()
    site1 = models.IntegerField()
    site2 = models.IntegerField()
    site3 = models.IntegerField()
    site4 = models.IntegerField()
    site5 = models.IntegerField()
    site6 = models.IntegerField()
    site7 = models.IntegerField()
    site8 = models.IntegerField()
    site9 = models.IntegerField()
    group = models.ForeignKey(ServerList, on_delete=models.CASCADE, related_name='servers')

    def __unicode__(self):
        return self.host


class TongjiNormalHuoyue(models.Model):
    huo_user_num = models.IntegerField()
    serverid = models.IntegerField()
    name = models.CharField("服务器名", max_length=40)
    dt = models.DateField()


class MonitorHistory(models.Model):
    '''同步历史'''

    sync_infos = models.CharField(max_length=4096)
    synctime = models.DateTimeField()


class MonitorSwitch(models.Model):
    '''同步历史'''
    open = models.BooleanField()
    monitor_host = models.IPAddressField()
