# _Hair Salon Appointment Tracker_

#### _Solo Project 8 C# - Epicodus Seattle_

#### By _**Isaac Niamatali**_

## Description

_A band tracker where you can set venues and add bands to said venues with performance dates..._


## Setup/Installation Requirements

* _Install MAMP_
* _Change port to 8889 for mySQL_
* _Start server_
* _Clone repository_
* _Have .NET installed_
* _go to myPHP and import the .sql file_
* _DotNET restore then DotNET run_
* _Navigate to (http://localhost:5000)_

### [Click here](https://github.com/aniamatali/BandTracker) to view this Project.

## Technologies Used
* _C#_
* _.NET_
* _HTML and CSS_
* _SQL_
* _MAMP_

## Creating Database
*_ CREATE DATABASE bandtracker
* _ CREATE TABLE venues (id serial PRIMARY KEY, name VARCHAR(255));_
* _ CREATE TABLE bands (id serial PRIMARY KEY, description VARCHAR(255), performancedate VARCHAR(255), stylist_id INT, hours VARCHAR(255));_
*_ CREATE TABLE venues_bands (id serial PRIMARY KEY, venue_id INT, band_id INT);


### License
Copyright (c) 2017 **_{Alvin Isaac Niamatali}_**
