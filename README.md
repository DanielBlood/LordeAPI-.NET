# LordeAPI .NET


A simple .NET Wrapper for LordeAI
Lorde is an artificial intelligence bot, An alternative to Cortana, Google Assistant and Siri. Lorde utilizes a machine learning algorithm to learn how to talk, including other languages without configuration. And offers helpful features for looking up information on the web

### Usage
It's pretty simple, I precompiled it in both C# and VB .NET, Check the Build folder for the compiled dll
### VB .NET Example
```vb
Module LordeVB
    Public API As New LordeAPI.Chat
    Sub Main()
        API.API_Key = "API Test"
        Do
            Console.Write("Message: ")
            Dim Message As String = Console.ReadLine
            Console.WriteLine("Lorde is typing...")
            Try
                Console.WriteLine(API.SendRequest(Message, True))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        Loop
    End Sub
End Module
```
### C# Example
```csharp
static class LordeCSharp
{
    public static LordeAPI.Chat API = new LordeAPI.Chat();
    public static void Main()
    {
        API.API_Key = "API Test";
        do {
            Console.Write("Message: ");
            string Message = Console.ReadLine;
            Console.WriteLine("Lorde is typing...");
            try {
                Console.WriteLine(API.SendRequest(Message, true));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        } while (true);
    }
}
```

### Lorde in action

If you want to see Lorde in action, Feel free to check out the links below.

* https://Telegram.me/LordeBot
* https://Kik.me/LordeAATA
