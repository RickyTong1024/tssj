#! /usr/bin/env python
#coding=utf-8

import os
from shutil import copy

fdir = "../soft/client/Assets/Resources/config_txt/"
fdir1 = "../soft/server/config/"
fdir2 = "../soft/server_python/pay_server/config/"
fdir3 = "../soft/server_python/gttool/static/other/"

def conv_UTF8():
    files = os.listdir ('./')

    for f in files:
        if os.path.splitext (f)[1] == '.txt':
            print(f)
            ifs = open(f,'rb')
            try:
                content = ifs.read ().decode('gbk').encode('utf8')
                ifs.close()

                ofs = None
                ofs1 = None
                ofs2 = None
                ofs3 = None
                if len (content) > 0:
                    try:
                        ofs = open(fdir + f, 'wb')
                        ofs.write(content)
                        ofs1 = open(fdir1 + f, 'wb')
                        ofs1.write(content)
                        if f == 't_recharge.txt':
                            ofs2 = open(fdir2 + f, 'wb')
                            ofs2.write(content)
                        if f == 't_baowu.txt' or f == 't_chenghao.txt' or f == 't_equip.txt' or f == 't_item.txt' or f == 't_role.txt':
                            ofs3 = open(fdir3 + f, 'wb')
                            ofs3.write(content)
                    finally:
                        if ofs:
                            ofs.close ()
                        if ofs1:
                            ofs1.close ()
                        if ofs2:
                            ofs2.close ()
                        if ofs3:
                            ofs3.close ()

            finally:
                ifs.close ()
        elif os.path.splitext (f)[1] == '.xml':
            print(f)
            copy(f, fdir + f)

if __name__ == '__main__':
    conv_UTF8 ()
