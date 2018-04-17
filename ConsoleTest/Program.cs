using EmitUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
         

            AssemblyMakeRequest useRequest = new AssemblyMakeRequest();


            useRequest.AssemblyName = "test";

            TypeMakeRequest useTypeRequest = new TypeMakeRequest();
            useRequest.LstUseTypeMakeRequest = new List<TypeMakeRequest>() { useTypeRequest };
          

            useTypeRequest.TypeName = "TestClass";

            StringFiledMakeRequest useFiled1 = new StringFiledMakeRequest() { FiledName = "srt1", DefualtValue = "aaaaaaa" };
            StringFiledMakeRequest useFiled2 = new StringFiledMakeRequest() { FiledName = "srt2", DefualtValue = "testtest" };
            StringFiledMakeRequest useFiled3 = new StringFiledMakeRequest() { FiledName = "srt3", DefualtValue = "5" };

            useTypeRequest.LstFiled = new List<FiledMakeRequest>() { useFiled1, useFiled2, useFiled3 };

            MethodRequest useMethod1 = new MethodRequest() { Name = "TestMethod", ReturnType = typeof(int), ParameterTypes = new Type[] { typeof(int) } };
            useMethod1.UseMethodDel = new MethodCreatDel(UseMethodCreated);


            useTypeRequest.LstMethodRequest = new List<MethodRequest>() { useMethod1 };


            AssemblyCreater useCreater = new AssemblyCreater();

            var returnValue = useCreater.CreatOneAssembly(useRequest);

            var useType = returnValue.LstClassBean[0].GetCreatType();

            var useValue = Activator.CreateInstance(useType);



            Console.Read();
        }

      


        public static void UseMethodCreated(MethodBuilder inputMethodBuilder, ClassBuilderBean useClassBuilderBean)
        {
            var useFileBuilder = useClassBuilderBean.UseFiledDic["srt3"];

            //获得使用的il
            var useIl = inputMethodBuilder.GetILGenerator();
            var useLocal = useIl.DeclareLocal(typeof(int));
            useIl.Emit(OpCodes.Ldarg_0);
            useIl.Emit(OpCodes.Ldfld, useFileBuilder);
            var useMethod = typeof(int).GetMethod("Parse", new Type[] { typeof(string) });
            useIl.Emit(OpCodes.Call, useMethod);
            useIl.Emit(OpCodes.Stloc, useLocal);
            useIl.Emit(OpCodes.Ldarg_1);
            useIl.Emit(OpCodes.Ldloc, useLocal);
            useIl.Emit(OpCodes.Add);
            useIl.Emit(OpCodes.Ret);
        }
    }
}
