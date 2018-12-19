with Interfaces; 
use Interfaces;
with adaasn1rtl;
use adaasn1rtl;


package uper_asn1_rtl with Spark_Mode is
   
   procedure UPER_Enc_Boolean (bs : in out BitStream; Val : in  Asn1Boolean) with
     Depends => (bs => (bs, Val)),
     Pre     => bs.Current_Bit_Pos < Natural'Last and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + 1;

   procedure UPER_Dec_boolean (bs : in out BitStream; val: out Asn1Boolean; result : out Boolean) with
     Depends => (bs => (bs), val => bs, result => bs),
     Pre     => bs.Current_Bit_Pos < Natural'Last and then  
                bs.Size_In_Bytes < Positive'Last/8 and then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8,
     Post    => result  and bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + 1;
   

   --UPER encode functions 
   procedure UPER_Enc_SemiConstraintWholeNumber (bs : in out BitStream; IntVal : in Asn1Int; MinVal : in     Asn1Int) with
     Depends => (bs => (bs, IntVal, MinVal)),
     Pre     => IntVal >= MinVal and then
                bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8);
   
   procedure UPER_Dec_SemiConstraintWholeNumber (bs : in out BitStream; IntVal : out Asn1Int; MinVal : in  Asn1Int;  Result :    out Boolean) with
      Depends => ((IntVal, Result) => (bs, MinVal), bs => (bs, MinVal)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8) and IntVal >= MinVal ;
   
   procedure UPER_Enc_SemiConstraintPosWholeNumber (bs : in out BitStream; IntVal : in Asn1UInt; MinVal : in     Asn1UInt) with
     Depends => (bs => (bs, IntVal, MinVal)),
     Pre     => IntVal >= MinVal and then
                bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8);  
   
   procedure UPER_Dec_SemiConstraintPosWholeNumber (bs : in out BitStream; IntVal : out Asn1UInt; MinVal : in     Asn1UInt; Result :    out Boolean)  with
     Depends => ((IntVal, Result) => (bs, MinVal), bs => (bs, MinVal)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8) and IntVal >= MinVal ;   
   
   
   procedure UPER_Enc_ConstraintWholeNumber (bs : in out BitStream; IntVal : in Asn1Int; MinVal : in Asn1Int; nBits : in Integer) with
     Depends => (bs => (bs, IntVal, MinVal, nBits)),
     Pre     => IntVal >= MinVal and then
                nBits >= 0 and then nBits < Asn1UInt'Size and then 
                bs.Current_Bit_Pos < Natural'Last - nBits and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - nBits,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + nBits;
   
   procedure UPER_Enc_ConstraintPosWholeNumber (bs : in out BitStream; IntVal: in Asn1UInt; MinVal : in Asn1UInt; nBits : in Integer) with
     Depends => (bs => (bs, IntVal, MinVal, nBits)),
     Pre     => IntVal >= MinVal and then
                nBits >= 0 and then nBits < Asn1UInt'Size and then 
                bs.Current_Bit_Pos < Natural'Last - nBits and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - nBits,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + nBits;
   
   procedure UPER_Dec_ConstraintWholeNumber (bs : in out BitStream; IntVal : out Asn1Int; MinVal : in Asn1Int; MaxVal : in Asn1Int; nBits : in Integer; Result : out Boolean) with
     Depends => ((bs, IntVal, Result) => (bs, MinVal, MaxVal, nBits)),
     Pre     => MinVal >= MaxVal and then
                nBits >= 0 and then nBits < Asn1UInt'Size and then 
                bs.Current_Bit_Pos < Natural'Last - nBits and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - nBits,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + nBits and
                (
                     (Result   and  (((IntVal >= MinVal) and (IntVal <= MaxVal)))) or
                     (not Result  and  (IntVal = MinVal))
                );
   
   procedure UPER_Dec_ConstraintPosWholeNumber (bs : in out BitStream; IntVal : out Asn1UInt; MinVal : in Asn1UInt; MaxVal : in Asn1UInt; nBits : in Integer; Result : out Boolean) with
     Depends => ((bs, IntVal, Result) => (bs, MinVal, MaxVal, nBits)),
     Pre     => MinVal >= MaxVal and then
                nBits >= 0 and then nBits < Asn1UInt'Size and then 
                bs.Current_Bit_Pos < Natural'Last - nBits and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - nBits,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + nBits and
                (
                     (Result   and  (((IntVal >= MinVal) and (IntVal <= MaxVal)))) or
                     (not Result  and  (IntVal = MinVal))
                );
   procedure UPER_Dec_ConstraintWholeNumberInt
     (bs : in out BitStream;
      IntVal      :    out Integer;
      MinVal      : in     Integer;
      MaxVal      : in     Integer;
      nBits : in     Integer;
      Result      :    out Boolean) with
     Depends => ((bs, IntVal, Result) => (bs, MinVal, MaxVal, nBits)),
     Pre     => MinVal >= MaxVal and then
                nBits >= 0 and then nBits < Asn1UInt'Size and then 
                bs.Current_Bit_Pos < Natural'Last - nBits and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - nBits,
     Post    => bs.Current_Bit_Pos = bs'Old.Current_Bit_Pos + nBits and
                (
                     (Result   and  (((IntVal >= MinVal) and (IntVal <= MaxVal)))) or
                     (not Result  and  (IntVal = MinVal))
                );
   
   procedure UPER_Enc_UnConstraintWholeNumber (bs : in out BitStream; IntVal : in Asn1Int)  with
     Depends => (bs => (bs, IntVal)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8);  


   procedure UPER_Dec_UnConstraintWholeNumber (bs : in out BitStream; IntVal :    out Asn1Int; Result :    out Boolean) with
     Depends => ((IntVal, bs, Result) => bs),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8);  
   
   procedure UPER_Dec_UnConstraintWholeNumberMax (bs : in out BitStream; IntVal : out Asn1Int;  MaxVal : in Asn1Int; Result : out Boolean) with
     Depends => ((IntVal, bs, Result) => (bs, MaxVal)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 8) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 8),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 8);  
   
   -- REAL FUNCTIONS
   
   function CalcReal (Factor : Asn1UInt; N : Asn1UInt; base : Integer;Exp : Integer) return Asn1Real 
   with
       Pre => 
         (Factor = 1 or Factor=2 or Factor=4 or Factor=8) and then
         (base = 2 or base = 8 or base = 16);
   
   procedure UPER_Enc_Real (bs : in out BitStream;  RealVal : in     Asn1Real) with
     Depends => (bs => (bs, RealVal)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 40) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 40),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 40);  
   
   procedure UPER_Dec_Real (bs : in out BitStream; RealVal : out Asn1Real; Result  : out ASN1_RESULT) with
     Depends => ((bs, RealVal, Result) => (bs)),
     Pre     => bs.Current_Bit_Pos < Natural'Last - (Asn1UInt'Size + 40) and then  
                bs.Size_In_Bytes < Positive'Last/8 and  then
                bs.Current_Bit_Pos < bs.Size_In_Bytes * 8 - (Asn1UInt'Size + 40),
     Post    => bs.Current_Bit_Pos >= bs'Old.Current_Bit_Pos and bs.Current_Bit_Pos <= bs'Old.Current_Bit_Pos + (Asn1UInt'Size + 40);  
   
     
end uper_asn1_rtl;
