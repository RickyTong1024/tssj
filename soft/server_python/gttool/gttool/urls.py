from django.conf.urls import include, url
from django.contrib import admin
from django.views.generic import RedirectView
from django.contrib.staticfiles import views
from django.conf import settings

admin.autodiscover()

urlpatterns = [
    url(r'^admin/', include(admin.site.urls)),
    url(r'^$', 'engine.views.index'),
    url(r'^favicon.ico$', RedirectView.as_view(url='/static/img/favicon.png',permanent=False)),

    url(r'^login/$', 'engine.views.login',name="login"),
    url(r'^logout/$',  'django.contrib.auth.views.logout', {'next_page': '/login/'},name="logout"),

    url(r'^manage/serverlook/$', 'engine.manage.serverlook',name="serverlook"),
    url(r'^manage/userlook/$', 'engine.manage.userlook',name="userlook"),
    url(r'^manage/huoban/$', 'engine.manage.huoban',name="huoban"),
    url(r'^manage/zhuangbei/$', 'engine.manage.zhuangbei',name="zhuangbei"),
    url(r'^manage/baowu/$', 'engine.manage.baowu',name="baowu"),
    url(r'^manage/baowuall/$', 'engine.manage.baowu_all',name="baowu_all"),
    url(r'^manage/daoju_all/$', 'engine.manage.daoju_all',name="daoju_all"),
    url(r'^manage/userlook/$', 'engine.manage.userlook',name="userlook"),
    url(r'^manage/userlook/$', 'engine.manage.userlook',name="userlook"),
    url(r'^manage/mail_one/$', 'engine.manage.mail_one',name="mail_one"),
    url(r'^manage/mail_all/$', 'engine.manage.mail_all',name="mail_all"),
    url(r'^manage/mail_show/$', 'engine.manage.mail_show',name="mail_show"),
    url(r'^manage/gonggao/$', 'engine.manage.gonggao',name="gonggao"),
    url(r'^manage/dingdan/$', 'engine.manage.dingdan',name="dingdan"),
    url(r'^manage/moni/$', 'engine.manage.moni',name="moni"),
    url(r'^manage/moni_show/$', 'engine.manage.moni_show',name="moni_show"), 
    url(r'^manage/fenghao/$', 'engine.manage.fenghao',name="fenghao"),
    url(r'^manage/looklibao/$', 'engine.manage.looklibao',name="looklibao"),
    url(r'^manage/clibao/$', 'engine.manage.clibao',name="clibao"),
    url(r'^manage/dlibao/$', 'engine.manage.dlibao',name="dlibao"),
    url(r'^manage/activity_group/$', 'engine.manage.activity_group',name="activity_group"),
    url(r'^manage/activity_group_modify_name/(?P<id>\d+)/$', 'engine.manage.activity_group_modify_name',name="activity_group_modify_name"),
    url(r'^manage/activity_time/$', 'engine.manage.activity_time_line',name="activity_time"),
    url(r'^manage/activity_time_line_add/(?P<id>\d+)/$', 'engine.manage.activity_time_line_add',name="activity_time_line_add"),
    url(r'^manage/activity_time_line_delete/(?P<id>\d+)/$', 'engine.manage.activity_time_line_delete',name="activity_time_line_delete"),
    url(r'^manage/activity_modify/(?P<id>\d+)/$', 'engine.manage.activity_modify',name="activity_modify"),
    url(r'^manage/activity_modify_modify/(?P<id>\d+)/$', 'engine.manage.activity_modify_modify',name="activity_modify_modify"),
    url(r'^manage/activity_sync/(?P<id>\d+)/$', 'engine.manage.activity_sync',name="activity_sync"),
    url(r'^manage/activity_group_delete/(?P<id>\d+)/$', 'engine.manage.activity_group_delete',name="activity_group_delete"),
    url(r'^manage/activity_reward_czfl/(?P<id>\d+)/$', 'engine.manage.activity_reward_czfl',name="activity_reward_czfl"),
    url(r'^manage/activity_reward_dbcz/(?P<id>\d+)/$', 'engine.manage.activity_reward_dbcz',name="activity_reward_dbcz"),
    url(r'^manage/activity_reward_dlsl/(?P<id>\d+)/$', 'engine.manage.activity_reward_dlsl',name="activity_reward_dlsl"),
    url(r'^manage/activity_reward_zkfs/(?P<id>\d+)/$', 'engine.manage.activity_reward_zkfs',name="activity_reward_zkfs"),
    url(r'^manage/activity_reward_djdh/(?P<id>\d+)/$', 'engine.manage.activity_reward_djdh',name="activity_reward_djdh"),
    url(r'^manage/activity_reward_hyhd/(?P<id>\d+)/$', 'engine.manage.activity_reward_hyhd',name="activity_reward_hyhd"),
    url(r'^manage/activity_reward_rqdl/(?P<id>\d+)/$', 'engine.manage.activity_reward_rqdl',name="activity_reward_rqdl"),
    url(r'^manage/activity_reward_zchd/(?P<id>\d+)/$', 'engine.manage.activity_reward_zchd',name="activity_reward_zchd"),
    url(r'^manage/activity_delete/(?P<id>\d+)/$', 'engine.manage.activity_delete',name="activity_delete"),
    url(r'^manage/activity_copy/(?P<id>\d+)/$', 'engine.manage.activity_copy',name="activity_copy"),
    url(r'^manage/activity_reward_delete/(?P<id>\d+)/$', 'engine.manage.activity_reward_delete',name="activity_reward_delete"),
    url(r'^manage/tongji_normal/$', 'engine.manage.tongji_normal',name="tongji_normal"),
    url(r'^manage/tongji_liucun/$', 'engine.manage.tongji_liucun',name="tongji_liucun"),
	url(r'^manage/tongji_qudao_normal/$', 'engine.manage.tongji_qudao_normal',name="tongji_qudao_normal"),
    url(r'^manage/tongji_qudao_liucun/$', 'engine.manage.tongji_qudao_liucun',name="tongji_qudao_liucun"),
	url(r'^manage/tongji_qudao_normal_xiangxi/(?P<id>\d+)/$', 'engine.manage.tongji_qudao_normal_xiangxi',name="tongji_qudao_normal_xiangxi"),
    url(r'^manage/tongji_qudao_liucun_xiangxi/(?P<id>\d+)/$', 'engine.manage.tongji_qudao_liucun_xiangxi',name="tongji_qudao_liucun_xiangxi"),
    url(r'^manage/tongji_normal_huoyue/$', 'engine.manage.tongji_normal_huoyue',name="tongji_normal_huoyue"),
    url(r'^manage/view_item/$', 'engine.manage.view_item',name="view_item"),
    url(r'^manage/view_huodong/$', 'engine.manage.view_huodong',name="view_huodong"),
    url(r'^manage/view_jewel/$', 'engine.manage.view_jewel',name="view_jewel"),
    url(r'^manage/player_level/$', 'engine.manage.player_level',name="player_level"),
    url(r'^manage/player_data/$', 'engine.manage.player_data',name="player_data"),
    url(r'^manage/query_passward/$', 'engine.manage.query_passward',name="query_passward"),
    url(r'^manage/view_resource/$', 'engine.manage.view_resource',name="view_resource"),
    url(r'^manage/get_gonggao/$', 'engine.manage.get_gonggao',name="get_gonggao"),
    url(r'^manage/modify_player/$', 'engine.manage.modify_player',name="modify_player"),
    url(r'^manage/server_open_time_add/$', 'engine.manage.server_open_time_add',name="server_open_time_add"),
    url(r'^manage/server_open_time_deploy/(?P<id>\d+)/$', 'engine.manage.server_open_time_deploy',name="server_open_time_deploy"),
    url(r'^manage/server_open_time_delete/(?P<id>\d+)/$', 'engine.manage.server_open_time_delete',name="server_open_time_delete"),
    url(r'^manage/server_open_time_modify_time/(?P<id>\d+)/$', 'engine.manage.server_open_time_modify_time',name="server_open_time_modify_time"),
    url(r'^manage/set_gonggao/$', 'engine.manage.set_gonggao',name="set_gonggao"),
]

if settings.DEBUG:
    urlpatterns += [
        url(r'^static/(?P<path>.*)$', views.serve),
    ]
