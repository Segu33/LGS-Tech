# LGS TECH · Gestión de servicio técnico

LGS TECH es un sistema web para administrar un taller de reparación de celulares y computadoras. Permite registrar clientes y equipos, crear órdenes de reparación, asignar técnicos, actualizar diagnósticos y estados, adjuntar evidencias y registrar pagos.

El objetivo es centralizar la información de cada reparación y facilitar el seguimiento del trabajo y del saldo pendiente.

## Funcionalidades

- Gestión de clientes con altas, bajas, modificaciones y listado mediante Vue y AJAX.
- Registro de equipos asociados a un cliente.
- Órdenes de reparación con técnico asignado, diagnóstico, estado y costo estimado.
- Acceso del técnico a sus órdenes y actualización del trabajo realizado.
- Adjuntos privados por orden, con validación de formato y tamaño.
- Registro de pagos y cálculo del saldo estimado.
- Autenticación, roles y avatar personalizado.
- API de consulta de clientes protegida mediante JWT.

## Tecnologías

ASP.NET Core MVC y C# sobre .NET 8, Entity Framework Core con Pomelo, MySQL/MariaDB, ASP.NET Core Identity, Vue 3, Bootstrap y JavaScript.

## Instalación local

### Requisitos

- SDK .NET 8.0.424, indicado en `global.json`.
- MySQL/MariaDB. La base de demostración fue exportada desde MariaDB 10.4.32 de XAMPP.
- Acceso a internet para restaurar los paquetes y cargar Vue desde su CDN.
- Postman para ejecutar la colección de pruebas de la API.

### 1. Obtener el proyecto

La versión de trabajo se encuentra en la rama `Main`.

```powershell
git clone --branch DevLuis https://github.com/Segu33/LGS-Tech.git
cd LGS-Tech
```

### 2. Importar la base de datos

Iniciar MySQL desde XAMPP. En phpMyAdmin, crear una base vacía llamada `lgs_tech`, con juego de caracteres `utf8mb4`. Seleccionarla e importar [database/lgs_tech.sql](database/lgs_tech.sql).

El script incluye las tablas, claves, índices, relaciones y datos de prueba. Debe ejecutarse en una base vacía. El historial de migraciones está incluido.

### 3. Restaurar los archivos de demostración

Desde la carpeta raíz del repositorio, ejecutar una vez sobre la copia recién descargada.

```powershell
Expand-Archive -LiteralPath .\database\archivos-demo.zip -DestinationPath .
```

El [paquete de archivos](database/archivos-demo.zip) contiene los dos adjuntos y los dos avatares referenciados en el SQL. Al extraerlo se crean las carpetas correspondientes dentro de `LGS.Tech/PrivateFiles/` y `LGS.Tech/wwwroot/uploads/avatars/`.

### 4. Configurar y ejecutar

Abrir PowerShell en la carpeta raíz del repositorio y ejecutar los siguientes comandos. La conexión del ejemplo corresponde a XAMPP local con el usuario `root` sin contraseña. Ajustar el usuario, la contraseña y el puerto según la instalación.

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Port=3306;Database=lgs_tech;User=root;Password=;"
$env:Jwt__Key = [guid]::NewGuid().ToString("N") + [guid]::NewGuid().ToString("N")
$env:Jwt__Issuer = "LGS.Tech"
$env:Jwt__Audience = "LGS.Tech.Local"
$env:SeedUsers__AdminPassword = "Admin123!"
$env:SeedUsers__TecnicoPassword = "Tecnico1234!"

