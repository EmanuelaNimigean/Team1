# Team 1's application
Web application implemented by Team 1's members: Ema, Sorina, Patrick, Tudor and Radu.

Available at: [Heroku app](http://p33-team-1-lab-work.herokuapp.com/)

## Github CI/CD 
The workflow of the app : [![.NET](https://github.com/EmanuelaNimigean/Team1/actions/workflows/dotnet.yml/badge.svg)](https://github.com/EmanuelaNimigean/Team1/actions/workflows/dotnet.yml)

## Initial class diagram
![alt text](https://raw.githubusercontent.com/EmanuelaNimigean/Team1/main/Team1Project/team1ClassDiagram.png)

## Roles within the application
### Visitor (not in database)
As visitor you can: 

- visit the application and see the login box (for user authorization login)

- register to the system (as a user)

- see the privacy policy of the application, aswell as the version of it, in any case you want to report a bug

### DevOps (not in database)
As DevOps you can:

- deploy the product with specific version so it can be deplyoed upon request


### User
As user you can:

- see information about all entities in read only mode

### Operator
As operator you can:

- conduct CRUD operations on all entities so it can inform users about upcoming changes

### Administrator
As administrator you can:

- make sure that there is at least one administrator in the system so I don’t need to ask 3rd level support for help

- see list of all registered users to know who is registered to the system

- assign users to role of operator or administrator to let them do their job

## How to deploy to DockerHub (Locally)
1. Having docker account and docker application installed on your machine
2. Open cmd in project's root folder and execute the following commands:
```
dotnet build /p:AssemblyVersion={number_version}
```
Replace ```number_version``` with the version that it will be build (e.g. 1.1.1.1)

```
docker build . -t p33_team_1_lab_work
```

```
docker run -d -p 8081:80 --name p33_team_1_lab_work_container p33_team_1_lab_work
```

3. Optional. Rename the docker container
```
docker rename {initial_name} {desired_name}
```

4. The app should work locally

## How to deploy to Heroku (Online)
1. Create heroku account
2. Create application
3. Add Postgres database to your heroku application
4. Choose container registry as deployment method
5. Make sure application works locally

Login to Heroku and follow the cmd instructions
```
heroku login
```

Login to Container Registry

You must have Docker set up locally to continue. You should see output when you run this command.
```
docker ps
```

Now you can sign into Container Registry.
```
heroku container:login
```

Set the version of the app
```
dotnet build /p:AssemblyVersion={number_version}
```
Replace ```number_version``` with the version that it will be build (e.g. 1.1.1.1)

Build the Dockerfile in the current directory and push the Docker image, "team-1-project" is the heroku application name.
```
heroku container:push -a p33-team-1-lab-work web
```

Release the newly pushed images to deploy the app.
```
heroku container:release -a p33-team-1-lab-work web
```

The app should work online