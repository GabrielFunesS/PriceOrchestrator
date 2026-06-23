# PriceOrchestrator 🚀

Plataforma backend de gestión de precios y sincronización de catálogos orientada a entornos retail complejos. El proyecto está diseñado como un laboratorio técnico para explorar y demostrar la evolución incremental de un monolito tradicional (arquitectura en capas) hacia patrones arquitectónicos avanzados como **Clean Architecture** y **Hexagonal Architecture**.

El objetivo es construir incrementalmente un sistema inspirado en escenarios reales de software enterprise, incorporando de manera práctica conceptos de resiliencia, observabilidad y procesamiento masivo de datos.

---

### 📌 Estado Actual del Proyecto: ~80% (En Desarrollo Activo)

El núcleo funcional del sistema ya se encuentra operativo sobre una estructura monolítica estructurada en capas, sirviendo como línea base (baseline) para las refactorizaciones planificadas en el Roadmap.

### 🛠️ Tech Stack & Herramientas

*   **Ecosistema:** .NET (Web API)
*   **Persistencia:** Entity Framework Core (Enfoque Code First)
*   **Base de Datos:** PostgreSQL
*   **Contenedores:** Docker (para empaquetado del entorno de desarrollo local)

---

### 🗺️ Roadmap de Evolución Técnica

Para reflejar un escenario de modernización de software del mundo real, el proyecto se dividirá y evolucionará a través de ramas específicas:

*   [x] **Fase 1 - Baseline:** Implementación de APIs REST esenciales, modelado de dominio retail y persistencia relacional con EF Core y PostgreSQL.
*   [ ] **Fase 2 - Dockerización:** Contenedorización completa de la app y base de datos para garantizar entornos portables y reproducibles.
*   [ ] **Fase 3 - Ramificación Arquitectónica:** 
    *   Creación de la rama `feature/clean-architecture` (Separación estricta de Entidades, Casos de Uso y Adaptadores).
    *   Creación de la rama `feature/hexagonal-architecture` (Aislamiento del dominio mediante Puertos y Adaptadores).
*   [ ] **Fase 4 - Enterprise Ready:** Incorporación progresiva de técnicas de procesamiento batch/masivo, mecanismos de resiliencia ante fallos y observabilidad estructurada.

---

### 👨‍💻 Propósito del Repositorio
Este espacio funciona como un reflejo de mi **capacidad de adaptación tecnológica y aprendizaje continuo**. Si bien cuento con un fuerte bagaje técnico resolviendo desafíos complejos con bases de datos relacionales tradicionales y arquitecturas preexistentes en producción, utilizo este proyecto personal para materializar, validar críticamente y llevar a la práctica nuevas herramientas y tendencias de la industria.
