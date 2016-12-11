using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
  internal class TubeNames
  {
    private static void MainTube()
    {
      // This has absolutely nothing to do with Advent of Code. I was just too lazy to create a new repo.

      /*
        There is currently an ad on the tube stating that only one tube station's name does not contain 
        any letters in the word mackerel. The solution to the puzzle is St John's wood.

        Are there any other words, for which there is only one station with no intersecting letters?
      */

      var wordStream = new HttpClient().GetStreamAsync(@"https://github.com/dwyl/english-words/blob/master/words.txt?raw=true").Result;
      //var shorterWordStream = new HttpClient().GetStreamAsync(@"https://raw.githubusercontent.com/first20hours/google-10000-english/master/google-10000-english-no-swears.txt").Result;
      var words = new StreamReader(wordStream).ReadToEnd().SplitLines().Select(word => word.ToLowerInvariant());
      var stations = Stations.SplitLines().Select(station => station.ToLowerInvariant());

      var nonIntersectingStationsPerWord = words.Distinct().ToDictionary(
        word => word,
        word => stations.Where(station => DoesNotIntersect(word, station)));

      var wordsWithOnlyOneNonIntersectingStation = nonIntersectingStationsPerWord.Where(kv => kv.Value.Count() == 1).Select(kv => kv.Key).ToList();

      wordsWithOnlyOneNonIntersectingStation.ForEach(word =>
        Console.WriteLine($"{word} ({stations.Single(station => DoesNotIntersect(word, station))})"));
      Console.WriteLine(wordsWithOnlyOneNonIntersectingStation.Count);

      var morningtonCrescents = nonIntersectingStationsPerWord.Where(kv => kv.Value.Contains("mornington crescent"));
      var minMorningtonCrescents = morningtonCrescents.OrderBy(kv => kv.Value.Count()).First();
      Console.WriteLine($"{minMorningtonCrescents.Key}: {minMorningtonCrescents.Value.Count()}");

      Console.ReadKey();
    }

    private static bool DoesNotIntersect(string word, string station)
    {
      var stationLetters = new HashSet<char>(station.ToCharArray());
      
      foreach (var chr in word)
      {
        if (stationLetters.Contains(chr)) return false;
      }

      return true;
    }
    
    private const string Stations = @"Acton Town
Aldgate
Aldgate East
All Saints
Alperton
Amersham
Angel
Archway
Arnos Grove
Arsenal
Baker Street
Balham
Bank
Barbican
Barking
Barkingside
Barons Court
Bayswater
Beckton
Beckton Park
Becontree
Belsize Park
Bermondsey
Bethnal Green
Blackfriars
Blackhorse Road
Blackwall
Bond Street
Borough
Boston Manor
Bounds Green
Bow Church
Bow Road
Brent Cross
Brixton
Bromley-by-Bow
Buckhurst Hill
Burnt Oak
Caledonian Road
Camden Town
Canada Water
Canary Wharf
Canary Wharf
Canning Town
Cannon Street
Canons Park
Chalfont & Latimer
Chalk Farm
Chancery Lane
Charing Cross
Chesham
Chigwell
Chiswick Park
Chorleywood
Clapham Common
Clapham North
Clapham South
Cockfosters
Colindale
Colliers Wood
Covent Garden
Crossharbour
Croxley
Custom House
Cutty Sark
for Maritime Greenwich
Cyprus
Dagenham East
Dagenham Heathway
Debden
Deptford Bridge
Devons Road
Dollis Hill
Ealing Broadway
Ealing Common
Earl's Court
East Acton
East Finchley
East Ham
East India
East Putney
Eastcote
Edgware
Edgware Road
Edgware Road
Elephant & Castle
Elm Park
Elverson Road
Embankment
Epping
Euston
Euston Square
Fairlop
Farringdon
Finchley Central
Finchley Road
Finsbury Park
Fulham Broadway
Gallions Reach
Gants Hill
Gloucester Road
Golders Green
Goldhawk Road
Goodge Street
Grange Hill
Great Portland Street
Greenford
Green Park
Greenwich
Gunnersbury
Hainault
Hammersmith
Hammersmith
Hampstead
Hanger Lane
Harlesden
Harrow & Wealdstone
Harrow-on-the-Hill
Hatton Cross
Heathrow Terminals 1, 2, 3
Heathrow Terminal 4
Heathrow Terminal 5
Hendon Central
Heron Quays
High Barnet
Highbury & Islington
Highgate
High Street Kensington
Hillingdon
Holborn
Holland Park
Holloway Road
Hornchurch
Hounslow Central
Hounslow East
Hounslow West
Hyde Park Corner
Ickenham
Island Gardens
Kennington
Kensal Green
Kensington (Olympia)
Kentish Town
Kenton
Kew Gardens
Kilburn
Kilburn Park
King George V
Kingsbury
King's Cross St. Pancras
Knightsbridge
Ladbroke Grove
Lambeth North
Lancaster Gate
Langdon Park
Latimer Road
Leicester Square
Lewisham
Leyton
Leytonstone
Limehouse
Liverpool Street
London Bridge
London City Airport
Loughton
Maida Vale
Manor House
Mansion House
Marble Arch
Marylebone
Mile End
Mill Hill East
Monument
Moorgate
Moor Park
Morden
Mornington Crescent
Mudchute
Neasden
Newbury Park
North Acton
North Ealing
North Greenwich
North Harrow
North Wembley
Northfields
Northolt
Northwick Park
Northwood
Northwood Hills
Notting Hill Gate
Oakwood
Old Street
Osterley
Oval
Oxford Circus
Paddington
Park Royal
Parsons Green
Perivale
Piccadilly Circus
Pimlico
Pinner
Plaistow
Pontoon Dock
Poplar
Preston Road
Prince Regent
Pudding Mill Lane
Putney Bridge
Queen's Park
Queensbury
Queensway
Ravenscourt Park
Rayners Lane
Redbridge
Regent's Park
Richmond
Rickmansworth
Roding Valley
Royal Albert
Royal Oak
Royal Victoria
Ruislip
Ruislip Gardens
Ruislip Manor
Russell Square
St. James's Park
St. John's Wood
St. Paul's
Seven Sisters
Shadwell
Shepherd's Bush
Shepherd's Bush Market
Sloane Square
Snaresbrook
South Ealing
South Harrow
South Kensington
South Kenton
South Quay
South Ruislip
South Wimbledon
South Woodford
Southfields
Southgate
Southwark
Stamford Brook
Stanmore
Stepney Green
Stockwell
Stonebridge Park
Stratford
Sudbury Hill
Sudbury Town
Swiss Cottage
Temple
Theydon Bois
Tooting Bec
Tooting Broadway
Tottenham Court Road
Tottenham Hale
Totteridge & Whetstone
Tower Gateway
Tower Hill
Tufnell Park
Turnham Green
Turnpike Lane
Upminster
Upminster Bridge
Upney
Upton Park
Uxbridge
Vauxhall
Victoria
Walthamstow Central
Wanstead
Warren Street
Warwick Avenue
Waterloo
Watford
Wembley Central
Wembley Park
West Acton
West Brompton
West Finchley
West Ham
West Hampstead
West Harrow
West India Quay
West Kensington
West Ruislip
West Silvertown
Westbourne Park
Westferry
Westminster
White City
Whitechapel
Willesden Green
Willesden Junction
Wimbledon
Wimbledon Park
Wood Green
Wood Lane
Woodford
Woodside Park
Woolwich Arsenal";

  }
}
