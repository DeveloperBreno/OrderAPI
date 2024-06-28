## mongo
docker pull mongo
docker run -d --name mongodb-container -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=sua_senha_segura mongo

para logar em algum gerenciado de banco não relacional
recomendo usar o NoSqlBooster
mongodb://admin:sua_senha_segura@localhost:27017

## redis
docker pull redis
docker run -d --name redis-container -p 6379:6379 redis

acesse: redis://localhost:6379

## serviço rabbitqm
docker pull rabbitmq:3.11-management
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management
http://localhost:15672


## WebAPI
via terminal vá até a pasta: WebAPI

faz build:
dotnet publish -c Release -o ./out --runtime linux-x64

construir imagem: 
docker build -t api .

iniciar:
docker run -p 5000:5000 api

depois acesse: http://localhost:5000/swagger/index.html