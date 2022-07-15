# SimpleRestAPI

## Table of contents
* [Description](#description)
* [Technologies](#technologies)
* [Features](#features)
* [Setup](#setup)
	* [Requirements](#requirements)
	* [Exemple of Request](#exemple-of-request)
	* [Warnings](#warnings)
* [License](#license)

## Description
A Simple REST API 	

## Technologies
Project is created with:
* EntityFrameworkCore version: 6.0.6
* Swagger(Swashbuckle.AspNetCore) version: 6.2.3
* AspNetCore.Authentication.JwtBearer version 6.0.5
* xunit version: 2.4.1
* Moq version: 4.18.1

	
## Features
* Authentication
* Get, Post, Put, Delete From User (need authentication)
* Get By Name From User (don't need authentication)
* Get, Get By Name, Post, Put, Delete From Students (need authentication)

## Setup
* Download PublishSimpleRestAPI.zip
* Extract File
* Open the extracted folder
* Execute SimpleRestAPI.exe

### Requirements
Have port 5001 and 5000 unused

### Exemple of Request
https://localhost:5001/api/User/string

### Warnings
There are no Students in the database and only are one User:
```cs
{
	Name: string
	Password: string
}
```
## License
[MIT](https://choosealicense.com/licenses/mit/)
