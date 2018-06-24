# AireLogic Technical Test

The aim of this technical test is to implement a simple bug tracker that meets the following requirements:
- It should be possible to view the list of open bugs, including their titles
- It should be possible to view the detail of individual bugs, including the title the full
description, and when it was opened
- It should be possible to create bugs
- It should be possible to close bugs
- It should be possible to assign bugs to people
- It should be possible to add people to the system
- It should be possible to change people’s names
- The web application should look nice
- The web application should expose some sort of API
- The data should be stored in some sort of database

## To do

- Logging configuration
- Production configuration
- Intergration tests
- Input validator service
- Move API to seperate project & container
- Parameterise DB credentials

## Pre-requisities

- dotnet core 2.0 sdk
- docker
- npm
- bower

## Run application

1. Clone repo to local

2. Run the following commands

```bash
$ docker-compose build
$ docker-compose up -d
```

3. Navigate to https://localhost:8080 in your browser of choice.

## Using the API

`GET /api/issues`

- Returns all of the issues.

```bash
$ curl -H "Content-Type: application/json" -i -X GET http://localhost:8080/api/issues
```

`GET /api/issues/{id}`

- Returns a single issues for a given id.

```bash
$ curl -H "Content-Type: application/json" -i -X GET http://localhost:8080/api/issues/1
```

`POST /api/issues`

- Creates a new issue.

```bash
$ curl --data '{"short_description":"This issue summary was created by the API.","long_description":"This issue description was created by the API."}' -H "Content-Type: application/json" -i -X POST http://localhost:8080/api/issues
```

`PUT /api/issues/{id}`

- Replaces issue for a given id.

```bash
$ curl --data '{"short_description":"This issue summary was updated by the API.","long_description":"This issue description was updated by the API.","assignee":0,"status":"Open"}' -H "Content-Type: application/json" -i -X PUT http://localhost:8080/api/issues/1
```

`PATCH /api/issues/{id}`

- Applies JSON patch to issue for a given id. _More info - http://jsonpatch.com/_

```bash
$ curl --data '[{"op":"replace","path":"short_description","value":"This issue summary was patched by the API."}]' -H "Content-Type: application/json" -i -X PATCH http://localhost:8080/api/issues/1
```

`DELETE /api/issues/{id}`

- Deletes issues.

```bash
$ curl -i -X DELETE http://localhost:8080/api/issues/1
```

`GET /api/people`

- Returns all of the people.

```bash
$ curl -H "Content-Type: application/json" -i -X GET http://localhost:8080/api/people
```

`GET /api/people/{id}`

- Returns a single person for a given id.

```bash
$ curl -H "Content-Type: application/json" -i -X GET http://localhost:8080/api/people/0
```

`POST /api/people`

- Creates a new people.

```bash
$ curl --data '{"first_name":"Niall","last_name":"Maloney"}' -H "Content-Type: application/json" -i -X POST http://localhost:8080/api/people
```

`PUT /api/people/{id}`

- Replaces person for a given id.

```bash
$ curl --data '{"first_name":"Niall","last_name":"Maloney"}' -H "Content-Type: application/json" -i -X PUT http://localhost:8080/api/people/1
```

`PATCH /api/people/{id}`

- Applies JSON patch to issue for a given id. _More info - http://jsonpatch.com/_

```bash
$ curl --data '[{"op":"replace","path":"first_name","value":"James"}]' -H "Content-Type: application/json" -i -X PATCH http://localhost:8080/api/people/1
```

`DELETE /api/people/{id}`

- Deletes people.

```bash
$ curl -i -X DELETE http://localhost:8080/api/people/1
```
