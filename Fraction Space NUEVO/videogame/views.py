import math
import sqlite3
import secrets
import requests
from django.shortcuts import render
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from django import forms
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.models import User
from django.contrib.auth import login, authenticate
from django.shortcuts import render, redirect
from django.db.models import Sum
#from .forms import SignUpForm
from rest_framework import viewsets
from random import randrange
from json import loads, dumps
from .serializers import *
from .models import Usuario, Partida, Nivel, Pregunta

class Fraccion:
    def __init__(self, num, den):
        self.num = int(num)
        self.den = int(den)

    def toJSON(self):
        return dumps(self, default=lambda o:o.__dict__, sort_keys=False, indent=4)

    def to_decimal(self) -> float:
        return self.num/self.den

    def simplify(self): 
        # Encontramos el máximo común divisor (MCD)
        gcd = math.gcd(self.num, self.den)

        # Dividimos el númerador y el denominador de la fracción por el MCD
        self.num = self.num / gcd
        self.den = self.den / gcd

        # Si el denominador es negativo multiplicamos toda la fracción por -1
        # para volverlo positivo
        if(self.den < 0):
            self.num = self.num * (-1)
            self.den = self.den * (-1)

        # Devolvemos tipo de dato entero en las fracciones si es un número entero
        if(self.num == int(self.num)):
            self.num = int(self.num)

        if(self.den == int(self.den)):
            self.den = int(self.den)

    def is_equal_to(self, fraccion) -> bool:
        if(type(fraccion) == Fraccion):
            return self.num * fraccion.den == self.den * fraccion.num
        else:
            return self.to_decimal() == fraccion

    def print(self):
        print(f"{self.num}/{self.den}")

class SignUpForm(UserCreationForm):
    email = forms.EmailField(required=True)

    class Meta:
        model = User
        fields = ('username', 'email', 'password1','password2')

# Create your views here.
# Principal page
def index (request):
    #return HttpResponse("Bienvenido")
    return render(request, 'index.html')

def procesamiento(request):
    nombre = request.POST['nombre']
    nombre = nombre.title()
    return render(request, 'index.html', {'name': nombre})

def about(request):
    return render(request, 'about.html')

def service(request):
    return render(request, 'service.html')

def contact(request):
    return render(request, 'contact.html')

def login(request):
    return render(request, 'sesion.html')

def register(request):
    return render(request, 'registro.html')

def administrator(request):
    return render(request, 'admin.html')

# Endpoint del panel del administrador donde se ven las estadísticas del videojuego
# mediante gráficas 
@csrf_exempt
def panel_admin(request):
    # return HttpResponse("<h1>Panel del administrador</h1>Si estas viendo esta página es porque un administrador inició sesión exitosamente.")
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
    url = "http://127.0.0.1:8080/grafica/datos"    
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

    # Definimos encabezado para gráfica de medidor radial
    data_gauge = []
    data_gauge.append(["Usuario", "Puntaje"])

    for elemento in resultados:
        if(elemento["nombre"] == "Antonio" or elemento["nombre"] == "Lalito" or elemento["nombre"] == "Emiliano"):
            print("Nombre de usuario: " + elemento["nombre"] + ", Puntaje acumulado: " + str(elemento["puntaje"]))
            data_gauge.append([elemento["nombre"], elemento["puntaje"]/68])

    # Serializamos los datos
    formatted_data = dumps(data_gauge)

    return render(request, "panel.html", {'losDato': formatted_data,
                                          'values': modified_data, 
                                          'titulo': titulo_formato, 
                                          'h_title': h_var_JSON, 
                                          'v_title': v_var_JSON, 
                                          'fill': fill_var_JSON, 
                                          'title_color': title_var_JSON,
                                          'axis_color': axis_var_JSON,
                                          'border_color': border_var_JSON}) 

# Para enviar correo electrónico de alguien que nos contacte en la página web
@csrf_exempt
def send_email(request):
    # Obtenemos datos del formulario enviado
    group = request.POST['group']
    num_list = int(request.POST['numList'])

    # 29 alumnos en el grupo A y 28 alumnos en el grupo B
    lists = {"A": range(1, 29), "B": range(1, 25)}

    # Validaciones
    if(group.upper() == "A" or group.upper() == "B"):
        if num_list in lists[group.upper()]:
            return render(request, "player.html", {"group": group, "numList": num_list})
        else: 
            return HttpResponse("Usuario no válido por favor intente de nuevo.")
    else:
        return HttpResponse("Usuario no válido por favor intente de nuevo.")

def signup(request):
    if request.method == 'POST':
        form = SignUpForm(request.POST)
        if form.is_valid():
            user = form.save()
            username = form.cleaned_data.get('username')
            raw_password = form.cleaned_data.get('password1')
            user = authenticate(username=username, password = raw_password)
            login(request, user)
            return redirect('index')
        else:
            form = SignUpForm()
        return render(request,'/registro.html', {'form': form})

