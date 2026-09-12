# Well-Aimed

> *Jogo de tiro em primeira pessoa de invasão zumbi no velho-oeste*

[![Engine](https://img.shields.io/badge/Unity-2021%20LTS-black?logo=unity)]()
[![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-blue)]()
[![License](https://img.shields.io/badge/License-MIT-lightgrey)]()

---

## Sumário

- [Visão Geral](#visão-geral)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Como Começar](#como-começar)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Controle de Versão](#controle-de-versão)
- [Convenções de Commit](#convenções-de-commit)
- [Autor](#autor)

---

## Visão Geral

| Campo       | Valor                                                    |
|-------------|----------------------------------------------------------|
| Nome        | Well-Aimed                                    |
| Gênero      | FPS                                                 |
| Objetivo    | Aprender conceitos de desenvolvimento de jogos com Unity |

### Objetivos de Aprendizado

- **Fundamentos** — Entender a arquitetura de GameObjects, Components e o ciclo de vida de scripts em C#.
- **Prática** — Experimentar mecânicas simples (movimentação, colisões, UI, áudio) para consolidar conceitos.
- **Organização** — Manter uma estrutura de pastas e um histórico de commits limpos, como se fosse um projeto profissional, para desenvolver bons hábitos.
- **Documentação** — Registrar o que foi aprendido a cada etapa, servindo de referência futura.

Este projeto não tem pretensão comercial. A ideia é usar o Unity como ferramenta de estudo, testando conceitos, tutoriais e ideias próprias em um ambiente controlado e sem pressão de prazos.

---

## Tecnologias Utilizadas

| Finalidade              | Ferramenta                            |
|-------------------------|---------------------------------------|
| Engine                  | Unity **2021 LTS**                    |
| Linguagem               | C#                                    |
| IDE                     | Visual Studio Code / Visual Studio    |
| Controle de versão      | Git (com Git LFS para assets grandes) |

---

## Como Começar

### Pré-requisitos

- [Unity Hub](https://unity.com/download) instalado
- Unity Editor 2021 LTS instalado
- [Visual Studio Code](https://code.visualstudio.com/) ou [Visual Studio Community](https://visualstudio.microsoft.com/pt-br/downloads/) com suporte a C#
- [Git](https://git-scm.com/) (com LFS instalado)

### Clonando / Abrindo o Projeto

```bash
git clone <url-do-repo>
cd <meu-projeto-unity>
```

1. Abra o **Unity Hub**
2. Clique em **Add** → selecione a pasta do projeto
3. Abra o projeto na versão do Editor indicada
4. Aguarde a importação inicial dos assets (pode levar alguns minutos na primeira vez)

---

## Estrutura do Projeto

```
root/
+---Assets
¦   +---ThirdParty          # Assets e plugins de terceiros
¦   +---_Project            # Todo o conteúdo próprio do projeto
¦       +---Art
¦       +---Audio
¦       ¦   +---Music
¦       ¦   +---SFX
¦       +---Level
¦       ¦   +---Physics
¦       ¦   +---Prefabs
¦       ¦   +---Scenes
¦       ¦   +---UI
¦       +---Scripts
¦       +---Settings
¦           +---Input
¦           +---URP
+---Docs                    # Documentação do projeto
¦   +---.obsidian           # Vault do Obsidian, se usado para anotações
+---Library                 # Cache gerado pela Unity — ignorado
+---Logs                    # Logs do Editor — ignorado
+---Packages                # Dependências gerenciadas pelo Package Manager — tracked
+---ProjectSettings         # Configurações do projeto — tracked
+---UserSettings            # Preferências locais do Editor — ignorado
```

---

## Controle de Versão

Como é um projeto local e didático, o uso de Git é opcional, mas recomendado para acompanhar sua própria evolução.

- Use o [`.gitignore` oficial da Unity](https://github.com/github/gitignore/blob/main/Unity.gitignore) para evitar versionar `Library/`, `Temp/`, `Obj/` e `Builds/`.
- Se o projeto crescer e passar a ter muitos assets grandes (texturas em alta resolução, áudio, vídeo), considere ativar o **Git LFS**.
- Não é necessário um fluxo de branches elaborado para um projeto solo — uma branch `main` (ou `master`) já é suficiente. Branches extras (`experimento/*`, `feature/*`) podem ser úteis apenas se você quiser testar algo sem bagunçar o que já funciona.

### Fluxo Simples Sugerido

```bash
git add .
git commit -m "feat: adiciona movimentação básica do personagem"
git push
```

Se quiser experimentar algo arriscado sem comprometer o que já está pronto:

```bash
git checkout -b experimento/pulo-duplo
# ... testar a ideia ...
# se der certo:
git checkout main
git merge experimento/pulo-duplo
```

---

## Convenções de Commit

Mesmo em um projeto solo, manter um padrão de commits ajuda a entender sua própria evolução ao revisar o histórico depois.

#### Formato

```text
tipo: descrição curta
```

#### Tipos de Commit

| Tipo       | Descrição                                              |
|------------|-----------------------------------------------------------|
| `feat`     | Nova funcionalidade ou mecânica implementada.              |
| `fix`      | Correção de bug ou comportamento.                          |
| `refactor` | Reorganização de código sem mudar comportamento.           |
| `content`  | Novos assets, cenas, sprites, áudio, etc.                  |
| `config`   | Ajustes de configuração do projeto ou do Editor.           |
| `docs`     | Alterações apenas na documentação.                         |
| `chore`    | Manutenção geral do repositório.                            |

**Exemplos:**

```text
feat: adiciona sistema de pulo do personagem
fix: corrige colisão do jogador com plataformas
content: importa sprites do personagem principal
docs: atualiza README com instruções de setup
```

#### Boas Práticas

- Mantenha a mensagem curta e objetiva.
- Use o modo imperativo (ex.: *adiciona*, *corrige*, *remove*).
- Um commit, uma mudança lógica — evite misturar várias coisas não relacionadas.

---

## Convenções de Nomenclatura (C# / .NET)
 
Para manter o código legível e alinhado com as práticas oficiais da Microsoft, este projeto segue as [diretrizes de nomenclatura do .NET](https://learn.microsoft.com/pt-br/dotnet/standard/design-guidelines/naming-guidelines). Abaixo, uma descrição de cada convenção utilizada.
 
#### PascalCase
 
Cada palavra do identificador começa com letra maiúscula, incluindo a primeira, e não há separadores como underline ou espaço (ex.: `PlayerHealth`, `MoveSpeed`, `CalculateDamage`). É usado para:
 
- **Classes, structs e interfaces** (interfaces também recebem o prefixo `I`, ex.: `IDamageable`)
- **Métodos** (ex.: `TakeDamage()`, `RespawnPlayer()`)
- **Propriedades** (ex.: `CurrentHealth`, `IsGrounded`)
- **Enums e seus valores** (ex.: `GameState.Paused`)
- **Eventos** (ex.: `OnPlayerDied`)
- **Namespaces**
#### camelCase
 
Semelhante ao PascalCase, mas a primeira palavra começa com letra minúscula; as demais continuam com inicial maiúscula (ex.: `moveSpeed`, `isJumping`, `currentTarget`). É usado para:
 
- **Parâmetros de métodos** (ex.: `void SetSpeed(float newSpeed)`)
- **Variáveis locais** dentro de métodos
- **Campos privados**, geralmente prefixados com underline (`_`) para diferenciá-los de propriedades e variáveis locais (ex.: `_currentHealth`, `_isDead`). Essa convenção com underline é amplamente adotada em projetos C#, incluindo os padrões internos da Microsoft para campos privados.
#### UPPER_CASE (ou SCREAMING_SNAKE_CASE)
 
Todas as letras em maiúsculo, com palavras separadas por underline (ex.: `MAX_HEALTH`, `DEFAULT_SPEED`). Não é uma convenção oficial do .NET (que recomenda PascalCase até para constantes), mas aparece em alguns projetos para destacar valores verdadeiramente imutáveis, como `const` globais.
 
#### Boas Práticas Adicionais
 
- **Nomes descritivos**: prefira nomes que expliquem o propósito da variável ou método (ex.: `CalculateJumpForce()` em vez de `Calc()` ou `Do()`).
- **Evite abreviações** não óbvias (ex.: prefira `Health` a `Hp`, salvo quando a abreviação for amplamente reconhecida, como `Ui` para *User Interface*).
- **Métodos são ações**: como representam comportamentos, seus nomes costumam começar com verbos (ex.: `Jump()`, `LoadLevel()`, `SpawnEnemy()`).
- **Booleanos como perguntas**: variáveis e propriedades booleanas devem soar como uma pergunta de sim/não (ex.: `isAlive`, `hasKey`, `canMove`).
- **Consistência acima de tudo**: mesmo que uma convenção pareça arbitrária, mantê-la consistente em todo o projeto facilita a leitura e a manutenção do código.

---

## Convenções de Nomenclatura (Assets da Unity)
 
Para facilitar a organização e a identificação rápida do tipo de cada asset dentro do Editor, este projeto utiliza prefixos padronizados nos nomes dos arquivos:
 
| Tipo de Asset          | Prefixo   | Exemplo                |
|-------------------------|-----------|-------------------------|
| AudioClip                | `Au_`     | `Au_Jump`               |
| Prefab                    | `P_`      | `P_Player`               |
| Mesh                      | `M_`      | `M_Rock`                 |
| Texture                   | `T_`      | `T_GrassAlbedo`          |
| AnimationClip             | `Anim_`   | `Anim_Run`               |
| AnimationController       | `AC_`     | `AC_Player`              |
| Material                  | `Mat_`    | `Mat_Metal`              |
| PhysicsMaterial           | `Phy_`    | `Phy_Ice`                |
 
O nome após o prefixo deve seguir PascalCase e ser descritivo o suficiente para identificar o asset sem precisar abri-lo (ex.: `T_GrassAlbedo`, `Anim_PlayerRun`, `Mat_MetalRusty`).
 
---

## Autor

**Desenvolvedor:** Kauã Souza (kauaclsouza@gmail.com)