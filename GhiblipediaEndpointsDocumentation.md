# GhiblipediaAPI Endpoints Documentation

---

URL for api calls to the service hosted on Render: https://ghiblipediaapi.onrender.com/api/v1/

<br>

**GET Endpoints:**

- /api/v1/movies - Gets all movie objects from database including all object properties.<br>
  > _Ex: GET /api/v1/movies_
- /api/v1/movies/{id} - Gets a movie object from database, by its ID.
  > _Ex: GET api/v1/movies/1_
- /api/v1/movies/{englishTitle} - Gets a movie object from database, by its english title.
  > _Ex: GET api/v1/movies/spirited away_
- /api/v1/omdb/{title} - Gets a movie from OMDb. Use if you need to check OMDb for a movie. 
  > _Ex: GET api/v1/omdb/spirited away_


<br>
<br>

**POST Endpoints:**

<br>

- /api/v1/movies - Adds a movie object to the database. Use the .json template ("Database_insert_template" on Trello) for all the available object properties (omit unwanted properties). It's not possible to set your own movie ID at the moment, so that field is omitted in POSTs. English title is required.
  > _Ex: POST /api/v1/movies_<br> _{"englishTitle": "Spirited Away","releaseYear": 2001}_
- api/v1/omdb/{title} - Adds a movie object to the database using available data fetched from OMDb.
  > _Ex: POST /api/v1/omdb/spirited away_

<br>
<br>

**PUT Endpoints:**

<br>

- /api/v1/movies/{englishTitle} - Edit a movie object in database. Add whichever property you wish, and omit those you won't update. Use the .json template ("Database_insert_template" on Trello) for all the available object properties.
  > _Ex: PUT /api/v1/movies/spirited away_<br> _{"japaneseTitle": "千と千尋の神隠し Sen to Chihiro no Kamikakushi","tags": ["Mythology", "Supernatural"]}_

<br>
<br>

**DELETE Endpoints:**

<br>

- /api/v1/movies/{englishTitle} - Delete the specified movie object in database. 
> _Ex: DELETE /api/v1/movies/spirited away_
- /api/v1/movies/{id} - Delete the movie of the specified ID in database. 
> _Ex: DELETE /api/v1/movies/13_
<br>
<br>
