from django.urls import include, path
from rest_framework import routers
from . import views

router = routers.DefaultRouter()
router.register(r'usuario', views.UsuarioViewSet)
router.register(r'partida', views.PartidaViewSet)
router.register(r'nivel', views.NivelViewSet)
router.register(r'pregunta', views.PreguntaViewSet)

urlpatterns = [
    path('',views.index, name = 'index'), 
    path('about', views.about, name = 'about'),
    path('service', views.service, name = 'service'),
    path('contact', views.contact, name = 'contact'),
    path('login', views.login, name = 'login'), 
    path('register', views.register, name = 'register'),
    path('administrator', views.administrator, name = 'administrator'),
    path('panel', views.panel_admin, name = 'panel'),
    path('email_send', views.send_email, name = 'email_send'),
    path('procesamiento', views.procesamiento, name = 'procesamiento'),
    path('suma', views.suma, name = 'suma'),
    path('resta', views.resta, name = 'resta'),
    path('multiplicacion', views.multiplicacion, name = 'multiplicacion'),
    path('division', views.division, name = 'division'),
    path('auth', views.auth, name = 'auth'),
    path('verify', views.verify, name = 'verify'),
    # path('api', include(router.urls)),
    # path('api-auth/', include('rest_framework.urls', namespace = 'rest_framework')),
    path('apiusuarioidunity', views.usuario_unity, name = 'apiusuarioidunity'),
    path('apipreguntasunity', views.preguntas_unity, name = 'apipreguntasunity'),
    path('apipartidasunity', views.partidas_unity, name = 'apipartidasunity'),
    path('grafica/', views.grafica, name = 'datosgrafica'),
    path('grafica/datos', views.datos_grafica),
    path('graphdata',views.graphdata),
    path('bar', views.barras, name = 'bar'),
]
