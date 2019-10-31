# -*- coding: utf8 -*-

from queue import Queue
from threading import Thread

class Worker(Thread):
    def __init__(self, tasks):
        Thread.__init__(self)
        self.tasks = tasks
        self.daemon = True
        self.start()

    def run(self):
        while True:
            try:
                func, args, kargs = self.tasks.get()
                func(*args, **kargs)
            except Exception as e:
                print(e)
            finally:
                self.tasks.task_done()


class ThreadPool(object):
    def __init__(self):
        pass

    @classmethod
    def instance(cls):
        if not hasattr(cls, "_instance"):
            cls._instance = cls()
        return cls._instance

    def add_task(self, func, *args, **kwargs):
        self.tasks.put((func, args, kwargs))

    def start(self, numthreads):
        self.tasks = Queue(numthreads)
        for _ in range(numthreads):
            Worker(self.tasks)

    def wait(self):
        self.tasks.join()
