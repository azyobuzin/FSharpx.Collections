// Learn more about F# at http://fsharp.org

open System
open FSharpx.Collections
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

type Benchmarks() =
    let targetDList =
        seq { for i in 0 .. 10 .. 999 do yield seq { i .. i + 9 } }
        |> Seq.map DList.ofSeq
        |> Seq.reduce DList.append

    [<Benchmark>]
    member __.GetEnumerator() =
        let results = Array.zeroCreate (DList.length targetDList)
        for i in targetDList do
            results.[i] <- i
        results

    [<Benchmark>]
    member __.GetEnumerator2() =
        let results = Array.zeroCreate (DList.length targetDList)
        use e = targetDList.GetEnumerator2()
        while e.MoveNext() do
            let i = e.Current
            results.[i] <- i
        results

[<EntryPoint>]
let main argv =
    BenchmarkRunner.Run<Benchmarks>() |> ignore
    0 // return an integer exit code