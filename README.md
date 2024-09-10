# Instruction for the project
## Commands

### Docker

 Создание и запуск docker container:

``` shell
docker compose up -d --build
```

###### Если нужно запустить только Postgresql и Redis в Docker, то нужно сделать docker compose в директории **server**
### Ports
Клиент доступен по адресу:
```
http://localhost:3000/
```
Сервер вместе со свагером:
```
http://localhost:5000/swagger
```
### Commands for EF Core migrations:
``` shell
dotnet ef migrations add initial -s Events.API -p Events.Persistence
```
``` shell
dotnet ef database update -s Events.API -p Events.Persistence
```
---
## Project description
Есть разделение на две роли, **Admin** и **User**(Participant).
#### Клиент:
###### При запуске будет иметься Admin user@example.com с паролем qweQWE123
###### А также учатник kuncovs19@gmail.com с паролем qweQWE123
Зарегистрироваться можно только от имени участника. Чтобы сделать это от роли Admin, нужно делать это через Swagger, например:
``` json
{
  "email": "user@example.com",
  "password": "qweQWE123",
  "passwordConfirmation": "qweQWE123",
  "role": "Admin"
}
```
После авторизации от имени  **участника** можно перейти на страницы:
- Все события (с возможностью зарегистрироваться)
- События на которые участник зарегистрировался
- Профиль участника (Переход при нажатии на почту в блоке nav)
От имени **администратора**:
- Все события (с возможностью **Изменить**, **Сохранить** и **Удалить**)
- Админ панель
	- Можно создать новое событие
	- Можно активировать и деактивировать аккаунт другого администратора