@csrf_exempt
def suma(request):
    body_unicode = request.POST['exercise']
    body = loads(body_unicode)

    # Elements of our JSON
    num_1 = body['numerador1']
    den_1 = body['denominador1']
    num_2 = body['numerador2']
    den_2 = body['denominador2']
    num = body['num']
    den = body['den']

    # (a/b) + (c/d) = ((ad + bc)/bd) 
    num_res = (num_1 * den_2) + (num_2 * den_1)
    den_res = den_1 * den_2
    
    expected_result = Fraccion(num_res, den_res)
    expected_result.simplify()

    given_result = Fraccion(num, den)
    given_result.simplify()

    # Verificar si la operacion es correcta
    message = (expected_result.to_decimal() == given_result.to_decimal())
    
    # Calculamos deviación por valor
    des_val = abs(expected_result.to_decimal() - given_result.to_decimal())

    # Calculamos desviación porcentual
    try:    
        des_por = abs(des_val/expected_result.to_decimal()) * 100
    except ZeroDivisionError:
        # Si el resultado esperado es cero ponemos la variación porcentual como infinito
        des_por = math.inf

    response_data = {"num": expected_result.num, 
                     "den": expected_result.den, 
                     "correcto": message,
                     "devpor": des_por,
                     "devval": des_val}

    return HttpResponse(dumps(response_data), content_type = "text/json-comment-filtered")

@csrf_exempt
def resta(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)
    
    # Elements of our JSON
    num_1 = body['numerador1']
    den_1 = body['denominador1']
    num_2 = body['numerador2']
    den_2 = body['denominador2']
    num = body['num']
    den = body['den']

    # (a/b) - (c/d) = ((ad - bc)/bd) 
    num_res = (num_1 * den_2) - (num_2 * den_1)
    den_res = den_1 * den_2

    expected_result = Fraccion(num_res, den_res)
    expected_result.simplify()

    given_result = Fraccion(num, den)
    given_result.simplify()

    # Verificar si la operacion es correcta
    message = (expected_result.to_decimal() == given_result.to_decimal())
    
    # Calculamos deviación por valor
    des_val = abs(expected_result.to_decimal() - given_result.to_decimal())

    # Calculamos desviación porcentual
    try:    
        des_por = abs(des_val/expected_result.to_decimal()) * 100
    except ZeroDivisionError:
        # Si el resultado esperado es cero ponemos la variación porcentual como infinito
        des_por = math.inf

    response_data = {"num": expected_result.num, 
                     "den": expected_result.den, 
                     "correcto": message,
                     "devpor": des_por,
                     "devval": des_val}

    return HttpResponse(dumps(response_data), content_type = "text/json-comment-filtered")

@csrf_exempt
def multiplicacion(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)
    
    # Elements of our JSON
    num_1 = body['numerador1']
    den_1 = body['denominador1']
    num_2 = body['numerador2']
    den_2 = body['denominador2']
    num = body['num']
    den = body['den']

    # (a/b) * (c/d) = (ac/bd) 
    num_res = num_1 * num_2 
    den_res = den_1 * den_2

    expected_result = Fraccion(num_res, den_res)
    expected_result.simplify()

    given_result = Fraccion(num, den)
    given_result.simplify()

    # Verificar si la operacion es correcta
    message = (expected_result.to_decimal() == given_result.to_decimal())
    
    # Calculamos deviación por valor
    des_val = abs(expected_result.to_decimal() - given_result.to_decimal())

    # Calculamos desviación porcentual
    try:    
        des_por = abs(des_val/expected_result.to_decimal()) * 100
    except ZeroDivisionError:
        # Si el resultado esperado es cero ponemos la variación porcentual como infinito
        des_por = math.inf

    response_data = {"num": expected_result.num, 
                     "den": expected_result.den, 
                     "correcto": message,
                     "devpor": des_por,
                     "devval": des_val} 

    return HttpResponse(dumps(response_data), content_type = "text/json-comment-filtered")

@csrf_exempt
def division(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)
    
    # Elements of our JSON
    num_1 = body['numerador1']
    den_1 = body['denominador1']
    num_2 = body['numerador2']
    den_2 = body['denominador2']
    num = body['num']
    den = body['den']

    # (a/b) / (c/d) = (ad/bc) 
    num_res = num_1 * den_2 
    den_res = den_1 * num_2

    expected_result = Fraccion(num_res, den_res)
    expected_result.simplify()

    given_result = Fraccion(num, den)
    given_result.simplify()

    # Verificar si la operacion es correcta
    message = (expected_result.to_decimal() == given_result.to_decimal())
    
    # Calculamos deviación por valor
    des_val = abs(expected_result.to_decimal() - given_result.to_decimal())

    # Calculamos desviación porcentual
    try:    
        des_por = abs(des_val/expected_result.to_decimal()) * 100
    except ZeroDivisionError:
        # Si el resultado esperado es cero ponemos la variación porcentual como infinito
        des_por = math.inf

    response_data = {"num": expected_result.num, 
                     "den": expected_result.den, 
                     "correcto": message,
                     "devpor": des_por,
                     "devval": des_val} 

    return HttpResponse(dumps(response_data), content_type = "text/json-comment-filtered")

