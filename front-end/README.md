antes de buildar a imagem faz o teste do build do react:
npm run build
se tudo der certo continue

build:
docker build -t react .

start docker:
docker run -p 80:80 react
