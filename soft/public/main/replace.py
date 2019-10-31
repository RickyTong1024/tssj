#!/usr/bin/env python  
#coding=utf-8

import sys

def rp(filename, r1, r2):
    ifs = open(filename, 'r')
    content = ifs.read()
    ifs.close()
    content = content.replace(r1, r2)
    
    ifs = open(filename, 'w')
    ifs.write(content)
    ifs.close()

if __name__ == '__main__':
    if len(sys.argv) == 4:
        rp(sys.argv[1], sys.argv[2], sys.argv[3])
