from django.db import models


class Platform(models.Model):
    # 大区
    name = models.CharField("名字", max_length=40)

    def __unicode__(self):
        return self.name


class Server(models.Model):
    # 服务器
    name = models.CharField("服务器名", max_length=40)
    host = models.CharField("服务器地址", max_length=40)
    serverid = models.IntegerField("服务器id")
    site = models.IntegerField("物理位置")
    # platform = models.ForeignKey(Platform, related_name='servers')

    def __str__(self):
        return self.name

    class Meta:
        ordering = ["name"]

    @property
    def port(self):
        return 8000 + 10 * self.site