@csrf_exempt
def auth(request):
    body_unicode = request.POST['player']
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

@csrf_exempt
def verify(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)          # Deserialización

    # Elements of our JSON
    correcto = body['correcto']         # Si la respuesta es correcta
    devpor = body['devpor']             # Desviación porcentual
    devval = body['devval']             # Desviación por valor
    errpor = body['errpor']             # Error porcentual tolerable
    errval = body['errval']             # Error de valor tolerable
    interva = body['interva']           # ¿Intervalo Abierto?

    # ¿Es válido para el sistema el resultado?
    if (interva):
        valid = correcto or devpor < errpor or devval < errval
    else:
        valid = correcto or devpor <= errpor or devval <= errval

    # Diccionario no serializado 
    json = {"valido": valid}
    return HttpResponse(dumps(json), content_type = "text/json-comment-filtered")

# API usuario ID para Unity
# API para obtener un ID de usuario dado su grupo y número de lista 
# Consulta a la base de datos pero no almacena nada
# Admite exclusivamente el verbo POST de HTTP
# Si no existe un ID de usuario dado un grupo y número de lista regresa un texto vacío
# Recibe un formulario ('jugador') que tiene un JSON en sus contenidos que tiene 
# grupo y número de lista
@csrf_exempt
def usuario_unity(request):
    # Deserializamos elemento recibido
    body_unicode = request.POST['jugador']
    body = loads(body_unicode)

    # Elements of our JSON
    grupo = body['group']
    num_lista = body['numList']

    # Accedemos a la base de datos
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    # Obtenemos ID del usuario
    res = cur.execute(f"""SELECT id FROM videogame_usuario 
                          WHERE videogame_usuario.grupo="{grupo}" AND videogame_usuario.num_lista={num_lista}""")

    text = res.fetchall()
    return HttpResponse(f"{text[0][0]}")

# API de preguntas para Unity
# Se almacena en la base de datos lo enviado desde Unity
# Admite exclusivamente el verbo POST de HTTP
# Retorna el ID de la pregunta enviada para que se pueda utilizar en partida
@csrf_exempt
def preguntas_unity(request):
    # Deserializamos elemento recibido
    body_unicode = request.POST['pregunta']
    body = loads(body_unicode)

    # Elements of our JSON
    texto = body['texto']
    operacion = body['operacion']
    num1 = body['num1']
    den1 = body['den1']
    num2 = body['num2']
    den2 = body['den2']
    puntaje = body['puntaje']             # Puntaje random del 1 al 1000

    # Accedemos a la base de datos
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    # Insertamos registro
    cur.execute("INSERT INTO videogame_pregunta (texto, operacion, num1, den1, num2, den2, puntaje) VALUES (?,?,?,?,?,?,?)",
               (texto, operacion, num1, den1, num2, den2, puntaje))
    
    # Guardamos los cambios realizados
    con.commit()

    # Obtenemos ID de nuestra pregunta enviada 
    res = cur.execute(f"""SELECT COUNT(*) FROM videogame_pregunta""")
    text = res.fetchall()

    return HttpResponse(f"{text[0][0]}")

# API de partidas para Unity
# Se almacena en la base de datos lo enviado desde Unity
# Admite exclusivamente el verbo POST de HTTP
@csrf_exempt
def partidas_unity(request):
    # Deserializamos elemento recibido
    body_unicode = request.POST['partida']
    body = loads(body_unicode)

    # Elements of our JSON
    fecha_inicio = body['fecha_inicio']
    fecha_fin = body['fecha_fin']
    puntaje = body['puntaje']
    nivel = body['nivel'] 
    usuario = body['usuario']

    # Accedemos a la base de datos
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    # Insertamos registro
    cur.execute("INSERT INTO videogame_partida (fecha_inicio, fecha_fin, puntaje, nivel_id, usuario_id) VALUES (?,?,?,?,?)",
               (fecha_inicio, fecha_fin, puntaje, nivel, usuario))
    
    # Guardamos los cambios realizados
    con.commit()

    return HttpResponse("Se ha guardado la partida exitosamente.")

