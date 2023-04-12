# Código para agregar partidas
# Ricardo Campos Luna
# Miércoles 5 de abril del 2023

import sqlite3
import random

# Accedemos a la base de datos
con = sqlite3.connect("db.sqlite3")
cur = con.cursor()

# Creamos 100 partidas 
for _ in range(100):
    nivel = random.choice(list(range(1, 5)))
    cur.execute("INSERT INTO Game_partida (fecha_inicio, fecha_fin, puntaje, nivel_id, usuario_id) VALUES (?,?,?,?,?)", 
                ('2023-04-05 22:30', f'2023-04-06 %02.d:%02.d' %(random.choice(list(range(24))), random.choice(list(range(60)))),
                ((2 ** (nivel - 1)) * 200), nivel, random.choice(list(range(1, 55)))))

# Guardamos los cambios realizados
con.commit()

