<h1 align="center">E.T. Nº12 D.E. 1º "Libertador Gral. José de San Martín"</h1>
<p align="center">
  <img src="https://et12.edu.ar/imgs/et12.svg">
</p>

## 💻 Computación 2025

**Asignaturas**:
- Base de datos
- Laboratorio de programacion orientada a objetos
- Proyecto Informático II
- Analisis de sistemas


**Integrantes del grupo**: 
- Zerpa Sierra, Sebastian Alberto
- Aguirre Ovando, Eric Alejandro
- Lopez, Angel Nahuel.

**Curso**: 5°7ma

# APIcultores de código QR 🐝

Este proyecto implementa un sistema de gestión de eventos, compra de entradas y validación mediante códigos QR, diseñado para brindar una solución completa a empresas organizadoras de espectáculos y actividades. La aplicación permite administrar clientes, eventos, funciones y ventas, generando códigos QR únicos y seguros para cada entrada. El sistema se apoya en una base de datos robusta y un backend desarrollado en C# con Dapper y ADO.NET, asegurando rendimiento y escalabilidad.

## Comenzando 🚀

Clonar el repositorio github, desde Github Desktop o ejecutar en la terminal o CMD:

```
git clone https://github.com/Darkops56/PlanEstudioOctubre
```

### Pre-requisitos 📋

- .NET 9.0 - [Descargar](https://dotnet.microsoft.com/download/dotnet/9.0)

- MySQL Server 8.0 - [Descargar](https://dev.mysql.com/downloads/installer/)

- [Visual Studio Code](https://code.visualstudio.com/download) o [Visual Studio 2022](https://learn.microsoft.com/es-es/visualstudio/install/install-visual-studio?view=vs-2022)

### Instalacion ⬇️

### a. Configuración de la base de datos.

El proyecto incluye scripts SQL dentro de /scripts/bd/MySQL. Ejecutar los siguientes en orden:
```
1. DDL.sql
2. De INSERT.sql ¡NECESARIO! hacer el INSERT INTO TipoEvento
```

### b. Configurar la cadena de conexión

Edita el archivo appsettings.json dentro de:
```
Src/Csharp/Eventos/
```

Reemplaza la cadena por tus credenciales de MySQL:
```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=bd_Eventos;User=tu_usuario;Password=tu_contraseña;"
    }
  }
```

### c. Restaurar dependencias

Dentro de **src\CSharp\Eventos** para instalar todos los paquetes requeridos:
```shell
dotnet restore
```

### d. Ejecuta el proyecto.
Inicia el proyecto:
```shell
dotnet run
```


## Construido con 🛠️

* Editor de código: [Visual Studio Code](https://code.visualstudio.com/#alt-downloads).

* Backend: [C#.](https://dotnet.microsoft.com/es-es/download)

* Acceso a datos: [Dapper](https://www.learndapper.com/) + [ADO](https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/).

* Base de datos: [MySQL](https://www.mysql.com/downloads/).

* Tests: [xUnit 2.4.2](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit)

## Autores ✒️

* **Eric Aguirre** - *Desarrollo* - [Erick-2008](https://github.com/Erick-2008/)
* **Sebastian Zerpa** - *Desarrollo* - [Darkops56](https://github.com/Darkops56/)
* **Angel Lopez** - *Documentación* - [angelnl610](https://github.com/angelnl610/)

## Documentación 📄

- [DER + UML](PlanEstudioOctubre\Proyecto\doc\MarkDown.md)
