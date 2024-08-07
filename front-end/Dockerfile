# Etapa de Construção
FROM node:18 AS build

WORKDIR /app

# Copia o package.json e package-lock.json (ou yarn.lock)
COPY package*.json ./

# Instala as dependências
RUN npm install

# Copia o restante do código
COPY . .

# Executa o comando de construção
RUN npm run build

# Etapa de Produção
FROM nginx:alpine

# Copia os arquivos construídos para o diretório do Nginx
COPY --from=build /app/build /usr/share/nginx/html

# Expõe a porta 80
EXPOSE 80

# Comando de inicialização do Nginx
CMD ["nginx", "-g", "daemon off;"]