dotnet restore .\LGS.Tech\LGS.Tech.csproj
dotnet build .\LGS.Tech\LGS.Tech.csproj
dotnet run --project .\LGS.Tech\LGS.Tech.csproj --no-launch-profile --urls http://localhost:5291
```

Abrir [http://localhost:5291](http://localhost:5291). Mantener la terminal abierta mientras se utiliza el sistema. Las variables de configuración permanecen en esa sesión de PowerShell. Al generar otra clave JWT se deben obtener nuevos tokens mediante el login.

Las contraseñas de inicialización se utilizan si faltan las cuentas iniciales de Administrador o Técnico. Los usuarios importados conservan las contraseñas del SQL.

## Usuarios de prueba

Estas credenciales están destinadas exclusivamente a la demostración local del proyecto.

| Rol | Correo | Contraseña |
|---|---|---|
| Administrador | admin@lgstech.com | `Admin123!` |
| Técnico | tecnico@lgstech.com | `Tecnico1234!` |
| Técnico | luis.s@gmail.com | `gabrieL33!` |

La base contiene 11 clientes, 3 equipos, 2 órdenes, 1 pago, 2 adjuntos y 3 usuarios con sus roles.

## Modelo de datos

Las seis entidades principales son Cliente, Equipo, OrdenReparacion, ArchivoOrden, Pago y ApplicationUser. Un cliente tiene varios equipos. Una orden pertenece a un equipo y puede asignarse a un técnico. Los pagos y los archivos dependen de la orden.

[Ver el diagrama de relaciones](docs/modelo-de-datos.md) · [Ver la imagen del DER](docs/Der%20Lgs%20Lab2.JPG).

## Matriz de evidencias



| Requerimiento mínimo | Estado | Enlace al código / controladores | Evidencia (captura / GIF / video) |
| 1. Seis entidades relacionadas | Un cliente tiene varios equipos. Cada orden corresponde a un equipo y puede asignarse a un técnico. De la orden dependen los archivos y pagos. | [Modelo y claves foráneas](LGS.Tech/Data/AplicationDbContext.cs) | [Diagrama de entidad-relación con las seis entidades](docs/modelo-de-datos.md) |

| 2. Login, roles y avatar | Identity gestiona el ingreso y la salida. El técnico consulta sus órdenes y cambia su foto desde Mi avatar. El administrador también puede cambiar el avatar de otro usuario. | [Login y logout](LGS.Tech/Controllers/AccountController.cs) · [Control de acceso a órdenes](LGS.Tech/Controllers/OrdenesReparacionController.cs#L89) · [Cambio de avatar](LGS.Tech/Controllers/AvatarController.cs) | [Cambio confirmado desde Mi avatar con el rol Técnico](docs/evidencias/avatar-tecnico.JPG). [Mis órdenes en una sesión de Técnico, con 0 resultados](docs/evidencias/mis-ordenes-tecnico.JPG). |

| 3. Manejo de archivos adicionales |  El administrador adjunta archivos a una orden. El servidor valida el tamaño y la extensión, guarda el archivo fuera de la carpeta pública y comprueba permisos antes de abrirlo. | [Subida](LGS.Tech/Controllers/ArchivosOrdenController.cs#L62) · [Apertura autorizada](LGS.Tech/Controllers/ArchivosOrdenController.cs#L264) | [GIF de la secuencia de capturas](docs/evidencias/subida-archivo-orden.gif) · [Archivo seleccionado](docs/evidencias/archivo-seleccionado.JPG) · [Archivo guardado en la orden](docs/evidencias/archivo-en-orden.JPG) · [Imagen abierta](docs/evidencias/archivo-abierto.JPG). |

| 4. ABM con Vue y AJAX | Clientes permite listar, crear, editar y eliminar mediante peticiones JSON sin recargar la página. | [Vista Vue y peticiones](LGS.Tech/Views/Clientes/Index.cshtml#L258) · [Acciones del controlador](LGS.Tech/Controllers/ClientesController.cs) | [GIF de la secuencia de capturas del ABM](docs/evidencias/abm-clientes-vue.gif) · [Listado](docs/evidencias/clientes-listado.JPG) · [Alta](docs/evidencias/clientes-alta.JPG) · [Edición](docs/evidencias/clientes-edicion.JPG) · [Baja](docs/evidencias/clientes-baja.JPG). |

| 5. Paginado por servidor |  Clientes, Equipos y Órdenes solicitan una página por vez. Usuarios todavía carga toda la lista. | [Clientes](LGS.Tech/Repositories/ClienteRepository.cs#L36) · [Equipos](LGS.Tech/Repositories/EquipoRepository.cs#L42) · [Órdenes](LGS.Tech/Repositories/OrdenReparacionRepository.cs#L76) · [Usuarios](LGS.Tech/Controllers/UsuariosController.cs#L32) | [Petición con pagina=2 y respuesta 200 OK](docs/evidencias/clientes-pagina-2-headers.JPG) · [Respuesta JSON de la página 2](docs/evidencias/clientes-pagina-2-response.JPG), con un cliente devuelto, paginaActual = 2, totalPaginas = 2 y totalRegistros = 11. |

| 6. Selección asíncrona de entidades |  Al cargar o editar un equipo se busca el cliente por AJAX. En una orden se buscan equipo y técnico. La búsqueda de técnicos carga el rol completo en el servidor antes de filtrar y devolver hasta diez coincidencias. | [Buscar clientes](LGS.Tech/Controllers/EquiposController.cs#L229) · [Formulario de equipo](LGS.Tech/Views/Equipos/Crear.cshtml) · [Buscar equipos y técnicos](LGS.Tech/Controllers/OrdenesReparacionController.cs#L460) · [Formulario de orden](LGS.Tech/Views/OrdenesReparacion/Crear.cshtml#L322) | [GIF de la secuencia de capturas](docs/evidencias/seleccion-cliente-ajax.gif) · [Formulario inicial](docs/evidencias/ajax-cliente-inicio.JPG) · [Búsqueda lo con petición fetch y estado 200](docs/evidencias/ajax-cliente-busqueda.JPG) · [Cliente seleccionado](docs/evidencias/ajax-cliente-seleccionado.JPG). |

| 7. Web API REST con JWT | El login emite un token. Las consultas de clientes por listado y por ID requieren un token de Administrador. | [Emisión de JWT](LGS.Tech/Controllers/Api/AuthController.cs#L31) · [Autorización y endpoints](LGS.Tech/Controllers/Api/ClientesApiController.cs#L10) | [Colección importable de Postman](postman/LGS-TECH-API.postman_collection.json). Pruebas sobre `GET /api/clientes`: [401 sin token](docs/evidencias/jwt-sin-token-401.JPG) · [403 con token de Técnico](docs/evidencias/jwt-tecnico-403.JPG) · [200 con token de Administrador](docs/evidencias/jwt-admin-200.JPG). |

## Pruebas de la API

Importar [LGS-TECH-API.postman_collection.json](postman/LGS-TECH-API.postman_collection.json) en Postman. En Variables, verificar que `baseUrl` sea `http://localhost:5291` y completar `adminPassword` con `Admin123!` y `tecnicoPassword` con `Tecnico1234!`.

