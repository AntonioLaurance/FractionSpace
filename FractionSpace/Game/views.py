import sqlite3
import secrets
import requests
from itertools import chain
from django.shortcuts import render
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from django.db.models import Count, Sum, Max
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

# API de gráfica
def datos_grafica(request):
    # Accedemos a la base de datos
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    # Guardamos consulta
    res = cur.execute("""SELECT Game_usuario.nombre, SUM((strftime("%s", Game_partida.fecha_fin) - strftime( "%s", Game_partida.fecha_inicio)) /60) AS minutos_transcurridos, SUM(Game_partida.puntaje) AS puntaje, Game_usuario.grupo, MAX(Game_partida.nivel_id) AS nivel
                         FROM Game_usuario, Game_partida
                         WHERE Game_usuario.id = Game_partida.usuario_id
                         GROUP BY Game_usuario.nombre
                         ORDER BY Game_partida.usuario_id;""")
    
    resultado = res.fetchall()
    json = []

    for register in resultado:
        register_list = list(register)
        json.append({"nombre": register_list[0], 
                     "minutos_transcurridos": int(register_list[1]),
                     "puntaje": int(register_list[2]),
                     "grupo": register_list[3],
                     "nivel": int(register_list[4])})
        

    return HttpResponse(dumps(json), content_type = "application/json")

# Endpoint que muestra gráfica de burbujas donde:
#      - Texto: <Nombre de usuario>
#      - X: <Minutos jugados>
#      - Y: <Puntaje>
#      - Color de burbuja: <Grupo>
#      - Tamaño de burbuja: <Nivel>
def grafica(request):
    # Definimos arreglo de datos 
    data = []
    data.append(['Nombre', 'Tiempo (minutos)', 'Puntaje', 'Grupo', 'Nivel'])

    # Definimos atributos de gráfica
    titulo = "Puntaje obtenido vs Tiempo jugado\nBubble size = Nivel, Bubble color = Grupo"
    titulo_formato = dumps(titulo)    
    h_var = "Tiempo (minutos)"
    v_var = "Puntaje"

    try:
        fill = request.GET['fill']
    except:
        fill = "black"

    try: 
        title_color = request.GET['title_color']
    except:
        title_color = "white"

    try:
        axis_color = request.GET['axis_color']
    except:
        axis_color = "orange"

    try:
        border_color = request.GET['border_color']
    except:
        border_color = "orange"

    # Consumimos la API para obtener datos de la gráfica
    url = "http://127.0.0.1:8000/grafica/datos"    
    resultados = requests.get(url)
    resultados = resultados.json()                 # lista de diccionarios
    
    # Agregamos los datos de la consulta al arreglo 
    for elemento in resultados:
        data.append([elemento["nombre"], elemento["minutos_transcurridos"], elemento["puntaje"], elemento["grupo"], elemento["nivel"]])

    # Serializamos atributos de gráfica
    h_var_JSON = dumps(h_var)
    v_var_JSON = dumps(v_var)
    title_var_JSON = dumps(title_color)
    fill_var_JSON = dumps(fill)
    axis_var_JSON = dumps(axis_color)
    border_var_JSON = dumps(border_color)

    # Serializamos los datos
    modified_data = dumps(data)

    return render(request, "bubble.html", {'values': modified_data, 
                                           'titulo': titulo_formato, 
                                           'h_title': h_var_JSON, 
                                           'v_title': v_var_JSON, 
                                           'fill': fill_var_JSON, 
                                           'title_color': title_var_JSON,
                                           'axis_color': axis_var_JSON,
                                           'border_color': border_var_JSON}) 
    ## return HttpResponse("Este servicio de visualización de gráfica está en actualización.")

def nuevoreto(request):
    pass

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
# class GraficaViewSet(viewsets.ModelViewSet):
#     # queryset = itertools.chain(Usuario.objects.values('nombre', 'grupo').order_by('id'), Partida)
#     queryset = Usuario.objects.values('nombre') # .union(Partida.objects.values('fecha_fin')) # .union(Partida.objects.annotate(minutos_transcurridos = 100))
#     serializer_class = GraficaSerializer
    
