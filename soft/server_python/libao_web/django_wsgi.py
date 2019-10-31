#!/usr/bin/env python
# coding: utf-8

import os
import sys


os.environ.setdefault(
    "DJANGO_SETTINGS_MODULE",
    "libao_web.settings"
)

from django.core.handlers.wsgi import WSGIHandler
