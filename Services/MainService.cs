using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ApplicationTemplate.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly IFileService _fileService;
    public MainService(IFileService fileService)
    {
        _fileService = fileService;
    }

   

    public void Invoke()
    {
        string choice;
        do
        {
            Console.WriteLine("1) Add Movie");
            Console.WriteLine("2) Display All Movies");
            Console.WriteLine("X) Quit");
            choice = Console.ReadLine();

            // Logic would need to exist to validate inputs and data prior to writing to the file
            // You would need to decide where this logic would reside.
            // Is it part of the FileService or some other service?
            if (choice == "1")
            {
                _fileService.Read();
                // Add Movie
                // ask user to input movie title
                Console.WriteLine("Enter the movie title");
                // input title
                string movieTitle = Console.ReadLine();
                // check for duplicate title
                List<string> LowerCaseMovieTitles =_fileService.movieTitles.ConvertAll(t => t.ToLower());
                if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
                {
                    Console.WriteLine("That movie has already been entered");
                   // Logger.Info("Duplicate movie title {Title}", movieTitle);
                }
                else
                {
                    // generate movie id - use max value in MovieIds + 1
                    int movieId = _fileService.movieIds.Max() + 1;
                    // input genres 
                    List<string> genres = new List<string>();
                    string genre;
                    do
                    {
                        // ask user to enter genre
                        Console.WriteLine("Enter genre (or done to quit)");
                        // input genre
                        genre = Console.ReadLine();
                        // if user enters "done"
                        // or does not enter a genre do not add it to list
                        if (genre != "done" && genre.Length > 0)
                        {
                            genres.Add(genre);
                        }
                    } while (genre != "done");
                    // specify if no genres are entered
                    if (genres.Count == 0)
                    {
                        genres.Add("(no genres listed)");
                    }
                    // use "|" as delimeter for genres
                    string genresString = string.Join("|", genres);
                    // if there is a comma(,) in the title, wrap it in quotes
                    movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;
                    // display movie id, title, genres
                    Console.WriteLine($"{movieId},{movieTitle},{genresString}");
                    _fileService.Write(movieId, movieTitle, genresString);
                }
            }
            else if (choice == "2")
            {
                _fileService.Read();
                _fileService.Display();
            }
        }
        while (choice != "X");

    }
}
