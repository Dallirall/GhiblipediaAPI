# GhiblipediaAPI
<sub>Readme last updated 2025-07-29</sub>

This API provides backend functionalities to the [Ghiblipedia website](https://ghiblipedia.onrender.com/). 
The frontend developement is made by [AMD-93](https://github.com/AMD-93), who is the owner of this project.
This API was originally made by [Dallirall](https://github.com/Dallirall/), 
but since August 2025 this project is continued on the [GhiblipediaAPI fork](https://github.com/AMD-93/GhiblipediaAPI) on AMD-93's GitHub repo.
The original repo on Dallirall's GitHub will be archived.

<br>

## To future developers of this API

I'm a beginner at programming and this is my first attempt at writing an API, so I'm sure there are many mistakes, not-so-good-practices, akward logic, etc. within the code. 
Feel free to make changes when you take over. And please bear this in mind when you encounter some confusion. 
<br>
Here follows some documentation of the project.

<br>
<br>

### Overview of Components

- __Render.com:__ This is the hosting platform where the project is deployed. 	
Render will automatically deploy commits to the connected GitHub repo's main branch, although it's faster to manually deploy from the API's main page on Render. 
API calls are made to https://ghiblipedia.onrender.com/api/v{version number}. For further information about the endpoints, see 'GhiblipediaEndpointsDocumentation.md' in this repos source folder.
At the moment of writing, we use the free-tier, which comes with some restrictions, mainly the cold-start (which takes up to 1 minute) after 15 minutes without receiving inbound traffic.

- __Frontend App (Client):__ We use a static site for the frontend, also deployed on Render. Calls are made from here to the backend API web service.

- __Auth0:__ We use Auth0 for handling login and user accounts. 

- __REST API (ASP.NET Core):__ This API is a REST API made in ASP.NET Core. The aim is for the API to handle all business logic, data processing, and communication with the database and external services. 
(Some features of the web page however might be more practical to program in the frontend.).
The API is structured with Controllers, Data (business logic), Models (DTOs/model classes), and Services (custom logic, external API connections).

- __Docker Container:__ The API is packaged and run inside a Docker container for both local development and production deployment. Render deploys this container.

- __Database on Supabase.com:__ This is where the PostgreSQL database for the project is hosted. 
To see information about connection strings etc, you go into the project page on Supabase and press the 'Connect' button in the top of the page. 
We use the 'Session Pooler' connection type, as Render only has support for IPv4 addresses for outbound traffic. 
The Postgres connection string is stored as an environment variable on Render. (From the project overview, go to GhiblipediaAPI -> Environment).
The tables have RLS by default, and needs RLS Policies to handle access control. At the moment, I have not yet looked over the security or anything, since I still have little knowledge in this area.

- __OMDb API:__ [OMDb API](https://www.omdbapi.com/) is the service we have used to get data for the Studio Ghibli movies. 
There are endpoints on GhiblipediaAPI that gets a movie from OMDb API and stores it in our Supabase 'movies' table. 
Data requests to OMDb are sent to 'http://www.omdbapi.com/?apikey=[yourkey]&'. Our apikey is stored as an environment variable on Render.

<br>
<br>

### Local development environment setup 
Here follows some general steps for you to set up the environment on your local machine. Hopefully it will work smoothly for you.

- Install Visual Studio with the '.NET desktop development' and 'ASP.NET and web development' workloads. 
- Install Docker desktop 
- Clone the repo from Github into Visual Studio.
- Add a new appsettings.json file to the project and name it 'appsettings.Local.json'. This is where to program will look for connection strings, passwords etc. Make sure to never track this file by git. It should be listed in the .gitignore file.
- Add the following objects to the appsettings.Local.json file: 
	- "ConnectionStrings": {
    "DefaultConnection": [Our Supabase Connection String] 
	}
	- "OmdbApi": {
    "ApiKey": [Our OMDb API key]
	}
- In Visual Studio, right click on the Dockerfile in the solution explorer and select 'Build Docker Image'.
- Now when you run/debug the application, instead of 'http' or 'https', the run button should say 'Container (Dockerfile)', otherwise select that option in the drop-down menu.
- Run the application. It should start a new Docker container. You can test if it works by calling the API through Postman (if not installed, install it). 
Try doing a GET request with this url: http://localhost:32770/api/v1/movies. 
If it doesen't find the API, open Docker desktop and check the running containers ports. By default it's usually 32770:8080 for HTTP requests, but it might be something else for you perhaps, in which case you use that port in the url instead. 

I hope this works. Remember that when you do git commits and push to the main branch, those changes will be deployed, so if you don't want your changes to go live, push to another branch.

### About the API's logic and structure
- __Controllers:__ At the moment, there are two controller classes: MoviesController, for handling transactions between frontend and the database's 'movies' table, 
and OmdbController, for transactions with OMDb API. 
- __Endpoints:__ The GET endpoints are self-explanatory. There are in total two POST endpoints. The one we mainly use is the PostMovieInDBWithDataFromOmdb() on the OmdbController. 
This calls the OMDb API and gets data on the specified movie, and inserts that into the Ghiblipedia database's 'movies' table. 
When making API calls to OMDb API, by default you get the short version of the movie plot, which I have mapped to the 'summary' column of the 'movies' table, 
and if you want the full plot, you have to make another call with the &plot=full query parameter, which is why in the logic behind PostMovieInDBWithDataFromOmdb(), two calls are made to OMDb. 
The second call is to get the full plot, which is stored in the 'plot' column of the 'movies' table.
The PUT endpoints is for updating an optional amount of properties to a database object. The columns 'japanese_title', 'trailer_url' and 'tags' (which can store an array of strings) in the 'movies' table needs to be manually updated, as this data is not available on OMDb.
The Japanese titles have been aquired from [Studio Ghiblis official website](https://www.ghibli.jp/works/), and the romanization has been done by me, but can usually be found elswhere, such as the movie's Wikipedia page.
- __Models:__ The model class 'MovieResponse' is for HttpGet requests, where read-only fields ('id' and 'created_at') are included.
The model class 'MovieCreate' is for HttpPost request, which require a value for the EnglishTitle property, and where only writable properties are included, as 'id' and 'created_at' are generated in the database and should not be assigned any values manually.
The 'MovieUpdate' model class is for HttpPut requests, which do not require any specific parameter in the request body.
The properties in 'MovieCreate' and 'MovieUpdate' has JsonPropertyName attributes, which I use to get the database column name in snake case for creating dynamcal SQL queries in CustomSqlServices.cs.
The JsonPropertyName attributes are not necessary for 'MovieResponse', because of the 'Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true' setting in Program.cs.
The OmdbMovie model class has properties corresponding to the JSON object fetched from the API call to OMDb API.
- __Dependencies (some of them):__ 
	- ORM: Dapper
	- Object-to-object mapper: Automapper
	- Npgsql: .NET Data provider for PostgreSQL
	

### Good to know 
Here are some notes about things you might want to know.

- For questions about login credentials, or anything in general, ask AMD-93.
- I have little knowledge about the proper use of HTTP status codes, so I have likely used the wrong status code at the wrong place in many cases. 
Feel free to correct this.
- ToDo's and other resources are on the project's Trello page.


Lastly, if there is anything you need to ask, feel free to email me and I will do my best to reply. Get my email address from AMD-93.
From 1st of september 2025 however, I will be starting my studies and might not be able to reply in a reasonable time, if at all.