$webFakeFolderItem = Get-Item "..\"
$dockerFileItem = Get-Item -Path "..\WebFake\Dockerfile"

$dockerfilePath = $dockerFileItem.FullName
$imageName = "web-fake"
$webFakeFolderPath = $webFakeFolderItem.FullName


Write-Host "Docker file path: $dockerfilePath"
Write-Host "Image name: $imageName"
Write-Host "Web fake folder path: $webFakeFolderPath"

docker build -t $imageName -f $dockerfilePath $webFakeFolderItem.FullName
