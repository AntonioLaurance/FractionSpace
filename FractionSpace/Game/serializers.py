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
        