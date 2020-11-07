using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Demo
{
    class Program
    {
     
        static void Main(string[] args)
        {
            new Program().Demo();
        }

        private static Random _rnd = new Random();
        private static List<BookReader> Readers { get; set; }
        
        public void Demo()
        {
            var tradingCount = 100;
            var tradingsForExperiment = new Thread[tradingCount];

            #region setup book readers
            var initialBooks = new List<Book>() { new Book { Name = "Book 1" }, new Book { Name = "Book 2" }, new Book  { Name = "Book 1" } };
            Readers = new BookReader[10].Select((r, i) => new BookReader(i, initialBooks)).ToList();
            #endregion
            
            for (var i = 0; i < tradingsForExperiment.Length; i++)
            {
                tradingsForExperiment[i] = new Thread(TradeBooks);
                tradingsForExperiment[i].Start();   
            }
            
            for (var i = 0; i < tradingsForExperiment.Length; i++)
            {
                tradingsForExperiment[i].Join();
            }
        }

        public static void TradeBooks()
        {
            #region pick random readers
            var firstReaderIndex = _rnd.Next(0, Readers.Count - 1);
            var secondReaderIndex = _rnd.Next(0, Readers.Count - 1);
            
            while (secondReaderIndex == firstReaderIndex)
            {
                secondReaderIndex = _rnd.Next(0, Readers.Count - 1);
            }
            
            #endregion
            
            Readers[firstReaderIndex].ExchangeBook(Readers[secondReaderIndex]);
        }
    }

    class BookReader
    {
        public Mutex _lock = new Mutex();
        public readonly int Id;
        private List<Book> _books = new List<Book>();

        public BookReader(int id, List<Book> initialBooks)
        {
            Id = id;
            _books = initialBooks;
        }
        
        public void ReceiveBook(Book book)
        {
            try
            {
                if (_lock.WaitOne())
                {
                    _books.Add(book);
                }
            }
            finally
            {
                _lock.ReleaseMutex();
            }
        }
        
        public void TakeBook(Book book)
        {
            try
            {
                if (_lock.WaitOne())
                {
                    _books.Remove(book);
                }
            }
            finally
            {
                _lock.ReleaseMutex();
            }
        }
        public Book GetBook()
        {
            try{
                if (_lock.WaitOne())
                {
                    return _books.First();
                }
            }
            finally
            {
                _lock.ReleaseMutex();
            }
            
            return null;
        }

        public int GetBookCount()
        {
            return _books.Count;
        }
        
        public void ExchangeBook(BookReader otherReader)
        {
            Mutex[] locks = new Mutex[] {_lock, otherReader._lock};

            if (WaitHandle.WaitAll(locks))
            {
                var book = otherReader.GetBook();

                if (book != null)
                {
                    ReceiveBook(book);
                    otherReader.TakeBook(book);  
                        
                    Console.WriteLine("Reader {0} -> {1} gave book {2}", otherReader.Id, Id, book.Name);
                }
            }

            foreach (var lockedMutex in locks)
            {
                lockedMutex.ReleaseMutex();
            }
        }
    }
}