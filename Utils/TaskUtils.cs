using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public static class TaskUtils
    {
        /// <summary>
        /// Aguarda um determinado período de forma assíncrona
        /// </summary>
        public static Task AguardarAsync(int segundos)
            => Task.Delay(segundos * 1000);

        /// <summary>
        /// Aguarda um determinado período e mostra a mensagem na tela
        /// </summary>
        public static void Executa(int segundos = 5)
        {
            AguardarAsync(segundos).Wait();
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: Aguardei {segundos} segundos...");
        }

        /// <summary>
        /// Aguarda um determinado período e mostra a mensagem na tela de forma assíncrona
        /// </summary>
        public static async Task ExecutaAsync()
        {
            await AguardarAsync(5);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: Aguardei 5 segundos de forma assíncrona...");
        }

        /// <summary>
        /// Cronometra o tempo de execução de uma Tarefa
        /// </summary>
        /// <param name="metodo">O método a ser cronometrado</param>
        public static void Cronometrar(Action metodo)
        {
            var sw = new Stopwatch();
            sw.Start();

            //Caso o método seja nulo, não para a aplicação
            metodo?.Invoke();

            sw.Stop();
            Console.WriteLine($"Tempo total de execução: {sw.Elapsed.Duration()}");
        }

        /// <summary>
        /// Cronometra o tempo de execução de uma Tarefa Assíncrona
        /// </summary>
        /// <param name="metodo">O método a ser cronometrado</param>
        public static async Task CronometrarAsync(Func<Task> metodo)
        {
            var sw = new Stopwatch();
            sw.Start();

            //Caso o método seja nulo, não para a aplicação
            await metodo?.Invoke();

            sw.Stop();
            Console.WriteLine($"Tempo total de execução: {sw.Elapsed.Duration()}");
        }

        /// <summary>
        /// Gerador de usuários para testes
        /// </summary>
        /// <param name="quantidade">quantidade de usuários</param>
        /// <returns></returns>
        public static IEnumerable<Usuario> GerarUsuarios(int quantidade)
        {
            var random = new Random(42);
            return Enumerable.Range(1, quantidade)
                .Select(s => new Usuario
                {
                    Nome = $"Usuario {s}",
                    Nascimento = DateTime.Now.AddYears(-random.Next(1, 100))
                });
        }
    }
}
