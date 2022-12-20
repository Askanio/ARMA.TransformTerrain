namespace TransformTerrain
{
    public class Transform
    {

        public void Build(int startDiffX, int startDiffY, int size, string sourceFileName, string destFileName)
        {
            using var source = new StreamReader(sourceFileName);
            using var writer = new StreamWriter(destFileName, false);

            //ncols         4096
            //nrows         4096
            //xllcorner     200000.000000
            //yllcorner     0.000000
            //cellsize      5.000000
            //NODATA_value - 9999

            var startline = 0;
            var endline = 0;
            var columnMax = 0;

            int line = 0;
            string? sourceLine;
            while((sourceLine = source.ReadLine()) != null)
            {
                if (line == 0)
                {
                    writer.WriteLine($"ncols         {size}");
                    var ncolString = sourceLine.Substring(5);
                    if (!int.TryParse(ncolString, out var ncol))
                    {
                        Console.WriteLine("Cant convert ncols to int");
                        return;
                    }
                    columnMax = ncol;
                }
                else if (line == 1)
                {
                    writer.WriteLine($"nrows         {size}");
                    var nrowString = sourceLine.Substring(5);
                    if (!int.TryParse(nrowString, out var nrow))
                    {
                        Console.WriteLine("Cant convert nrows to int");
                        return;
                    }
                    startline = nrow - size - startDiffY + 6;
                    endline = nrow - startDiffY + 6;
                }
                else if (line <= 5)
                {
                    writer.WriteLine(sourceLine);
                }
                else if (line >= startline && line < endline)
                {
                    var destLine = TransformLine(startDiffX, size, columnMax, sourceLine);
                    if (destLine != null)
                        writer.WriteLine(destLine);
                }
                line += 1;
            }

        }

        private string? TransformLine(int diffX, int size, int columnMax, string sourceLine)
        {
            //127.61 127.94 128.14 128.16 128.01 127.76 127.48
            var fSize = diffX + size;

            var posStartX = IndexOf(sourceLine, ' ', diffX);
            if (posStartX == -1)
            {
                Console.WriteLine("Not found start position X");
                return null;
            }
            if (fSize == columnMax)
                return sourceLine.Substring(posStartX + 1);

            var posEndX = IndexOf(sourceLine, ' ', fSize);
            if (posEndX == -1)
            {
                Console.WriteLine("Not found end position X");
                return null;
            }

            return sourceLine.Substring(posStartX + 1, posEndX - posStartX - 1);
        }
        
        public static int IndexOf(string sourceString, char value, int iteration)
        {
            var startPosition = 0;
            int pos;
            do
            {
                pos = sourceString.IndexOf(value, startPosition + 1);
                iteration -= 1;
                startPosition = pos + 1;
            } while (pos >= 0 && iteration > 0);
            return pos;
        }

    }
}
