﻿module Asn1Fold

open Asn1AcnAst

let rec foldMap func state lst =
    match lst with
    | []        -> [],state
    | h::tail   -> 
        let procItem, newState = func state h
        let restList, finalState = tail |> foldMap func newState
        procItem::restList, finalState


let foldGenericConstraint unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    (c:GenericConstraint<'v>) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:GenericConstraint<'v>) (s0:'UserState) =
        match c with
        | UnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | IntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | AllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | ExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | RootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | RootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | SingleValueConstraint (v)    -> singleValueFunc v s0
    loopRecursiveConstraint c s


let foldRangeTypeConstraint unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    rangeFunc range_val_max_func range_min_val_func
    (c:RangeTypeConstraint<'v,'vr>) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:RangeTypeConstraint<'v,'vr>) (s0:'UserState) =
        match c with
        | RangeUnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | RangeIntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | RangeAllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | RangeExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | RangeRootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | RangeRootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | RangeSingleValueConstraint (v)            -> singleValueFunc v s0
        | RangeContraint((v1), (v2), b1,b2)     -> rangeFunc v1 v2 b1 b2 s
        | RangeContraint_val_MAX ((v), b)            -> range_val_max_func v b s
        | RangeContraint_MIN_val ((v), b)            -> range_min_val_func v b s
    loopRecursiveConstraint c s

let foldSizableTypeConstraint unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    sunionFunc sintersectionFunc sallExceptFunc sexceptFunc srootFunc srootFunc2 ssingleValueFunc 
    srangeFunc srange_val_max_func srange_min_val_func
    (c:SizableTypeConstraint<'v>) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:SizableTypeConstraint<'v>) (s0:'UserState) =
        match c with
        | SizeUnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | SizeIntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | SizeAllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | SizeExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | SizeRootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | SizeRootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | SizeSingleValueConstraint (v)    -> singleValueFunc v s0
        | SizeContraint    intCon   -> foldRangeTypeConstraint sunionFunc sintersectionFunc sallExceptFunc sexceptFunc srootFunc srootFunc2 ssingleValueFunc srangeFunc srange_val_max_func srange_min_val_func intCon s
    loopRecursiveConstraint c s

let foldSizableTypeConstraint2 unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    foldRangeTypeConstraint
    (c:SizableTypeConstraint<'v>) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:SizableTypeConstraint<'v>) (s0:'UserState) =
        match c with
        | SizeUnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | SizeIntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | SizeAllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | SizeExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | SizeRootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | SizeRootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | SizeSingleValueConstraint (v)    -> singleValueFunc v s0
        | SizeContraint    intCon   -> foldRangeTypeConstraint intCon s0
    loopRecursiveConstraint c s


let foldStringTypeConstraint unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    sunionFunc sintersectionFunc sallExceptFunc sexceptFunc srootFunc srootFunc2 ssingleValueFunc srangeFunc srange_val_max_func srange_min_val_func
    aunionFunc aintersectionFunc aallExceptFunc aexceptFunc arootFunc arootFunc2 asingleValueFunc arangeFunc arange_val_max_func arange_min_val_func 
    (c:IA5StringConstraint) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:IA5StringConstraint) (s0:'UserState) =
        match c with
        | StrUnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | StrIntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | StrAllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | StrExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | StrRootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | StrRootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | StrSingleValueConstraint (v)    -> singleValueFunc v s0
        | StrSizeContraint        intCon   -> foldRangeTypeConstraint sunionFunc sintersectionFunc sallExceptFunc sexceptFunc srootFunc srootFunc2 ssingleValueFunc srangeFunc srange_val_max_func srange_min_val_func intCon s
        | AlphabetContraint       alphaCon -> foldRangeTypeConstraint aunionFunc aintersectionFunc aallExceptFunc aexceptFunc arootFunc arootFunc2 asingleValueFunc arangeFunc arange_val_max_func arange_min_val_func alphaCon s        
    loopRecursiveConstraint c s


let foldStringTypeConstraint2 unionFunc intersectionFunc allExceptFunc exceptFunc rootFunc rootFunc2 singleValueFunc 
    foldRangeSizeConstraint foldRangeAlphaConstraint
    (c:IA5StringConstraint) 
    (s:'UserState) =
    let rec loopRecursiveConstraint (c:IA5StringConstraint) (s0:'UserState) =
        match c with
        | StrUnionConstraint(c1,c2,b)         -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            unionFunc nc1 nc2 b s2
        | StrIntersectionConstraint(c1,c2)    -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            intersectionFunc nc1 nc2 s2
        | StrAllExceptConstraint(c1)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            allExceptFunc nc1 s1
        | StrExceptConstraint(c1,c2)          -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            exceptFunc nc1 nc2 s2
        | StrRootConstraint(c1)               -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            rootFunc nc1 s1
        | StrRootConstraint2(c1,c2)           -> 
            let nc1, s1 = loopRecursiveConstraint c1 s0
            let nc2, s2 = loopRecursiveConstraint c2 s1
            rootFunc2 nc1 nc2 s2
        | StrSingleValueConstraint (v)     -> singleValueFunc v s0
        | StrSizeContraint        intCon   -> foldRangeSizeConstraint  intCon s
        | AlphabetContraint       alphaCon -> foldRangeAlphaConstraint alphaCon s        
    loopRecursiveConstraint c s


let foldType
    intFunc
    realFunc
    ia5StringFunc 
    numStringFunc 
    octStringFunc 
    nullTypeFunc
    bitStringFunc
    boolFunc
    enumFunc 
    seqOfFunc
    seqFunc
    seqAsn1ChildFunc
    seqAcnChildFunc
    choiceFunc 
    chChildFunc
    refType 
    typeFunc
    (t:Asn1Type) 
    (us:'UserState) 
    =
    let rec loopType (t:Asn1Type) (us:'UserState) =
        let newKind, newState =
            match t.Kind with
            | Integer        ti -> intFunc ti us
            | Real           ti -> realFunc ti us
            | IA5String      ti -> ia5StringFunc ti us
            | NumericString  ti -> numStringFunc ti us
            | OctetString    ti -> octStringFunc ti us
            | NullType       ti -> nullTypeFunc ti us
            | BitString      ti -> bitStringFunc ti us
            | Boolean        ti -> boolFunc ti us
            | Enumerated     ti -> enumFunc ti us
            | SequenceOf     ti -> 
                let newChild, newState = loopType ti.child us
                seqOfFunc ti newChild newState
            | Sequence       ti -> 
                let newChildren, newState = 
                    ti.children |> 
                    foldMap (fun curState ch -> 
                        match ch with
                        | Asn1Child asn1Chlld   ->
                            let newChildType, newState = loopType asn1Chlld.Type curState
                            seqAsn1ChildFunc asn1Chlld newChildType newState
                        | AcnChild  acnChild    ->  
                            seqAcnChildFunc acnChild curState) us
                seqFunc ti newChildren newState
            | Choice         ti -> 
                let newChildren, newState = 
                    ti.children |> 
                    foldMap (fun curState ch -> 
                        let newChildType, newState = loopType ch.Type curState
                        chChildFunc ch newChildType newState) us
                choiceFunc ti newChildren newState
            | ReferenceType  ti -> 
               let newBaseType, newState = loopType ti.baseType us
               refType ti newBaseType newState
        typeFunc t newKind newState
    loopType t us
