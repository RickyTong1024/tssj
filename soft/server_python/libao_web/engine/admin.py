from django.contrib import admin
from .models import Server


@admin.register(Server)
class ServerAdmin(admin.ModelAdmin):
    list_display = ('name', 'host', 'serverid', 'site')
    ordering = ('name',)
