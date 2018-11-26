# Asynchronous Client Socket Example

The following example program creates a **client** that connects to a server.
The client is built with an **asynchronous** socket,
so execution of the client application is not suspended while the server returns a response.
The application sends a string to the server and then displays the string returned by the server on the console.

# Reference
This source is based on the Microsoft site example. Microsoft example is written based on the old IAsyncResult pattern.
But this source is rewritten according to the **Task-based Asynchronous Pattern**. (TAP)

# See Also
[Microsoft Example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example)

[Task-based Asynchronous Pattern](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)

[Interop with Other Asynchronous Patterns and Types](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/interop-with-other-asynchronous-patterns-and-types)
