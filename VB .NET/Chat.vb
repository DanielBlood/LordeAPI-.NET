Public Class Chat
    'VB.NET Class writeen by Daniel Blood
    'Github: http://github.com/danielblood
    'Version v1.0 First public release

    Public Property API_Endpoint As String = "https://lorde.material-cloud.net/api.php" 'The API Endpoint, Don't change this unless we moved the API
    Public Property API_Key As String 'Your API Key, you need to register it
    Public Property Client As String = "VB.Net Client"
    Public Function SendRequest(ByVal Message As String, Optional ByVal ReplaceNewLine As Boolean = False, Optional ByVal Username As String = "Unknown")
        'First check for Empty settings
        If API_Key = Nothing Then Throw New Exception("Missing property: API_Key")
        If Message = Nothing Then Throw New Exception("Missing argument: Message")
        If Client = Nothing Then Client = "VB.Net Client"
        If Username = Nothing Then Username = "Unknown"

        'Prepare the request
        Dim webRequest As System.Net.HttpWebRequest = DirectCast(System.Net.WebRequest.Create(API_Endpoint), System.Net.HttpWebRequest)
        'Add the headers
        webRequest.Headers.Add("client", Client)
        webRequest.Headers.Add("username", Username)
        webRequest.Headers.Add("key", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(API_Key)))
        webRequest.Headers.Add("msg", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Message)))

        'Read the response
        Dim response As System.Net.HttpWebResponse = CType(webRequest.GetResponse(), System.Net.HttpWebResponse)
        Dim receiveStream As System.IO.Stream = response.GetResponseStream()
        Dim readStream As New System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8)
        Dim StrResponse As String = readStream.ReadToEnd

        'Check response status
        If StrResponse.ToLower.Contains("{""status"":""500""") Then
            Throw New Exception("Error 500: Server error/Being updated")
        ElseIf StrResponse.ToLower.Contains("{""status"":""401""") Then
            Throw New Exception("Error 401: Invalid/Banned/Incorrect API Key")
        ElseIf StrResponse.ToLower.Contains("{""status"":""400""") Then
            Throw New Exception("Error 400: Invalid/Empty message")
        ElseIf StrResponse.ToLower.Contains("{""status"":""200""") Then
            'Use Regex to read the JSON Response
            Dim r As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(".*?"".*?"".*?"".*?"".*?"".*?"".*?("".*?"")", System.Text.RegularExpressions.RegexOptions.IgnoreCase Or System.Text.RegularExpressions.RegexOptions.Singleline)
            Dim m As System.Text.RegularExpressions.Match = r.Match(StrResponse)
            If (m.Success) Then
                'Remove the quotes in a lazy way.
                Dim G1 As String = m.Groups(1).ToString()
                Dim G2 As String = G1.Remove(0, 1)

                'Finally return the response from Lorde
                If ReplaceNewLine = True Then
                    Return G2.Substring(0, G2.Length - 1).Replace("\n", vbCrLf)
                Else
                    Return G2.Substring(0, G2.Length - 1)
                End If

            Else
                'If it cannot get the response from Lorde, Simply return the full JSON Data
                Return StrResponse
            End If
        Else
            Throw New Exception("Error 0: Unknown Response")
        End If
    End Function
End Class
