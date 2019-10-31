#!/usr/bin/python
# -*-coding:utf-8-*-

import re
from django.core.validators import RegexValidator
# from django.core.exceptions import ValidationError

username_re = re.compile(r'^([\w]{9}|[a-zA-Z]{1}[\w]+?)$')
username = RegexValidator(username_re, u'用户名由字母数字下划线组成,首字母为字母', 'invalid')

password_re = re.compile(r'^[\w]+?$')
password = RegexValidator(password_re, u'密码由字母数字下划线组成的字符串，最少为6位', 'invalid')
