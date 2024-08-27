# Instruction for the project
#### Commands for EF Core migrations:
``` shell
dotnet ef migrations add initial -s Events.API -p Events.Persistence
```
``` shell
dotnet ef database update -s Events.API -p Events.Persistence
```
#### Command for create docker container:
``` shell
docker compose up -d
```