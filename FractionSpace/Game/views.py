import secrets
import sqlite3
import requests
from django.shortcuts import render
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from rest_framework import viewsets
from json import loads, dumps
from .serializers import *
from .models import Usuario, Partida, Nivel, Pregunta

# Create your views here.
def index(request):
    return HttpResponse("Bienvenido a la página principal del videojuego FractionSpace.")

# En este endpoint se validá la existencia de un usuario para el inicio de sesión
@csrf_exempt
def auth(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)

    # Elements of our JSON
    group = body['group']
    numList = body['numList']

    # 29 alumnos en el grupo A y 28 alumnos en el grupo B
    lists = {"A": range(1, 29), "B": range(1, 25)}

    # Validaciones
    if(group.upper() == "A" or group.upper() == "B"):
        if numList in lists[group.upper()]:
            valido = True
            token = secrets.token_hex(16)
        else: 
            valido = False
            token = None
    else:
        valido = False
        token = None

    json = {"valido": valido, "token": token}
    return HttpResponse(dumps(json), content_type = "text/json-comment-filtered")

# Cuidar usar JSON's en lugar de Queries
def grafica(request, fill = "black"):
    # Usamos servicios CRUD para obtener datos
    url_usuario = "http://127.0.0.1:8000/apiusuario/"
    url_partida = "http://127.0.0.1:8000/apipartida/"

    response_usuario = requests.get(url_usuario)
    response_partida = requests.get(url_partida)

    usuario_json = response_usuario.json()
    partida_json = response_partida.json()

    print(usuario_json)

    # Deserializamos para el manejo de los datos 
    # usuarios = loads(usuario_json)
    # partidas = loads(partida_json)

    # Accedemos a la base de datos
    ## con = sqlite3.connect("db.sqlite3")
    ## cur = con.cursor()
 
    ## # Guardamos consulta
    ## res = cur.execute("""SELECT Game_usuario.nombre, SUM((strftime("%s", Game_partida.fecha_fin) - strftime( "%s", Game_partida.fecha_inicio)) /60) AS minutos_transcurridos, SUM(Game_partida.puntaje) AS puntaje, Game_usuario.grupo, MAX(Game_partida.nivel_id) AS nivel
    ##                      FROM Game_usuario, Game_partida
    ##                      WHERE Game_usuario.id = Game_partida.usuario_id
    ##                      GROUP BY Game_usuario.nombre
    ##                      ORDER BY Game_partida.usuario_id;""")
    ## 
    ## # Definimos arreglo de datos 
    ## data = []
    ## data.append(['Nombre', 'Tiempo (minutos)', 'Puntaje', 'Grupo', 'Nivel'])

    ## resultados = res.fetchall()
    ## titulo = "Puntaje obtenido vs Tiempo jugado\nBubble size = Nivel, Bubble color = Grupo"
    ## titulo_formato = dumps(titulo)    
    ## h_var = "Tiempo (minutos)"
    ## v_var = "Puntaje"
    
    # Agregamos los datos de la consulta al arreglo 
    ## for elemento in resultados:
    ##     data.append(list(elemento))

    ## h_var_JSON = dumps(h_var)
    ## v_var_JSON = dumps(v_var)
    ## fill_var_JSON = dumps(fill)
    ## modified_data = dumps(data)

    ## return render(request, "bubble.html", {'values': modified_data, 'titulo': titulo_formato, 'h_title': h_var_JSON, 'v_title': v_var_JSON, 'fill': fill_var_JSON}) 
    return HttpResponse("Este servicio de visualización de gráfica está en actualización.")

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
 
# SELECT nombre, grupo FROM Game_usuario ORDER BY usuario_id
class GraficaViewSet(viewsets.ModelViewSet):
    queryset = Usuario.objects.values('nombre', 'grupo').order_by('id') 
    serializer_class = GraficaUsuarioSerializer
    
