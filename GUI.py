# -*- coding: utf-8 -*-
"""
Created on Sun Oct 25 22:36:58 2020

@author: DACR9
"""
import math
import tkinter 
import cv2 
from random import randint
import time
import numpy as np



tk= tkinter.Tk()
canvas = tkinter.Canvas(tk, width = 500 , height = 500)
tk.title('Cruise control')
tk.geometry("500x500")



theta = int(input())
x1= 100
x2= 100+400*math.cos(theta*math.pi/180)
y1= 400 
y2= 400- 400*math.sin(theta*math.pi/180)

confirm=math.atan2(abs(y2-y1), abs(x1-x2))*180/math.pi

#Create canvas line (x1,y1,x2,y2,fill=color)
canvas.create_line(x1,y1,x2,y2,fill='Red')
canvas.create_line(100,400,x2,400,fill='blue')
canvas.create_line(x2,400,x2,y2,fill='Red')
canvas.pack()
#img = cv2.imread('')
tk.mainloop()
# while True:
#     try:
#         tk.update()
#     except:
#         break


