para criar uma migração

no package Manager Console selecione o projeto Infraestrutura e execute esse comando:

Add-Migration Criacao -Context Contexto 

para atualizar no banco:
Update-Database -Context Contexto