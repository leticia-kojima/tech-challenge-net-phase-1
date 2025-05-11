## FCG - FIAP Cloud Games

**MVP para uma plataforma de jogos voltados para a educação de tecnologia.**

O projeto conta com algumas funcionalidades obrigatórias e alguns requisitos técnicos que serão compridos para o desenvolvimento desta primeira fase do desafio. A ideia proposta é desenvolver uma plataforma de venda de jogos digitais e gestão de servidores para partidas online.

TODO: Add a summary

## Organização
Semanalmente são realizadas reuniões para acompanhar o progresso das tarefas, discutir aspectos técnicos e priorizar demandas do projeto. O fluxo de trabalho é organizado em um board simples no trello, em que tarefas são trafegadas em listas conforme sua execução.

#### Equipe
Fazem parte do quadro do time os seguintes membros:
 - [Paulo](https://github.com/paulobusch)
 - [Geovanne](https://github.com/gehcosta)
 - [Letícia](https://github.com/leticia-kojima)
 - [Matheus](https://github.com/M4theusVieir4)
 - [Marcelo](https://github.com/marceloalvees)

## Ferramentas
// TODO
 - **[Trello](https://trello.com/)**: Gestão do fluxo de trabalho;
 - **[Egon.io](https://egon.io/)**: Elaboração do *Domain Storytelling*;
 - **[Notion](https://www.notion.so/)**: Documentação e check list das entregas.
 - Miro
 - GitHub

## Diagramas
// TODO
### Storytelling
Foi confeccionado um diagrama de fluxo utilizando a ferramenta [Egon.io](https://egon.io/) para mapear as iterações que acontecem entre os atores e elementos do sistema.

![Storytelling](docs/Storytelling.png)

A plataforma **FCG** prevê dois níveis de acesso diferentes, um para Administrador e outro para Usuário, representando os atores da aplicação. Do item 1 ao 3, o **Administrador** faz a gestão das informações dos usuários, catálogos e jogos. Já do item 4 ao 7, o **Usuário** pode criar uma conta, acessar os catálogos, baixar e fazer a avaliação de um jogo.

### Event Storming
// TODO
1 - Brainstorming
2 - Fluxograma
3 - Comandos e Eventos
4 - Agregados
![Agregados](docs/Event%20Storming/4%20-%20Agregados.jpg)

![Diagrama de Classes](docs/Diagrama%20de%20Classes.png)

## Arquitetura
// TODO
CQRS
Projetos:
 - FCG.API
 - FCG.Application
 - FCG.Domain
 - FCG.Infrastructure
 - FCG.UnitTests

Bibliotecas:
 - MediatR
 - ...

## Execução
Para trabalhar no projeto, é necessário garantir a execução dos servidores dos bancos de dados. Entretanto, se não os tiver, é possível fazer download nos links descritos na sub-seção seguinte.

### Requisitos e Ferramentas
 - **[Visual Studio Community 2022](https://visualstudio.microsoft.com/pt-br/vs/community/)**: IDE recomendada para trabalhar no projeto;
 - **[MongoDB Community Server](https://www.mongodb.com/try/download/community)**: Banco de dados não relacional utilizado para consultas;
 - **[MySQL Community Server](https://dev.mysql.com/downloads/mysql/)**: Banco de dados relacional utilizado para comandos.

### Configuração
A conexão com cada um dos banco de dados é configurada no arquivo `appsettings.json`. Caso necessário, atualize com o usuário e senha definidos no servidor. A tabela abaixo detalha estas configurações.

| Chave | Descrição |
| - | - |
| `ConnectionStrings:FCGCommands` | String de conexão com o banco de dados MySQL. |
| `ConnectionStrings:FCGQueries` | String de conexão com o banco de dados MongoDB. |

### Debug e Teste
É necessário definir o projeto `FCG.API` como sendo de inicialização e escolher a opção `http` para executar a API. O arquivo `FCG.API.http` presente neste projeto, contempla os endpoints existentes. No Visual Studio aparecerá a opção para fazer debug e envio de requisição.

## Migrations
Para fazer atualizações nas tabelas do banco de dados é necessário gerar migrations. A seguir são apresentados os comandos que devem ser usados para fazer e aplicar estas alterações:

1. Faça, primeiro o acesso ao diretório do projeto de infraestrutura, por de um terminal console ou do gerenciador de pacotes do Visual Studio.
2. Execute o comando abaixo para registrar as alterações em uma migration:
    ```
    Add-Migration MigrationName
    ```
    Substitua `MigrationName` por um nome significativo que será dado à migration.


3. Aplique as alterações no banco de dados executando o comando abaixo:
    ```
    Update-Database
    ```

Também é possível fazer a remoção e reversão de migrations e alterações no banco de dados, caso necessário consulto a opção do ajuda do EF Core `Get-Help EntityFramework`.