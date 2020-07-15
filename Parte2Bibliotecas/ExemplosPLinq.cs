using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Parte2Bibliotecas
{
    /// <summary>
    /// Nesta classe, demontro como criar uma query paralela com PLinq
    /// </summary>
    public static class ExemplosPLinq
    {
        /// <summary>
        /// Executa o Teste de paralelismo com PLinq
        /// </summary>
        public static void Executa()
        {

            TaskUtils.Cronometrar(() =>
            {
                var total = 10000000;
                Console.WriteLine("Neste exemplo, vou gerar uma lista com 10mi usuários, e com Linq e PLinq, calcular a idade de cada um e mostrar uma proporção etária.");
                var usuarios = TaskUtils.GerarUsuarios(total);

                var itens = usuarios
                    .AsParallel()
                    .Select(usuario =>
                    {
                        var idade = (DateTime.Now - usuario.Nascimento).Days / 365;

                        if (idade <= 14)
                            return "0-14";

                        if (idade > 14 && idade <= 24)
                            return "15-24";

                        if (idade > 24 && idade <= 54)
                            return "25-54";

                        return ">54";
                    })
                    .GroupBy(b => b)
                    .Select(s => new
                    {
                        Faixa = s.Key,
                        Quantidade = s.Count() / (float)total
                    })
                    .OrderByDescending(b => b.Quantidade);

                foreach (var item in itens)
                    Console.WriteLine($"{item.Faixa}: {item.Quantidade:P}");
            });
        }
    }
}
