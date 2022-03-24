using Accord.Math;
using Accord.Statistics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Model
{
    class OptimizerService
    {
        double dohPortf(double[] r, double[] dohMean)
        {
            return Matrix.Dot(dohMean, r);
        }

        double[,] PctChange(double[,] source)
        {
            var pcts = new double[source.GetLength(0), source.GetLength(1)];
            for (int i = 1; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    var current = source[i, j];
                    var previous = source[i - 1, j];
                    var difference = (current - previous) / current;
                    pcts[i, j] = difference;
                }
            }
            return pcts;
        }
        double[] randPortf(int count)
        {
            double[] res = Vector.Random(count, 0d, 1000);
            var sum = res.Sum();
            for (int i = 0; i < count; i++)
            {
                res[i] = res[i] / sum;
            }
            return res;
        }

        double riskPortf(double[] r, double[,] cov)
        {
            var covdot = Matrix.Dot(r, cov);
            return Math.Sqrt(Matrix.Dot(r, covdot));
        }

        async Task<double[,]> extractSourceFromYahooJson(Portfolio portfolio)
        {
            var source = new double[portfolio.Positions.Count, 24];
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://yfapi.net/");
            httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                Constants.YAHOO_API_CODER);
            httpClient.DefaultRequestHeaders.Add("accept",
                "application/json");

            var symbols = string.Join(",", portfolio.Positions.Select(x => x.Name));

            var uri = new Uri($"{Constants.YAHOO_API_ROOT}/v8/finance/spark?symbols={symbols}" +
                $"&interval=1mo&range=2y");

            var request = await httpClient.GetAsync(uri);
            var text = await request.Content.ReadAsStringAsync();
            var json = JObject.Parse(text);
            var counter = 0;
            for (int i = 0; i < portfolio.Positions.Count; i++)
            {
                var item = portfolio.Positions[i];
                for (int j = 0; j < json[item.Name]["close"].Count(); j++)
                {
                    var jsonval = json[item.Name]["close"][j].ToString();
                    source[i, j] = double.Parse(jsonval);
                }                
            }

            return source;
        }

        /// <summary>
        /// Returns portfolio Ratios for min and max scenarios
        /// </summary>
        /// <param name="portfolio"></param>
        /// <returns></returns>
        public async Task<Tuple<double[],double[]>> Optimize(Portfolio portfolio)
        {
            double[,] source = await extractSourceFromYahooJson(portfolio);
                     
            var dCloseData = PctChange(source);

            var dohMean = Measures.Mean(dCloseData, 0);
            var cov = Measures.Covariance(dCloseData);

            var cnt = dCloseData.GetLength(1);

            var n = 1000;

            var r = new double[n][];
            var risk = new double[n];
            var doh = new double[n];

            for (int i = 0; i < n; i++)
            {
                r[i] = randPortf(cnt);
                risk[i] = riskPortf(r[i], cov);
                doh[i] = dohPortf(r[i], dohMean) / risk[i];
            }

            var minrisk = Matrix.ArgMin(risk);
            var maxSharpCoeff = Matrix.ArgMax(doh);

            var minportfolio = r[minrisk];
            var maxportfolio = r[maxSharpCoeff];

            return new Tuple<double[], double[]>(minportfolio, maxportfolio);
        }
    }
}
