# GhiblipediaAPI

This API functions as the backend to the Ghiblipedia website. The frontend developement is made by [AMD-93](https://github.com/AMD-93), who is the owner of this project.


## To future developers of this API

I'm a beginner at programming and this is my first attempt at writing an API, so I'm sure there are many mistakes, not-so-good-practices, akward logic, etc. within the code. 
Feel free to make changes when you take over. And please bear this in mind when you encounter some confusion. 
<br>
Here follows some documentation of the project.

<br>
<br>

__Overview of Components and Data Flow__

- __Render.com:__ This is the hosting platform where the project is deployed. 
Render will automatically at some point deploy commits to the connected GitHub repo's main branch, although it's faster to manually deploy. 
API calls are made to https://ghiblipedia.onrender.com/api/. For further information about the endpoints, see 'GhiblipediaEndpointsDocumentation.md' in this repos source folder.
At the moment of writing, we use the free-tier, which comes with some restrictions, mainly the cold-start (which takes up to 1 minute) after 15 minutes without receiving inbound traffic.

- __Frontend App (Client):__ We use a static site for the frontend, also deployed on Render. Calls are made from here to the backend API web service.

- __REST API (ASP.NET Core):__ This API is a REST API made in ASP .NET Core. The aim is for the API to handle all business logic, data processing, and communication with the database and external services. 
It is structured with Controllers, Data (business logic), Models (DTOs/model classes), and Services (custom logic, external API connections).

- __Docker Container:__ The API is packaged and run inside a Docker container for both local development and production deployment. Render deploys this container.

- __Database on Supabase.com:__ This is where the PostgreSQL database for the project is hosted. 
To see information about connection strings etc, you go into the project and press the 'Connect' button in the top header of the page. 
We use the 'Session Pooler' connection type, as Render only has support for IPv4 addresses for outbound traffic. 
The Postgres connection string is stored as an environment variable in the service on Render. (From the project overview, go to GhiblipediaAPI -> Environment).
The tables have RLS by default, and needs RLS Policies to handle access control. At the moment, I have not yet looked over the security or anything, since I still have little knowledge in this area.

- External API: Represents any third-party service the backend interacts with, such as a payment gateway, a weather service, or an OAuth provider for user authentication.

__Data Flow:__

- The Frontend App sends HTTP requests (GET, POST, PUT) to the deployed Backend API on Render.com.

- The Backend API processes the request. It might read from or write to the PostgreSQL Database on Supabase or interact with an External API via the Services layer.

- The API sends a response back to the Frontend App with the requested data or a status code.



-credentials, location/owner, where I put the in dev env
-


Database info:
-Movies table
