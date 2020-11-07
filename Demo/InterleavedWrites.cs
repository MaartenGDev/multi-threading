using System;
using System.Threading;

namespace Demo
{
    /*
   * The following code demonstrates reading the same book with multiple readers(threads). This will showcase that values are sometimes cached.
   * This caching behaviour can cause problems in multi-threaded applications. This can be proven by running the Program and seeing that
   * the end result of the print lines for "Finished reading, has read x pages does not add up to the original number.
   *
   * Example:
   *
   * Finished reading, Reader #0 has read 1057 pages!
   * Finished reading, Reader #1 has read 1057 pages!
   * Finished reading, Reader #2 has read 1057 pages!
   * Finished reading, Reader #6 has read 1057 pages!
   * Finished reading, Reader #5 has read 1057 pages!
   * Finished reading, Reader #8 has read 1057 pages!
   * Finished reading, Reader #9 has read 1058 pages!
   * Finished reading, Reader #7 has read 1058 pages!
   * Finished reading, Reader #3 has read 1058 pages!
   * Finished reading, Reader #4 has read 1058 pages!
   *
   * And this totals for: 10.574, which is more then the original TotalPages.
   *
   * The fix for this problem would be adding Interlocked.Decrement to prevent value caching and preventing interleaving read/writes
   */
    public class InterleavedWrites
    {        
        private const int TotalWorkers = 10;
        public const int TotalPages = 10000;
        
        public void Run()
        {
            var book = new ReadableBook(TotalPages);

            for (var i = 0; i < TotalWorkers; i++)
            {
                var reader = new SharedBookReader($"Reader #{i}", book);
                new Thread(reader.Read).Start();
            }

            var reporter = new StatusReporter(book);
            new Thread(reporter.ReportStatus).Start();
        }
    }
    
    
    public class ReadableBook
    {
        // adding volatile results in ~10500 because it still allows interleaving write/read
        // this only makes sure that all CPU's see the same result
        private int _pagesLeftToRead; 
        

        public ReadableBook(int totalPages)
        {
            _pagesLeftToRead = totalPages;
        }

        public void ReadPage()
        {
            _pagesLeftToRead -= 1; // not thread safe
            // Interlocked.Decrement(ref _pagesLeftToRead); -> results in exactly TotalPages constant value(10k)
        }

        public int GetPagesLeftToRead()
        {
            return _pagesLeftToRead;
        }
    }
    
    public class SharedBookReader
    {
        private readonly string _readerName;
        private readonly ReadableBook _book;
        private int _readPages = 0;

        public SharedBookReader(string readerName, ReadableBook book)
        {
            _readerName = readerName;
            _book = book;
        }
        public void Read()
        {
            while(_book.GetPagesLeftToRead() > 0)
            {
                _book.ReadPage();
                _readPages += 1;
                Thread.Sleep(20);
            }
            
            Console.WriteLine($"Finished reading, {_readerName} has read {_readPages} pages!");
        }
    }
    
    public class StatusReporter
    {
        private readonly ReadableBook _book;

        public StatusReporter(ReadableBook book)
        {
            _book = book;
        }
        
        public void ReportStatus()
        {
            while (_book.GetPagesLeftToRead() > 0)
            {
                Console.WriteLine($"There are {_book.GetPagesLeftToRead()} pages left to read of the {InterleavedWrites.TotalPages} pages.");

                Thread.Sleep(200);
            }
        }
    }
}