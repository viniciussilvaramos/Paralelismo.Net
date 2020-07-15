using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Utils;

namespace Parte2Bibliotecas
{
    /// <summary>
    /// Nesta classe, mostro como criar um fluxo de dados com o dataflow
    /// </summary>
    public static class ExemplosDataflow
    {
        private static readonly HttpClient _client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
        public static void Executa()
        {
            TaskUtils.CronometrarAsync(CriarPipelineAsync)
                .Wait();
        }

        /// <summary>
        /// Exemplo de criação de um pipeline com Dataflow
        /// </summary>

        private static async Task CriarPipelineAsync()
        {
            Console.WriteLine("Exemplo Dataflow: Criação de uma pipeline de processamento de dados com o Dataflow");

            /*
             * A proposta deste código será: Baixar alguns livros do site gutemberg.org, 
             * contar as palavras com mais de 3 letras em cada um deles
             * e mostra o resultado em tela
             */


            //Definição dos blocos de processamento
            var blocoDownload = new TransformBlock<string, string>(BaixarLivroAsync, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });
            var blocoPalavras = new TransformBlock<string, string[]>(CriarBlocoPalavras);
            var blocoFiltro = new TransformBlock<string[], string[]>(FiltrarPalavras);
            var blocoFrequencia = new TransformBlock<string[], (string, int)[]>(ContarFrequencia);
            var blocoExibicao = new ActionBlock<(string, int)[]>(MostrarFrequencia);


            var options = new DataflowLinkOptions { PropagateCompletion = true };

            blocoDownload.LinkTo(blocoPalavras, options);
            blocoPalavras.LinkTo(blocoFiltro, options);
            blocoFiltro.LinkTo(blocoFrequencia, options);
            blocoFrequencia.LinkTo(blocoExibicao, options);

            var livros = new[]
            {
                "http://www.gutenberg.org/files/1661/1661-0.txt", //The Adventures of Sherlock Holmes
                "http://www.gutenberg.org/files/2701/2701-0.txt", //Moby Dick; or The Whale
                "http://www.gutenberg.org/files/1342/1342-0.txt", //Pride and Prejudice
                "http://www.gutenberg.org/files/11/11-0.txt", //Alice’s Adventures in Wonderland,
                "http://www.gutenberg.org/cache/epub/25525/pg25525.txt", //The Works of Edgar Allan Poe
            };

            //Popula o primeiro bloco com as urls de download
            foreach (var livro in livros)
                await blocoDownload.SendAsync(livro);

            //Informa que não haverá mais nenhum outro livro para baixar
            blocoDownload.Complete();

            //Aguarda até que o ultimo bloco termine todo o processamento
            await blocoFrequencia.Completion;
        }

        private static string[] CriarBlocoPalavras(string texto)
        {
            Console.WriteLine("Criando lista de palavras...");
            texto = new string(texto.Select(c => char.IsLetter(c) ? c : ' ').ToArray());
            return texto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        private static void MostrarFrequencia((string palavra, int quantidade)[] freq)
        {
            Console.WriteLine("Mostrando as 10 palavras mais frequentes de um livro...");
            float totalPalavras = freq.Sum(s => s.quantidade);

            foreach (var (palavra, quantidade) in freq.Take(10))
                Console.WriteLine($"Palavra: {palavra}, Frequência: {quantidade} ({quantidade / totalPalavras:P})");

            Console.WriteLine(new string('#', 50));
        }

        private static (string palavra, int quantidade)[] ContarFrequencia(string[] arg)
        {
            Console.WriteLine("Contando frequencia das palavras...");

            return arg
                .AsParallel()
                .GroupBy(b => b)
                .Select(s => (s.Key, s.Count()))
                .OrderByDescending(b => b.Item2)
                .ToArray();
        }

        private static string[] FiltrarPalavras(string[] palavras)
        {
            Console.WriteLine("Filtrando todas as palavras com menos de 3 caracteres...");

            return palavras
                .Where(w => w.Length > 3)
                .ToArray();
        }

        private static async Task<string> BaixarLivroAsync(string url)
        {
            Console.WriteLine("Baixando Livro: {0}", url);

            var text = await _client.GetStringAsync(url);
            return text;
        }
    }
}
