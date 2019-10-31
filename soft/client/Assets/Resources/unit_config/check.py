#! /usr/bin/env python
#encoding=utf-8

import os
import string
from xml.etree import ElementTree

files = os.listdir ('./')
for f in files:
     if os.path.splitext (f)[1] == '.xml':
        root = ElementTree.parse(f)
        allactions = root.findall('actions')
        for actions in allactions:
            name = actions.get('name')
            if name == 'attack' or name == 'jn01':
                exp = 0
                for action in actions:
                    if action.get('type') == 'skill_export':
                        exp += string.atof(action.get('export'))
                if exp != 1:
                    print f, name, 'error export =', exp

os.system("pause")