# API de gráfica
def datos_grafica(request):
    # Accedemos a la base de datos
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    # Guardamos consulta
    res = cur.execute("""SELECT videogame_usuario.nombre, SUM((strftime("%s", videogame_partida.fecha_fin) - strftime( "%s", videogame_partida.fecha_inicio)) /60) AS minutos_transcurridos, SUM(videogame_partida.puntaje) AS puntaje, videogame_usuario.grupo, MAX(videogame_partida.nivel_id) AS nivel
                         FROM videogame_usuario, videogame_partida
                         WHERE videogame_usuario.id = videogame_partida.usuario_id
                         GROUP BY videogame_usuario.nombre
                         ORDER BY videogame_partida.usuario_id;""")
    
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
    url = "http://127.0.0.1:8080/grafica/datos"    
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

class UsuarioViewSet(viewsets.ModelViewSet):
    queryset = Usuario.objects.all()        # SELECT * FROM videogame_usuario
    serializer_class = UsuarioSerializer

class PartidaViewSet(viewsets.ModelViewSet):
    queryset = Partida.objects.all()        # SELECT * FROM videogame_partida
    serializer_class = PartidaSerializer

class NivelViewSet(viewsets.ModelViewSet):
    queryset = Nivel.objects.all()          # SELECT * FROM videogame_nivel
    serializer_class = NivelSerializer

class PreguntaViewSet(viewsets.ModelViewSet):
    queryset = Pregunta.objects.all()       # SELECT * FROM videogame_pregunta
    serializer_class = PreguntaSerializer
 
# SELECT nombre, grupo FROM videogame_usuario ORDER BY usuario_id
# class GraficaViewSet(viewsets.ModelViewSet):
#     # queryset = itertools.chain(Usuario.objects.values('nombre', 'grupo').order_by('id'), Partida)
#     queryset = Usuario.objects.values('nombre') # .union(Partida.objects.values('fecha_fin')) # .union(Partida.objects.annotate(minutos_transcurridos = 100))
#     serializer_class = GraficaSerializer

# API para obtener los datos para elaborar la gráfica de barras
def graphdata(request):
    con = sqlite3.connect("db.sqlite3")
    cur = con.cursor()

    res = cur.execute("""SELECT videogame_nivel.id, SUM(videogame_partida.puntaje) as puntaje
                         FROM videogame_nivel
                         INNER JOIN videogame_partida ON videogame_nivel.id = videogame_partida.nivel_id
                         WHERE videogame_partida.usuario_id = 7
                         GROUP BY videogame_partida.nivel_id;""")
    resultado = res.fetchall()
    json = []

    for register in resultado:
        register_list = list(register)
        json.append({"Nivel": register_list[0],
                     "Puntaje": int(register_list[1])})
        
    return HttpResponse(dumps(json), content_type = "application/json")

def barras(request):
    # Definimos encabezado 
    data = []
    data.append(['Nivel','Puntaje'])

    # Definimos atributos de la gráfica
    h_var = "Nivel"
    v_var = "Puntaje"

    titulo = "Puntaje obtenido por nivel de un usuario"
    titulo_formato = dumps(titulo)

    # Usamos la API para obtener datos
    url = "http://127.0.0.1:8080/graphdata" 
    resultados = requests.get(url)
    resultados = resultados.json()

    # Llenamos con los datos la gráfica
    for elemento in resultados:
        print("ID (nivel): " + str(elemento["Nivel"]) + ", Puntaje: " + str(elemento["Puntaje"]))
        data.append([str(elemento["Nivel"]), elemento["Puntaje"]])

    h_var_JSON = dumps(h_var)
    v_var_JSON = dumps(v_var)

    modified_data = dumps(data)

    print(data)

    return render(request, "bar.html", {'values': modified_data, 
                                        'titulo': titulo_formato, 
                                        'h_title': h_var_JSON, 
                                        'v_title': v_var_JSON})

def pie(request):
    data = []
    data.append(['Jugador', 'Grado'])
    resultados = Partida.objects.all() #select nombres from Usuarios
    titulo = 'Fraction Space'
    titulo_formato = dumps(titulo)
    subtitulo= 'Puntuacion de alumnos'
    subtitulo_formato = dumps(subtitulo)
    if len(resultados)>0:
        for registro in resultados:
            nombre = registro.usuario.name
            puntaje = registro.puntaje
            data.append([nombre,puntaje])
        data_formato = dumps(data) #formatear los datos en string para JSON
        elJSON = {'losData':data_formato,'titulo':titulo_formato,'subtitulo':subtitulo_formato}
        return render(request,'pie.html',elJSON)
    else:
        return HttpResponse("<h1> No hay registros al mostrar</h1>")

# Gráfica de columnas de un alumno en específico de la fecha y puntaje
def columnas(request):
    data = []
    data.append(["Fecha", "Puntaje"])

    titulo = f"Progreso del alumno"
    titulo_formato = dumps(titulo)

    return render(request, "columnas.html", {"titulo": })
