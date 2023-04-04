from django.shortcuts import render
from django.http import HttpResponse
from rest_framework import viewsets
from .serializers import *
from .models import Usuario, Partida, Nivel, Pregunta

# Create your views here.
def index(request):
    return HttpResponse("Bienvenido a la página principal del videojuego FractionSpace.")

class UsuarioViewSet(viewsets.ModelViewSet):
    queryset = Usuario.objects.all()        # SELECT * FROM Game_usuario
    serializer_class = UsuarioSerializer

class PartidaViewSet(viewsets.ModelViewSet):
    queryset = Partida.objects.all()        # SELECT * FROM Game_partida
    serializer_class = PartidaSerializer

class NivelViewSet(viewsets.ModelViewSet):
    queryset = Nivel.objects.all()          # SELECT * FROM Game_nivel
    serializer_class = NivelSerializer

class PreguntaViewSet(viewsets.ModelViewSet):
    queryset = Pregunta.objects.all()       # SELECT * FROM Game_pregunta
    serializer_class = PreguntaSerializer
 