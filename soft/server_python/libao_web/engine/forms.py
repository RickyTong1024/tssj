import struct
import http.client
from django import forms
from .models import Server
from .rpc_pb2 import tmsg_req_libao_exchange
import engine.opcodes


class LibaoForm(forms.Form):
    username = forms.CharField(
        required=True,
        label='账号',
        min_length=4,
        max_length=64,
        error_messages={'required': '必选项'}
    )

    server = forms.ModelChoiceField(
        required=True,
        label='服务器',
        queryset=Server.objects.all(),
        error_messages={'required': '必选项'}
    )

    code = forms.CharField(
        required=True,
        label='兑换码',
        min_length=4,
        max_length=64,
        error_messages={'required': '必选项'}
    )

    def __init__(self, *args, **kwargs):
        self.result = ""
        super(LibaoForm, self).__init__(*args, **kwargs)

    def clean(self):
        if not self.is_valid():
            raise forms.ValidationError('无效字段')
        self.cleaned_data = super(LibaoForm, self).clean()

        username = self.cleaned_data["username"]
        servername = self.cleaned_data["server"]
        code = self.cleaned_data["code"]

        try:
            server = Server.objects.get(name=servername)
        except Exception as e:
            print(e)
            self.result = "兑换失败，读取服务器列表错误"
            return self.cleaned_data

        if not server:
            self.result = "服务器不存在"
            return self.cleaned_data

        msg = tmsg_req_libao_exchange()
        msg.code = code
        msg.username = username
        msg.serverid = str(server.serverid)
        s = msg.SerializeToString()

        http_client = None
        success = True
        try:
            http_client = http.client.HTTPConnection(server.host + ":" + str(server.port))
            headers = {'Content-type': 'text/xml;charset=UTF-8'}
            http_client.request("POST", "/" + str(engine.opcodes.opcodes["TMSG_LIBAO_EXCHANGE"]), s, headers)
            response = http_client.getresponse()
            if response.status == 200:
                text = response.read()
                fmt = "i%ds" % (len(text) - 4)
                error_code, info = struct.unpack(fmt, text)
                if error_code != 0:
                    result = "兑换失败,code=%d, error=%s" % (error_code, info)
                    success = False
            else:
                result = "兑换失败，请求错误"
                success = False
        except Exception as e:
            print(e)
            result = "兑换失败，系统错误"
            success = False
        finally:
            if http_client:
                http_client.close()

        if success:
            result = "兑换成功，奖励将以邮件形式发送"

        self.result = result
        return self.cleaned_data
