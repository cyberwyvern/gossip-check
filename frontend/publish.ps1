if ((Get-Item .).FullName -notmatch 'frontend$') {
  throw 'the script must be run from its directory'
}

$registryUrl = Read-Host 'Registry URL'

docker build --tag latest .
docker login $registryUrl
docker tag latest $registryUrl/frontend
docker push $registryUrl/frontend