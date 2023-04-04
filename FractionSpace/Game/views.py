from django.shortcuts import render
from django.http import HttpResponse
from rest_framework import viewsets
from .serializers import *
from .models import Usuario, Partida, Nivel, Pregunta

# Create your views here.
def index(request):
    return HttpResponse("Bienvenido a la página principal del videojuego FractionSpace.")

class UsuarioViewSet(viewsets.ModelViewSet):
    queryset = Usuario.objects.all()    # SELECT * FROM Usuario
    serializer_class = UsuarioSerializer