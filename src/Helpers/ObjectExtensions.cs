using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommunAxiom.DotnetSdk.Helpers
{
    public static class ObjectExtensions
    {
        public static bool Try<T>(this T obj, Action<T> act) 
        {
            try
            {
                act(obj);
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static (bool, TRes?) Try<T, TRes>(this T obj, Func<T, TRes> act)
        {
            try
            {
                var res = act(obj);
                return (true, res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (false, default(TRes));
            }
        }

        public static async Task<bool> Try<T>(this T obj, Func<T, Task> act)
        {
            try
            {
                await act(obj);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static async Task<(bool, TRes?)> Try<T, TRes>(this T obj, Func<T, Task<TRes>> act)
        {
            try
            {
                var res = await act(obj);
                return (true, res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (false, default(TRes));
            }
        }
    }
}
