from django.contrib import admin
from engine.models import Server, GonggaoPlatform, ServerList, OpenServer, ServerStart, MyUser, TongjiQudao, MonitorSwitch


# Register your models here.
class ServerAdmin(admin.ModelAdmin):
    list_display = ('name', 'host', 'serverid', 'site', 'merge')
    ordering = ('name',)


class MyUserAdmin(admin.ModelAdmin):
    fields = ('user', 'name', 'qudao1', 'qudao2', 'qudaon', 'tag')


admin.site.register(Server, ServerAdmin)
admin.site.register(GonggaoPlatform)
admin.site.register(ServerList)
admin.site.register(OpenServer)
admin.site.register(ServerStart)
admin.site.register(TongjiQudao)
admin.site.register(MyUser, MyUserAdmin)
admin.site.register(MonitorSwitch)
