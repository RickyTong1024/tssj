import os
from models import ServerStart
import threading


def deploy(remote, path, sid, ip, site, id):
    try:
        cmd = "fab -f task.py do:remote=%s,path=%s,sid=%s,ip=%s,site=%s" % (remote, path, sid, ip, site)
        result = os.system(cmd)
        if result == 0:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 2
            openserver.save()
        else:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 0
            openserver.save()
    except Exception:
        return -1

    return 0


def make(remote, sid, site, osss, path, huodong, id, recharge):
    try:
        cmd = "fab -f task.py start:remote=%s,sid=%s,site=%s,osss=%s,path=%s,huodong=%s,recharge=%s" % (remote, sid, site, osss, path, huodong, recharge)
        result = os.system(cmd)
        if result == 0:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 4
            openserver.sync = True
            openserver.save()
        else:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 2
            openserver.save()
    except Exception as e:
        print(e)
        return -1

    return 0


def upload(remote, sid, site, osss, path, huodong, id):
    try:
        cmd = "fab -f task.py onlyupload:remote=%s,sid=%s,site=%s,osss=%s,path=%s,huodong=%s" % (remote, sid, site, osss, path, huodong)
        result = os.system(cmd)
        if result == 0:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 4
            openserver.sync = True
            openserver.save()
        else:
            openserver = ServerStart.objects.get(id=id)
            openserver.openstat = 2
            openserver.save()
    except Exception, e:
        print e
        return -1

    return 0


class DeployThreading(threading.Thread):
    def __init__(self, remote, path, sid, ip, site, id):
        threading.Thread.__init__(self)
        self.remote = remote
        self.path = path
        self.sid = sid
        self.ip = ip
        self.site = site
        self.id = id

    def run(self):
        deploy(self.remote, self.path, self.sid, self.ip, self.site, self.id)
