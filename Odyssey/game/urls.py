from django.urls import path
from . import views                 # "."" is the actual directory 

# ------------------------------------------------------------------------
#                        PATRONES DE URL (endpoints)
# ------------------------------------------------------------------------

# path(<Cierto elemento de la url, nombre que teclea el usuario>, 
#      <Clase que procesa la url, en archivo views>, 
#      <Nombre que se utilizaría en esa clase>)

urlpatterns = [
    path('', views.index, name = 'index'),
    path('proceso', views.proceso, name = 'proceso'), 
    path('suma', views.suma, name = 'suma'),
    path('resta', views.resta, name = 'resta'),
    path('bienvenida', views.bienvenida, name = 'bienvenida'),
    path('multiplicacion', views.multiplicacion, name = 'multiplicacion'),
    path('division', views.division, name = 'division'),
    ]
