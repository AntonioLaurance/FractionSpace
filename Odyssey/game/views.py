from django.shortcuts import render
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from json import loads, dumps

class Fraccion:
    def __init__(self, num, den):
        self.num = num
        self.den = den

    def toJSON(self) -> str:
        return dumps(self, default = lambda o:o.__dict__, 
                     sort_keys = False, indent = 4)

    def to_decimal(self) -> float:
        return self.num/self.den

    def simplify(self): 
        i = 2
        while(i <= self.den):
            if(self.num % i == 0 and self.den % i == 0):
                self.num = self.num/i
                self.den = self.den/i
                i = 2
            else: 
                i = i + 1

    def is_equal_to(self, fraccion) -> bool:
        if(type(fraccion) == Fraccion):
            return self.num * fraccion.den == self.den * fraccion.num
        else:
            return self.to_decimal() == fraccion

    def print(self):
        print(f"{self.num}/{self.den}")


# Create your views here.
def index(request):
    # return HttpResponse('<h1> Bienvenidos a la sección del jueves! </h1>')
    return render(request, '../templates/index.html')

# Function invoked when the user click 'submit' (endpoint)
def proceso(request):
    nombre = request.POST['nombre']
    nombre = nombre.upper()
    return HttpResponse(f'Hola {nombre}')   

# Function invoked when the user wants sum two fractions sending a JSON 
# throught the endpoint /suma
@csrf_exempt
def suma(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)

    p1 = body['numerador1']
    p2 = body['numerador2']
    q1 = body['denominador1']
    q2 = body['denominador2']

    resultado = Fraccion(p1 * q2 + p2 * q1, q1 * q2)
    resultado.simplify()

    json_resultado = resultado.toJSON()
    return HttpResponse(json_resultado,
                        content_type = "text/json-comment-filtered")

# Function invoked when the user send a JSON throught the endpoint /resta
@csrf_exempt
def resta(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)

    p1 = body['numerador1']
    p2 = body['numerador2']
    q1 = body['denominador1']
    q2 = body['denominador2']

    resultado = Fraccion(p1 * q2 - p2 * q1, q1 * q2)
    resultado.simplify()

    json_resultado = resultado.toJSON()
    return HttpResponse(json_resultado,
                        content_type = "text/json-comment-filtered")


def bienvenida(request):
    letrero = "Bienvenida"
    return HttpResponse(letrero)

# Function invoked when the user pass two parameters that multiplies 2 numbers 
# Multiplies thorougnt PUSH of a JSON on the endpoint /multiplicacion
@csrf_exempt
def multiplicacion(request):
    body_unicode = request.body.decode('utf-8')
    body = loads(body_unicode)

    p1 = body['numerador1']
    p2 = body['numerador2']
    q1 = body['denominador1']
    q2 = body['denominador2']

    resultado = Fraccion(p1 * p2, q1 * q2)
    resultado.simplify()

    json_resultado = resultado.toJSON()
    return HttpResponse(json_resultado,
                        content_type = "text/json-comment-filtered")
    # return HttpResponse(f"La multiplicación de {p} x {q} = {r}")
    # return HttpResponse("Multiplicación + str(p) + " x " + str(q) + " = " + str(r)")

# La función de división va a ser exenta del token de seguridad (csrf)
@csrf_exempt                  # Anotación (genera código por nosotros)
def division(request):
    body_unicode = request.body.decode('utf-8')   # the codec is utf-8 
    body = loads(body_unicode)                    # JSON to list

    p1 = body['numerador1']
    p2 = body['numerador2']
    q1 = body['denominador1']
    q2 = body['denominador2']

    resultado = Fraccion(p1 * q2, p2 * q1)
    resultado.simplify()

    json_resultado = resultado.toJSON()          # Serialization
    return HttpResponse(json_resultado,
                        content_type = "text/json-comment-filtered")

