# **Events**

This project is the test task for Modsen

## **Introduction**

This project is a backend for a website or application for working with events. It is possible to get events and users, get by id or name/login, add/update/delete events or users, get a list of user events or a list of event participants. Authorization is required to add and update, and administrator authorization is required to delete. Also addition a user as an event participant and removing them from the event participants are possible.

## **Structure**

The project is built on a clean architecture. There are three layers: Domain, Application and Infrastructure. At the domain level, there are business logic entities. At the application level, there are repository and UnitOfWork interfaces, DTO and use cases. The infrastructure level is divided into two projects: one for the database, the other for the web api. In the project with the database, all work with data occurs and repository interfaces and UnitOfWork are implemented, and an entity for storing refresh tokens is added. This layer works on Postgres. In the WebApi level, work with controllers occurs, which use UseCase classes from Application. Also at this level, work with refresh and access tokens is implemented. The project also contains a project with several validator and base repository tests.

## **Installation**

To install Project Title, follow these steps:

1. Clone the repository: **`git clone https://github.com/Pavelabramovich/Events`**
2. Create Postgress database with `events` name.
3. Then open: **`Events.Infrastructure/Events.DataBase/EventsContext.cs`** and update connection string with your settings
4. Then open VisualStudio, set `Events.DataBase` as startup project and run initial migration. These steps init database tables.
5. Then run `Events.WebApi`. Then a window with `swagger` should open.

## **Usage**

The use of the project is not difficult:

To authorize, find the `authenticate-user` field in the swagger in the user controller and enter valid data there. To authorize as an admin, use login=`lol@gmail.com`, password=`Pass123`. For a regular user, login=`crol@mail.ru`, password=`Vass123`. The field is called HashedPassword, but it is not actually hashed). After authorization, you will receive an access token and a refresh token. To authorize, you need to find the `Authorize` field in the upper right corner of the swagger window. In the input field that opens, enter `Bearer your-auth-token`, as indicated in the instructions. After the token is generated, you have **one minute** to do something, otherwise the token will become expired. To update it, find the `refresh-token` action and enter the previous access token and refresh token. Use the new access token for authorization.
