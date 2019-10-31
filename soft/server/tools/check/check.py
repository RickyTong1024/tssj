# -*- coding: utf-8 -*-
import psutil
import fabric


# 检测服务器是否正常运行
def check_work_process(work=[]):
    for i in work:
        work[i] = str(work[i])
    for i in work:
        worktmp = 'work' + i
        cnt = 0
        for pid in psutil.pids():
            p = psutil.Process(pid)
            if worktmp in p.cmdline():
                cnt += 1
                print(p.cmdline())
                if cnt == 10:
                    print(worktmp + ' is running')
                    break
        if cnt < 10 and cnt > 0:
            print(worktmp + ' is not running complete')
        elif cnt == 0:
            print(worktmp + ' is not running')
    return


# 检测账号服是否在运行
def check_account_process():
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'account.py' in p.cmdline():
            print('accoun_server is running')
            return
    print('accoun_server is not running')
    return


# 检测充值服是否在运行
def check_pay_process():
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'pay.py' in p.cmdline():
            print('pay_server is running')
            return
    print('pay_server is not running')
    return


# 检测礼包服是否在运行
def check_libao_process():
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'libao.py' in p.cmdline():
            print('libao_server is running')
            return
    print('libao_server is not running')
    return


# 检测储存服是否在运行
def check_storage_process():
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'storage.py' in p.cmdline():
            print('storage_server is running')
            return
    print('storage_server is not running')
    return


# 检测pvp是否正常运行
def check_pvp_process():
    cnt = 0
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'workpvp' in p.cmdline():
            cnt += 1
            print(p.cmdline())
            if cnt == 5:
                print('pvp is running')
                return
    if cnt < 5 and cnt > 0:
        print('pvp is not running complete')
    else:
        print('pvp is not running')
        return


# 检测team是否正常运行
def check_team_process():
    cnt = 0
    for pid in psutil.pids():
        p = psutil.Process(pid)
        if 'workteam' in p.cmdline():
            cnt += 1
            print(p.cmdline())
            if cnt == 5:
                print('team is running')
                return
    if cnt < 5 and cnt > 0:
        print('team is not running complete')
    else:
        print('team is not running')
        return


# 进程检测
def check_process():
    check_account_process()
    check_pay_process()
    # check_libao_process()
    check_storage_process()
    check_pvp_process()
    check_team_process()
    check_work_process(work=[0, 1])


def main():
    check_process()


if __name__ == '__main__':
    main()
