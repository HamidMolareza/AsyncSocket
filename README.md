# Asynchronous Client Socket

The following example is a **client** program that connects to a server.
The client is an **asynchronous** socket,
so execution of the client is not suspended while the server returns a response.
The program sends a string to the server and then displays string returned by the server.

## Reference
This source is based on the Microsoft site example. Microsoft example is written based on the old IAsyncResult pattern.
But this source is rewritten according to the **Task-based Asynchronous Pattern**. (TAP)

## Two Templates
I use two templates source for rewrite old pattern to Task-based Asynchronous Pattern.

**Template1:** use **Begin*** and **End*** methods to change Pattern. **(Recommended)**

**Template2:** use base socket methods to change Pattern (like Send, Receive, ...) and use Task.Run.

## See Also
[Microsoft Example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example)

[Task-based Asynchronous Pattern](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)

[Interop with Other Asynchronous Patterns and Types](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/interop-with-other-asynchronous-patterns-and-types)

### TODO
- [x] Config Github
- [x] Add licence
- [x] Rewrite testable methods
- [ ] Add server for testing
- [ ] Unit Test
- [ ] Add files structure in readme
- [ ] Add versions
