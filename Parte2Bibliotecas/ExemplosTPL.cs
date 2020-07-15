using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Utils;

namespace Parte2Bibliotecas
{
    /// <summary>
    /// Nesta classe, demonstro um exemplo simples de classe que utiliza os recursos básicos de Task, Async Await e mostra como paralelizar de forma simplificada um código
    /// </summary>
    class ExemplosTPL
    {
        /// <summary>
        /// Exemplo de execução síncrona do código
        /// </summary>
        public static void ExecutaSincrono()
        {
            Console.WriteLine("Execução síncrona: Mostrará três mensagens sequenciais, em um total de 15 segundos");
            TaskUtils.Cronometrar(() =>
            {
                TaskUtils.Executa();
                TaskUtils.Executa();
                TaskUtils.Executa();
            });
            Console.WriteLine("Execução síncrona: Execução concluída!");
            Console.WriteLine(new string('=', 20));
        }

        /// <summary>
        /// Demonstração da utilização do WhenAll
        /// </summary>
        public static async Task ExecutaWhenAllAsync()
        {
            Console.WriteLine("Execução WhenAll: Mostrará as três mensagens, em menos de 15 segundos e ao final da mesma, mostrará outra mensagem.");
            await TaskUtils.CronometrarAsync(() => Task.WhenAll(
                TaskUtils.ExecutaAsync(),
                TaskUtils.ExecutaAsync(),
                TaskUtils.ExecutaAsync()
            ));
            Console.WriteLine("Execução WhenAll: Execução concluída!");
            Console.WriteLine(new string('=', 20));
        }

        /// <summary>
        /// Demonstração da utilização do WaitAll
        /// </summary>
        public static void ExecutaWaitAll()
        {
            Console.WriteLine("Execução WaitAll: Mostrará as três mensagens, em menos de 15 segundos e ao final da mesma, mostrará outra mensagem.");
            TaskUtils.Cronometrar(() => Task.WaitAll(
                TaskUtils.ExecutaAsync(),
                TaskUtils.ExecutaAsync(),
                TaskUtils.ExecutaAsync()
            ));
            Console.WriteLine("Execução  WaitAll: Execução concluída!");
            Console.WriteLine(new string('=', 20));
        }

        /// <summary>
        /// Demonstração da utilização do Task.Run
        /// </summary>
        public static void ExecutaTaskRun()
        {
            Console.WriteLine("Execução Task.Run: Simula a execução de várias tasks simultâneas.");

            TaskUtils.Cronometrar(() =>
            {
                var task1 = Task.Run(() => TaskUtils.Executa(1));
                var task2 = Task.Run(() => TaskUtils.Executa(2));
                var task3 = Task.Run(() => TaskUtils.Executa(3));
                var task4 = Task.Run(() => TaskUtils.Executa(4));

                task1.Wait();
                task2.Wait();
                task3.Wait();
                task4.Wait();
            });

            Console.WriteLine(new string('=', 20));
        }
    }
}
