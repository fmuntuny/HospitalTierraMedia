
# Hospital Tierra Media

Hospital Tierra Media es un proyecto que incluye una API y una aplicaci�n web para la gesti�n de informaci�n hospitalaria.

## Contenido

- `HospitalTierraMedia.API/`: Directorio que contiene el proyecto de la API.
- `HospitalTierraMedia.Web/`: Directorio que contiene el proyecto de la aplicaci�n web.
- `docker-compose.yml`: Archivo de configuraci�n para Docker Compose.

## Requisitos

- Docker v27.2.0
- Docker Compose v2.29.2
- Visual Studio versi�n 17.7.6

## Instalaci�n

1. Clona el repositorio a tu m�quina local.
2. Instala Ubuntu WSL2 desde la [Microsoft Store](https://www.microsoft.com/store/productId/9PN20MSR04DW?ocid=pdpshare).
3. Una vez instalado WSL, crea un usuario llamado `hospital` y establece una contrase�a que puedas recordar.
4. Crea una carpeta para persistir los datos de la base de datos de MongoDB:

   ```bash
   mkdir -p /home/hospital/mongo_data
   ```

5. Ejecuta los siguientes comandos uno a uno. A veces, se te pedir� que ingreses "y" para aceptar o que presiones "Enter":

   ```bash
   sudo apt-get update
   sudo apt-get upgrade
   sudo apt-get install apt-transport-https ca-certificates curl software-properties-common
   curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
   sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
   sudo apt-get update
   sudo apt-get install docker-ce docker-ce-cli containerd.io
   sudo service docker start
   sudo usermod -aG docker hospital
   newgrp docker
   sudo curl -L "https://github.com/docker/compose/releases/download/v2.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
   sudo chmod +x /usr/local/bin/docker-compose
   ```

6. Verifica la instalaci�n ejecutando:

   ```bash
   docker --version
   docker-compose --version
   ```

   Deber�as ver algo como:

   - Docker: `Docker version 27.2.0, build 3ab4256`
   - Docker Compose: `Docker Compose version v2.29.2`

7. Reinicia Docker con el comando:

   ```bash
   sudo service docker restart
   ```

8. Accede a la carpeta del proyecto desde WSL para poder ejecutarlo. Por ejemplo:

   ```bash
   cd /mnt/c/REPOS/PERSONALES/PlanCarreraSSR/HospitalTierraMedia
   ```

   Aseg�rate de reemplazar con la letra de la partici�n del disco y la ruta correcta de tu carpeta del proyecto.

9. Ejecuta el siguiente comando para crear los contenedores:

   ```bash
   docker-compose up --build
   ```

10. Abre un navegador y accede a:

   - [http://localhost:28807](http://localhost:28807) para ver la aplicaci�n web.
   - [http://localhost:28806/swagger/index.html](http://localhost:28806/swagger/index.html) para ver la API.

11. Se crea un usuario admin por defecto con el correo `admin@hospital.com` y la contrase�a `admin`. Ingresando esos datos en la web, deber�as poder acceder al sistema y ver la p�gina de los Pacientes. No hay pacientes agregados inicialmente; puedes agregar uno con el bot�n "Agregar Paciente". Luego, puedes editarlo o eliminarlo con los botones correspondientes.
