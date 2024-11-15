# SolarWatch

## About
SolarWatch is an ASP.NET Web API project that provides the sunrise and sunset times for a given city on a given date, in UTC times.
- Core Features:
      - sunrise and sunset times for a given city on a given date
      - Register, Login, Logout

## Built Using:
- React/ Vite
- Tailwind CSS
- ASP.NET
- MSSQL
- Docker

## Prerequisites: 
- Docker
- Terminal / Powershell

## How To Run

###`sample.env`: 
```
APIKEY={openweathermap_APIKEY}
DB_USERNAME={USERNAME}
DB_NAME=solar-watch
DB_USER_PASSWORD={PASSWORD}
DB_PORT={DBPORT //reffer to docker compose.yml}
DOCKER_SERVER_NAME=solar-watch
LOCAL_SERVER_NAME=localhost
JWTSETTINGS__ISSUERSIGNINGKEY={JWT_ISSUER_KEY}
JWTSETTINGS__VALIDISSUER={JWT_ISSUER}
JWTSETTINGS__VALIDAUDIENCE={JWT_AUDIENCE}
```

-  OpenWeather API is an API that has a free tier available for public use. To access the API, you need an API key, which can be obtained by signing up on the website.
-  https://openweathermap.org/
- From Terminal:
	- clone the directory
 - 	- Enter the project directory with ```bash cd SolarWatch ```
	- On Macbook:
		- Run
  		``` bash touch .env```
    		this will create a file call `.env`.
		- it might help to have a look at our `sample.env` file 
		to get an idea of the environment variables you need
		- you can run
    		```vim sample.env
    		```
      		in the same directory to have a look. you can exit it with `:q`.
		- Run
   		```bash vim .env
   		```
   		now we can edit the file line by line when done, enter `:wq` which will write and quit the file.
	- On Windows:
		- Run
    		```bash
    		copy con .env
      		```
      		this will let you start writing your environment variable values. I'd recommend opening the sample `.env` file in a separate terminal tab or referring above for the values it should contain.
		- Once you have set all the values, hit `CRTL+Z` to save.
		- Now that our environment variables are done, run
  		```bash
    		cd SolarWatch
    		```
    		to enter the correct directory.
		- Here, we will run
    		```bash
    		dotnet ef migrations add InitialCreate
    		```
    		this will create a migration script to run when you run the project.
		- Finally, we can run
    		```bash
    		docker compose up --build
    		```
    		to get the docker running.
		- Note: The way to project is setup on the repo, it expects the backend on port `8080` and the frontend on port `5173`. If you wish to change these you need to manually modify the frontend `Dockerfile` and `vite.config.js`, otherwise enjoy! The only port that needs to open on your machine is `5173`.
    
- In an IDE
	- clone the directory
	- Enter the project and create a new `.env` file in the main directory that contains both the `SolarWatch` directory and `SolarWatchFrontend` directory.
	- Copy and paste the values from the `sample.env` into the new `.env` changing anything in `{}`.
	- In your own terminal or the IDE's built in interface, run
   	```bash
   	cd SolarWatch
    	```
    	and finally run
    	```bash
    	dotnet ef migrations add InitialCreate
    	```
    	to create a migration.
	- Finally run
    	```bash
    	docker compose up --build
    	```
	- Note: The way to project is setup on the repo, it expects the backend on port `8080` and the frontend on port `5173`. If you wish to change these you need to manually modify the frontend `Dockerfile` and `vite.config.js`, otherwise enjoy! The only port that needs to open on your machine is `5173`.
