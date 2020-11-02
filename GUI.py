    # -*- coding: utf-8 -*-
"""
Created on Sun Oct 25 22:36:58 2020

@author: DACR9
"""
import math
import tkinter as tk
from PIL import ImageTk,Image
#import cv2 
# from random import randint
# import time
# import numpy as np

def rampa():
    thet = theta.get()
    x1= 100
    x2= 100+400*math.cos(float(thet)*math.pi/180)
    y1= 400 
    y2= 400-400*math.sin(float(thet)*math.pi/180)
    # confirm=math.atan2(abs(y2-y1), abs(x1-x2))*180/math.pi
#Create canvas line (x1,y1,x2,y2,fill=color)
    canvas.create_line(x1,y1,x2,y2,fill='Red')
    canvas.create_line(100,400,x2,400,fill='blue')
    canvas.create_line(x2,400,x2,y2,fill='Red')
    #label1=tk.Label(root, text="Pendiente:")
    #tk.Label(root, text="Velocidad")
    #canvas.create_window(200, 180, window=label1)
def carimage (vell):
    img = Image.open(r"C:\Users\DACR9\Documents\9 semestre\Control\Local_reto\car.png")
    img = img.resize((50, 50), Image.ANTIALIAS)
    img = img.rotate(vell)
    image  = ImageTk.PhotoImage(img)


def V_nominal():
    vnom=vel.get()
    thet= theta.get()
    img = Image.open(r"C:\Users\DACR9\Documents\9 semestre\Control\Local_reto\opacity.png")
    img = img.resize((50, 50), Image.ANTIALIAS)
    img = img.rotate(float(thet))
    global image
    image  = ImageTk.PhotoImage(img)
    x=100+(float(vnom)*4)*math.cos(float(thet)*math.pi/180)
    y=390-(float(vnom)*4)*math.sin(float(thet)*math.pi/180)
    canvas.create_image(x,y, image = image)
    
    
    
    
root= tk.Tk()
canvas = tk.Canvas(root, width = 500 , height = 500)
canvas.pack()
root.title('Cruise control')
root.geometry("500x500")
#leer ángulo
theta = tk.Entry(root)
vel = tk.Entry(root)
canvas.create_window(0, 50, window= theta)
canvas.create_window(0, 75, window= vel)
#imprimir triangulo
boton = tk.Button(text='Pendiente', command=rampa)
canvas.create_window(100, 50, window= boton)
#imprimir carrito opaco
boton2 = tk.Button(text='Velocidad', command=V_nominal)
canvas.create_window(100, 75,window= boton2 )
#root.mainloop()


while True:
    try:
        root.update()
    except:
        break


