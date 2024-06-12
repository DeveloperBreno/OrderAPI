# Usar a imagem base do Node.js
FROM node:14-alpine

# Definir o diretório de trabalho
WORKDIR /app

# Copiar os arquivos do projeto
COPY package*.json ./
COPY . .

# Instalar as dependências
RUN npm install

# Construir o aplicativo React para produção
RUN npm run build

# Expor a porta 3000
EXPOSE 3000

# Iniciar o servidor de desenvolvimento
CMD ["npm", "start"]
