# Practice environment
This project has been setup to practice with more advanced c#/.NET topics such as multi-threaded applications. It includes a demo-api and various examples of multi-threading approaches

## Topics
- Locks/Mutex ([Demo/Program.cs](./Demo/Program.cs))
- Interleaved writes ([Demo/InterleavedWrites](./Demo/InterleavedWrites.cs))
- Async/await ([Demo/WeatherClient](./Demo/WeatherClient.cs))

## Setup
- Run `Demo/Program.cs`

## Example demonstration
```c#
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
```