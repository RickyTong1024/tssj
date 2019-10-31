# -*- coding: utf-8 -*-

import struct


class dbc(object):
    def __init__(self):
        self.x = 0
        self.y = 0
        self.data = []
        self.records = []
        self.name = ""

    def load(self, path, name):
        self.name = name
        with open(path + name, "r", encoding="utf-8") as f:
            i = 0
            content = f.read()
            for byte in content:
                self.data.append(byte)
                if byte == '\n':
                    self.y = self.y + 1
                if (byte == '\n' or byte == '\t') and self.y > 1:
                    self.records.append(i + 1)
                i = i + 1

            i = 0
            f.seek(0, 0)
            for byte in f.read():
                if byte == '\t':
                    self.x = self.x + 1
                elif byte == '\n':
                    break

        self.x = self.x + 1
        self.y = self.y - 2

    def get(self, x, y):
        index = y * self.x + x
        if index >= len(self.records):
            return None

        begin = self.records[index]
        end = begin
        while True:
            if self.data[end] != '\t' and self.data[end] != '\n' and self.data[end] != '\r':
                end = end + 1
            else:
                break

        if begin == end:
            return '0'
        tmpdata = ('').join(self.data[begin:end])
        tmpdata = tmpdata.encode("utf8", "ignore")
        out, = struct.unpack("%ds" % (end - begin), tmpdata)
        out = out.decode('utf8', 'strict')
        out = out.replace("{nn}", "\n")
        return out

    def get_x(self):
        return self.x

    def get_y(self):
        return self.y


class Recharge(object):
    def __init__(self, path):
        self._path = path
        self._recharges = {}
        self._recharge_iosid = {}
        self._recharge_iosid2 = {}
        self._recharge_vip = {}

    def get_vip(self, rid):
        return self._recharge_vip.get(rid, 0)

    def get_iosid(self, rid):
        return self._recharge_iosid.get(rid, 0)

    def parse(self):
        self._parse_recharge()

    def _parse_recharge(self):
        dbc_file = dbc()
        dbc_file.load(self._path, "t_recharge.txt")

        for i in range(dbc_file.get_y()):
            #self._recharges[int(dbc_file.get(0, i))] = int(float(dbc_file.get(9, i)))
            self._recharge_iosid[int(dbc_file.get(0, i))] = int(dbc_file.get(10, i))
            self._recharge_vip[int(dbc_file.get(0, i))] = int(dbc_file.get(7, i))
