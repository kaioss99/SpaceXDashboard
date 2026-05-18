# 🚀 SpaceX Dashboard

Aplicação WPF desenvolvida como painel de análise de dados da SpaceX, consumindo uma API REST e exibindo relatórios e insights visuais.

---

## 📋 Sobre o Projeto

Este projeto faz parte de um trabalho acadêmico em grupo simulando um ambiente de microserviços. A aplicação consome dados da API REST desenvolvida pelo Aluno 2 e os exibe em um painel visual com gráficos e tabelas.

---

## 🏗️ Arquitetura

O projeto segue o padrão **MVVM** (Model-View-ViewModel) com **Repository Pattern**.

```
SpaceXDashboard/
├── Commands/           → RelayCommand e FileFontResolver
├── Data/               → DatabaseContext (SQLite)
├── Models/             → Launch, Rocket, Stats
├── Repositories/       → Interfaces e implementações
├── Services/           → ApiService (consumo da API)
├── ViewModels/         → BaseViewModel e MainViewModel
└── Views/              → MainWindow.xaml
```

---

## 🚀 Funcionalidades

- 📊 **Estatísticas gerais** — total de lançamentos, sucessos e taxa de sucesso
- 🛸 **Lista de lançamentos** — nome, status e detalhes de cada missão
- 🔩 **Lista de foguetes** — nome, status, taxa de sucesso e descrição
- 📈 **Gráficos** — comparativo visual de sucesso vs falha
- 📄 **Geração de PDF** — relatório completo exportado para a Área de Trabalho
- 💾 **Cache local** — dados salvos em SQLite para uso offline

---

## 🔄 Fluxo de Dados

```
API REST (Aluno 2)
      ↓
  ApiService
      ↓
  Repository → salva no SQLite
      ↓
 MainViewModel
      ↓
  MainWindow (WPF)
```

---

## 🛠️ Tecnologias

| Tecnologia | Uso |
|---|---|
| C# / .NET 8 | Linguagem e framework principal |
| WPF | Interface gráfica |
| SQLite | Persistência local |
| Newtonsoft.Json | Deserialização de JSON |
| PdfSharp | Geração de relatórios PDF |

---

## ⚙️ Como Rodar

**1. Clone o repositório:**

```bash
git clone https://github.com/kaioss99/SpaceXDashboard.git
```

**2. Abra o arquivo `SpaceXDashboard.sln` no Visual Studio**

**3. Restaure os pacotes NuGet:**

```bash
dotnet restore
```

**4. Rode o projeto:**

Pressione **F5** ou clique em **▶ SpaceXDashboard**

---

## 📦 Pacotes NuGet

| Pacote | Uso |
|---|---|
| `Microsoft.Data.Sqlite` | Persistência local com SQLite |
| `Newtonsoft.Json` | Deserialização do JSON da API |
| `PdfSharp` | Geração de relatório em PDF |

---

## 📡 API Consumida

A aplicação consome a API REST disponível em:

```
http://apispacex.runasp.net
```

### Endpoints utilizados:

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/SpaceX/launches` | Lista de lançamentos |
| GET | `/api/SpaceX/rockets` | Lista de foguetes |
| GET | `/api/SpaceX/stats` | Estatísticas gerais |

### Exemplo de resposta — Lançamentos:

```json
[
  {
    "Id": "abc123",
    "Name": "Falcon 1 Flight 3",
    "Success": false,
    "Details": "Falha na separação dos estágios"
  }
]
```

### Exemplo de resposta — Estatísticas:

```json
{
  "TotalLaunches": 3,
  "SuccessfulLaunches": 2,
  "FailedLaunches": 1,
  "SuccessRate": 66.67
}
```

---

## 💾 Persistência Local

Os dados são salvos localmente em um banco **SQLite** (`spacex.db`) gerado automaticamente na pasta `bin/Debug` ao iniciar o aplicativo.

**Tabelas criadas:**

```sql
Launches (Id, Name, Success, Details)
Rockets  (Id, Name, Description, Active, SuccessRatePct)
Stats    (Id, TotalLaunches, SuccessfulLaunches, FailedLaunches, SuccessRate)
```

**Comportamento:**
- API online → busca dados e salva no SQLite
- API offline → exibe dados do cache SQLite

---

## 📄 Geração de PDF

O botão **Gerar PDF** exporta um relatório completo com 3 páginas:

| Página | Conteúdo |
|---|---|
| 1 | Estatísticas gerais |
| 2 | Tabela de lançamentos |
| 3 | Tabela de foguetes |

O arquivo é salvo automaticamente na **Área de Trabalho** como `SpaceX_Report.pdf`.

---

## 👥 Equipe

| Aluno | Repositório | Responsabilidade |
|---|---|---|
| Aluno 1 | - | WPF consumidor da API pública SpaceX |
| Aluno 2 | [apispacex.runasp.net](http://apispacex.runasp.net) | API REST em C# hospedada na nuvem |
| Aluno 3 | [SpaceXDashboard](https://github.com/kaioss99/SpaceXDashboard) | WPF painel de análise (este repositório) |

---


