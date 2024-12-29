module TestSuite;

func ptr(x: i32 = 42): i32*
{

}

func unit(x: unit cm): none
{

}

func main(): none  {
    let myFlag = true;
    let t = test(1, false);
    let t : System.Type = typeof(i32);

    print("hello");
    println("world");
    print(1+2*3);
    System.Console::WriteLine("Hello World!");
    test(default(i32), true);
}

private func test(hello: i32, flag: bool): i32 -> 42;
private func expressionBody(hello: i32, flag: bool): none -> print(42);

func complex(): Functions
{

}

func external(): System.Text.StringBuilder
{

}

func internal_type(): Hello
{

}

class MyClass implements ILixou
{

}


internal class Hello extends System.Text.StringBuilder
{

}

static class Blub
{

}

union Color
{

}

enum ShortColor : i8
{
    R = 0,
    G = 1,
    B = 2
}

unit cm of f64;
unit ml of f64;
unit seconds of f64;
unit minutes of f64;

public interface ILixou
{

}


rules for unit seconds
{
  //  from seconds to minutes = seconds / 60;
   // from minutes to seconds = derive from seconds; // automaticly derive conversion rule. In this case the opposite of seconds: minutes * 60
}