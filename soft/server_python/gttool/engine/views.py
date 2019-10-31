#!/usr/bin/python
# -*-coding:utf-8-*-

from django.template import RequestContext
from django.shortcuts import render_to_response, HttpResponseRedirect
from forms import LoginForm
from django.contrib.auth.decorators import login_required


@login_required(login_url="/login/")
def index(request):
    username = request.user.username
    request.session.set_expiry(600)
    return render_to_response('index.html', RequestContext(request, {
        "title": '主页',
        "username": username,
    }))


def login(request):
    if request.method == "POST":
        form = LoginForm(request=request, data=request.POST)
        if form.is_valid():
            return HttpResponseRedirect('/')
        else:
            return render_to_response('login.html', RequestContext(request, {
                'form': form,
            }))
    else:
        form = LoginForm()
        if request.user.is_authenticated():
            return HttpResponseRedirect('/')

    return render_to_response('login.html', RequestContext(request, {
        'form': form,
    }))
