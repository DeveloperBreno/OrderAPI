# Toda a documentação e domínio do negócio serão escritos aqui
## MongoDB
docker pull mongo

docker run -d --name mongodb-container -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc mongo

Para logar em algum gerenciador de banco não relacional, recomendo usar o NoSQLBooster:

mongodb://admin:sua_senha_segura@localhost:27017

## PostgreSQL
### 1. Pull e Run do contêiner do PostgreSQL
docker run -e "POSTGRES_DB=MyDatabase" -e "POSTGRES_PASSWORD=DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc" -p 5432:5432 --name postgres -d postgres:latest

### baixe o pg admin para gerenciar o banco de dados postgres sql

## Redis
docker pull redis

docker run -d --name redis-container -p 6379:6379 redis

Acesse: redis://localhost:6379

## RabbitMQ Services
docker pull rabbitmq:3.11-management

docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_PASS="DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc" rabbitmq:3.11-management

http://localhost:15672

## WebAPI
Via terminal, na pasta: WebAPI

Faz build:

dotnet publish -c Release -o ./out --runtime linux-x64

Construir imagem:

docker build -t api .

Iniciar:

docker run -p 443:443 api

Depois acesse: http://localhost:5000/swagger/index.html

### Atualização rapida:
git reset --hard HEAD && git pull && dotnet publish -c Release -o ./out --runtime linux-x64 && docker build -t api . && docker run -p 443:443 api

## Front-end
Antes de buildar a imagem, faça o teste do build do React:

npm run build

Se tudo der certo, continue.

Build:

docker build -t react .

Iniciar Docker:

docker run -p 80:80 react

## Workers

### createandedituser
dentro da pasta:

back-end\Workers\CreateAndEditUser

git reset --hard HEAD && git pull && dotnet publish -c Release -o ./out && docker build -t createandedituser . && docker run -it --rm createandedituser


# DOCS
Cliente
O cliente abre um link via WhatsApp da loja ou por alguma rede social, monta um carrinho de compras. Depois disso, pode fazer um cadastro caso não tenha ou pode fazer o login caso já tenha um cadastro.

## Pedido
O cliente escolhe os produtos, mas também pode adicionar itens adicionais. Isso é uma opção do sistema. Todo sub-item pode adicionar valor ao pedido.

## Web Service
Processa o pedido para fazer um pagamento via PIX. De início, teremos apenas o PIX como forma de pagamento. No futuro, teremos todas as formas de pagamento, e para cada loja, terá suas configurações de pagamento, como por exemplo: chave de API no Mercado Pago.

## Loja
A loja receberá um pedido de aprovação de cada pedido e terá a responsabilidade de entregar esse pedido. Cada loja deve notificar o sistema que já está enviando o pedido (em tráfego), assim o sistema deve informar ao cliente final que o pedido saiu da loja.

Cada loja pode montar um cardápio do dia. Os cardápios podem ser pré-configurados e selecionados para cada dia diferente.

Cada loja deve informar a quantidade de produtos para cada produto no cardápio selecionado, assim o sistema atualizará a quantidade em estoque automaticamente.

A loja deve informar as vendas fora do sistema para manter o controle de caixa e o estoque do produto atualizado.

# Domínio Cliente Final
## Endereço
O cliente deverá informar o seu CEP, logradouro, número e uma observação (opcional).

## Contato
O cliente deverá informar seu número de celular ou telefone fixo.

## Pagamento
O cliente deve pagar seu pedido via Mercado Pago utilizando PIX. Futuramente, teremos todas as formas de pagamento.

## Pedidos
O cliente pode ver o histórico de pedidos.

Pode acompanhar seus pedidos em tempo real, exemplo: pedido em andamento na loja > pedido a caminho > entregue.

# Domínio Loja
## Status
O logista deve abrir o sistema e informar o status da loja, como por exemplo: Disponível, Fechado.

Ao abrir a loja e mudar o status para disponível, a loja deve informar os produtos e quantidade disponível que irá vender no dia ou na virada do dia, podendo abrir às 18h e fechando às 1h da manhã.

Ao fechar o sistema, automaticamente o sistema deve deixar o status como "Fechada", impedindo compras do cliente final.

## Vendas Externas (vendas por fora do sistema online)
Caso a loja faça um pedido de venda fora do sistema, deve informar o pedido para atualizar o estoque e manter o fluxo de caixa atualizado.

## Divulgação
A loja pode ordenar os produtos para o cliente final.

A loja pode deixar uma observação e uma observação pequena, um título, até duas fotos (isso não pode ser hardcode, podendo ter alterações futuras).

# Domínio Web Services
## Cliente Final
Deve ser salvo na base de dados.

## Pedidos
Serão salvos, mas nunca alterados.

## Pedido Finalizado
Deve informar a loja, via real-time SignalR.

## Pagamentos
Salvos em uma base de dados, mas nunca alterados. Nesse ponto, poderá utilizar banco não relacional, apenas utilizando alguns campos como IDs externos, como UserId, lojaId.

## Pedido a Caminho
O logista deve informar ao sistema que deve informar/atualizar o status do pedido na tela do cliente final.

## Cliente Final
Pode ver seus pedidos com seus status atualizados em tempo real.
