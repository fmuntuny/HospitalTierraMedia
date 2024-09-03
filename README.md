# Hospital Tierra Media

Hospital Tierra Media es un proyecto que incluye una API y una aplicaci�n web para la gesti�n de informaci�n hospitalaria. 

## Contenido 

- ` HospitalTierraMedia.API/`: Directorio que contiene el proyecto de la API. 
- ` HospitalTierraMedia.Web/`: Directorio que contiene el proyecto de la aplicaci�n web. 
- `docker-compose.yml`: Archivo de configuraci�n para Docker Compose. 

## Requisitos 

- Docker v27.2.0
- Docker Compose v2.29.2
- Visual Studio versi�n 17.7.6

## Instalaci�n 

1.	Clona el repositorio a tu m�quina local.
2.	Instala Ubuntu WSL2 desde la Microsoft Store con el siguiente link: https://www.microsoft.com/store/productId/9PN20MSR04DW?ocid=pdpshare
3.	Una vez instalado el WSL te pide que crees el usuario, ll�malo �hospital� y pon la contrase�a que quieras pero recuerdala.
4.	Crear una carpeta para persistir los datos de la BD de mongoDB con el siguiente comando: mkdir -p /home/hospital/mongo_data
5.	Luego ingresa todos estos comandos uno a uno. A veces requiere que escribas la letra �y� para aceptar o que des a la tecla �enter�:
	a.	sudo apt-get update (te va a pedir que ingreses la contrase�a de tu usuario)
	b.	sudo apt-get upgrade
	c.	sudo apt-get install apt-transport-https ca-certificates curl software-properties-common
	d.	curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add �
	e.	sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
	f.	sudo apt-get update
	g.	sudo apt-get install docker-ce docker-ce-cli containerd.io
	h.	sudo service docker start
	i.	sudo usermod -aG docker hospital
	j.	newgrp Docker
	k.	sudo curl -L "https://github.com/docker/compose/releases/download/v2.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
	l.	sudo chmod +x /usr/local/bin/docker-compose
6.	Verificar la instalaci�n ejecutando:
	a.	docker --version (deber�a dar algo semejante a �Docker version 27.2.0, build 3ab4256�)
	b.	docker-compose --version (deber�a dar algo semejante a �Docker Compose version v2.29.2�)
7.	Reiniciar Docker con el comando: sudo service docker restart
8.	Acceder a la carpeta del proyecto desde WSL para poder ejecutarlo. Dejo un ejemplo, reemplazar por la letra de la partici�n del disco y path correspondiente a tu carpeta del proyecto: cd /mnt/c/REPOS/PERSONALES/PlanCarreraSSR/HospitalTierraMedia
9.	Ejecutar docker-compose up --build para crear los contenedores.
10.	Abrir un navegador y acceder a http://localhost:28807 para ver la web y a http://localhost:28806/swagger/index.html para ver la API.
11.	Se crea un usuario admin por defecto con el mail �admin@hospital.com� y la contrase�a �admin�. Ingresando esos datos en la web deber�a permitir el ingreso al sistema mostrando la p�gina de los Pacientes. No hay pacientes agregados, puede agregar uno con el bot�n �Agregar Paciente�. Luego puede editarlo o eliminarlo con los botones correspondientes.
