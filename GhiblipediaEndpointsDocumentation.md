# GhiblipediaAPI Endpoints Documentation

---

URL for api calls to the service hosted on Render: https://ghiblipediaapi.onrender.com/api

<br>

**GET Endpoints:**

- /api/movies - Gets all movie objects from database including all object properties.<br>
  > _Ex: GET /api/movies_
- /api/movies/{id} - Gets a movie object from database, by its ID.
  > _Ex: GET api/movies/1_
- /api/movies/{englishTitle} - Gets a movie object from database, by its english title.
  > _Ex: GET api/movies/spirited away_
- /api/omdb/{title} - Gets a movie from OMDb. Use if you need to check OMDb for a movie. 
  > _Ex: GET api/omdb/spirited away_


<br>
<br>

**POST Endpoints:**

<br>

- /api/movies - Adds a movie object to the database. Use the .json template ("Database_insert_template" on Trello) for all the available object properties (omit unwanted properties). It's not possible to set your own movie ID at the moment, so that field is omitted in POSTs. English title is required.
  > _Ex: POST /api/movies_<br> _{"englishTitle": "Spirited Away","releaseYear": 2001}_
- api/omdb/{title} - Adds a movie object to the database using available data fetched from OMDb.
  > _Ex: POST /api/omdb/spirited away_

<br>
<br>

**PUT Endpoints:**

<br>

- /api/movies/{englishTitle} - Edit a movie object in database. Add whichever property you wish, and omit those you won't update. Use the .json template ("Database_insert_template" on Trello) for all the available object properties.
  > _Ex: PUT /api/movies/spirited away_<br> _{"japaneseTitle": "千と千尋の神隠し Sen to Chihiro no Kamikakushi","tags": ["Mythology", "Supernatural"]}_

<br>
<br>
