using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DipuAlba.Artilugios.Extensions;


namespace DipuAlba.Artilugios.Benchmarks
{
    [MemoryDiagnoser]
    public class MyBenchmarks
    {
        private string _elQuijote;
        [GlobalSetup]
        public void GlobalSetup()
        {
            //Write your initialization code here
            using var client = new HttpClient();
            _elQuijote = client.GetStringAsync("https://gist.githubusercontent.com/jsdario/6d6c69398cb0c73111e49f1218960f79/raw/8d4fc4548d437e2a7203a5aeeace5477f598827d/el_quijote.txt").Result;
            if (string.IsNullOrEmpty(_elQuijote) || _elQuijote.Length < 10000)
            {
                throw new Exception("El Quijote no se ha descargado bien");
            }
        }

        [Benchmark]
        public void MyLibraryBenchmarkMethod()
        {
            var resultado = _elQuijote.RemoveSymbols();
        }


        [Benchmark]
        public void MyCandidateBenchmarkMethod()
        {
            //Write your code here
            var resultado = CandidateBenchmarkMethod(_elQuijote);
        }

        private static string CandidateBenchmarkMethod(string txt, string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ")
        {
            if (txt == null) throw new ArgumentNullException(nameof(txt));
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));

            var allowedChars = new HashSet<char>(alphabet);
            var result = new StringBuilder();

            foreach (var c in txt.Where(c => allowedChars.Contains(c)))
            {
                result.Append(c);
            }

            return result.ToString();
        }
    }

}