Ejecutar las seis solicitudes en orden.

| Solicitud | Resultado esperado |
|---|---|
| Login de Administrador | 200 y token JWT |
| Login de Técnico | 200 y token JWT |
| Listado sin token | 401 |
| Listado con token de Técnico | 403 |
| Listado con token de Administrador | 200 y clientes paginados |
| Consulta por ID con token de Administrador | 200 y datos del cliente |

Los logins guardan automáticamente los tokens. El listado selecciona un ID existente para la última consulta. Si el token expira, ejecutar nuevamente el login correspondiente. La colección entregada contiene las contraseñas y los tokens vacíos.

La aplicación MVC utiliza la sesión de Identity. La API de consulta utiliza autenticación Bearer JWT y autorización por rol.

## Alcance actual

Clientes, Equipos y Órdenes tienen paginado por servidor. La administración de Usuarios muestra la lista completa, actualmente de tres cuentas. La búsqueda de técnicos devuelve hasta diez coincidencias, aunque primero carga en memoria los usuarios de ese rol.

La restauración del SQL y del paquete de archivos se comprobó en una base independiente. La copia del proyecto compiló con cero errores y cero advertencias. Se verificaron los accesos de las tres cuentas, las respuestas JWT 401/403/200, la consulta paginada, el detalle de una orden y la apertura de los dos adjuntos y los avatares.

## Archivos de entrega

- `database/lgs_tech.sql` contiene la estructura y los datos de prueba.
- `database/archivos-demo.zip` contiene los archivos asociados a esos datos.
- `docs/` contiene el DER y las evidencias.
- `postman/` contiene la colección de pruebas.
- `.gitignore` excluye los archivos de compilación y las carpetas de subidas utilizadas durante la ejecución.