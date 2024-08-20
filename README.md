
# API de Gestão de Pedidos de Venda
## Como Executar o Projeto

1. **Clonar o Repositório:**
   ```bash
   git clone https://github.com/davidsretzlaff/Store.git
   cd <nome-do-diretorio>
   ```

2. **Instalar as Dependências:**
   - Certifique-se de ter o .NET SDK versão 6.0 ou superior instalado.
   - Instale as dependências necessárias com o comando:
     ```bash
     dotnet restore
     ```

3. **Executar o Projeto:**
   - Para iniciar a aplicação, utilize:
     ```bash
     <nome-do-diretorio>\Store\tests\Store.EndToEndTest dotnet run
     <nome-do-diretorio>\Store\tests\Store.IntegrationTest dotnet run
     <nome-do-diretorio>\Store\tests\Store.UnitTest dotnet run
     ```
   - A API estará disponível em `https://localhost:7017/`.

4. **Executar Testes:**
   - Para executar os testes, utilize:
     ```bash
     dotnet test
     ```

## Descrição

Este projeto é uma API desenvolvida em .NET Core para a gestão de pedidos de venda para clientes jurídicos (empresas). A API permite aos usuários gerenciar clientes, produtos e pedidos de venda, além de lidar com entregas por meio de transportadoras disponíveis. O projeto segue os princípios de Clean Architecture e inclui testes unitários para garantir a qualidade do código.

## Funcionalidades

- **Gestão de Clientes:** Criação, busca por ID, listagem, atualização e exclusão lógica de clientes.
- **Gestão de Produtos:** Integração com uma API externa para consumir produtos das categorias `electronics` e `jewelery`.
- **Gestão de Pedidos de Venda:** Criação, aprovação, cancelamento, busca por código, e listagem de pedidos.
- **Gestão de Entregas:** Criação de entregas e atualização de status das entregas.
- **Autenticação:** Implementação de autenticação para proteger as rotas da API.
- **Middleware:** Implementação de um middleware para adicionar um `Correlation-Id` no header da resposta.
- **Health Check:** Implementação de um endpoint de health check para verificar o estado da aplicação.

## Arquitetura

Este projeto foi desenvolvido seguindo os princípios de **Clean Architecture**, com uma organização clara e separação de responsabilidades em diferentes camadas:

- **Camada de Aplicação:** Contém os casos de uso e a lógica de orquestração.
- **Camada de Domínio:** Contém as entidades, agregados, e regras de negócio.
- **Camada de Infraestrutura:** Contém a implementação de repositórios, acesso a banco de dados, e integrações com APIs externas.
- **Camada de Apresentação:** Contém os controladores da API e a configuração da injeção de dependência.

### Diagrama da Arquitetura

![image](https://github.com/user-attachments/assets/24e98f8e-28e4-4f44-9198-7926dea4c1e2)
![image](https://github.com/user-attachments/assets/97737786-e7bf-4cbb-9458-6affc3989ff5)
![image](https://github.com/user-attachments/assets/fe5e3085-d6f8-48a4-a10e-26f7b113ad94)

## Documentação das Rotas

As rotas da API foram documentadas utilizando o Postman. O arquivo de exportação das rotas está incluído no repositório. Para importar as rotas:

1. Abra o Postman
2. Importe o arquivo de rotas incluído no repositório.

## Comentários Finais

Por favor, se houver qualquer dúvida ou problema durante a execução, sinta-se à vontade para entrar em contato. Se algumas funcionalidades não foram implementadas, elas estarão devidamente sinalizadas no código ou neste README.

---

Esse README fornece uma visão geral clara do projeto, as instruções para execução e onde o diagrama de arquitetura deve ser inserido.
