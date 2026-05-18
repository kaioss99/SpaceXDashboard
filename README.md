# SpaceX Dashboard

Aplicação WPF desenvolvida como painel de análise de dados da SpaceX, consumindo uma API REST e exibindo relatórios e insights visuais.

## 📋 Sobre o Projeto

Este projeto faz parte de um trabalho acadêmico em grupo simulando um ambiente de microserviços. A aplicação consome dados da API REST desenvolvida pelo Aluno 2 e os exibe em um painel visual com gráficos e tabelas.

## 🏗️ Arquitetura

O projeto segue o padrão **MVVM** (Model-View-ViewModel) com **Repository Pattern**.
SpaceXDashboard/
├── Commands/           → RelayCommand e FileFontResolver
├── Data/               → DatabaseContext (SQLite)
├── Models/             → Launch, Rocket, Stats
├── Repositories/       → Interfaces e implementações
├── Services/           → ApiService (consumo da API)
├── ViewModels/         → BaseViewModel e MainViewModel
└── Views/              → MainWindow.xaml
