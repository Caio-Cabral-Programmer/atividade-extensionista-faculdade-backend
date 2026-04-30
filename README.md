<div align="center">
  <h1>Atividade Extensionista 2026 - Faculdade UNINTER</h1>
  <h3>App oferecido à comunidade carente do Bairro de Torrões - Recife-PE</h3>
  <p><strong>Backend oficial da plataforma My Smart Money</strong></p>
  <p>
    <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=logoColor=white" alt=".NET 10" />
    <img src="https://img.shields.io/badge/ASP.NET%20Core-Web%20API-0F6CBD?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core Web API" />
    <img src="https://img.shields.io/badge/Entity%20Framework%20Core-10.0-6DB33F?style=for-the-badge" alt="Entity Framework Core 10" />
    <img src="https://img.shields.io/badge/SQL%20Server-2025-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
    <img src="https://img.shields.io/badge/JWT-Autentica%C3%A7%C3%A3o-black?style=for-the-badge&logo=jsonwebtokens" alt="JWT" />
  </p>
</div>

## Intuito do projeto

Este projeto nasceu com um propósito social direto e concreto: auxiliar as famílias em situação de vulnerabilidade do Bairro de Torrões, em Recife-PE, a administrarem suas finanças com segurança, organização e clareza.

Por meio do aplicativo My Smart Money, a proposta é incentivar bons hábitos financeiros, facilitar o controle de entradas e saídas, apoiar a definição de metas, melhorar o planejamento doméstico e fortalecer a tomada de decisão sobre o uso do dinheiro. A expectativa é que, com acompanhamento, disciplina e acesso a uma ferramenta simples e confiável, essas famílias consigam reduzir significativamente sua vulnerabilidade econômica e construir uma realidade financeira mais estável.

Mais do que um software, esta iniciativa extensionista representa uma ação prática de impacto social, conectando tecnologia, educação financeira e transformação comunitária.

## Visão geral da solução

| Item | Descrição |
| --- | --- |
| Nome da solução | My Smart Money |
| Tipo de projeto | API REST em ASP.NET Core 10 |
| Finalidade | Gestão financeira pessoal com foco em organização, segurança e autonomia |
| Público-alvo | Famílias da comunidade do Bairro de Torrões |
| Arquitetura | Tradicional em camadas |
| Frontend | https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-frontend |

## Galeria do app (Frontend)

> Esta seção é destinada ao frontend do projeto.
>
> **Acesse o repositório do frontend:** [https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-frontend](https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-frontend)

**Tela Inicial:**
<img width="1880" height="954" alt="Tela Inicial" src="https://github.com/user-attachments/assets/ae80aefe-e197-4d1b-8de0-450983afbf4e" />
-
**Dashboard:**
<img width="1881" height="952" alt="Dashboard" src="https://github.com/user-attachments/assets/b3855665-dfca-48c4-b660-a9393d4f72f2" />
-
**Transações:**
<img width="1875" height="955" alt="Transações" src="https://github.com/user-attachments/assets/93f72557-38af-4e1d-a617-29d4d1414303" />
-
**Contas:**
<img width="1880" height="958" alt="Contas" src="https://github.com/user-attachments/assets/d90aea77-e2ed-4949-bf2c-3b8c53a03be6" />
-
**Categorias:**
<img width="1880" height="955" alt="Categorias" src="https://github.com/user-attachments/assets/1ee79233-157c-4873-a612-3887606183d1" />
-
**Orçamentos:**
<img width="1876" height="957" alt="Orçamentos" src="https://github.com/user-attachments/assets/99cf3f4f-097d-40ca-b15a-b2079ba1c72e" />
-
**Metas:**
<img width="1876" height="955" alt="Metas" src="https://github.com/user-attachments/assets/dedeb05f-6fb5-4e33-84f3-583c8658ef11" />
-

## Principais funcionalidades do backend

- Autenticação com JWT.
- Cadastro e gerenciamento de contas financeiras.
- Controle de cartões de crédito.
- Cadastro de categorias e tags.
- Lançamento e acompanhamento de transações.
- Suporte a transações recorrentes.
- Gestão de orçamentos.
- Definição e monitoramento de metas financeiras.
- Dashboard com visão consolidada dos dados.
- Validação de dados com FluentValidation.
- Observabilidade com logs estruturados, métricas Prometheus e integração com Grafana.

## Stack tecnológica

- ASP.NET Core 10 para construção da API REST.
- Entity Framework Core 10 para acesso a dados.
- SQL Server como banco de dados relacional.
- JWT Bearer Token para autenticação.
- FluentValidation para validação de payloads.
- Serilog com saída JSON para observabilidade.
- Prometheus e Grafana para métricas e monitoramento.
- OpenAPI + Scalar para documentação técnica da API.
- MailKit para envio de e-mails via Gmail SMTP.

## Como instalar e rodar o projeto

### 1. Pré-requisitos

Antes de começar, garanta que sua máquina tenha:

- .NET SDK 10.0 instalado.
- SQL Server disponível localmente ou via Docker.
- Docker Desktop ou Docker Engine no Ubuntu, caso deseje subir a infraestrutura em contêineres.
- CLI do Entity Framework Core instalada.
- Uma conta Gmail com senha de aplicativo, caso o fluxo de e-mail seja utilizado.

Se ainda não tiver a CLI do EF Core, instale com:

```bash
dotnet tool install --global dotnet-ef
```

### 2. Clonar o repositório

```bash
git clone https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-backend.git
cd atividade-extensionista-faculdade-backend
```

### 3. Preparar o banco de dados

Você pode usar uma instância local do SQL Server ou subir o banco com Docker.

