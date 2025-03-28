

## 📝 Dockerizando una aplicación .NET Core 9.0

# Tarea3_Core - Aplicación .NET 9 en Docker

Este proyecto es una aplicación .NET 9.0 dockerizada, que puede ejecutarse sin necesidad de instalar Visual Studio ni el SDK de .NET en tu máquina. Solo necesitas tener **Docker** instalado.

## 📌 **Requisitos previos**
Antes de empezar, asegúrate de tener:
- [Docker instalado](https://www.docker.com/get-started) en tu computadora.
- Acceso a una terminal (CMD, PowerShell, Git Bash, etc.).
_ VPN por si el firewall tiene bloqueado el puerto 1433 (Caso del IPN)

Credenciales por default:
- Usuario: steven@test.com
- Password: steven
Esta cuenta es de rol: Administrador

---

## 🚀 **1. Construir la imagen Docker**
Ejecuta el siguiente comando en la terminal dentro del directorio donde se encuentra el `Dockerfile`:

```sh
docker build -t tarea3_core .
```

📌 **Explicación**:
- `docker build` → Construye la imagen Docker.
- `-t tarea3_core` → Asigna el nombre `tarea3_core` a la imagen.
- `.` → Indica que el `Dockerfile` está en el directorio actual.

---

## 🚀 **2. Ejecutar el contenedor**
Después de construir la imagen, inicia un contenedor con:

```sh
docker run -d -p 5000:5000 --name tarea3_container tarea3_core
```

📌 **Explicación**:
- `docker run` → Crea y ejecuta un nuevo contenedor.
- `-d` → Ejecuta el contenedor en segundo plano (modo *detached*).
- `-p 5000:5000` → Mapea el puerto **5000** del contenedor al **5000** de la máquina host.
- `--name tarea3_container` → Asigna el nombre `tarea3_container` al contenedor.
- `tarea3_core` → Es el nombre de la imagen creada en el paso anterior.

---

## 🚀 **3. Verificar que el contenedor está corriendo**
Para asegurarte de que el contenedor se está ejecutando, usa:

```sh
docker ps
```

Si ves `tarea3_container` en la lista, ¡la aplicación está corriendo! 🎉

---

## 🚀 **4. Acceder a la aplicación**
Abre tu navegador y visita:

```
http://localhost:5000
```


## 🚀 **5. Ver logs del contenedor**
Si quieres ver lo que está sucediendo en la aplicación en tiempo real:

```sh
docker logs -f tarea3_container
```

---

## 🚀 **6. Detener y eliminar el contenedor**
Si necesitas detener el contenedor:

```sh
docker stop tarea3_container
```

Si luego quieres eliminarlo:

```sh
docker rm tarea3_container
```

---

## 🚀 **7. Eliminar la imagen (opcional)**
Si deseas eliminar la imagen para reconstruirla desde cero:

```sh
docker rmi tarea3_core
```

---

## ✅ **Resumen rápido de comandos**
```sh
# 1. Construir la imagen
docker build -t tarea3_core .

# 2. Ejecutar el contenedor
docker run -d -p 5000:5000 --name tarea3_container tarea3_core

# 3. Verificar que el contenedor está corriendo
docker ps

# 4. Ver logs del contenedor
docker logs -f tarea3_container

# 5. Detener el contenedor
docker stop tarea3_container

# 6. Eliminar el contenedor
docker rm tarea3_container

# 7. Eliminar la imagen (opcional)
docker rmi tarea3_core
```

---

