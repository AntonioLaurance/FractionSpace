from django.db import models

# Create your models here.
class Usuario(models.Model):
    nombre = models.CharField(max_length = 30)
    contraseña = models.CharField(max_length = 60)
    grado = models.IntegerField()                       # All 6 for now
    grupo = models.CharField(max_length = 1)            # Can be 'A' or 'B'
    num_lista = models.IntegerField()                   # Define a range

# Ver el asunto de quitar el nivel de la pregunta 
# Plantilla del banco de preguntas
class Pregunta(models.Model):
    # Modalidad de pregunta teórica
    texto = models.CharField(max_length = 255, null = True)

    # Modalidad de pregunta práctica
    operacion = models.CharField(max_length = 1, null = True)        # Can be '+', '-', '*' or '/'
    num1 = models.IntegerField(null = True)
    den1 = models.IntegerField(null = True)
    num2 = models.IntegerField(null = True)
    den2 = models.IntegerField(null = True)

    # Valor de la pregunta (puntaje)
    puntaje = models.IntegerField()                     # Should be a non-negative number

# En nuestro videojuego un nivel es una pregunta aleatorea del banco de preguntas
# Esto debido a que cada nivel tiene solamente una pregunta
class Nivel(models.Model):
    # ID de pregunta (no se puede borrar una pregunta si ya se le aplicó a un usuario)
    pregunta = models.ForeignKey(Pregunta, null = False, blank = False, on_delete = models.PROTECT)

    # Modalidad de respuesta práctica
    num = models.IntegerField(null = True)
    den = models.IntegerField(null = True)

    # Modalidad de respuesta teórica
    respuesta = models.CharField(max_length = 255, null = True)

# PROTECT no se puede borrar nivel hasta que no haya ninguna partida asociada a ese nivel
# Es equivalente a una sesión del usuario
class Partida(models.Model):
    # Claves foraneas (FK)
    usuario = models.ForeignKey(Usuario, null = False, blank = False, on_delete = models.CASCADE)
    nivel = models.ForeignKey(Nivel, null = False, blank = False, on_delete = models.PROTECT)

    # Para ver cuanto tiempo pasan nuestros usuarios frente al videojuego
    fecha_inicio = models.DateTimeField()
    fecha_fin = models.DateTimeField()

    # Puntaje obtenido
    puntaje = models.IntegerField()
