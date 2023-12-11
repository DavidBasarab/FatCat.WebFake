docker create -p 14888:80 -p 27017 --network fog_network -e ASPNETCORE_URLS=http://+ -e fakeid=david --name web-fake dbasarab/web-fake:latest

docker start web-fake
