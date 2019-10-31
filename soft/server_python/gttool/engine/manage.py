#!/usr/bin/python
#-*-coding:utf-8-*-

from django.template import RequestContext
from django.shortcuts import render_to_response, HttpResponse
from forms import *
from models import Server
from django.contrib.auth.decorators import login_required
from django.core.paginator import Paginator, PageNotAnInteger, EmptyPage
from django.contrib import messages
from django.db.models import Q
from django.http import HttpResponseRedirect, StreamingHttpResponse
from django.conf import settings
import helper
import datetime
import json
import tasks


import logging
logger = logging.getLogger(__name__)


@login_required(login_url="/login/")
def serverlook(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.serverlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag

    index = request.GET.get('index')
    if index == None:
        items = ServerlookItems(1, request.user)
    else:
        items = ServerlookItems(int(index), request.user)

    if has_tag:
        return render_to_response('serverlook_fake.html', RequestContext(request, {
            "title": '服务器查询',
            "username": username,
            'items': items,
        }))
    else:
        return render_to_response('serverlook.html', RequestContext(request, {
            "title": '服务器查询',
            "username": username,
            'items': items,
        }))


@login_required(login_url="/login/")
def userlook(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = UserlookForm(request.POST)
        form.is_valid()
    else:
        form = UserlookForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('userlook.html', RequestContext(request, {
        "title": '玩家查询',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def huoban(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    form = HuobanForm(request.GET['guid'], request.GET['servername'])
    form.is_valid()

    return render_to_response('huoban.html', RequestContext(request, {
        "title": '伙伴查看',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def zhuangbei(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    form = ZhuangbeiForm(request.GET['guid'], request.GET['servername'])
    form.is_valid()

    return render_to_response('zhuangbei.html', RequestContext(request, {
        "title": '装备查看',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def baowu(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    form = BaowuForm(request.GET['guid'], request.GET['servername'])
    form.is_valid()

    return render_to_response('baowu.html', RequestContext(request, {
        "title": '宝物查看',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def baowu_all(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    form = BaowuAllForm(request.GET['guid'], request.GET['servername'])
    form.is_valid()

    return render_to_response('baowu_all.html', RequestContext(request, {
        "title": '宝物查看',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def daoju_all(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.userlook'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    form = DaojuForm(request.GET['guid'], request.GET['servername'])
    form.is_valid()

    return render_to_response('daoju.html', RequestContext(request, {
        "title": '道具查看',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def mail_one(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.mail_one'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        reward_str = ""
        jewel_num = 0

        form = MailOneForm(request.POST)
        form.items = MailItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            if form.items.type1 == 1 and int(request.POST['reward1_2']) == 2:
                jewel_num = jewel_num + int(request.POST['reward1_3'])
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
            reward_str = reward_str + str(request.POST['reward1_1']) + " " + str(
                request.POST['reward1_2']) + " " + str(request.POST['reward1_3']) + " " + "0" + " "
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            if form.items.type2 == 1 and int(request.POST['reward2_2']) == 2:
                jewel_num = jewel_num + int(request.POST['reward2_3'])
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
            reward_str = reward_str + str(request.POST['reward2_1']) + " " + str(
                request.POST['reward2_2']) + " " + str(request.POST['reward2_3']) + " " + "0" + " "
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            if form.items.type2 == 1 and int(request.POST['reward3_2']) == 2:
                jewel_num = jewel_num + int(request.POST['reward3_3'])
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
            reward_str = reward_str + str(request.POST['reward3_1']) + " " + str(
                request.POST['reward3_2']) + " " + str(request.POST['reward3_3']) + " " + "0" + " "
        form.is_valid()

        try:
            server = Server.objects.get(name=form.cleaned_data['server_select'])

            accnames = request.POST['accname']
            accnames = accnames.split('\r\n')
            content_str = ""
            for i in range(len(accnames)):
                sstr = '["server":{0}, "accname":{1}]'.format(server.name, accnames[i])
                content_str = content_str + sstr + " "
            post_log = PostLog(puser=request.user.username, title=request.POST['title'], text=request.POST['text'],
                               content=content_str, reward=reward_str, jewel=jewel_num, suc=form.suc,
                               dt=datetime.datetime.now().replace(tzinfo=None))
            post_log.save()
        except:
            pass
    else:
        form = MailOneForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('mail_one.html', RequestContext(request, {
        "title": '单人邮件',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def mail_all(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.mail_all'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        reward_str = ""

        form = MailAllForm(request.POST)
        form.items = MailItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
            reward_str = reward_str + str(request.POST['reward1_1']) + " " + str(
                request.POST['reward1_2']) + " " + str(request.POST['reward1_3']) + " " + "0" + " "
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
            reward_str = reward_str + str(request.POST['reward2_1']) + " " + str(
                request.POST['reward2_2']) + " " + str(request.POST['reward2_3']) + " " + "0" + " "
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
            reward_str = reward_str + str(request.POST['reward3_1']) + " " + str(
                request.POST['reward3_2']) + " " + str(request.POST['reward3_3']) + " " + "0" + " "
        form.is_valid()

        try:
            content_str = ""
            for i in range(len(form.cleaned_data['server_select'])):
                server = Server.objects.get(name=form.cleaned_data['server_select'][i])
                sst = '["server":{0}, "level":{1}]'.format(server.name, request.POST['level'])
                content_str = content_str + sst + " "
            post_log = PostLog(puser=request.user.username,
                               title=request.POST['title'], text=request.POST['text'],
                               content=content_str, reward=reward_str, suc=form.suc,
                               dt=datetime.datetime.now().replace(tzinfo=None))
            post_log.save()
        except:
            pass
    else:
        form = MailAllForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)
    return render_to_response('mail_all.html', RequestContext(request, {
        "title": '单服邮件',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def mail_show(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.mail_show'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    lines = PostLog.objects.all().order_by("-id")
    paginator = Paginator(lines, 50)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('mail_show.html', RequestContext(request, {
        "title": '邮件显示',
        "line": show_lines,
    }))


@login_required(login_url="/login/")
def moni_show(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.mail_show'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    lines = MoniLog.objects.all().order_by("-id")
    paginator = Paginator(lines, 50)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('moni_show.html', RequestContext(request, {
        "title": '邮件显示',
        "line": show_lines,
    }))


@login_required(login_url="/login/")
def gonggao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.gonggao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        logger.info("gonggao:username=%s, text=%s", username, request.POST['text'])
        form = GonggaoForm(request.POST)
        form.is_valid()
        logger.info("gonggao:username=%s, result=%s", username, form.suc)
    else:
        form = GonggaoForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)
    return render_to_response('gonggao.html', RequestContext(request, {
        "title": '发送公告',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def dingdan(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.dingdan'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        form = DingdanForm(request.POST)
        form.is_valid()
    else:
        form = DingdanForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('dingdan.html', RequestContext(request, {
        "title": '订单查询',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def moni(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.moni'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        logger.info("moni:username=%s, server=%s, acc=%s", username,
                    request.POST['server_select'], request.POST['accname'])
        form = MoniForm(request.POST)
        form.is_valid()
        try:
            reward_id = 0
            rewards = ''
            moni_log = MoniLog(uname=username, server=request.POST['server_select'], acc=request.POST['accname'],
                               pid=request.POST['yueka'], suc=form.suc,
                               dt=datetime.datetime.now().replace(tzinfo=None))
            moni_log.save()
        except:
            pass
    else:
        form = MoniForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('moni.html', RequestContext(request, {
        "title": '模拟充值',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def fenghao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.fenghao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        logger.info("fenghao:username=%s, server=%s, acc=%s, fenghao_time=%s", username,
                    request.POST['server_select'], request.POST['accname'], request.POST['fenghao_time'])
        form = FenghaoForm(request.POST)
        form.is_valid()
        logger.info("fenghao:username=%s, result=%s", username, form.suc)
    else:
        form = FenghaoForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('fenghao.html', RequestContext(request, {
        "title": '踢人封号',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def looklibao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.looklibao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    index = request.GET.get('index')
    if index == None:
        items = LibaoItems(1)
    else:
        items = LibaoItems(int(index))

    return render_to_response('looklibao.html', RequestContext(request, {
        "title": '查看礼包',
        "username": username,
        'items': items,
    }))


@login_required(login_url="/login/")
def clibao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.clibao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        logger.info("clibao:username=%s, type=%s, name=%s, pc=%s, num=%s, reward=%s|%s|%s,%s|%s|%s,%s|%s|%s", username, request.POST['libao_type'],
                    request.POST['libao_name'], request.POST['libao_pc'], request.POST['libao_num'],
                    request.POST['reward1_1'], request.POST.get('reward1_2', '0'), request.POST['reward1_3'],
                    request.POST['reward2_1'], request.POST.get('reward2_2', '0'), request.POST['reward2_3'],
                    request.POST['reward3_1'], request.POST.get('reward3_2', '0'), request.POST['reward3_3'])
        form = ClibaoForm(request.POST)
        form.items = MailItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.is_valid()
        logger.info("clibao:username=%s, result=%s", username, form.suc)
    else:
        form = ClibaoForm()

    return render_to_response('clibao.html', RequestContext(request, {
        "title": '创建礼包',
        "username": username,
        'form': form,
    }))


@login_required(login_url="/login/")
def dlibao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.dlibao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        logger.info("dlibao:username=%s, type=%s", username, request.POST['libao_type'])
        form = DlibaoForm(request.POST)
        form.is_valid()
        logger.info("dlibao:username=%s, result=%s", username, form.suc)
    else:
        form = DlibaoForm()

    return render_to_response('dlibao.html', RequestContext(request, {
        "title": '删除礼包',
        "username": username,
        'form': form,
    }))


###############################################################################
@login_required(login_url="/login/")
def activity_group(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_group'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityGroupForm(request.POST)
        form.is_valid()
    else:
        form = ActivityGroupForm()

    lines = ActivityGroup.objects.all().order_by("-id")
    paginator = Paginator(lines, 20)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('activity_group.html', RequestContext(request, {
        "title": '活动中心',
        "line": show_lines,
        "form": form,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_modify(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_modify'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityForm(request.POST, {"group": id})
        form.is_valid()
    else:
        form = ActivityForm()

    group = ActivityGroup.objects.get(id=id)
    lines = group.activitys.all().order_by("start_show_day")
    paginator = Paginator(lines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_modify.html', RequestContext(request, {
        "title": '活动管理',
        "line": show_lines,
        "form": form,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_modify_modify(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_modify_modify'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityModifyForm(request.POST, {"activity": id})
        form.is_valid()
    else:
        form = ActivityModifyForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id

    return render_to_response('activity_modify_modify.html', RequestContext(request, {
        "title": '活动管理',
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_group_modify_name(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_group_modify_name'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityModifyGroupNameForm(request.POST, {"group": id})
        form.is_valid()
    else:
        form = ActivityModifyGroupNameForm()

    return render_to_response('activity_group_name.html', RequestContext(request, {
        "title": '活动管理',
        "form": form,
        "previous": id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_sync(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_sync'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityGroupSyncForm(request.POST, {"group": id})
        form.is_valid()
    else:
        form = ActivityGroupSyncForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1).filter(merge="0")

    group = ActivityGroup.objects.get(id=id)
    lines = group.historys.order_by("-id")
    paginator = Paginator(lines, 10)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('activity_sync.html', RequestContext(request, {
        "title": '活动管理',
        "line": show_lines,
        "form": form,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_group_delete(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_group_delete'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    try:
        group = ActivityGroup.objects.get(id=id)
        for timeline in group.timelines.all():
            timeline.activitygroup_set.remove(group)
        group.delete()
    except:
        messages.success(request, "删除失败")
        return HttpResponseRedirect("/manage/activity_group")

    messages.success(request, "删除成功")
    return HttpResponseRedirect("/manage/activity_group")


@login_required(login_url="/login/")
def activity_reward_czfl(request, id):
    '''充值返利'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_czfl'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityCzflRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityCzflRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_czfl.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_dbcz(request, id):
    '''单笔充值'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_dbcz'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityDbczRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityDbczRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_dbcz.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_dlsl(request, id):
    '''登录送礼'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_dlsl'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityDlslRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityDlslRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_dlsl.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_zkfs(request, id):
    '''折扣贩售'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_zkfs'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityZkfsRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityZkfsRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_zkfs.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_djdh(request, id):
    '''道具兑换'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_djdh'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityDjdhRewardForm(request.POST, {"activity": id})

        form.duihuans = ActivityRewardItem()
        form.duihuans.type1 = int(request.POST['duihuan1_1'])
        if form.duihuans.type1 > 0:
            form.duihuans.value11 = int(request.POST['duihuan1_2'])
            form.duihuans.value12 = int(request.POST['duihuan1_3'])
        form.duihuans.type2 = int(request.POST['duihuan2_1'])
        if form.duihuans.type2 > 0:
            form.duihuans.value21 = int(request.POST['duihuan2_2'])
            form.duihuans.value22 = int(request.POST['duihuan2_3'])

        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.is_valid()
    else:
        form = ActivityDjdhRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_djdh.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_hyhd(request, id):
    '''活跃活动'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_hyhd'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityHyhdRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityHyhdRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_hyhd.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_rqdl(request, id):
    '''日期登录'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_rqdl'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityRqdlRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityRqdlRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_rqdl.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_reward_zchd(request, id):
    '''直冲活动'''

    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_zchd'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityZchdRewardForm(request.POST, {"activity": id})
        form.items = ActivityRewardItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.items.type2 = int(request.POST['reward2_1'])
        if form.items.type2 > 0:
            form.items.value21 = int(request.POST['reward2_2'])
            form.items.value22 = int(request.POST['reward2_3'])
        form.items.type3 = int(request.POST['reward3_1'])
        if form.items.type3 > 0:
            form.items.value31 = int(request.POST['reward3_2'])
            form.items.value32 = int(request.POST['reward3_3'])
        form.items.type4 = int(request.POST['reward4_1'])
        if form.items.type4 > 0:
            form.items.value41 = int(request.POST['reward4_2'])
            form.items.value42 = int(request.POST['reward4_3'])
        form.is_valid()
    else:
        form = ActivityZchdRewardForm()

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    lines = activity.entrys.order_by("order")
    newlines = []
    for line in lines:
        l = ActivityRewardDesc(line)
        newlines.append(l)
    paginator = Paginator(newlines, 20)
    show_lines = paginator.page(1)

    return render_to_response('activity_reward_zchd.html', RequestContext(request, {
        "title": '活动奖励管理',
        "huodong_name": activity.name,
        "line": show_lines,
        "form": form,
        "previous": group_id,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_delete(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_delete'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    try:
        activity = Activity.objects.get(id=id)
        group_id = activity.group_id
        activity.delete()
    except:
        messages.success(request, "删除失败")
        return HttpResponseRedirect("/manage/activity_modify" + "/%d/" % group_id)

    messages.success(request, "删除成功")
    return HttpResponseRedirect("/manage/activity_modify" + "/%d/" % group_id)


@login_required(login_url="/login/")
def activity_copy(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_copy'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    activity = Activity.objects.get(id=id)
    group_id = activity.group_id
    old_group = ActivityGroup.objects.get(id=group_id)

    if request.method == "POST":
        form = ActivityCopyForm(request.POST, {"activity": id})
        form.is_valid()
    else:
        form = ActivityCopyForm()
        form.fields['huodong_select'].queryset = ActivityGroup.objects.filter(isjieri=old_group.isjieri)

    if request.method == "POST":
        if form.suc:
            messages.success(request, "拷贝成功")
            return HttpResponseRedirect("/manage/activity_modify" + "/%d/" % group_id)
        else:
            return render_to_response('activity_copy.html', RequestContext(request, {
                "title": '活动拷贝',
                "huodong_name": activity.name,
                "form": form,
                "previous": group_id,
                'messages': messages,
            }))
    else:
        return render_to_response('activity_copy.html', RequestContext(request, {
            "title": '活动拷贝',
            "huodong_name": activity.name,
            "form": form,
            "previous": group_id,
            'messages': messages,
        }))


@login_required(login_url="/login/")
def activity_reward_delete(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_reward_delete'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    cond_names = {1: '_czfl',
                  2: "_hyhd",
                  3: "_dbcz",
                  4: "_dlsl",
                  5: "_zkfs",
                  6: "_djdh",
                  7: "_rqdl", }
    try:
        reward = Entry.objects.get(id=id)
        activity_id = reward.activity_id
        activity = Activity.objects.get(id=activity_id)
        cond = activity.type
        reward.delete()
    except:
        messages.success(request, "删除失败")
        return HttpResponseRedirect("/manage/activity_reward" + cond_names[cond] + "/%d/" % activity_id)

    messages.success(request, "删除成功")
    return HttpResponseRedirect("/manage/activity_reward" + cond_names[cond] + "/%d/" % activity_id)


@login_required(login_url="/login/")
def activity_time_line(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_time_line'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityTimeLineForm(request.POST)
        form.is_valid()
    else:
        form = ActivityTimeLineForm()

    newlines = []

    lines = ActivityTime.objects.filter(isnew=True).order_by("gp", "startime")
    for line in lines:
        l = ActivityTimeLine(line.id, line.startime, line.endtime, line.endday, line.isnew, line.gp)
        groups = line.activitygroup_set.all()
        for g in groups:
            l.add_name(g.name, g.id)
        l.resize()
        newlines.append(l)

    lines = ActivityTime.objects.filter(sync=False).order_by("gp", "startime")
    for line in lines:
        l = ActivityTimeLine(line.id, line.startime, line.endtime, line.endday, line.isnew, line.gp)
        groups = line.activitygroup_set.all()
        for g in groups:
            l.add_name(g.name, g.id)
        l.resize()
        newlines.append(l)

    paginator = Paginator(newlines, 20)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('activity_time.html', RequestContext(request, {
        "title": '活动同步时间线',
        "line": show_lines,
        "form": form,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_time_line_add(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_time_line_add'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ActivityTimeLineAddGroupForm(request.POST, {"timeline": id})
        form.is_valid()
        messages.success(request, "添加成功")
        return HttpResponseRedirect("/manage/activity_time")
    else:
        form = ActivityTimeLineAddGroupForm()
        timeline = ActivityTime.objects.get(id=id)
        all_ids = []
        for g in timeline.activitygroup_set.all():
            all_ids.append(g.id)
        form.fields['huodong_select'].queryset = ActivityGroup.objects.exclude(id__in=all_ids)

    return render_to_response('activity_time_add.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def activity_time_line_delete(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.activity_time_line_delete'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    try:
        timeline = ActivityTime.objects.get(id=id)
        for g in timeline.activitygroup_set.all():
            g.timelines.remove(timeline)
        timeline.delete()
    except:
        messages.success(request, "删除失败")
        return HttpResponseRedirect("/manage/activity_time")

    messages.success(request, "删除成功")
    return HttpResponseRedirect("/manage/activity_time")


############################统计######################################
@login_required(login_url="/login/")
def tongji_normal(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_normal'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag

    lines = TongjiNormal.objects.order_by("-id")
    paginator = Paginator(lines, 20)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_normal_fake.html', RequestContext(request, {
            "title": '一般统计数据',
            "line": show_lines,
        }))

    else:
        return render_to_response('tongji_normal.html', RequestContext(request, {
            "title": '一般统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_liucun(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_liucun'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag

    lines = TongjiLiucun.objects.filter(dt__gte=datetime.date(2016, 7, 23)).order_by("-id")
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_liucun_fake.html', RequestContext(request, {
            "title": '留存统计数据',
            "line": show_lines,
        }))
    else:
        return render_to_response('tongji_liucun.html', RequestContext(request, {
            "title": '留存统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_qudao_normal(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_qudao_normal'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag
        has_qudao = user_profile.qudaon.split(";")
        allqudao = []
        for q in has_qudao:
            try:
                qudaoo = TongjiQudao.objects.get(qudao_name=q)
                allqudao.append(qudaoo)
            except:
                pass
    else:
        allqudao = TongjiQudao.objects.all()

    last_day = datetime.date.today() - datetime.timedelta(days=1)
    lines = []
    for qudao in allqudao:
        try:
            line = qudao.servers.get(dt=last_day)
            if line:
                lines.append([qudao.qudao_name, line, qudao.id])
        except:
            fake_server = TongjiQudaoServer(new_user_num=0,
                                            huo_user_num=0,
                                            total_rmb=0,
                                            total_user_num=0,
                                            total_num=0,
                                            dpu=0,
                                            aku=0,
                                            dt=last_day,
                                            group=qudao)
            lines.append([qudao.qudao_name, fake_server, qudao.id])
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_qudao_normal_fake.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))
    else:
        return render_to_response('tongji_qudao_normal.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_qudao_normal_xiangxi(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_qudao_normal_xiangxi'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag

    qudao = TongjiQudao.objects.get(id=id)
    lines = qudao.servers.all()
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_qudao_normal_xiangxi_fake.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))

    else:
        return render_to_response('tongji_qudao_normal_xiangxi.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_qudao_liucun(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_qudao_liucun'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag
        has_qudao = user_profile.qudaon.split(";")
        allqudao = []
        for q in has_qudao:
            try:
                qudaoo = TongjiQudao.objects.get(qudao_name=q)
                allqudao.append(qudaoo)
            except:
                pass
    else:
        allqudao = TongjiQudao.objects.all()

    last_day = datetime.date.today() - datetime.timedelta(days=2)
    lines = []
    for qudao in allqudao:
        try:
            line = qudao.liucuns.get(dt=last_day)
            if line:
                lines.append([qudao.qudao_name, line, qudao.id])
        except:
            fake_server = TongjiQudaoLicun(dt=last_day,
                                           total_num=0,
                                           liucun_2ltv=0,
                                           liucun_2rate=0.0,
                                           liucun_3ltv=0,
                                           liucun_3rate=0.0,
                                           liucun_4ltv=0,
                                           liucun_4rate=0.0,
                                           liucun_5ltv=0,
                                           liucun_5rate=0.0,
                                           liucun_6ltv=0,
                                           liucun_6rate=0.0,
                                           liucun_7ltv=0,
                                           liucun_7rate=0.0,
                                           liucun_15ltv=0,
                                           liucun_15rate=0.0,
                                           liucun_30ltv=0,
                                           liucun_30rate=0.0,
                                           group=qudao)
            lines.append([qudao.qudao_name, fake_server, qudao.id])
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_qudao_liucun_fake.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))

    else:
        return render_to_response('tongji_qudao_liucun.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_qudao_liucun_xiangxi(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_qudao_liucun_xiangxi'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    has_tag = False
    if hasattr(request.user, 'profile'):
        user_profile = request.user.profile
        has_tag = user_profile.tag

    qudao = TongjiQudao.objects.get(id=id)
    lines = qudao.liucuns.all()
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    if has_tag:
        return render_to_response('tongji_qudao_liucun_xiangxi_fake.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))

    else:
        return render_to_response('tongji_qudao_liucun_xiangxi.html', RequestContext(request, {
            "title": '渠道统计数据',
            "line": show_lines,
        }))


@login_required(login_url="/login/")
def tongji_normal_huoyue(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.tongji_normal_huoyue'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    lines = TongjiNormalHuoyue.objects.filter(
        dt=datetime.date.today() - datetime.timedelta(days=1)).order_by("serverid")
    servers = Server.objects.all()
    for line in lines:
        for s in servers:
            if int(s.serverid) == line.serverid:
                line.name = s.name
                break

    paginator = Paginator(lines, 50)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('tongji_normal_huoyue.html', RequestContext(request, {
        "title": '活跃数据',
        "line": show_lines,
    }))


############################统计######################################
@login_required(login_url="/login/")
def view_resource(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.view_resource'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ViewResourceLogForm(request.POST)
        form.is_valid()
    else:
        form = ViewResourceLogForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    paginator = Paginator(form.desc, 100)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('view_resource.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def view_item(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.view_item'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        item_id = int(request.POST['reward1_1'])
        if item_id > 0:
            item_id = int(request.POST['reward1_2'])
        form = ViewItemLogForm(request.POST, {"item_id": item_id})
        form.is_valid()
    else:
        form = ViewItemLogForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    paginator = Paginator(form.desc, 3000)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('view_item.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def view_jewel(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.view_jewel'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        form = ViewJewelLogForm(request.POST)
        form.is_valid()
    else:
        form = ViewJewelLogForm()

    paginator = Paginator(form.desc, 100)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('view_jewel.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def player_level(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.player_level'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        form = PlayerLevelForm(request.POST)
        form.is_valid()
    else:
        form = PlayerLevelForm()

    paginator = Paginator(form.desc, 100)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)
    return render_to_response('player_level.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def player_data(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.player_data'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))
    if request.method == "POST":
        form = PlayerdataForm(request.POST)
        form.is_valid()
    else:
        form = PlayerdataForm()

    paginator = Paginator(form.desc, 100)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)
    return render_to_response('player_data.html', RequestContext(request, {
        "title": '活动添加',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def query_passward(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.query_passward'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = QueryPasswardForm(request.POST)
        form.is_valid()
    else:
        form = QueryPasswardForm()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('query_passwd.html', RequestContext(request, {
        "title": '查询密码',
        "form": form,
        "username": form.uname,
        "passwd": form.pwd,
        "suc": form.suc,
        "mail": form.goolge,
        "old_username": form.old_username,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def change_gonggao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.change_gonggao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = QueryPasswardForm(request.POST)
        form.is_valid()
    else:
        form = QueryPasswardForm()

    return render_to_response('query_passwd.html', RequestContext(request, {
        "title": '查询密码',
        "form": form,
        "username": form.uname,
        "passwd": form.pwd,
        "suc": form.suc,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def get_gonggao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.get_gonggao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = QueueGonggaolist(request.POST)
        form.is_valid()

        def download_file(filename):
            with open(filename) as f:
                while True:
                    c = f.read(512)
                    if c:
                        yield c
                    else:
                        break
        if form.suc:
            resp = StreamingHttpResponse(download_file(settings.STATIC_ROOT + "/gonggao.txt"))
            resp['Content-Type'] = 'application/octet-stream'
            resp['Content-Disposition'] = 'attachment;filename="{0}"'.format("gonggao.txt")
            return resp
    else:
        form = QueueGonggaolist()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['pingtai'].queryset = GonggaoPlatform.objects.exclude(qudao__gt=user_profile.qudao2).exclude(
                qudao__lt=user_profile.qudao1)

    return render_to_response('query_gonggao.html', RequestContext(request, {
        "title": '获取已有公告',
        "form": form,
        "suc": form.suc,
        "content": form.con,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def set_gonggao(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.set_gonggao'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ModifyGonggaolist(request.POST, request.FILES)
        form.is_valid()
    else:
        form = ModifyGonggaolist()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['pingtai'].queryset = GonggaoPlatform.objects.exclude(qudao__gt=user_profile.qudao2).exclude(
                qudao__lt=user_profile.qudao1)

    return render_to_response('set_gonggao.html', RequestContext(request, {
        "title": '上传已有公告',
        "form": form,
        "suc": form.suc,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def modify_player(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.modify_player'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ModifyPlayerInfo(request.POST)
        form.items = MailItem()
        form.items.type1 = int(request.POST['reward1_1'])
        if form.items.type1 > 0:
            form.items.value11 = int(request.POST['reward1_2'])
            form.items.value12 = int(request.POST['reward1_3'])
        form.is_valid()
    else:
        form = ModifyPlayerInfo()
        if hasattr(request.user, 'profile'):
            user_profile = request.user.profile
            form.fields['server_select'].queryset = Server.objects.exclude(serverid__gt=user_profile.qudao2).exclude(
                serverid__lt=user_profile.qudao1)

    return render_to_response('modify_player.html', RequestContext(request, {
        "title": '上传已有公告',
        "form": form,
        "suc": form.suc,
        'messages': messages,
    }))


#############################################################################
@login_required(login_url="/login/")
def server_open_time_add(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.server_open_time_add'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ServerTimeLineForm(request.POST)
        form.is_valid()
    else:
        form = ServerTimeLineForm()

    lines = ServerStart.objects.filter(openflag=False).order_by("-id")
    paginator = Paginator(lines, 30)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    all_count = 0
    all_avserver = OpenServer.objects.all()
    for al in all_avserver:
        if al.site0 == 0:
            all_count += 1
        if al.site1 == 0:
            all_count += 1
        if al.site2 == 0:
            all_count += 1
        if al.site3 == 0:
            all_count += 1
        if al.site4 == 0:
            all_count += 1
        if al.site5 == 0:
            all_count += 1
        if al.site6 == 0:
            all_count += 1
        if al.site7 == 0:
            all_count += 1
        if al.site8 == 0:
            all_count += 1
        if al.site9 == 0:
            all_count += 1

    return render_to_response('server_open_time_add.html', RequestContext(request, {
        "title": '添加服务',
        "form": form,
        "suc": form.suc,
        "line": show_lines,
        "count": all_count,
        'messages': messages,
    }))


@login_required(login_url="/login/")
def server_open_time_deploy(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.server_open_time_deploy'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    try:
        openserver = ServerStart.objects.get(id=id)
        if openserver.openstat == 0:
            openserver.openstat = 1
        elif openserver.openstat == 2:
            openserver.openstat = 3
        else:
            messages.success(request, "操作失败,请检查状态")
            return HttpResponseRedirect("/manage/server_open_time_add")

        openserver.save()
        pt = ServerList.objects.get(id=openserver.ptid)

        sname = "serverlist.xml"
        if openserver.hebing:
            sname = "config_yymoon.xml"

        if openserver.openstat == 1:
            #tasks.deploy(openserver.host, pt.path + "\\" + pt.version, openserver.serverid, openserver.hostip, openserver.site, id)
            t = tasks.DeployThreading(openserver.host, pt.path + "\\" + pt.version,
                                      openserver.serverid, openserver.hostip, openserver.site, id)
            t.start()
        elif openserver.openstat == 3:
            if openserver.zhuserver:
                tasks.make(openserver.host, openserver.serverid, openserver.site, pt.osspath,
                           settings.STATIC_ROOT + "/huodong/" + sname, pt.huodong_path, id, pt.recharge_path)
            else:
                tasks.upload(openserver.host, openserver.serverid, openserver.site, pt.osspath,
                             settings.STATIC_ROOT + "/huodong/" + sname, pt.huodong_path, id)
    except:
        messages.success(request, "操作失败")
        return HttpResponseRedirect("/manage/server_open_time_add")

    messages.success(request, "操作成功")
    return HttpResponseRedirect("/manage/server_open_time_add")


@login_required(login_url="/login/")
def server_open_time_delete(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.server_open_time_delete'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    try:
        activity = ServerStart.objects.get(id=id)
        activity.delete()
    except:
        messages.success(request, "删除失败")
        return HttpResponseRedirect("/manage/server_open_time_add")

    messages.success(request, "删除成功")
    return HttpResponseRedirect("/manage/server_open_time_add")


@login_required(login_url="/login/")
def server_open_time_modify_time(request, id):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.server_open_time_modify_time'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ServerTimeLineModifyNameForm(request.POST, {"id": id})
        form.is_valid()
    else:
        form = ServerTimeLineModifyNameForm()

    return render_to_response('server_open_time_name.html', RequestContext(request, {
        "title": '修改名字',
        "form": form,
    }))


@login_required(login_url="/login/")
def view_huodong(request):
    username = request.user.username
    request.session.set_expiry(600)
    if not request.user.has_perm('engine.view_huodong'):
        return render_to_response('noperm.html', RequestContext(request, {
            "title": '没有访问权限',
            "username": username,
        }))

    if request.method == "POST":
        form = ViewHuodongLogForm(request.POST)
        form.is_valid()
    else:
        form = ViewHuodongLogForm()

    paginator = Paginator(form.desc, 500)
    page = request.GET.get('page')
    try:
        show_lines = paginator.page(page)
    except PageNotAnInteger:
        show_lines = paginator.page(1)
    except EmptyPage:
        show_lines = paginator.page(paginator.num_pages)

    return render_to_response('view_huodong.html', RequestContext(request, {
        "title": '活动显示',
        "form": form,
        "line": show_lines,
        'messages': messages,
    }))
