using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ConceptArchitect.Utils;
using ConceptArchitect.BookManagement;
//using ConceptArchitect.BookManagement;



namespace ConceptArchitect.BookManagement
{
    public class BookManagerRecordCreator
    {
        public  IBookManager BookManager { get; set; }
        public  IAuthorManager AuthorManager { get; set; }

        public BookManagerRecordCreator(IBookManager bookManager,IAuthorManager authorManager)
        {
            BookManager = bookManager;
            AuthorManager = authorManager;
        }
 
        public  void AddWellknownAuthors()
        {
            List<Author> authors = new List<Author>()
            {
                new Author(){ Name="Alexandre Dumas", BirthDate=new DateTime(1802,07,24), DeathDate=new DateTime(1870,10,05), Biography="One of the all time greatest story teller"},
                new Author(){ Name="Jeffrey Archer", BirthDate=new DateTime(1940,04,15), DeathDate=null, Biography="Contemporary Author of largest number of best seller",Email="jarcher@booksweb.org"},
                new Author(){ Name="Charles Dickens", BirthDate=new DateTime(1812,02,07), DeathDate=new DateTime(1870,06,09), Biography="One of most famous classic author"},
                new Author(){ Name="John Grisham", BirthDate=new DateTime(1955,2,8), DeathDate=null, Biography="Leading author of Legal Fiction",Email="john@grisham.net"},
                new Author(){ Name="JK Rowlings", BirthDate=new DateTime(1965,7,31), DeathDate=null, Biography="Author of Best selling Harry Potter Series",Email="jkr@hogwards.net"},
                new Author(){ Name="Munshi Premchand", BirthDate=new DateTime(1880,7,31), DeathDate=new DateTime(1936,10,08), Biography="Known as magician with Pen"},
                new Author(){ Name="Ramdhari Singh Dinkar", BirthDate=new DateTime(1908,9,23), DeathDate=new DateTime(1974,4,24), Biography="National Poet of India"},
                new Author(){ Name="Conan Doyle", BirthDate=new DateTime(1859,5,22), DeathDate=new DateTime(1930,7,7), Biography="Author of the greatest detective Sherlock Holmes"}
            };
            var basePath = "/photos/authors/";
            foreach (var author in authors)
            {
                if (string.IsNullOrEmpty(author.Photograph))
                    author.Photograph = IdTool.Normalize(author.Name)+".jpg";
                if (!author.Photograph.Contains("/"))
                    author.Photograph = basePath + author.Photograph;

                AuthorManager.AddAuthor(author);
            }
            
        }

        public  void AddWellKnownBooks()
        {
            List<Book> books = new List<Book>()
            {
                CreateBook("The Count of Monte Cristo","Alexandre Dumas","One of the all time greatest classic", "history,fiction,adventure,love,revenge",300),
                CreateBook("Sons of Fortune","Jeffrey Archer","Story of two brothers separated at Birth", "chronology,politics",230),
                CreateBook("A Study In Scarlet","Conan Doyle","First Novel of Sherlock Holmes", "Sherlock Holmes,crime,detectie",125),
                CreateBook("Kane and Able","Jeffrey Archer","Signature book of Jeffrey Archer", "chronology,best seller",325),
                CreateBook("The Appeal","John Grisham","Father summons sons and dies before they arrive", "suspense,thriller",275),
                CreateBook("Only Time Will Tell","Jeffrey Archer","First of the 5 part clifton chronicle", "chronical,best seller",190),
                CreateBook("Return of Sherlock Holmes","Conan Doyle","Collection of Sherlock Holmes short stories", "sherlock holmes, detective,crime",300),
                CreateBook("Harry Potter and Deathly Hollows","JK Rowlings","The final book of Harry Potter Series", "harry potter,wizard,best seller",300)
            };
            var basePath = "/photos/book-covers/";
            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(book.CoverPage))
                    book.CoverPage = IdTool.Normalize(book.Title) + ".jpg";
                if (!book.CoverPage.Contains("/"))
                    book.CoverPage = basePath + book.CoverPage;

