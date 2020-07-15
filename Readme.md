# Programação Paralela com C#

## Introdução

O Objetivo deste respositório é demonstrar formas de programação paralela com C#. Neste projeto, apresento projetos simples para apresentar de conceitos úteis que podem ser utilizados em praticamente todos as soluções desenvolvidas com C#.

O conteúdo esta dividido da seguinte forma:

1. Apresentação de conceitos:
	 - O que significa desenvolvimento paralelo?
	 - O que são de threads?
	 - Diferença entre paralelismo e concorrência;
	 - C#: Tasks, async e await;

2. Introdução a Bibliotecas:
	 - Microsoft Task Parallel Library (TPL);
	 - Task.Run, Task.WhenAll, Task.WaitAll
	 - Paralell.ForEach;
	 - PLinq: Queries Linq com paralelismo
	 - Microsoft Dataflow
	
3. Cuidados a serem tomados:
	- Utilize o objetos imutáveis sempre que possível.
	- Utilize coleções thread-safe.

4. Leitura Recomendada

5. Considerações Finais

# 1.Apresentação de conceitos

## O que significa desenvolvimento paralelo?

De maneira bem simples, desenvolvimento paralelo é a utilização de mais de um núcleo do processador para a execução de uma determinada tarefa. 
Desenvolver um software desta maneira é mais complexa pois o programador é responsável por manter a integridade da aplicação. 
Porém, se utilizada de maneira correta, as aplicações ganham velocidade, responsividade e podem reduzir a complexidade de uma solução distribuída. 
Ex: em padrões mais recentes de desenvolvimento, uma maneira relativamente comum de ganhar agilidade em uma aplicação é a replicando em multiplos processos e colocando uma fila de mensageria para gerenciar a carga de trabalho. 
Há várias vantagens neste tipo de abordagem, porém, uma grande desvatagem é a a dificuldade de apuração de erros e realização do debug neste tipo de ambinete distribuído. (O Erro que pode acontecer em um processor agora pode acontecer em N).
Utilizando os conceitos que apresento aqui, é possível desenvolver aplicações robustas, paralelas e quem são mais simples de debugar se comparado a um ambiente distribuído.

## O que são threads?

Threads é a unidade básica que o Sistema Operacional utiliza para alocar o tempo do processador. Toda thread tem uma prioridade de agendamento e mantém um conjunto de estruturas internas referentes ao seu contexto, para o caso de ser pausada e após isto, ser resumida.
Por padrão, toda aplicação C# (que é um processo) é iniciada com uma Thread, que também é chamada de Thread Primária (Primary Thread).

Fonte: https://docs.microsoft.com/en-us/dotnet/standard/threading/threads-and-threading

## Diferença entre paralelismo e concorrência

Conforme explicado anteriormente, paralelismo é a utilização de mais de uma CPU. Concorrência, por outro lado, é a disputa de uso pelo mesmo recurso. Em termos de computação, um exemplo são os computadores com um único núcleo. 
Todas as aplicações disputam pelo uso do tempo de processamento para execução. 

No nosso caso, o .Net fica responsável pela análise do ambiente para escolha de execução entre um ou múltiplo núcleos, pela abordagem da concorrência x paralelismo. Para fins desta apresnetação, utilizarei para ambos os casos o termo paralelismo.

## C#: Tasks, async e await

Task é a maneira mais recente para desenvolvimento paralelo disponível no .Net. Internamente, ele é uma máquina de estado que gerencia o estado da execução de um código assíncrono. 
A grande vantagem dele em relação a antiga classe Thread é, (além não precisar gerenciar a ThreadPool diretamente) é a possibilidade de criar códigos assíncronos e responsivos de maneira extremamente simples com Async e Await.

Async tem a responsabilidade de marcar um método para ser executado de forma assíncrona, e delega para quem o chama a responsabilidade de lidar com os estados assíncronos da execução em questão.
Await tem a responsabilidade de trazer a execução assíncrona para o contexto do método que o chamar.

