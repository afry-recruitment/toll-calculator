# -*- coding: utf-8 -*-
"""
Created on Fri Sep 16 00:17:43 2022

@author: Aravindhan
"""


def cost(time):
    time = time.split(":")
    hour = int(time[0])
    mins = int(time[1])
    time1 = hour + mins/60
    if 0<=time1<=8:
         pay = 8
    if 8<time1<=12:
        pay = 12
    else:
        pay = 18
    return pay

#%% One time data
day = input(" Day (sun,mon,tue,wed,thu,fri,sat) :")

#%% Initializing the database
count = 0    
database = {}
veh_no = []   
nos = []

#%% User inputs for all vehicles during the day

while veh_no != 'end':
    veh_no = input("Vehicle number (type end to terminate) :")
    if veh_no == 'end':
        break
    
    if veh_no in nos:
        db_in = database[veh_no]
    else:
        database[veh_no]=[]
        db_in = database[veh_no]
        nos.append(veh_no)

    veh_type = input("Is Emergency vehicle (Y/N)")
    veh_time = input("Toll time (hh:mm:ss)")
    toll_cost = cost(veh_time)
    veh_time = veh_time.split(':')
    entry_current = [veh_type, int(veh_time[0]) ,int(veh_time[1]),int(veh_time[2]),toll_cost]
    db_in.append(entry_current)
    

veh_no = []


#%% Calculating final bill for the day

bills = [0]*len(database)

for v in range(len(database)):
    dat = database[nos[v]]
    bill =0
    for i in range(len(dat)):
        if (i==0): 
            last_time = int(dat[i][1]) + int(dat[i][2])/60
            bill = int(dat[i][4])
        else:
            if (int(dat[i][1]) + int(dat[i][2])/60 - last_time)>1:
                last_time = int(dat[i][1]) + int(dat[i][2])/60
                bill = bill+int(dat[i][4])
    
    if bill > 60:
        bill = 60
    
    if day == 'sun' or dat[0][0] == 'Y':
        bill = 0
    
    bills[v] = bill
        

for i  in range(len(bills)):
    print('%s - %d SEK' % (nos[i] , bills[i]))
    

        
    

