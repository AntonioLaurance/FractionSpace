# Código para agregar a los estudiantes de acuerdo a su número de lista y grupo
# Ricardo Campos Luna
# Miércoles 5 de abril del 2023 

import string
import sqlite3
import random

# Accedemos a la base de datos
con = sqlite3.connect("db.sqlite3")
cur = con.cursor()

# Nombres de nuestros usuarios 
names = ["Topito", "Lalito", "Antonio", "Laura", "Emiliano", "Sofía", "Úrsula", "Patricia",
	 	"Samuel", "Einstein", "Hittler", "Eleonora", "Moises", "Luis", "Eduardo",
	 	"Terabithia", "Alejandra", "Lorena", "Rodrigo", "Francisco", "Bertha", "Tau", 
	 	"Gabriela", "Citlali", "Andrea", "Alejandro", "Christian", "Sergio", "Magali",		      
	 	"Natalia", "Legna", "Kiara", "Miguel", "Mojanet", "Jenny", "David", "Goliat", 
 	 	"José", "Josefina", "Karla", "Jonathan", "Isai", "Lovachesky", "Paulina", "Isis", 
	 	"Vanessa", "Daniel", "Orlando", "Unix", "Samantha", "Tania", "Mayra", "Isabela", "Javier"]

# Contraseñas de nuestros usuarios (generamos cadenas aleatorias)
passwords = []

for i in range(54):
	passwords.append(''.join(random.SystemRandom().choice(string.ascii_letters + string.digits) for _ in range(20)))

# Agregamos los números de lista del grupo A
for i in range(1, 30):        # [1, 29]
	cur.execute("INSERT INTO Game_usuario (nombre, contraseña, grado, grupo, num_lista) VALUES (?,?,?,?,?)", (names[i - 1], passwords[i - 1], 6, 'A', i))

# Agregamos los números de lista del grupo B
for i in range(1, 26):        # [1, 25]
	cur.execute("INSERT INTO Game_usuario (nombre, contraseña, grado, grupo, num_lista) VALUES (?,?,?,?,?)", (names[i + 28], passwords[i + 28], 6, 'B', i))

# Guardamos los cambios realizados 
con.commit() 


