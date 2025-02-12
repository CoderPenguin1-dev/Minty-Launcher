using DiscordRPC.Logging;
using DiscordRPC;
using System.Diagnostics;
using System.Reflection;
using Doom_Loader.Properties;
using System.Resources;
using Doom_Loader;

/*
Code adapted from https://github.com/Lachee/discord-rpc-csharp

MIT License

Copyright (c) 2021 Lachee

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

public static class RPCClient
{
    public static DiscordRpcClient client;

    //Called when your application first starts. Sets up RPC
    public static void Initialize()
    {
        client = new DiscordRpcClient(Resources.DiscordAPI)
        {
            Logger = new ConsoleLogger() { Level = LogLevel.Warning }
        };

        //Subscribe to events
        client.OnReady += (sender, e) =>
        {
            Debug.WriteLine("Received Ready from user {0}", e.User.Username);
        };

        client.OnPresenceUpdate += (sender, e) =>
        {
            Debug.WriteLine("Received Update! {0}", e.Presence);
        };

        //Connect to the RPC
        client.Initialize();
    }
}