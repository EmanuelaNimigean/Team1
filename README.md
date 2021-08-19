# Team1's application
Web application implemented by Team1's members: Ema, Sorina, Patrick, Tudor, and Radu.

## Initial class diagram
![alt text](https://raw.githubusercontent.com/EmanuelaNimigean/Team1/main/Team1Project/team1ClassDiagram.png)

## Implemented user stories

As user I want to specify github login of the intern to be able to see his activity on the github.

As user I want to see list of public projects of the intern in the github, to be able to navigate to them

As user I want to see how many public projects each intern has in the github

## How to create a docker container and image of the project.
Open cmd in project's root folder and execute the following commands:

```
docker build . -t team-1-project 

```

```
docker run -d -p 8081:80 --name team-1-project-container team-1-project

```

## How to deploy to Heroku
1. Create heroku account
2. Create application
3. Add Postgres database to your heroku application
4. Choose container registry as deployment method
5. Make sure application works locally

Log in to Heroku and follow the cmd instructions
```
heroku login

```
Log in to Container Registry
You must have Docker set up locally to continue. You should see output when you run this command.
```

docker ps

```

Now you can sign into Container Registry.
```

heroku container:login

```

Build the Dockerfile in the current directory and push the Docker image, "team-1-project" is the heroku application name.
```
heroku container:push -a team-1-project web

```

Release the newly pushed images to deploy the app.
```
heroku container:release -a team-1-project web

```
