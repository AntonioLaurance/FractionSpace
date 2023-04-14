from rest_framework import serializers
from .models import Usuario, Partida, Nivel, Pregunta

class UsuarioSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Usuario
        fields = ('id', 'nombre', 'contraseña', 'grado', 'grupo', 'num_lista')

class PartidaSerializer(serializers.ModelSerializer):
    class Meta:
        model = Partida
        fields = ('id', 'usuario', 'nivel', 'fecha_inicio', 'fecha_fin', 'puntaje')
     
class NivelSerializer(serializers.ModelSerializer):
    class Meta:
        model = Nivel
        fields = ('id', 'pregunta', 'num', 'den', 'respuesta')

class PreguntaSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Pregunta
        fields = ('id', 'texto', 'operacion', 'num1', 'den1', 'num2', 'den2', 'puntaje')

class GraficaUsuarioSerializer(serializers.ModelSerializer):
    class Meta:
        model = Usuario
        fields = ('id', 'nombre', 'grupo')  

class GraficaPartidaSerializar(serializers.ModelSerializer):    
    class Meta:
        model = Partida
        fields = ('fecha_inicio', 'fecha_fin', 'nivel')

class GraficaSerializer(serializers.Serializer):
    nombre = serializers.CharField(max_length = 30)
    fecha_fin = serializers.DateField()
    puntaje = serializers.IntegerField()
    grupo = serializers.CharField(max_length = 1)
    nivel = serializers.IntegerField()
    