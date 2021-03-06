using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public partial class RandomGenerator
    {
        internal static class Data
        {
            public const string Numbers = "0123456789";
            public const string CharsLower = "abcdefghijklmnopqrstuvwxyz";
            public static readonly string CharsUpper = CharsLower.ToUpperInvariant();
            public const string HexPool = Numbers + "abcdef";
            public static readonly CustomBasicList<(string Month, string Abb, string Code, int Day)> Months =
                new CustomBasicList<(string Month, string Abb, string Code, int Day)>
                {
                    ("January", "Jan", "01", 31),
                    ("February", "Feb", "02", 28),
                    ("March", "Mar", "03", 31),
                    ("April", "Apr", "04", 30),
                    ("May", "May", "05", 31),
                    ("June", "Jun", "06", 30),
                    ("July", "Jul", "07", 31),
                    ("August", "Aug", "08", 31),
                    ("September", "Sep", "09", 30),
                    ("October", "Oct", "10", 31),
                    ("November", "Nov", "11", 30),
                    ("December", "Dec", "12", 31)
                };

            public static readonly CustomBasicList<string> Tlds = new CustomBasicList<string>
            {
                "com",
                "org",
                "edu",
                "gov",
                "net",
            };

            public static readonly CustomBasicList<string> FirstNamesMale = new CustomBasicList<string>
            {
                "James",
                "John",
                "Robert",
                "Michael",
                "William",
                "David",
                "Richard",
                "Joseph",
                "Brenden",
                "Charles",
                "Thomas",
                "Christopher",
                "Daniel",
                "Matthew",
                "George",
                "Donald",
                "Anthony",
                "Paul",
                "Mark",
                "Edward",
                "Steven",
                "Kenneth",
                "Andy",
                "Andrew",
                "Brian",
                "Bob",
                "Joshua",
                "Kevin",
                "Ronald",
                "Timothy",
                "Jason",
                "Jeffrey",
                "Frank",
                "Gary",
                "Ryan",
                "Nicholas",
                "Eric",
                "Stephen",
                "Jacob",
                "Larry",
                "Jonathan",
                "Scott",
                "Raymond",
                "Justin",
                "Brandon",
                "Gregory",
                "Samuel",
                "Benjamin",
                "Ben",
                "Pat",
                "Patrick",
                "Jack",
                "Henry",
                "Walter",
                "Dennis",
                "Jerry",
                "Alexander",
                "Peter",
                "Tyler",
                "Douglas",
                "Doug",
                "Harold",
                "Aaron",
                "Jose",
                "Adam",
                "Arthur",
                "Zachary",
                "Carl",
                "Nathan",
                "Albert",
                "Kyle",
                "Lawrence",
                "Joe",
                "Willie",
                "Gerald",
                "Roger",
                "Keith",
                "Jeremy",
                "Terry",
                "Harry",
                "Ralph",
                "Sean",
                "Jesse",
                "Roy",
                "Louis",
                "Billy",
                "Austin",
                "Bruce",
                "Eugene",
                "Christian",
                "Bryan",
                "Wayne",
                "Russell",
                "Howard",
                "Fred",
                "Ethan",
                "Jordan",
                "Philip",
                "Alan",
                "Juan",
                "Randy",
                "Vincent",
                "Bobby",
                "Dylan",
                "Johnny",
                "Phillip",
                "Phil",
                "Victor",
                "Clarence",
                "Ernest",
                "Martin",
                "Craig",
                "Stanley",
                "Shawn",
                "Travis",
                "Bradley",
                "Leonard",
                "Earl",
                "Gabriel",
                "Jimmy",
                "Francis",
                "Todd",
                "Noah",
                "Danny",
                "Dale",
                "Cody",
                "Carlos",
                "Allen",
                "Frederick",
                "Logan",
                "Curtis",
                "Alex",
                "Joel",
                "Luis",
                "Norman",
                "Marvin",
                "Glenn",
                "Tony",
                "Nathaniel",
                "Rodney",
                "Melvin",
                "Alfred",
                "Steve",
                "Cameron",
                "Chad",
                "Edwin",
                "Caleb",
                "Evan",
                "Antonio",
                "Lee",
                "Herbert",
                "Jeffery",
                "Isaac",
                "Derek",
                "Ricky",
                "Marcus",
                "Theodore",
                "Elijah",
                "Luke",
                "Jesus",
                "Eddie",
                "Troy",
                "Mike",
                "Dustin",
                "Ray",
                "Adrian",
                "Bernard",
                "Leroy",
                "Angel",
                "Randall",
                "Wesley",
                "Ian",
                "Jared",
                "Mason",
                "Hunter",
                "Calvin",
                "Oscar",
                "Clifford",
                "Jay",
                "Shane",
                "Ronnie",
                "Barry",
                "Lucas",
                "Corey",
                "Manuel",
                "Leo",
                "Tommy",
                "Warren",
                "Jackson",
                "Isaiah",
                "Connor",
                "Don",
                "Dean",
                "Jon",
                "Julian",
                "Miguel",
                "Bill",
                "Lloyd",
                "Charlie",
                "Mitchell",
                "Leon",
                "Jerome",
                "Darrell",
                "Jeremiah",
                "Alvin",
                "Brett",
                "Seth",
                "Floyd",
                "Jim",
                "Blake",
                "Micheal",
                "Gordon",
                "Trevor",
                "Lewis",
                "Erik",
                "Edgar",
                "Vernon",
                "Devin",
                "Gavin",
                "Jayden",
                "Chris",
                "Clyde",
                "Tom",
                "Derrick",
                "Mario",
                "Brent",
                "Marc",
                "Herman",
                "Chase",
                "Dominic",
                "Ricardo",
                "Franklin",
                "Maurice",
                "Max",
                "Aiden",
                "Owen",
                "Lester",
                "Gilbert",
                "Elmer",
                "Gene",
                "Francisco",
                "Glen",
                "Cory",
                "Garrett",
                "Clayton",
                "Sam",
                "Jorge",
                "Chester",
                "Alejandro",
                "Jeff",
                "Harvey",
                "Milton",
                "Cole",
                "Ivan",
                "Andre",
                "Duane",
                "Hugh",
                "Landon"
            };

            public static readonly CustomBasicList<string> FirstNamesFemale = new CustomBasicList<string>
            {
                "Mary",
                "Emma",
                "Elizabeth",
                "Minnie",
                "Margaret",
                "Ida",
                "Alice",
                "Bertha",
                "Sarah",
                "Annie",
                "Clara",
                "Cristina",
                "Ella",
                "Florence",
                "Cora",
                "Martha",
                "Laura",
                "Nellie",
                "Grace",
                "Carrie",
                "Maude",
                "Mabel",
                "Bessie",
                "Jennie",
                "Gertrude",
                "Julia",
                "Judy",
                "Hattie",
                "Edith",
                "Mattie",
                "Rose",
                "Catherine",
                "Lillian",
                "Ada",
                "Lillie",
                "Helen",
                "Jessie",
                "Louise",
                "Ethel",
                "Lula",
                "Myrtle",
                "Eva",
                "Frances",
                "Lena",
                "Lucy",
                "Edna",
                "Maggie",
                "Pearl",
                "Daisy",
                "Fannie",
                "Josephine",
                "Dora",
                "Rosa",
                "Katherine",
                "Agnes",
                "Marie",
                "Nora",
                "May",
                "Mamie",
                "Blanche",
                "Stella",
                "Ellen",
                "Nancy",
                "Effie",
                "Sallie",
                "Nettie",
                "Della",
                "Lizzie",
                "Flora",
                "Susie",
                "Maud",
                "Mae",
                "Etta",
                "Harriet",
                "Sadie",
                "Caroline",
                "Katie",
                "Lydia",
                "Elsie",
                "Kate",
                "Susan",
                "Mollie",
                "Alma",
                "Addie",
                "Georgia",
                "Eliza",
                "Lulu",
                "Nannie",
                "Lottie",
                "Amanda",
                "Belle",
                "Charlotte",
                "Rebecca",
                "Ruth",
                "Viola",
                "Olive",
                "Amelia",
                "Hannah",
                "Jane",
                "Virginia",
                "Emily",
                "Matilda",
                "Irene",
                "Kathryn",
                "Esther",
                "Willie",
                "Henrietta",
                "Ollie",
                "Amy",
                "Rachel",
                "Sara",
                "Estella",
                "Theresa",
                "Augusta",
                "Ora",
                "Pauline",
                "Josie",
                "Lola",
                "Sophia",
                "Leona",
                "Anne",
                "Mildred",
                "Ann",
                "Beulah",
                "Callie",
                "Lou",
                "Delia",
                "Eleanor",
                "Barbara",
                "Iva",
                "Louisa",
                "Maria",
                "Mayme",
                "Evelyn",
                "Estelle",
                "Nina",
                "Betty",
                "Marion",
                "Bettie",
                "Dorothy",
                "Luella",
                "Inez",
                "Lela",
                "Rosie",
                "Allie",
                "Millie",
                "Janie",
                "Cornelia",
                "Victoria",
                "Ruby",
                "Winifred",
                "Alta",
                "Celia",
                "Christine",
                "Beatrice",
                "Birdie",
                "Harriett",
                "Mable",
                "Myra",
                "Sophie",
                "Tillie",
                "Isabel",
                "Sylvia",
                "Carolyn",
                "Isabelle",
                "Leila",
                "Sally",
                "Ina",
                "Essie",
                "Bertie",
                "Nell",
                "Alberta",
                "Katharine",
                "Lora",
                "Rena",
                "Mina",
                "Rhoda",
                "Mathilda",
                "Abbie",
                "Eula",
                "Dollie",
                "Hettie",
                "Eunice",
                "Fanny",
                "Ola",
                "Lenora",
                "Adelaide",
                "Christina",
                "Lelia",
                "Nelle",
                "Sue",
                "Johanna",
                "Lilly",
                "Lucinda",
                "Minerva",
                "Lettie",
                "Roxie",
                "Cynthia",
                "Helena",
                "Hilda",
                "Hulda",
                "Bernice",
                "Genevieve",
                "Jean",
                "Cordelia",
                "Marian",
                "Francis",
                "Jeanette",
                "Adeline",
                "Gussie",
                "Leah",
                "Lois",
                "Lura",
                "Mittie",
                "Hallie",
                "Isabella",
                "Olga",
                "Phoebe",
                "Teresa",
                "Hester",
                "Lida",
                "Lina",
                "Winnie",
                "Claudia",
                "Marguerite",
                "Vera",
                "Cecelia",
                "Bess",
                "Emilie",
                "John",
                "Rosetta",
                "Verna",
                "Myrtie",
                "Cecilia",
                "Elva",
                "Olivia",
                "Ophelia",
                "Georgie",
                "Elnora",
                "Violet",
                "Adele",
                "Lily",
                "Linnie",
                "Loretta",
                "Madge",
                "Polly",
                "Virgie",
                "Eugenia",
                "Lucile",
                "Lucille",
                "Mabelle",
                "Rosalie"
            };

            public static readonly CustomBasicList<string> LastNames = new CustomBasicList<string>
            {
                "Smith",
                "Johnson",
                "Ashby",
                "Williams",
                "Jones",
                "Brown",
                "Davis",
                "Miller",
                "Wilson",
                "Moore",
                "Taylor",
                "Anderson",
                "Thomas",
                "Jackson",
                "White",
                "Harris",
                "Martin",
                "Thompson",
                "Garcia",
                "Martinez",
                "Robinson",
                "Clark",
                "Rodriguez",
                "Lewis",
                "Lee",
                "Walker",
                "Hall",
                "Allen",
                "Young",
                "Hernandez",
                "King",
                "Wright",
                "Lopez",
                "Hill",
                "Scott",
                "Green",
                "Adams",
                "Baker",
                "Gonzalez",
                "Nelson",
                "Carter",
                "Mitchell",
                "Perez",
                "Roberts",
                "Turner",
                "Phillips",
                "Campbell",
                "Parker",
                "Evans",
                "Edwards",
                "Collins",
                "Stewart",
                "Sanchez",
                "Morris",
                "Rogers",
                "Reed",
                "Cook",
                "Morgan",
                "Bell",
                "Murphy",
                "Bailey",
                "Rivera",
                "Cooper",
                "Richardson",
                "Cox",
                "Howard",
                "Ward",
                "Torres",
                "Peterson",
                "Gray",
                "Ramirez",
                "James",
                "Watson",
                "Brooks",
                "Kelly",
                "Sanders",
                "Price",
                "Bennett",
                "Wood",
                "Barnes",
                "Ross",
                "Henderson",
                "Coleman",
                "Jenkins",
                "Perry",
                "Powell",
                "Long",
                "Patterson",
                "Hughes",
                "Flores",
                "Washington",
                "Butler",
                "Simmons",
                "Foster",
                "Gonzales",
                "Bryant",
                "Alexander",
                "Russell",
                "Griffin",
                "Diaz",
                "Hayes",
                "Myers",
                "Ford",
                "Hamilton",
                "Graham",
                "Sullivan",
                "Wallace",
                "Woods",
                "Cole",
                "West",
                "Jordan",
                "Owens",
                "Reynolds",
                "Fisher",
                "Ellis",
                "Harrison",
                "Gibson",
                "McDonald",
                "Cruz",
                "Marshall",
                "Ortiz",
                "Gomez",
                "Murray",
                "Freeman",
                "Wells",
                "Webb",
                "Simpson",
                "Stevens",
                "Tucker",
                "Porter",
                "Hunter",
                "Hicks",
                "Crawford",
                "Henry",
                "Boyd",
                "Mason",
                "Morales",
                "Kennedy",
                "Warren",
                "Dixon",
                "Ramos",
                "Reyes",
                "Burns",
                "Gordon",
                "Shaw",
                "Holmes",
                "Rice",
                "Robertson",
                "Hunt",
                "Black",
                "Daniels",
                "Palmer",
                "Mills",
                "Nichols",
                "Grant",
                "Knight",
                "Ferguson",
                "Rose",
                "Stone",
                "Hawkins",
                "Dunn",
                "Perkins",
                "Hudson",
                "Spencer",
                "Gardner",
                "Stephens",
                "Payne",
                "Pierce",
                "Berry",
                "Matthews",
                "Arnold",
                "Wagner",
                "Willis",
                "Ray",
                "Watkins",
                "Olson",
                "Carroll",
                "Duncan",
                "Snyder",
                "Hart",
                "Cunningham",
                "Bradley",
                "Lane",
                "Andrews",
                "Ruiz",
                "Harper",
                "Fox",
                "Riley",
                "Armstrong",
                "Carpenter",
                "Weaver",
                "Greene",
                "Lawrence",
                "Elliott",
                "Chavez",
                "Sims",
                "Austin",
                "Peters",
                "Kelley",
                "Franklin",
                "Lawson",
                "Fields",
                "Gutierrez",
                "Ryan",
                "Schmidt",
                "Carr",
                "Vasquez",
                "Castillo",
                "Wheeler",
                "Chapman",
                "Oliver",
                "Montgomery",
                "Richards",
                "Williamson",
                "Johnston",
                "Banks",
                "Meyer",
                "Bishop",
                "McCoy",
                "Howell",
                "Alvarez",
                "Morrison",
                "Hansen",
                "Fernandez",
                "Garza",
                "Harvey",
                "Little",
                "Burton",
                "Stanley",
                "Stauffer",
                "Nguyen",
                "George",
                "Jacobs",
                "Reid",
                "Kim",
                "Fuller",
                "Lynch",
                "Dean",
                "Gilbert",
                "Garrett",
                "Romero",
                "Welch",
                "Larson",
                "Frazier",
                "Burke",
                "Hanson",
                "Day",
                "Mendoza",
                "Moreno",
                "Bowman",
                "Medina",
                "Fowler",
                "Brewer",
                "Hoffman",
                "Carlson",
                "Silva",
                "Pearson",
                "Holland",
                "Douglas",
                "Fleming",
                "Jensen",
                "Vargas",
                "Byrd",
                "Davidson",
                "Hopkins",
                "May",
                "Terry",
                "Herrera",
                "Wade",
                "Soto",
                "Walters",
                "Curtis",
                "Neal",
                "Caldwell",
                "Lowe",
                "Jennings",
                "Barnett",
                "Graves",
                "Jimenez",
                "Horton",
                "Shelton",
                "Barrett",
                "Obrien",
                "Castro",
                "Sutton",
                "Gregory",
                "McKinney",
                "Lucas",
                "Miles",
                "Craig",
                "Rodriquez",
                "Chambers",
                "Holt",
                "Lambert",
                "Fletcher",
                "Watts",
                "Bates",
                "Hale",
                "Rhodes",
                "Pena",
                "Beck",
                "Newman",
                "Haynes",
                "McDaniel",
                "Mendez",
                "Bush",
                "Vaughn",
                "Parks",
                "Dawson",
                "Santiago",
                "Norris",
                "Hardy",
                "Love",
                "Steele",
                "Curry",
                "Powers",
                "Schultz",
                "Barker",
                "Guzman",
                "Page",
                "Munoz",
                "Ball",
                "Keller",
                "Chandler",
                "Weber",
                "Leonard",
                "Walsh",
                "Lyons",
                "Ramsey",
                "Wolfe",
                "Schneider",
                "Mullins",
                "Benson",
                "Sharp",
                "Bowen",
                "Daniel",
                "Barber",
                "Cummings",
                "Hines",
                "Baldwin",
                "Griffith",
                "Valdez",
                "Hubbard",
                "Salazar",
                "Reeves",
                "Warner",
                "Stevenson",
                "Burgess",
                "Santos",
                "Tate",
                "Cross",
                "Garner",
                "Mann",
                "Mack",
                "Moss",
                "Thornton",
                "Dennis",
                "McGee",
                "Farmer",
                "DeLong",
                "Delgado",
                "Aguilar",
                "Vega",
                "Glover",
                "Manning",
                "Cohen",
                "Harmon",
                "Rodgers",
                "Robbins",
                "Newton",
                "Todd",
                "Blair",
                "Higgins",
                "Ingram",
                "Reese",
                "Cannon",
                "Strickland",
                "Townsend",
                "Potter",
                "Goodwin",
                "Walton",
                "Rowe",
                "Hampton",
                "Ortega",
                "Patton",
                "Swanson",
                "Joseph",
                "Francis",
                "Goodman",
                "Maldonado",
                "Yates",
                "Becker",
                "Erickson",
                "Hodges",
                "Rios",
                "Conner",
                "Adkins",
                "Webster",
                "Norman",
                "Malone",
                "Hammond",
                "Flowers",
                "Cobb",
                "Moody",
                "Quinn",
                "Blake",
                "Maxwell",
                "Pope",
                "Floyd",
                "Osborne",
                "Paul",
                "McCarthy",
                "Guerrero",
                "Lindsey",
                "Estrada",
                "Sandoval",
                "Gibbs",
                "Tyler",
                "Gross",
                "Fitzgerald",
                "Stokes",
                "Doyle",
                "Sherman",
                "Saunders",
                "Wise",
                "Colon",
                "Gill",
                "Alvarado",
                "Greer",
                "Padilla",
                "Simon",
                "Waters",
                "Nunez",
                "Ballard",
                "Schwartz",
                "McBride",
                "Houston",
                "Christensen",
                "Klein",
                "Pratt",
                "Briggs",
                "Parsons",
                "McLaughlin",
                "Zimmerman",
                "French",
                "Buchanan",
                "Moran",
                "Copeland",
                "Roy",
                "Pittman",
                "Brady",
                "McCormick",
                "Holloway",
                "Brock",
                "Poole",
                "Frank",
                "Logan",
                "Owen",
                "Bass",
                "Marsh",
                "Drake",
                "Wong",
                "Jefferson",
                "Park",
                "Morton",
                "Abbott",
                "Sparks",
                "Patrick",
                "Norton",
                "Huff",
                "Clayton",
                "Massey",
                "Lloyd",
                "Figueroa",
                "Carson",
                "Bowers",
                "Roberson",
                "Barton",
                "Tran",
                "Lamb",
                "Harrington",
                "Casey",
                "Boone",
                "Cortez",
                "Clarke",
                "Mathis",
                "Singleton",
                "Wilkins",
                "Cain",
                "Bryan",
                "Underwood",
                "Hogan",
                "McKenzie",
                "Collier",
                "Luna",
                "Phelps",
                "McGuire",
                "Allison",
                "Bridges",
                "Wilkerson",
                "Nash",
                "Summers",
                "Atkins"
            };
            //were not going to worry about suffixes

            public static readonly CustomBasicList<string> ColorNames = new CustomBasicList<string>
            {
                "AliceBlue",
                "Black",
                "Navy",
                "DarkBlue",
                "MediumBlue",
                "Blue",
                "DarkGreen",
                "Green",
                "Teal",
                "DarkCyan",
                "DeepSkyBlue",
                "DarkTurquoise",
                "MediumSpringGreen",
                "Lime",
                "SpringGreen",
                "Aqua",
                "Cyan",
                "MidnightBlue",
                "DodgerBlue",
                "LightSeaGreen",
                "ForestGreen",
                "SeaGreen",
                "DarkSlateGray",
                "LimeGreen",
                "MediumSeaGreen",
                "Turquoise",
                "RoyalBlue",
                "SteelBlue",
                "DarkSlateBlue",
                "MediumTurquoise",
                "Indigo",
                "DarkOliveGreen",
                "CadetBlue",
                "CornflowerBlue",
                "RebeccaPurple",
                "MediumAquaMarine",
                "DimGray",
                "SlateBlue",
                "OliveDrab",
                "SlateGray",
                "LightSlateGray",
                "MediumSlateBlue",
                "LawnGreen",
                "Chartreuse",
                "Aquamarine",
                "Maroon",
                "Purple",
                "Olive",
                "Gray",
                "SkyBlue",
                "LightSkyBlue",
                "BlueViolet",
                "DarkRed",
                "DarkMagenta",
                "SaddleBrown",
                "Ivory",
                "White",
                "DarkSeaGreen",
                "LightGreen",
                "MediumPurple",
                "DarkViolet",
                "PaleGreen",
                "DarkOrchid",
                "YellowGreen",
                "Sienna",
                "Brown",
                "DarkGray",
                "LightBlue",
                "GreenYellow",
                "PaleTurquoise",
                "LightSteelBlue",
                "PowderBlue",
                "FireBrick",
                "DarkGoldenRod",
                "MediumOrchid",
                "RosyBrown",
                "DarkKhaki",
                "Silver",
                "MediumVioletRed",
                "IndianRed",
                "Peru",
                "Chocolate",
                "Tan",
                "LightGray",
                "Thistle",
                "Orchid",
                "GoldenRod",
                "PaleVioletRed",
                "Crimson",
                "Gainsboro",
                "Plum",
                "BurlyWood",
                "LightCyan",
                "Lavender",
                "DarkSalmon",
                "Violet",
                "PaleGoldenRod",
                "LightCoral",
                "Khaki",
                "AliceBlue",
                "HoneyDew",
                "Azure",
                "SandyBrown",
                "Wheat",
                "Beige",
                "WhiteSmoke",
                "MintCream",
                "GhostWhite",
                "Salmon",
                "AntiqueWhite",
                "Linen",
                "LightGoldenRodYellow",
                "OldLace",
                "Red",
                "Fuchsia",
                "Magenta",
                "DeepPink",
                "OrangeRed",
                "Tomato",
                "HotPink",
                "Coral",
                "DarkOrange",
                "LightSalmon",
                "Orange",
                "LightPink",
                "Pink",
                "Gold",
                "PeachPuff",
                "NavajoWhite",
                "Moccasin",
                "Bisque",
                "MistyRose",
                "BlanchedAlmond",
                "PapayaWhip",
                "LavenderBlush",
                "SeaShell",
                "Cornsilk",
                "LemonChiffon",
                "FloralWhite",
                "Snow",
                "Yellow",
                "LightYellow"
            };

            public static readonly CustomBasicList<(string Name, string Abb)> StreetSuffixes = new CustomBasicList
                <(string Name, string Abb)>
            {
                ("Avenue", "Ave"),
                ("Boulevard", "Blvd"),
                ("Center", "Ctr"),
                ("Circle", "Cir"),
                ("Court", "Ct"),
                ("Drive", "Dr"),
                ("Extension", "Ext"),
                ("Glen", "Gln"),
                ("Grove", "Grv"),
                ("Heights", "Hts"),
                ("Highway", "Hwy"),
                ("Junction", "Jct"),
                ("Key", "Key"),
                ("Lane", "Ln"),
                ("Loop", "Loop"),
                ("Manor", "Mnr"),
                ("Mill", "Mill"),
                ("Park", "Park"),
                ("Parkway", "Pkwy"),
                ("Pass", "Pass"),
                ("Path", "Path"),
                ("Pike", "Pike"),
                ("Place", "Pl"),
                ("Plaza", "Plz"),
                ("Point", "Pt"),
                ("Ridge", "Rdg"),
                ("River", "Riv"),
                ("Road", "Rd"),
                ("Square", "Sq"),
                ("Street", "St"),
                ("Terrace", "Ter"),
                ("Trail", "Trl"),
                ("Turnpike", "Tpke"),
                ("View", "Vw"),
                ("Way", "Way")
            };

            public static readonly CustomBasicList<(string Name, string Abb)> USStates = new CustomBasicList
                <(string Name, string Abb)>
            {
                ("Alabama", "AL"),
                ("Alaska", "AK"),
                ("Arizona", "AZ"),
                ("Arkansas", "AR"),
                ("California", "CA"),
                ("Colorado", "CO"),
                ("Connecticut", "CT"),
                ("Delaware", "DE"),
                ("District of Columbia", "DC"),
                ("Florida", "FL"),
                ("Georgia", "GA"),
                ("Hawaii", "HI"),
                ("Idaho", "ID"),
                ("Illinois", "IL"),
                ("Indiana", "IN"),
                ("Iowa", "IA"),
                ("Kansas", "KS"),
                ("Kentucky", "KY"),
                ("Louisiana", "LA"),
                ("Maine", "ME"),
                ("Maryland", "MD"),
                ("Massachusetts", "MA"),
                ("Michigan", "MI"),
                ("Minnesota", "MN"),
                ("Mississippi", "MS"),
                ("Missouri", "MO"),
                ("Montana", "MT"),
                ("Nebraska", "NE"),
                ("Nevada", "NV"),
                ("New Hampshire", "NH"),
                ("New Jersey", "NJ"),
                ("New Mexico", "NM"),
                ("New York", "NY"),
                ("North Carolina", "NC"),
                ("North Dakota", "ND"),
                ("Ohio", "OH"),
                ("Oklahoma", "OK"),
                ("Oregon", "OR"),
                ("Pennsylvania", "PA"),
                ("Rhode Island", "RI"),
                ("South Carolina", "SC"),
                ("South Dakota", "SD"),
                ("Tennessee", "TN"),
                ("Texas", "TX"),
                ("Utah", "UT"),
                ("Vermont", "VT"),
                ("Virginia", "VA"),
                ("Washington", "WA"),
                ("West Virginia", "WV"),
                ("Wisconsin", "WI"),
                ("Wyoming", "WY")
            };

            public static readonly CustomBasicList<(string Company, string Abb, string Code, int Digits)> CcTypes = new CustomBasicList
                <(string Company, string Abb, string Code, int Digits)>
            {
                ("American Express", "amex", "37", 15),
                ("Discover Card", "discover", "6011", 16),
                ("Mastercard", "mc", "51", 16),
                ("Visa", "visa", "4", 16),
            };
        }
    }
}