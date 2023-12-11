docker create -p 14888:80 -p 27017 --network fog_network -e ASPNETCORE_URLS=http://+ -e fakeid=david --name web-fake web-fake

docker start web-fake