#### Opção recomendada para desenvolvimento com Docker

Crie um arquivo `.env` na raiz do repositório com o conteúdo abaixo:

```env
MSSQL_SA_PASSWORD=SuaSenhaForte123!
MSSQL_PID=Developer
GF_ADMIN_USER=admin
GF_ADMIN_PASSWORD=admin123
```

Em seguida, suba apenas o SQL Server:

```bash
docker compose up -d sqlserver
```

> Observação técnica: o arquivo `docker-compose.yaml` também contempla Prometheus e Grafana. Para subir a stack completa de observabilidade, mantenha um arquivo `prometheus-web.yml` disponível na raiz do projeto, pois ele é referenciado pela composição.

### 4. Configurar as variáveis de ambiente da API

O projeto usa placeholders no ambiente de desenvolvimento e resolve essas variáveis em tempo de execução.

Variáveis esperadas pela API:

| Variável | Finalidade | Exemplo |
| --- | --- | --- |
| `MSSQL_SERVER` | Host e porta do SQL Server | `localhost,1435` |
| `MSSQL_DATABASE` | Nome do banco | `MySmartMoney_DB` |
| `MSSQL_USERID` | Usuário do banco | `sa` |
| `MSSQL_PASSWORD` | Senha do banco | `SuaSenhaForte123!` |
| `JWT_SECRET_KEY` | Chave secreta do token JWT | `uma-chave-grande-e-segura` |
| `GMAIL_SENDER_EMAIL` | Conta remetente do Gmail | `seuemail@gmail.com` |
| `GMAIL_APP_PASSWORD` | Senha de aplicativo do Gmail | `senha-de-app` |

Exemplo no PowerShell:

```powershell
$env:MSSQL_SERVER="localhost,1435"
$env:MSSQL_DATABASE="MySmartMoney_DB"
$env:MSSQL_USERID="sa"
$env:MSSQL_PASSWORD="SuaSenhaForte123!"
$env:JWT_SECRET_KEY="uma-chave-grande-e-segura"
$env:GMAIL_SENDER_EMAIL="seuemail@gmail.com"
$env:GMAIL_APP_PASSWORD="senha-de-app"
```

### 5. Restaurar dependências do projeto

Entre na pasta da aplicação e restaure os pacotes NuGet:

```bash
cd src
dotnet restore
```

### 6. Aplicar as migrations no banco

Com as variáveis configuradas e o banco disponível, execute:

```bash
dotnet ef database update
```

### 7. Executar a API

Para rodar com HTTPS em ambiente de desenvolvimento:

```bash
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

Se preferir o perfil HTTP:

```bash
dotnet run --launch-profile http
```

### 8. Recursos disponíveis após a inicialização

Depois que a aplicação subir com sucesso, você terá acesso a:

- API local em ambiente de desenvolvimento.
- Documentação OpenAPI e interface Scalar.
- Endpoint de métricas em `/metrics`.
- Política de CORS preparada para o frontend local em `http://localhost:5173`.

## Explicação técnica detalhada

### Arquitetura adotada

O backend segue uma arquitetura tradicional em camadas, organizada para manter separação de responsabilidades, facilitar manutenção e permitir evolução segura do sistema.

- `Controllers`: expõem os endpoints HTTP.
- `DTOs`: definem os contratos de entrada e saída.
- `Validators`: aplicam regras de validação com FluentValidation.
- `Services`: concentram as regras de negócio.
- `Repositories`: encapsulam o acesso ao banco via Entity Framework Core.
- `Entities`: representam o domínio persistido no SQL Server.
- `Extensions`: realizam mapeamentos manuais entre entidades e DTOs.
- `Middlewares`: tratam erros globais e comportamento transversal.
- `Migrations`: versionam a estrutura do banco de dados.

### Fluxo técnico de uma requisição

1. O cliente envia uma requisição HTTP para um controller.
2. O payload é validado por DTOs e validators.
3. O service correspondente aplica as regras de negócio.
4. O repository executa as operações no SQL Server via EF Core.
5. O resultado é transformado manualmente para DTO de resposta.
6. Em caso de falha de negócio, exceções customizadas são tratadas pelo middleware global.

### Segurança da aplicação

- Autenticação baseada em JWT Bearer Token.
- Uso de HTTPS no pipeline da aplicação.
- CORS configurado para integração com o frontend.
- Tratamento centralizado de exceções com Problem Details.
- Validação de entrada para reduzir inconsistências e riscos.

### Observabilidade e suporte operacional

- Logs estruturados em JSON com Serilog.
- Persistência de logs locais em arquivos rotativos.
- Exposição de métricas para Prometheus.
- Possibilidade de visualização analítica via Grafana.

### Módulos expostos pela API

O backend foi organizado em torno dos principais módulos do negócio financeiro:

- Autenticação e usuários.
- Contas.
- Cartões de crédito.
- Categorias.
- Tags.
- Transações.
- Transações recorrentes.
- Orçamentos.
- Metas financeiras.
- Dashboard.

### Estrutura resumida do projeto

```plaintext
src/
├── Controllers/
├── DTOs/
├── Entities/
├── Exceptions/
├── Extensions/
├── Middlewares/
├── Migrations/
├── Repositories/
├── Services/
├── Validators/
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

## Integração com o frontend

Este repositório representa exclusivamente o backend da solução. A interface do usuário está em um projeto separado, que consome esta API para oferecer a experiência visual completa da aplicação.

**Frontend oficial:** [https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-frontend](https://github.com/Caio-Cabral-Programmer/atividade-extensionista-faculdade-frontend)

---

**Plano de evolução do app para 2027: Versão Desktop e Mobile**
