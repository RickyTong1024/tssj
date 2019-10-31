#! /usr/bin/env python
#coding=utf-8

import xml.dom.minidom
import sys
import os
from shutil import copy

def do_tps(s):
    ss = s + '.tps'
    dom = xml.dom.minidom.parse(ss)
    cc = dom.getElementsByTagName('filename')
    ccs = []
    for i in range(len(cc)):
        a = cc[i].childNodes[0].nodeValue
        index = a.rfind('.png')
        if index == -1:
            continue
        for j in range(len(ccs)):
            if ccs[j] == a:
                bb = True
                break
            if bb == False:
                ccs.append(a)
            else:
                continue
        index = a.rfind('/')
        if index == -1:
            continue
        aa = a[index + 1:]
        copy(a, aa)

do_tps("D:/work/tssj/trunk/soft/client/Assets/ui_src/overseas_gui")
if __name__ != '__main__':
    for i in range(1, len(sys.argv)):
        s = sys.argv[i]
        print s
        if len(s) < 4:
            continue
        if s[len(s) - 4:] != '.tps':
            continue
        s = s[:len(s) - 4]
        try:
            do_tps(s)
        except Exception as e:
            print e
        
    os.system("pause")
