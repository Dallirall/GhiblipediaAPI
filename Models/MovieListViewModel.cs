namespace GhiblipediaAPI.Models
{
    public class MovieListViewModel
    {
        public List<MovieInList> MovieList { get; set; }
        public int PaginationCount { get; set; }
    }
    public class MovieInList
    {
        public string Title { get; set; }
        public string JapanseTitle { get; set; }
        public string Year { get; set; }
    }
}