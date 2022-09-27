using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ApplicationTemplate.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    private readonly ILogger<IFileService> _logger;
    private string _fileName;

    public List<int> movieIds { get; set; }
    public List<string> movieTitles { get; set; }
    public List<string> movieGenres { get; set; }

    public List<string> MovieGenres { get => movieGenres; set => movieGenres = value; }

    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;

        _fileName = $"{Environment.CurrentDirectory}/movies.csv";
       
         movieIds = new List<int>();
         movieTitles = new List<string>();
         movieGenres = new List<string>();
    }
    public void Read()
    {
        _logger.Log(LogLevel.Information, "Reading");
        Console.WriteLine("*** I am reading");

        
       
        // create parallel lists of movie details
        // lists must be used since we do not know number of lines of data
        //List<UInt64> MovieIds = new List<UInt64>();
        //List<string> MovieTitles = new List<string>();
        //List<string> MovieGenres = new List<string>();
        // to populate the lists with data, read from the data file
        try
        {
            StreamReader sr = new StreamReader(_fileName);
            // first line contains column headers
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // first look for quote(") in string
                // this indicates a comma(,) in movie title
                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    // no quote = no comma in movie title
                    // movie details are separated with comma(,)
                    string[] movieDetails = line.Split(',');
                    // 1st array element contains movie id
                    movieIds.Add(int.Parse(movieDetails[0]));
                    // 2nd array element contains movie title
                    movieTitles.Add(movieDetails[1]);
                    // 3rd array element contains movie genre(s)
                    // replace "|" with ", "
                    movieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    // quote = comma in movie title
                    // extract the movieId
                    movieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    movieTitles.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    line = line.Substring(idx + 2);
                    // replace the "|" with ", "
                    movieGenres.Add(line.Replace("|", ", "));
                }
            }
            // close file when done
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Movies in file {Count}", movieIds.Count);
    }

    public void Write(int movieId, string movieTitle, string genresString )
    {
        _logger.Log(LogLevel.Information, "Writing");
        Console.WriteLine("*** I am writing");

        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine($"{movieId},{movieTitle},{genresString}");
        sw.Close();
        
        movieIds.Add(movieId);
        movieTitles.Add(movieTitle);
        movieGenres.Add(genresString);
        // log transaction
        _logger.LogInformation("Movie id {Id} added", movieId);
    }   

    public void Display()
    {
        // Display All Movies
        // loop thru Movie Lists
        for (int i = 0; i < movieIds.Count; i++)
        {
            // display movie details
            Console.WriteLine($"Id: {movieIds[i]}");
            Console.WriteLine($"Title: {movieTitles[i]}");
            Console.WriteLine($"Genre(s): {movieGenres[i]}");
            Console.WriteLine();
        }
    }
}
