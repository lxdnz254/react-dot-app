# A .NET + React application with Neo4j Graph DB

Testing out the intricacies of developing a .NET middleware application, using React + Redux as a front-end framework, and linking to a Neo4j Graph Database.

The scope for this application comes from the initial development of a team [application build](https://github.com/OpenPolytechnicBITProjectGroup/Musician-Venue_App) from the Open Polytechnic of NZ Bachelor of Information Technology course 2016-2017.

My idea is to develop the API using the .NET framework and implement the front-page with React. Full details can be found in the [Microsoft ASP Documentation](https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-2.2)

But to start your own application quickly
```bash
dotnet new react -o my-new-app
cd my-new-app
```
will create a scaffolding to start from. All your .NET code is placed in the top project folder `/my-new-app` and the React scripts, and any additional front-end packages you wish to add are under `/my-new-app/ClientApp/`

If you wish to add Redux, this can be done automatically too by using 
```bash
dotnet new reactredux -o my-new-app
```
instead.

## Database platform

A Neo4j graph database is required which can be installed locally from [Neo4j](https://neo4j.com/download/)
