# EzWebServerV2

EzWebServerV2 is a lightweight C# web server inspired by Express.js.  
It’s a complete rewrite of my first version, with improved stability, cleaner syntax, and more features.
> ***NOTICE:*** *The first version was never published anywhere.*
- - -
**How to set up?**
```csharp
using EzWebServerV2;

public class Program
{
    static void Main()
    {
        Server server = new Server("http://localhost:3000/"); //creates server on given IP

        server.Get("/", (req, res) => //routes to "/" when a request is given
        {
            res.SendFile("./index.html"); //sends a file
        });

        server.Start(); //starts the server
    }
}
```
- - -
EzWebServerV2 supports both GET and POST requests, with a simple and expressive routing API.

## server.Start();  
`server.Start()` - Starts the given server  
`server.Start(true)` - Starts the server in a new thread

## server.Get/Post
There are 2 ways to set it up, and that is what kind of request you need:  
`server.Get("/", (req, res) =>`  
OR  
`server.Post("/", (req, res) =>`  
Get creates a get-request, while post handles post-requests.
## REQ
Req handles the "request" data, which includes Query-strings (`/helloWorld?query=hello`)  
There are a few commands, which are useful:  
`req.Body(<key>)` - Gets something specific from a POST-Request.  
`req.Param(<key>)` - Gets a dynamic parameter from the URL (e.g., in `/user/:id`)  
`req.Json(<key>)` - When the post-request is JSON (will return objects instead of strings with Body)  
`req.Session(<key>)` - Gets a session when it exists with given name  
`req.Session(<key>, <value>)` - Sets a given session

## RES
The `RES` is known as `Response`, which is supposed to send data to client, such as: Text, JSON, files.  
**COMMANDS:**  
`res.Status(<int>)` - Sets the status response with an HTTP response code. (default = 200)  
`res.Text(<string>)` - Sends string-based content to the user  
`res.Html(<string>)` - Sends plain HTML to the user  
`res.Json(new { <JSON> })` - Sends JSON-data to the user  
`res.SendFile(<string>)` - Path to the file the user will recieve  
> ***Notice:*** *You can have `res.Status(200).SendFile("./index.html");` as a structure.*

## Routes
Routes are certain strings the server listens for connections for, like `GET` and `POST` requests.  
Here are some examples:  
### GET-REQUEST
```csharp
server.Get("/", (req, res) => //Handles GET requests to "/"
{
    res.Text("This is a GET-Request!");
});
```
### POST-REQUEST
```csharp
server.Post("/", (req, res) => //Handles POST requests for "/"
{
    res.Text("This is a POST-Request!");
});
```
A POST-Request has a `req.Body` where post-data is sent to.  
A GET-Request has everything a POST-Request has, except the `Req.Body`.  
Both have access to Sessions.

## Features
- Express.js-style routing
- Built-in session handling
- JSON & file responses
- Threaded server mode
- Clean and minimal C# API

## Still in development
This project is still evolving. Feel free to open issues or contribute on GitHub!