De forma bem simplificada, o async marca o método como assíncrono e o await aguarda a execução do método assíncrono.
Async e Await são palavras reservadas no C#. 

OBS: Colocar async na declaração do método não o torna assíncrono automaticamente. Se em seu corpo não houver uma Task ou um await indicando a espera da conclusão de outro método assíncrono, o método em questão executará de forma síncrona (isto, além dos warnings que o compilar irá gerar). 

# 2.Introdução a Bibliotecas

Nesta estapa, irei apresentar as bibliotecas e exemplos de códigos de utilização em alguns exemplos simples de fixação.

## Microsoft Task Parallel Library (TPL)

Esta é a responsável pela disponibilização das principais ferramentas de paralelismo no .Net. É um namespace publico, o System.Threading.Tasks, e pode ser usado em praticamente qualquer classe.

## Task.Run, Task.WhenAll, Task.WaitAll

Verificar exemplos no  na classe ExemplosTPL do projeto Parte2Bibliotecas

## Paralell.ForEach

Verificar exemplos no  na classe ExemplosParallel do projeto Parte2Bibliotecas

## PLinq: Queries Linq com paralelismo

Verificar exemplos no  na classe ExemplosPLinq do projeto Parte2Bibliotecas

## Microsoft Dataflow

O Dataflow é uma biblioteca da microsoft que facilita o desenvolvimento de aplicações assíncronas, com maior robustez para processamento de grandes quantidades de dados. 
Ele simplifica muito a criação de filas de processamento asssíncronas, em um pattern conhecido como Producer-Consumer, onde um ou mais agentes são responsáveis por produzir os dados, inserí-los em uma fila, e outro agentes os consomem.

Os exemplos de código estão na classe ExemplosDataflow no projeto Parte2Bibliotecas

# 3.Cuidados a serem tomados

Apesar do .Net facilitar muito trabalhar com paralelismo, é muito importante se atentar a alguns pontos para não ter problemas com o paralelismo:

## Utilize o objetos imutáveis sempre que possível.

Dentro da orientação a objetos, não é necessariamente um problema mudar uma propriedade ou um atributo de um objeto. 
Porém, quando este objeto pode ser acessado e alterado por multiplas Tasks, isto pode quebrar o estado de uma aplicação e criar problemas em runtime muito complexos e deixar todo o ambiente instável.
Segue dica de leitura sobre este tema:

https://en.wikipedia.org/wiki/Thread_safety \
https://docs.microsoft.com/pt-br/dotnet/standard/threading/managed-threading-best-practices \
Em Java: https://web.mit.edu/6.005/www/fa15/classes/20-thread-safety/


## Utilize coleções thread-safe.

Pelo mesmo motivo do tópico anterior, também é importante trabalhar com coleções Thread-Safe. Uma coleção que pode ter seu estado alterado por multiplas threads pode tornar toda a aplicação instável.\
Ao invés de usar List\<T>, Dictionary\<K,V>, Queue\<T>, utilize ConcurrentBag\<T>, ConcurrentDictionary\<K,V> e ConcurrentQueue\<T>.

Segue dica de leitura sobre o tema:

https://docs.microsoft.com/pt-br/dotnet/standard/collections/thread-safe/


# 4.Leitura recomendada

Sobre conceitos gerais sobre paralelismo:\
https://docs.microsoft.com/en-us/dotnet/standard/threading/threads-and-threading  
https://docs.microsoft.com/pt-br/dotnet/standard/parallel-programming/

Sobre o conceito de Producer-Consumer e profundidade no Dataflow:\
https://docs.microsoft.com/pt-br/dotnet/standard/parallel-programming/dataflow-task-parallel-library

Outras bibliotecas com Actor Pattern e arquitetura distribuída:\
https://dotnet.github.io/orleans/  
https://petabridge.com/bootcamp/

# 5.Considerações Finais

O Tópico sobre paralelismo é bem abrangente. Aqui, deixei algumas práticas que me auxliaram na minha jornada com desenvolvimento paralelo em C# e um pouco do meu conhecimento.  Espero que este tópico lhe seja útil. Obrigado pela atenção!
