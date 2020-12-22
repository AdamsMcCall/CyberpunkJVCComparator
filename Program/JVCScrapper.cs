using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class JVCScrapper
    {
        string _url = "https://www.jeuxvideo.com/tous-les-jeux/";
        string _titleLandmark = "class=\"gameTitleLink__196nPy\"";
        string _gradeLandmark = "class=\"editorialRating__1tYu_r\"";
        public List<GameInfo> gameInfos;

        public JVCScrapper()
        {
            gameInfos = new List<GameInfo>();
        }

        string GetContent(string data, string landmark)
        {
            var idx = data.IndexOf(landmark);
            var output = data.Substring(idx);
            idx = output.IndexOf('>') + 1;
            output = output.Substring(idx);
            if (output[0] == '<')
                output = output.Substring(output.IndexOf('>') + 1);
            output = output.Remove(output.IndexOf('<'));
            return output;
        }

        string FormatGrade(string rawGrade)
        {
            var output = rawGrade.Remove(rawGrade.IndexOf('/'));

            if (float.TryParse(output, out _))
                return output;
            else
                return "-";
        }

        async Task<bool> ParsePage(int pageNb)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync(_url + "?p=" + pageNb.ToString());

            try
            {
                while (true)
                {
                    var title = GetContent(result, _titleLandmark);
                    title = title.Replace("&#x27;", "'");
                    var grade = GetContent(result, _gradeLandmark);
                    result = result.Substring(result.IndexOf(_gradeLandmark) + 50);
                    result = result.Substring(result.IndexOf(_titleLandmark) - 50);
                    grade = FormatGrade(grade);
                    if (grade == "-")
                        continue;
                    gameInfos.Add(new GameInfo
                    {
                        name = title,
                        grade = grade
                    });
                    Console.WriteLine(title + " - " + grade);
                }
            }
            catch
            {

            }
            return true;
        }

        public async void GetGrades()
        {
            for (int i = 1; i <= 2000; ++i)
            {
                await ParsePage(i);
                Console.WriteLine("Page " + i.ToString() + " done");
            }
            foreach (GameInfo info in gameInfos)
                Console.WriteLine(info.name + " - " + info.grade + "/20");
        }
    }
}
