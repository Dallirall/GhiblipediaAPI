# GhiblipediaAPI

This API functions as the backend to the Ghiblipedia website. The frontend developement is made by [AMD-93](https://github.com/AMD-93), who is the owner of this project.


## To future developers of this API

I'm a beginner developer, and this is my first attempt at writing an API, so I'm sure there are many mistakes, not-so-good-practices, akward logic, etc. within the code. 
Feel free to make changes when you take over. And please bear this in mind when you encounter some confusion. 
<br>
Here follows some documentation of the project.

<br>
<br>

__Explanation of Components and Data Flow__

- Frontend App (Client): The user-facing application (e.g., a web app or mobile app) that consumes the API endpoints to display data and interact with the system.

- Backend API (ASP.NET Core): The core of the backend system. This is a REST API that handles all business logic, data processing, and communication with the database and external services. It is structured with Controllers, Data (business logic), Models (DTOs/schemas), and Services (custom logic, external API connections).

- Docker Container: The API is packaged and run inside a Docker container. This ensures a consistent environment for both local development and production deployment.

- Render.com: The platform where the Docker container is deployed to host the production API. Render manages the hosting, scaling, and uptime of the API service.

- PostgreSQL Database: The primary data store for the application, where all persistent data is stored. It is hosted and managed as a service on Supabase.com.

- External API: Represents any third-party service the backend interacts with, such as a payment gateway, a weather service, or an OAuth provider for user authentication.

__Data Flow:__

- The Frontend App sends HTTP requests (GET, POST, PUT) to the deployed Backend API on Render.com.

- The Backend API processes the request. It might read from or write to the PostgreSQL Database on Supabase or interact with an External API via the Services layer.

- The API sends a response back to the Frontend App with the requested data or a status code.



-credentials, location/owner, where I put the in dev env
-