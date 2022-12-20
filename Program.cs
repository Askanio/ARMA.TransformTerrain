using TransformTerrain;

int diffX = 0; // start X
int diffY = 0; // start Y
int size = 0; // new size
string sourceFileName = string.Empty; // source file
string destFileName = string.Empty; // destination file
var run = true;
Console.WriteLine("Start transform");
#if DEBUG
diffX = 1856;
diffY = 3336;
size = 4096;
sourceFileName = @"..\..\..\Test\TestSource.txt";
destFileName = @"..\..\..\Test\TransformResult.txt";
#else
if (args[0] == "?")
{
    Console.WriteLine("TransformTerrain startX startY newSize sourceFile destinationFile");
    Console.WriteLine("Example:");
    Console.WriteLine("TransformTerrain 1856 3336 4096 TestSource.txt TransformResult.txt");
    run = false;
}
else
{
    if (!int.TryParse(args[0], out diffX))
    {
        Console.WriteLine("Start X undefined");
        run = false;
    }

    if (!int.TryParse(args[1], out diffY))
    {
        Console.WriteLine("Start Y undefined");
        run = false;
    }

    if (!int.TryParse(args[2], out size))
    {
        Console.WriteLine("New size undefined");
        run = false;
    }

    sourceFileName = args[3];
    if (!File.Exists(sourceFileName))
    {
        Console.WriteLine("Source file is not exists");
        run = false;
    }

    destFileName = args[4];    
}

#endif

if (run)
{
    try
    {
        var t = new Transform();
        t.Build(diffX, diffY, size, sourceFileName, destFileName);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }

}

Console.WriteLine("Transform finished");
Console.ReadLine();
