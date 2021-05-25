if ((Get-Item .).FullName -notmatch 'stance-detection-ai$') {
  throw 'the script must be run from its directory'
}

$registryUrl = Read-Host 'Registry URL'

docker build --tag latest .
docker login $registryUrl
docker tag latest $registryUrl/stance-detection-ai
docker push $registryUrl/stance-detection-ai