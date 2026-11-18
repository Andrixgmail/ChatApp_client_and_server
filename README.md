# ChatApp client and server

A small multi user chat app I built with Windows Forms and plain TCP sockets. One window runs the server, and as many client windows as I like can connect to it, pick a name and talk to each other in real time.

## What is in here

| Folder | What it is |
| --- | --- |
| `Chatting_App/TCPIPdemo2` | The server app (`ChatServer.exe`). It listens for clients, keeps track of who is online and relays every message to everyone. |
| `Chatting_App/Client` | The client app (`ChatClient.exe`). I enter a name, the server address and the port, then chat. |
| `Chatting_App/Shared` | A small library both apps use. It holds the packet format and the code that sends and receives packets, so the two sides always agree. |

## Requirements

* Windows 10 or Windows 11
* The .NET 10 SDK
* Visual Studio 2026 (or Visual Studio 2022 17.14 with .NET 10 support), or just the `dotnet` command line

## Running it

1. Open `Chatting_App/Chatting_App.sln`.
2. Start the **Server** project, leave the address as `0.0.0.0` and the port as `8910`, then press **Start**. The activity log shows the addresses other computers can use.
3. Start one or more copies of the **Client** project, type a name, use `127.0.0.1` on the same PC (or the address the server printed), and press **Connect**.
4. Type a message and press **Enter**. **Shift+Enter** adds a new line.

From the command line:

```
cd Chatting_App
dotnet run --project TCPIPdemo2
dotnet run --project Client
```

The first time the server starts, Windows Firewall may ask whether to allow it. I allow it on private networks so clients on other machines can reach it.

## How the protocol works

Every packet is one line of JSON ending in a newline, for example:

```json
{"type":"Message","sender":"Andrea","text":"Hello!","timestamp":"2026-11-18T17:25:00+00:00","users":null}
```

1. The client connects and sends a `Join` packet with the name it wants.
2. The server replies with `Welcome` and the final name. If the name is already taken it adds a number, so a second Andrea becomes "Andrea 2".
3. The server broadcasts `Join`, `Leave` and `UserList` packets as people come and go.
4. Chat messages are `Message` packets. The server always stamps them with the sender's real name and its own time, so nobody can pretend to be someone else.
5. `Notice` packets are general messages from the server, such as the shutdown warning.

## What changed in the modernised version

* Upgraded from .NET 8 (out of support since November 2026) to .NET 10 LTS.
* Replaced the old SimpleTCP package with the built in `TcpListener` and `TcpClient` using async and await, so the window never freezes while waiting on the network.
* Fixed the original message framing bug: the server split messages on byte `0x13` while the client ended them with a newline, so messages could arrive merged or not at all.
* The server now relays messages to every connected client, which turns it into a real group chat instead of an echo server.
* Added display names, a live list of online users, join and leave notices and timestamps.
* Wired up the **Stop** button, which was never connected before, and the server now shuts down cleanly when the window closes.
* Added input checks for the address, port and name, a five second connect timeout and friendly messages when something goes wrong.
* Added a dark theme, resizable windows, high DPI support and Enter to send.
* The client remembers the last name, host and port in `%LocalAppData%\ChatApp\client-settings.json`.
