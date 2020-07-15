using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Parte2Bibliotecas
{
    /// <summary>
    /// Nesta classe, demonstro exemplos da utilização da classe Parallel.For e Parallel.ForEach.
    /// A vantagem da classe Parallel é que ela gerencia o paralelismo internamente, onde fica por responsabilidade do desenvolvedor
    /// controlar apenas o nível máximo de paralelismo
    /// </summary>
    public static class ExemplosParallel
    {
        /// <summary>
        /// Demonstra o uso do ForEach
        /// </summary>
        public static void ExecutaForEach()
        {
            var total = 10000000;
            Console.WriteLine("Neste exemplo, vou gerar uma lista com 10mi usuários, e de forma paralela, calcular a idade de cada um e mostrar uma proporção etária.");
            var usuarios = TaskUtils.GerarUsuarios(total);

            TaskUtils.Cronometrar(() =>
            {
                var faixasEtarias = new ConcurrentDictionary<string, float>();
                Parallel.ForEach(usuarios, new ParallelOptions { MaxDegreeOfParallelism = -1 }, usuario =>
                {
                    var idade = (DateTime.Now - usuario.Nascimento).Days / 365;

                    if (idade <= 14)
                    {
                        faixasEtarias.AddOrUpdate("0-14", 1, (key, old) => old + 1);
                        return;
                    }

                    if (idade > 14 && idade <= 24)
                    {
                        faixasEtarias.AddOrUpdate("15-24", 1, (key, old) => old + 1);
                        return;
                    }

                    if (idade > 24 && idade <= 54)
                    {
                        faixasEtarias.AddOrUpdate("25-54", 1, (key, old) => old + 1);
                        return;
                    }

                    faixasEtarias.AddOrUpdate(">54", 1, (key, old) => old + 1);
                });

                foreach (var kv in faixasEtarias.OrderByDescending(b => b.Value))
                    Console.WriteLine($"{kv.Key}: {kv.Value / total:P}");
            });
        }
    }
}
