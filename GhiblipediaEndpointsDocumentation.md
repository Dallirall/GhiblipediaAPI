# GhiblipediaAPI Endpoints Documentation

---

URL for api calls to the service hosted on Render: https://ghiblipediaapi.onrender.com/api

### BrowseView Endpoints

<br>

**GET Functions:**

- /api/movies - Gets all movie objects from database including all object properties.<br>
  > _Ex: GET /api/movies_
- /api/movies/{movieID} - Gets a movie object from database, by its ID.
  > _Ex: GET api/movies/1_
- /api/movies/{englishTitle} - Gets a movie object from database, by its english title. (Write spaces with '%20')
  > _Ex: GET api/movies/spirited%20away_


<br>
<br>

**POST Functions:**

<br>

- /api/movies - Adds a movie object to the database. Use the .json template ("Database_insert_template" on Trello) for all the available object properties (omit unwanted properties). It's not possible to set your own movie ID at the moment, so that field is omitted in POSTs. English title is required.
  > _Ex: POST /api/movies_<br> _{"englishTitle": "Spirited Away","releaseYear": 2001}_
- api/movies/omdb/{englishTitle} - Adds a movie object to the database using available data fetched from omdbapi.com.
  > _Ex: POST /api/movies/omdb/spirited%20away_

<br>
<br>

**PUT Functions:**

<br>

- /api/movies/{englishTitle} - Edit a movie object in database. Add whichever property you wish, and omit those you won't update. Use the .json template ("Database_insert_template" on Trello) for all the available object properties.
  > _Ex: PUT /api/movies/spirited%20away_<br> _{"japaneseTitle": "千と千尋の神隠し Sen to Chihiro no Kamikakushi","tags": ["Mythology", "Supernatural"]}_

<br>
<br>
