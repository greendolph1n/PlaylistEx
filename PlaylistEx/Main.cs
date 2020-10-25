using System;
using System.Collections.Generic;
using System.Net.Http;


namespace PlaylistEx
{
    class Main2
    {

        private static void Main(string[] args)
        {

            Console.WriteLine("Playlist URL: ");
            var playlistUrl = Console.ReadLine();


            if (playlistUrl.ToLower().Contains("list="))
            {
                var pathPlaylist = playlistUrl
                    .Replace("https", "")
                    .Replace("http", "")
                    .Replace("://", "")
                    .Replace("www.", "")
                    .Replace("youtube.com/", "").Trim();

                var client = new HttpClient();
                client.BaseAddress = new Uri("https://www.youtube.com");
                var result = client.GetAsync(pathPlaylist).Result;

                //entire page source as a string
                var pageSourceStr = result.Content.ReadAsStringAsync().Result;


                //"▶" can be seen as a "flag", only one instance of this character exists in the entire page source, so this is a handy reference point
                int index = pageSourceStr.IndexOf("▶"), index2 = index;
                int total = index;
                List<string> list = new List<string>();
                while (index > -1)
                {
                    index = pageSourceStr.IndexOf(@"""url"":""/watch?v=", total); //this string only appears before the youtube URL
                  //  Console.WriteLine("index 1 found it is " + index);
                    index2 = pageSourceStr.IndexOf(@"index=", total);//this string only appears after the youtube URL
                   // Console.WriteLine("index 2 found it is " + index2);

                    int difference = index2 - index; //difference between the index reference points is the length of the youtube URL
                    string stringURL = "youtube.com"+ pageSourceStr.Substring(index + 7, difference-13);
                    
                    Console.WriteLine("string is " + stringURL + " string length is " + stringURL.Length);
                    if (stringURL.Length > 96) //string URL will only ever be 96 characters long
                    {
                        Console.WriteLine("we are done");
                        break;
                    }

                    list.Add(stringURL);
                    Console.WriteLine("list length is +"+list.Count);
                    total = index2 + 1;
                    if (total > pageSourceStr.Length)
                    {
                        Console.WriteLine("total is too big");
                        break;
                    }
                }
                String[] str = list.ToArray();

                Console.WriteLine("LENGTH OF LIST IS " + str.Length);
                Console.ReadLine();


                if (str.Length == 0)
                {
                    Console.WriteLine("None video found in the playlist. Exiting...");
                    Console.ReadLine();
                    return;
                }


            }
            else
            {
                Console.WriteLine("URL is not a playlist");
                return;
            }
        }

        
        

        
    }
}
