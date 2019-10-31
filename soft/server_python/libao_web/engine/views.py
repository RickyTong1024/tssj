from django.shortcuts import render
from .forms import LibaoForm
from django.http import HttpResponse


def libao_view(request):
    if request.method == "POST":
        form = LibaoForm(request.POST)
        form.is_valid()
    else:
        form = LibaoForm()

    context = {'form': form, "result": form.result}
    return HttpResponse(render(request, 'libao.html', context))
