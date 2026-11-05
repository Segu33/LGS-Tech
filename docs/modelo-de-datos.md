# Diagrama de entidad-relación


Una orden pertenece a un equipo. Puede ingresar sin técnico y asignarse más adelante. Los pagos y los archivos se registran sobre la orden correspondiente.

```mermaid
erDiagram
    Cliente ||--o{ Equipo : posee
    Equipo ||--o{ OrdenReparacion : recibe
    ApplicationUser |o--o{ OrdenReparacion : atiende
    OrdenReparacion ||--o{ ArchivoOrden : contiene
    OrdenReparacion ||--o{ Pago : registra

    Cliente {
        int Id PK
        string Documento UK
    }
    Equipo {
        int Id PK
        int ClienteId FK
    }
    ApplicationUser {
        string Id PK
    }
    OrdenReparacion {
        int Id PK
        int EquipoId FK
        string TecnicoId FK "Opcional"
    }
    ArchivoOrden {
        int Id PK
        int OrdenReparacionId FK
    }
    Pago {
        int Id PK
        int OrdenReparacionId FK
        decimal Monto
    }
```

`ApplicationUser` representa al usuario de Identity. En la relación con la orden, el usuario asignado desempeña el rol de técnico. El diagrama muestra las claves y los campos necesarios para interpretar las relaciones. [Configuración del modelo](../LGS.Tech/Data/AplicationDbContext.cs)

