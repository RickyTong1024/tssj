from django.urls import path

from . import views

urlpatterns = [
    path('', views.libao_view, name='libao')
]
