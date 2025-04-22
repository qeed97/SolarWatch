<a id="readme-top"></a>


# SolarWatch

## About

![project screenshot][screenshot]


SolarWatch is an ASP.NET Web API project that provides the sunrise and sunset times for a given city on a given date, in UTC times.
- Core Features:
  - sunrise and sunset times for a given city on a given date
  - Register, Login, Logout

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Built Using:

<p align="left">
  <a href="https://react.dev/" target="_blank"><img src="https://img.shields.io/badge/React-61DAFB?style=for-the-badge&logo=react&logoColor=white" alt="React"/></a>
  <a href="https://vitejs.dev/" target="_blank"><img src="https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white" alt="Vite"/></a>
  <a href="https://dotnet.microsoft.com/en-us/" target="_blank"><img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET"/></a>
  <a href="https://www.mysql.com/" target="_blank"><img src="https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white" alt="MySQL"/></a>
  <a href="https://www.docker.com/" target="_blank"><img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker"/></a>
  <a href="https://developer.mozilla.org/en-US/docs/Web/CSS" target="_blank"><img src="https://img.shields.io/badge/CSS-1572B6?style=for-the-badge&logo=css3&logoColor=white" alt="CSS"/></a>
</p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Prerequisites: 
- Docker
- Terminal / Powershell

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## How To Run

### `sample.env`: 
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

- OpenWeather API is an API that has a free tier available for public use. To access the API, you need an API key, which can be obtained by signing up on the website.
- https://openweathermap.org/
- From Terminal:
	- clone the directory
  	- Enter the project directory with
  		```bash
		  cd SolarWatch
   		```
	- On Macbook:
		- Run 
  			```bash 
    	  touch .env
    	  ``` 
        this will create a file call `.env`.
		- it might help to have a look at our `sample.env` file 
		to get an idea of the environment variables you need
		- you can run
      ```bash
      vim sample.env
      ```
      in the same directory to have a look. you can exit it with `:q`.
		- Run
      ```bash
      vim .env
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
      to create a migration. The application will apply the migrations in the Program.cs file.
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
      to create a migration. The application will apply the migrations in the Program.cs file.
	- Finally run
     ```bash
     docker compose up --build
     ``` 
	- Note: The way to project is setup on the repo, it expects the backend on port `8080` and the frontend on port `5173`. If you wish to change these you need to manually modify the frontend `Dockerfile` and `vite.config.js`, otherwise enjoy! The only port that needs to open on your machine is `5173`.


<p align="right">(<a href="#readme-top">back to top</a>)</p>






[screenshot]: PROJECTPICTURE.PNG
