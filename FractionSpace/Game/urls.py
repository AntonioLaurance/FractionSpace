from django.urls import include, path
from rest_framework import routers
from . import views

router = routers.DefaultRouter()
router.register(r'usuario', views.UsuarioViewSet)
router.register(r'partida', views.PartidaViewSet)
router.register(r'nivel', views.NivelViewSet)
router.register(r'pregunta', views.PreguntaViewSet)
router.register(r'grafica', views.GraficaViewSet)

urlpatterns = [
    path('', views.index, name = 'index'),
    path('auth', views.auth, name = 'auth'),
    path('grafica/', views.grafica, name = 'grafica'),
    path('api', include(router.urls)),
    path('api-auth/', include('rest_framework.urls', namespace = 'rest_framework')),
]
