docker create -p 14888:80 -p 27017 --network fog_network -e ASPNETCORE_URLS=http://+ -e fakeid=webfake --name web-fake --restart unless-stopped dbasarab/web-fake:latest

docker start web-fake
