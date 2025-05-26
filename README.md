# Proyecto Fullstack React & dotnet core 8

Arquitectura del proyecto:

- ProductsApi: Proyecto API Rest con dotnet core, se implementó un rateLimiter para proteccion contra ataques ddos o block CORS.
  - Base de datos InMemory
  - Modelos:
    - Product & ProductColorVariation
  - Controladores:
    - ProductsController
  - Context:
    - ProductContext para gestionar la integración con la DB
 
 - ProductsWebApp: Proyecto web que consume el API de productos, React Typescript
   -  Componentes:
      - ProductsList: Mostrar el listado de productos en una tabla.
  
JSON para crear un producto via el API:

{
  "name": "Macbook Pro M4 16 pulgadas",
  "description": "Equipo de alto rendimiento para tareas exigentes",
  "price": 2500,
  "inStock": true,
  "colorVariations": [
    {
      "name": "Space Black",
      "price": 2500
    },
    {
      "name": "Gray",
      "price": 2499
    }
  ]
}

Notas: 
- En la parte frontend solo llegué a implementar la tabla que consulta al iniciar la aplicación los productos disponibles. Me faltó integrar la gestion de ver el detalle de un producto y poder asi manipular el precio del mismo en base al color elegido.
- En el productController esta toda la logica de gestión de data de los productos. Deseaba implementar algun patrón para la capa de datos como el Repository o crear un archivo Service y gestionar desde alli, dejando al controller solo el manejo de excepciones y retorno de data segun requiera el cliente.
