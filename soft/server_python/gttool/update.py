#!/usr/bin/env python
# coding=utf-8
import os

old_str = '14'
new_str = '15'


def replace(fin, s):
    f = open(fin, 'r', encoding='UTF-8')
    a = f.read()
    f.close()

    a = a.replace(s + '?' + old_str, s + '?' + new_str)

    f = open(fin, 'w', encoding='UTF-8')
    f.write(a)
    f.close()


def main():
    replace('static/js/mail.js', 'name + ".txt')
    replace('static/js/libao.js', 'name + ".txt')

    files = os.listdir('templates')
    for f in files:
        if os.path.splitext(f)[1] == '.html':
            replace('templates/' + f, 'mail.js')
            replace('templates/' + f, 'libao.js')


if __name__ == '__main__':
    main()
