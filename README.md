# Service Template — Serviço Windows em .NET 8

## Visão geral
Este repositório contém um template de Serviço Windows construído com .NET 8 e TopShelf. O projeto já vem estruturado com camadas separadas (Host → Business → Library), configuração centralizada, logging com Serilog e execução baseada em timer.

O propósito é criar um "esqueleto" que sirva como base para a criação de serviços Windows que sigam um padrão tanto de implementação quanto de configuração e logging.

Funcionalidade de exemplo disponível:
- Timer configurável que executa threads de trabalho em intervalos definidos via `appsettings.json`.

## Tecnologias e bibliotecas essenciais
- .NET 8 (Console Application)
- TopShelf (hospedagem e instalação de serviços Windows)
- Serilog (logging para console e arquivo)
- Microsoft.Extensions.Configuration (leitura de `appsettings.json`)

## Estrutura do projeto
- `Service.Host/Program.cs`: ponto de entrada; configura TopShelf, carrega configurações e inicializa o serviço.
- `Service.Business/ServiceWork.cs`: classe principal que contém a lógica de negócio e o timer de execução.
- `Service.Business/Configuration/Config.cs`: gerenciador centralizado de configurações via `appsettings.json`.
- `Service.Business/Logging/Logger.cs`: inicialização e wrappers do Serilog com métodos `Debug`, `Info` e `Error`.
- `Service.Library/Models/`: camada para modelos de dados (placeholder para futura implementação).
- `Service.Host/appsettings.json`: configurações da aplicação (intervalo de execução, diretório de logs, níveis de log).

## Arquitetura e padrões de projeto
- Hospedagem e ciclo de vida
    - Usa TopShelf para facilitar instalação, execução e gerenciamento como serviço Windows.
    - Executa como `LocalSystem` por padrão.
    - Nome do serviço: `ServiceTemplate`.
    - Callbacks de `WhenStarted` e `WhenStopped` para controle do ciclo de vida.

- Separação de camadas
    - **Service.Host**: responsável apenas pela hospedagem e bootstrap.
    - **Service.Business**: contém toda a lógica de negócio, configuração e logging.
    - **Service.Library**: camada para modelos compartilhados.

- Logging (Serilog)
    - Logs em console e arquivo rolling diário em `logs/system_log_.txt` (diretório configurável).
    - Formato padronizado: `ClassName - MethodName - Message`.
    - Em falhas na inicialização, um arquivo é escrito em `StartupErrors/` para garantir rastreabilidade mesmo antes do logger estar ativo.

- Tratamento de erros
    - Exceções no startup são capturadas e registradas em arquivo dedicado antes de encerrar com `Environment.Exit(1)`.
    - Exceções durante execução são logadas e re-lançadas para que o TopShelf gerencie adequadamente.

## Configuração
Arquivo: `Service.Host/appsettings.json`

Seções disponíveis:
- **`ServiceLogging`**:
  - `LogDirectory` (string): pasta para gravação dos logs (relativa ao diretório base da aplicação).
  - `LogLevel` (string): nível mínimo de log ("Debug", "Information", "Warning", "Error").

- **`ServiceConfig`**:
  - `Interval` (int): intervalo em milissegundos entre execuções do timer (padrão: 10000 = 10 segundos).
