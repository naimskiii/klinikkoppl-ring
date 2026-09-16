

## Klinikkopplæring.no

En læringsplattform for opplæring av nye ansatte i tannklinikker. Studenter logger inn med en unik kode, går gjennom opplæringsmoduler med quizer for at studenten skal få mest mulig forståelse av fagstoff. 
Dette er et fullstack-prosjekt bygget fra bunnen av som et læringsprosjekt i .NET-utvikling.
Hjelp av den beste læreren, altså(Youtube)
Videoer som jeg fikk læringsutbytte av: https://www.youtube.com/watch?v=qcczkv-Hz5c og https://www.youtube.com/watch?v=O8DUfVBT4Qk&t=5175s



**Backend delen**
- [.NET 10](https://dotnet.microsoft.com/) — ASP.NET Core Web API
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) — ORM mot databasen
- **SQLite** — foreløpig database (bytte til PostgreSQL eller MySQL)
- Bruker en code generator i Infrastructure, foreløpig slik at den kan generere innloggings kode for klienten 

**Frontend del**
- HTML/CSS/JavaScript 
- Kommuniserer med backend med REST-API 

## Hva som skjer next

- Bytte SQLite ut 
- Bygge admin grensesnitt. Administrere klienter/studenter og generere innloggings kode derfra 
- Flytte quiz og fremgang av klient fra frontend til backend
- Deploye backend-en til en server
- Gjøre nettsiden mer sikker bla. hashe studentkoder i databasen 
