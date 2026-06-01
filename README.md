# Mini Degreed 🎓

Plataforma de aprendizado inspirada no Degreed, construída com arquitetura de microsserviços em .NET 10.
O projeto é dividido em serviços independentes — cada um com sua responsabilidade, seu banco de dados e sua API — mas todos trabalhando em harmonia para entregar a experiência completa ao usuário.

## Arquitetura

O sistema é composto por 5 microsserviços independentes que se comunicam via REST APIs, JWT e mensageria assíncrona com RabbitMQ.

Client
└── Auth Service           → emite e valida tokens JWT
├── Course Service         → catálogo de cursos (protegido por JWT)
├── User Service           → perfil do usuário (evento via RabbitMQ)
├── Progress Service       → matrícula e progresso por aulas
└── Recommendation Service → sugestões baseadas em categorias

## Serviços

| Serviço | Porta | Responsabilidade | Banco |
|---------|-------|-----------------|-------|
| Auth Service | 5003 | Registro, login e validação JWT | auth.db |
| Course Service | 5002 | CRUD de cursos | course.db |
| User Service | 5004 | Perfil e histórico do usuário | user.db |
| Progress Service | 5005 | Matrícula e progresso por aulas | progress.db |
| Recommendation Service | 5006 | Sugestões por categoria | — |

## Tecnologias

- **.NET 10** — framework principal
- **Angular 18** — frontend SPA
- **Entity Framework Core** — ORM com SQLite
- **JWT (Bearer)** — autenticação stateless entre serviços
- **BCrypt** — hash de senha com salt
- **RabbitMQ** — mensageria assíncrona entre serviços
- **Docker** — containerização dos serviços
- **xUnit + Moq** — testes unitários com mocks
- **Clean Architecture** — Controller → Service → Repository

## Como rodar

### Pré-requisitos
- .NET 10 SDK
- Node.js 20+
- Angular CLI 18
- Docker Desktop

### Subindo o RabbitMQ
```bash
docker start rabbitmq
```

### Rodando os serviços

Clone o repositório:
```bash
git clone https://github.com/paulovitorlr/mini-dagreed.git
cd mini-dagreed
```

Rode cada serviço em um terminal separado:
```bash
cd services/auth-service && dotnet run
cd services/course-service && dotnet run
cd services/user-service && dotnet run
cd services/progress-service && dotnet run
cd services/recommendation-service && dotnet run
```

### Rodando o frontend
```bash
cd frontend/mini-degreed-web
ng serve
```

Acessa: http://localhost:4200

### Rodando os testes
```bash
cd services/auth-service.Tests
dotnet test
```

## Fluxo principal

POST /auth/register            → cria conta + publica evento RabbitMQ
POST /auth/login               → retorna JWT
POST /courses                  → cria curso (requer JWT)
POST /progress/enroll          → matricula no curso
POST /progress/complete-lesson → conclui aula
GET  /progress/my-progress     → vê progresso %
GET  /recommendations          → recebe sugestões por categoria

## Mensageria (RabbitMQ)

| Evento | Producer | Consumer | Ação |
|--------|----------|----------|------|
| user.created | Auth Service | User Service | Cria perfil do usuário |

## Decisões técnicas

**Por que microsserviços?**
Cada serviço pode ser desenvolvido, testado e escalado de forma independente.
Uma falha no Recommendation Service não derruba o Auth ou o Progress.

**Por que bancos separados?**
Evita acoplamento entre serviços. Cada um evolui seu schema sem impactar os outros.

**Por que JWT stateless?**
Cada serviço valida o token localmente usando a chave secreta compartilhada,
sem precisar consultar o Auth Service a cada requisição.

**Por que RabbitMQ?**
Garante que eventos entre serviços não se percam mesmo que um serviço esteja fora do ar.
A mensagem fica na fila até o serviço consumidor estar disponível.

**Trade-offs conhecidos**
- Sem API Gateway centralizado (evolução: YARP ou Ocelot)
- Docker com problema de espaço em disco no ambiente de desenvolvimento
- Testes unitários implementados apenas no Auth Service

## Próximos passos

- [x] Docker + docker-compose para orquestração local
- [x] RabbitMQ para comunicação assíncrona entre serviços
- [x] Frontend em Angular
- [ ] API Gateway com YARP ou Ocelot
- [ ] Testes unitários para Course, Progress e Recommendation Services
- [ ] CI/CD com GitHub Actions