                BookManager.AddBook(book);
            }
            BookManager.Save();
        }

        Book CreateBook(string title, string author, string description, string tags, int price)
        {
            var book = new Book()
            {
                Title = title,
                Description = description,
                Tags = tags,
                Price = price

            };
            //Author a = BookManager.GetAuthorByName(author);
            //Author a = AuthorManager.SearchAuthors(author).FirstOrDefault();
            Author a = AuthorManager.GetAuthorById(IdTool.Normalize(author));
            if (a == null)
            {
                throw new Exception(author + " not found");
            }
            book.Author = a;

            return book;
        }

        public  void AddRandomAuthor(int count)
        {
            string[] fNames = { "Ketan", "Suresh", "Chetan", "Monish" ,"Ramdhari","John","Yenn","Amol","Ved","Bimal","Shiva","Vishnu","Alvin","Vinita","Kavita","Yana", "Shivani", "Smita" };
            string[] mNames = { "", "", "Narayan", "Kumar", "", "Soni", "kant", "dutta", "", "", "", "shiv", "", "", "" };
            string[] lNames = { "Mehta", "Panth", "Bhagat", "Sahni", "Thakur" ,"Mishra","Pathak","Palak","Chowdhary","Thomas","Pillai","Tripathi","Singh",
                              "Aryan","Gomes","Kher","Kumar"};
            string[] prefix = { "welknown", "one of the best", "aclaimed", "popular", "controversial","best seller" };
            string[] suffix ={"classic","suspense","thriller","detective","fiction","non-fiction",
                                    "history","legal","medical","motivation","finance","fantasy" };
            int j = 0;
            DateTime now = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                var fName = I(fNames);
                var gender = "men";
                if (IndexOf(fNames, fName) > 12)
                    gender = "women";



                string name = string.Format("{0} {2} {1}", fName, I(lNames),I(mNames));

                

                //if (AuthorManager.SearchAuthors(name).FirstOrDefault() != null)
                //if(AuthorManager.GetAuthorByName(name)!=null)
                if(AuthorManager.GetAuthorById(IdTool.Normalize(name))!=null)
                {
                    i--;    //try again
                    j++;    // retry count
                    if (j == 5) // after 5 retry skip
                    {
                        i++;
                        j = 0;
                    }
                    continue;
                }

                string biography = string.Format("{0} author of {1}", I(prefix), I(suffix));
                int baseYear = r.Next(1, 100) < 30 ? 1800 : 1920;

                int yy=r.Next(baseYear, 1990);
                int mm=r.Next(1,13);
                int dd=r.Next(1,29);
                DateTime dob = new DateTime(yy,mm,dd);

                DateTime? dod = null;

                

                int ageOnDate =(DateTime.Now-dob).Days/365 ;
                bool isAlive=false;

                if (ageOnDate < 90)
                {
                    //int chance = 130 - ageOnDate;
                    isAlive = r.Next(100) <= 60;

                }
                
                if(!isAlive)
                {
                    if(ageOnDate>100)
                        ageOnDate=100;

                    int min = 30;
                    if (ageOnDate < 30)
                        min = 10;

                    dod = dob.AddYears(r.Next(min, ageOnDate));
                }


                Author author = new Author()
                {
                    Name = name,
                    Biography = biography,
                    BirthDate = dob,
                    DeathDate = dod
                    //Picture = name.Replace(" ", "_") + ".jpg"
                    , Photograph = string.Format("https://randomuser.me/api/portraits/{0}/{1}.jpg", gender, r.Next(100))
                };

                if (author.DeathDate==null && r.Next(100)>10)
                {
                    author.Email = RandomEmail(author.Name.Split(' '));
                }

                AuthorManager.AddAuthor(author);
                

                

            }

           // AuthorManager.Save();



        }

        private int IndexOf(string[] fNames, string fName)
        {
            for (int i = 0; i < fNames.Length; i++)
                if (fNames[i] == fName)
                    return i;

            return -1;
        }

        public  void AddRandomBook(int count)
        {
            IBookManager bm = BookManager;      //CatalogManager.Single<IBookManager>();
            //var authors = AuthorManager.ListAllAuthors().ToArray();
            //var authors = AuthorManager.ListAllAuthors().ToArray();
            var authors = AuthorManager.GetAllAuthors().ToArray();
            Random random = new Random();

            string[] part1 = { "The", "A", "One", "Let", "Seven", "Around", "Night", "Dark", "the", "a", "the", "a", "one", "the", "a" };
            string[] part2 = { "Mystry", "Dark", "Night", "Sun", "Case", "Story", "Love", "Horror" };
            string[] part3 = { "of", "within", "", "cloud", "in", "at", "in", "at", "of", "a", "of", "at", "in" };
            string[] part4 = { "cloud", "style", "agent", "hotel", "india", "phantom", "story", "series", "system" };

            string[] fNames = { "Ketan", "Suresh", "Chetan", "Monish", "Smita" };
            string[] lNames = { "Mehta", "Panth", "Bhagat", "Sahni", "Thakur" };

            string[] tags = { "suspense", "crime", "love", "history", "action", "detective", "nonfiction", "chronology", "classic","legal","medical" };

            string[] descriptions = { "just superb", "must read", "spell bound", "time pass", "horrible", "read if you have nothing to do" };
            for (int i = 0; i < count; i++)
            {
                string title = string.Format("{0} {1} {2} {3}", I(part1), I(part2), I(part3), I(part4));
                
                

                Book b = new Book()
                {
                    Title = title,
                    Author = I(authors),
                    Price = random.Next(20, 600),
                    Tags = string.Format("{0},{1},{2}", I(tags), I(tags), I(tags)),
                    Description = I(descriptions)
                };


                bm.AddBook(b);


            }

            bm.Save();
        }

        Random r = new Random();

        T I<T>(T[] value)
        {

            //if (r.Next(1, 100) > 60)
            //    return default(T);
            //else
            return value[r.Next(value.Length)];

        }

        string RandomEmail(params string[] nameParts)
        {
            string[] domains = { "hotmail.com", "yahoo.com", "gmail.com", "yahoo.co.in", "conceptarchitect.in", "indiatimes.com", "rediff.com" };
            return Email(I(domains), nameParts);
        }


        string Email(string domain, params string [] nameParts)
        {
            string n = "";
            foreach (var name in nameParts)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(n))
                        n += ".";
                    n +=  name;
                }
            }

            return n + "@" + domain;
        }

        public  void BuildNewDB()
        {
            AddWellknownAuthors();
            AddWellKnownBooks();
            //AddRandomAuthor(20);
            //AddRandomBook(50);
        }

        public  void RefillDB()
        {
            AddRandomAuthor(20);
            AddRandomBook(50);
            // AddRandomUsers(20);
           // AddRandomReviews(20);

        }

        


    }
}
