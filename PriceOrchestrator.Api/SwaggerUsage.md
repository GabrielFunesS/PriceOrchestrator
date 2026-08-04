Configuración y uso de Swagger API Key
=====================================

Resumen
------
Este proyecto expone Swagger UI en /swagger. En entornos Development Swagger se muestra sin protección. En entornos no Development, si existe la clave de configuración `Swagger:ApiKey`, el acceso a `/swagger` requiere enviar la cabecera HTTP `X-Swagger-Key` con el valor configurado.

Cómo configurar
---------------
1. Abre el fichero de configuración del entorno (por ejemplo `appsettings.Development.json` o en Kubernetes/hosting la variable de entorno equivalente).
2. Añade la sección `Swagger` con la propiedad `ApiKey`:

```json
"Swagger": {
  "ApiKey": "mi-clave-secreta"
}
```

3. Reinicia la aplicación para que lea la nueva configuración.

Cómo usar
---------
- En Development: abre `http://localhost:{puerto}/swagger` sin cabeceras.
- En Production/Stage (cuando `Swagger:ApiKey` está configurado): envía la cabecera `X-Swagger-Key` con el valor de la clave.

Ejemplos
--------

1) Acceso al HTML de Swagger con curl (entorno protegido):

```
curl -H "X-Swagger-Key: mi-clave-secreta" http://localhost:5000/swagger/index.html
```

2) Abrir en el navegador: usar una extensión o herramienta que añada la cabecera `X-Swagger-Key` a la petición, o configurar un proxy que inyecte la cabecera.

Notas de seguridad
------------------
- Guarda la clave en un secreto (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault) o en variables de entorno; evita poner la clave en repositorio.
- Considera habilitar autenticación/ACL real para entornos públicos; la API key es una protección ligera para UI de desarrollo.
