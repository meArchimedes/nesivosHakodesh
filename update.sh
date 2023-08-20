echo "Starting to update Front End"
cd /var/www/nesivoshakodeshapp/nesivos-hakodesh-portal

sudo git checkout -- package-lock.json

sudo git pull
echo "git updated"

sudo npm i
echo "npm updated"

sudo ng build --delete-output-path=false --output-hashing=all --build-optimizer=false
echo "Front End Updated"

echo "Starting to update Back End"
cd /var/www/nesivoshakodeshapp/NesivosHakodesh

sudo service kestrel-nesivos stop
echo "service stoped"

sudo dotnet publish
echo "published"

sudo service kestrel-nesivos start
echo "service started"


sudo service kestrel-nesivos status
echo "Done"
