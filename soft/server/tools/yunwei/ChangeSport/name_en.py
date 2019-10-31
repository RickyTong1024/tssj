#coding=utf-8
import struct
import random

class dbc(object):
    def __init__(self):
        self.x = 0
        self.y = 0
        self.data = []
        self.records = []
        self.name = ""
    
    def load(self, path, name):
        self.name = name
        with open(path + name , "r") as f:
            i = 0
            for byte in f.read():
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
            return ''

        out, = struct.unpack("%ds" % (end - begin), ('').join(self.data[begin:end]))
        out = out.replace("{nn}", "\n")
        return out

    def get_x(self):
        return self.x

    def get_y(self):
        return self.y
    
class Name(object):
    def __init__(self, path):
        self._path = path
        self._name_frist_list = []
        self._name_second_list = []

    def get_length(self):
        dbc_file = dbc()
        dbc_file.load(self._path, "t_name.txt")
        return dbc_file.get_y()

    def parse(self):
        self._parse_name()
    
    def _parse_name(self):
        dbc_file = dbc()
        dbc_file.load(self._path, "t_name.txt")

        for i in range(dbc_file.get_y()):
            name1 = dbc_file.get(0, i)
            name2 = dbc_file.get(1, i)
            if name2 != "":
                self._name_second_list.append(name2)
            if name1 != "":
                self._name_frist_list.append(name1)


    def get_random_name(self):
        index1 = random.randint(0, len(self._name_frist_list) - 1)
        index2 = random.randint(0, len(self._name_second_list) - 1)
        return self._name_frist_list[index1] + self._name_second_list[index2]



gname = Name("./config/")
gname.parse()

if __name__ == "__main__":
    pass
    '''
    gname = Name("./config/")
    gname.parse()

    for i in range(5000):
        print gname.get_random_name()
    '''
