# GhiblipediaAPI

This API functions as the backend to the Ghiblipedia website. The frontend developement is made by [AMD-93](https://github.com/AMD-93), who is the owner of this project.


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
API calls are made to https://ghiblipedia.onrender.com/api/. For further information about the endpoints, see 'GhiblipediaEndpointsDocumentation.md' in this repos source folder.
At the moment of writing, we use the free-tier, which comes with some restrictions, mainly the cold-start (which takes up to 1 minute) after 15 minutes without receiving inbound traffic.

- __Frontend App (Client):__ We use a static site for the frontend, also deployed on Render. Calls are made from here to the backend API web service.

- __Auth0:__ We use Auth0 for handling login and user accounts. 

- __REST API (ASP.NET Core):__ This API is a REST API made in ASP .NET Core. The aim is for the API to handle all business logic, data processing, and communication with the database and external services. 
(Some features of the web page however might be more practical to program in the frontend, such as search boxes).
The API is structured with Controllers, Data (business logic), Models (DTOs/model classes), and Services (custom logic, external API connections).

- __Docker Container:__ The API is packaged and run inside a Docker container for both local development and production deployment. Render deploys this container.

- __Database on Supabase.com:__ This is where the PostgreSQL database for the project is hosted. 
To see information about connection strings etc, you go into the project page on Supabase and press the 'Connect' button in the top of the page. 
We use the 'Session Pooler' connection type, as Render only has support for IPv4 addresses for outbound traffic. 
The Postgres connection string is stored as an environment variable on Render. (From the project overview, go to GhiblipediaAPI -> Environment).
The tables have RLS by default, and needs RLS Policies to handle access control. At the moment, I have not yet looked over the security or anything, since I still have little knowledge in this area.

- __OMDb API:__ [OMDb API](https://www.omdbapi.com/) is the service we have used to get data for the Studio Ghibli movies. 
There is an endpoint on GhiblipediaAPI that gets a movie from OMDb API and stores it in our Supabase 'movies' table. 
Data requests are sent to 'http://www.omdbapi.com/?apikey=[yourkey]&'. Our apikey is stored as an environment variable on Render.

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
	- "Jwt": {
    "JwtAuthority": [Our Auth0 domain adress (found in the account on Auth0)]
	}
- In Visual Studio, right click on the Dockerfile in the solution explorer and select 'Build Docker Image'.
- Now when you run/debug the application, instead of 'http' or 'https', the run button should say 'Container (Dockerfile)', otherwise select that option in the drop-down menu.
- Run the application. It should start a new Docker container. You can test if it works by calling the API through Postman (if not installed, install it). 
Try doing a GET call with this url: http://localhost:32770/api/movies. 
If it doesen't find the API, open Docker desktop and check the running containers ports. By default it's usually 32770:8080 for HTTP requests, but it might be something else for you perhaps, in which case you use that port in the url instead. 

I hope this works. Remember that when you do git commits and push to the main branch, those changes will be deployed, so if you don't want your changes to go live, push to another branch.

### About the API's logic and structure
- __Controllers:__ At the moment, there is only one Controller class (MoviesController), for handling transactions between frontend and the database 'movies' table. 
- __Endpoints:__ The GET endpoints are self-explanatory. There are two POST endpoints. The one we mainly use is the PostMovieInDBWithDataFromOmdb(). This calls the OMDb API and gets data to insert in the 'movies' table. 
When making API calls to OMDb API, by default you get the short version of the movie plot, which I have mapped to the 'summary' column of the 'movies' table, 
and if you want the full plot, you have to make another call with the &plot=full parameter, which is why in the logic behind PostMovieInDBWithDataFromOmdb(), two calls are made to OMDb. 
The second call is to get the full plot, which is stored in the 'plot' column of the 'movies' table.
The main PUT endpoints is for updating an optional amount of properties to a db row. The columns 'japanese_title', 'trailer_url' and 'tags' (which can store an array of strings) in the 'movies' table needs to be manually updated, as this data is not available on OMDb.
The Japanese titles have been aquired from Studio Ghiblis official website, and the romanization has been done by me, but can usually be found elswhere, such as the movie's Wikipedia page.
- __Models:__ There is definately a better way of doing this, but what I've done is to have one model class for HttpGet's, where read-only fields ('movie_id' and 'created_at') are included,
and another class for HttpPost's and Put's, where only writable fields are included, as 'movie_id' and 'created_at' are generated in the database and should not be assigned any values.
The properties of these classes are in the camel case standard (e.g. EnglishTitle), but because I use the property names to create custom dynamic SQL, I needed the property names as they are in the db table too, 
which is why I made the Dto classes (MovieDtoGet corresponds to MovieGet, etc), and convert between the models using Automapper (the mapping is done in MappingProfile.cs).
The OmdbMovie class has properties corresponding to the JSON object got from the API call to OMDb API.


### Good to know 
Here are some notes about things you might want to know.

- For questions about login credentials, or anything in general, ask AMD-93.
- ToDo's and other resources are on the project's Trello page.


Lastly, if there is anything you need to ask, feel free to email me and I will do my best to reply. Get my email address from AMD-93.
From 1st of september 2025 however, I will be starting my studies and might not be able to reply in a reasonable time, if at all.