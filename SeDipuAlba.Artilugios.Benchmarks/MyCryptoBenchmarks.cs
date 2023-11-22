using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeDipuAlba.Artilugios.Extensions;


namespace SeDipuAlba.Artilugios.Benchmarks
{
    [MemoryDiagnoser]
    public class MyCryptoBenchmarks
    {
        private string? _elQuijote;
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
        public void CryptoBenchmarkMethodQuijote()
        {
            var resultado = SeDipuAlba.Artilugios.Crypto.EncryptStringAES(_elQuijote, "secret", "saltatleast8bytes");
        }

        [Benchmark]
        public void CryptoBenchmarkMethod2Quijote()
        {
            var resultado = SeDipuAlba.Artilugios.Crypto.EncryptStringAES(_elQuijote, "secret", "saltatleast8bytes");
            resultado = SeDipuAlba.Artilugios.Crypto.DecryptStringAES(resultado, "secret", "saltatleast8bytes");
        }

        [Benchmark]
        public void CryptoBenchmarkMethod()
        {
            var resultado = SeDipuAlba.Artilugios.Crypto.EncryptStringAES("Long text to encrypt", "secret", "saltatleast8bytes");
        }

        [Benchmark]
        public void CryptoBenchmarkMethod2()
        {
            var resultado = SeDipuAlba.Artilugios.Crypto.EncryptStringAES("Long text to encrypt", "secret", "saltatleast8bytes");
            resultado = SeDipuAlba.Artilugios.Crypto.DecryptStringAES(resultado, "secret", "saltatleast8bytes");
        }


        //[Benchmark]
        //public void EncryptStringBenchmarkMethodQuijote()
        //{
        //    var resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Encrypt(_elQuijote, "secret");
        //}

        //[Benchmark]
        //public void EncryptStringBenchmarkMethod2Quijote()
        //{
        //    var resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Encrypt(_elQuijote, "secret");
        //    resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Decrypt(resultado, "secret");
        //}

        //[Benchmark]
        //public void EncryptStringBenchmarkMethod()
        //{
        //    var resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Encrypt("Long text to encrypt", "secret");
        //}

        //[Benchmark]
        //public void EncryptStringBenchmarkMethod2()
        //{
        //    var resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Encrypt("Long text to encrypt", "secret");
        //    resultado = SeDipuAlba.Artilugios.EncryptString.StringCipher.Decrypt(resultado, "secret");
        //}


    }

}
