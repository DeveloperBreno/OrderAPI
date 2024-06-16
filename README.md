# Todas a documentação e dominio do negocio serão escritas aqui

## mongo
docker pull mongo
docker run -d --name mongodb-container -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=sua_senha_segura mongo

para logar em algum gerenciado de banco nao relacional
recomendo usar o NoSqlBooster
mongodb://admin:sua_senha_segura@localhost:27017

## redis
docker pull redis
docker run -d --name redis-container -p 6379:6379 redis
acesse: redis://localhost:6379

## rabbitqm services
docker pull rabbitmq:3.11-management
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management
http://localhost:15672

## WebAPI
via terminal, na pasta: WebAPI
faz build:
dotnet publish -c Release -o ./out --runtime linux-x64
construir imagem: 
docker build -t api .
iniciar:
docker run -p 5000:5000 api
depois acesse: http://localhost:5000/swagger/index.html

## front
antes de buildar a imagem faz o teste do build do react:
npm run build
se tudo der certo continue
build:
docker build -t react .
start docker:
docker run -p 3000:3000 react

# DOCS
## Cliete
Cliente abre um link via whatsapp da loja ao por alguma rede social, monta um carrinho de compras depois disso pode fazer umcadastro caso não tenha ou pode fazer o login caso já tenha um caastro.

## Pedido
O cliente escolhe os produtos mas tambem pode adicionar itens adicionais isso é uma opçãp de sistema, todo o sub-item pode adicionar valor ao pedido.

## Web service 
processa o pedido para fazer um pagamento via PIX, de inicio teremos apenas o PIX como forma de pagamento, no futuro teremos todas as formas de pagamento e para cada loja terá suas configurações de pagamento como por exemplo: chave de api no mercado pago.

## Loja
A loja receberá um pedido de aprovação de cada pedido e terá a responsabilidade de entregar esse pedido, cada loja deve notificar o sistema que já está enviando o pedido (em trafego) assim o sistema deve informa ao cliente final que o pedido saiu da loja.

cada loja pode montar um cardapio do dia, os cardapios podem ser pre-configurados e selecionados para cada dia diferente.

cada loja deve informar a quantidade de produtos para produto no cardapio selecionado, assim o sistema atualizará a quantidade em estoque automaticamente.

A loja deve informar as vendas fora do sistema para manter o controle de caixa e o estoque do produto atualizado.





