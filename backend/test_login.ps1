try {
    $body = @{Login="pedro.almeida"; Senha="abc123"} | ConvertTo-Json
    $response = Invoke-RestMethod -Uri "http://localhost:5100/api/User/login" -Method Post -Body $body -ContentType "application/json"
    Write-Output "SUCCESS:"
    Write-Output $response
} catch {
    Write-Output "ERROR:"
    if ($_.Exception.Response) {
        Write-Output $_.Exception.Response.StatusCode
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        Write-Output $reader.ReadToEnd()
    } else {
        Write-Output $_.Exception.Message
    }
}
