via terminal vá até a pasta: WebAPI

faz build:
dotnet publish -c Release -o /out --runtime linux-x64

construir imagem: 
docker build -t api .

iniciar:
docker run -p 5000:5000 api

depois acesse: http://localhost:5000/swagger/index.html